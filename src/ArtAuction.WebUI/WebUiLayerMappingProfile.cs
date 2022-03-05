using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.WebUI.Models.Account;
using ArtAuction.WebUI.Models.AuctionCatalog;
using AutoMapper;

namespace ArtAuction.WebUI
{
    public class WebUiLayerMappingProfile : Profile
    {
        public WebUiLayerMappingProfile()
        {
            CreateMap<AccountRegistrationViewModel, RegisterUserCommand>();
            CreateMap<CreateAuctionLotViewModel, CreateAuctionCommand>();
            CreateMap<AuctionCatalogDto, AuctionViewModel>();
        }
    }
}