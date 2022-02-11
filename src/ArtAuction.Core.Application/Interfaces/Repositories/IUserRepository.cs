using ArtAuction.Core.Domain.Entities;

namespace ArtAuction.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string login);
        void AddUser(User user);
        void UpdateUser(User user);
    }
}