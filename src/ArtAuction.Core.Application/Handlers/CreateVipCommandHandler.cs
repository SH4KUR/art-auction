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
        private readonly IConfiguration _configuration;

        public CreateVipCommandHandler(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(CreateVipCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccount(request.UserId);
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
            await _accountRepository.AddOperation(new Operation
            {
                OperationId = operationGuid,
                AccountId = account.AccountId,
                OperationType = OperationType.Withdraw,
                DateTime = DateTime.Now,
                Description = $"Purchase of VIP status from {DateTime.Now.ToShortDateString()}",
                SumBefore = account.Sum,
                SumOperation = vipCost,
                SumAfter = account.Sum - vipCost
            });
            
            await _accountRepository.AddVip(new Vip
            {
                VipId = Guid.NewGuid(),
                OperationId = operationGuid,
                UserId = request.UserId,
                DateFrom = DateTime.Now,
                DateUntil = DateTime.Now.AddDays(Convert.ToInt32(_configuration["App:VipDaysCount"]))
            });
            
            return Unit.Value;
        }
    }
}