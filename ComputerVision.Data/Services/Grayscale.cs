
using ImageManipulation.Data.Services;

namespace ComputerVision.Data.Services;

internal class Grayscale
{
    // Metod som konverterar en bild till en svartvitt bild
    public void GrayFilter (int height, int width, RGBTriple[,] image)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int gray = image[i, j].Blue + image[i, j].Red + image[i, j].Green;
                image[i, j].Blue = (byte)Math.Round(gray / 3f); // (byte)Cast Math.Round till byte. 3f för att dividera med float
                image[i, j].Red = (byte)Math.Round(gray / 3f);
                image[i, j].Green = (byte)Math.Round(gray / 3f);
            }
        }
    }
}
