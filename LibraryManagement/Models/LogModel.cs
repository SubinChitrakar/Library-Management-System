using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class LogModel
    {
        [Key]
        public int LogId { get; set; }
        public int UserId { get; set; }
        public string TableName { get; set; }
        public string Activity { get; set; }
        public DateTime LogDate { get; set; }

        [NotMapped]
        public String Username { get; set; }
    }
}