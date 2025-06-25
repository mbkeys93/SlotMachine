namespace SlotMachineAPI.DTOs;

public record SpinRequest(decimal BetAmount = 1.0m);

public record SpinResponse
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

public record GameHistoryResponse
{
    public int GameId { get; init; }
    public string SpinResult { get; init; } = string.Empty;
    public decimal BetAmount { get; init; }
    public decimal WinAmount { get; init; }
    public bool IsWin { get; init; }
    public bool UsedFreeSpin { get; init; }
    public DateTime SpinTime { get; init; }
} 