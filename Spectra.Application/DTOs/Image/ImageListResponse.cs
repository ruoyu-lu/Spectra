namespace Spectra.Application.DTOs.Image;

/// <summary>
/// Response model for paginated image lists
/// </summary>
public class ImageListResponse
{
    /// <summary>
    /// List of images
    /// </summary>
    public List<ImageDto> Images { get; set; } = new();

    /// <summary>
    /// Total number of images available
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number (1-based)
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Whether there are more pages available
    /// </summary>
    public bool HasNextPage { get; set; }

    /// <summary>
    /// Whether there are previous pages available
    /// </summary>
    public bool HasPreviousPage { get; set; }
}
