namespace ImageManipulation.Data.Services;

public class RGBTriple
    {
        public byte Blue { get; set; }
        public byte Green { get; set; }
        public byte Red { get; set; }

        public RGBTriple() { }

        public RGBTriple(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }
    }
