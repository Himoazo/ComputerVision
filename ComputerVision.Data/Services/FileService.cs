using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Runtime.Versioning;
using ComputerVision.Data.Services;
using ComputerVision.Data.Models.DTO;

namespace ImageManipulation.Data.Services;

public interface IFileService
{
    Task<(string fileName, string fileNameWithPath)> SaveFileAsync(IFormFile imageFile, FilterType filter, string[] allowedFileExtensions);
    void DeleteFile(string fileNameWithExtension);
}
//[SupportedOSPlatform("windows")]
public class FileService(IWebHostEnvironment environment) : IFileService
{

    public async Task<(string fileName, string fileNameWithPath)> SaveFileAsync(IFormFile imageFile, FilterType filter, string[] allowedFileExtensions)
    {
        if (imageFile == null)
        {
            throw new ArgumentNullException(nameof(imageFile));
        }

        var contentPath = environment.ContentRootPath;
        var path = Path.Combine(contentPath, "Uploads");
        

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // Check the allowed extenstions
        var ext = Path.GetExtension(imageFile.FileName);
        if (!allowedFileExtensions.Contains(ext))
        {
            throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
        }

        // generate a unique filename
        var fileName = $"{Guid.NewGuid().ToString()}{ext}";
        var fileNameWithPath = Path.Combine(path, fileName);
        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
        await imageFile.CopyToAsync(stream);
        stream.Close();

        try
        {
            ImageProcessor imageProcessor = new ImageProcessor();
            RGBTriple[,] imageBitmap = imageProcessor.LoadImage(fileNameWithPath);

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
            
            imageProcessor.SaveImage(fileNameWithPath, imageBitmap);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        var imageUrl = $"/api/images/{fileName}";
        return (fileName, imageUrl);
    }


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