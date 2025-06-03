using Microsoft.EntityFrameworkCore;
using Spectra.Application.Interfaces;
using Spectra.Domain.Entities;
using Spectra.Infrastructure.Data;

namespace Spectra.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for user-related data operations
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(int ImageCount, int FollowersCount, int FollowingCount, int TotalLikesReceived, int TotalCommentsReceived)> GetUserStatsAsync(string userId)
    {
        var imageCount = await _context.Images.CountAsync(i => i.UserId == userId);
        var followersCount = await _context.Follows.CountAsync(f => f.FollowingId == userId);
        var followingCount = await _context.Follows.CountAsync(f => f.FollowerId == userId);
        
        var totalLikesReceived = await _context.Likes
            .Where(l => l.Image.UserId == userId)
            .CountAsync();
        
        var totalCommentsReceived = await _context.Comments
            .Where(c => c.Image.UserId == userId)
            .CountAsync();

        return (imageCount, followersCount, followingCount, totalLikesReceived, totalCommentsReceived);
    }

    public async Task<bool> IsFollowingAsync(string followerId, string followingId)
    {
        return await _context.Follows
            .AnyAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);
    }

    public async Task<List<ApplicationUser>> SearchUsersAsync(string searchTerm, int limit)
    {
        var searchTermLower = searchTerm.ToLower();
        
        return await _context.Users
            .Where(u => u.DisplayName!.ToLower().Contains(searchTermLower) || 
                       u.UserName!.ToLower().Contains(searchTermLower))
            .Take(limit)
            .ToListAsync();
    }
}
