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
            cmd.CommandText = @"INSERT INTO Competition (AreaInterestID, CompetitionName, StartDate, EndDate, ResultReleasedDate)
OUTPUT INSERTED.CompetitionID
VALUES(@interest, @name, @startDate, @endDate, @resultDate)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@interest", competition.AreaInterestID);
            cmd.Parameters.AddWithValue("@name", competition.CompetitionName);
            if (competition.StartDate != null)
                cmd.Parameters.AddWithValue("@startDate", competition.StartDate.Value);
            else
                cmd.Parameters.AddWithValue("@startDate", DBNull.Value);
            if (competition.EndDate != null)
                cmd.Parameters.AddWithValue("@endDate", competition.EndDate.Value);
            else
                cmd.Parameters.AddWithValue("@endDate", DBNull.Value);
            if (competition.ResultReleasedDate != null)
                cmd.Parameters.AddWithValue("@resultDate", competition.ResultReleasedDate.Value);
            else
                cmd.Parameters.AddWithValue("@resultDate", DBNull.Value);
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
        public int Update(Competition competition)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Competition SET AreaInterestID=@interestID,
 CompetitionName=@name, StartDate = @startDate, EndDate = @endDate, ResultReleasedDate = @resultDate
WHERE CompetitionID = @selectedCompetitionID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", competition.CompetitionName);
            cmd.Parameters.AddWithValue("@interestID", competition.AreaInterestID);
            
            if (competition.StartDate != null)
                cmd.Parameters.AddWithValue("@startDate", competition.StartDate.Value);
            else 
                cmd.Parameters.AddWithValue("@startDate", DBNull.Value);
            if (competition.EndDate != null)
                cmd.Parameters.AddWithValue("@endDate", competition.EndDate.Value);
            else
                cmd.Parameters.AddWithValue("@endDate", DBNull.Value);
            if (competition.ResultReleasedDate != null)
                cmd.Parameters.AddWithValue("@resultDate", competition.ResultReleasedDate.Value);
            else
                cmd.Parameters.AddWithValue("@resultDate", DBNull.Value);
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competition.CompetitionID);
            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();
            //Close the database connection
            conn.Close();
            return count;
        }
        public bool IsModifiable(int competitionID)
        {
            bool recordFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT CompetitionID FROM CompetitionSubmission
 WHERE CompetitionID=@selectedCompetitionID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);
            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            { //Records found
                recordFound = true;
            }
            else
            { //No record
                recordFound = false;
            }
            reader.Close();
            conn.Close();

            return recordFound;
        }

        public int Delete(int competitionID)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a area interest record specified by a AreaInterest ID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM Competition WHERE CompetitionID = @selectCompetitionID";
            cmd.Parameters.AddWithValue("@selectCompetitionID", competitionID);
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

        public List<Judge> GetCompetitionJudge(int competitionID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SQL statement that select all branches
            cmd.CommandText = @"SELECT * FROM CompetitionJudge WHERE CompetitionID = @selectedCompetitionID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “branchNo”.
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);

            //Open a database connection
            conn.Open();
            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            List<Judge> judgeList = new List<Judge>();
            while (reader.Read())
            {
                judgeList.Add(
                new Judge
                {
                    JudgeID = reader.GetInt32(0), //0: 1st column
                    JudgeName = reader.GetString(1), //1: 2nd column
                    Salutation = reader.GetString(2),
                    AreaInterestID = reader.GetInt32(3),
                    EmailAddr = reader.GetString(4),
                    Password = reader.GetString(5)
                    //Get the first character of a string
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close database connection
            conn.Close();
            return judgeList;
        }
    }
}
