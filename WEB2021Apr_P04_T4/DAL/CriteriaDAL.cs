using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using WEB2021Apr_P04_T4.Models;

namespace WEB2021Apr_P04_T4.DAL
{
    public class CriteriaDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        //Constructor
        public CriteriaDAL()
        {
            //Read ConnectionString from appsettings.json file
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString(
            "NPBookConnectionString");
            //Instantiate a SqlConnection object with the
            //Connection String read.
            conn = new SqlConnection(strConn);
        }

        public List<Criteria> GetAllCriteria()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Criteria ORDER BY CriteriaID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Criteria> criteriaList = new List<Criteria>();
            while (reader.Read())
            {
                criteriaList.Add(
                new Criteria
                {
                    CriteriaID = reader.GetInt32(0), //0: 1st column
                    CompetitionID = reader.GetInt32(1), //1: 2nd column
                    CriteriaName = reader.GetString(2),
                    Weightage = reader.GetInt32(3),
                    //Get the first character of a string
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return criteriaList;
        }

        public int Add(Criteria criteria)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Criteria (CompetitionID, CriteriaName, Weightage)
            OUTPUT INSERTED.CriteriaID
            VALUES(@competitionid, @criterianame, @weightage)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@competitionid", criteria.CompetitionID);
            cmd.Parameters.AddWithValue("@criterianame", criteria.CriteriaName);
            cmd.Parameters.AddWithValue("@weightage", criteria.Weightage);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            criteria.CriteriaID = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return criteria.CriteriaID;
        }

        public bool IsCriteriaNameExist(string criteriaName, int criteriaID, int competitionID)
        {
            bool criteriaFound = false;
            //Create a SqlCommand object and specify the SQL statement 
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CriteriaID FROM Criteria 
                              WHERE CriteriaName=@selectedCriteriaName AND CompetitionID=@selectedCompetitionID";
            cmd.Parameters.AddWithValue("@selectedCriteriaName", criteriaName);
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);

            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != criteriaID)
                        //Criteria is already present in database
                        criteriaFound = true;
                }
            }
            else
            { //No record
                criteriaFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return criteriaFound;
        }

        public Criteria GetDetails(int criteriaId)
        {
            Criteria criteria = new Criteria();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a criteria record.
            cmd.CommandText = @"SELECT * FROM Criteria
            WHERE CriteriaID = @selectedCriteriaID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “CriteriaId”.
            cmd.Parameters.AddWithValue("@selectedCriteriaID", criteriaId);
            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    criteria.CriteriaID = criteriaId;
                    criteria.CompetitionID = reader.GetInt32(1);
                    criteria.CriteriaName = reader.GetString(2);
                    criteria.Weightage = reader.GetInt32(3);
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return criteria;
        }
        public int Update(Criteria criteria)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Criteria SET CompetitionID=@compid, CriteriaName=@name,
            Weightage=@weightage WHERE CriteriaID = @selectedCriteriaID";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@selectedCriteriaID", criteria.CriteriaID);
            cmd.Parameters.AddWithValue("@compid", criteria.CompetitionID);
            cmd.Parameters.AddWithValue("@name", criteria.CriteriaName);
            cmd.Parameters.AddWithValue("@weightage", criteria.Weightage);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();

            //Close the database connection
            conn.Close();
            return count;
        }

        public int Delete(int criteriaId)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a area interest record specified by a CriteriaID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Criteria WHERE CriteriaID = @selectCriteriaID";
            cmd.Parameters.AddWithValue("@selectCriteriaID", criteriaId);
            //Open a database connection
            conn.Open();
            int rowAffected = 0;
            //Execute the DELETE SQL to remove the Criteria record
            rowAffected += cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
            //Return number of row of Criteria record updated or deleted
            return rowAffected;
        }
    }
}
