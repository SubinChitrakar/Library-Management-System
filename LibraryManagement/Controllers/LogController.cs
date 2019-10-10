using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Repository;

namespace LibraryManagement.Controllers
{
    public class LogController : Controller
    {
        public ActionResult GetAllLog()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            logRepository logRepository = new logRepository();
            ModelState.Clear();
            return View(logRepository.GetAllLog());
        }
    }
}
