namespace ImageManipulation.Data.Services;
public class Sobel
{
    // Metod som implementerar Sobel operatorn
    public void EdgeDetect(int height, int width, RGBTriple[,] image)
    {
        // temporär arr för att lagra den bearbetade array:n
        RGBTriple[,] temp = new RGBTriple[height, width];

        // Loopa genom varje pixel i bilden
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                // lagrar totala x och y värden
                float blueX = 0, greenX = 0, redX = 0;
                float blueY = 0, greenY = 0, redY = 0;

                // loopa genom 3x3 rutnätet runt varje pixel
                for (int k = i - 1; k <= i + 1; k++)
                {
                    for (int l = j - 1; l <= j + 1; l++)
                    {
                        // ignorerar pixlarna utanför bilden
                        if (k < 0 || k >= height || l < 0 || l >= width)
                        {
                            continue;
                        }
                            
                        // Avgör multiplicering med -1 och 1 enligt pixelns position
                        int xFactor = 0, yFactor = 0;

                        if (l == j - 1) 
                        { 
                            xFactor = -1; 
                        }
                        else if (l == j + 1) 
                        { 
                            xFactor = 1; 
                        }

                        if (k == i - 1) 
                        { 
                            yFactor = -1; 
                        }
                        else if (k == i + 1) 
                        { 
                            yFactor = 1; 
                        }

                        // x2 för mitten pixlarna
                        if (k == i) 
                        { 
                            yFactor *= 2; 
                        }

                        if (l == j) 
                        { 
                            xFactor *= 2; 
                        }

                        // multiplicera i x och y riktning och akumulera värden för varje färgkanal
                        blueX += image[k, l].Blue * xFactor;
                        greenX += image[k, l].Green * xFactor;
                        redX += image[k, l].Red * xFactor;

                        blueY += image[k, l].Blue * yFactor;
                        greenY += image[k, l].Green * yFactor;
                        redY += image[k, l].Red * yFactor;
                    }
                }

                // addera summorna för X axel upphöjt till två med summorna för Y axel upp-höjt till två
                // och beräkna kvadratroten av resultatet
                int sobelBlue = (int)Math.Round(Math.Sqrt((blueX * blueX) + (blueY * blueY)));
                int sobelGreen = (int)Math.Round(Math.Sqrt((greenX * greenX) + (greenY * greenY)));
                int sobelRed = (int)Math.Round(Math.Sqrt((redX * redX) + (redY * redY)));

                //begränsa värdena till max 255 för att passa inom byte
                temp[i, j] = new RGBTriple(
                    (byte)Math.Min(sobelRed, 255),
                    (byte)Math.Min(sobelGreen, 255),
                    (byte)Math.Min(sobelBlue, 255)
                );
            }
        }

        // Byt ut orginal array med Sobel array
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                image[i, j] = temp[i, j];
            }
        }
    }
}
