using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Core.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.Core.Application.Handlers
{
    public class PlaceBetCommandHandler : IRequestHandler<PlaceBetCommand, Unit>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public PlaceBetCommandHandler(IAuctionRepository auctionRepository, IUserRepository userRepository, IAccountRepository accountRepository, IConfiguration configuration)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(PlaceBetCommand request, CancellationToken cancellationToken)    // TODO: Refactor with SRP
        {
            var bidDateTime = DateTime.Now;
            var auction = await _auctionRepository.GetAuctionAsync(request.AuctionNumber);

            if (auction.EndBillingDateTime <= bidDateTime)
            {
                throw new Exception("This Auction has already ended!");     // TODO: Custom exception
            }
            
            var user = await _userRepository.GetUserAsync(request.UserLogin);
            var userAccount = await _accountRepository.GetAccount(user.UserId);

            if (userAccount.Sum < request.Sum)
            {
                throw new Exception("The amount on the Account isn't enough for the Bet!");     // TODO: Custom exception
            }

            // anti-sniper
            if (auction.IsVip)
            {
                var secsForAntiSniper = Convert.ToInt32(_configuration["App:AntiSniperSecs"]);
                if (auction.EndBillingDateTime.Subtract(bidDateTime).Seconds <= secsForAntiSniper)
                {
                    auction.EndBillingDateTime = auction.EndBillingDateTime.AddSeconds(Convert.ToInt32(_configuration["App:AntiSniperAdditionalSecs"]));
                }
            }

            var operation = new Operation
            {
                OperationId = Guid.NewGuid(),
                AccountId = userAccount.AccountId,
                OperationType = OperationType.Withdraw,
                DateTime = bidDateTime,
                Description = $"Bid for Auction #{auction.AuctionNumber} at {bidDateTime.ToShortDateString()} - {bidDateTime.ToShortTimeString()}",
                SumBefore = userAccount.Sum,
                SumOperation = request.Sum,
                SumAfter = userAccount.Sum - request.Sum
            };

            userAccount.LastUpdate = bidDateTime;
            userAccount.Sum = operation.SumAfter;
            
            await _accountRepository.AddOperation(operation);
            await _accountRepository.UpdateAccount(userAccount);

            auction.CurrentPrice = request.Sum;
            await _auctionRepository.UpdateAuctionAsync(auction);

            // notification message in chat
            await _auctionRepository.AddMessageAsync(new Message
            {
                MessageId = Guid.NewGuid(),
                AuctionId = auction.AuctionId,
                UserId = user.UserId,
                DateTime = bidDateTime,
                IsAdmin = false,
                MessageText = $"User placed bid {request.Sum}!"
            });

            // returns of the last bid
            var lastBid = auction.Bids.OrderByDescending(b => b.DateTime).FirstOrDefault();
            if (lastBid != null)
            {
                var lastBidUserAccount = await _accountRepository.GetAccount(lastBid.UserId);

                var lastBidRefundOperation = new Operation
                {
                    OperationId = Guid.NewGuid(),
                    AccountId = lastBidUserAccount.AccountId,
                    OperationType = OperationType.Replenishment,
                    DateTime = bidDateTime,
                    Description = $"Refund of the bid on the Auction #{auction.AuctionNumber} at {bidDateTime.ToShortDateString()} - {bidDateTime.ToShortTimeString()}",
                    SumBefore = lastBidUserAccount.Sum,
                    SumOperation = lastBid.Sum,
                    SumAfter = lastBidUserAccount.Sum + lastBid.Sum
                };

                lastBidUserAccount.LastUpdate = bidDateTime;
                lastBidUserAccount.Sum = lastBidRefundOperation.SumAfter;

                await _accountRepository.AddOperation(lastBidRefundOperation);
                await _accountRepository.UpdateAccount(lastBidUserAccount);
            }

            return Unit.Value;
        }
    }
}