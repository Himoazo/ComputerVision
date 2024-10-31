using ComputerVision.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ComputerVision.Data.Repositories;

public interface IImageRepository
{
    Task<Image> AddImageAsync(Image image);
    Task<Image> UpdateImageAsync(Image image);
    Task<IEnumerable<Image>> GetImagesAsync();
    Task<IEnumerable<Image>> GetImagesByUserId(string userId);
    Task<Image?> FindImageByIdAsync(int id);
    Task DeleteImageAsync(Image image);
}

public class ImageRepository(ApplicationDbContext context) : IImageRepository
{
    
    public async Task<Image> AddImageAsync(Image image)
    {
        context.Images.Add(image);
        await context.SaveChangesAsync();
        return image; 
    }

    public async Task<Image> UpdateImageAsync(Image image)
    {
        context.Images.Update(image);
        await context.SaveChangesAsync();
        return image;
    }

    public async Task DeleteImageAsync(Image image)
    {
        context.Images.Remove(image);
        await context.SaveChangesAsync();
    }

    public async Task<Image?> FindImageByIdAsync(int id)
    {
        var image = await context.Images.FindAsync(id);
        return image;
    }

    public async Task<IEnumerable<Image>> GetImagesAsync()
    {
        return await context.Images.ToListAsync();
    }

    public async Task<IEnumerable<Image>> GetImagesByUserId(string userId)
    {
        return await context.Images
            .Where(x => x.UserId == userId).ToListAsync();
    }
}