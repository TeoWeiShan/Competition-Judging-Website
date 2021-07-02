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
    public class CompetitionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CompetitionDAL()
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
        public List<Competition> GetAllCompetition()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Competition ORDER BY CompetitionID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<Competition> competitionList = new List<Competition>();
            while (reader.Read())
            {
                competitionList.Add(
                new Competition
                {
                    CompetitionID = reader.GetInt32(0), //0: 1st column
                    AreaInterestID = reader.GetInt32(1),//1: 2nd column
                    CompetitionName = reader.GetString(2),
                    StartDate = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,
                    EndDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                    ResultReleasedDate = !reader.IsDBNull(5) ? reader.GetDateTime(5) : (DateTime?)null
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return competitionList;
        }

        public int Add(Competition competition)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            //, StartDate, EndDate,ResultReleaseDate  //, @start, @end, @result
            cmd.CommandText = @"INSERT INTO Competition (AreaInterestID, CompetitionName)
OUTPUT INSERTED.CompetitionID
VALUES(@interest, @name)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@interest", competition.AreaInterestID);
            cmd.Parameters.AddWithValue("@name", competition.CompetitionName);
            //cmd.Parameters.AddWithValue("@start", competition.StartDate);
            //cmd.Parameters.AddWithValue("@end", competition.EndDate);
            //cmd.Parameters.AddWithValue("@result", competition.ResultReleasedDate);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            competition.CompetitionID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return competition.CompetitionID;
        }

        public bool IsCompetitionExist(string competitionName, int competitionID)
        {
            bool competitionFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CompetitionID FROM Competition
 WHERE CompetitionName=@selectedCompetitionName";
            cmd.Parameters.AddWithValue("@selectedCompetitionName", competitionName);
            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                while (reader.Read())
                {
                    if (reader.GetInt32(0) != competitionID)
                        //The email address is used by another staff
                        competitionFound = true;
                }
            }
            else
            { //No record
                competitionFound = false; // The email address given does not exist
            }
            reader.Close();
            conn.Close();

            return competitionFound;
        }

        public Competition GetDetails(int competitionId)
        {
            Competition competition = new Competition();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Competition
 WHERE CompetitionID = @selectedCompetitionID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “staffId”.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionId);
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
                    competition.CompetitionID = competitionId;
                    competition.AreaInterestID =
                    reader.GetInt32(1);
                    competition.CompetitionName = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    // (char) 0 - ASCII Code 0 - null value
                    
                    competition.StartDate= !reader.IsDBNull(3) ?
                    reader.GetDateTime(3) : (DateTime?)null;
                    competition.EndDate = !reader.IsDBNull(4) ?
                    reader.GetDateTime(4) : (DateTime?)null;
                    competition.ResultReleasedDate = !reader.IsDBNull(5) ?
                    reader.GetDateTime(5) : (DateTime?)null;
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return competition;
        }
    }
}
