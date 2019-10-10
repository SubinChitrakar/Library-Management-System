using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Repository
{
    public class UserRepository
    {
        private SqlConnection _connect;
        private void Connection()
        {
            string configuration = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(configuration);
        }

        public bool AddUser(UserModel obj)
        {
            Connection();
            SqlCommand command = new SqlCommand("AddNewUsers", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserName", obj.UserName);
            command.Parameters.AddWithValue("@Password", obj.Password);
            command.Parameters.AddWithValue("@AuthenticationID", obj.AuthenticationId);

            _connect.Open();
            int i = command.ExecuteNonQuery();
            _connect.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<UserModel> GetAllUsers()
        {
            Connection();
            SqlCommand command= new SqlCommand("GetUsers", _connect);
            List<UserModel> userList=new List<UserModel>();
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            userList = (from DataRow dataRow in dataTable.Rows

                select new UserModel()
                {
                    UserId = Convert.ToInt32(dataRow["UserID"]),
                    UserName = Convert.ToString(dataRow["UserName"]),
                    Password = Convert.ToString(dataRow["Password"]),
                    AuthenticationId = Convert.ToInt32(dataRow["AuthenticationID"])
                }).ToList();

            return userList;
        }

        public List<UserWithAuthentication> GetAllUsersWithAuthentication()
        {
            Connection();
            SqlCommand command = new SqlCommand("GetUserWithAuthentication", _connect);
            List<UserWithAuthentication> userWithAuthenticationList = new List<UserWithAuthentication>();
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            userWithAuthenticationList = (from DataRow dataRow in dataTable.Rows

                select new UserWithAuthentication()
                {
                    UserId = Convert.ToInt32(dataRow["UserID"]),
                    UserName = Convert.ToString(dataRow["UserName"]),
                    Password = Convert.ToString(dataRow["Password"]),
                    AuthenticationName = Convert.ToString(dataRow["AuthenticationName"])
                }).ToList();

            return userWithAuthenticationList;
        }


        public UserModel GetAllAuthentication()
        {
            Connection();
            SqlCommand command = new SqlCommand("GetAuthentication", _connect);
            UserModel userModel = new UserModel();
            List<AuthModel> authList = new List<AuthModel>();
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            authList = (from DataRow dataRow in dataTable.Rows

                select new AuthModel()
                {
                    AuthenticationId = Convert.ToInt32(dataRow["AuthenticationID"]),
                    AuthenticationName = Convert.ToString(dataRow["AuthenticationName"]),
                    
                }).ToList();

            userModel.AuthenticationList = authList;

            return userModel;
        }

        public bool UpdateUser(UserModel obj)
        {
            Connection();
            SqlCommand com = new SqlCommand("UpdateUserDetails", _connect);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserID", obj.UserId);
            com.Parameters.AddWithValue("@UserName", obj.UserName);
            com.Parameters.AddWithValue("@Password", obj.Password);
            com.Parameters.AddWithValue("@AuthenticationID", obj.AuthenticationId);

            _connect.Open();
            int i = com.ExecuteNonQuery();
            _connect.Close();

            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteUser(int id)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteUserById", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserID", id);

            _connect.Open();
            int i = com.ExecuteNonQuery();
            _connect.Close();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<UserModel> VerifyUser(UserModel user)
        {
            Connection();
            SqlCommand command = new SqlCommand("CheckUser", _connect);
            List<UserModel> checkUserList=new List<UserModel>();
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserName", user.UserName);
            command.Parameters.AddWithValue("@Password", user.Password);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            checkUserList = (from DataRow dataRow in dataTable.Rows

                select new UserModel()
                {
                    UserId = Convert.ToInt32(dataRow["UserID"]),
                    UserName = Convert.ToString(dataRow["UserName"]),
                    Password = Convert.ToString(dataRow["Password"]),
                    AuthenticationId = Convert.ToInt32(dataRow["AuthenticationID"])
                }).ToList();

            return checkUserList;
        }

        public Boolean ChangePassword(UserModel user)
        {
            Connection();
            SqlCommand com = new SqlCommand("ChangePassword", _connect);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@UserID", user.UserId);
            com.Parameters.AddWithValue("@Password", user.Password);
            _connect.Open();
            int i = com.ExecuteNonQuery();
            _connect.Close();

            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}