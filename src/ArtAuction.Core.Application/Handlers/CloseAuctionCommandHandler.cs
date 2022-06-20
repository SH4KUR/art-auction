using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Core.Domain.Enums;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class CloseAuctionCommandHandler : IRequestHandler<CloseAuctionCommand, Unit>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public CloseAuctionCommandHandler(IAuctionRepository auctionRepository, IUserRepository userRepository, IAccountRepository accountRepository)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }

        public async Task<Unit> Handle(CloseAuctionCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAuctionAsync(request.AuctionNumber);
            if (auction.Bids.Any())
            {
                var sellerAccount = await _accountRepository.GetAccount(auction.SellerId);
                var wonBet = auction.Bids.OrderBy(a => a.DateTime).First();

                auction.CustomerId = wonBet.UserId;
                
                await _accountRepository.AddOperation(new Operation
                {
                    OperationId = Guid.NewGuid(),
                    AccountId = sellerAccount.AccountId,
                    OperationType = OperationType.Replenishment,
                    DateTime = DateTime.Now,
                    Description = $"Auction Lot Sold #{auction.AuctionNumber}",
                    SumBefore = sellerAccount.Sum,
                    SumOperation = wonBet.Sum,
                    SumAfter = sellerAccount.Sum + wonBet.Sum
                });

                sellerAccount.LastUpdate = DateTime.Now;
                sellerAccount.Sum = wonBet.Sum;
                await _accountRepository.UpdateAccount(sellerAccount);
            }
            
            auction.IsClosed = true;
            await _auctionRepository.UpdateAuctionAsync(auction);
            
            return Unit.Value;
        }
    }
}