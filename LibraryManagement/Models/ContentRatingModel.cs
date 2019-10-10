using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class ContentRatingModel
    {
        [Key]

        public int ContentRatingId { get; set; }

        public string ContentRatingName { get; set; }
    }
}