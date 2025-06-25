using System.ComponentModel.DataAnnotations;

namespace SlotMachineAPI.Models;

public class Symbol
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    public decimal Value { get; set; } // Value in dollars
    
    [Required]
    public int Weight { get; set; } // Frequency on the reel
        
    // Constants for default symbol values (in dollars)
    public static class Values
    {
        public const decimal Nine = 0.25m;
        public const decimal Ten = 0.50m;
        public const decimal Jack = 1.00m;
        public const decimal Queen = 2.00m;
        public const decimal King = 4.00m;
        public const decimal Ace = 8.00m;
        public const decimal Bonus = 0.00m;
        public const decimal Jackpot = 100.00m;
    }
    
    // Constants for default symbol weights
    public static class Weights
    {
        public const int Nine = 256;
        public const int Ten = 128;
        public const int Jack = 64;
        public const int Queen = 32;
        public const int King = 16;
        public const int Ace = 8;
        public const int Bonus = 4;
        public const int Jackpot = 2;
    }
    
    // Static method to get default symbols for seeding
    public static List<Symbol> GetDefaultSymbols()
    {
        return new List<Symbol>
        {
            new Symbol { Id = 1, Name = "Nine", Value = Values.Nine, Weight = Weights.Nine },
            new Symbol { Id = 2, Name = "Ten", Value = Values.Ten, Weight = Weights.Ten },
            new Symbol { Id = 3, Name = "Jack", Value = Values.Jack, Weight = Weights.Jack },
            new Symbol { Id = 4, Name = "Queen", Value = Values.Queen, Weight = Weights.Queen },
            new Symbol { Id = 5, Name = "King", Value = Values.King, Weight = Weights.King },
            new Symbol { Id = 6, Name = "Ace", Value = Values.Ace, Weight = Weights.Ace },
            new Symbol { Id = 7, Name = "Bonus", Value = Values.Bonus, Weight = Weights.Bonus },
            new Symbol { Id = 8, Name = "Jackpot", Value = Values.Jackpot, Weight = Weights.Jackpot }
        };
    }
    
    public override string ToString()
    {
        return Name;
    }
} 