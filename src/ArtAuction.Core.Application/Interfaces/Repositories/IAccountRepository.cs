using System;
using System.Threading.Tasks;
using ArtAuction.Core.Domain.Entities;

namespace ArtAuction.Core.Application.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> GetAccount(Guid userId);
        Task AddOperation(Operation operation);
        Task AddVip(Vip vip);
    }
}