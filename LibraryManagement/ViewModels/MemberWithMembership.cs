using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagement.ViewModels
{
    public class MemberWithMembership
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }

        private DateTime _tempDob;

        public string DOB
        {
            get { return _tempDob.Date.ToString("yyyy MMMM dd"); }
            set { _tempDob = Convert.ToDateTime(value); }
        }

        public String PhoneNo { get; set; }

        public string Email { get; set; }

        public string MembershipType { get; set; }

        public int NoOfBooks { get; set; }
    }
}