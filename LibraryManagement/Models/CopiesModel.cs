using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibraryManagement.Models
{
    public class CopiesModel
    {
        public int CopiesId { get; set; }
        public int BookId { get; set; }
        private DateTime _dateBought;
        public String DateBought
        {
            get { return _dateBought.Date.ToString("yyyy MMMM dd"); }
            set { _dateBought = Convert.ToDateTime(value); }
        }
        public int Location { get; set; }
        public bool CopyStatus { get; set; }
        public int CopyNo { get; set; }
    }
}