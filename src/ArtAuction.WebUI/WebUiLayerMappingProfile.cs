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
            CreateMap<AuctionCatalogDto, AuctionViewModel>()
                .ForMember(vm => vm.Image, dto => dto.MapFrom(d => d.Photo));
            CreateMap<BidDto, BidViewModel>();
            CreateMap<MessageDto, MessageViewModel>();
            CreateMap<UserDto, UserViewModel>();
            CreateMap<OperationDto, OperationViewModel>();
            CreateMap<ComplaintDto, ComplaintViewModel>();
            CreateMap<ReviewDto, ReviewViewModel>();
        }
    }
}