using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class ShelfModel
    {
        [Key]
        public int ShelfId { get; set; }
        public int RackNo { get; set; }

    }
}