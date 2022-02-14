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

        public User GetUser(string login)
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

            using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            return connection.QueryFirstOrDefault<User>(query, new { UserLogin = login });
        }

        public void AddUser(User user)
        {
            var query = @"
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
                )";

            using var connection = new SqlConnection(_configuration.GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            connection.Execute(query, new
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
            });
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
    }
}