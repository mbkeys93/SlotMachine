using Microsoft.AspNetCore.Mvc;
using SlotMachineAPI.Services;
using SlotMachineAPI.DTOs;
using SlotMachineAPI.Models;
using SlotMachineAPI.Data;

namespace SlotMachineAPI.Controllers;

/// <summary>
/// Controller for managing users and their account operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IGameLogicService _gameService;
    private readonly SlotMachineDbContext _context;
    
    public UsersController(IGameLogicService gameService, SlotMachineDbContext context)
    {
        _gameService = gameService;
        _context = context;
    }
    
    /// <summary>
    /// Creates a new user with the specified username
    /// </summary>
    /// <param name="request">The user creation request containing the username</param>
    /// <returns>The created user information</returns>
    [HttpPost]
    public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = await _gameService.CreateUserAsync(request.UserName);
            return Ok(MapToUserResponse(user));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Gets a user by username (for login/reuse)
    /// </summary>
    /// <param name="userName">The username to search for</param>
    /// <returns>The user information if found</returns>
    [HttpGet("username/{userName}")]
    public async Task<ActionResult<UserResponse>> GetUserByUsername(string userName)
    {
        var user = await _gameService.GetUserByUsernameAsync(userName);
        if (user == null)
            return NotFound(new { message = "User not found" });
            
        return Ok(MapToUserResponse(user));
    }
    
    /// <summary>
    /// Gets all users (for selection)
    /// </summary>
    /// <returns>List of all users</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
    {
        var users = await _gameService.GetAllUsersAsync();
        return Ok(users.Select(MapToUserResponse));
    }
    
    /// <summary>
    /// Retrieves user information by user ID
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve</param>
    /// <returns>The user information</returns>
    [HttpGet("id/{userId}")]
    public async Task<ActionResult<UserResponse>> GetUser(int userId)
    {
        var user = await _gameService.GetUserAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });
            
        return Ok(MapToUserResponse(user));
    }
    
    /// <summary>
    /// Adds cash to the user's balance
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="request">The amount to add to the user's balance</param>
    /// <returns>The updated user information</returns>
    [HttpPost("{userId}/balance")]
    public async Task<ActionResult<UserResponse>> AddCash(int userId, [FromBody] AddCashRequest request)
    {
        var user = await _gameService.GetUserAsync(userId);
        if (user == null)
            return NotFound(new { message = "User not found" });
                
        user.AddCash(request.Amount);
        await _context.SaveChangesAsync();
        
        return Ok(MapToUserResponse(user));
    }
    
    /// <summary>
    /// Adds free spins to the user's account
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="request">The number of free spins to add (default: 10)</param>
    /// <returns>The updated user information</returns>
    [HttpPost("{userId}/free-spins")]
    public async Task<ActionResult<UserResponse>> AddFreeSpins(int userId, [FromBody] AddFreeSpinsRequest request)
    {
        try
        {
            await _gameService.AddFreeSpinsAsync(userId, request.Count);
            var user = await _gameService.GetUserAsync(userId);
            
            if (user == null)
                return NotFound(new { message = "User not found" });
                
            return Ok(MapToUserResponse(user));
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Cashes out the user's current balance
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <returns>The cashout amount</returns>
    [HttpPost("{userId}/cashout")]
    public async Task<ActionResult<CashoutResponse>> Cashout(int userId)
    {
        try
        {
            var cashoutAmount = await _gameService.CashoutAsync(userId);
            return Ok(new CashoutResponse(cashoutAmount));
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
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
} 