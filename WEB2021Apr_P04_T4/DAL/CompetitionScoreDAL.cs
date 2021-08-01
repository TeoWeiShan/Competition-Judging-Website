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
    public class CompetitionScoreDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        //Constructor
        public CompetitionScoreDAL()
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

        public List<CompetitionScore> GetAllScore(int JudgeID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement to select CompetitionScore only
            //for the competition the Judge is currently in
            cmd.CommandText = @"SELECT * FROM CompetitionScore WHERE
            CompetitionID IN (SELECT CompetitionID FROM CompetitionJudge WHERE JudgeID = @selectedJudgeID)
            AND CompetitionID IN (SELECT CompetitionID from Competition WHERE ResultReleasedDate > GETDATE())";
            cmd.Parameters.AddWithValue("@selectedJudgeID", JudgeID);

            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a Competition Submission list
            List<CompetitionScore> scoreList = new List<CompetitionScore>();

            while (reader.Read())
            {
                scoreList.Add(
                new CompetitionScore
                {
                    CriteriaID = reader.GetInt32(0),
                    CompetitorID = reader.GetInt32(1), //0: 1st column
                    CompetitionID = reader.GetInt32(2), //1: 2nd column
                    Score = reader.GetInt32(3),
                }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return scoreList;
        }

        public List<Competition> GetAvailableCompetition(int JudgeID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement to select the ongoing competition where the judge is judging
            cmd.CommandText = @"select * from Competition where CompetitionID IN (select CompetitionID
            FROM CompetitionJudge WHERE JudgeID = @selectedJudgeID AND CompetitionID IN
            (select CompetitionID from Competition where ResultReleasedDate > GETDATE()))";
            cmd.Parameters.AddWithValue("@selectedJudgeID", JudgeID);

            //Open a database connection
            conn.Open();

            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a competition list
            List<Competition> competitionList = new List<Competition>();
            while (reader.Read())
            {
                competitionList.Add(
                new Competition
                {
                    CompetitionID = reader.GetInt32(0), //0: 1st column
                    AreaInterestID = reader.GetInt32(1), //1: 2nd column
                    CompetitionName = reader.GetString(2),
                    StartDate = reader.GetDateTime(3),
                    EndDate = reader.GetDateTime(4),
                    ResultReleasedDate = reader.GetDateTime(5)
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return competitionList;
        }

        public List<Criteria> GetAvailableCriteria(int JudgeID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement to select Criteria based on the selected CompetitionID where the Judge is in
            cmd.CommandText = @"select * from Criteria where CompetitionID IN (select CompetitionID
            FROM CompetitionJudge WHERE JudgeID = @selectedJudgeID AND CompetitionID IN
            (select CompetitionID from Competition where ResultReleasedDate > GETDATE()))";
            cmd.Parameters.AddWithValue("@selectedJudgeID", JudgeID);

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

        public int Add(CompetitionScore Cscore)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an INSERT SQL statement which will
            //return the auto-generated CriteriaID after insertion
            cmd.CommandText = @"INSERT INTO CompetitionScore (CriteriaID, CompetitorID, CompetitionID, Score)
            OUTPUT INSERTED.CriteriaID
            VALUES(@criteriaid, @competitorid, @competitionid, @score)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@criteriaid", Cscore.CriteriaID);
            cmd.Parameters.AddWithValue("@competitorid", Cscore.CompetitorID);
            cmd.Parameters.AddWithValue("@competitionid", Cscore.CompetitionID);
            cmd.Parameters.AddWithValue("@score", Cscore.Score);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            cmd.ExecuteNonQuery();

            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return Cscore.CriteriaID;
        }

        public CompetitionScore GetDetails(int competitorID)
        {
            CompetitionScore score = new CompetitionScore();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a score record.
            cmd.CommandText = @"SELECT * FROM CompetitionScore WHERE CompetitorID = @selectedCompetitorID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “competitorId”.
            cmd.Parameters.AddWithValue("@selectedCompetitorID", competitorID);

            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //Fill Competitionscore object with values from data reader
                    score.CompetitorID = competitorID;
                    score.CriteriaID = reader.GetInt32(1); //0: 1st column                
                    score.CompetitionID = reader.GetInt32(2);
                    score.Score = reader.GetInt32(3);
                }
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return score;
        }

        public int Update(CompetitionScore score)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE CompetitionScore SET CriteriaID=@criteriaID,
            CompetitionID=@competitionID, Score=@score WHERE CompetitorID = @selectedCompetitorID";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@criteriaID", score.CriteriaID);
            cmd.Parameters.AddWithValue("@selectedCompetitorID", score.CompetitorID);
            cmd.Parameters.AddWithValue("@competitionID", score.CompetitionID);
            cmd.Parameters.AddWithValue("@score", score.Score);

            //Open a database connection
            conn.Open();

            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();

            //Close the database connection
            conn.Close();
            return count;
        }
    }
}
