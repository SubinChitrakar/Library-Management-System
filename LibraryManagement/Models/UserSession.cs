using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagement.Models
{
    public class UserSession
    {
        public int UserID { get; set; }
        public string UserName { get; set; }

        public int Designation { get; set; }
    }
}