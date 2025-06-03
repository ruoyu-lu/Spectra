using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Spectra.Application.DTOs.Image;

/// <summary>
/// Request model for creating a new image
/// </summary>
public class CreateImageRequest
{
    /// <summary>
    /// Title/name of the image
    /// </summary>
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Description of the image
    /// </summary>
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string? Description { get; set; }

    /// <summary>
    /// Tags associated with the image (comma-separated)
    /// </summary>
    [StringLength(500, ErrorMessage = "Tags cannot exceed 500 characters")]
    public string? Tags { get; set; }

    /// <summary>
    /// The image file to upload
    /// </summary>
    [Required(ErrorMessage = "Image file is required")]
    public IFormFile ImageFile { get; set; } = null!;
}
