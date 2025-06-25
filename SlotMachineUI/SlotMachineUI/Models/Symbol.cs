namespace SlotMachineUI.Models;

public class Symbol
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public int Weight { get; set; }
}

public class SymbolStatistics
{
    public int TotalWeight { get; set; }
    public int SymbolCount { get; set; }
    public List<Symbol> Symbols { get; set; } = new();
} 