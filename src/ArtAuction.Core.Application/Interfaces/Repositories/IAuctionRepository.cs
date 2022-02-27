using System.Collections.Generic;
using System.Threading.Tasks;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Core.Domain.Enums;

namespace ArtAuction.Core.Application.Interfaces.Repositories
{
    public interface IAuctionRepository
    {
        Task<Auction> GetAuctionAsync(int auctionNumber);
        Task<IEnumerable<Auction>> GetAuctions(
            SortingRule sort, 
            IEnumerable<string> filterCategories, 
            decimal? minCurrentPrice,
            decimal? maxCurrentPrice, 
            int pageNumber = 1, 
            int rowsOnPage = 10, 
            bool isClosed = false);
        Task AddAuctionAsync(Auction auction);
        
        Task AddBidAsync(Bid bid);
        Task AddMessageAsync(Message message);

        Task<IEnumerable<Category>> GetCategories();
    }
}