namespace ImageManipulation.Data.Services;

public class RGBTriple
    {

    //Properties som lagrar de blåa, gröna och röda färgvärdena för en pixel
    public byte Blue { get; set; }
        public byte Green { get; set; }
        public byte Red { get; set; }

    // Constructor utan parametrar som gör det möjligt att skapa en RGBTriple-instans utan att ställa in initiala värden.
    public RGBTriple() { }

    //Initierar red blå och grön när de inisieras med värden.
        public RGBTriple(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
