using ArtAuction.Core.Application.Commands;
using ArtAuction.WebUI.Models.Account;
using AutoMapper;

namespace ArtAuction.WebUI
{
    public class WebUiLayerMappingProfile : Profile
    {
        public WebUiLayerMappingProfile()
        {
            CreateMap<AccountRegistrationViewModel, RegisterUserCommand>();
        }
    }
}