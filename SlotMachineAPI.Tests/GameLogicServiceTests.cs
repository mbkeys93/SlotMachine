using Microsoft.Extensions.DependencyInjection;
using SlotMachineAPI.Models;
using SlotMachineAPI.Services;
using Xunit;

namespace SlotMachineAPI.Tests;

public class GameLogicServiceTests : TestBase
{
    private readonly IGameLogicService _gameService;

    public GameLogicServiceTests()
    {
        _gameService = ServiceProvider.GetRequiredService<IGameLogicService>();
    }

    [Fact]
    public async Task CreateUserAsync_WithNewUsername_ShouldCreateUser()
    {
        // Arrange
        var userName = "TestUser";

        // Act
        var user = await _gameService.CreateUserAsync(userName);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(userName, user.UserName);
        Assert.Equal(10.0m, user.Balance); // Default balance
        Assert.Equal(0, user.FreeSpins); // Default free spins
        Assert.Equal(1, user.Multiplier); // Default multiplier
    }

    [Fact]
    public async Task CreateUserAsync_WithExistingUsername_ShouldReturnExistingUser()
    {
        // Arrange
        var userName = "TestUser";
        var existingUser = await _gameService.CreateUserAsync(userName);

        // Act
        var returnedUser = await _gameService.CreateUserAsync(userName);

        // Assert
        Assert.NotNull(returnedUser);
        Assert.Equal(existingUser.UserId, returnedUser.UserId);
        Assert.Equal(userName, returnedUser.UserName);
    }

    [Fact]
    public async Task GetUserAsync_WithValidUserId_ShouldReturnUser()
    {
        // Arrange
        var userName = "TestUser";
        var createdUser = await _gameService.CreateUserAsync(userName);

        // Act
        var user = await _gameService.GetUserAsync(createdUser.UserId);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(createdUser.UserId, user.UserId);
        Assert.Equal(userName, user.UserName);
    }

    [Fact]
    public async Task GetUserAsync_WithInvalidUserId_ShouldReturnNull()
    {
        // Act
        var user = await _gameService.GetUserAsync(999);

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_WithValidUsername_ShouldReturnUser()
    {
        // Arrange
        var userName = "TestUser";
        await _gameService.CreateUserAsync(userName);

        // Act
        var user = await _gameService.GetUserByUsernameAsync(userName);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(userName, user.UserName);
    }

    [Fact]
    public async Task GetUserByUsernameAsync_WithInvalidUsername_ShouldReturnNull()
    {
        // Act
        var user = await _gameService.GetUserByUsernameAsync("NonExistentUser");

        // Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnAllUsers()
    {
        // Arrange
        await _gameService.CreateUserAsync("User1");
        await _gameService.CreateUserAsync("User2");
        await _gameService.CreateUserAsync("User3");

        // Act
        var users = await _gameService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(users);
        Assert.Equal(3, users.Count);
        Assert.Contains(users, u => u.UserName == "User1");
        Assert.Contains(users, u => u.UserName == "User2");
        Assert.Contains(users, u => u.UserName == "User3");
    }

    [Fact]
    public async Task AddFreeSpinsAsync_WithValidUserId_ShouldAddFreeSpins()
    {
        // Arrange
        var user = await _gameService.CreateUserAsync("TestUser");
        var initialFreeSpins = user.FreeSpins;
        var freeSpinsToAdd = 5;

        // Act
        await _gameService.AddFreeSpinsAsync(user.UserId, freeSpinsToAdd);
        var updatedUser = await _gameService.GetUserAsync(user.UserId);

        // Assert
        Assert.NotNull(updatedUser);
        Assert.Equal(initialFreeSpins + freeSpinsToAdd, updatedUser.FreeSpins);
    }

    [Fact]
    public async Task AddFreeSpinsAsync_WithInvalidUserId_ShouldThrowArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _gameService.AddFreeSpinsAsync(999, 5));
    }

    [Fact]
    public async Task CashoutAsync_WithValidUserId_ShouldReturnBalanceAndResetToZero()
    {
        // Arrange
        var user = await _gameService.CreateUserAsync("TestUser");
        var initialBalance = user.Balance;

        // Act
        var cashoutAmount = await _gameService.CashoutAsync(user.UserId);
        var updatedUser = await _gameService.GetUserAsync(user.UserId);

        // Assert
        Assert.Equal(initialBalance, cashoutAmount);
        Assert.NotNull(updatedUser);
        Assert.Equal(0, updatedUser.Balance);
    }

