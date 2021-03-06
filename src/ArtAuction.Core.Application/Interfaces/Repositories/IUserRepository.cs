using System;
using System.Threading.Tasks;
using ArtAuction.Core.Domain.Entities;

namespace ArtAuction.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string login);
        User GetUser(Guid userId);
        Task AddUserAsync(User user);
        void UpdateUser(User user);

        Task<bool> IsUserAlreadyRegisteredAsync(string login, string email);
    }
}