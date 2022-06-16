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
    public class AddAuctionMessageCommandHandler : IRequestHandler<AddAuctionMessageCommand, Unit>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;

        public AddAuctionMessageCommandHandler(IAuctionRepository auctionRepository, IUserRepository userRepository)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(AddAuctionMessageCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.Login);
            var auction = await _auctionRepository.GetAuctionAsync(request.AuctionNumber);

            var message = new Message
            {
                MessageId = Guid.NewGuid(),
                AuctionId = auction.AuctionId,
                UserId = user.UserId,
                DateTime = DateTime.Now,
                IsAdmin = user.Role == UserRole.Administrator,
                MessageText = request.Message
            };

            await _auctionRepository.AddMessageAsync(message);
            
            return Unit.Value;
        }
    }
}