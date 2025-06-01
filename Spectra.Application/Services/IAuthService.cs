using Spectra.Application.DTOs.Auth;

namespace Spectra.Application.Services;

/// <summary>
/// Interface for authentication services
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="request">Registration request</param>
    /// <returns>Authentication response</returns>
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Authenticates a user
    /// </summary>
    /// <param name="request">Login request</param>
    /// <returns>Authentication response</returns>
    Task<AuthResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Refreshes an access token using a refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>Authentication response with new tokens</returns>
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);

    /// <summary>
    /// Logs out a user by invalidating their refresh token
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>True if successful</returns>
    Task<bool> LogoutAsync(string userId);

    /// <summary>
    /// Gets user information by ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User information</returns>
    Task<UserInfo?> GetUserInfoAsync(string userId);
}
