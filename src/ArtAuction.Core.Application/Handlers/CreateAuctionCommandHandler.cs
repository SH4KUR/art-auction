using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class CreateAuctionCommandHandler : IRequestHandler<CreateAuctionCommand, bool>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;

        public CreateAuctionCommandHandler(IAuctionRepository auctionRepository, IUserRepository userRepository)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
        {
            var sellerUser = await _userRepository.GetUserAsync(request.SellerLogin);
            if (sellerUser == null)
            {
                return false;   // TODO: Add error handling
            }
            
            var auction = new Auction
            {
                SellerId = sellerUser.UserId,
                
                StartPrice = request.StartPrice,
                CurrentPrice = request.StartPrice,
                FullPrice = request.FullPrice,
                BidStep = request.BidStep,
                
                StartBillingDate = request.StartBillingDate,
                EndBillingDate = request.EndBillingDate,
                
                Lot = new Lot
                {
                    Name = request.LotName,
                    Category = new Category { Name = request.CategoryName },
                    PaintingDate = request.PaintingDate,
                    Photo = request.Photo,
                    Description = request.Description
                }
            };

            await _auctionRepository.AddAuctionAsync(auction);

            return true;
        }
    }
}