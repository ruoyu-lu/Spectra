using System.ComponentModel.DataAnnotations;

namespace Spectra.Application.DTOs.User;

/// <summary>
/// Data transfer object for user profile information
/// </summary>
public class UserProfileDto
{
    /// <summary>
    /// User's unique identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's username
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// User's display name/nickname
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// User's bio/description
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// URL to user's avatar image
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Date when the user joined the platform
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date when the user profile was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// User statistics
    /// </summary>
    public UserStatsDto Stats { get; set; } = new();

    /// <summary>
    /// Whether the current user is following this user (only set when viewing other users' profiles)
    /// </summary>
    public bool? IsFollowing { get; set; }

    /// <summary>
    /// Whether this is the current user's own profile
    /// </summary>
    public bool IsOwnProfile { get; set; }
}
