using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ComputerVision.Data.Services;
using ComputerVision.Data.Models.DTO;

namespace ImageManipulation.Data.Services;

//Interface definierar filhantering för att spara och radera bilder
public interface IFileService
{
    // Sparar en uppladdad bild med valt filter
    Task<(string fileName, string fileNameWithPath)> SaveFileAsync(IFormFile imageFile, FilterType filter, string[] allowedFileExtensions);
    // Raderar bild
    void DeleteFile(string fileNameWithExtension);
}

// Klass som implementerar IFileService
public class FileService(IWebHostEnvironment environment) : IFileService
{

    public async Task<(string fileName, string fileNameWithPath)> SaveFileAsync(IFormFile imageFile, FilterType filter, string[] allowedFileExtensions)
    {
        // Bilden ska inte vara null
        if (imageFile == null)
        {
            throw new ArgumentNullException(nameof(imageFile));
        }

        //Skapa sökvägen för Uploads mappen
        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, "Uploads");
        
        //Skapa Uploads mappen om den inte finns
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // Kontrollera tillåtna filtyper
        var ext = Path.GetExtension(imageFile.FileName);
        if (!allowedFileExtensions.Contains(ext))
        {
            throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
        }

        // genererar ett unikt filnamn 
        var fileName = $"{Guid.NewGuid().ToString()}{ext}";
        var fileNameWithPath = Path.Combine(path, fileName);

        // Kopiera den uppladdade bilden till servern
        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        stream.Close();

        try
        {
            // Ladda bilden med ImageProcessor
            ImageProcessor imageProcessor = new ImageProcessor();
            RGBTriple[,] imageBitmap = imageProcessor.LoadImage(fileNameWithPath);

            // Kör användarens valt filter på bilden
            switch (filter)
            {
                case FilterType.blur:
                    Blur blur = new Blur();
                    blur.BlurFilter(imageBitmap.GetLength(0), imageBitmap.GetLength(1), imageBitmap);
                    break;
                case FilterType.reflect:
                    Reflect reflect = new Reflect();
                    reflect.ReflectFilter(imageBitmap.GetLength(0), imageBitmap.GetLength(1), imageBitmap);
                    break;
                case FilterType.grayscale:
                    Grayscale grayscale = new Grayscale();
                    grayscale.GrayFilter(imageBitmap.GetLength(0), imageBitmap.GetLength(1), imageBitmap);
                    break;
                case FilterType.sobel:
                    Sobel sobel = new Sobel();
                    sobel.EdgeDetect(imageBitmap.GetLength(0), imageBitmap.GetLength(1), imageBitmap);
                    break;
                default:
                    throw new ArgumentException($"Unsupported filter type {filter}");
            }
            
            //Spara den filtrerade bilden
            imageProcessor.SaveImage(fileNameWithPath, imageBitmap);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        //Returnera filnamnet och Url
        var imageUrl = $"/api/images/{fileName}";
        return (fileName, imageUrl);
    }


    //Radera en sparad bild från Uploads
    public void DeleteFile(string fileNameWithExtension)
    {
        if (string.IsNullOrEmpty(fileNameWithExtension))
        {
            throw new ArgumentNullException(nameof(fileNameWithExtension));
        }
        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, $"Uploads", fileNameWithExtension);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Invalid file path");
        }
        File.Delete(path);
    }

}