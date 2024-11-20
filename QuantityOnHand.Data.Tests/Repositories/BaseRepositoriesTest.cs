using Microsoft.EntityFrameworkCore;
using QuantityOnHand.Data.Entities;
using QuantityOnHand.Data.Repositories;

namespace QuantityOnHand.Data.Tests.Repositories;

[TestFixture]
public class BaseRepositoriesTest
{
    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new BaseRepository<HospitalItem>(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private ApplicationDbContext _context;
    private BaseRepository<HospitalItem> _repository;

    [Test]
    public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
    {
        // Arrange
        var hospitalItem = new HospitalItem
        {
            Id = Guid.NewGuid(),
            ItemNo = "Item001",
            ItemDescription = "Hospital Item 1",
            Quantity = 10,
            Price = 123.45m
        };
        _context.Set<HospitalItem>().Add(hospitalItem);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(hospitalItem.Id);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(hospitalItem.Id));
    }

    [Test]
    public async Task GetByIdAsync_ShouldReturnNull_WhenEntityDoesNotExist()
    {
        // Act
        var result = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetPageAsync_ShouldReturnEntitiesWithFilterApplied()
    {
        // Arrange
        var hospitalItems = new List<HospitalItem>
        {
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item001", ItemDescription = "Hospital Item Main 1", Quantity = 10,
                Price = 100
            },
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item002", ItemDescription = "Hospital Item Main 2", Quantity = 20,
                Price = 200
            },
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item003", ItemDescription = "Another Hospital Item", Quantity = 30,
                Price = 300
            }
        };
        await _context.Set<HospitalItem>().AddRangeAsync(hospitalItems);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPageAsync(e => e.ItemDescription.Contains("Hospital Item Main"), null, 1, 2);

        // Assert
        Assert.That(result.entities.Count, Is.EqualTo(2));
        Assert.That(result.totalCount, Is.EqualTo(2));
    }

    [Test]
    public async Task GetPageAsync_ShouldReturnEntitiesWithOrderApplied()
    {
        // Arrange
        var hospitalItems = new List<HospitalItem>
        {
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item003", ItemDescription = "Hospital Item 3", Quantity = 20, Price = 300
            },
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item001", ItemDescription = "Hospital Item 1", Quantity = 10, Price = 100
            },
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item002", ItemDescription = "Hospital Item 2", Quantity = 30, Price = 200
            }
        };
        await _context.Set<HospitalItem>().AddRangeAsync(hospitalItems);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPageAsync(null, q => q.OrderBy(e => e.ItemDescription), 1, 2);

        // Assert
        Assert.That(result.entities.Count, Is.EqualTo(2));
        Assert.That(result.totalCount, Is.EqualTo(3));
        Assert.That(result.entities.First().ItemDescription, Is.EqualTo("Hospital Item 1"));
    }

    [Test]
    public async Task GetPageAsync_ShouldReturnEntitiesWithoutFilterOrOrder()
    {
        // Arrange
        var hospitalItems = new List<HospitalItem>
        {
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item001", ItemDescription = "Hospital Item 1", Quantity = 10, Price = 100
            },
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item002", ItemDescription = "Hospital Item 2", Quantity = 20, Price = 200
            },
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item003", ItemDescription = "Hospital Item 3", Quantity = 30, Price = 300
            },
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item004", ItemDescription = "Hospital Item 4", Quantity = 40, Price = 400
            },
            new()
            {
                Id = Guid.NewGuid(), ItemNo = "Item005", ItemDescription = "Hospital Item 5", Quantity = 50, Price = 500
            }
        };
        await _context.Set<HospitalItem>().AddRangeAsync(hospitalItems);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetPageAsync(null, null, 2, 2);

        // Assert
        Assert.That(result.entities.Count, Is.EqualTo(2));
        Assert.That(result.totalCount, Is.EqualTo(5));
    }
}