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
    public class CompetitionSubmissionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CompetitionSubmissionDAL()
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

        public List<CompetitionSubmission> GetAllSubmission(int JudgeID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement to select all submissions
            //for the ongoing competition the judge is in
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission WHERE
            CompetitionID IN (SELECT CompetitionID FROM CompetitionJudge WHERE JudgeID = @selectedJudgeID
            AND COMPETITIONID IN (SELECT CompetitionID from Competition WHERE ResultReleasedDate > GETDATE()))";
            cmd.Parameters.AddWithValue("@selectedJudgeID", JudgeID);

            //Open a database connection
            conn.Open();

            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a Competition Submission list
            List<CompetitionSubmission> submissionList = new List<CompetitionSubmission>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    submissionList.Add(
                new CompetitionSubmission
                {
                    CompetitionID = reader.GetInt32(0), //0: 1st column
                    CompetitorID = reader.GetInt32(1),
                    FileSubmitted = reader.IsDBNull(2) ? null : reader.GetString(2),
                    DateTimeFileUpload = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,
                    Appeal = reader.IsDBNull(4) ? null : reader.GetString(4),
                    VoteCount = reader.GetInt32(5),
                    Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null,
                }
                );
                }

            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return submissionList;
        }

        public List<CompetitionSubmission> GetAvailableSubmissions(int JudgeID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement to select the submission(s) from the competition where the judge is judging
            cmd.CommandText = @"select * from CompetitionSubmission where CompetitionID IN (select CompetitionID
            FROM CompetitionJudge WHERE JudgeID = @selectedJudgeID AND CompetitionID IN
            (select CompetitionID from Competition where ResultReleasedDate > GETDATE()))";
            cmd.Parameters.AddWithValue("@selectedJudgeID", JudgeID);

            //Open a database connection
            conn.Open();

            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a competitionsubmission list
            List<CompetitionSubmission> submissionList = new List<CompetitionSubmission>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    submissionList.Add(
                    new CompetitionSubmission
                    {
                        CompetitionID = reader.GetInt32(0), //0: 1st column
                        CompetitorID = reader.GetInt32(1), //1: 2nd column
                        FileSubmitted = reader.GetString(2),
                        DateTimeFileUpload = reader.GetDateTime(3),
                        Appeal = reader.GetString(4),
                        VoteCount = reader.GetInt32(5),
                        Ranking = reader.GetInt32(6)
                    }
                    );
                }
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return submissionList;
        }

        public CompetitionSubmission GetDetails(int competitorId)
        {
            CompetitionSubmission submission = new CompetitionSubmission();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that retrieves all attributes of
            //a submission record for the competition that the Judge is in
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission WHERE CompetitorID = @selectedCompetitorID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “competitionId”.
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
                    // Fill competitionsubmission object with values from the data reader 
                    submission.CompetitorID = competitorId;
                    submission.CompetitionID = reader.GetInt32(1);
                    submission.FileSubmitted = reader.IsDBNull(2) ? null : reader.GetString(2);
                    submission.DateTimeFileUpload = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null;
                    submission.Appeal = reader.IsDBNull(4) ? null : reader.GetString(4);
                    submission.VoteCount = reader.GetInt32(5);
                    submission.Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null;
                }
            }

            //Close DataReader
            reader.Close();
            //Close Database connection
            conn.Close();

            return submission;
        }

        public int Update(CompetitionSubmission submission)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE CompetitionSubmission SET 
            Ranking=@ranking WHERE CompetitorID = @selectedCompetitorID";

            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@selectedCompetitorID", submission.CompetitorID);         
            if (submission.Ranking != null)
                cmd.Parameters.AddWithValue("@ranking", submission.Ranking.Value);
            else
                cmd.Parameters.AddWithValue("@ranking", DBNull.Value);

            //Open a database connection
            conn.Open();
            //ExecuteNonQuery is used for UPDATE and DELETE
            int count = cmd.ExecuteNonQuery();

            //Close the database connection
            conn.Close();
            return count;
        }

        //list for score and weightage to be calculated together to get total score
        //public List<CompetitionScore> GetScoreAndWeightage(int CompetitorID, int CompetitionID)
        //{
        //    //Create a SqlCommand object from connection object
        //    SqlCommand cmd = conn.CreateCommand();
        //    //Specify the SELECT SQL statement to select the submission(s) from the competition where the judge is judging
        //    cmd.CommandText = @"SELECT cs.Score, c.Weightage FROM CompetitionScore cs
        //    INNER JOIN Criteria c ON cs.CriteriaID = c.CriteriaID 
        //    WHERE (cs.CompetitorID = 8) AND (cs.CompetitionID = @selectedCompetitionID)";

        //    cmd.Parameters.AddWithValue("@selectedCompetitorID", CompetitorID);
        //    cmd.Parameters.AddWithValue("@selectedCompetitionID", CompetitionID);

        //    //Open a database connection
        //    conn.Open();

        //    //Execute the SELECT SQL through a DataReader
        //    SqlDataReader reader = cmd.ExecuteReader();

        //    //Read all records until the end, save data into a competitionsubmission list
        //    List<CompetitionScore> scoreList = new List<CompetitionScore>();
        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            scoreList.Add(
        //            new CompetitionScore
        //            {
        //                CriteriaID = reader.GetInt32(0), //0: 1st column
        //                CompetitorID = reader.GetInt32(1), //1: 2nd column
        //                CompetitionID = reader.GetInt32(2),
        //                Score = reader.GetInt32(3)
        //            }
        //            );
        //        }
        //    }

        //    //Close DataReader
        //    reader.Close();
        //    //Close the database connection
        //    conn.Close();

        //    return scoreList;
        //}
    }
}

