using SlotMachineAPI.Models;
using Xunit;

namespace SlotMachineAPI.Tests;

public class UserModelTests
{
    [Fact]
    public void User_DefaultValues_ShouldBeCorrect()
    {
        // Act
        var user = new User();

        // Assert
        Assert.Equal(0, user.UserId);
        Assert.Equal(string.Empty, user.UserName);
        Assert.Equal(10.0m, user.Balance); // Default balance
        Assert.Equal(0, user.FreeSpins); // Default free spins
        Assert.Equal(1, user.Multiplier); // Default multiplier
        Assert.NotNull(user.Games);
        Assert.Empty(user.Games);
    }

    [Fact]
    public void User_CanPlay_WithSufficientBalance_ShouldReturnTrue()
    {
        // Arrange
        var user = new User
        {
            Balance = 10.0m,
            Multiplier = 1,
            FreeSpins = 0
        };

        // Act
        var canPlay = user.CanPlay();

        // Assert
        Assert.True(canPlay);
    }

    [Fact]
    public void User_CanPlay_WithInsufficientBalanceButFreeSpins_ShouldReturnTrue()
    {
        // Arrange
        var user = new User
        {
            Balance = 0.0m,
            Multiplier = 1,
            FreeSpins = 5
        };

        // Act
        var canPlay = user.CanPlay();

        // Assert
        Assert.True(canPlay);
    }

    [Fact]
    public void User_CanPlay_WithInsufficientBalanceAndNoFreeSpins_ShouldReturnFalse()
    {
        // Arrange
        var user = new User
        {
            Balance = 0.0m,
            Multiplier = 1,
            FreeSpins = 0
        };

        // Act
        var canPlay = user.CanPlay();

        // Assert
        Assert.False(canPlay);
    }

    [Fact]
    public void User_CanPlay_WithLowBalanceButHighMultiplier_ShouldReturnTrue()
    {
        // Arrange
        var user = new User
        {
            Balance = 0.5m,
            Multiplier = 2, // 0.5 * 2 = 1.0, which is sufficient
            FreeSpins = 0
        };

        // Act
        var canPlay = user.CanPlay();

        // Assert
        Assert.True(canPlay);
    }

    [Fact]
    public void User_AddCash_ShouldIncreaseBalance()
    {
        // Arrange
        var user = new User { Balance = 10.0m };
        var amountToAdd = 5.0m;
        var expectedBalance = 15.0m;

        // Act
        user.AddCash(amountToAdd);

        // Assert
        Assert.Equal(expectedBalance, user.Balance);
    }

    [Fact]
    public void User_AddFreeSpins_ShouldIncreaseFreeSpins()
    {
        // Arrange
        var user = new User { FreeSpins = 2 };
        var spinsToAdd = 3;
        var expectedFreeSpins = 5;

        // Act
        user.AddFreeSpins(spinsToAdd);

        // Assert
        Assert.Equal(expectedFreeSpins, user.FreeSpins);
    }

    [Fact]
    public void User_AddFreeSpins_WithDefaultCount_ShouldAdd10Spins()
    {
        // Arrange
        var user = new User { FreeSpins = 5 };
        var expectedFreeSpins = 15; // 5 + 10 (default)

        // Act
        user.AddFreeSpins(); // Uses default count of 10

        // Assert
        Assert.Equal(expectedFreeSpins, user.FreeSpins);
    }

    [Fact]
    public void User_Cashout_ShouldReturnBalanceAndResetToZero()
    {
        // Arrange
        var user = new User { Balance = 25.50m };
        var expectedCashoutAmount = 25.50m;

        // Act
        var cashoutAmount = user.Cashout();

        // Assert
        Assert.Equal(expectedCashoutAmount, cashoutAmount);
        Assert.Equal(0, user.Balance);
    }

    [Fact]
    public void User_Cashout_WithZeroBalance_ShouldReturnZero()
    {
        // Arrange
        var user = new User { Balance = 0.0m };

        // Act
        var cashoutAmount = user.Cashout();

        // Assert
        Assert.Equal(0, cashoutAmount);
        Assert.Equal(0, user.Balance);
    }

    [Fact]
    public void User_Properties_ShouldBeSettable()
    {
        // Arrange
        var user = new User
        {
            UserId = 123,
            UserName = "TestUser",
            Balance = 100.0m,
            FreeSpins = 5,
            Multiplier = 3
        };

        // Assert
        Assert.Equal(123, user.UserId);
        Assert.Equal("TestUser", user.UserName);
        Assert.Equal(100.0m, user.Balance);
        Assert.Equal(5, user.FreeSpins);
        Assert.Equal(3, user.Multiplier);
    }
} 