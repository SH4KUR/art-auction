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
    public class CreateVipCommandHandler : IRequestHandler<CreateVipCommand, Unit>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public CreateVipCommandHandler(IAccountRepository accountRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(CreateVipCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserAsync(request.UserLogin);

            var account = await _accountRepository.GetAccount(user.UserId);
            if (account == null)
            {
                return Unit.Value;  // TODO: throw custom exception
            }

            var vipCost = Convert.ToDecimal(_configuration["App:VipCost"]);
            if (account.Sum < vipCost)
            {
                return Unit.Value;  // TODO: throw custom exception
            }

            var operationGuid = Guid.NewGuid();     // TODO: refactor for transaction
            var sumAfter = account.Sum - vipCost;
            var dateTimeNow = DateTime.Now;
            
            await _accountRepository.AddOperation(new Operation
            {
                OperationId = operationGuid,
                AccountId = account.AccountId,
                OperationType = OperationType.Withdraw,
                DateTime = dateTimeNow,
                Description = $"Purchase of VIP status from {dateTimeNow.ToShortDateString()}",
                SumBefore = account.Sum,
                SumOperation = vipCost,
                SumAfter = sumAfter
            });
            
            await _accountRepository.AddVip(new Vip
            {
                VipId = Guid.NewGuid(),
                OperationId = operationGuid,
                UserId = user.UserId,
                DateFrom = dateTimeNow,
                DateUntil = dateTimeNow.AddDays(Convert.ToInt32(_configuration["App:VipDaysCount"]))
            });

            account.LastUpdate = dateTimeNow;
            account.Sum = sumAfter;
            
            await _accountRepository.UpdateAccount(account);

            user.IsVip = true;
            _userRepository.UpdateUser(user);
            
            return Unit.Value;
        }
    }
}