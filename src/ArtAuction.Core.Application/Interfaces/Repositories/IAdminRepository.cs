using System;
using System.Threading.Tasks;

namespace ArtAuction.Core.Application.Interfaces.Repositories
{
    public interface IAdminRepository
    {
        Task MarkComplaintProcessed(Guid complaintId);

        Task BlockUser(Guid userId);
        Task UnblockUser(Guid userId);
    }
}