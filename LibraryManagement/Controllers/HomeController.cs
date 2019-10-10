using System.Collections.Generic;
using System.Web.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Repository;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData != null && profileData.UserID != 0)
                return RedirectToAction("Dashboard");
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult VerifyUser(SearchandUser checkUser)
        {
            try
            {
                var returnedCheckedList = new List<UserModel>();
                if (ModelState.IsValid)
                {
                    var userRepository = new UserRepository();
                    returnedCheckedList = userRepository.VerifyUser(checkUser.User);
                    if (returnedCheckedList[0].AuthenticationId !=0)
                    {
                        var profileData = new UserSession
                        {
                            UserID = returnedCheckedList[0].UserId,
                            UserName = returnedCheckedList[0].UserName,
                            Designation = returnedCheckedList[0].AuthenticationId
                        };
                        Session["UserProfile"] = profileData;
                        return RedirectToAction("Dashboard");
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Dashboard()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData != null)
            {
                BookRepository bookRepository = new BookRepository();
                List<BookInfo> bookInfo = bookRepository.GetAllBook();
                return View(bookInfo);
            }
            return RedirectToAction("Index");
            
        }

        public ActionResult LogOut()
        {
            Session["UserProfile"] = new UserSession();
            return RedirectToAction("Index");
        }
    }
}