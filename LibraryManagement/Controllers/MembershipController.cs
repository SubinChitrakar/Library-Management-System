using System;
using System.Web.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Repository;

namespace LibraryManagement.Controllers
{
    public class MembershipController : Controller
    {
        public ActionResult GetAllMembershipDetails()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            MembershipRepository MemshipRepo = new MembershipRepository();
            ModelState.Clear();
            return View(MemshipRepo.GetAllMembership());
        }

        public ActionResult AddMembership()
        {
            var profileData = this.Session["UserProfile"] as UserSession;
            if (profileData == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddMembership(MembershipModel Memship)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MembershipRepository MemshipRepo = new MembershipRepository();
                    MemshipRepo.AddMembership(Memship);

                    var profileData = this.Session["UserProfile"] as UserSession;
                    LogModel logModel = new LogModel()
                    {
                        UserId = profileData.UserID,
                        TableName = "Membership",
                        Activity = "Added Membership",
                        LogDate = DateTime.Now
                    };
                    logRepository logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                }

                return RedirectToAction("GetAllMembershipDetails");
            }
            catch
            {
                return RedirectToAction("GetAllMembershipDetails");
            }
        }

        public ActionResult EditMembershipDetails(int id)
        {
            var profileData = this.Session["UserProfile"] as UserSession;
            if (profileData == null)
            {
                return RedirectToAction("Index", "Home");
            }

            MembershipRepository MemshipRepo = new MembershipRepository();
            return View(MemshipRepo.GetAllMembership().Find(Memp => Memp.MembershipId == id));

        }

        [HttpPost]
        public ActionResult EditMembershipDetails(int id, MembershipModel obj)
        {
            try
            {
                MembershipRepository MemshipRepo = new MembershipRepository();
                MemshipRepo.UpdateMembership(obj);

                var profileData = this.Session["UserProfile"] as UserSession;
                LogModel logModel = new LogModel()
                {
                    UserId = profileData.UserID,
                    TableName = "Membership",
                    Activity = "Updated Membership",
                    LogDate = DateTime.Now
                };
                logRepository logRepository = new logRepository();
                logRepository.AddLog(logModel);

                return RedirectToAction("GetAllMembershipDetails");
            }
            catch
            {
                return RedirectToAction("GetAllMembershipDetails");
            }
        }
        public ActionResult DeleteMembership(int id)
        {
            var profileData = this.Session["UserProfile"] as UserSession;
            if (profileData == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                MembershipRepository CrgRepo = new MembershipRepository();
                if (CrgRepo.DeleteMembership(id))
                {
                    ViewBag.AlertMsg = "Employee details deleted successfully";

                }
                LogModel logModel = new LogModel()
                {
                    UserId = profileData.UserID,
                    TableName = "Membership",
                    Activity = "Deleted Membership",
                    LogDate = DateTime.Now
                };
                logRepository logRepository = new logRepository();
                logRepository.AddLog(logModel);
                return RedirectToAction("GetAllMembershipDetails");

            }
            catch
            {
                return RedirectToAction("GetAllMembershipDetails");
            }
        }    
    }
}