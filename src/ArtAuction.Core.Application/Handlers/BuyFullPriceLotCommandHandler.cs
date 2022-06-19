using System;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Core.Domain.Enums;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class BuyFullPriceLotCommandHandler : IRequestHandler<BuyFullPriceLotCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuctionRepository _auctionRepository;

        public BuyFullPriceLotCommandHandler(IUserRepository userRepository, IAccountRepository accountRepository, IAuctionRepository auctionRepository)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _auctionRepository = auctionRepository;
        }

        public async Task<Unit> Handle(BuyFullPriceLotCommand request, CancellationToken cancellationToken)
        {
            var bidDateTime = DateTime.Now;
            
            var user = await _userRepository.GetUserAsync(request.UserLogin);
            var userAccount = await _accountRepository.GetAccount(user.UserId);
            var auction = await _auctionRepository.GetAuctionAsync(request.AuctionNumber);

            if (!auction.FullPrice.HasValue)
            {
                throw new Exception("FullPrice is not set");     // TODO: Custom exception
            }

            if (userAccount.Sum < auction.FullPrice.Value)
            {
                throw new Exception("The amount on the Account isn't enough!");     // TODO: Custom exception
            }

            var operation = new Operation
            {
                OperationId = Guid.NewGuid(),
                AccountId = userAccount.AccountId,
                OperationType = OperationType.Withdraw,
                DateTime = bidDateTime,
                Description = $"FullPrice Purchase of Auction #{auction.AuctionNumber}",
                SumBefore = userAccount.Sum,
                SumOperation = auction.FullPrice.Value,
                SumAfter = userAccount.Sum - auction.FullPrice.Value
            };
            await _accountRepository.AddOperation(operation);

            userAccount.LastUpdate = bidDateTime;
            userAccount.Sum = operation.SumAfter;
            await _accountRepository.UpdateAccount(userAccount);

            auction.CurrentPrice = auction.FullPrice.Value;
            auction.CustomerId = user.UserId;
            auction.IsClosed = true;
            await _auctionRepository.UpdateAuctionAsync(auction);

            return Unit.Value;
        }
    }
}