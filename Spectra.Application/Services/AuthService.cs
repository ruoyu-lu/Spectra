using Microsoft.AspNetCore.Identity;
using Spectra.Application.DTOs.Auth;
using Spectra.Domain.Entities;

namespace Spectra.Application.Services;

/// <summary>
/// Implementation of authentication services
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtService _jwtService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User with this email already exists",
                    Errors = new List<string> { "Email is already registered" }
                };
            }

            // Create new user
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                DisplayName = request.DisplayName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Failed to create user",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            // Generate tokens
            var token = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResponse
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtService.GetTokenExpirationMinutes()),
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email!,
                    DisplayName = user.DisplayName!,
                    AvatarUrl = user.AvatarUrl,
                    CreatedAt = user.CreatedAt
                }
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred during registration",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password",
                    Errors = new List<string> { "Authentication failed" }
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid email or password",
                    Errors = new List<string> { "Authentication failed" }
                };
            }

            // Generate tokens
            var token = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();

            return new AuthResponse
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtService.GetTokenExpirationMinutes()),
                User = new UserInfo
                {
                    Id = user.Id,
                    Email = user.Email!,
                    DisplayName = user.DisplayName ?? user.UserName!,
                    AvatarUrl = user.AvatarUrl,
                    CreatedAt = user.CreatedAt
                }
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred during login",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        // For now, return not implemented
        // In a production app, you'd store refresh tokens in the database
        // and validate them here
        await Task.CompletedTask;
        return new AuthResponse
        {
            Success = false,
            Message = "Refresh token functionality not implemented yet"
        };
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        // For JWT tokens, logout is typically handled client-side
        // In a production app, you might maintain a blacklist of tokens
        await Task.CompletedTask;
        return true;
    }

    public async Task<UserInfo?> GetUserInfoAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return new UserInfo
            {
                Id = user.Id,
                Email = user.Email!,
                DisplayName = user.DisplayName ?? user.UserName!,
                AvatarUrl = user.AvatarUrl,
                CreatedAt = user.CreatedAt
            };
        }
        catch
        {
            return null;
        }
    }
}
