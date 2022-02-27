using System.Threading;
using System.Threading.Tasks;
using ArtAuction.Core.Application.Commands;
using ArtAuction.Core.Application.Handlers;
using ArtAuction.Core.Application.Interfaces.Repositories;
using ArtAuction.Core.Domain.Entities;
using ArtAuction.Tests.Base.Attributes;
using ArtAuction.Tests.Base.Extensions;
using AutoFixture.Xunit2;
using FluentAssertions;
using Moq;
using Xunit;

namespace ArtAuction.Core.UnitTests.Handlers
{
    public class CreateAuctionCommandHandlerTests
    {
        [Theory, MockAutoData]
        public async Task handler_returns_false_if_user_with_requested_login_was_not_founded(
            CreateAuctionCommand request,
            [Frozen] IUserRepository userRepository,
            [Frozen] IAuctionRepository auctionRepository,
            CreateAuctionCommandHandler sut
        )
        {
            // Arrange
            userRepository.AsMock()
                .Setup(r => r.GetUserAsync(request.SellerLogin))
                .ReturnsAsync((User)null);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }

        [Theory, MockAutoData]
        public async Task handler_adds_new_auction_correctly(
            User user,
            [Frozen] IUserRepository userRepository,
            [Frozen] IAuctionRepository auctionRepository,
            CreateAuctionCommand request,
            CreateAuctionCommandHandler sut
        )
        {
            // Arrange
            userRepository.AsMock()
                .Setup(r => r.GetUserAsync(request.SellerLogin))
                .ReturnsAsync(user);
            
            Auction auction = null;
            auctionRepository.AsMock()
                .Setup(r => r.AddAuctionAsync(It.IsAny<Auction>()))
                .Callback((Auction a) => auction = a);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            auction.Should().NotBeNull();

            auction.SellerId.Should().Be(user.UserId);

            auction.StartPrice.Should().Be(request.StartPrice);
            auction.CurrentPrice.Should().Be(request.StartPrice);
            auction.FullPrice.Should().Be(request.FullPrice);
            auction.BidStep.Should().Be(request.BidStep);
            
            auction.StartBillingDateTime.Should().Be(request.StartBillingDate);
            auction.EndBillingDateTime.Should().Be(request.EndBillingDate);
        }

        [Theory, MockAutoData]
        public async Task handler_adds_new_lot_correctly(
            User user,
            [Frozen] IUserRepository userRepository,
            [Frozen] IAuctionRepository auctionRepository,
            CreateAuctionCommand request,
            CreateAuctionCommandHandler sut
        )
        {
            // Arrange
            userRepository.AsMock()
                .Setup(r => r.GetUserAsync(request.SellerLogin))
                .ReturnsAsync(user);

            Auction auction = null;
            auctionRepository.AsMock()
                .Setup(r => r.AddAuctionAsync(It.IsAny<Auction>()))
                .Callback((Auction a) => auction = a);

            // Act
            var result = await sut.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            auction.Should().NotBeNull();
            
            var lot = auction.Lot;
            lot.Should().NotBeNull();
            
            lot.Category.Should().NotBeNull();
            lot.Category.Name.Should().Be(request.CategoryName);
            
            lot.PaintingDate.Should().Be(request.PaintingDate);
            lot.Photo.Should().Be(request.Photo);
            lot.Description.Should().Be(request.Description);
        }
    }
}