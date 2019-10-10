using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LibraryManagement.Models;

namespace LibraryManagement.Repository
{
    public class MembershipRepository
    {
        private SqlConnection _connect;

        private void Connection()
        {
            var configuration = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(configuration);
        }

        public bool AddMembership(MembershipModel obj)
        {
            Connection();
            SqlCommand command = new SqlCommand("AddNewMembership", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MembershipCategory", obj.MembershipCategory);
            command.Parameters.AddWithValue("@NoOfBooks", obj.NoOfBooks);

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
        public List<MembershipModel> GetAllMembership()
        {
            Connection();
            List<MembershipModel> MembershipList = new List<MembershipModel>();
            SqlCommand com = new SqlCommand("GetMembership", _connect);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();

           
            MembershipList = (from DataRow dr in dt.Rows

                select new MembershipModel()
                {
                    MembershipId = Convert.ToInt32(dr["MembershipId"]),
                    MembershipCategory = Convert.ToString(dr["MembershipCategory"]),
                    NoOfBooks = Convert.ToInt32(dr["NoOfBooks"]),
                   
                }).ToList();


            return MembershipList;


        }

        public MembershipModel GetMembershipByID(int id)
        {
            Connection();
            List<MembershipModel> MembershipList = new List<MembershipModel>();
            MembershipModel membershipModel = new MembershipModel();
            SqlCommand com = new SqlCommand("GetMembershipById", _connect);
            com.Parameters.AddWithValue("@MembershipID", id);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();


            MembershipList = (from DataRow dr in dt.Rows

                select new MembershipModel()
                {
                    MembershipId = Convert.ToInt32(dr["MembershipId"]),
                    MembershipCategory = Convert.ToString(dr["MembershipCategory"]),
                    NoOfBooks = Convert.ToInt32(dr["NoOfBooks"]),

                }).ToList();

            membershipModel = MembershipList[0];

            return membershipModel;
        }

        public bool UpdateMembership(MembershipModel obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateMembership", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@MembershipID", obj.MembershipId);
            com.Parameters.AddWithValue("@MembershipCategory", obj.MembershipCategory);
            com.Parameters.AddWithValue("@NoOfBooks", obj.NoOfBooks);
        
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
        public bool DeleteMembership(int MembershipId)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteMembershipById", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@MembershipID",MembershipId);
           
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

