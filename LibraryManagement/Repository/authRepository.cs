using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LibraryManagement.Models;

namespace LibraryManagement.Repository
{
    public class authRepository
    {
        private SqlConnection _connect;
        //To Handle connection related activities
        private void Connection()
        {
            var constr = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(constr);

        }
        //To Add Author details
        public bool AddAuthenication(AuthModel obj)
        {

            Connection();
            SqlCommand command = new SqlCommand("AddNewAuthentication", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@AuthenticationName", obj.AuthenticationName);

            _connect.Open();
            var i = command.ExecuteNonQuery();
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

        //To view Author details with generic list 
        public List<AuthModel> GetAllAuthentication()
        {
            Connection();
            List<AuthModel> AuthenticationList = new List<AuthModel>();
            SqlCommand command = new SqlCommand("GetAuthentication", _connect);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();

            //Bind AuthorModel generic list using LINQ 
            AuthenticationList = (from DataRow dr in dt.Rows

                          select new AuthModel()
                          {
                              AuthenticationId = Convert.ToInt32(dr["AuthenticationId"]),
                              AuthenticationName = Convert.ToString(dr["AuthenticationName"]),

                          }).ToList();


            return AuthenticationList;


        }

        //To Update Author details
        public bool UpdateAuthor(AuthModel obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateAuthentication", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@AuthenticationId", obj.AuthenticationId);
            com.Parameters.AddWithValue("@AuthenticationName", obj.AuthenticationName);
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
        //To delete Author details
        public bool DeleteAuthentication(int authenticationId)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteAuthenticationID", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@AuthenticationID", authenticationId);

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