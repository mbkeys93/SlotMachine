namespace SlotMachineAPI.DTOs;

public record SymbolResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Value { get; init; }
    public int Weight { get; init; }
}

public record UpdateSymbolRequest
{
    public decimal Value { get; init; }
    public int Weight { get; init; }
}

public record SymbolStatisticsResponse
{
    public int TotalWeight { get; init; }
    public int SymbolCount { get; init; }
    public List<SymbolResponse> Symbols { get; init; } = new();
}       