using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class MembershipModel
    {
        [Key]
        public int MembershipId { get; set; }
        public string MembershipCategory { get; set; }
        public int NoOfBooks { get; set; }
    }
}