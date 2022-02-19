using System.Threading.Tasks;
using ArtAuction.Core.Domain.Entities;

namespace ArtAuction.Core.Application.Interfaces.Repositories
{
    public interface IAuctionRepository
    {
        Task<Auction> GetAuctionAsync(int auctionNumber);
        Task AddAuctionAsync(Auction auction);
        Task AddBidAsync(Bid bid);
        Task AddMessageAsync(Message message);
    }
}