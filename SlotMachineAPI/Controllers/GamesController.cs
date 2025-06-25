using Microsoft.AspNetCore.Mvc;
using SlotMachineAPI.Services;
using SlotMachineAPI.DTOs;
using SlotMachineAPI.Models;

namespace SlotMachineAPI.Controllers;

/// <summary>
/// Controller for managing slot machine games and spins
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly IGameLogicService _gameService;
    
    public GamesController(IGameLogicService gameService)
    {
        _gameService = gameService;
    }
    
    /// <summary>
    /// Spins the slot machine for the specified user
    /// </summary>
    /// <param name="userId">The ID of the user spinning</param>
    /// <param name="request">The spin request containing bet amount</param>
    /// <returns>The spin result with win information</returns>
    [HttpPost("{userId}/spin")]
    public async Task<ActionResult<SpinResponse>> Spin(int userId, [FromBody] SpinRequest request)
    {
        try
        {
            var game = await _gameService.SpinAsync(userId, request.BetAmount);
            var user = await _gameService.GetUserAsync(userId);
            
            if (user == null)
                return NotFound(new { message = "User not found" });
                
            var response = new SpinResponse
            {
                GameId = game.GameId,
                SpinResult = game.SpinResult,
                BetAmount = game.BetAmount,
                WinAmount = game.WinAmount,
                IsWin = game.IsWin,
                UsedFreeSpin = game.UsedFreeSpin,
                SpinTime = game.SpinDateTime,
                User = MapToUserResponse(user)
            };
            
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Retrieves the game history for the specified user
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="limit">The maximum number of games to return (default: 20)</param>
    /// <returns>The list of recent games</returns>
    [HttpGet("{userId}/history")]
    public async Task<ActionResult<IEnumerable<GameHistoryResponse>>> GetGameHistory(int userId, [FromQuery] int limit = 20)
    {
        var user = await _gameService.GetUserAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });
            
        var gameHistory = user.Games
            .OrderByDescending(g => g.SpinDateTime)
            .Take(limit)
            .Select(MapToGameHistoryResponse)
            .ToList();
            
        return Ok(gameHistory);
    }
    
    /// <summary>
    /// Checks if the user can play (has balance or free spins)
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <returns>Whether the user can play</returns>
    [HttpGet("{userId}/can-play")]
    public async Task<ActionResult<object>> CanPlay(int userId)
    {
        var user = await _gameService.GetUserAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });
            
        return Ok(new { canPlay = user.CanPlay() });
    }
    
    private static UserResponse MapToUserResponse(User user) => new()
    {
        UserId = user.UserId,
        UserName = user.UserName,
        Balance = user.Balance,
        FreeSpins = user.FreeSpins,
        Multiplier = user.Multiplier,
        CanPlay = user.CanPlay()
    };
    
    private static GameHistoryResponse MapToGameHistoryResponse(Game game) => new()
    {
        GameId = game.GameId,
        SpinResult = game.SpinResult,
        BetAmount = game.BetAmount,
        WinAmount = game.WinAmount,
        IsWin = game.IsWin,
        UsedFreeSpin = game.UsedFreeSpin,
        SpinTime = game.SpinDateTime
    };
} 