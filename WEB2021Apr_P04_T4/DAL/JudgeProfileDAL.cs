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

        public List<Judge> GetAllJudge()
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement
            cmd.CommandText = @"SELECT * FROM Judge ORDER BY JudgeID";
            //Open a database connection
            conn.Open();
            //Execute the SELECT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();

            //Read all records until the end, save data into a staff list
            List<Judge> judgeList = new List<Judge>();
            while (reader.Read())
            {
                judgeList.Add(
                new Judge
                {
                    JudgeID = reader.GetInt32(0), //0: 1st column
                    JudgeName = reader.GetString(1), //1: 2nd column
                    Salutation = reader.GetString(2),
                    AreaInterestID = reader.GetInt32(3),
                    EmailAddr = reader.GetString(4),
                    JudgePassword = reader.GetString(5)
                    //Get the first character of a string
                }
                );
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();

            return judgeList;
        }

        public int Add(Judge judge)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify an INSERT SQL statement which will
            //return the auto-generated StaffID after insertion
            cmd.CommandText = @"INSERT INTO Judge (JudgeName)
            OUTPUT INSERTED.JudgeID VALUES(@name)";
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            cmd.Parameters.AddWithValue("@name", judge.JudgeName);
            //A connection to database must be opened before any operations made.
            conn.Open();
            //ExecuteScalar is used to retrieve the auto-generated
            //StaffID after executing the INSERT SQL statement
            judge.JudgeID = (int)cmd.ExecuteScalar();
            //A connection should be closed after operations.
            conn.Close();
            //Return id when no error occurs.
            return judge.JudgeID;
        }

        public Judge GetDetails(int judgeId)
        {
            Judge judge = new Judge();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();
            //Specify the SELECT SQL statement that
            //retrieves all attributes of a staff record.
            cmd.CommandText = @"SELECT * FROM Judge
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
                    judge.JudgeID = judgeId;
                    judge.JudgeName = reader.GetString(1);
                    judge.Salutation = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    judge.AreaInterestID = reader.GetInt32(3);
                    judge.EmailAddr = reader.GetString(4);
                    judge.JudgePassword = reader.GetString(5);
                }
            }
            //Close DataReader
            reader.Close();
            //Close the database connection
            conn.Close();
            return judge;
        }

        public int Delete(int judgeId)
        {
            //Instantiate a SqlCommand object, supply it with a DELETE SQL statement
            //to delete a area interest record specified by a Judge ID
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"DELETE FROM judge WHERE JudgeID = @selectJudgeID";
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
