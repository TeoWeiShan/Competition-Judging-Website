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

        public List<Competition> GetAllCompetitorCompetition(int competitorID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"select * from Competition
                                where (DATEDIFF(day, StartDate, GETDATE())) < 3 and
                                (CompetitionID not in (select CompetitionID from CompetitionSubmission where CompetitorID = 1)) and 
                                (CompetitionID in(SELECT CompetitionID FROM CompetitionJudge 
                                GROUP BY CompetitionID HAVING COUNT(CompetitionID) >= 2) )";
            cmd.Parameters.AddWithValue("selectedCompetitorID", competitorID);
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

        public List<Competition> GetCompetitorCompetition(int competitorID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"select * from Competition
                                where (CompetitionID in (select CompetitionID from CompetitionSubmission where CompetitorID = @selectedCompetitorID))";
            cmd.Parameters.AddWithValue("selectedCompetitorID", competitorID);
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

        //public int Add(CompetitorCompetition competitorcompetition)
        //{
            
        //}
    }
}
