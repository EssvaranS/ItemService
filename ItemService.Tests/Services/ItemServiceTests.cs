using ItemService.Application.DTOs;
using ItemService.Application.IServices;
using ItemService.Domain.Entities;
using ItemService.Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;
using Moq;
using MongoDB.Driver;
using ItemService.Infrastructure.IRepositories;
using ItemService.Application.Services;
public class ItemServiceTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<IItemRepository> _itemRepoMock;
    private readonly Mock<ILogger<ItemServices>> _loggerMock;
    private readonly IItemService _service;

    public ItemServiceTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _itemRepoMock = new Mock<IItemRepository>();
        _loggerMock = new Mock<ILogger<ItemServices>>();
        _uowMock.SetupGet(u => u.Items).Returns(_itemRepoMock.Object);
        _service = new ItemServices(_uowMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateItem()
    {
        var dto = new CreateItemDto("Test", "Desc", 10);
        _itemRepoMock.Setup(r => r.AddAsync(It.IsAny<Item>())).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.BeginTransactionAsync(default)).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.CommitAsync(default)).Returns(Task.CompletedTask);
        var result = await _service.CreateAsync(dto);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Description, result.Description);
        Assert.Equal(dto.Price, result.Price);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllItems()
    {
        var items = new List<Item> {
            new Item { Id = "1", Name = "A", Description = "D", Price = 1 },
            new Item { Id = "2", Name = "B", Description = "E", Price = 2 }
        };
        _itemRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(items);
        var result = await _service.GetAllAsync();
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnItem_WhenExists()
    {
        var item = new Item { Id = "1", Name = "A", Description = "D", Price = 1 };
        _itemRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(item);
        var result = await _service.GetByIdAsync("1");
        Assert.NotNull(result);
        Assert.Equal("A", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        _itemRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync((Item)null);
        var result = await _service.GetByIdAsync("1");
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateItem_WhenExists()
    {
        var dto = new UpdateItemDto("New", null, null);
        _itemRepoMock.Setup(r => r.PatchAsync("1", It.IsAny<UpdateDefinition<Item>>())).ReturnsAsync(true);
        _itemRepoMock.Setup(r => r.GetByIdAsync("1")).ReturnsAsync(new Item { Id = "1", Name = "New", Description = "D", Price = 1 });
        _uowMock.Setup(u => u.BeginTransactionAsync(default)).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.CommitAsync(default)).Returns(Task.CompletedTask);
        var result = await _service.UpdateAsync("1", dto);
        Assert.NotNull(result);
        Assert.Equal("New", result.Name);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenNotExists()
    {
        var dto = new UpdateItemDto("New", null, null);
        _itemRepoMock.Setup(r => r.PatchAsync("1", It.IsAny<UpdateDefinition<Item>>())).ReturnsAsync(false);
        _uowMock.Setup(u => u.BeginTransactionAsync(default)).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
        var result = await _service.UpdateAsync("1", dto);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteItem_WhenExists()
    {
        _itemRepoMock.Setup(r => r.DeleteAsync("1")).ReturnsAsync(true);
        _uowMock.Setup(u => u.BeginTransactionAsync(default)).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.CommitAsync(default)).Returns(Task.CompletedTask);
        var result = await _service.DeleteAsync("1");
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenNotExists()
    {
        _itemRepoMock.Setup(r => r.DeleteAsync("1")).ReturnsAsync(false);
        _uowMock.Setup(u => u.BeginTransactionAsync(default)).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.RollbackAsync()).Returns(Task.CompletedTask);
        var result = await _service.DeleteAsync("1");
        Assert.False(result);
    }
}
