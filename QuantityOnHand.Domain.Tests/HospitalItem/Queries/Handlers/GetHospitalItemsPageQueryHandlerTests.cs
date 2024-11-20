using Moq;
using QuantityOnHand.Data.Repositories.HospitalItems;
using QuantityOnHand.Domain.HospitalItem.Queries;
using QuantityOnHand.Domain.HospitalItem.Queries.Handlers;

namespace QuantityOnHand.Tests.HospitalItem.Queries.Handlers;

[TestFixture]
public class GetHospitalItemsPageQueryHandlerTests
{
    [SetUp]
    public void SetUp()
    {
        _hospitalItemRepositoryMock = new Mock<IHospitalItemRepository>();
        _handler = new GetHospitalItemsPageQueryHandler(_hospitalItemRepositoryMock.Object);
    }

    private Mock<IHospitalItemRepository> _hospitalItemRepositoryMock;
    private GetHospitalItemsPageQueryHandler _handler;

    [Test]
    public async Task Handle_ShouldReturnPagedEntityResponse_WhenDataIsAvailable()
    {
        // Arrange
        var query = new GetHospitalItemsPageQuery
            { ItemDescription = "Test", MinQuantity = 1, MaxQuantity = 10, PageNumber = 1, PageSize = 10 };
        var hospitalItems = new List<Data.Entities.HospitalItem>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ItemNo = "1",
                ItemDescription = "Test",
                Quantity = 10,
                Price = 10
            }
        };

        var pagedResult = (hospitalItems, hospitalItems.Count);

        _hospitalItemRepositoryMock
            .Setup(repo => repo.GetHospitalItemsPageAsync(query.ItemDescription, query.MinQuantity, query.MaxQuantity,
                query.PageNumber, query.PageSize))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Entities.Count, Is.EqualTo(pagedResult.Item1.Count));
            Assert.That(result.TotalCount, Is.EqualTo(pagedResult.Item2));
        });

        _hospitalItemRepositoryMock.Verify(repo => repo.GetHospitalItemsPageAsync(
                query.ItemDescription,
                query.MinQuantity,
                query.MaxQuantity,
                query.PageNumber,
                query.PageSize),
            Times.Once);
    }

    [Test]
    public async Task Handle_ShouldReturnEmptyPagedEntityResponse_WhenNoDataIsAvailable()
    {
        // Arrange
        var query = new GetHospitalItemsPageQuery
            { ItemDescription = "Test", MinQuantity = 1, MaxQuantity = 10, PageNumber = 1, PageSize = 10 };
        var pagedResult = (new List<Data.Entities.HospitalItem>(), 0);

        _hospitalItemRepositoryMock
            .Setup(repo => repo.GetHospitalItemsPageAsync(query.ItemDescription, query.MinQuantity, query.MaxQuantity,
                query.PageNumber, query.PageSize))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Entities.Count, Is.EqualTo(0));
            Assert.That(result.TotalCount, Is.EqualTo(0));
        });

        _hospitalItemRepositoryMock.Verify(repo => repo.GetHospitalItemsPageAsync(
                query.ItemDescription,
                query.MinQuantity,
                query.MaxQuantity,
                query.PageNumber,
                query.PageSize),
            Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenRepositoryThrowsException()
    {
        // Arrange
        var query = new GetHospitalItemsPageQuery
            { ItemDescription = "Test", MinQuantity = 1, MaxQuantity = 10, PageNumber = 1, PageSize = 10 };

        _hospitalItemRepositoryMock
            .Setup(repo => repo.GetHospitalItemsPageAsync(query.ItemDescription, query.MinQuantity, query.MaxQuantity,
                query.PageNumber, query.PageSize))
            .ThrowsAsync(new Exception("Repository error"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _handler.Handle(query, CancellationToken.None));

        _hospitalItemRepositoryMock.Verify(repo => repo.GetHospitalItemsPageAsync(
                query.ItemDescription,
                query.MinQuantity,
                query.MaxQuantity,
                query.PageNumber,
                query.PageSize),
            Times.Once);
    }
}