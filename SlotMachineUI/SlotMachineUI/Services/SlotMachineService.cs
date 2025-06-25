using System.Text.Json;
using SlotMachineUI.Models;

namespace SlotMachineUI.Services;

public interface ISlotMachineService
{
    Task<User?> CreateUserAsync(string userName);
    Task<User?> GetUserAsync(int userId);
    Task<User?> GetUserByUsernameAsync(string userName);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> AddCashAsync(int userId, decimal amount);
    Task<User?> AddFreeSpinsAsync(int userId, int count);
    Task<Game?> SpinAsync(int userId, decimal betAmount);
    Task<List<Game>> GetGameHistoryAsync(int userId, int limit = 10);
    Task<List<Symbol>> GetSymbolsAsync();
    Task<SymbolStatistics> GetSymbolStatisticsAsync();
    Task<decimal> CashoutAsync(int userId);
}

public class SlotMachineService : ISlotMachineService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public SlotMachineService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<User?> CreateUserAsync(string userName)
    {
        try
        {
            var request = new { userName };
            var response = await _httpClient.PostAsJsonAsync("api/users", request);
            
            if (response.IsSuccessStatusCode)
            {
                var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>(_jsonOptions);
                return userResponse != null ? MapToUser(userResponse) : null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Service Error: {ex.Message}");
        }
        
        return null;
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        var response = await _httpClient.GetAsync($"api/users/id/{userId}");
        
        if (response.IsSuccessStatusCode)
        {
            var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>(_jsonOptions);
            return userResponse != null ? MapToUser(userResponse) : null;
        }
        
        return null;
    }

    public async Task<User?> GetUserByUsernameAsync(string userName)
    {
        var response = await _httpClient.GetAsync($"api/users/username/{userName}");
        
        if (response.IsSuccessStatusCode)
        {
            var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>(_jsonOptions);
            return userResponse != null ? MapToUser(userResponse) : null;
        }
        
        return null;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var response = await _httpClient.GetAsync("api/users");
        
        if (response.IsSuccessStatusCode)
        {
            var userResponses = await response.Content.ReadFromJsonAsync<List<UserResponse>>(_jsonOptions);
            return userResponses?.Select(MapToUser).ToList() ?? new List<User>();
        }
        
        return new List<User>();
    }

    public async Task<User?> AddCashAsync(int userId, decimal amount)
    {
        var request = new { amount };
        var response = await _httpClient.PostAsJsonAsync($"api/users/{userId}/balance", request);
        
        if (response.IsSuccessStatusCode)
        {
            var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>(_jsonOptions);
            return userResponse != null ? MapToUser(userResponse) : null;
        }
        
        return null;
    }

    public async Task<User?> AddFreeSpinsAsync(int userId, int count)
    {
        var request = new { count };
        var response = await _httpClient.PostAsJsonAsync($"api/users/{userId}/free-spins", request);
        
        if (response.IsSuccessStatusCode)
        {
            var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>(_jsonOptions);
            return userResponse != null ? MapToUser(userResponse) : null;
        }
        
        return null;
    }

    public async Task<Game?> SpinAsync(int userId, decimal betAmount)
    {
        try
        {
            var request = new { betAmount };
            var response = await _httpClient.PostAsJsonAsync($"api/games/{userId}/spin", request);
            
            if (response.IsSuccessStatusCode)
            {
                var spinResponse = await response.Content.ReadFromJsonAsync<SpinResponse>(_jsonOptions);
                return spinResponse != null ? MapToGame(spinResponse, userId) : null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Spin Service Error: {ex.Message}");
        }
        
        return null;
    }

    public async Task<List<Game>> GetGameHistoryAsync(int userId, int limit = 10)
    {
        var response = await _httpClient.GetAsync($"api/games/{userId}/history?limit={limit}");
        
        if (response.IsSuccessStatusCode)
        {
            var gameResponses = await response.Content.ReadFromJsonAsync<List<GameHistoryResponse>>(_jsonOptions);
            return gameResponses?.Select(gr => MapToGame(gr, userId)).ToList() ?? new List<Game>();
        }
        
        return new List<Game>();
    }

    public async Task<List<Symbol>> GetSymbolsAsync()
    {
        var response = await _httpClient.GetAsync("api/symbols");
        
        if (response.IsSuccessStatusCode)
        {
            var symbolResponses = await response.Content.ReadFromJsonAsync<List<SymbolResponse>>(_jsonOptions);
            return symbolResponses?.Select(MapToSymbol).ToList() ?? new List<Symbol>();
        }
        
        return new List<Symbol>();
    }

    public async Task<SymbolStatistics> GetSymbolStatisticsAsync()
    {
        var response = await _httpClient.GetAsync("api/symbols/statistics");
        
        if (response.IsSuccessStatusCode)
        {
            var statsResponse = await response.Content.ReadFromJsonAsync<SymbolStatisticsResponse>(_jsonOptions);
            return statsResponse != null ? MapToSymbolStatistics(statsResponse) : new SymbolStatistics();
        }
        
        return new SymbolStatistics();
    }

    public async Task<decimal> CashoutAsync(int userId)
    {
        var response = await _httpClient.PostAsync($"api/users/{userId}/cashout", null);
        
        if (response.IsSuccessStatusCode)
        {
            var cashoutResponse = await response.Content.ReadFromJsonAsync<CashoutResponse>(_jsonOptions);
            return cashoutResponse?.Amount ?? 0m;
        }
        
        return 0m;
    }

    private static User MapToUser(UserResponse userResponse) => new()
    {
        UserId = userResponse.UserId,
        UserName = userResponse.UserName,
        Balance = userResponse.Balance,
        FreeSpins = userResponse.FreeSpins,
        Multiplier = userResponse.Multiplier,
        CanPlay = userResponse.CanPlay
    };

    private static Game MapToGame(SpinResponse spinResponse, int userId) => new()
    {
        GameId = spinResponse.GameId,
        SpinResult = spinResponse.SpinResult,
        BetAmount = spinResponse.BetAmount,
        WinAmount = spinResponse.WinAmount,
        IsWin = spinResponse.IsWin,
        UsedFreeSpin = spinResponse.UsedFreeSpin,
        SpinDateTime = spinResponse.SpinTime,
        UserId = userId
    };

    private static Game MapToGame(GameHistoryResponse gameResponse, int userId) => new()
    {
        GameId = gameResponse.GameId,
        SpinResult = gameResponse.SpinResult,
        BetAmount = gameResponse.BetAmount,
        WinAmount = gameResponse.WinAmount,
        IsWin = gameResponse.IsWin,
        UsedFreeSpin = gameResponse.UsedFreeSpin,
        SpinDateTime = gameResponse.SpinTime,
        UserId = userId
    };

    private static Symbol MapToSymbol(SymbolResponse symbolResponse) => new()
    {
        Id = symbolResponse.Id,
        Name = symbolResponse.Name,
        Value = symbolResponse.Value,
        Weight = symbolResponse.Weight
    };

    private static SymbolStatistics MapToSymbolStatistics(SymbolStatisticsResponse statsResponse) => new()
    {
        TotalWeight = statsResponse.TotalWeight,
        SymbolCount = statsResponse.SymbolCount,
        Symbols = statsResponse.Symbols.Select(MapToSymbol).ToList()
    };

    // Internal DTOs for API communication
    internal record CashoutResponse(decimal Amount);

    internal record SpinResponse
    {
        public int GameId { get; init; }
        public string SpinResult { get; init; } = string.Empty;
        public decimal BetAmount { get; init; }
        public decimal WinAmount { get; init; }
        public bool IsWin { get; init; }
        public bool UsedFreeSpin { get; init; }
        public DateTime SpinTime { get; init; }
        public UserResponse User { get; init; } = null!;
    }

    internal record UserResponse
    {
        public int UserId { get; init; }
        public string UserName { get; init; } = string.Empty;
        public decimal Balance { get; init; }
        public int FreeSpins { get; init; }
        public int Multiplier { get; init; }
        public bool CanPlay { get; init; }
    }

    internal record GameHistoryResponse
    {
        public int GameId { get; init; }
        public string SpinResult { get; init; } = string.Empty;
        public decimal BetAmount { get; init; }
        public decimal WinAmount { get; init; }
        public bool IsWin { get; init; }
        public bool UsedFreeSpin { get; init; }
        public DateTime SpinTime { get; init; }
    }

    internal record SymbolResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Value { get; init; }
        public int Weight { get; init; }
    }

    internal record SymbolStatisticsResponse
    {
        public int TotalWeight { get; init; }
        public int SymbolCount { get; init; }
        public List<SymbolResponse> Symbols { get; init; } = new();
    }
} 