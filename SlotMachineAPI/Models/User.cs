using System.ComponentModel.DataAnnotations;

namespace SlotMachineAPI.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string UserName { get; set; } = string.Empty;
    
    [Range(0, 1000000)]
    public decimal Balance { get; set; } = 10.0m;
    
    [Range(0, 1000000)]
    public int FreeSpins { get; set; } = 0;

    [Range(1, 10)]
    public int Multiplier { get; set; } = 1;

    public DateTime ModifiedDateTime { get; set; } = DateTime.UtcNow;
    
    // Navigation property for games
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    
    // Methods for user operations
    public void AddCash(decimal amount) => Balance += amount;

    public void AddFreeSpins(int count = 10) => FreeSpins += count;

    public bool CanPlay() => Balance * Multiplier >= 1 || FreeSpins > 0;
    
    public decimal Cashout()
    {
        var cashoutAmount = Balance;
        Balance = 0;
        return cashoutAmount;
    }   
} 