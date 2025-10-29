namespace SlotMachineUI.Models;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public int FreeSpins { get; set; }
    public int Multiplier { get; set; }
    public bool CanPlay { get; set; }
    public DateTime ModifiedDateTime { get; set; }
} 