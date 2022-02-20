using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Domain.Entities;
using AutoMapper;

namespace ArtAuction.Core.Application
{
    public class ApplicationLayerMappingProfile : Profile
    {
        public ApplicationLayerMappingProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}   