using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
namespace ImageManipulation.Data.Services;


public class ImageProcessor
{
    public RGBTriple[,] LoadImage(string filepath)
    {
        using (Image<Rgba32> bitmap = Image.Load<Rgba32>(filepath))
        {
            
            int width = bitmap.Width;
            int height = bitmap.Height;
            RGBTriple[,] image = new RGBTriple[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var pixel = bitmap[j, i];
                    image[i, j] = new RGBTriple(pixel.R, pixel.G, pixel.B);
                }
            }
            return image;
        }
    }

    public void SaveImage(string filepath, RGBTriple[,] image)
    {
        int height = image.GetLength(0);
        int width = image.GetLength(1);
        
        using (var bitmap = new Image<Rgba32>(width, height))
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    bitmap[j, i] = new Rgba32(image[i, j].Red, image[i, j].Green, image[i, j].Blue);
                }
            }
            
            bitmap.Save(filepath);
        }
    }
}
