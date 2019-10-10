using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryManagement.Models;

namespace LibraryManagement.ViewModels
{
    public class BookInfo
    {
        public int BookID { get; set; }
        public String BookName { get; set; }
        public String AuthorName { get; set; }

        public List<AuthorModel> authorList { get; set; }
        public String PublicationName { get; set; }
    }
}