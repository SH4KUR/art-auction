using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.WebUI.Models.AuctionCatalog;
using ArtAuction.WebUI.Models.Lot;
using ArtAuction.WebUI.Models.Profile;
using ArtAuction.WebUI.Models.UserAccount;
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
            CreateMap<BidDto, BidViewModel>();
            CreateMap<MessageDto, MessageViewModel>();
            CreateMap<UserDto, UserProfileViewModel>();
        }
    }
}