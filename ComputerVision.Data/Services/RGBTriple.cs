namespace ImageManipulation.Data.Services;

public class RGBTriple
    {

    //Properties som lagrar de bl�a, gr�na och r�da f�rgv�rdena f�r en pixel
    public byte Blue { get; set; }
        public byte Green { get; set; }
        public byte Red { get; set; }

    // Constructor utan parametrar som g�r det m�jligt att skapa en RGBTriple-instans utan att st�lla in initiala v�rden.
    public RGBTriple() { }

    //Initierar red bl� och gr�n n�r de inisieras med v�rden.
        public RGBTriple(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
