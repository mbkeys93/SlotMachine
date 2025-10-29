namespace SlotMachineAPI.DTOs;

public record CreateUserRequest(string UserName);

public record UserResponse
{
    public int UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public decimal Balance { get; init; }
    public int FreeSpins { get; init; }
    public int Multiplier { get; init; }
    public bool CanPlay { get; init; }
    public DateTime ModifiedDateTime { get; init; }
}

public record AddCashRequest(decimal Amount);

public record AddFreeSpinsRequest(int Count = 10);

public record CashoutResponse(decimal Amount); 