using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class MemberModel
    {
        [Key]
        public int MemberId { get; set; }
        public string MemberName { get; set; }

        private DateTime _tempDob;
        public String Dob
        {
            get { return _tempDob.Date.ToString("yyyy MMMM dd"); }
            set { _tempDob = Convert.ToDateTime(value); }
        }
        public String PhoneNumber { get; set; }
        public string Email { get; set; }
        public int MembershipId { get; set; }
        
        [NotMapped]
        public List<MembershipModel> MembershipList { get; set; }
    }
}