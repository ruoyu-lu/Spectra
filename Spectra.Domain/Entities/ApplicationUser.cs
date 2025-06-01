using Microsoft.AspNetCore.Identity;

namespace Spectra.Domain.Entities;

/// <summary>
/// Represents a user in the Spectra platform
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// User's display name/nickname
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// User's bio/description
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// URL to user's avatar image
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// Date when the user joined the platform
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date when the user profile was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property for images uploaded by this user
    /// </summary>
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    /// <summary>
    /// Navigation property for users this user is following
    /// </summary>
    public virtual ICollection<Follow> Following { get; set; } = new List<Follow>();

    /// <summary>
    /// Navigation property for users following this user
    /// </summary>
    public virtual ICollection<Follow> Followers { get; set; } = new List<Follow>();

    /// <summary>
    /// Navigation property for likes given by this user
    /// </summary>
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();

    /// <summary>
    /// Navigation property for comments made by this user
    /// </summary>
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
