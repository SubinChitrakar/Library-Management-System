using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryManagement.ViewModels
{
    public class CopyDetail
    {        
        public int CopiesId { get; set; }
        public string BookName { get; set; }
        public int RackNo { get; set; }
        public int Quantity { get; set; }
        public string BookType { get; set; }
    }
}