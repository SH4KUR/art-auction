using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Handlers;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Application.Interfaces.Services;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Tests.Base.Attributes;
using ArtAuction.Tests.Base.Extensions;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Xunit;

namespace ArtAuction.Core.UnitTests.Handlers
{
    public class RegisterUserCommandHandlerTests
    {
        [Theory, MockAutoData]
        public async Task handler_returns_false_if_user_with_same_login_or_email_already_registered(
            [Frozen] IUserRepository userRepository,
            RegisterUserCommand request,
            RegisterUserCommandHandler sut
        )
        {
            // Arrange
            userRepository.AsMock()
                .Setup(r => r.IsUserAlreadyRegisteredAsync(request.Login, request.Email))
                .ReturnsAsync(true);

            // Act
           var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Theory, MockAutoData]
        public async Task handler_returns_true_if_user_was_registered(
            string passwordHash,
            [Frozen] IUserRepository userRepository,
            [Frozen] IPasswordService passwordService,
            RegisterUserCommand request,
            RegisterUserCommandHandler sut
        )
        {
            // Arrange
            userRepository.AsMock()
                .Setup(r => r.IsUserAlreadyRegisteredAsync(request.Login, request.Email))
                .ReturnsAsync(false);

            userRepository.AsMock()
                .Setup(r => r.AddUserAsync(It.IsAny<User>()))
                .Callback((User _) => { });

            passwordService.AsMock()
                .Setup(s => s.GetHash(request.Password))
                .Returns(passwordHash);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Theory, InlineMockAutoData]
        public async Task handler_adds_new_user_correctly(
            string passwordHash,
            [Frozen] IUserRepository userRepository,
            [Frozen] IPasswordService passwordService,
            RegisterUserCommand request,
            RegisterUserCommandHandler sut
        )
        {
            // Arrange
            userRepository.AsMock()
                .Setup(r => r.IsUserAlreadyRegisteredAsync(request.Login, request.Email))
                .ReturnsAsync(false);
            
            User addedUser = null;
            userRepository.AsMock()
                .Setup(r => r.AddUserAsync(It.IsAny<User>()))
                .Callback((User u) => addedUser = u);
            
            passwordService.AsMock()
                .Setup(s => s.GetHash(request.Password))
                .Returns(passwordHash);

            // Act
            await sut.Handle(request, CancellationToken.None);

            // Assert
            addedUser.Should().NotBeNull();
            addedUser.Login.Should().Be(request.Login);
            addedUser.Email.Should().Be(request.Email);
            addedUser.Address.Should().Be(request.Address);
            addedUser.FirstName.Should().Be(request.FirstName);
            addedUser.LastName.Should().Be(request.LastName);
            addedUser.Patronymic.Should().Be(request.Patronymic);
            addedUser.Password.Should().Be(passwordHash);
            addedUser.BirthDate.Date.Should().Be(request.BirthDate.Date);
            addedUser.Role.Should().Be(request.Role);
            addedUser.IsBlocked.Should().BeFalse();
            addedUser.IsVip.Should().BeFalse();
        }
    }
}