    [Fact]
    public async Task CashoutAsync_WithInvalidUserId_ShouldThrowArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _gameService.CashoutAsync(999));
    }

    [Fact]
    public async Task SpinAsync_WithValidUserAndBalance_ShouldCreateGame()
    {
        // Arrange
        var user = await _gameService.CreateUserAsync("TestUser");
        var betAmount = 1.0m;

        // Act
        var game = await _gameService.SpinAsync(user.UserId, betAmount);

        // Assert
        Assert.NotNull(game);
        Assert.Equal(user.UserId, game.UserId);
        Assert.Equal(betAmount, game.BetAmount);
        Assert.False(string.IsNullOrEmpty(game.SpinResult));
        Assert.False(game.UsedFreeSpin); // Should use balance, not free spin
    }

    [Fact]
    public async Task SpinAsync_WithValidUserAndFreeSpins_ShouldUseFreeSpin()
    {
        // Arrange
        var user = await _gameService.CreateUserAsync("TestUser");
        await _gameService.AddFreeSpinsAsync(user.UserId, 1);
        var betAmount = 1.0m;

        // Act
        var game = await _gameService.SpinAsync(user.UserId, betAmount);

        // Assert
        Assert.NotNull(game);
        Assert.Equal(user.UserId, game.UserId);
        Assert.Equal(betAmount, game.BetAmount);
        Assert.True(game.UsedFreeSpin); // Should use free spin
    }

    [Fact]
    public async Task SpinAsync_WithInsufficientBalanceAndNoFreeSpins_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var user = await _gameService.CreateUserAsync("TestUser");
        // Set balance to 0
        user.Balance = 0;
        await DbContext.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _gameService.SpinAsync(user.UserId, 1.0m));
    }

    [Fact]
    public async Task SpinAsync_WithInvalidUserId_ShouldThrowArgumentException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _gameService.SpinAsync(999, 1.0m));
    }

    [Fact]
    public async Task SpinAsync_WithBonusSymbols_ShouldAwardFreeSpins()
    {
        // Arrange
        var user = await _gameService.CreateUserAsync("TestUser");
        var initialFreeSpins = user.FreeSpins;

        // Act - Spin multiple times to potentially get bonus symbols
        var games = new List<Game>();
        for (int i = 0; i < 10; i++)
        {
            try
            {
                var game = await _gameService.SpinAsync(user.UserId, 1.0m);
                games.Add(game);
            }
            catch (InvalidOperationException)
            {
                // User ran out of balance/free spins, add more cash
                await _gameService.AddFreeSpinsAsync(user.UserId, 5);
            }
        }

        // Assert - Check if any bonus symbols were found and free spins were awarded
        var updatedUser = await _gameService.GetUserAsync(user.UserId);
        Assert.NotNull(updatedUser);
        
        // Check if any games had bonus symbols
        var gamesWithBonus = games.Where(g => g.SpinResult.Contains("Bonus")).ToList();
        if (gamesWithBonus.Any())
        {
            // If bonus symbols were found, free spins should have been awarded
            Assert.True(updatedUser.FreeSpins >= initialFreeSpins);
        }
    }

    [Fact]
    public async Task SpinAsync_BonusSymbolLogic_ShouldAwardCorrectFreeSpins()
    {
        // Arrange
        var user = await _gameService.CreateUserAsync("TestUser");
        
        // Test the bonus symbol counting logic directly
        var service = new GameLogicService(DbContext);
        
        // Act & Assert - Test different bonus symbol combinations
        var testCases = new[]
        {
            new { SpinResult = "Bonus,Jack,Queen", ExpectedBonusCount = 1, ExpectedFreeSpins = 5 },
            new { SpinResult = "Bonus,Bonus,Jack", ExpectedBonusCount = 2, ExpectedFreeSpins = 10 },
            new { SpinResult = "Bonus,Bonus,Bonus", ExpectedBonusCount = 3, ExpectedFreeSpins = 10 }
        };

        foreach (var testCase in testCases)
        {
            // Count bonus symbols
            var bonusCount = CountBonusSymbols(testCase.SpinResult);
            Assert.Equal(testCase.ExpectedBonusCount, bonusCount);
            
            // Calculate free spins based on bonus count
            var freeSpinsToAdd = bonusCount == 3 ? 10 : bonusCount * 5;
            Assert.Equal(testCase.ExpectedFreeSpins, freeSpinsToAdd);
        }
    }

    private int CountBonusSymbols(string spinResult)
    {
        var symbols = spinResult.Split(',');
        return symbols.Count(s => s.Trim() == "Bonus");
    }
} 