using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlotMachineAPI.Models;

public class Game
{
    [Key]
    public int GameId { get; set; }
    
    public int UserId { get; set; }

    // virtual allows lazy loading of the user object   
    // null! is a non-nullable reference type
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
    
    public DateTime SpinDateTime { get; set; } = DateTime.UtcNow;
    
    // Store spin results as JSON string for simplicity
    [Column(TypeName = "nvarchar(50)")]
    public string SpinResult { get; set; } = string.Empty;
    
    public decimal BetAmount { get; set; } = 1.0m;
    public decimal WinAmount { get; set; } = 0.0m;
    public bool IsWin { get; set; } = false;
    public bool UsedFreeSpin { get; set; } = false;
    
    // Computed property to check if user can play
    public bool IsUserAbleToPlay => User?.CanPlay() ?? false;
    
    // Method to check if this is a winning spin
    public bool CheckWin()
    {
        if (string.IsNullOrEmpty(SpinResult))
            return false;
            
        var symbols = SpinResult.Split(',');
        return symbols.Length == 3 && symbols[0] == symbols[1] && symbols[0] == symbols[2];
    }
}