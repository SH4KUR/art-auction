using System;
using System.Threading.Tasks;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Infrastructure.IntegrationTests.DataAttributes;
using ArtAuction.Infrastructure.Persistence;
using ArtAuction.Infrastructure.Persistence.Repositories;
using ArtAuction.Tests.Base;
using AutoFixture.Xunit2;
using Dapper;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ArtAuction.Infrastructure.IntegrationTests.Repositories
{
    public class UserRepositoryTests
    {
        private const string UserId = "{5C6EEC32-4ED7-4209-A765-03E50E440FC7}";
        private const string Login = "user_test_login";
        private const string Email = "user-repository.test@email.com";
        private const string BirthDate = "1999-01-01";
        private const string Password = "B97BDCB0B4D1802A2E727FBE143CC631935BCA83C60019E1733620C08916095B";

        [Fact]
        [UseUser(Login = Login)]
        public async Task repository_gets_user_by_login_correctly()
        {
            // Arrange
            var sut = new UserRepository(TestConfiguration.Get());
            
            // Act
            var result = await sut.GetUserAsync(Login);

            // Assert
            result.Should().NotBeNull();
            result.Login.Should().Be(Login);
        }

        [Fact]
        [UseUser(UserId = UserId, Login = Login)]
        public void repository_gets_user_by_user_id_correctly()
        {
            // Arrange
            var sut = new UserRepository(TestConfiguration.Get());

            // Act
            var result = sut.GetUser(new Guid(UserId));

            // Assert
            result.Should().NotBeNull();
            result.Login.Should().Be(Login);
        }

        [Theory, AutoData]
        [UseUser(UserId = UserId, Login = Login, BirthDate = BirthDate)]
        public async Task repository_updates_user_correctly(
            string expectedLogin,
            DateTime expectedBirthDate,
            User user
        )
        {
            // Arrange
            var sut = new UserRepository(TestConfiguration.Get());
            
            user.UserId = new Guid(UserId);
            user.Login = expectedLogin;
            user.BirthDate = expectedBirthDate.Date;
            
            // Act
            sut.UpdateUser(user);

            // Assert
            var result = await GetUserAsync(expectedLogin);
            
            result.Should().NotBeNull();
            result.Login.Should().Be(expectedLogin);
            result.BirthDate.Should().Be(expectedBirthDate.Date);
        }

        [Theory, AutoData]
        [RemoveUserAfter(Login)]
        public async Task repository_adds_user_correctly(
            User user
        )
        {
            // Arrange
            var sut = new UserRepository(TestConfiguration.Get());

            user.Login = Login;
            user.Password = Password;

            // Act
            await sut.AddUserAsync(user);

            // Assert
            var result = await GetUserAsync(user.Login);

            result.Should().NotBeNull();
            result.Login.Should().Be(Login);
            result.Email.Should().Be(user.Email);
            result.Password.Should().Be(user.Password);
            result.Role.Should().Be(user.Role);
            result.FirstName.Should().Be(user.FirstName);
            result.LastName.Should().Be(user.LastName);
            result.Patronymic.Should().Be(user.Patronymic);
            result.BirthDate.Should().Be(user.BirthDate.Date);
            result.Address.Should().Be(user.Address);
            result.IsVip.Should().Be(user.IsVip);
            result.IsBlocked.Should().Be(user.IsBlocked);
        }

        [Theory, AutoData]
        [RemoveUserAfter(Login)]
        public async Task repository_adds_user_account_correctly(
            User user
        )
        {
            // Arrange
            var sut = new UserRepository(TestConfiguration.Get());

            user.Login = Login;
            user.Password = Password;

            // Act
            await sut.AddUserAsync(user);

            // Assert
            var result = await IsUserAccountAddedAsync(user.Login);

            result.Should().BeTrue();
        }

        [Theory, AutoData]
        [UseUser(Login = Login)]
        public async Task repository_returns_true_if_user_with_the_same_login_exists(
            string email
        )
        {
            // Arrange
            var sut = new UserRepository(TestConfiguration.Get());

            // Act
            var result = await sut.IsUserAlreadyRegisteredAsync(Login, email);

            // Assert
            result.Should().BeTrue();
        }

        [Theory, AutoData]
        [UseUser(Email = Email)]
        public async Task repository_returns_true_if_user_with_the_same_email_exists(
            string login
        )
        {
            // Arrange
            var sut = new UserRepository(TestConfiguration.Get());

            // Act
            var result = await sut.IsUserAlreadyRegisteredAsync(login, Email);

            // Assert
            result.Should().BeTrue();
        }

        private async Task<User> GetUserAsync(string login)
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
                    [login] = @login";

            await using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            return await connection.QueryFirstOrDefaultAsync<User>(query, new
            {
                login
            });
        }

        private async Task<bool> IsUserAccountAddedAsync(string login)
        {
            var query = @"
                SELECT 1 
                FROM [dbo].[user] AS u INNER JOIN [dbo].[account] AS a
                    ON u.[user_id] = a.[user_id]
                WHERE 
                    u.[login] = @login";

            await using var connection = new SqlConnection(TestConfiguration.Get().GetConnectionString(InfrastructureConstants.ArtAuctionDbConnection));
            return await connection.ExecuteScalarAsync<bool>(query, new
            {
                login
            });
        }
    }
}