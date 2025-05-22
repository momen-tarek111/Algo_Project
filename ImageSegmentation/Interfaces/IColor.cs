using ImageTemplate.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate.Interfaces
{
   
    public class Vertix
    {
        public long x;
        public long y;
        public Vertix Parent;
        public Component Component = new Component();
    }
    public class Component
    {
        public long ComponentId = 0;
        public double MaxInternalWeight = 0;
        public long VertixCount = 1;
    }
    static class data
    {
        public static int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1};
        public static int[] dy = { 0 , 0, 1, -1, 1, -1, 1,-1 };
        public static int[,] FinalLabels;
        public static RGBPixel[,] OriginImage;
        public static double time;
        public static long time2;
        public static long time3;
        public static long time4;
        public static int K;
        public static List<int>[] edgesR;
        public static List<int>[] edgesG;
        public static List<int>[] edgesB;
        public static bool isValid(int n,int m,int i, int j)
        {
            return i>=0 && j>=0 && i<n && j<m;
        }
        public static int counter = 0;
        public static Vertix Find(Vertix v)
        {
            Vertix temp = v;
            while (temp.Parent != temp)
                temp = temp.Parent;
            v.Parent = temp;
            return temp;
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
                if (root2.Component.ComponentId == 0)
                {
                    root2.Component.ComponentId = ++data.counter;
                }
            }
            else
            {
                root2.Parent = root1;
                root1.Component.VertixCount += root2.Component.VertixCount;
                root1.Component.MaxInternalWeight = Math.Max(
                    Math.Max(root2.Component.MaxInternalWeight
                    , edgeWeight), root1.Component.MaxInternalWeight);
                if (root1.Component.ComponentId == 0)
                {
                    root1.Component.ComponentId = ++data.counter;
                }
            }
        }
    }
    
}
