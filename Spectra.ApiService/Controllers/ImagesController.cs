using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spectra.Application.DTOs.Image;
using Spectra.Application.Services;
using System.Security.Claims;

namespace Spectra.ApiService.Controllers;

/// <summary>
/// Controller for image management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;
    private readonly ILogger<ImagesController> _logger;

    public ImagesController(IImageService imageService, ILogger<ImagesController> logger)
    {
        _imageService = imageService;
        _logger = logger;
    }

    /// <summary>
    /// Upload a new image
    /// </summary>
    /// <param name="request">Image upload request</param>
    /// <returns>Created image information</returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ImageDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ImageDto>> UploadImage([FromForm] CreateImageRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var imageDto = await _imageService.UploadImageAsync(request, userId);
            
            _logger.LogInformation("Image uploaded successfully by user {UserId}: {ImageId}", userId, imageDto.Id);
            return CreatedAtAction(nameof(GetImage), new { id = imageDto.Id }, imageDto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid image upload request: {Error}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image");
            return StatusCode(500, "An error occurred while uploading the image");
        }
    }

    /// <summary>
    /// Get a specific image by ID
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <returns>Image information</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ImageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ImageDto>> GetImage(Guid id)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var image = await _imageService.GetImageByIdAsync(id, currentUserId);
            
            if (image == null)
            {
                return NotFound();
            }

            return Ok(image);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting image {ImageId}", id);
            return StatusCode(500, "An error occurred while retrieving the image");
        }
    }

    /// <summary>
    /// Get current user's images
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20, max: 50)</param>
    /// <returns>Paginated list of user's images</returns>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ImageListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ImageListResponse>> GetMyImages(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Validate pagination parameters
            page = Math.Max(1, page);
            pageSize = Math.Min(Math.Max(1, pageSize), 50);

            var images = await _imageService.GetUserImagesAsync(userId, page, pageSize, userId);
            return Ok(images);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user images");
            return StatusCode(500, "An error occurred while retrieving images");
        }
    }

    /// <summary>
    /// Get images from a specific user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20, max: 50)</param>
    /// <returns>Paginated list of user's images</returns>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(ImageListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ImageListResponse>> GetUserImages(
        string userId,
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // Validate pagination parameters
            page = Math.Max(1, page);
            pageSize = Math.Min(Math.Max(1, pageSize), 50);

            var images = await _imageService.GetUserImagesAsync(userId, page, pageSize, currentUserId);
            return Ok(images);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting images for user {UserId}", userId);
            return StatusCode(500, "An error occurred while retrieving images");
        }
    }

    /// <summary>
    /// Get personalized feed (images from followed users)
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20, max: 50)</param>
    /// <returns>Paginated list of feed images</returns>
    [HttpGet("feed")]
    [Authorize]
    [ProducesResponseType(typeof(ImageListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ImageListResponse>> GetFeed(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Validate pagination parameters
            page = Math.Max(1, page);
            pageSize = Math.Min(Math.Max(1, pageSize), 50);

            var images = await _imageService.GetFeedImagesAsync(userId, page, pageSize);
            return Ok(images);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting feed for user");
            return StatusCode(500, "An error occurred while retrieving the feed");
        }
    }

    /// <summary>
    /// Get all images (public feed)
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20, max: 50)</param>
    /// <returns>Paginated list of all images</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ImageListResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<ImageListResponse>> GetAllImages(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            // Validate pagination parameters
            page = Math.Max(1, page);
            pageSize = Math.Min(Math.Max(1, pageSize), 50);

            var images = await _imageService.GetAllImagesAsync(page, pageSize, currentUserId);
            return Ok(images);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all images");
            return StatusCode(500, "An error occurred while retrieving images");
        }
    }

    /// <summary>
    /// Update image metadata
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <param name="request">Update request</param>
    /// <returns>Updated image information</returns>
    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(ImageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ImageDto>> UpdateImage(Guid id, [FromBody] UpdateImageRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var updatedImage = await _imageService.UpdateImageAsync(id, request, userId);
            if (updatedImage == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Image updated successfully by user {UserId}: {ImageId}", userId, id);
            return Ok(updatedImage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating image {ImageId}", id);
            return StatusCode(500, "An error occurred while updating the image");
        }
    }

    /// <summary>
    /// Delete an image
    /// </summary>
    /// <param name="id">Image ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteImage(Guid id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var deleted = await _imageService.DeleteImageAsync(id, userId);
            if (!deleted)
            {
                return NotFound();
            }

            _logger.LogInformation("Image deleted successfully by user {UserId}: {ImageId}", userId, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting image {ImageId}", id);
            return StatusCode(500, "An error occurred while deleting the image");
        }
    }
}
