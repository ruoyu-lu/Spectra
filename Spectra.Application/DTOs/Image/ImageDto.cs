using System.ComponentModel.DataAnnotations;

namespace Spectra.Application.DTOs.Image;

/// <summary>
/// Data transfer object for image information
/// </summary>
public class ImageDto
{
    /// <summary>
    /// Unique identifier for the image
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Title/name of the image
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the image
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// URL to the image file in storage
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// URL to the thumbnail version of the image
    /// </summary>
    public string? ThumbnailUrl { get; set; }

    /// <summary>
    /// Original filename of the uploaded image
    /// </summary>
    public string? OriginalFileName { get; set; }

    /// <summary>
    /// File size in bytes
    /// </summary>
    public long FileSizeBytes { get; set; }

    /// <summary>
    /// MIME type of the image
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Width of the image in pixels
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// Height of the image in pixels
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Tags associated with the image
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// Date when the image was uploaded
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date when the image metadata was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// ID of the user who uploaded the image
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the user who uploaded the image
    /// </summary>
    public string UserDisplayName { get; set; } = string.Empty;

    /// <summary>
    /// Avatar URL of the user who uploaded the image
    /// </summary>
    public string? UserAvatarUrl { get; set; }

    /// <summary>
    /// Number of likes on this image
    /// </summary>
    public int LikeCount { get; set; }

    /// <summary>
    /// Number of comments on this image
    /// </summary>
    public int CommentCount { get; set; }

    /// <summary>
    /// Whether the current user has liked this image
    /// </summary>
    public bool IsLikedByCurrentUser { get; set; }

    /// <summary>
    /// Whether the current user owns this image
    /// </summary>
    public bool IsOwnedByCurrentUser { get; set; }
}
