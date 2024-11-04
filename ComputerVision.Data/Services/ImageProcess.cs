using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
namespace ImageManipulation.Data.Services;

// ImageProcessor laddar och sparar bilder med hjälp av ImageSharp
public class ImageProcessor
{
    // metod som laddar bildens bitmap i 2d array och returnerar den 
    public RGBTriple[,] LoadImage(string filepath)
    {
        using (Image<Rgba32> bitmap = Image.Load<Rgba32>(filepath))
        {
            
            int width = bitmap.Width;
            int height = bitmap.Height;
            RGBTriple[,] image = new RGBTriple[height, width];

            // Loopa över den inlästa bilden och spara pixel info i image
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

    // skapar den färdig filtrerade bilden och sparar den i filepath. obs filepath innehåller filändelsen
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
