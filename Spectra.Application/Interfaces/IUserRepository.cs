using Spectra.Domain.Entities;

namespace Spectra.Application.Interfaces;

/// <summary>
/// Repository interface for user-related data operations
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets user statistics
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>User statistics</returns>
    Task<(int ImageCount, int FollowersCount, int FollowingCount, int TotalLikesReceived, int TotalCommentsReceived)> GetUserStatsAsync(string userId);

    /// <summary>
    /// Checks if a user is following another user
    /// </summary>
    /// <param name="followerId">The follower user ID</param>
    /// <param name="followingId">The following user ID</param>
    /// <returns>True if following, false otherwise</returns>
    Task<bool> IsFollowingAsync(string followerId, string followingId);

    /// <summary>
    /// Searches for users by display name or username
    /// </summary>
    /// <param name="searchTerm">The search term</param>
    /// <param name="limit">Maximum number of results</param>
    /// <returns>List of matching users</returns>
    Task<List<ApplicationUser>> SearchUsersAsync(string searchTerm, int limit);
}
