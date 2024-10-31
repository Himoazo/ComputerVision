using ComputerVision.Data.Repositories;
using ImageManipulation.Data.Services;
using Microsoft.AspNetCore.Mvc;
using ComputerVision.Data.Models.DTO;
using ComputerVision.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ComputerVision.API.Controllers;

[Route("api/images")]
[ApiController]
public class ImageController(IFileService fileService, IImageRepository imageRepo, 
    ILogger<ImageController> logger, IWebHostEnvironment environment) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateImage([FromForm] ImageDTO imageToAdd)
    {
        try
        {
            if (imageToAdd.ImageFile is null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid file");
            }
            if (imageToAdd.ImageFile.Length > 1 * 1024 * 1024)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
            }
            string[] allowedFileExtensions = [".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff"];
            var (createdImageName, createdImageUrl) = await fileService.SaveFileAsync(imageToAdd.ImageFile, imageToAdd.Filter, allowedFileExtensions);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return Unauthorized(); }

            // mapping `ImageDTO` to `Image` manually. You can use automapper.
            var image = new Image
            {
                ImageName = imageToAdd.ImageName,
                ImageMaterial = createdImageName,
                ImageUrl = createdImageUrl,
                UserId = userId
            };
            var createdImage = await imageRepo.AddImageAsync(image);
            return CreatedAtAction(nameof(CreateImage), createdImage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }


    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteImage(int id)
    {
        try
        {
            var existingImage = await imageRepo.FindImageByIdAsync(id);
            if (existingImage == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Image with id: {id} not found");
            }

            await imageRepo.DeleteImageAsync(existingImage);
            // After deleting image from database, remove file from directory.
            fileService.DeleteFile(existingImage.ImageMaterial!);
            return NoContent();  // return 204
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetImage(int id)
    {
        // Retrieve the image details from the repository using the provided ID
        var image = await imageRepo.FindImageByIdAsync(id);

        // Check if the image exists
        if (image == null)
        {
            return NotFound($"Image with id: {id} not found");
        }

        // Construct the path to the image file
        var filePath = Path.Combine(environment.ContentRootPath, "Uploads", image.ImageMaterial!);

        // Check if the file exists
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound($"Image file for id: {id} not found");
        }

        // Return the physical file
        var contentType = Path.GetExtension(filePath).ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".bmp" => "image/bmp",
            ".gif" => "image/gif",
            ".tiff" => "image/tiff",
            _ => "application/octet-stream"
        };

        return PhysicalFile(filePath, contentType, fileDownloadName: image.ImageName, enableRangeProcessing: true);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetImages()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }
        var images = await imageRepo.GetImagesByUserId(userId);
        return Ok(images);
    }


    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateImage(int id, [FromBody] string newName)
    {
        try
        {
            var existingImage = await imageRepo.FindImageByIdAsync(id);
            if (existingImage == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Image with id: {id} not found");
            }
            else if (String.IsNullOrEmpty(newName) || newName.Length > 30) 
            {
                return StatusCode(StatusCodes.Status400BadRequest, "The new name is not valid");
            }
 
            existingImage.ImageName = newName;
            
            var updatedImage = await imageRepo.UpdateImageAsync(existingImage);

            return Ok(updatedImage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
