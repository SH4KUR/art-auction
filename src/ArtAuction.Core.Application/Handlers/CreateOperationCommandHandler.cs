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
    public class CreateOperationCommandHandler : IRequestHandler<CreateOperationCommand, Unit>
    {
        private readonly IAccountRepository _accountRepository;

        public CreateOperationCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<Unit> Handle(CreateOperationCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetAccount(request.UserId);
            if (account == null)
            {
                return Unit.Value;  // TODO: throw custom exception
            }

            var operation = new Operation
            {
                AccountId = account.AccountId,
                OperationType = request.OperationType,
                DateTime = DateTime.Now,
                Description = request.Description,
                SumBefore = account.Sum,
                SumOperation = request.Sum
            };

            if (request.OperationType == OperationType.Replenishment)
            {
                operation.SumAfter = operation.SumBefore + operation.SumOperation;
            }
            else
            {
                if (account.Sum < request.Sum)
                {
                    return Unit.Value;  // TODO: throw custom exception
                }

                operation.SumAfter = operation.SumBefore - operation.SumOperation;
            }

            await _accountRepository.AddOperation(operation);

            return Unit.Value;
        }
    }
}