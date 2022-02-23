﻿using ArtAuction.Core.Application.Commands;
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
        }
    }
}