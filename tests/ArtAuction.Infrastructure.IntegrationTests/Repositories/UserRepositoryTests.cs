using System;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Infrastructure.IntegrationTests.DataAttributes;
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
        private const string BirthDate = "1999-01-01";
        private const string Password = "B97BDCB0B4D1802A2E727FBE143CC631935BCA83C60019E1733620C08916095B";

        [Fact]
        [UseUser(Login = Login)]
        public void repository_gets_user_correctly()
        {
            // Arrange
            var sut = new UserRepository(IntegrationTestsConfig.Get());
            
            // Act
            var result = sut.GetUser(Login);

            // Assert
            result.Should().NotBeNull();
            result.Login.Should().Be(Login);
        }

        [Theory, AutoData]
        [UseUser(UserId = UserId, Login = Login, BirthDate = BirthDate)]
        public void repository_updates_user_correctly(
            string expectedLogin,
            DateTime expectedBirthDate,
            User user
        )
        {
            // Arrange
            var sut = new UserRepository(IntegrationTestsConfig.Get());
            
            user.UserId = new Guid(UserId);
            user.Login = expectedLogin;
            user.BirthDate = expectedBirthDate.Date;
            
            // Act
            sut.UpdateUser(user);

            // Assert
            var result = GetUser(expectedLogin);
            
            result.Should().NotBeNull();
            result.Login.Should().Be(expectedLogin);
            result.BirthDate.Should().Be(expectedBirthDate.Date);
        }

        [Theory, AutoData]
        public void repository_adds_user_correctly(
            User user
        )
        {
            try
            {
                // Arrange
                var sut = new UserRepository(IntegrationTestsConfig.Get());

                user.Password = Password;
                
                // Act
                sut.AddUser(user);

                // Assert
                var result = GetUser(user.Login);

                result.Should().NotBeNull();
                result.Login.Should().Be(user.Login);
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
            catch
            {
                using var connection = new SqlConnection(IntegrationTestsConfig.Get().GetConnectionString("ArtAuctionDbConnection"));
                connection.Execute("DELETE FROM [dbo].[user] WHERE [login] = @Login", new { user.Login });
                
                throw;
            }
        }

        private User GetUser(string login)
        {
            var query = @"
                SELECT 
                    [user_id] AS UserId,
                    [login],
                    [email],
                    [password],
                    [role],
                    [first_name] AS FirstName,
                    [last_name] AS LastName,
                    [patronymic],
                    [birth_date] AS BirthDate,
                    [address],
                    [is_vip] AS IsVip,
                    [is_blocked] AS IsBlocked
                FROM [dbo].[user]
                WHERE 
                    [login] = @login";

            using var connection = new SqlConnection(IntegrationTestsConfig.Get().GetConnectionString("ArtAuctionDbConnection"));
            return connection.QueryFirstOrDefault<User>(query, new { login });
        }
    }
}