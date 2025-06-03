using Spectra.Domain.Entities;

namespace Spectra.Application.Interfaces;

/// <summary>
/// Repository interface for image-related data operations
/// </summary>
public interface IImageRepository
{
    /// <summary>
    /// Gets an image by ID
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <returns>Image entity or null if not found</returns>
    Task<Image?> GetByIdAsync(Guid id);

    /// <summary>
    /// Gets an image by ID with user information
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <returns>Image entity with user information or null if not found</returns>
    Task<Image?> GetByIdWithUserAsync(Guid id);

    /// <summary>
    /// Gets paginated images for a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>Tuple containing images and total count</returns>
    Task<(List<Image> Images, int TotalCount)> GetUserImagesAsync(string userId, int page, int pageSize);

    /// <summary>
    /// Gets paginated images for the feed (from followed users)
    /// </summary>
    /// <param name="userId">Current user ID</param>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>Tuple containing images and total count</returns>
    Task<(List<Image> Images, int TotalCount)> GetFeedImagesAsync(string userId, int page, int pageSize);

    /// <summary>
    /// Gets all images with pagination (public feed)
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>Tuple containing images and total count</returns>
    Task<(List<Image> Images, int TotalCount)> GetAllImagesAsync(int page, int pageSize);

    /// <summary>
    /// Adds a new image
    /// </summary>
    /// <param name="image">Image entity to add</param>
    /// <returns>Added image entity</returns>
    Task<Image> AddAsync(Image image);

    /// <summary>
    /// Updates an existing image
    /// </summary>
    /// <param name="image">Image entity to update</param>
    /// <returns>Updated image entity</returns>
    Task<Image> UpdateAsync(Image image);

    /// <summary>
    /// Deletes an image
    /// </summary>
    /// <param name="image">Image entity to delete</param>
    /// <returns>Task</returns>
    Task DeleteAsync(Image image);

    /// <summary>
    /// Checks if a user has liked a specific image
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="imageId">Image ID</param>
    /// <returns>True if liked, false otherwise</returns>
    Task<bool> IsLikedByUserAsync(string userId, Guid imageId);

    /// <summary>
    /// Gets the like count for an image
    /// </summary>
    /// <param name="imageId">Image ID</param>
    /// <returns>Number of likes</returns>
    Task<int> GetLikeCountAsync(Guid imageId);

    /// <summary>
    /// Gets the comment count for an image
    /// </summary>
    /// <param name="imageId">Image ID</param>
    /// <returns>Number of comments</returns>
    Task<int> GetCommentCountAsync(Guid imageId);

    /// <summary>
    /// Checks if an image exists
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> ExistsAsync(Guid id);
}
