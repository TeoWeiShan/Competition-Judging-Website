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
    }
}
