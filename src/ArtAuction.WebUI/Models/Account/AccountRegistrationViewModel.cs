using System;
using System.ComponentModel.DataAnnotations;
using ArtAuction.Core.Domain.Enums;

namespace ArtAuction.WebUI.Models.Account
{
    public class AccountRegistrationViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
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
        [DataType(DataType.Password)]
        public string PasswordRepeat { get; set; }
        
        [Required]
        public UserRole Role { get; set; }
    }
}