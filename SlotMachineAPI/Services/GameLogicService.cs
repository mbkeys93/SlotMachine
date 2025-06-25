using SlotMachineAPI.Data;
using SlotMachineAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace SlotMachineAPI.Services;

public interface IGameLogicService
{
    Task<Game> SpinAsync(int userId, decimal betAmount = 1.0m);
    Task AddFreeSpinsAsync(int userId, int count = 10);
    Task<decimal> CashoutAsync(int userId);
    Task<User?> GetUserAsync(int userId);
    Task<User?> GetUserByUsernameAsync(string userName);
    Task<List<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(string userName);
}

public class GameLogicService : IGameLogicService
{
    private readonly SlotMachineDbContext _context;
    private readonly Random _random = new Random();
    
    public GameLogicService(SlotMachineDbContext context)
    {
        _context = context;
    }
    
    public async Task<Game> SpinAsync(int userId, decimal betAmount = 1.0m)
    {
        var user = await _context.Users
            .Include(u => u.Games)
            .FirstOrDefaultAsync(u => u.UserId == userId);
            
        if (user == null)
            throw new ArgumentException("User not found");
            
        if (!user.CanPlay())
            throw new InvalidOperationException("User cannot play - insufficient balance and no free spins");
            
        // Determine if using free spin or regular bet
        bool useFreeSpin = user.FreeSpins > 0;
        
        // Generate spin result
        var spinResult = await GenerateSpinResultAsync();
        
        // Create game record
        var game = new Game
        {
            UserId = userId,
            SpinResult = spinResult,
            BetAmount = betAmount,
            UsedFreeSpin = useFreeSpin,
            IsWin = CheckWin(spinResult),
            SpinDateTime = DateTime.UtcNow
        };
        
        // Calculate win amount
        game.WinAmount = await CalculateWinAmountAsync(game);
        
        // Check for Bonus symbols and award free spins
        var bonusCount = CountBonusSymbols(spinResult);
        if (bonusCount > 0)
        {
            var freeSpinsToAdd = bonusCount == 3 ? 10 : bonusCount * 5; // 10 free spins for 3 bonus, 5 per bonus otherwise
            user.AddFreeSpins(freeSpinsToAdd);
        }
        
        // Update user state
        if (useFreeSpin)
        {
            user.FreeSpins--;
        }
        else
        {
            user.Balance -= betAmount;
        }
        
        // Add winnings to balance
        user.Balance += game.WinAmount;
        
        // Save to database
        _context.Games.Add(game);
        await _context.SaveChangesAsync();
        
        return game;
    }
    
    public async Task AddFreeSpinsAsync(int userId, int count = 10)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new ArgumentException("User not found");
            
        user.AddFreeSpins(count);
        await _context.SaveChangesAsync();
    }
    
    public async Task<decimal> CashoutAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new ArgumentException("User not found");
            
        var cashoutAmount = user.Cashout();
        await _context.SaveChangesAsync();
        return cashoutAmount;
    }
    
    public async Task<User?> GetUserAsync(int userId)
    {
        return await _context.Users
            .Include(u => u.Games.OrderByDescending(g => g.SpinDateTime).Take(10))
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }
    
    public async Task<User?> GetUserByUsernameAsync(string userName)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
    }
    
    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }
    
    public async Task<User> CreateUserAsync(string userName)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        if (existingUser != null)
        {
            // Return existing user instead of throwing exception
            return existingUser;
        }
            
        var user = new User 
        { 
            UserName = userName
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    private async Task<string> GenerateSpinResultAsync()
    {
        var result = new string[3];
        
        for (int i = 0; i < 3; i++)
        {
            var symbol = await GetRandomWeightedSymbolAsync();
            result[i] = symbol.Name;
        }
        
        return string.Join(",", result);
    }
    
    private async Task<Symbol> GetRandomWeightedSymbolAsync()
    {
        // Get all symbols from database
        var symbols = await _context.Symbols.ToListAsync();
        
        if (!symbols.Any())
            throw new InvalidOperationException("No symbols found in database");
        
        // Calculate total weight
        int totalWeight = symbols.Sum(s => s.Weight);
        
        // Generate random number between 1 and total weight
        int randomValue = _random.Next(1, totalWeight + 1);
        
        // Find which symbol corresponds to this random value
        int currentWeight = 0;
        foreach (var symbol in symbols)
        {
            currentWeight += symbol.Weight;
            if (randomValue <= currentWeight)
            {
                return symbol;
            }
        }
        
        // Fallback to first symbol (should never reach here)
        return symbols.First();
    }
    
    private bool CheckWin(string spinResult)
    {
        var symbols = spinResult.Split(',');
        return symbols.Length == 3 && symbols[0] == symbols[1] && symbols[1] == symbols[2];
    }
    
    private async Task<decimal> CalculateWinAmountAsync(Game game)
    {
        if (!game.IsWin || string.IsNullOrEmpty(game.SpinResult))
            return 0.0m;
            
        var symbolName = game.SpinResult.Split(',')[0];
        
        // Find the symbol in the database
        var symbol = await _context.Symbols.FirstOrDefaultAsync(s => s.Name == symbolName);
        
        if (symbol != null)
        {
            return symbol.Value * game.BetAmount;
        }
        
        return 0.0m;
    }

    private int CountBonusSymbols(string spinResult)
    {
        var symbols = spinResult.Split(',');
        return symbols.Count(s => s == "Bonus");
    }
} 