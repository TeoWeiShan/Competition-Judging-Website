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
    public class CompetitorDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CompetitorDAL()
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
        public List<Competitor> GetAllCompetitor()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Competitor ORDER BY CompetitorID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Competitor> competitorList = new List<Competitor>();
            while (reader.Read())
            {
                competitorList.Add(
                new Competitor
                {
                    CompetitorID = reader.GetInt32(0), //0: 1st column
                    CompetitorName = reader.GetString(1), //1: 2nd column
                    Salutation = reader.GetString(2), //2: 3rd column
                    EmailAddr = reader.GetString(3), //3: 4th column
                    Password = reader.GetString(4), //4: 5th column
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return competitorList;
        }

        public int Add(Competitor competitor)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Competitor (CompetitorName, Salutation, EmailAddr, Password)
                                OUTPUT INSERTED.CompetitorID
                                VALUES(@name, @salutation, @email, @password)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", competitor.CompetitorName);
            cmd.Parameters.AddWithValue("@salutation", competitor.Salutation);
            cmd.Parameters.AddWithValue("@email", competitor.EmailAddr);
            cmd.Parameters.AddWithValue("@password", competitor.Password);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            competitor.CompetitorID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return competitor.CompetitorID;
        }

        internal void Delete(object competitorID)
        {
            throw new NotImplementedException();
        }

        public bool IsEmailExist(string EmailAddr, int competitorID)
        {
            bool emailFound = false;
            //Create a SqlCommand object and specify the SQL statement 
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CompetitorID FROM Competitor 
                              WHERE EmailAddr=@selectedEmailAddr";
            cmd.Parameters.AddWithValue("@selectedEmailAddr", EmailAddr);

            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != competitorID)
                        //The email address is used by another judge
                        emailFound = true;
                }
            }
            else
            { //No record
                emailFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return emailFound;
        }

        public Competitor GetDetails(int competitorId)
        {
            Competitor competitor = new Competitor();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Competitor
                                WHERE CompetitorID = @selectedCompetitorID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorId);
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
                    competitor.CompetitorID = competitorId;
                    competitor.CompetitorName = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    competitor.Salutation = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    competitor.EmailAddr = !reader.IsDBNull(3) ? reader.GetString(3) : null;
                    competitor.Password = !reader.IsDBNull(4) ? reader.GetString(4) : null;

                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return competitor;
        }

        public int Delete(int competitorId)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a area interest record specified by a Competitor ID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Competitor WHERE CompetitorID = @selectCompetitorID";
            cmd.Parameters.AddWithValue("@selectCompetitorID", competitorId);
            //Open a database connection
            conn.Open();
            int rowAffected = 0;
            //Execute the DELETE SQL to remove the staff record
            rowAffected += cmd.ExecuteNonQuery();
            //Close database connection
            conn.Close();
            //Return number of row of staff record updated or deleted
            return rowAffected;
        }
    }
}
