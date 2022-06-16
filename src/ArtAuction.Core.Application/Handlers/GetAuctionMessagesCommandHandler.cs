using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Enums;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetAuctionMessagesCommandHandler : IRequestHandler<GetAuctionMessagesCommand, IEnumerable<MessageDto>>
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IUserRepository _userRepository;

        public GetAuctionMessagesCommandHandler(IAuctionRepository auctionRepository, IUserRepository userRepository)
        {
            _auctionRepository = auctionRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<MessageDto>> Handle(GetAuctionMessagesCommand request, CancellationToken cancellationToken)
        {
            var auction = await _auctionRepository.GetAuctionAsync(request.AuctionNumber);
            var messages = new List<MessageDto>();
            
            foreach (var message in auction.Messages)
            {
                var user = _userRepository.GetUser(message.UserId);

                messages.Add(new MessageDto
                {
                    UserLogin = user.Login,
                    IsAdmin = user.Role == UserRole.Administrator,
                    DateTime = message.DateTime,
                    MessageText = message.MessageText
                });
            }

            return messages;
        }
    }
}