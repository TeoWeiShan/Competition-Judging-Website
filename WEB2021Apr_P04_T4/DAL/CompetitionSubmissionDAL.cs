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

            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission WHERE
            CompetitionID = (SELECT CompetitionID FROM CompetitionJudge WHERE JudgeID = @selectedJudgeID)";
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

        public CompetitionSubmission GetDetails(int competitionId)
        {
            CompetitionSubmission submission = new CompetitionSubmission();

            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that retrieves all attributes of
            //a submission record for the competition that the Judge is in
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission WHERE CompetitionID = @selectedCompetitionID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “competitionId”.
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
                    // Fill competitionsubmission object with values from the data reader 
                    submission.CompetitionID = competitionId;
                    submission.CompetitorID = reader.GetInt32(1);
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
    }
}

