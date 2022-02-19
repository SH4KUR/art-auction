using System.Threading.Tasks;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;

namespace ArtAuction.Infrastructure.Persistence.Repositories
{
    public class AuctionRepository : IAuctionRepository
    {
        public Task<Auction> GetAuctionAsync(int auctionNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task AddAuction(Auction auction)
        {
            throw new System.NotImplementedException();
        }
    }
}