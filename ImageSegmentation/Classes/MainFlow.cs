using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate.Classes
{
    public class FinalAnsewer
    {
        public RGBPixel[,] FinalImage;
        public int NumOfComponents;
    }
    public class Component
    {
        public long ComponentId = 0;
        public double MinInternalWeight = 1e12;
        public RGBPixel UniqueColor = new RGBPixel();
        public long VertixCount = 1;
    }

    public static class MainFlow
    {
        public static void First(RGBPixel [,] image)
        {
            Vertix[,] verticesR ;
            Vertix[,] verticesG;
            Vertix[,] verticesB;
            (verticesR, verticesG, verticesB) =MakeGraph(image,new RGBColor());
            Console.WriteLine("cancelo");

        }
        public static (Vertix[,], Vertix[,], Vertix[,]) MakeGraph(RGBPixel[,] image , IColor color)
        {
            return color.construncGraph(image);
        }
        public static Vertix[,] MakeSegmentation(Vertix[,] graph)
        {
            // make segmentation here
            return null;
        }
        public static FinalAnsewer MakeFinalImage(Vertix[,] graph)
        {
            return null;
        }
    }
}
