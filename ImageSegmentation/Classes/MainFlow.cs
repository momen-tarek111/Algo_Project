using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate.Classes
{
 
    public static class MainFlow
    {
        public static RGBPixel[,] First(RGBPixel [,] image)
        {
            Vertix[,] verticesR ;
            Vertix[,] verticesG;
            Vertix[,] verticesB;
            (verticesR, verticesG, verticesB) = MakeGraph(image,new RGBColor());

            verticesB = SegmentationLogic(verticesB, data.edgesB);
            verticesG = SegmentationLogic(verticesG, data.edgesG);
            verticesR = SegmentationLogic(verticesR, data.edgesR);
            
            Dictionary<int, int> pixelCounts;
            RGBPixel[,] outputImage = CombineAndVisualize(verticesR, verticesG, verticesB, out pixelCounts);
            data.edgesR.Clear();
            data.edgesG.Clear();
            data.edgesB.Clear();
            data.counter = 0;
            DictionaryWriter.WriteValuesToFile(pixelCounts, "C:\\Users\\karas\\Desktop\\output_project_algo.txt");
            return outputImage;
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


                var parent1 = data.Find(vertixTo);
                var parent2 = data.Find(vertixFrom);
                if (parent1 == parent2)
                    continue;

                double intensityC1 = parent1.Component.MaxInternalWeight;
                double intensityC2 = parent2.Component.MaxInternalWeight;
                int k = 1;

                double threshold1 = (double)k / (double)parent1.Component.VertixCount;
                double threshold2 = (double)k / (double)parent2.Component.VertixCount;
                
                double min = Math.Min(intensityC1 + threshold1, intensityC2 + threshold2);

                if (min >= diff)
                {
                    data.Union(parent1, parent2, diff);
                }
            }
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    var root = data.Find(graph[i, j]);
                    graph[i, j].Component.ComponentId = root.Component.ComponentId;
                }
            }
            return graph;
        }
        public static RGBPixel[,] CombineAndVisualize(Vertix[,] redGraph,Vertix[,] greenGraph,Vertix[,] blueGraph,out Dictionary<int, int> regionPixelCounts)
        {
            int height = redGraph.GetLength(0);
            int width = redGraph.GetLength(1);
            RGBPixel[,] result = new RGBPixel[height, width];
            Dictionary<string, int> labelMap = new Dictionary<string, int>();
            Dictionary<int, RGBPixel> labelToColor = new Dictionary<int, RGBPixel>();
            regionPixelCounts = new Dictionary<int, int>();
            Random rand = new Random();
            int labelCounter = 1;
            data.FinalLabels=new int[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    long redLabel = redGraph[i, j].Component.ComponentId;
                    long greenLabel = greenGraph[i, j].Component.ComponentId;
                    long blueLabel = blueGraph[i, j].Component.ComponentId;
                    string key = $"{redLabel}{greenLabel}{blueLabel}";
                    if (!labelMap.ContainsKey(key))
                    {
                        labelMap[key] = labelCounter++;
                    }
                    int finalLabel = labelMap[key];

                    // Assign random color if first time
                    if (!labelToColor.ContainsKey(finalLabel))
                    {
                        byte r = (byte)rand.Next(256);
                        byte g = (byte)rand.Next(256);
                        byte b = (byte)rand.Next(256);
                        labelToColor[finalLabel] = new RGBPixel(r, g, b);
                    }
                    // Count pixels in each final region
                    if (!regionPixelCounts.ContainsKey(finalLabel))
                    {
                        regionPixelCounts[finalLabel] = 0;
                    }
                    regionPixelCounts[finalLabel]++;

                    result[i, j] = labelToColor[finalLabel];

                    data.FinalLabels[i, j] = finalLabel;
                }
            }
            return result;
        }
    }
}
