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
    public class MemberRepository
    {
        private SqlConnection _connect;

        private void Connection()
        {
            var configuration = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(configuration);
        }

        public bool AddNewMember(MemberModel obj)
        {
            Connection();
            SqlCommand command = new SqlCommand("AddNewMember", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MemberName", obj.MemberName);
            command.Parameters.AddWithValue("@DOB", Convert.ToDateTime(obj.Dob));
            command.Parameters.AddWithValue("@PhoneNo", obj.PhoneNumber);
            command.Parameters.AddWithValue("@Email", obj.Email);
            command.Parameters.AddWithValue("@MembershipID", obj.MembershipId);

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
        public List<MemberModel> GetAllMember()
        {
            Connection();
            List<MemberModel> MemberList = new List<MemberModel>();
            SqlCommand com = new SqlCommand("GetMember", _connect);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(com);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();


            MemberList = (from DataRow dr in dataTable.Rows

                select new MemberModel()
                {
                    MemberId = Convert.ToInt32(dr["MemberID"]),
                    MemberName = Convert.ToString(dr["MemberName"]),
                    Dob = Convert.ToString(dr["DOB"]),
                    PhoneNumber = Convert.ToString(dr["PhoneNo"]),
                    Email = Convert.ToString(dr["Email"]),
                    MembershipId = Convert.ToInt32(dr["MembershipID"]),

                }).ToList(); 
            return MemberList;
        }

        public MemberModel GetAllMemberShip()
        {
            Connection();
            SqlCommand command = new SqlCommand("GetMembership", _connect);
            MemberModel member=new MemberModel();
            List<MembershipModel> membershipList = new List<MembershipModel>();
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            membershipList = (from DataRow dataRow in dataTable.Rows

                select new MembershipModel()
                {
                    MembershipId = Convert.ToInt32(dataRow["MembershipID"]),
                    MembershipCategory = Convert.ToString(dataRow["MembershipCategory"]),
                }).ToList();

            member.MembershipList = membershipList;

            return member;
        }

        public List<MemberWithMembership> GetAllMemberWithMemberships()
        {
            Connection();
            SqlCommand command = new SqlCommand("GetMemberWithMembership", _connect);
            List<MemberWithMembership> memberWithMembershipList = new List<MemberWithMembership>();
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();

            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            memberWithMembershipList = (from DataRow dataRow in dataTable.Rows

                select new MemberWithMembership()
                {
                    MemberID = Convert.ToInt32(dataRow["MemberID"]),
                    MemberName = Convert.ToString(dataRow["MemberName"]),
                    DOB = Convert.ToString(dataRow["DOB"]),
                    PhoneNo = Convert.ToString(dataRow["PhoneNo"]),
                    Email = Convert.ToString(dataRow["Email"]),
                    MembershipType = Convert.ToString(dataRow["MembershipCategory"]),
                    NoOfBooks = Convert.ToInt32(dataRow["NoOfBooks"])
                }).ToList();
            return memberWithMembershipList;
        }

        public MemberModel SearchMemberById(int id)
        {
            Connection();
            MemberModel memberModel = new MemberModel();
            List<MemberModel> memberList = new List<MemberModel>();
            SqlCommand command = new SqlCommand("SearchMemberById", _connect);
            command.Parameters.AddWithValue("@MemberID", id);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            memberList = (from DataRow dataRow in dataTable.Rows

                select new MemberModel()
                {
                    MemberId = Convert.ToInt32(dataRow["MemberID"]),
                    MemberName = Convert.ToString(dataRow["MemberName"]),
                    Dob = Convert.ToString(dataRow["DOB"]),
                    PhoneNumber = Convert.ToString(dataRow["PhoneNo"]),
                    Email = Convert.ToString(dataRow["Email"]),
                    MembershipId = Convert.ToInt32(dataRow["MembershipID"]),
                }).ToList();

            memberModel = memberList[0];
            return memberModel;
        }



        public bool UpdateMember(MemberModel obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateMember", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@MemberName", obj.MemberName);
            com.Parameters.AddWithValue("@DOB", Convert.ToDateTime(obj.Dob));
            com.Parameters.AddWithValue("@PhoneNo", obj.PhoneNumber);
            com.Parameters.AddWithValue("@Email", obj.Email);
            com.Parameters.AddWithValue("@MembershipId", obj.MembershipId);
            com.Parameters.AddWithValue("@MemberID", obj.MemberId);

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
        public bool DeleteMember(int MemberId)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteMemberById", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@MemberID", MemberId);

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

        public List<MemberModel> getInactiveUser()
        {
            Connection();
            DateTime inActive;
            inActive=DateTime.Now.AddDays(-31);
            List<MemberModel> memberList=new List<MemberModel>();
            SqlCommand command = new SqlCommand("getInactiveUser", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LastLoan", inActive);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            memberList = (from DataRow dataRow in dataTable.Rows

                select new MemberModel()
                {
                    MemberId = Convert.ToInt32(dataRow["MemberID"]),
                    MemberName = Convert.ToString(dataRow["MemberName"]),
                    Dob = Convert.ToString(dataRow["DOB"]),
                    PhoneNumber = Convert.ToString(dataRow["PhoneNo"]),
                    Email = Convert.ToString(dataRow["Email"]),
                    MembershipId = Convert.ToInt32(dataRow["MembershipID"]),
                }).ToList();

            return memberList;
        }
    }
}