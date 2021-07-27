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

        public List<CompetitionSubmission> GetAllSubmission()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM CompetitionSubmission ORDER BY CompetitionID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a Competition Submission list
            List<CompetitionSubmission> submissionList = new List<CompetitionSubmission>();

            while (reader.Read())
            {
                submissionList.Add(
                new CompetitionSubmission
                {
                    CompetitionID = reader.GetInt32(0), //0: 1st column
                    CompetitorID = reader.GetInt32(1), //1: 2nd column
                    FileSubmitted = reader.IsDBNull(2) ? null : reader.GetString(2),
                    DateTimeFileUpload = !reader.IsDBNull(3) ? reader.GetDateTime(3) : (DateTime?)null,
                    Appeal = reader.IsDBNull(4) ? null : reader.GetString(4),
                    VoteCount = reader.GetInt32(5),
                    Ranking = !reader.IsDBNull(6) ? reader.GetInt32(6) : (int?)null,
                }
                );
            }

            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return submissionList;
        }

    }
}

