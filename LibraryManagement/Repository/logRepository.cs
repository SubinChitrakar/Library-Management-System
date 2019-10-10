using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LibraryManagement.Models;

namespace LibraryManagement.Repository
{
    public class logRepository
    {
        private SqlConnection _connect;

        //To Handle connection related activities
        private void Connection()
        {
            var constr = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(constr);
        }

        //To Add Author details
        public bool AddLog(LogModel obj)
        {
            Connection();
            var command = new SqlCommand("AddNewLog", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserId", obj.UserId);
            command.Parameters.AddWithValue("@TableName", obj.TableName);
            command.Parameters.AddWithValue("@Activity", obj.Activity);
            command.Parameters.AddWithValue("@LogDate", obj.LogDate);

            _connect.Open();
            var i = command.ExecuteNonQuery();
            _connect.Close();
            if (i >= 1)
                return true;
            return false;
        }

        //To view Author details with generic list 
        public List<LogModel> GetAllLog()
        {
            Connection();
            var LogList = new List<LogModel>();
            var command = new SqlCommand("GetLog", _connect);
            command.CommandType = CommandType.StoredProcedure;
            var dataAdapter = new SqlDataAdapter(command);
            var dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind AuthorModel generic list using LINQ 
            LogList = (from DataRow dataRow in dataTable.Rows
                select new LogModel
                {
                    LogId = Convert.ToInt32(dataRow["LogID"]),
                    Username = Convert.ToString(dataRow["Username"]),
                    TableName = Convert.ToString(dataRow["TableName"]),
                    Activity = Convert.ToString(dataRow["Activity"]),
                    LogDate = Convert.ToDateTime(dataRow["LogDate"])
                }).ToList();
            return LogList;
        }
    }
}