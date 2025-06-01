namespace Spectra.Application.DTOs.Auth;

/// <summary>
/// Response model for authentication operations
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Indicates if the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// JWT access token
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Refresh token for obtaining new access tokens
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Token expiration time
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// User information
    /// </summary>
    public UserInfo? User { get; set; }

    /// <summary>
    /// Error message if operation failed
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// List of validation errors
    /// </summary>
    public List<string> Errors { get; set; } = new();
}

/// <summary>
/// User information included in auth response
/// </summary>
public class UserInfo
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
    /// User's display name
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// User's avatar URL
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Date when the user joined
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
