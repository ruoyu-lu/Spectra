namespace Spectra.Application.DTOs.User;

/// <summary>
/// Data transfer object for user statistics
/// </summary>
public class UserStatsDto
{
    /// <summary>
    /// Number of images uploaded by the user
    /// </summary>
    public int ImageCount { get; set; }

    /// <summary>
    /// Number of users following this user
    /// </summary>
    public int FollowersCount { get; set; }

    /// <summary>
    /// Number of users this user is following
    /// </summary>
    public int FollowingCount { get; set; }

    /// <summary>
    /// Total number of likes received on all user's images
    /// </summary>
    public int TotalLikesReceived { get; set; }

    /// <summary>
    /// Total number of comments received on all user's images
    /// </summary>
    public int TotalCommentsReceived { get; set; }
}
