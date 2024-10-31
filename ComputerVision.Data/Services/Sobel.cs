namespace ImageManipulation.Data.Services;
public class Sobel
{
    public void EdgeDetect(int height, int width, RGBTriple[,] image)
    {
        RGBTriple[,] temp = new RGBTriple[height, width];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                float blueX = 0, greenX = 0, redX = 0;
                float blueY = 0, greenY = 0, redY = 0;

                for (int k = i - 1; k <= i + 1; k++)
                {
                    for (int l = j - 1; l <= j + 1; l++)
                    {
                        if (k < 0 || k >= height || l < 0 || l >= width)
                            continue;

                        int xFactor = 0, yFactor = 0;

                        if (l == j - 1) xFactor = -1;
                        else if (l == j + 1) xFactor = 1;
                        if (k == i - 1) yFactor = -1;
                        else if (k == i + 1) yFactor = 1;

                        if (k == i) yFactor *= 2;
                        if (l == j) xFactor *= 2;

                        blueX += image[k, l].Blue * xFactor;
                        greenX += image[k, l].Green * xFactor;
                        redX += image[k, l].Red * xFactor;

                        blueY += image[k, l].Blue * yFactor;
                        greenY += image[k, l].Green * yFactor;
                        redY += image[k, l].Red * yFactor;
                    }
                }

                int sobelBlue = (int)Math.Round(Math.Sqrt((blueX * blueX) + (blueY * blueY)));
                int sobelGreen = (int)Math.Round(Math.Sqrt((greenX * greenX) + (greenY * greenY)));
                int sobelRed = (int)Math.Round(Math.Sqrt((redX * redX) + (redY * redY)));

                temp[i, j] = new RGBTriple(
                    (byte)Math.Min(sobelRed, 255),
                    (byte)Math.Min(sobelGreen, 255),
                    (byte)Math.Min(sobelBlue, 255)
                );
            }
        }

        // Replace the original image with the Sobel-filtered result
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                image[i, j] = temp[i, j];
            }
        }
    }
}
