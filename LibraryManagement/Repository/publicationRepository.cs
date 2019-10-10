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
    public class PublicationRepository
    {
        private SqlConnection _connect;
        //To Handle connection related activities
        private void Connection()
        {
            var constr = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(constr);

        }
        //To Add Publication details
        public bool AddPublication(PublicationModel obj)
        {

            Connection();
            SqlCommand command = new SqlCommand("AddNewPublication", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PublicationName", obj.PublicationName);

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

        //To view Publication details with generic list 
        public List<PublicationModel> GetAllPublication()
        {
            Connection();
            List<PublicationModel> PublicationList = new List<PublicationModel>();
            SqlCommand command = new SqlCommand("GetPublication", _connect);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();

            //Bind PublicationModel generic list using LINQ 
            PublicationList = (from DataRow dr in dt.Rows

                select new PublicationModel()
                {
                    PublicationId = Convert.ToInt32(dr["PublicationID"]),
                    PublicationName = Convert.ToString(dr["PublicationName"]),

                }).ToList();


            return PublicationList;


        }

        public PublicationModel GetByPublicationId(int id)
        {
            Connection();
            List<PublicationModel> publicationList = new List<PublicationModel>();
            PublicationModel publicationModel = new PublicationModel();
            SqlCommand command = new SqlCommand("GetPublicationByID", _connect);
            command.Parameters.AddWithValue("@PublicationID", id);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind AuthorModel generic list using LINQ 
            publicationList = (from DataRow dataRow in dataTable.Rows

                select new PublicationModel()
                {
                    PublicationId = Convert.ToInt32(dataRow["PublicationID"]),
                    PublicationName = Convert.ToString(dataRow["PublicationName"]),

                }).ToList();

            publicationModel = publicationList[0];

            return publicationModel;

        }

        public List<SearchandUser> searchPublisherByName(String name)
        {
            Connection();
            List<SearchandUser> BookList = new List<SearchandUser>();
            SqlCommand command = new SqlCommand("SearchPublicationByName", _connect);
            command.Parameters.AddWithValue("@PublicationName", name);
            command.CommandType = CommandType.StoredProcedure;

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

        //To Update Publication details
        public bool UpdatePublication(PublicationModel obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdatePublication", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@PublicationID", obj.PublicationId);
            com.Parameters.AddWithValue("@PublicationName", obj.PublicationName);
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
        //To delete Publication details
        public bool DeletePublication(int id)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeletePublication", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@PublicationID", id);

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