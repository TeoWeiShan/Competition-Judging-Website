using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WEB2021Apr_P04_T4.Models;

namespace WEB2021Apr_P04_T4.DAL
{
    public class CompetitorCompetitionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CompetitorCompetitionDAL()
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

        public List<CompetitorCompetition> GetAllCompetitorCompetition(int competitorID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"select Competition.CompetitionName, CompetitionSubmission.* from (CompetitionSubmission
INNER JOIN Competition ON Competition.CompetitionID = CompetitionSubmission.CompetitionID) Where CompetitionSubmission.CompetitorID = @selectedCompetitorID";
            cmd.Parameters.AddWithValue("selectedCompetitorID", competitorID);
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            //Read all records until the end, save data into a staff list
            List<CompetitorCompetition> competitorcompetitionList = new List<CompetitorCompetition>();
            while (reader.Read())
            {
                competitorcompetitionList.Add(
                new CompetitorCompetition
                {
                    CompetitionID = reader.GetInt32(1), //0: 1st column
                    CompetitionName = reader.GetString(0),//1: 2nd column
                    CompetitorID = reader.GetInt32(2),
                    FileSubmitted = !reader.IsDBNull(3) ? reader.GetString(3) : (string?)null,
                    DateTimeFileUpload = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                    Appeal = !reader.IsDBNull(5) ? reader.GetString(5) : (string?)null,
                    VoteCount = reader.GetInt32(6),
                    Ranking = !reader.IsDBNull(7) ? reader.GetInt32(7) : (int?)null,
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return competitorcompetitionList;
        }

        public int Join(int competitionID, int competitorID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Competitor (CompetitionID, CompetitorID, VoteCount)
                                OUTPUT INSERTED.CompetitorID
                                VALUES(@competitionID, @competitorID, @vote)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@competitionID", competitionID);
            cmd.Parameters.AddWithValue("@competitorID", competitorID);
            cmd.Parameters.AddWithValue("@vote", 0);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return (competitionID);
        }
    }
}
