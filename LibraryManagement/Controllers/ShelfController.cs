using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using libraryManagement.Repository;
using LibraryManagement.Models;
using LibraryManagement.Repository;

namespace LibraryManagement.Controllers
{
    public class ShelfController : Controller
    {
        //GET: All Book Details
        public ActionResult GetAllShelfDetails()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            ShelfRepository ShelfRepo = new ShelfRepository();
            ModelState.Clear();
            return View(ShelfRepo.GetAllShelf());
        }

        [HttpGet]
        public ActionResult AddShelf()
        {
            var profileData = this.Session["UserProfile"] as UserSession;
            if (profileData == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        //POST: Add Book Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddShelf(ShelfModel shelf)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ShelfRepository ShelfRepo = new ShelfRepository();
                    ShelfRepo.AddShelf(shelf);

                    var profileData = this.Session["UserProfile"] as UserSession;
                    LogModel logModel = new LogModel()
                    {
                        UserId = profileData.UserID,
                        TableName = "Shelf",
                        Activity = "Added Shelf",
                        LogDate = DateTime.Now
                    };
                    logRepository logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                }

                return RedirectToAction("GetAllShelfDetails");
            }
            catch
            {
                return RedirectToAction("GetAllShelfDetails");
            }
        }

        // GET: Update Book Details
        public ActionResult EditShelfDetails(int id)
        {
            var profileData = this.Session["UserProfile"] as UserSession;
            if (profileData == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ShelfRepository ShelfRepo = new ShelfRepository();
            return View(ShelfRepo.GetAllShelf().Find(Shelf => Shelf.ShelfId == id));
        }

        //// POST: Update Book Details    
        [HttpPost]
        public ActionResult EditShelfDetails(int id, ShelfModel shelf)
        {
            try
            {
                ShelfRepository ShelfRepo = new ShelfRepository();
                ShelfRepo.UpdateShelf(shelf);
                var profileData = this.Session["UserProfile"] as UserSession;
                LogModel logModel = new LogModel()
                {
                    UserId = profileData.UserID,
                    TableName = "Shelf",
                    Activity = "Updated Shelf",
                    LogDate = DateTime.Now
                };
                logRepository logRepository = new logRepository();
                logRepository.AddLog(logModel);
                return RedirectToAction("GetAllShelfDetails");
            }
            catch
            {
                return RedirectToAction("GetAllShelfDetails");
            }
        }

        //// GET: Delete Book Details
        public ActionResult DeleteShelf(int id)
        {
            var profileData = this.Session["UserProfile"] as UserSession;
            if (profileData == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                ShelfRepository ShelfRepo = new ShelfRepository();
                if (ShelfRepo.DeleteShelf(id))
                {
                    ViewBag.AlertMsg = "Shelf details deleted successfully";

                }
                LogModel logModel = new LogModel()
                {
                    UserId = profileData.UserID,
                    TableName = "Shelf",
                    Activity = "Deleted Shelf",
                    LogDate = DateTime.Now
                };
                logRepository logRepository = new logRepository();
                logRepository.AddLog(logModel);
                return RedirectToAction("GetAllShelfDetails");

            }
            catch
            {
                return RedirectToAction("GetAllShelfDetails");
            }
        }
    }
}
