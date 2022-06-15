using System;
using System.ComponentModel.DataAnnotations;
using ArtAuction.Core.Domain.Enums;

namespace ArtAuction.WebUI.Models.UserAccount
{
    public class AccountRegistrationViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public string Patronymic { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Required]
        public string Address { get; set; }
        
        [Required]
        public string Login { get; set; }
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password", ErrorMessage = "Password mismatch")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
        
        [Required]
        public UserRole Role { get; set; }
    }
}