using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Repository
{
    public class AuthorRepository
    {
         private SqlConnection _connect;
        //To Handle connection related activities
        private void Connection()
        {
            var constr = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(constr);

        }
        //To Add Author details
        public bool AddAuthor(AuthorModel obj)
        {

            Connection();
            SqlCommand command = new SqlCommand("AddNewAuthor", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@AuthorName", obj.AuthorName);
          
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
        public List<AuthorModel> GetAllAuthor()
        {
            Connection();
            List<AuthorModel> AuthorList = new List<AuthorModel>();
            SqlCommand command = new SqlCommand("GetAuthor", _connect);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();

            //Bind AuthorModel generic list using LINQ 
            AuthorList = (from DataRow dr in dt.Rows

                select new AuthorModel()
                {
                    AuthorId = Convert.ToInt32(dr["AuthorID"]),
                    AuthorName = Convert.ToString(dr["AuthorName"]),
                
                }).ToList();


            return AuthorList;


        }

        public AuthorModel GetByAuthorId(int id)
        {
            Connection();
            List<AuthorModel> AuthorList = new List<AuthorModel>();
            AuthorModel authorModel=new AuthorModel();
            SqlCommand command = new SqlCommand("GetAuthorByID", _connect);
            command.Parameters.AddWithValue("@AuthorID", id);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind AuthorModel generic list using LINQ 
            AuthorList = (from DataRow dataRow in dataTable.Rows

                select new AuthorModel()
                {
                    AuthorId = Convert.ToInt32(dataRow["AuthorID"]),
                    AuthorName = Convert.ToString(dataRow["AuthorName"]),

                }).ToList();

            authorModel = AuthorList[0];

            return authorModel;


        }

        public List<SearchandUser> searchAuthorByName(String name)
        {
            Connection();
            List<SearchandUser> BookList = new List<SearchandUser>();
            SqlCommand command = new SqlCommand("SearchAuthorByName", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@AuthorName", name);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind BookModel generic list using LINQ 
            BookList = (from DataRow dataRow in dataTable.Rows

                select new SearchandUser()
                {
                    BookName = Convert.ToString(dataRow["BookName"]),
                    AuthorName = Convert.ToString(dataRow["AuthorName"]),
                    PublicationName = Convert.ToString(dataRow["PublicationName"]),
                    RackNo = Convert.ToInt32(dataRow["RackNo"]),
                    Quantity = Convert.ToInt32(dataRow["NoOfCopies"]),
                }).ToList();
            return BookList;
        }


        //To Update Author details
        public bool UpdateAuthor(AuthorModel obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateAuthor", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@AuthorID", obj.AuthorId);
            com.Parameters.AddWithValue("@AuthorName", obj.AuthorName);
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
        public bool DeleteAuthor(int authorId)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteAuthor", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@AuthorID", authorId);

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