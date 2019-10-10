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
    public class LoanRepository
    {
        private SqlConnection _connect;

        private void Connection()
        {
            var configuration = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(configuration);
        }

        public bool AddLoan(LoanModel obj)
        {
            Connection();
            SqlCommand command = new SqlCommand("AddNewLoan", _connect);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@LoanDate", obj.LoanDate);
            command.Parameters.AddWithValue("@ReturnDate", obj.ReturnDate);
            command.Parameters.AddWithValue("@CopiesID", obj.CopiesId);
            command.Parameters.AddWithValue("@MemberID", obj.MembershipId);
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
        public List<LoanModel> GetAllLoan()
        {
            Connection();
            List<LoanModel> LoanList = new List<LoanModel>();
            SqlCommand com = new SqlCommand("GetLoan", _connect);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();

            LoanList = (from DataRow dr in dt.Rows
                        select new LoanModel()
                        {
                            MembershipId = Convert.ToInt32(dr["MemberID"]),
                            CopiesId = Convert.ToInt32(dr["CopiesID"]),
                            LoanDate = Convert.ToString(dr["LoanDate"]),
                            ReturnDate = Convert.ToString(dr["LoanDate"]),
                            LoanId = Convert.ToInt32(dr["LoanID"]),


                        }).ToList();
            return LoanList;
        }

        public List<BookLoaned> GetAllLoanDetails()
        {
            Connection();
            List<BookLoaned> LoanList = new List<BookLoaned>();
            SqlCommand com = new SqlCommand("GetLoanDetails", _connect);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();

            LoanList = (from DataRow dr in dt.Rows
                select new BookLoaned()
                {
                    LoanID = Convert.ToInt32(dr["LoanID"]),
                    LoanDate = Convert.ToString(dr["LoanDate"]),
                    ReturnDate = Convert.ToString(dr["ReturnDate"]),
                    MemberName = Convert.ToString(dr["MemberName"]),
                    BookName = Convert.ToString(dr["BookName"])
                }).ToList();
            return LoanList;
        }

        public BookCopies GetAllLoanByMemberID(int id)
        {
            Connection();
            List<BookCopies> bookCopiesList = new List<BookCopies>();
            BookCopies bookCopies = new BookCopies();
            SqlCommand com = new SqlCommand("CountLoan", _connect);
            com.Parameters.AddWithValue("@MemberID", id);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();

            bookCopiesList = (from DataRow dr in dt.Rows
                select new BookCopies()
                {
                    Quantities = Convert.ToInt32(dr["NoOfBooks"])
                }).ToList();

            bookCopies = bookCopiesList[0];

            return bookCopies;
        }

        public LoanModel GetAllLoanByID(int id)
        {
            Connection();
            List<LoanModel> loanList = new List<LoanModel>();
            LoanModel loanModel = new LoanModel();
            SqlCommand com = new SqlCommand("GetLoanByID", _connect);
            com.Parameters.AddWithValue("@loanID", id);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();

            loanList = (from DataRow dr in dt.Rows
                select new LoanModel()
                {
                    LoanId = Convert.ToInt32(dr["LoanID"]),
                    LoanDate = Convert.ToString(dr["LoanDate"]),
                    ReturnDate = Convert.ToString(dr["ReturnDate"]),
                    MembershipId = Convert.ToInt32(dr["MemberID"]),
                    CopiesId = Convert.ToInt32(dr["CopiesID"])

                }).ToList();

            loanModel = loanList[0];

            return loanModel;
        }

        public List<BookLoaned> SearchByMember(String name)
        {
            Connection();
            DateTime currentDate = DateTime.Now.AddDays(-31);
            List<BookLoaned> loanList = new List<BookLoaned>();
            SqlCommand com = new SqlCommand("GetLoanByMember", _connect);
            com.Parameters.AddWithValue("@MemberName", name);
            com.Parameters.AddWithValue("@CheckedTime", currentDate);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            _connect.Open();
            da.Fill(dt);
            _connect.Close();

            loanList = (from DataRow dr in dt.Rows
                select new BookLoaned()
                {
                    LoanDate = Convert.ToString(dr["LoanDate"]),
                    ReturnDate = Convert.ToString(dr["ReturnDate"]),
                    MemberName = Convert.ToString(dr["MemberName"]),
                    BookName = Convert.ToString(dr["BookName"])
                }).ToList();

            return loanList;
        }


        public bool UpdateLoan(int id)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateLoan", _connect);

            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@LoanID", id);

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
        public bool DeleteLoan(int id)
        {

            Connection();
            SqlCommand com = new SqlCommand("DeleteLoanById", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@LoanID", id);

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
