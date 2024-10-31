
using ImageManipulation.Data.Services;

namespace ComputerVision.Data.Services;

public class Reflect
{
    public void ReflectFilter(int height, int width, RGBTriple[,] image)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width / 2; j++)
            {
                RGBTriple temp = image[i, j];
                image[i, j] = image[i, width - 1 - j];
                image[i, width - 1 - j] = temp;
            }
        }
    }
}
