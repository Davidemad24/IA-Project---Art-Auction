using Xunit;
using Moq;
using ArtAuction.Application.Services;
using ArtAuction.Application.Interfaces.Repositories;
using ArtAuction.Application.Interfaces.Services;
using ArtAuction.Application.Entities;
using AutoMapper;

public class AuctionServicesTests
{
    private readonly Mock<IPostSoldRepo> _postSoldRepo = new();
    private readonly Mock<IEmailServices> _email = new();
    private readonly Mock<INotificationServices> _notification = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<IBuyerRepo> _buyerRepo = new();
    private readonly Mock<IArtworkPostRepo> _artworkRepo = new();
    private readonly Mock<IPostBidRepo> _bidRepo = new();

    private AuctionServices CreateService()
    {
        return new AuctionServices(
            _postSoldRepo.Object,
            _email.Object,
            _notification.Object,
            _mapper.Object,
            _buyerRepo.Object,
            _artworkRepo.Object,
            _bidRepo.Object
        );
    }

    [Fact]
    public async Task Should_Send_Email_Notification_And_Create_PostSold_When_Auction_Ends()
    {
        // Arrange
        _bidRepo.Setup(x => x.GetTopBid(It.IsAny<int>()))
            .ReturnsAsync(new PostBid
            {
                ArtworkPostId = 1,
                BuyerId = 10,
                BuyerPrice = 500
            });

        _artworkRepo.Setup(x => x.GetArtworkPost(It.IsAny<int>()))
            .ReturnsAsync(new ArtworkPost
            {
                Id = 1,
                Title = "Test Artwork",
                EndDate = DateTime.UtcNow.AddSeconds(-10)
            });

        _buyerRepo.Setup(x => x.GetBuyerById(It.IsAny<int>()))
            .ReturnsAsync(new Buyer
            {
                Id = 10,
                Name = "Ali",
                Email = "ali@test.com"
            });

        _postSoldRepo.Setup(x => x.CreatePostSold(It.IsAny<PostSold>()))
            .ReturnsAsync(true);

        var service = CreateService();

        // Act
        await service.DetermineWinner(1);

        // Assert
        _notification.Verify(x =>
                x.SendToUserAsync("10", It.IsAny<object>()),
            Times.Once);

        _email.Verify(x =>
                x.SendEmail("ali@test.com", It.IsAny<string>(), It.IsAny<string>()),
            Times.Once);

        _postSoldRepo.Verify(x =>
                x.CreatePostSold(It.IsAny<PostSold>()),
            Times.Once);
    }
}