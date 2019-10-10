using System;
using System.Web.Mvc;
using LibraryManagement.Models;
using LibraryManagement.Repository;

namespace LibraryManagement.Controllers
{
    public class MemberController : Controller
    {
        public ActionResult GetAllMemberDetails()
        {
            var profileData = Session["UserProfile"] as UserSession;
            if (profileData == null)
                return RedirectToAction("Index", "Home");

            MemberRepository memberRepository = new MemberRepository();
            ModelState.Clear();
            return View(memberRepository.GetAllMemberWithMemberships());
        }

        public ActionResult AddMember()
        {
            var profileData = this.Session["UserProfile"] as UserSession;
            if (profileData == null)
            {
                return RedirectToAction("Index", "Home");
            }

            MemberRepository memberRepository = new MemberRepository();
            MemberModel memberModel = new MemberModel();
            ModelState.Clear();
            memberModel = memberRepository.GetAllMemberShip();

            return View(memberModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember(MemberModel member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    MemberRepository memberRepository = new MemberRepository();
                    memberRepository.AddNewMember(member);

                    var profileData = this.Session["UserProfile"] as UserSession;
                    LogModel logModel = new LogModel()
                    {
                        UserId = profileData.UserID,
                        TableName = "Member",
                        Activity = "Added Member",
                        LogDate = DateTime.Now
                    };
                    logRepository logRepository = new logRepository();
                    logRepository.AddLog(logModel);
                }
                return RedirectToAction("GetAllMemberDetails");
            }
            catch
            {
                return RedirectToAction("GetAllMemberDetails");
            }
        }

        public ActionResult EditMemberDetails(int id)
        {
            var profileData = this.Session["UserProfile"] as UserSession;
            if (profileData == null)
            {
                return RedirectToAction("Index", "Home");
            }

            MemberRepository MemRepo = new MemberRepository();
            return View(MemRepo.GetAllMember().Find(Mem => Mem.MemberId == id));
        }

        [HttpPost]
        public ActionResult EditMemberDetails(int id, MemberModel member)
        {
            try
            {
                MemberRepository MemRepo = new MemberRepository();
                MemRepo.UpdateMember(member);

                var profileData = this.Session["UserProfile"] as UserSession;
                LogModel logModel = new LogModel()
                {
                    UserId = profileData.UserID,
                    TableName = "Member",
                    Activity = "Updated Member",
                    LogDate = DateTime.Now
                };
                logRepository logRepository = new logRepository();
                logRepository.AddLog(logModel);

                return RedirectToAction("GetAllMemberDetails");
            }
            catch
            {
                return RedirectToAction("GetAllMemberDetails");
            }
        }
        public ActionResult DeleteMember(int id)
        {
            var profileData = this.Session["UserProfile"] as UserSession;
            if (profileData == null)
            {
                return RedirectToAction("Index", "Home");
            }

            try
            {
                MemberRepository MemRepo = new MemberRepository();
                if (MemRepo.DeleteMember(id))
                {
                    ViewBag.AlertMsg = "Employee details deleted successfully";

                }
                LogModel logModel = new LogModel()
                {
                    UserId = profileData.UserID,
                    TableName = "Member",
                    Activity = "Deleted Member",
                    LogDate = DateTime.Now
                };
                logRepository logRepository = new logRepository();
                logRepository.AddLog(logModel);
                return RedirectToAction("GetAllMemberDetails");

            }
            catch
            {
                return RedirectToAction("GetAllMemberDetails");
            }
        }

        public ActionResult InactiveUser()
        {
            MemberRepository memberRepository=new MemberRepository();
            return View(memberRepository.getInactiveUser());
        }
    }
}