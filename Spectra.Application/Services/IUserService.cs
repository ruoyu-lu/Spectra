using Spectra.Application.DTOs.User;

namespace Spectra.Application.Services;

/// <summary>
/// Interface for user management services
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user's profile by their ID
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve</param>
    /// <param name="currentUserId">The ID of the current user (for determining follow status)</param>
    /// <returns>User profile information or null if not found</returns>
    Task<UserProfileDto?> GetUserProfileAsync(string userId, string? currentUserId = null);

    /// <summary>
    /// Gets the current user's profile with full information
    /// </summary>
    /// <param name="userId">The ID of the current user</param>
    /// <returns>User profile information or null if not found</returns>
    Task<UserProfileDto?> GetCurrentUserProfileAsync(string userId);

    /// <summary>
    /// Updates the current user's profile
    /// </summary>
    /// <param name="userId">The ID of the user to update</param>
    /// <param name="request">The profile update request</param>
    /// <returns>Updated user profile information or null if update failed</returns>
    Task<UserProfileDto?> UpdateUserProfileAsync(string userId, UpdateProfileRequest request);

    /// <summary>
    /// Gets user statistics
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <returns>User statistics</returns>
    Task<UserStatsDto> GetUserStatsAsync(string userId);

    /// <summary>
    /// Checks if a user exists
    /// </summary>
    /// <param name="userId">The ID of the user to check</param>
    /// <returns>True if user exists, false otherwise</returns>
    Task<bool> UserExistsAsync(string userId);

    /// <summary>
    /// Searches for users by display name or username
    /// </summary>
    /// <param name="searchTerm">The search term</param>
    /// <param name="currentUserId">The ID of the current user (for determining follow status)</param>
    /// <param name="limit">Maximum number of results to return</param>
    /// <returns>List of matching user profiles</returns>
    Task<List<UserProfileDto>> SearchUsersAsync(string searchTerm, string? currentUserId = null, int limit = 20);
}
