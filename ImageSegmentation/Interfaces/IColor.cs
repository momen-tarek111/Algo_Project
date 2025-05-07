using ImageTemplate.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate.Interfaces
{
    public class Edge : IComparable<Edge>
    {
        public Vertix toVertix { set; get; }
        public Vertix fromVertix { set; get; }
        public double Weight { set; get; }

        public int CompareTo(Edge other)
        {
            if (other == null) return 1;

            // 1. First by Weight (ascending)
            int weightComparison = this.Weight.CompareTo(other.Weight);
            if (weightComparison != 0) return weightComparison;

            // 2. Then by fromVertix.x (ascending)
            int fromXComparison = this.fromVertix.x.CompareTo(other.fromVertix.x);
            if (fromXComparison != 0) return fromXComparison;

            // 3. Then by fromVertix.y (ascending)
            int fromYComparison = this.fromVertix.y.CompareTo(other.fromVertix.y);
            if (fromYComparison != 0) return fromYComparison;

            // 4. Then by toVertix.x (ascending)
            int toXComparison = this.toVertix.x.CompareTo(other.toVertix.x);
            if (toXComparison != 0) return toXComparison;

            // 5. Finally by toVertix.y (ascending)
            return this.toVertix.y.CompareTo(other.toVertix.y);
        }
    }
    public class Vertix
    {
        public long x;
        public long y;
        public Vertix Parent;
        public List<Edge> Edges = new List<Edge>();
        public Component Component = new Component();
    }
    public class Component
    {
        public long ComponentId = 0;
        public double MaxInternalWeight = 0;
        public RGBPixel UniqueColor = new RGBPixel();
        public long VertixCount = 1;

    }
    static class data
    {
        public static int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1};
        public static int[] dy = { 0 , 0, 1, -1, 1, -1, 1,-1 };
        public static bool isValid(int n,int m,int i, int j)
        {
            
            return i>=0 && j>=0 && i<n && j<m;
        }
        public static int counter = 0;

        public static List<Edge> edgesG = new List<Edge>();
        public static List<Edge> edgesB = new List<Edge>();
        public static List<Edge> edgesR = new List<Edge>();

        public static Vertix Find(Vertix v)
        {
            if (v.Parent != v)
                v.Parent = Find(v.Parent);
            return v.Parent;
        }

        public static void Union(Vertix v1, Vertix v2, double edgeWeight)
        {
            var root1 = Find(v1);
            var root2 = Find(v2);

            if (root1 == root2)
                return;

            if (root1.Component.VertixCount < root2.Component.VertixCount)
            {
                root1.Parent = root2;
                root2.Component.VertixCount += root1.Component.VertixCount;
                root2.Component.MaxInternalWeight = Math.Max(
                    Math.Max(root2.Component.MaxInternalWeight
                    , edgeWeight), root1.Component.MaxInternalWeight);


                root2.Component.ComponentId = ++data.counter;
            }
            else
            {
                root2.Parent = root1;
                root1.Component.VertixCount += root2.Component.VertixCount;
                root1.Component.MaxInternalWeight = Math.Max(
                    Math.Max(root2.Component.MaxInternalWeight
                    , edgeWeight), root1.Component.MaxInternalWeight);

                root1.Component.ComponentId = ++data.counter;
            }
        }

    }
    public interface IColor
    {
        (Vertix[,], Vertix[,], Vertix[,]) construncGraph(RGBPixel [,] image);
    }
}
