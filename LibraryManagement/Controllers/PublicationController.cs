using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Repository;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Controllers
{
    public class PublicationController : Controller
    {
        public ActionResult GetAllPublicationDetails()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            PublicationRepository pubRepo = new PublicationRepository();
            ModelState.Clear();
            return View(pubRepo.GetAllPublication());
        }

        public ActionResult AddPublication()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult AddPublication(PublicationModel publication)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PublicationRepository publicationRepository = new PublicationRepository();
                    publicationRepository.AddPublication(publication);

                    var profileData = Session["UserProfile"] as UserSession;
                    var logModel = new LogModel
                    {
                        UserId = profileData.UserID,
                        TableName = "Publication",
                        Activity = "Added Publication",
                        LogDate = DateTime.Now
                    };
                    var logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                }

                return RedirectToAction("GetAllPublicationDetails");
            }
            catch
            {
                return RedirectToAction("GetAllPublicationDetails");
            }
        }

        public ActionResult EditPublicationDetails(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            PublicationRepository pubRepo = new PublicationRepository();
            return View(pubRepo.GetAllPublication().Find(pub => pub.PublicationId == id));
        }

        [HttpPost]
        public ActionResult EditPublicationDetails(int id, PublicationModel obj)
        {
            try
            {
                PublicationRepository publicationRepository = new PublicationRepository();
                publicationRepository.UpdatePublication(obj);
                var profileData = Session["UserProfile"] as UserSession;
                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "Publication",
                    Activity = "Updated Publication",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);

                return RedirectToAction("GetAllPublicationDetails");
            }
            catch
            {
                return RedirectToAction("GetAllPublicationDetails");
            }
        }

        public ActionResult DeletePublication(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            try
            {
                PublicationRepository PubRepo = new PublicationRepository();
                if (PubRepo.DeletePublication(id))
                {
                    ViewBag.AlertMsg = "Publication details deleted successfully";

                }

                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "Publication User",
                    Activity = "Deleted Publication",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);

                return RedirectToAction("GetAllPublicationDetails");

            }
            catch
            {
                return RedirectToAction("GetAllPublicationDetails");
            }
        }

        public ActionResult AddPublicationFromBooks(BookDetails bookDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PublicationRepository publicationRepository = new PublicationRepository();
                    publicationRepository.AddPublication(bookDetails.TempPublication);

                    var profileData = Session["UserProfile"] as UserSession;
                    var logModel = new LogModel
                    {
                        UserId = profileData.UserID,
                        TableName = "Publication",
                        Activity = "Added Publication",
                        LogDate = DateTime.Now
                    };
                    var logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                }

                return RedirectToAction("AddBook", "Book");
            }
            catch
            {
                return RedirectToAction("GetAllPublicationDetails");
            }
        }

        public ActionResult SearchPublication(SearchandUser searchedPublication)
        {
            PublicationRepository publicationRepository = new PublicationRepository();
            List<SearchandUser> publicationList = new List<SearchandUser>();
            publicationList =publicationRepository.searchPublisherByName(searchedPublication.Publication.PublicationName);
            return View(publicationList);
        }
    }
}