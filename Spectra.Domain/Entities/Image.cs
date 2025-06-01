using System.ComponentModel.DataAnnotations;

namespace Spectra.Domain.Entities;

/// <summary>
/// Represents an image uploaded to the platform
/// </summary>
public class Image
{
    /// <summary>
    /// Unique identifier for the image
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Title/name of the image
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the image
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// URL to the image file in storage
    /// </summary>
    [Required]
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
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the image metadata was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// ID of the user who uploaded the image
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property to the user who uploaded the image
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;

    /// <summary>
    /// Navigation property for likes on this image
    /// </summary>
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    /// <summary>
    /// Navigation property for comments on this image
    /// </summary>
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    /// <summary>
    /// Computed property for like count
    /// </summary>
    public int LikeCount => Likes?.Count ?? 0;

    /// <summary>
    /// Computed property for comment count
    /// </summary>
    public int CommentCount => Comments?.Count ?? 0;
}
