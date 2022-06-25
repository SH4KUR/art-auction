using System;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Interfaces.Repositories;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.Infrastructure.Persistence.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IConfiguration _configuration;

        public AdminRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public async Task MarkComplaintProcessed(Guid complaintId)
        {
            var query = @"
                UPDATE [dbo].[complaint] 
                SET
                    [is_processed] = 1
                WHERE 
                    [complaint_id] = @ComplaintId";

            await using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            await connection.ExecuteAsync(query, new
            {
                ComplaintId = complaintId
            });
        }

        public async Task BlockUser(Guid userId)
        {
            var query = @"
                UPDATE [dbo].[user] 
                SET
                    [is_blocked] = 1
                WHERE 
                    [user_id] = @UserId";

            await using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            await connection.ExecuteAsync(query, new
            {
                UserId = userId
            });
        }

        public async Task UnblockUser(Guid userId)
        {
            var query = @"
                UPDATE [dbo].[user] 
                SET
                    [is_blocked] = 0
                WHERE 
                    [user_id] = @UserId";

            await using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            await connection.ExecuteAsync(query, new
            {
                UserId = userId
            });
        }
    }
}