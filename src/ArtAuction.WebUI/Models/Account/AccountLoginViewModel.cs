using System.ComponentModel.DataAnnotations;

namespace ArtAuction.WebUI.Models.Account
{
    public class AccountLoginViewModel
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}