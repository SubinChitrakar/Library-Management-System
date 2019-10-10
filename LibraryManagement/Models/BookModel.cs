using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    public class BookModel
    {
        [Key]

        public int BookId { get; set; }

        public string BookName { get; set; }

        private DateTime _tempPublishedDate;
        public String PublishedDate
        {
            get { return _tempPublishedDate.Date.ToString("yyyy MMMM dd"); }
            set { _tempPublishedDate = Convert.ToDateTime(value); }
        }

        public int ContentRatingId { get; set; }

        public string Edition { get; set; }

        public int PublicationId { get; set; }

        public string BookType { get; set; }

        public string Genre { get; set; }

        [NotMapped]
        public List<PublicationModel> PublicationList { get; set; }

        [NotMapped] 
        public List<ContentRatingModel> ContentRatingList { get; set; }

        [NotMapped]
        public List<String> BookTypeList { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}