﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using WEB2021Apr_P04_T4.Models;

namespace WEB2021Apr_P04_T4.DAL
{
    public class JudgeProfileDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        //Constructor
        public JudgeProfileDAL()
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

        public List<JudgeProfile> GetAllJudgeProfile()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM JudgeProfile ORDER BY JudgeID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<JudgeProfile> judgeProfileList = new List<JudgeProfile>();
            while (reader.Read())
            {
                judgeProfileList.Add(
                new JudgeProfile
                {
                    JudgeID = reader.GetInt32(0), //0: 1st column
                    JudgeName = reader.GetString(1), //1: 2nd column
                    //Get the first character of a string
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return judgeProfileList;
        }

        public int Add(JudgeProfile judgeProfile)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO JudgeProfile (JudgeName)
            OUTPUT INSERTED.JudgeID VALUES(@name)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", judgeProfile.JudgeName);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            judgeProfile.JudgeID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return judgeProfile.JudgeID;
        }

        public JudgeProfile GetDetails(int judgeId)
        {
            JudgeProfile judgeProfile = new JudgeProfile();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM JudgeProfile
            WHERE JudgeID = @selectedJudgeID";
            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “judgeId”.
            cmd.Parameters.AddWithValue("@selectedJudgeID", judgeId);
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
                    judgeProfile.JudgeID = judgeId;
                    judgeProfile.JudgeName = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    judgeProfile.AreaOfInterest = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    judgeProfile.JudgeEmail = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    judgeProfile.JudgePassword = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return judgeProfile;
        }

        public int Delete(int judgeId)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a area interest record specified by a Judge ID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM judgeProfile WHERE JudgeID = @selectJudgeID";
            cmd.Parameters.AddWithValue("@selectJudgeID", judgeId);
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
    }
}
