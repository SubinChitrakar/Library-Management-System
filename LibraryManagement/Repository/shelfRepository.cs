using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using LibraryManagement.Models;
using System.Linq;
using System.Web;

namespace libraryManagement.Repository
{
    public class ShelfRepository
    {
        private SqlConnection _connect;

        private void Connection()
        {
            var configuration = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(configuration);

        }

        //To Add
        public bool AddShelf(ShelfModel obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("AddNewShelf", _connect);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@RackNo", obj.RackNo);
            
            _connect.Open();
            var i = com.ExecuteNonQuery();
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
        //To view
        public List<ShelfModel> GetAllShelf()
        {
            Connection();
            List<ShelfModel> ShelfList = new List<ShelfModel>();
            SqlCommand command = new SqlCommand("GetShelf", _connect);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();


            ShelfList = (from DataRow dr in dataTable.Rows

                select new ShelfModel()
                {
                    ShelfId = Convert.ToInt32(dr["ShelfId"]),
                    RackNo = Convert.ToInt32(dr["RackNo"]),
                }).ToList();


            return ShelfList;


        }

        //To Update
        public bool UpdateShelf(ShelfModel obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateShelf", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ShelfId", obj.ShelfId);
            com.Parameters.AddWithValue("@RackNo", obj.RackNo);
            
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

        //To delete
        public bool DeleteShelf(int id)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteShelf", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ShelfId", id);

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