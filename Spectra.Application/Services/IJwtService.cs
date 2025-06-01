using Spectra.Domain.Entities;

namespace Spectra.Application.Services;

/// <summary>
/// Interface for JWT token generation and validation
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Generates a JWT token for the specified user
    /// </summary>
    /// <param name="user">The user to generate the token for</param>
    /// <returns>JWT token string</returns>
    string GenerateToken(ApplicationUser user);

    /// <summary>
    /// Generates a refresh token
    /// </summary>
    /// <returns>Refresh token string</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates a JWT token
    /// </summary>
    /// <param name="token">The token to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    bool ValidateToken(string token);

    /// <summary>
    /// Gets the user ID from a JWT token
    /// </summary>
    /// <param name="token">The JWT token</param>
    /// <returns>User ID if valid, null otherwise</returns>
    string? GetUserIdFromToken(string token);

    /// <summary>
    /// Gets the token expiration time
    /// </summary>
    /// <returns>Token expiration time in minutes</returns>
    int GetTokenExpirationMinutes();
}
