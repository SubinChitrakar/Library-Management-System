using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibraryManagement.Models;

namespace LibraryManagement.ViewModels
{
    public class SearchandUser
    {
        public UserModel User { get; set; }
        public BookModel Book { get; set; }
        public AuthorModel Author { get; set; }
        public PublicationModel Publication { get; set; }

        public String BookName { get; set; }
        public String AuthorName { get; set; }
        public String PublicationName { get; set; }
        public int RackNo { get; set; }
        public int Quantity { get; set; }
    }
}