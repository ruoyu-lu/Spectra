using Spectra.Application.DTOs.Image;

namespace Spectra.Application.Services;

/// <summary>
/// Interface for image management services
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Uploads a new image
    /// </summary>
    /// <param name="request">Image upload request</param>
    /// <param name="userId">ID of the user uploading the image</param>
    /// <returns>Created image DTO</returns>
    Task<ImageDto> UploadImageAsync(CreateImageRequest request, string userId);

    /// <summary>
    /// Gets an image by ID
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <param name="currentUserId">Current user ID (optional)</param>
    /// <returns>Image DTO or null if not found</returns>
    Task<ImageDto?> GetImageByIdAsync(Guid id, string? currentUserId = null);

    /// <summary>
    /// Gets paginated images for a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="currentUserId">Current user ID (optional)</param>
    /// <returns>Paginated image list response</returns>
    Task<ImageListResponse> GetUserImagesAsync(string userId, int page, int pageSize, string? currentUserId = null);

    /// <summary>
    /// Gets paginated images for the current user's feed
    /// </summary>
    /// <param name="userId">Current user ID</param>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <returns>Paginated image list response</returns>
    Task<ImageListResponse> GetFeedImagesAsync(string userId, int page, int pageSize);

    /// <summary>
    /// Gets all images with pagination (public feed)
    /// </summary>
    /// <param name="page">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="currentUserId">Current user ID (optional)</param>
    /// <returns>Paginated image list response</returns>
    Task<ImageListResponse> GetAllImagesAsync(int page, int pageSize, string? currentUserId = null);

    /// <summary>
    /// Updates image metadata
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <param name="request">Update request</param>
    /// <param name="userId">ID of the user making the request</param>
    /// <returns>Updated image DTO or null if not found/unauthorized</returns>
    Task<ImageDto?> UpdateImageAsync(Guid id, UpdateImageRequest request, string userId);

    /// <summary>
    /// Deletes an image
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <param name="userId">ID of the user making the request</param>
    /// <returns>True if deleted successfully, false if not found/unauthorized</returns>
    Task<bool> DeleteImageAsync(Guid id, string userId);

    /// <summary>
    /// Checks if an image exists
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> ImageExistsAsync(Guid id);
}
