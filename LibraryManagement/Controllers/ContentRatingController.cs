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
    public class ContentRatingController : Controller
    {
        //GET: All Book Details
        public ActionResult GetAllContentRatingDetails()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            ContentRatingRepository ContentRatingRepo = new ContentRatingRepository();
            ModelState.Clear();
            return View(ContentRatingRepo.GetAllContentRating());
        }

        [HttpGet]
        public ActionResult AddContentRating()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        //POST: Add Book Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddContentRating(ContentRatingModel contentRating)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContentRatingRepository ContentRatingRepo = new ContentRatingRepository();
                    ContentRatingRepo.AddContentRating(contentRating);

                    var profileData = Session["UserProfile"] as UserSession;
                    var logModel = new LogModel
                    {
                        UserId = profileData.UserID,
                        TableName = "Content Rating",
                        Activity = "Added Content Rating",
                        LogDate = DateTime.Now
                    };
                    var logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                }

                return RedirectToAction("GetAllContentRatingDetails");
            }
            catch
            {
                return RedirectToAction("GetAllContentRatingDetails");
            }
        }

        // GET: Update Book Details
        public ActionResult EditContentRatingDetails(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            ContentRatingRepository ContentRatingRepo = new ContentRatingRepository();
            return View(ContentRatingRepo.GetAllContentRating().Find(ContentRatings => ContentRatings.ContentRatingId == id));
        }

        //// POST: Update Book Details    
        [HttpPost]
        public ActionResult EditContentRatingDetails(int id, ContentRatingModel contentRating)
        {
            try
            {
                ContentRatingRepository ContentRatingRepo = new ContentRatingRepository();
                ContentRatingRepo.UpdateContentRating(contentRating);
                var profileData = Session["UserProfile"] as UserSession;
                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "Content Rating",
                    Activity = "Updated Rating",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);
                return RedirectToAction("GetAllContentRatingDetails");
            }
            catch
            {
                return RedirectToAction("GetAllContentRatingDetails");
            }
        }

        //// GET: Delete Book Details    
        //[HttpPost]
        public ActionResult DeleteContentRating(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            try
            {
                ContentRatingRepository ContentRatingRepo = new ContentRatingRepository();
                if (ContentRatingRepo.DeleteContentRating(id))
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
                return RedirectToAction("GetAllContentRatingDetails");

            }
            catch
            {
                return RedirectToAction("GetAllContentRatingDetails");
            }
        }

        public ActionResult AddContentRatingFromBooks(BookDetails bookDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ContentRatingRepository ContentRatingRepo = new ContentRatingRepository();
                    ContentRatingRepo.AddContentRating(bookDetails.TempContentRating);
                }

                var profileData = Session["UserProfile"] as UserSession;
                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "Content Rating",
                    Activity = "Added Content Rating",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);

                return RedirectToAction("AddBook","Book");
            }
            catch
            {
                return RedirectToAction("GetAllContentRatingDetails");
            }
        }
    }
}
