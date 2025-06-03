using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spectra.Application.DTOs.User;
using Spectra.Application.Services;
using System.Security.Claims;

namespace Spectra.ApiService.Controllers;

/// <summary>
/// Controller for user management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Get current user's profile
    /// </summary>
    /// <returns>Current user's profile information</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> GetCurrentUserProfile()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var profile = await _userService.GetCurrentUserProfileAsync(userId);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current user profile");
            return StatusCode(500, "An error occurred while retrieving the profile");
        }
    }

    /// <summary>
    /// Get a user's profile by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User profile information</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> GetUserProfile(string id)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var profile = await _userService.GetUserProfileAsync(id, currentUserId);
            
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile for user {UserId}", id);
            return StatusCode(500, "An error occurred while retrieving the profile");
        }
    }

    /// <summary>
    /// Update current user's profile
    /// </summary>
    /// <param name="request">Profile update request</param>
    /// <returns>Updated user profile</returns>
    [HttpPut("me")]
    [Authorize]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserProfileDto>> UpdateCurrentUserProfile([FromBody] UpdateProfileRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var updatedProfile = await _userService.UpdateUserProfileAsync(userId, request);
            if (updatedProfile == null)
            {
                return NotFound();
            }

            return Ok(updatedProfile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile");
            return StatusCode(500, "An error occurred while updating the profile");
        }
    }

    /// <summary>
    /// Get user statistics
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User statistics</returns>
    [HttpGet("{id}/stats")]
    [ProducesResponseType(typeof(UserStatsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserStatsDto>> GetUserStats(string id)
    {
        try
        {
            var userExists = await _userService.UserExistsAsync(id);
            if (!userExists)
            {
                return NotFound();
            }

            var stats = await _userService.GetUserStatsAsync(id);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user stats for user {UserId}", id);
            return StatusCode(500, "An error occurred while retrieving user statistics");
        }
    }

    /// <summary>
    /// Search for users
    /// </summary>
    /// <param name="q">Search query</param>
    /// <param name="limit">Maximum number of results (default: 20, max: 50)</param>
    /// <returns>List of matching user profiles</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<UserProfileDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<UserProfileDto>>> SearchUsers(
        [FromQuery] string q, 
        [FromQuery] int limit = 20)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest("Search query cannot be empty");
            }

            if (limit <= 0 || limit > 50)
            {
                limit = 20;
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var users = await _userService.SearchUsersAsync(q, currentUserId, limit);
            
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with query: {Query}", q);
            return StatusCode(500, "An error occurred while searching for users");
        }
    }
}
