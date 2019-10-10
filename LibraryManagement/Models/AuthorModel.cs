using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class AuthorModel
    {
        [Key]
        public int AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}