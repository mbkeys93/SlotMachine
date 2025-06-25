using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlotMachineAPI.Data;
using SlotMachineAPI.Models;
using SlotMachineAPI.DTOs;

namespace SlotMachineAPI.Controllers;

/// <summary>
/// Controller for managing slot machine symbols
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SymbolsController : ControllerBase
{
    private readonly SlotMachineDbContext _context;
    
    public SymbolsController(SlotMachineDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Gets all symbols with their values and weights
    /// </summary>
    /// <returns>List of all symbols</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SymbolResponse>>> GetSymbols()
    {
        var symbols = await _context.Symbols
            .OrderBy(s => s.Id)
            .Select(s => new SymbolResponse
            {
                Id = s.Id,
                Name = s.Name,
                Value = s.Value,
                Weight = s.Weight
            })
            .ToListAsync();
            
        return Ok(symbols);
    }
    
    /// <summary>
    /// Gets a specific symbol by ID
    /// </summary>
    /// <param name="id">The symbol ID</param>
    /// <returns>The symbol information</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<SymbolResponse>> GetSymbol(int id)
    {
        var symbol = await _context.Symbols.FindAsync(id);
        
        if (symbol == null)
            return NotFound(new { message = "Symbol not found" });
            
        return Ok(MapToSymbolResponse(symbol));
    }
    
    /// <summary>
    /// Updates a symbol's value and weight
    /// </summary>
    /// <param name="id">The symbol ID</param>
    /// <param name="request">The update request</param>
    /// <returns>The updated symbol</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<SymbolResponse>> UpdateSymbol(int id, [FromBody] UpdateSymbolRequest request)
    {
        var symbol = await _context.Symbols.FindAsync(id);
        
        if (symbol == null)
            return NotFound(new { message = "Symbol not found" });
            
        symbol.Value = request.Value;
        symbol.Weight = request.Weight;
        
        await _context.SaveChangesAsync();
        
        return Ok(MapToSymbolResponse(symbol));
    }
    
    /// <summary>
    /// Gets symbol statistics including total weight and probabilities
    /// </summary>
    /// <returns>Symbol statistics</returns>
    [HttpGet("statistics")]
    public async Task<ActionResult<SymbolStatisticsResponse>> GetSymbolStatistics()
    {
        var symbols = await _context.Symbols.ToListAsync();
        
        if (!symbols.Any())
            return NotFound(new { message = "No symbols found" });
        
        var totalWeight = symbols.Sum(s => s.Weight);
        var symbolResponses = symbols.Select(MapToSymbolResponse).ToList();
        
        var response = new SymbolStatisticsResponse
        {
            TotalWeight = totalWeight,
            SymbolCount = symbols.Count,
            Symbols = symbolResponses
        };
        
        return Ok(response);
    }
    
    /// <summary>
    /// Resets all symbols to their default values
    /// </summary>
    /// <returns>Success message</returns>
    [HttpPost("reset-to-defaults")]
    public async Task<ActionResult<object>> ResetToDefaults()
    {
        var defaultSymbols = Symbol.GetDefaultSymbols();
        
        // Clear existing symbols
        _context.Symbols.RemoveRange(await _context.Symbols.ToListAsync());
        
        // Add default symbols
        _context.Symbols.AddRange(defaultSymbols);
        
        await _context.SaveChangesAsync();
        
        return Ok(new { message = "Symbols reset to default values" });
    }
    
    private static SymbolResponse MapToSymbolResponse(Symbol symbol) => new()
    {
        Id = symbol.Id,
        Name = symbol.Name,
        Value = symbol.Value,
        Weight = symbol.Weight
    };
}