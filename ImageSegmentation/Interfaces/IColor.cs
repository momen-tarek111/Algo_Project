using ImageTemplate.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate.Interfaces
{
    public class Edge
    {
        public Vertix Vertix { set; get; }
        public double Weight { set; get; }
    }
    public class Vertix
    {
        public long x;
        public long y;
        public List<Edge> Edges = new List<Edge>();
        public Component Component = new Component();
    }
    static class data
    {
        public static int[] dx = { -1, 1, 0, 0, -1, -1,1,1};
        public static int[] dy = { 0,0,1,-1,1,-1,1,-1 };
        public static bool isValid(int n,int m,int i, int j)
        {
            
            return i>=0 && j>=0 && i<n && j<m;
        }
    }
    public interface IColor
    {
        (Vertix[,], Vertix[,], Vertix[,]) construncGraph(RGBPixel [,] image);
    }
}
