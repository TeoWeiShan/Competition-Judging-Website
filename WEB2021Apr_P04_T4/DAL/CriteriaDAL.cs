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

        public bool IsCriteriaNameExist(string criteriaName, int criteriaID)
        {
            bool criteriaFound = false;
            //Create a SqlCommand object and specify the SQL statement 
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CriteriaID FROM Criteria 
                              WHERE CriteriaName=@selectedCriteriaName";
            cmd.Parameters.AddWithValue("@selectedCriteriaName", criteriaName);

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
    }
}
