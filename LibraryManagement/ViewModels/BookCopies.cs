using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LibraryManagement.Models;

namespace LibraryManagement.ViewModels
{
    public class BookCopies
    {
        public BookModel BookModel { get; set; }
        public int Quantities { get; set; }
        public CopiesModel CopiesModel { get; set; }

        [NotMapped]
        public List<ShelfModel> ShelfList = new List<ShelfModel>();
    }
}