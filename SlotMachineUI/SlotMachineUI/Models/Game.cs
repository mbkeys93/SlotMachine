namespace SlotMachineUI.Models;

public class Game
{
    public int GameId { get; set; }
    public int UserId { get; set; }
    public DateTime SpinDateTime { get; set; }
    public string SpinResult { get; set; } = string.Empty;
    public decimal BetAmount { get; set; }
    public decimal WinAmount { get; set; }
    public bool IsWin { get; set; }
    public bool UsedFreeSpin { get; set; }
} 