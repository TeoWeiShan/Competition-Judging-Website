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
    public class CompetitionJudgeDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        //Constructor
        public CompetitionJudgeDAL()
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

        public bool IsJudgeModifiable(int competitionID)
        {
            bool recordFound = false;
            //Create a SqlCommand object and specify the SQL statement
            //to get a staff record with the email address to be validated
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT EndDate FROM Competition
 WHERE CompetitionID=@selectedCompetitionID";
            cmd.Parameters.AddWithValue("@selectedCompetitionID", competitionID);
            //Open a database connection and execute the SQL statement
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DateTime EndDate = reader.GetDateTime(0);
                if (EndDate > DateTime.Today)
                {
                    recordFound = false;
                }
                else
                {
                    recordFound = true;
                }

            }
            reader.Close();
            conn.Close();

            return recordFound;
        }

        

        public int AddCompJudge(CompetitionJudgeViewModel competitionJudge)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO CompetitionJudge (CompetitionID, JudgeID)
OUTPUT INSERTED.CompetitionID
VALUES(@competitionId, @judgeId)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@competitionId", competitionJudge.CompetitionID);
            cmd.Parameters.AddWithValue("@judgeId", competitionJudge.JudgeID);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            cmd.ExecuteNonQuery();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return competitionJudge.CompetitionID;
        }
        public int RemoveCompJudge(CompetitionJudgeViewModel competitionJudge)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"DELETE FROM CompetitionJudge WHERE CompetitionID = @selectCompetitionID and JudgeID = @selectJudgeID";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@selectCompetitionID", competitionJudge.CompetitionID);
            cmd.Parameters.AddWithValue("@selectJudgeID", competitionJudge.JudgeID);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            cmd.ExecuteNonQuery();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return competitionJudge.CompetitionID;

        }
    }
}
