using System.ComponentModel.DataAnnotations;

namespace ArtAuction.WebUI.Models.UserAccount
{
    public class AccountLoginViewModel
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }
}