using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using LibraryManagement.Models;

namespace LibraryManagement.ViewModels
{
    public class BookDetails
    {
        public BookModel BookModel { get; set; }
        public int Author1 { get; set; }
        public int Author2 { get; set; }
        public int Author3 { get; set; }

        [NotMapped]
        public List<AuthorModel> AuthorList { get; set; }
        public AuthorModel AuthorInfo1 { get; set; }
        public AuthorModel AuthorInfo2 { get; set; }
        public AuthorModel AuthorInfo3 { get; set; }

        public PublicationModel PublicationModel { get; set; }
        public ContentRatingModel ContentRatingModel { get; set; }


        public AuthorModel TempAuthor { get; set; }
        public PublicationModel TempPublication { get; set; }
        public ContentRatingModel TempContentRating { get; set; }
    }
}