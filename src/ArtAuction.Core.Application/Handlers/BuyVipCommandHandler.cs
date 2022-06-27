using System;
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
    public class BuyVipCommandHandler : IRequestHandler<BuyVipCommand, Unit>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public BuyVipCommandHandler(IAccountRepository accountRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(BuyVipCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.UserLogin);

            var account = await _accountRepository.GetAccount(user.UserId);
            if (account == null)
            {
                return Unit.Value;  // TODO: throw custom exception
            }

            var vipCost = Convert.ToDecimal(_configuration["App:VipStatusCost"]);
            if (account.Sum < vipCost)
            {
                return Unit.Value;  // TODO: throw custom exception
            }

            Operation operation;
            
            if (request.ByCard)
            {
                operation = new Operation
                {
                    OperationId = Guid.NewGuid(),
                    AccountId = account.AccountId,
                    OperationType = OperationType.Withdraw,
                    DateTime = DateTime.Now,
                    Description = "Purchase of VIP status by Card.",
                    SumBefore = account.Sum,
                    SumOperation = 0,
                    SumAfter = account.Sum
                };
            }
            else
            {
                operation = new Operation
                {
                    OperationId = Guid.NewGuid(),
                    AccountId = account.AccountId,
                    OperationType = OperationType.Withdraw,
                    DateTime = DateTime.Now,
                    Description = "Purchase of VIP status.",
                    SumBefore = account.Sum,
                    SumOperation = vipCost,
                    SumAfter = account.Sum - vipCost
                };
            }
            
            await _accountRepository.AddOperation(operation);
            
            await _accountRepository.AddVip(new Vip
            {
                VipId = Guid.NewGuid(),
                OperationId = operation.OperationId,
                UserId = user.UserId,
                DateFrom = operation.DateTime,
                DateUntil = operation.DateTime.AddDays(Convert.ToInt32(_configuration["App:VipStatusDaysCount"]))
            });
            
            user.IsVip = true;
            _userRepository.UpdateUser(user);
            
            return Unit.Value;
        }
    }
}