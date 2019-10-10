using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class PublicationModel
    {
        [Key]
        public int PublicationId { get; set; }

        public string PublicationName { get; set; }
    }
}