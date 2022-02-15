using System.Threading.Tasks;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ArtAuction.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<User> GetUserAsync(string login)
        {
            var query = @"
                SELECT 
                     [user_id] AS UserId
                    ,[login]
                    ,[email]
                    ,[password]
                    ,[role]
                    ,[first_name] AS FirstName
                    ,[last_name] AS LastName
                    ,[patronymic]
                    ,[birth_date] AS BirthDate
                    ,[address]
                    ,[is_vip] AS IsVip
                    ,[is_blocked] AS IsBlocked
                FROM [dbo].[user]
                WHERE 
                    [login] = @UserLogin";

            await using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            return await connection.QueryFirstOrDefaultAsync<User>(query, new
            {
                UserLogin = login
            });
        }

        public async Task AddUserAsync(User user)
        {
            var query = @"
                DECLARE @AddedUser TABLE ([user_id] UNIQUEIDENTIFIER)
                    
                INSERT INTO [dbo].[user] (
	                 [login]
	                ,[email]
	                ,[password]
	                ,[role]
	                ,[first_name]
	                ,[last_name]
	                ,[patronymic]
	                ,[birth_date]
	                ,[address]
	                ,[is_vip]
	                ,[is_blocked]
                )
                OUTPUT INSERTED.[user_id] INTO @AddedUser
                VALUES (
	                 @Login
	                ,@Email
	                ,@Password
	                ,@Role
	                ,@FirstName
	                ,@LastName
	                ,@Patronymic
	                ,@BirthDate
	                ,@Address
	                ,@IsVip
	                ,@IsBlocked
                )

                INSERT INTO [dbo].[account] (
	                 [account_id]
                    ,[user_id]
                    ,[sum]
                    ,[last_update]
                )
                VALUES (
	                 NEWID()
	                ,(SELECT TOP 1 [user_id] FROM @AddedUser)
	                ,0
	                ,GETDATE()
                )";

            await using (var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection)))
            {
                await connection.OpenAsync();
                await using (var transaction = await connection.BeginTransactionAsync())
                {
                    var sqlParams = new
                    {
                        user.Login,
                        user.Email,
                        user.Password,
                        user.Role,
                        user.FirstName,
                        user.LastName,
                        user.Patronymic,
                        user.BirthDate,
                        user.Address,
                        user.IsVip,
                        user.IsBlocked
                    };
                    
                    await connection.ExecuteAsync(query, sqlParams, transaction);
                    await transaction.CommitAsync();
                }
            }
        }

        public void UpdateUser(User user)
        {
            var query = @"
                UPDATE [dbo].[user]
                SET 
	                 [user_id] = @UserId
	                ,[login] = @Login
	                ,[email] = @Email
	                ,[password] = @Password
	                ,[role] = @Role
	                ,[first_name] = @FirstName
	                ,[last_name] = @LastName
	                ,[patronymic] = @Patronymic
	                ,[birth_date] = @BirthDate
	                ,[address] = @Address
	                ,[is_vip] = @IsVip
	                ,[is_blocked] = @IsBlocked
                WHERE 
	                [user_id] = @UserId";

            using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            connection.Execute(query, new
            {
                user.UserId,
                user.Login,
                user.Email,
                user.Password,
                user.Role,
                user.FirstName,
                user.LastName,
                user.Patronymic,
                user.BirthDate,
                user.Address,
                user.IsVip,
                user.IsBlocked
            });
        }

        public async Task<bool> IsUserAlreadyRegisteredAsync(string login, string email)
        {
            var query = @"
                SELECT 1 
                FROM [dbo].[user]
                WHERE 
                      [login] = @login
                   OR [email] = @email";
            
            await using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            return await connection.ExecuteScalarAsync<bool>(query, new
            {
                login, 
                email
            });
        }
    }
}