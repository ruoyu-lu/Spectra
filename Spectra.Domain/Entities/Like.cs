using System.ComponentModel.DataAnnotations;

namespace Spectra.Domain.Entities;

/// <summary>
/// Represents a like on an image
/// </summary>
public class Like
{
    /// <summary>
    /// Unique identifier for the like
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// ID of the user who liked the image
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// ID of the image that was liked
    /// </summary>
    [Required]
    public Guid ImageId { get; set; }

    /// <summary>
    /// Date when the like was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property to the user who liked the image
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;

    /// <summary>
    /// Navigation property to the liked image
    /// </summary>
    public virtual Image Image { get; set; } = null!;
}
