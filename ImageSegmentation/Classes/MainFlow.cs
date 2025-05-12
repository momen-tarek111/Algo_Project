using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate.Classes
{
 
    public static class MainFlow
    {
        public static RGBPixel[,] First(RGBPixel [,] image)
        {
            Stopwatch timer = Stopwatch.StartNew();
            Vertix[,] verticesR ;
            Vertix[,] verticesG;
            Vertix[,] verticesB;
            (verticesR, verticesG, verticesB) = MakeGraph(image,new RGBColor());
            verticesB = SegmentationLogic(verticesB, data.edgesB);
            
            data.counter = 0;
            verticesG = SegmentationLogic(verticesG, data.edgesG);
            data.counter = 0;
            verticesR = SegmentationLogic(verticesR, data.edgesR);
            data.counter = 0;

            Dictionary<int, int> pixelCounts;
            RGBPixel[,] outputImage = CombineAndVisualize(verticesR, verticesG, verticesB, out pixelCounts);
            timer.Stop();
            data.time = timer.ElapsedMilliseconds;
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
        public static List<Edge> SortEdges2(List<Edge> edges)
        {
            return edges.OrderBy(e => e.Weight).ToList();
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
                double threshold1 = (double)data.K / (double)parent1.Component.VertixCount;
                double threshold2 = (double)data.K / (double)parent2.Component.VertixCount;
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
        public static RGBPixel[,] CombineAndVisualize(Vertix[,] redGraph, Vertix[,] greenGraph, Vertix[,] blueGraph,out Dictionary<int, int> regionPixelCounts)
        {
            int height = redGraph.GetLength(0);
            int width = redGraph.GetLength(1);
            RGBPixel[,] result = new RGBPixel[height, width];
            int[,] labels = new int[height, width];
            bool[,] visited = new bool[height, width];

            Dictionary<int, RGBPixel> labelToColor = new Dictionary<int, RGBPixel>();
            regionPixelCounts = new Dictionary<int, int>();
            Random rand = new Random();

            int RegionId = 1;
            data.FinalLabels = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (visited[i, j])
                        continue;

                    // Start a new region
                    int currentRegion = RegionId++;
                    RGBPixel regionColor = new RGBPixel(
                        (byte)rand.Next(256),
                        (byte)rand.Next(256),
                        (byte)rand.Next(256)
                    );

                    labelToColor[currentRegion] = regionColor;
                    regionPixelCounts[currentRegion] = 0;

                    Queue<(int, int)> queue = new Queue<(int, int)>();
                    queue.Enqueue((i, j));
                    visited[i, j] = true;

                    while (queue.Count > 0)
                    {
                        var (x, y) = queue.Dequeue();

                        labels[x, y] = currentRegion;
                        data.FinalLabels[x, y] = currentRegion;
                        result[x, y] = regionColor;
                        regionPixelCounts[currentRegion]++;

                        for (int k = 0; k < 8; k++)
                        {
                            int nx = x + data.dx[k];
                            int ny = y + data.dy[k];

                            if (!data.isValid(height, width, nx, ny))
                                continue;
                            if (visited[nx, ny])
                                continue;

                            if (
                                redGraph[x, y].Component.ComponentId == redGraph[nx, ny].Component.ComponentId &&
                                greenGraph[x, y].Component.ComponentId == greenGraph[nx, ny].Component.ComponentId &&
                                blueGraph[x, y].Component.ComponentId == blueGraph[nx, ny].Component.ComponentId
                            )
                            {
                                queue.Enqueue((nx, ny));
                                visited[nx, ny] = true;
                            }
                        }
                    }
                }
            }
            return result;
        }


    }
}

        //public static RGBPixel[,] CombineAndVisualize(Vertix[,] redGraph, Vertix[,] greenGraph, Vertix[,] blueGraph, out Dictionary<int, int> regionPixelCounts)
        //{
        //    int height = redGraph.GetLength(0);
        //    int width = redGraph.GetLength(1);
        //    RGBPixel[,] result = new RGBPixel[height, width];
        //    Dictionary<string, int> labelMap = new Dictionary<string, int>();
        //    Dictionary<int, RGBPixel> labelToColor = new Dictionary<int, RGBPixel>();
        //    regionPixelCounts = new Dictionary<int, int>();
        //    Random rand = new Random();
        //    int labelCounter = 1;
        //    data.FinalLabels = new int[height, width];
        //    for (int i = 0; i < height; i++)
        //    {
        //        for (int j = 0; j < width; j++)
        //        {
        //            long redLabel = redGraph[i, j].Component.ComponentId;
        //            long greenLabel = greenGraph[i, j].Component.ComponentId;
        //            long blueLabel = blueGraph[i, j].Component.ComponentId;
        //            string key = $"{redLabel}{greenLabel}{blueLabel}";
        //            if (!labelMap.ContainsKey(key))
        //            {
        //                labelMap[key] = labelCounter++;
        //            }
        //            int finalLabel = labelMap[key];
        //            if (!labelToColor.ContainsKey(finalLabel))
        //            {
        //                byte r = (byte)rand.Next(256);
        //                byte g = (byte)rand.Next(256);
        //                byte b = (byte)rand.Next(256);
        //                labelToColor[finalLabel] = new RGBPixel(r, g, b);
        //            }
        //            if (!regionPixelCounts.ContainsKey(finalLabel))
        //            {
        //                regionPixelCounts[finalLabel] = 0;
        //            }
        //            regionPixelCounts[finalLabel]++;
        //            result[i, j] = labelToColor[finalLabel];
        //            data.FinalLabels[i, j] = finalLabel;
        //        }
        //    }
        //    return result;
        //}
        //public static RGBPixel[,] CombineAndVisualize(Vertix[,] redGraph, Vertix[,] greenGraph, Vertix[,] blueGraph, out Dictionary<int, int> regionPixelCounts)
        //{
        //    int height = redGraph.GetLength(0);
        //    int width = redGraph.GetLength(1);
        //    RGBPixel[,] result = new RGBPixel[height, width];
        //    //Dictionary<string, int> labelMap = new Dictionary<string, int>();
        //    Dictionary<int, RGBPixel> labelToColor = new Dictionary<int, RGBPixel>();
        //    regionPixelCounts = new Dictionary<int, int>();
        //    bool[,] visited = new bool[height, width];
        //    Random rand = new Random();
        //    int RegionId = 0;
        //    data.FinalLabels = new int[height, width];
        //    for (int i = 0; i < height; i++)
        //    {
        //        for (int j = 0; j < width; j++)
        //        {
        //            if (redGraph[i, j].RegionId == 0 && greenGraph[i, j].RegionId == 0 && blueGraph[i, j].RegionId == 0)
        //            {
        //                RegionId += 1;
        //                regionPixelCounts.Add(RegionId, 0);
        //                byte r = (byte)rand.Next(256);
        //                byte g = (byte)rand.Next(256);
        //                byte b = (byte)rand.Next(256);
        //                labelToColor[RegionId] = new RGBPixel(r, g, b);
        //                redGraph[i, j].RegionId = RegionId;
        //                greenGraph[i, j].RegionId = RegionId;
        //                blueGraph[i, j].RegionId = RegionId;
        //                regionPixelCounts[RegionId] += 1;
        //                result[i, j] = labelToColor[RegionId];
        //                data.FinalLabels[i, j] = RegionId;
        //            }
        //            for (int k = 0; k < 8; k++)
        //            {
        //                int x = i + data.dx[k];
        //                int y = j + data.dy[k];

        //                if (!data.isValid(height, width, x, y))
        //                    continue;
        //                if(visited[x, y])
        //                    continue;
        //                visited[x,y] = true;
        //                long redLabel = redGraph[x, y].Component.ComponentId;
        //                long greenLabel = greenGraph[x, y].Component.ComponentId;
        //                long blueLabel = blueGraph[x, y].Component.ComponentId;
        //                if (redLabel == redGraph[i, j].Component.ComponentId && greenLabel == greenGraph[i, j].Component.ComponentId && blueLabel == blueGraph[i, j].Component.ComponentId)
        //                {
        //                    int RegionIdNow = redGraph[i, j].RegionId;
        //                    redGraph[x, y].RegionId = RegionIdNow;
        //                    greenGraph[x, y].RegionId = RegionIdNow;
        //                    blueGraph[x, y].RegionId = RegionIdNow;
        //                    result[x, y] = labelToColor[RegionIdNow];
        //                    regionPixelCounts[RegionIdNow] += 1;
        //                    data.FinalLabels[x, y] = RegionIdNow;
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}