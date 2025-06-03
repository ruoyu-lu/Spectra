using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Spectra.Application.DTOs.User;
using Spectra.Application.Interfaces;
using Spectra.Domain.Entities;

namespace Spectra.Application.Services;

/// <summary>
/// Service for user management operations
/// </summary>
public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IUserRepository userRepository,
        ILogger<UserService> logger)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserProfileDto?> GetUserProfileAsync(string userId, string? currentUserId = null)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            var stats = await GetUserStatsAsync(userId);
            
            bool? isFollowing = null;
            if (!string.IsNullOrEmpty(currentUserId) && currentUserId != userId)
            {
                isFollowing = await _userRepository.IsFollowingAsync(currentUserId, userId);
            }

            return new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email!,
                UserName = user.UserName,
                DisplayName = user.DisplayName,
                Bio = user.Bio,
                AvatarUrl = user.AvatarUrl,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Stats = stats,
                IsFollowing = isFollowing,
                IsOwnProfile = currentUserId == userId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user profile for user {UserId}", userId);
            return null;
        }
    }

    public async Task<UserProfileDto?> GetCurrentUserProfileAsync(string userId)
    {
        return await GetUserProfileAsync(userId, userId);
    }

    public async Task<UserProfileDto?> UpdateUserProfileAsync(string userId, UpdateProfileRequest request)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            // Update user properties
            user.DisplayName = request.DisplayName?.Trim();
            user.Bio = request.Bio?.Trim();
            user.AvatarUrl = request.AvatarUrl?.Trim();
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to update user profile for user {UserId}: {Errors}", 
                    userId, string.Join(", ", result.Errors.Select(e => e.Description)));
                return null;
            }

            return await GetCurrentUserProfileAsync(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile for user {UserId}", userId);
            return null;
        }
    }

    public async Task<UserStatsDto> GetUserStatsAsync(string userId)
    {
        try
        {
            var stats = await _userRepository.GetUserStatsAsync(userId);

            return new UserStatsDto
            {
                ImageCount = stats.ImageCount,
                FollowersCount = stats.FollowersCount,
                FollowingCount = stats.FollowingCount,
                TotalLikesReceived = stats.TotalLikesReceived,
                TotalCommentsReceived = stats.TotalCommentsReceived
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user stats for user {UserId}", userId);
            return new UserStatsDto();
        }
    }

    public async Task<bool> UserExistsAsync(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if user exists: {UserId}", userId);
            return false;
        }
    }

    public async Task<List<UserProfileDto>> SearchUsersAsync(string searchTerm, string? currentUserId = null, int limit = 20)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return new List<UserProfileDto>();
            }

            var users = await _userRepository.SearchUsersAsync(searchTerm.Trim(), limit);

            var userProfiles = new List<UserProfileDto>();

            foreach (var user in users)
            {
                var profile = await GetUserProfileAsync(user.Id, currentUserId);
                if (profile != null)
                {
                    userProfiles.Add(profile);
                }
            }

            return userProfiles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            return new List<UserProfileDto>();
        }
    }
}
