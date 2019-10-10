using System;
using System.Collections.Generic;
using System.Web.Mvc;
using libraryManagement.Repository;
using LibraryManagement.Models;
using LibraryManagement.Repository;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Controllers
{
    public class CopiesController : Controller
    {
        public ActionResult GetAllCopies()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            copiesRepository copiesRepository=new copiesRepository();
            List<CopyDetail> copyDetailList=new List<CopyDetail>();
            copyDetailList = copiesRepository.GetAllCopyDetails();

            return View(copyDetailList);
        }

        public ActionResult GetCopiesofBook(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            copiesRepository copiesRepository = new copiesRepository();
            List<CopyDetail> copyDetailList = new List<CopyDetail>();
            copyDetailList = copiesRepository.GetAllCopiesOfABook(id);

            return View(copyDetailList);
        }

        [HttpGet]
        public ActionResult AddCopies(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            BookRepository bookRepository=new BookRepository();
            ShelfRepository shelfRepository=new ShelfRepository();
            BookCopies bookCopies=new BookCopies();
            
            BookModel bookModel = bookRepository.SearchBookById(id);
            bookCopies.BookModel = bookModel;
            List<ShelfModel> shelfList = shelfRepository.GetAllShelf();
            bookCopies.ShelfList = shelfList;

            return View(bookCopies);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCopies(BookCopies bookCopies)
        {
            try
            {
                copiesRepository copiesRepository = new copiesRepository();
                for (int i = 0; i < bookCopies.Quantities; i++)
                {
                    CopiesModel copiesModel=new CopiesModel()
                    {
                        BookId = bookCopies.BookModel.BookId,
                        CopyNo = i+1,
                        DateBought = bookCopies.CopiesModel.DateBought,
                        Location= bookCopies.CopiesModel.Location
                    };
                    copiesRepository.AddCopies(copiesModel);

                    var profileData = Session["UserProfile"] as UserSession;
                    var logModel = new LogModel
                    {
                        UserId = profileData.UserID,
                        TableName = "Copies",
                        Activity = "Copies Added",
                        LogDate = DateTime.Now
                    };
                    var logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                }
               return RedirectToAction("Dashboard","Home");
            }
            catch
            {
                return RedirectToAction("Dashboard","Home");
            }
        }

        public ActionResult CheckOldBooks()
        {
            copiesRepository copiesRepository=new copiesRepository();
            return View(copiesRepository.checkOldBooks());
        }

        public ActionResult DeleteCopy(int id)
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            try
            {
                copiesRepository copiesRepository = new copiesRepository();
                if (copiesRepository.DeleteCopies(id))
                {
                    ViewBag.AlertMsg = "Copies deleted successfully";

                }

                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "Copies",
                    Activity = "Deleted Copies",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);
                return RedirectToAction("Dashboard","Home");

            }
            catch
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }
    }
}
