using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Spectra.Application.DTOs.Image;
using Spectra.Application.Interfaces;
using Spectra.Domain.Entities;
using SixLabors.ImageSharp.Formats;
using ImageSharpImage = SixLabors.ImageSharp.Image;

namespace Spectra.Application.Services;

/// <summary>
/// Implementation of image management services
/// </summary>
public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;
    private readonly ILogger<ImageService> _logger;
    private readonly string _uploadsPath;
    private readonly string _baseUrl;
    private readonly long _maxFileSizeBytes = 5 * 1024 * 1024; // 5MB
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
    private readonly string[] _allowedMimeTypes = { "image/jpeg", "image/png", "image/gif", "image/webp" };

    public ImageService(IImageRepository imageRepository, ILogger<ImageService> logger)
    {
        _imageRepository = imageRepository;
        _logger = logger;
        
        // For now, use local file storage. In production, this would be Azure Blob Storage
        _uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images");
        _baseUrl = "https://localhost:7502"; // This should come from configuration
        
        // Ensure uploads directory exists
        Directory.CreateDirectory(_uploadsPath);
    }

    public async Task<ImageDto> UploadImageAsync(CreateImageRequest request, string userId)
    {
        try
        {
            // Validate file and get dimensions in one pass to avoid multiple stream reads
            var (isValid, width, height, errorMessage) = await ValidateAndGetImageInfoAsync(request.ImageFile);
            if (!isValid)
            {
                throw new ArgumentException(errorMessage);
            }

            // Generate unique filename
            var fileExtension = Path.GetExtension(request.ImageFile.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_uploadsPath, fileName);

            // Save file to disk
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(fileStream);
            }

            // Create image entity
            var image = new Image
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                ImageUrl = $"{_baseUrl}/uploads/images/{fileName}",
                OriginalFileName = request.ImageFile.FileName,
                FileSizeBytes = request.ImageFile.Length,
                ContentType = request.ImageFile.ContentType,
                Width = width,
                Height = height,
                Tags = request.Tags,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save to database
            var savedImage = await _imageRepository.AddAsync(image);

            // Get the image with user information for the response
            var imageWithUser = await _imageRepository.GetByIdWithUserAsync(savedImage.Id);
            
            return MapToImageDto(imageWithUser!, userId);
        }
        catch (ArgumentException)
        {
            // Re-throw validation errors as-is
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image for user {UserId}. File: {FileName}, Size: {FileSize}",
                userId, request.ImageFile?.FileName, request.ImageFile?.Length);
            throw new InvalidOperationException("An error occurred while processing the image upload", ex);
        }
    }

    public async Task<ImageDto?> GetImageByIdAsync(Guid id, string? currentUserId = null)
    {
        try
        {
            var image = await _imageRepository.GetByIdWithUserAsync(id);
            if (image == null)
            {
                return null;
            }

            return await MapToImageDtoWithStatsAsync(image, currentUserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting image {ImageId}", id);
            throw;
        }
    }

    public async Task<ImageListResponse> GetUserImagesAsync(string userId, int page, int pageSize, string? currentUserId = null)
    {
        try
        {
            var (images, totalCount) = await _imageRepository.GetUserImagesAsync(userId, page, pageSize);
            
            var imageDtos = new List<ImageDto>();
            foreach (var image in images)
            {
                imageDtos.Add(await MapToImageDtoWithStatsAsync(image, currentUserId));
            }

            return CreateImageListResponse(imageDtos, totalCount, page, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting images for user {UserId}", userId);
            throw;
        }
    }

    public async Task<ImageListResponse> GetFeedImagesAsync(string userId, int page, int pageSize)
    {
        try
        {
            var (images, totalCount) = await _imageRepository.GetFeedImagesAsync(userId, page, pageSize);
            
            var imageDtos = new List<ImageDto>();
            foreach (var image in images)
            {
                imageDtos.Add(await MapToImageDtoWithStatsAsync(image, userId));
            }

            return CreateImageListResponse(imageDtos, totalCount, page, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting feed images for user {UserId}", userId);
            throw;
        }
    }

    public async Task<ImageListResponse> GetAllImagesAsync(int page, int pageSize, string? currentUserId = null)
    {
        try
        {
            var (images, totalCount) = await _imageRepository.GetAllImagesAsync(page, pageSize);
            
            var imageDtos = new List<ImageDto>();
            foreach (var image in images)
            {
                imageDtos.Add(await MapToImageDtoWithStatsAsync(image, currentUserId));
            }

            return CreateImageListResponse(imageDtos, totalCount, page, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all images");
            throw;
        }
    }

    public async Task<ImageDto?> UpdateImageAsync(Guid id, UpdateImageRequest request, string userId)
    {
        try
        {
            var image = await _imageRepository.GetByIdWithUserAsync(id);
            if (image == null || image.UserId != userId)
            {
                return null;
            }

            // Update image metadata
            image.Title = request.Title;
            image.Description = request.Description;
            image.Tags = request.Tags;

            var updatedImage = await _imageRepository.UpdateAsync(image);
            return await MapToImageDtoWithStatsAsync(updatedImage, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating image {ImageId} for user {UserId}", id, userId);
            throw;
        }
    }

    public async Task<bool> DeleteImageAsync(Guid id, string userId)
    {
        try
        {
            var image = await _imageRepository.GetByIdAsync(id);
            if (image == null || image.UserId != userId)
            {
                return false;
            }

            // Delete file from disk
            var fileName = Path.GetFileName(new Uri(image.ImageUrl).LocalPath);
            var filePath = Path.Combine(_uploadsPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Delete from database
            await _imageRepository.DeleteAsync(image);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting image {ImageId} for user {UserId}", id, userId);
            throw;
        }
    }

    public async Task<bool> ImageExistsAsync(Guid id)
    {
        return await _imageRepository.ExistsAsync(id);
    }

    private Task<(bool isValid, int? width, int? height, string errorMessage)> ValidateAndGetImageInfoAsync(IFormFile file)
    {
        return Task.FromResult(ValidateAndGetImageInfoSync(file));
    }

    private (bool isValid, int? width, int? height, string errorMessage) ValidateAndGetImageInfoSync(IFormFile file)
    {
        // Basic validation
        if (file == null || file.Length == 0)
        {
            return (false, null, null, "File is required");
        }

        if (file.Length > _maxFileSizeBytes)
        {
            return (false, null, null, $"File size cannot exceed {_maxFileSizeBytes / (1024 * 1024)}MB");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
        {
            return (false, null, null, $"File type not supported. Allowed types: {string.Join(", ", _allowedExtensions)}");
        }

        if (!_allowedMimeTypes.Contains(file.ContentType))
        {
            return (false, null, null, $"MIME type not supported. Allowed types: {string.Join(", ", _allowedMimeTypes)}");
        }

        // Validate image and get dimensions in one pass
        try
        {
            using var stream = file.OpenReadStream();
            using var image = ImageSharpImage.Load(stream);

            if (image == null)
            {
                return (false, null, null, "File is not a valid image");
            }

            return (true, image.Width, image.Height, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to validate image file: {FileName}", file.FileName);
            return (false, null, null, "File is not a valid image or is corrupted");
        }
    }

    private ImageDto MapToImageDto(Image image, string? currentUserId)
    {
        return new ImageDto
        {
            Id = image.Id,
            Title = image.Title,
            Description = image.Description,
            ImageUrl = image.ImageUrl,
            ThumbnailUrl = image.ThumbnailUrl,
            OriginalFileName = image.OriginalFileName,
            FileSizeBytes = image.FileSizeBytes,
            ContentType = image.ContentType,
            Width = image.Width,
            Height = image.Height,
            Tags = image.Tags,
            CreatedAt = image.CreatedAt,
            UpdatedAt = image.UpdatedAt,
            UserId = image.UserId,
            UserDisplayName = image.User?.DisplayName ?? "Unknown User",
            UserAvatarUrl = image.User?.AvatarUrl,
            LikeCount = 0, // Will be populated by MapToImageDtoWithStatsAsync
            CommentCount = 0, // Will be populated by MapToImageDtoWithStatsAsync
            IsLikedByCurrentUser = false, // Will be populated by MapToImageDtoWithStatsAsync
            IsOwnedByCurrentUser = currentUserId == image.UserId
        };
    }

    private async Task<ImageDto> MapToImageDtoWithStatsAsync(Image image, string? currentUserId)
    {
        var dto = MapToImageDto(image, currentUserId);
        
        // Get like and comment counts
        dto.LikeCount = await _imageRepository.GetLikeCountAsync(image.Id);
        dto.CommentCount = await _imageRepository.GetCommentCountAsync(image.Id);
        
        // Check if current user has liked this image
        if (!string.IsNullOrEmpty(currentUserId))
        {
            dto.IsLikedByCurrentUser = await _imageRepository.IsLikedByUserAsync(currentUserId, image.Id);
        }

        return dto;
    }

    private ImageListResponse CreateImageListResponse(List<ImageDto> images, int totalCount, int page, int pageSize)
    {
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        
        return new ImageListResponse
        {
            Images = images,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasNextPage = page < totalPages,
            HasPreviousPage = page > 1
        };
    }
}
