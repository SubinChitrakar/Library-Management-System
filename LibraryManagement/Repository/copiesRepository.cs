using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using LibraryManagement.Models;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Repository
{
    public class copiesRepository
    {
        private SqlConnection _connect;

        //To Handle connection related activities
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(constr);

        }

        //To Add Book details
        public bool AddCopies(CopiesModel obj)
        {

            Connection();
            SqlCommand command = new SqlCommand("AddNewCopies", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@BookID", obj.BookId);
            command.Parameters.AddWithValue("@DateBought", obj.DateBought);
            command.Parameters.AddWithValue("@Location", obj.Location);
            command.Parameters.AddWithValue("@CopyNo", obj.CopyNo);

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

        //To view Book details with generic list 
        public List<CopiesModel> GetAllCopies()
        {
            Connection();
            List<CopiesModel> BookList = new List<CopiesModel>();
            SqlCommand command = new SqlCommand("GetCopies", _connect);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind BookModel generic list using LINQ 
            BookList = (from DataRow dataRow in dataTable.Rows

                select new CopiesModel()
                {
                    CopiesId = Convert.ToInt32(dataRow["CopiesID"]),
                    BookId = Convert.ToInt32(dataRow["BookID"]),
                    DateBought = Convert.ToString(dataRow["DateBought"]),
                    Location = Convert.ToInt32(dataRow["Location"]),

                }).ToList();


            return BookList;
        }

        public CopiesModel SearchCopyById(int id)
        {
            Connection();
            List<CopiesModel> copiesList = new List<CopiesModel>();
            CopiesModel copiesModel = new CopiesModel();
            SqlCommand command = new SqlCommand("GetCopyByID", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CopiesID", id);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind BookModel generic list using LINQ 
            copiesList = (from DataRow dataRow in dataTable.Rows

                select new CopiesModel()
                {
                    CopiesId = Convert.ToInt32(dataRow["CopiesID"]),
                    BookId = Convert.ToInt32(dataRow["BookID"]),
                    DateBought = Convert.ToString(dataRow["DateBought"]),
                    Location = Convert.ToInt32(dataRow["Location"]),

                }).ToList();

            copiesModel = copiesList[0];

            return copiesModel;
        }

        public List<CopyDetail> GetAllCopyDetails()
        {
            Connection();
            List<CopyDetail> copiesList = new List<CopyDetail>();
            SqlCommand command = new SqlCommand("GetCopyDetails", _connect);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            copiesList = (from DataRow dataRow in dataTable.Rows

                select new CopyDetail()
                {
                    BookName = Convert.ToString(dataRow["BookName"]),
                    RackNo = Convert.ToInt32(dataRow["RackNo"]),
                    Quantity = Convert.ToInt32(dataRow["TotalCopies"])
                }).ToList();

            return copiesList;
        }

        public List<CopyDetail> GetAllCopiesOfABook(int id)
        {
            Connection();
            List<CopyDetail> copiesList = new List<CopyDetail>();
            SqlCommand command = new SqlCommand("GetCopiesOfBook", _connect);
            command.Parameters.AddWithValue("@BookID", id);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            copiesList = (from DataRow dataRow in dataTable.Rows

                select new CopyDetail()
                {
                    BookName = Convert.ToString(dataRow["BookName"]),
                    RackNo = Convert.ToInt32(dataRow["RackNo"]),
                    BookType = Convert.ToString(dataRow["BookType"]),
                    CopiesId = Convert.ToInt32(dataRow["CopiesID"]),
                }).ToList();

            return copiesList;
        }


        public Boolean UpdateCopiesStatus(int id)
        {
            Connection();
            SqlCommand com = new SqlCommand("UpdateCopiesStatus", _connect);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CopiesID", id);
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

        public Boolean ChangeCopiesStatus(int id)
        {
            Connection();
            SqlCommand com = new SqlCommand("UpdateCopiesStatustoFalse", _connect);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CopiesID", id);
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

        public List<CopyDetail> checkOldBooks()
        {
            Connection();
            DateTime checkDate;
            checkDate=DateTime.Now.AddDays(-356);
            List<CopyDetail> copiesList = new List<CopyDetail>();
            SqlCommand command = new SqlCommand("CheckBook", _connect);
            command.Parameters.AddWithValue("@CheckDate",checkDate);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            copiesList = (from DataRow dataRow in dataTable.Rows

                select new CopyDetail()
                {
                    BookName = Convert.ToString(dataRow["BookName"]),
                    RackNo = Convert.ToInt32(dataRow["RackNo"]),
                    BookType = Convert.ToString(dataRow["BookType"]),
                    CopiesId = Convert.ToInt32(dataRow["CopiesID"]),
                }).ToList();

            return copiesList;
        }

        public bool DeleteCopies(int id)
        {
            Connection();
            SqlCommand com = new SqlCommand("DeleteCopies", _connect);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CopiesID", id);

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