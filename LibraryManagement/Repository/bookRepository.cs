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
    public class BookRepository
    {
        private SqlConnection _connect;
        //To Handle connection related activities
        private void Connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            _connect = new SqlConnection(constr);
        }
        //To Add Book details
        public bool AddBook(BookDetails obj)
        {
            Connection();
            List<BookModel> addedBook = new List<BookModel>();
            BookAuthorRepository bookAuthorRepository = new BookAuthorRepository();
            SqlCommand commandBook = new SqlCommand("AddNewBook", _connect);
            commandBook.CommandType = CommandType.StoredProcedure;
            commandBook.Parameters.AddWithValue("@BookName", obj.BookModel.BookName);
            commandBook.Parameters.AddWithValue("@PublishedDate", obj.BookModel.PublishedDate);
            commandBook.Parameters.AddWithValue("@ContentRatingID", obj.BookModel.ContentRatingId);
            commandBook.Parameters.AddWithValue("@Edition", obj.BookModel.Edition);
            commandBook.Parameters.AddWithValue("@PublicationID", obj.BookModel.PublicationId);
            commandBook.Parameters.AddWithValue("@BookType", obj.BookModel.BookType);
            commandBook.Parameters.AddWithValue("@Genre", obj.BookModel.Genre);

            _connect.Open();
            int i = commandBook.ExecuteNonQuery();
            _connect.Close();

            if (i >= 1)
            {
                DateTime publishedDate = Convert.ToDateTime(obj.BookModel.PublishedDate);
                var searchedBook=SearchedBook(obj.BookModel.BookName, publishedDate);
                var bookAuthorModel1=new BookAuthorModel()
                {
                    BookId = searchedBook.BookId,
                    AuthorId = obj.Author1
                };
                bookAuthorRepository.AddBookAuthor(bookAuthorModel1);

                if(obj.Author2!=0)
                {
                    var bookAuthorModel2 = new BookAuthorModel()
                    {
                        BookId = searchedBook.BookId,
                        AuthorId = obj.Author2
                    };
                    bookAuthorRepository.AddBookAuthor(bookAuthorModel2);
                }

                if (obj.Author3 != 0)
                {
                    var bookAuthorModel3 = new BookAuthorModel()
                    {
                        BookId = searchedBook.BookId,
                        AuthorId = obj.Author3
                    };
                    bookAuthorRepository.AddBookAuthor(bookAuthorModel3);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public BookModel SearchBookById(int id)
        {
            Connection();
            BookModel searchedBook=new BookModel();
            List<BookModel> bookList= new List<BookModel>();
            SqlCommand command = new SqlCommand("SearchBookById", _connect);
            command.Parameters.AddWithValue("@BookID", id);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();
            
            bookList = (from DataRow dataRow in dataTable.Rows

                select new BookModel()
                {
                    BookId = Convert.ToInt32(dataRow["BookID"]),
                    BookName = Convert.ToString(dataRow["BookName"]),
                    PublishedDate = Convert.ToString(dataRow["PublishedDate"]),
                    ContentRatingId = Convert.ToInt32(dataRow["ContentRatingID"]),
                    Edition = Convert.ToString(dataRow["Edition"]),
                    PublicationId = Convert.ToInt32(dataRow["PublicationID"]),
                    BookType = Convert.ToString(dataRow["BookType"]),
                    Genre = Convert.ToString(dataRow["Genre"])
                }).ToList();

            searchedBook = bookList[0];
            return searchedBook;
        }

        public BookModel SearchedBook(string bookName, DateTime publicationDate)
        {
            Connection();
            List<BookModel>BookList = new List<BookModel>();
            BookModel bookModel=new BookModel();
            SqlCommand command = new SqlCommand("SearchBookandPublishedDate", _connect);
            command.Parameters.AddWithValue("@BookName", bookName);
            command.Parameters.AddWithValue("@PublishedDate", publicationDate);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind BookModel generic list using LINQ 
            BookList = (from DataRow dr in dataTable.Rows

                select new BookModel()
                {
                    BookId = Convert.ToInt32(dr["BookID"]),
                    BookName = Convert.ToString(dr["BookName"]),
                    PublishedDate = Convert.ToString(dr["PublishedDate"]),
                    ContentRatingId = Convert.ToInt32(dr["ContentRatingID"]),
                    Edition = Convert.ToString(dr["Edition"]),
                    PublicationId = Convert.ToInt32(dr["PublicationID"]),
                    BookType = Convert.ToString(dr["BookType"]),
                    Genre = Convert.ToString(dr["Genre"])
                }).ToList();

            bookModel = BookList[0];
            return bookModel;
        }


        //To view Book details with generic list 
        public List<BookInfo> GetAllBook()
        {
            Connection();
            List<BookInfo> BookList = new List<BookInfo>();
            SqlCommand command = new SqlCommand("GetBookInfo", _connect);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind BookModel generic list using LINQ 
            BookList = (from DataRow dataRow in dataTable.Rows

                select new BookInfo()
                {
                    BookID = Convert.ToInt32(dataRow["BookID"]),
                    BookName = Convert.ToString(dataRow["BookName"]),
                    PublicationName = Convert.ToString(dataRow["PublicationName"]),
                    AuthorName = Convert.ToString(dataRow["AuthorName"])
                }).ToList();
            return BookList;
        }

        public List<SearchandUser> searchBookByName(String name)
        {
            Connection();
            List<SearchandUser> BookList = new List<SearchandUser>();
            SqlCommand command = new SqlCommand("SearchBookByName", _connect);
            command.Parameters.AddWithValue("@BookName", name);
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


        //To Update Book details
        public bool UpdateBook(BookModel obj)
        {

            Connection();
            SqlCommand com = new SqlCommand("UpdateBook", _connect);

            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@BookID", obj.BookId);
            com.Parameters.AddWithValue("@BookName", obj.BookName);
            com.Parameters.AddWithValue("@Year", obj.PublishedDate);
            com.Parameters.AddWithValue("@ContentRatingID", obj.ContentRatingId);
            com.Parameters.AddWithValue("@Edition", obj.Edition);
            com.Parameters.AddWithValue("@PublicationID", obj.PublicationId);
            com.Parameters.AddWithValue("@BookType", obj.BookType);

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
        public bool DeleteBooks(int id)
        {
            Connection();
            SqlCommand com = new SqlCommand("DeleteBook", _connect);

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

        public List<BookModel> booksNotLoaned()
        {
            Connection();
            DateTime checkDate = DateTime.Now.AddDays(-31);
            List<BookModel> BookList = new List<BookModel>();
            SqlCommand command = new SqlCommand("getbooksNotLoaned", _connect);
            command.Parameters.AddWithValue("@CheckDate", checkDate);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            _connect.Open();
            dataAdapter.Fill(dataTable);
            _connect.Close();

            //Bind BookModel generic list using LINQ 
            BookList = (from DataRow dr in dataTable.Rows

                select new BookModel()
                {
                    BookId = Convert.ToInt32(dr["BookID"]),
                    BookName = Convert.ToString(dr["BookName"]),
                    PublishedDate = Convert.ToString(dr["PublishedDate"]),
                    ContentRatingId = Convert.ToInt32(dr["ContentRatingID"]),
                    Edition = Convert.ToString(dr["Edition"]),
                    PublicationId = Convert.ToInt32(dr["PublicationID"]),
                    BookType = Convert.ToString(dr["BookType"]),
                    Genre = Convert.ToString(dr["Genre"])
                }).ToList();

            return BookList;
        }

    }
}