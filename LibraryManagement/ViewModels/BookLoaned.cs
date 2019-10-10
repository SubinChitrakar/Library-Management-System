using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagement.ViewModels
{
    public class BookLoaned
    {
        public int LoanID { get; set; }

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
        public String MemberName { get; set; }
        public String BookName { get; set; }

        private DateTime _actualReturnDate;
        public String ActualReturnDate
        {
            get { return _actualReturnDate.Date.ToString("yyyy MMMM dd"); }
            set { _actualReturnDate = Convert.ToDateTime(value); }
        }

        public float Charge { get; set; }
    }
}