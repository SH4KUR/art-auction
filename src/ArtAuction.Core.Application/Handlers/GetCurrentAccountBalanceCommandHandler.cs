using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Interfaces.Repositories;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetCurrentAccountBalanceCommandHandler : IRequestHandler<GetCurrentAccountBalanceCommand, decimal>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public GetCurrentAccountBalanceCommandHandler(IUserRepository userRepository, IAccountRepository accountRepository)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
        }

        public async Task<decimal> Handle(GetCurrentAccountBalanceCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.UserLogin);
            if (user == null)
            {
                throw new("User not found!");   // TODO: Custom exception
            }
            
            var account = await _accountRepository.GetAccount(user.UserId);
            return account.Sum;
        }
    }
}