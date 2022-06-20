using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.DTO;
using ArtAuction.Core.Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace ArtAuction.Core.Application.Handlers
{
    public class GetAccountOperationsCommandHandler : IRequestHandler<GetAccountOperationsCommand, IEnumerable<OperationDto>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetAccountOperationsCommandHandler(IAccountRepository accountRepository, IUserRepository userRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OperationDto>> Handle(GetAccountOperationsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.UserLogin);
            var account = await _accountRepository.GetAccount(user.UserId);

            return account.Operations.Select(_mapper.Map<OperationDto>);
        }
    }
}