using ImageManipulation.Data.Services;

namespace ComputerVision.Data.Services;

public class Blur
{
    // Suddighets filter
    public void BlurFilter (int height, int width, RGBTriple[,] image)
    {
        RGBTriple[,] temp = new RGBTriple[height, width]; //temporär 2d array

        float n = 0;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int blue = 0;
                int green = 0;
                int red = 0;

                n = 0;

                for (int k = i - 3; k <= i + 3; k++)  //obs iteration 3 steg bakot och 3 steg framåt
                {
                    if (k < 0 || k >= height)
                    {
                        continue;
                    }

                    for (int l = j - 3; l <= j + 3; l++) //obs iteration 3 steg bakot och 3 steg framåt
                    {
                        if (l < 0 || l >= width)
                        {
                            continue;
                        }
                        blue += image[k, l].Blue;
                        green += image[k, l].Green;
                        red += image[k, l].Red;
                        n++;
                    }
                }
                temp[i, j] = new RGBTriple
                {
                    Blue = (byte)Math.Round(blue / (float)n),
                    Green = (byte)Math.Round(green / (float)n),
                    Red = (byte)Math.Round(red / (float)n)
                };
            }
        }

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                image[i, j] = temp[i, j];
            }
        }
    }   
}
