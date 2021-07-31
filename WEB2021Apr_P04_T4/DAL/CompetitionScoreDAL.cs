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
                   // DateTimeLastEdit = reader.GetDateTime(3)                    
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

       
        //public List<CompetitionSubmission> GetAvailableSubmissions(int CompetitionID)
        //{
        //    //Create a SqlCommand object from connection object
        //    SqlCommand cmd = conn.CreateCommand();
        //    //Specify the SELECT SQL statement to select the submission(s) from the competition where the judge is judging
        //    cmd.CommandText = @"SELECT * from CompetitionSubmission WHERE where CompetitionID = @selectedCompetitionID";
        //    cmd.Parameters.AddWithValue("@selectedCompetitionID", CompetitionID);
        //    //Open a database connection
        //    conn.Open();
        //    //Execute the SELECT SQL through a DataReader
        //    SqlDataReader reader = cmd.ExecuteReader();
        //    //Read all records until the end, save data into a competitionsubmission list
        //    List<CompetitionSubmission> submissionList = new List<CompetitionSubmission>();
        //    while (reader.Read())
        //    {
        //        submissionList.Add(
        //        new CompetitionSubmission
        //        {
        //            CompetitionID = reader.GetInt32(0), //0: 1st column
        //            CompetitorID = reader.GetInt32(1), //1: 2nd column
        //            FileSubmitted = reader.GetString(2),
        //            DateTimeFileUpload = reader.GetDateTime(3),
        //            Appeal = reader.GetString(4),
        //            VoteCount = reader.GetInt32(5),
        //            Ranking = reader.GetInt32(6)
        //        }
        //        );
        //    }
        //    //Close DataReader
        //    reader.Close();
        //    //Close the database connection
        //    conn.Close();

        //    return submissionList;
        //}
        public int Add(CompetitionScore score)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO CompetitionScore (CriteriaID, CompetitorID, CompetitionID, Score)
            OUTPUT INSERTED.CompetitorID
            VALUES(@criteriaid, @competitorid, @competitionid, @score)";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@criteriaid", score.CriteriaID);
            cmd.Parameters.AddWithValue("@competitorid", score.CompetitorID);
            cmd.Parameters.AddWithValue("@competitionid", score.CompetitionID);
            cmd.Parameters.AddWithValue("@score", score.Score);

            //A connection to database must be opened before any operations made.
            conn.Open();

            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            score.CompetitorID = (int)cmd.ExecuteScalar();

            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return score.CompetitorID;
        }
    }
}
