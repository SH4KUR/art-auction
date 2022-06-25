using System.Collections.Generic;

namespace ArtAuction.WebUI.Models.Profile
{
    public class UserOperationsViewModel
    {
        public UserViewModel User { get; set; }
        public IEnumerable<OperationViewModel> Operations { get; set; }
    }
}