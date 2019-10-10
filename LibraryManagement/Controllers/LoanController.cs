using System;
using System.Collections.Generic;
using System.Web.Mvc;
using libraryManagement.Repository;
using LibraryManagement.Models;
using LibraryManagement.Repository;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Controllers
{
    public class LoanController : Controller
    {
        public ActionResult GetAllLoan()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            LoanRepository loanRepository=new LoanRepository();
            List<BookLoaned> bookLoanList=new List<BookLoaned>();
            bookLoanList = loanRepository.GetAllLoanDetails();
            return View(bookLoanList);
        }

        [HttpGet]
        public ActionResult AddLoan(int id)
        {
            MemberRepository memberRepository=new MemberRepository();
            LoanModel loanModel=new LoanModel();
            loanModel.CopiesId = id;
            loanModel.MemberList = memberRepository.GetAllMember();
            return View(loanModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLoan(LoanModel loanModel)
        {
            try
            {
                LoanRepository loanRepository = new LoanRepository();
                copiesRepository copiesRepository=new copiesRepository();
                BookRepository bookRepository = new BookRepository();
                ContentRatingRepository contentRatingRepository=new ContentRatingRepository();
                MemberRepository memberRepository=new MemberRepository();
                MembershipRepository membershipRepository=new MembershipRepository();

                List<MemberModel> memberList=new List<MemberModel>();
                LoanModel newLoanModel=new LoanModel();
                newLoanModel.MemberList = memberRepository.GetAllMember();         

                CopiesModel copiesModel=new CopiesModel();
                copiesModel = copiesRepository.SearchCopyById(loanModel.CopiesId);

                BookModel bookModel=new BookModel();
                bookModel = bookRepository.SearchBookById(copiesModel.BookId);

                if (bookModel.BookType.Equals("Reference Book"))
                {
                    ViewBag.Message = "Reference Books Can't be Loaned";
                    return View(newLoanModel);
                }

                ContentRatingModel contentRatingModel=new ContentRatingModel();
                contentRatingModel = contentRatingRepository.GetByContentId(bookModel.ContentRatingId);

                MemberModel memberModel=new MemberModel();
                memberModel = memberRepository.SearchMemberById(loanModel.MembershipId);

                if (contentRatingModel.ContentRatingName.Equals("18+"))
                {
                    var today = DateTime.Today;
                    var age = Convert.ToDateTime(memberModel.Dob);
                    var todayAge = today.Year - age.Year;
                    if (todayAge<18)
                    {
                        ViewBag.Message = "Age Restricted Books Are Not Allowed";
                        return View(newLoanModel);
                    }
                }

                MembershipModel membershipModel=new MembershipModel();
                membershipModel = membershipRepository.GetMembershipByID(memberModel.MembershipId);

                BookCopies bookCopiesModel=new BookCopies();
                bookCopiesModel = loanRepository.GetAllLoanByMemberID(memberModel.MemberId);

                if(bookCopiesModel.Quantities>=membershipModel.NoOfBooks)
                {
                    ViewBag.Message = "No More Books Can be Loaned";
                    return View(newLoanModel);
                }

                DateTime returnedDate = Convert.ToDateTime(loanModel.LoanDate);
                loanModel.ReturnDate = returnedDate.AddDays(14).ToString("yyyy MMMM dd");
                loanRepository.AddLoan(loanModel);
                copiesRepository.UpdateCopiesStatus(loanModel.CopiesId);

                var profileData = Session["UserProfile"] as UserSession;
                var logModel = new LogModel
                {
                    UserId = profileData.UserID,
                    TableName = "Loan",
                    Activity = "Added Loan",
                    LogDate = DateTime.Now
                };
                var logRepository = new logRepository();
                logRepository.AddLog(logModel);
                return RedirectToAction("Dashboard", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ReturnLoan(int id)
        {
            LoanRepository loanRepository=new LoanRepository();
            LoanModel loanModel=new LoanModel();
            loanModel = loanRepository.GetAllLoanByID(id);
            return View(loanModel);
        }

        [HttpPost]
        public ActionResult ReturnLoan(LoanModel loanModel)
        {
            try
            {
                LoanRepository loanRepository = new LoanRepository();
                copiesRepository copiesRepository=new copiesRepository();
                BookRepository bookRepository=new BookRepository();
                MemberRepository memberRepository=new MemberRepository();

                LoanModel newLoanModel = new LoanModel();
                newLoanModel = loanRepository.GetAllLoanByID(loanModel.LoanId);
                MemberModel memberModel = memberRepository.SearchMemberById(newLoanModel.MembershipId);
                CopiesModel copiesModel = copiesRepository.SearchCopyById(newLoanModel.CopiesId);
                BookModel bookModel = bookRepository.SearchBookById(copiesModel.BookId);

                BookLoaned bookLoaned=new BookLoaned();

                bookLoaned.LoanID = newLoanModel.LoanId;
                bookLoaned.LoanDate = newLoanModel.LoanDate;
                bookLoaned.ReturnDate = newLoanModel.ReturnDate;
                bookLoaned.MemberName = memberModel.MemberName;
                bookLoaned.BookName = bookModel.BookName;
                bookLoaned.ActualReturnDate = loanModel.ActualReturnDate;

                DateTime _returnDate = Convert.ToDateTime(newLoanModel.ReturnDate);
                DateTime _actualReturnDate = Convert.ToDateTime(loanModel.ActualReturnDate);

                double _numberOfDays = (_actualReturnDate - _returnDate).TotalDays;
                int numberOfDays = Convert.ToInt32(_numberOfDays);

                if (numberOfDays<0 || numberOfDays==0)
                {
                    bookLoaned.Charge = 0;
                }
                else
                {
                    bookLoaned.Charge = numberOfDays * 10;
                }

               TempData["BookLoaned"] = bookLoaned;
                bool returnedData = loanRepository.UpdateLoan(newLoanModel.LoanId);
                bool updatedData = copiesRepository.ChangeCopiesStatus(newLoanModel.CopiesId);

                if (returnedData && updatedData)
                {
                    var profileData = Session["UserProfile"] as UserSession;
                    var logModel = new LogModel
                    {
                        UserId = profileData.UserID,
                        TableName = "Loan",
                        Activity = "Loan Returned",
                        LogDate = DateTime.Now
                    };
                    var logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                    return RedirectToAction("ShowReturnedDetails");
                }
                return RedirectToAction("Dashboard", "Home");
            }
            catch 
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }

        public ActionResult ShowReturnedDetails()
        {
            BookLoaned bookLoaned=new BookLoaned();
            bookLoaned = (BookLoaned) TempData["BookLoaned"];
            return View(bookLoaned);
        }

        public ActionResult SearchByMemberName()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult SearchByMemberName(BookLoaned bookLoaned)
        {
            List<BookLoaned> listBookCopies=new List<BookLoaned>();
            LoanRepository loanRepository=new LoanRepository();
            listBookCopies = loanRepository.SearchByMember(bookLoaned.MemberName);
            TempData["LoanInfo"] = listBookCopies;
            return RedirectToAction("ViewLoanMember");
        }

        public ActionResult ViewLoanMember()
        {
            List<BookLoaned> listBookCopies = new List<BookLoaned>();
            listBookCopies = (List<BookLoaned>) TempData["LoanInfo"];
            return View(listBookCopies);
        }


    }
}