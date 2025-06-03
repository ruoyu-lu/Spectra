using System.ComponentModel.DataAnnotations;

namespace Spectra.Application.DTOs.User;

/// <summary>
/// Request model for updating user profile
/// </summary>
public class UpdateProfileRequest
{
    /// <summary>
    /// User's display name/nickname
    /// </summary>
    [MaxLength(100, ErrorMessage = "Display name cannot exceed 100 characters")]
    public string? DisplayName { get; set; }

    /// <summary>
    /// User's bio/description
    /// </summary>
    [MaxLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
    public string? Bio { get; set; }

    /// <summary>
    /// URL to user's avatar image
    /// </summary>
    [MaxLength(500, ErrorMessage = "Avatar URL cannot exceed 500 characters")]
    [Url(ErrorMessage = "Avatar URL must be a valid URL")]
    public string? AvatarUrl { get; set; }
}
