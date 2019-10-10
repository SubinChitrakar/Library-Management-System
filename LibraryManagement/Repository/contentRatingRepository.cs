using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LibraryManagement.Models;

namespace LibraryManagement.Repository
{
    public class ContentRatingRepository
    {
        private SqlConnection _connect;
        //To Handle connection related activities
        private void Connection()
        {
            var constr = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(constr);

        }
        //To Add ContentRating details
        public bool AddContentRating(ContentRatingModel obj)
        {

            Connection();
            SqlCommand command = new SqlCommand("AddNewContentRating", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ContentRating", obj.ContentRatingName);

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

        //To view ContentRating details with generic list 
        public List<ContentRatingModel> GetAllContentRating()
        {
            Connection();
            List<ContentRatingModel> ContentRatingList = new List<ContentRatingModel>();
            SqlCommand command = new SqlCommand("GetContentRating", _connect);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();

            //Bind ContentRatingModel generic list using LINQ 
            ContentRatingList = (from DataRow dr in dt.Rows

                select new ContentRatingModel()
                {
                    ContentRatingId = Convert.ToInt32(dr["ContentRatingID"]),
                    ContentRatingName = Convert.ToString(dr["ContentRating"]),

                }).ToList();


            return ContentRatingList;


        }

        public ContentRatingModel GetByContentId(int id)
        {
            Connection();
            List<ContentRatingModel> contentList = new List<ContentRatingModel>();
            ContentRatingModel contentRatingModel = new ContentRatingModel();
            SqlCommand command = new SqlCommand("GetContentRatingByID", _connect);
            command.Parameters.AddWithValue("@ContentRatingID", id);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind AuthorModel generic list using LINQ 
            contentList = (from DataRow dataRow in dataTable.Rows

                select new ContentRatingModel()
                {
                    ContentRatingId = Convert.ToInt32(dataRow["ContentRatingID"]),
                    ContentRatingName = Convert.ToString(dataRow["ContentRating"]),

                }).ToList();

            contentRatingModel = contentList[0];

            return contentRatingModel;

        }

        //To Update ContentRating details
        public bool UpdateContentRating(ContentRatingModel obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateContentRating", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ContentRatingID", obj.ContentRatingId);
            com.Parameters.AddWithValue("@ContentRating", obj.ContentRatingName);
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
        //To delete ContentRating details
        public bool DeleteContentRating(int contentRatingId)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteContentRating", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ContentRatingID", contentRatingId);

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