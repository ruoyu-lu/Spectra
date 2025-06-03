using Microsoft.EntityFrameworkCore;
using Spectra.Application.Interfaces;
using Spectra.Domain.Entities;
using Spectra.Infrastructure.Data;

namespace Spectra.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for image-related data operations
/// </summary>
public class ImageRepository : IImageRepository
{
    private readonly ApplicationDbContext _context;

    public ImageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Image?> GetByIdAsync(Guid id)
    {
        return await _context.Images
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Image?> GetByIdWithUserAsync(Guid id)
    {
        return await _context.Images
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<(List<Image> Images, int TotalCount)> GetUserImagesAsync(string userId, int page, int pageSize)
    {
        var query = _context.Images
            .Include(i => i.User)
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.CreatedAt);

        var totalCount = await query.CountAsync();
        var images = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (images, totalCount);
    }

    public async Task<(List<Image> Images, int TotalCount)> GetFeedImagesAsync(string userId, int page, int pageSize)
    {
        // Get images from users that the current user follows
        var query = _context.Images
            .Include(i => i.User)
            .Where(i => _context.Follows
                .Any(f => f.FollowerId == userId && f.FollowingId == i.UserId))
            .OrderByDescending(i => i.CreatedAt);

        var totalCount = await query.CountAsync();
        var images = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (images, totalCount);
    }

    public async Task<(List<Image> Images, int TotalCount)> GetAllImagesAsync(int page, int pageSize)
    {
        var query = _context.Images
            .Include(i => i.User)
            .OrderByDescending(i => i.CreatedAt);

        var totalCount = await query.CountAsync();
        var images = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (images, totalCount);
    }

    public async Task<Image> AddAsync(Image image)
    {
        _context.Images.Add(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task<Image> UpdateAsync(Image image)
    {
        image.UpdatedAt = DateTime.UtcNow;
        _context.Images.Update(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task DeleteAsync(Image image)
    {
        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsLikedByUserAsync(string userId, Guid imageId)
    {
        return await _context.Likes
            .AnyAsync(l => l.UserId == userId && l.ImageId == imageId);
    }

    public async Task<int> GetLikeCountAsync(Guid imageId)
    {
        return await _context.Likes
            .CountAsync(l => l.ImageId == imageId);
    }

    public async Task<int> GetCommentCountAsync(Guid imageId)
    {
        return await _context.Comments
            .CountAsync(c => c.ImageId == imageId);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Images
            .AnyAsync(i => i.Id == id);
    }
}
