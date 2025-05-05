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


    public static class MainFlow
    {
        public static void First(RGBPixel [,] image)
        {
            Vertix[,] verticesR ;
            Vertix[,] verticesG;
            Vertix[,] verticesB;
            (verticesR, verticesG, verticesB) = MakeGraph(image,new RGBColor());
            //Console.WriteLine("cancelo");

            verticesB = SegmentationLogic(verticesB, data.edgesB);
            verticesG = SegmentationLogic(verticesG, data.edgesG);
            verticesR = SegmentationLogic(verticesR, data.edgesR);
        }
        public static (Vertix[,], Vertix[,], Vertix[,]) MakeGraph(RGBPixel[,] image , IColor color)
        {
            return color.construncGraph(image);
        }
        public static List<Edge> SortEdges(List<Edge> edges)
        {
            edges.Sort((e1, e2) => e1.Weight.CompareTo(e2.Weight));
            return edges;
        }
        public static List<Edge> SortEdges2(List<Edge> edges)
        {
            return edges.OrderBy(e => e.Weight)               // First by Weight (ascending)
                        .ThenBy(e => e.fromVertix.x)          // Then by fromVertix.x (ascending)
                        .ThenBy(e => e.fromVertix.y)          // Then by fromVertix.y (ascending)
                        .ThenBy(e => e.toVertix.x)            // Then by toVertix.x (ascending)
                        .ThenBy(e => e.toVertix.y)            // Finally by toVertix.y (ascending)
                        .ToList();
        }
        public static Vertix[,] SegmentationLogic(Vertix[,] graph , List<Edge> edges)
        {
            edges = SortEdges2(edges);

            foreach (var item in edges)
            {
                double diff = item.Weight;

                Vertix vertixTo = graph[item.toVertix.x, item.toVertix.y];
                Vertix vertixFrom = graph[item.fromVertix.x, item.fromVertix.y];



                double intensityC1 = vertixTo.Component.MaxInternalWeight;
                double intensityC2 = vertixFrom.Component.MaxInternalWeight;

                int k = 1;

                double threshold1 = (double)k / (double)vertixTo.Component.VertixCount;
                double threshold2 = (double)k / (double)vertixFrom.Component.VertixCount;
                
                double min = Math.Min(intensityC1 + threshold1, intensityC2 + threshold2);

                if (vertixTo.Component.ComponentId == 0)
                    vertixTo.Component.ComponentId = ++data.counter;
                if (vertixFrom.Component.ComponentId == 0)
                    vertixFrom.Component.ComponentId = ++data.counter;

                if (vertixTo.Component.ComponentId == vertixFrom.Component.ComponentId)
                    continue;

                if (min >= diff)
                {
                    if(vertixTo.Component.VertixCount > 1 || vertixFrom.Component.VertixCount > 1)
                    {
                        vertixFrom.Component.MaxInternalWeight = 
                            Math.Max(vertixFrom.Component.MaxInternalWeight,vertixTo.Component.MaxInternalWeight);
                    }
                    else
                    {
                        vertixFrom.Component.MaxInternalWeight = item.Weight;
                    }
                    
                    vertixFrom.Component.VertixCount += vertixTo.Component.VertixCount;

                    vertixTo.Component = vertixFrom.Component;

                }


            }

            return graph;
        }

        public static FinalAnsewer MakeFinalImage(Vertix[,] graph)
        {
            return null;
        }
    }
}
