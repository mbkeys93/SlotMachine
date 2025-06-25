using SlotMachineAPI.Models;
using Xunit;

namespace SlotMachineAPI.Tests;

public class GameModelTests
{
    [Fact]
    public void Game_DefaultValues_ShouldBeCorrect()
    {
        // Act
        var game = new Game();

        // Assert
        Assert.Equal(0, game.GameId);
        Assert.Equal(0, game.UserId);
        Assert.Null(game.User); // User is null when Game is created without being loaded from DB
        Assert.Equal(DateTime.UtcNow.Date, game.SpinDateTime.Date); // Should be today
        Assert.Equal(string.Empty, game.SpinResult);
        Assert.Equal(1.0m, game.BetAmount);
        Assert.Equal(0.0m, game.WinAmount);
        Assert.False(game.IsWin);
        Assert.False(game.UsedFreeSpin);
    }

    [Fact]
    public void Game_CheckWin_WithWinningResult_ShouldReturnTrue()
    {
        // Arrange
        var game = new Game
        {
            SpinResult = "Ace,Ace,Ace"
        };

        // Act
        var isWin = game.CheckWin();

        // Assert
        Assert.True(isWin);
    }

    [Fact]
    public void Game_CheckWin_WithNonWinningResult_ShouldReturnFalse()
    {
        // Arrange
        var game = new Game
        {
            SpinResult = "Ace,King,Queen"
        };

        // Act
        var isWin = game.CheckWin();

        // Assert
        Assert.False(isWin);
    }

    [Fact]
    public void Game_CheckWin_WithEmptyResult_ShouldReturnFalse()
    {
        // Arrange
        var game = new Game
        {
            SpinResult = ""
        };

        // Act
        var isWin = game.CheckWin();

        // Assert
        Assert.False(isWin);
    }

    [Fact]
    public void Game_CheckWin_WithNullResult_ShouldReturnFalse()
    {
        // Arrange
        var game = new Game
        {
            SpinResult = null!
        };

        // Act
        var isWin = game.CheckWin();

        // Assert
        Assert.False(isWin);
    }

    [Fact]
    public void Game_CheckWin_WithPartialMatch_ShouldReturnFalse()
    {
        // Arrange
        var game = new Game
        {
            SpinResult = "Ace,Ace,King"
        };

        // Act
        var isWin = game.CheckWin();

        // Assert
        Assert.False(isWin);
    }

    [Fact]
    public void Game_CheckWin_WithMoreThanThreeSymbols_ShouldReturnFalse()
    {
        // Arrange
        var game = new Game
        {
            SpinResult = "Ace,Ace,Ace,King"
        };

        // Act
        var isWin = game.CheckWin();

        // Assert
        Assert.False(isWin);
    }

    [Fact]
    public void Game_CheckWin_WithLessThanThreeSymbols_ShouldReturnFalse()
    {
        // Arrange
        var game = new Game
        {
            SpinResult = "Ace,Ace"
        };

        // Act
        var isWin = game.CheckWin();

        // Assert
        Assert.False(isWin);
    }

    [Fact]
    public void Game_Properties_ShouldBeSettable()
    {
        // Arrange
        var game = new Game
        {
            GameId = 123,
            UserId = 456,
            SpinResult = "Jack,Jack,Jack",
            BetAmount = 5.0m,
            WinAmount = 15.0m,
            IsWin = true,
            UsedFreeSpin = true
        };

        // Assert
        Assert.Equal(123, game.GameId);
        Assert.Equal(456, game.UserId);
        Assert.Equal("Jack,Jack,Jack", game.SpinResult);
        Assert.Equal(5.0m, game.BetAmount);
        Assert.Equal(15.0m, game.WinAmount);
        Assert.True(game.IsWin);
        Assert.True(game.UsedFreeSpin);
    }

    [Fact]
    public void Game_IsUserAbleToPlay_WithUserThatCanPlay_ShouldReturnTrue()
    {
        // Arrange
        var user = new User
        {
            Balance = 10.0m,
            Multiplier = 1,
            FreeSpins = 0
        };

        var game = new Game
        {
            User = user
        };

        // Act
        var canPlay = game.IsUserAbleToPlay;

        // Assert
        Assert.True(canPlay);
    }

    [Fact]
    public void Game_IsUserAbleToPlay_WithUserThatCannotPlay_ShouldReturnFalse()
    {
        // Arrange
        var user = new User
        {
            Balance = 0.0m,
            Multiplier = 1,
            FreeSpins = 0
        };

        var game = new Game
        {
            User = user
        };

        // Act
        var canPlay = game.IsUserAbleToPlay;

        // Assert
        Assert.False(canPlay);
    }

    [Fact]
    public void Game_IsUserAbleToPlay_WithNullUser_ShouldReturnFalse()
    {
        // Arrange
        var game = new Game
        {
            User = null!
        };

        // Act
        var canPlay = game.IsUserAbleToPlay;

        // Assert
        Assert.False(canPlay);
    }

    [Fact]
    public void Game_IsUserAbleToPlay_WithDefaultNullUser_ShouldReturnFalse()
    {
        // Arrange
        var game = new Game(); // User is null by default

        // Act
        var canPlay = game.IsUserAbleToPlay;

        // Assert
        Assert.False(canPlay);
    }
} 