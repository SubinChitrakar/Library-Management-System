using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class BookAuthorModel
    {
        [Key]

        public int BookAuthorId { get; set; }

        public int BookId { get; set; }

        public int AuthorId { get; set; }

    }
}