using System;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;

namespace ArtAuction.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<Account> GetAccount(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}