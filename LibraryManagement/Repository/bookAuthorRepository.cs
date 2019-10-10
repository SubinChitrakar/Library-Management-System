using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LibraryManagement.Models;

namespace LibraryManagement.Repository
{
    public class BookAuthorRepository
    {
        private SqlConnection _connect;
        //To Handle connection related activities
        private void Connection()
        {
            var constr = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(constr);

        }
        //To Add BookAuthor details
        public virtual bool AddBookAuthor(BookAuthorModel obj)
        {

            Connection();
            SqlCommand command = new SqlCommand("AddNewBookAuthor", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BookID", obj.BookId);
            command.Parameters.AddWithValue("@AuthorID", obj.AuthorId);

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

        //To view BookAuthor details with generic list 
        public List<BookAuthorModel> GetAllBookAuthor()
        {
            Connection();
            List<BookAuthorModel> BookAuthorList = new List<BookAuthorModel>();
            SqlCommand command = new SqlCommand("GetBookAuthor", _connect);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind BookAuthorModel generic list using LINQ 
            BookAuthorList = (from DataRow dataRow in dataTable.Rows

                select new BookAuthorModel()
                {
                    BookAuthorId = Convert.ToInt32(dataRow["BookAuthorID"]),
                    BookId = Convert.ToInt32(dataRow["BookID"]),
                    AuthorId = Convert.ToInt32(dataRow["AuthorID"]),

                }).ToList();


            return BookAuthorList;
        }

        public List<BookAuthorModel> GetByBookId(int bookID)
        {
            Connection();
            List<BookAuthorModel> BookAuthorList = new List<BookAuthorModel>();
            SqlCommand command = new SqlCommand("GetByBookID", _connect);
            command.Parameters.AddWithValue("@BookID", bookID);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind BookAuthorModel generic list using LINQ 
            BookAuthorList = (from DataRow dataRow in dataTable.Rows

                select new BookAuthorModel()
                {
                    BookAuthorId = Convert.ToInt32(dataRow["BookAuthorID"]),
                    BookId = Convert.ToInt32(dataRow["BookID"]),
                    AuthorId = Convert.ToInt32(dataRow["AuthorID"]),
                }).ToList();

            return BookAuthorList;
        }

        //To Update Book details
        public bool UpdateBookAuthor(BookAuthorModel obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateBookAuthor", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookAuthorID", obj.BookAuthorId);
            com.Parameters.AddWithValue("@BookID", obj.BookId);
            com.Parameters.AddWithValue("@AuthorID", obj.AuthorId);

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
        //To delete Book details
        public bool DeleteBookAuthor(int id)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteBookAuthor", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookAuthorID", id);

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

        public bool DeleteBookAuthorByBookId(int id)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteBookAuthorByBookID", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookID", id);

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