using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class AuthModel
    {
        [Key]
        public int AuthenticationId { get; set; }
        public string AuthenticationName { get; set; }
    }
}