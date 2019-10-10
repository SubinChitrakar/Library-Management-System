using System;
using System.Web.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Repository;

namespace LibraryManagement.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult GetAllUserDetails()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            var userRepository = new UserRepository();
            ModelState.Clear();
            return View(userRepository.GetAllUsersWithAuthentication());
        }

        public ActionResult AddUser()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            var userRepository = new UserRepository();
            var userModel = new UserModel();
            ModelState.Clear();
            userModel = userRepository.GetAllAuthentication();

            return View(userModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(UserModel User)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserRepository userRepository = new UserRepository();
                    userRepository.AddUser(User);

                    var profileData = Session["UserProfile"] as UserSession;
                    var logModel = new LogModel
                    {
                        UserId = profileData.UserID,
                        TableName = "User",
                        Activity = "Added User",
                        LogDate = DateTime.Now
                    };
                    var logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                }
                return RedirectToAction("GetAllUserDetails");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult EditUserDetails(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            var userRepository = new UserRepository();

            return View(userRepository.GetAllUsers().Find(User => User.UserId == id));
        }

        [HttpPost]
        public ActionResult EditUserDetails(int id, UserModel User)
        {
            try
            {
                var userRepository = new UserRepository();
                userRepository.UpdateUser(User);
                var profileData = Session["UserProfile"] as UserSession;
                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "User",
                    Activity = "Updated User",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);

                return RedirectToAction("GetAllUserDetails");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult DeleteUser(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            try
            {
                var userRepository = new UserRepository();
                if (userRepository.DeleteUser(id))
                    ViewBag.AlertMsg = "Employee details deleted successfully";

                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "User",
                    Activity = "Deleted User",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);
                return RedirectToAction("GetAllUserDetails");
            }
            catch
            {
                return RedirectToAction("GetAllUserDetails");
            }
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(UserModel obj)
        {
            var profileData = Session["UserProfile"] as UserSession;
            var userRepository = new UserRepository();
            obj.UserId = profileData.UserID;
            userRepository.ChangePassword(obj);
            return RedirectToAction("Dashboard","Home");
        }
    }
}