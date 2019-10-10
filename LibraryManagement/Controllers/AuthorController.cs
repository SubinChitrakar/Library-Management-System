using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Repository;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Controllers
{
    public class AuthorController : Controller
    {
        //GET: All Book Details
        public ActionResult GetAllAuthorDetails()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            AuthorRepository AuthorRepo = new AuthorRepository();
            ModelState.Clear();
            return View(AuthorRepo.GetAllAuthor());
        }

        [HttpGet]
        public ActionResult AddAuthor()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        //POST: Add Book Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAuthor(AuthorModel author)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AuthorRepository AuthorRepo = new AuthorRepository();
                    AuthorRepo.AddAuthor(author);

                    var profileData = Session["UserProfile"] as UserSession;
                    var logModel = new LogModel
                    {
                        UserId = profileData.UserID,
                        TableName = "Author",
                        Activity = "Added Author",
                        LogDate = DateTime.Now
                    };
                    var logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                }
                return RedirectToAction("GetAllAuthorDetails");
            }
            catch
            {
                return RedirectToAction("GetAllAuthorDetails");
            }
        }

        // GET: Update Book Details
        public ActionResult EditAuthorDetails(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            AuthorRepository AuthorRepo = new AuthorRepository();
            return View(AuthorRepo.GetAllAuthor().Find(Author => Author.AuthorId == id));
        }

        //// POST: Update Book Details    
        [HttpPost]
        public ActionResult EditAuthorDetails(int id, AuthorModel author)
        {
            try
            {
                AuthorRepository AuthorRepo = new AuthorRepository();
                AuthorRepo.UpdateAuthor(author);
                var profileData = Session["UserProfile"] as UserSession;
                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "Author",
                    Activity = "Updated Author",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);

                return RedirectToAction("GetAllAuthorDetails");
            }
            catch
            {
                return RedirectToAction("GetAllAuthorDetails");
            }
        }

        //// GET: Delete Book Details    
        //[HttpPost]
        public ActionResult DeleteAuthor(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            try
            {
                AuthorRepository AuthorRepo = new AuthorRepository();
                if (AuthorRepo.DeleteAuthor(id))
                {
                    ViewBag.AlertMsg = "Book details deleted successfully";

                }

                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "Author",
                    Activity = "Deleted Author",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);
                return RedirectToAction("GetAllAuthorDetails");

            }
            catch
            {
                return RedirectToAction("GetAllAuthorDetails");
            }
        }

        public ActionResult AddAuthorFromBooks(BookDetails bookDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AuthorRepository AuthorRepo = new AuthorRepository();
                    AuthorRepo.AddAuthor(bookDetails.TempAuthor);

                    var profileData = Session["UserProfile"] as UserSession;
                    var logModel = new LogModel
                    {
                        UserId = profileData.UserID,
                        TableName = "Author",
                        Activity = "Added Author",
                        LogDate = DateTime.Now
                    };
                    var logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                }

                return RedirectToAction("AddBook","Book");
            }
            catch
            {
                return RedirectToAction("GetAllAuthorDetails");
            }
        }

        public ActionResult SearchAuthor(SearchandUser searchedAuthor)
        {
            AuthorRepository authorRepository=new AuthorRepository();
            List<SearchandUser> authorList = new List<SearchandUser>();
            authorList = authorRepository.searchAuthorByName(searchedAuthor.Author.AuthorName);
            return View(authorList);
        }

    }
}
