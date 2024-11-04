
using ImageManipulation.Data.Services;

namespace ComputerVision.Data.Services;

public class Reflect
{
    //Metod som spegelvänder bilden
    public void ReflectFilter(int height, int width, RGBTriple[,] image)
    {
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width / 2; j++) //Loopar genom halva bredden
            {
                RGBTriple temp = image[i, j];  //Avlastnings variabel innan pixelbytet
                image[i, j] = image[i, width - 1 - j];
                image[i, width - 1 - j] = temp;
            }
        }
    }
}
