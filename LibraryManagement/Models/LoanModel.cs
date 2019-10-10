using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class LoanModel
    {
        [Key]
        public int LoanId { get; set; }

        private DateTime _loanDate;
        private DateTime _returDate;

        public String LoanDate
        {
            get { return _loanDate.Date.ToString("yyyy MMMM dd"); }
            set { _loanDate = Convert.ToDateTime(value); }
        }

        public String ReturnDate
        {
            get { return _returDate.Date.ToString("yyyy MMMM dd"); }
            set { _returDate = Convert.ToDateTime(value); }
        }
        public int CopiesId { get; set; }
        public int MembershipId { get; set; }

        [NotMapped]
        public List<MemberModel> MemberList { get; set; }

        private DateTime _actualReturDate;

        public String ActualReturnDate
        {
            get { return _actualReturDate.Date.ToString("yyyy MMMM dd"); }
            set { _actualReturDate = Convert.ToDateTime(value); }
        }
    }
}