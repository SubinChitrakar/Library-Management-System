using LibraryManagement.Models;
using LibraryManagement.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagement.ViewModels;
using Microsoft.Owin.Security.Provider;

namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        public ActionResult GetBook(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            BookRepository bookRepository=new BookRepository();
            BookAuthorRepository bookAuthorRepository=new BookAuthorRepository();
            AuthorRepository authorRepository=new AuthorRepository();
            PublicationRepository publicationRepository=new PublicationRepository();
            ContentRatingRepository contentRatingRepository=new ContentRatingRepository();
            
            List<BookAuthorModel> bookAuthorList=new List<BookAuthorModel>();
            BookDetails bookDetail = new BookDetails();
            BookModel book=new BookModel();

            book = bookRepository.SearchBookById(id);
            bookDetail.BookModel = book;

            bookAuthorList = bookAuthorRepository.GetByBookId(book.BookId);

            if (bookAuthorList.Count==1)
            {
                bookDetail.AuthorInfo1 = authorRepository.GetByAuthorId(bookAuthorList[0].AuthorId);
            }
            
            else if (bookAuthorList.Count == 2)
            {
                bookDetail.AuthorInfo1 = authorRepository.GetByAuthorId(bookAuthorList[0].AuthorId);
                bookDetail.AuthorInfo2 = authorRepository.GetByAuthorId(bookAuthorList[1].AuthorId);
            }

            else
            {
                bookDetail.AuthorInfo1 = authorRepository.GetByAuthorId(bookAuthorList[0].AuthorId);
                bookDetail.AuthorInfo2 = authorRepository.GetByAuthorId(bookAuthorList[1].AuthorId);
                bookDetail.AuthorInfo3 = authorRepository.GetByAuthorId(bookAuthorList[2].AuthorId);
            }

            bookDetail.PublicationModel = publicationRepository.GetByPublicationId(book.PublicationId);

            bookDetail.ContentRatingModel = contentRatingRepository.GetByContentId(book.ContentRatingId);

            return View(bookDetail);
        }

        [HttpGet]
        public ActionResult AddBook()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            var publicationRepository = new PublicationRepository();
            var contentRatingRepository=new ContentRatingRepository();
            var authorRepository=new AuthorRepository();
            var bookDetail = new BookDetails();
            var bookModel = new BookModel();

            ModelState.Clear();

            bookModel.PublicationList = publicationRepository.GetAllPublication();
            bookModel.ContentRatingList = contentRatingRepository.GetAllContentRating();
            bookModel.BookTypeList=new List<String>()
            {
                "Reference Book",
                "Loan Book"
            };
            
            bookDetail.BookModel = bookModel;
            bookDetail.AuthorList = authorRepository.GetAllAuthor();

            return View(bookDetail);
        }

        //POST: Add Book Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBook(BookDetails book)
        {
            try
            {
                BookRepository bookRepository = new BookRepository();
                bookRepository.AddBook(book);

                var profileData = Session["UserProfile"] as UserSession;
                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "Book",
                    Activity = "Added Book",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);

                return RedirectToAction("Dashboard", "Home");
            }
            catch
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }


        //// GET: Delete Book Details    
        //[HttpPost]
        public ActionResult DeleteBooks(int id)
        {
            try
            {
                var profileData = Session["UserProfile"] as UserSession;
                if (profileData == null)
                    return RedirectToAction("Index", "Home");

                BookRepository bookRepository = new BookRepository();
                BookAuthorRepository bookAuthorRepository=new BookAuthorRepository();

                bookAuthorRepository.DeleteBookAuthorByBookId(id);
                bookRepository.DeleteBooks(id);

                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "Book",
                    Activity = "Deleted Book",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);

                return RedirectToAction("Dashboard","Home");

            }
            catch
            {
                return RedirectToAction("Dashboard","Home");
            }
        }

        public ActionResult SearchBook(SearchandUser searchedBook)
        {
            BookRepository bookRepository=new BookRepository();
            List<SearchandUser> publicationList=new List<SearchandUser>();
            publicationList = bookRepository.searchBookByName(searchedBook.Book.BookName);
            return View(publicationList);
        }

        public ActionResult BooksNotLoaned()
        {
            BookRepository bookRepository = new BookRepository();
            return View(bookRepository.booksNotLoaned());
        }
    }
}
