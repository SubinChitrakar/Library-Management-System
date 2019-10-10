using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Repository;

namespace LibraryManagement.Models
{
    public class UserModel : IEnumerable
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [DisplayName("Authentication Type")]
        public int AuthenticationId { get; set; }

        [NotMapped]
        public List<AuthModel> AuthenticationList { get; set; }
        
        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}