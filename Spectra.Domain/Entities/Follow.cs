using System.ComponentModel.DataAnnotations;

namespace Spectra.Domain.Entities;

/// <summary>
/// Represents a follow relationship between users
/// </summary>
public class Follow
{
    /// <summary>
    /// Unique identifier for the follow relationship
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// ID of the user who is following
    /// </summary>
    [Required]
    public string FollowerId { get; set; } = string.Empty;

    /// <summary>
    /// ID of the user being followed
    /// </summary>
    [Required]
    public string FollowingId { get; set; } = string.Empty;

    /// <summary>
    /// Date when the follow relationship was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Navigation property to the user who is following
    /// </summary>
    public virtual ApplicationUser Follower { get; set; } = null!;

    /// <summary>
    /// Navigation property to the user being followed
    /// </summary>
    public virtual ApplicationUser Following { get; set; } = null!;
}
