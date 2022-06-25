using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ArtAuction.Core.Domain.Entities;

namespace ArtAuction.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string login);
        User GetUser(Guid userId);
        Task<IEnumerable<Review>> GetUserReviews(Guid userId);
        Task<IEnumerable<Complaint>> GetUserComplaints(Guid userId);
        Task<IEnumerable<Complaint>> GetComplaints();
        Task AddUserAsync(User user);
        void UpdateUser(User user);

        Task AddComplaintAsync(Complaint complaint);
        Task AddReviewAsync(Review review);

        Task<bool> IsUserAlreadyRegisteredAsync(string login, string email);
    }
}