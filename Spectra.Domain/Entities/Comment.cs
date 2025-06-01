using System.ComponentModel.DataAnnotations;

namespace Spectra.Domain.Entities;

/// <summary>
/// Represents a comment on an image
/// </summary>
public class Comment
{
    /// <summary>
    /// Unique identifier for the comment
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Content of the comment
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// ID of the user who made the comment
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// ID of the image the comment is on
    /// </summary>
    [Required]
    public Guid ImageId { get; set; }

    /// <summary>
    /// Date when the comment was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the comment was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property to the user who made the comment
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;

    /// <summary>
    /// Navigation property to the image the comment is on
    /// </summary>
    public virtual Image Image { get; set; } = null!;
}
