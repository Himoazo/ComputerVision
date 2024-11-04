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
    //Metoden för att skapa bild via POST anrop till api/images
    [HttpPost]
    [Authorize] //kräver inloggning
    public async Task<IActionResult> CreateImage([FromForm] ImageDTO imageToAdd)
    {
        try
        {
            if (imageToAdd.ImageFile is null) //Kontrollerar om filen finns
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Invalid file");
            }
            // kontrollerar att filstorleken inte stiger över 1mb
            if (imageToAdd.ImageFile.Length > 1 * 1024 * 1024)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "File size should not exceed 1 MB");
            }

            //Array av de tillåtna filtyperna
            string[] allowedFileExtensions = [".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff"];
            
            //Anropar SaveFileAsync för loading och applicering av bildfilter på bilden
            var (createdImageName, createdImageUrl) = await fileService.SaveFileAsync(imageToAdd.ImageFile, imageToAdd.Filter, allowedFileExtensions);

            //Hämtar userID från JWT Claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) { return Unauthorized(); }

            // Anger parameterarna som kom med ImageDTO till Image.
            var image = new Image
            {
                ImageName = imageToAdd.ImageName,
                ImageMaterial = createdImageName,
                ImageUrl = createdImageUrl,
                UserId = userId
            };

            //Spara bild data i databasen
            var createdImage = await imageRepo.AddImageAsync(image);

            return CreatedAtAction(nameof(CreateImage), createdImage); // 201 status
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message); //loggar errors
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Metoden för att radera en bild vid delete anrop på api/images/id 
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteImage(int id)
    {
        try
        {
            //Kontroll om bilden finns
            var existingImage = await imageRepo.FindImageByIdAsync(id);
            if (existingImage == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Image with id: {id} not found");
            }
            //Radera bild data från databasen
            await imageRepo.DeleteImageAsync(existingImage);

            // Radera själva bilden från Uploads mappen
            fileService.DeleteFile(existingImage.ImageMaterial!);
            return NoContent();  // returnerar 204
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    // Hämtar specifik bild med ID via GET anrop på api/images/id
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetImage(int id)
    {
        // Hämtar bildens uppgifter med hjälp av bildens ID
        var image = await imageRepo.FindImageByIdAsync(id);

        // Kontroll om bild info finns i image
        if (image == null)
        {
            return NotFound($"Image with id: {id} not found");
        }

        // Skapar url till bilden
        var filePath = Path.Combine(environment.ContentRootPath, "Uploads", image.ImageMaterial!);

        // Kontroll att bildfilen existerar
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound($"Image file for id: {id} not found");
        }

        // Sätter filensändelse. Swtich statement som går genom de stödjade filtyperna
        var contentType = Path.GetExtension(filePath).ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".bmp" => "image/bmp",
            ".gif" => "image/gif",
            ".tiff" => "image/tiff",
            _ => "application/octet-stream"
        };

        // returnerar själva filen (inte bara filuppgifterna) och skicka den till klienten i nedladdningsbar form
        return PhysicalFile(filePath, contentType, fileDownloadName: image.ImageName, enableRangeProcessing: true);
    }
    
    // Hämtar alla bilder som är uppladdade av användaren via GET api/images
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetImages()
    {
        //Hämtar userID från JWT claims
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
        {
            return Unauthorized();
        }
        // hämtar bilderna som användaren har laddat/ som innehåller användarens ID
        var images = await imageRepo.GetImagesByUserId(userId);
        return Ok(images);
    }

    // Obs denna redigerar endast bildnamnet inget annat. Via PUT anrop till api/images/id
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateImage(int id, [FromBody] string newName)
    {
        try
        {
            //hämtar bilden med hjälp av bildens ID
            var existingImage = await imageRepo.FindImageByIdAsync(id);
            if (existingImage == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, $"Image with id: {id} not found");
            }
            else if (String.IsNullOrEmpty(newName) || newName.Length > 30) //Kontrollerar om nya namnet är valid
            {
                return StatusCode(StatusCodes.Status400BadRequest, "The new name is not valid");
            }
 
            existingImage.ImageName = newName; //Updaterar bildnamnet
            
            var updatedImage = await imageRepo.UpdateImageAsync(existingImage); //Updaterar i databasen

            return Ok(updatedImage);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
