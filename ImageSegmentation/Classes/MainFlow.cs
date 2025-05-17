using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

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

            Parallel.Invoke(
            () => {
                verticesG = SegmentationLogic(verticesG, data.edgesG);
                },
                () => {
                    verticesR = SegmentationLogic(verticesR, data.edgesR);
                },
                () => {
                    verticesB = SegmentationLogic(verticesB, data.edgesB);
                }
            );
            //verticesG = SegmentationLogic(verticesG, data.edgesG);
            //data.counter = 0;
            //verticesR = SegmentationLogic(verticesR, data.edgesR);
            //data.counter = 0;
            //verticesB = SegmentationLogic(verticesB, data.edgesB);
            //data.counter = 0;
            Dictionary<int, int> pixelCounts;
            HashSet<long> redIDs = new HashSet<long>();
            HashSet<long> greenIDs = new HashSet<long>();
            HashSet<long> blueIDs = new HashSet<long>();
            for (int i = 0; i < verticesR.GetLength(0); i++)
            {
                for (int j = 0; j < verticesR.GetLength(1); j++)
                {
                    redIDs.Add(verticesR[i, j].Component.ComponentId);
                    greenIDs.Add(verticesG[i, j].Component.ComponentId);
                    blueIDs.Add(verticesB[i, j].Component.ComponentId);
                }
            }
            Console.WriteLine($"Red Segments: {redIDs.Count}");
            Console.WriteLine($"Green Segments: {greenIDs.Count}");
            Console.WriteLine($"Blue Segments: {blueIDs.Count}");
            RGBPixel[,] outputImage = CombineAndVisualize(verticesR, verticesG, verticesB, out pixelCounts);
            for (int i = 0; i < verticesR.GetLength(0); i++)
            {
                for (int j = 0; j < verticesR.GetLength(1); j++)
                {
                    var root = data.Find(verticesR[i, j]);
                    if (root.Component.ComponentId != verticesR[i, j].Component.ComponentId)
                    {
                        Console.WriteLine($"❌ Mismatch at ({i},{j})");
                    }
                }
            }
            using (StreamWriter sw = new StreamWriter("C:\\Users\\karas\\Desktop\\final_labels_debug.txt"))
            {
                for (int i = 0; i < data.FinalLabels.GetLength(0); i++)
                {
                    for (int j = 0; j < data.FinalLabels.GetLength(1); j++)
                    {
                        sw.Write(data.FinalLabels[i, j] + " ");
                    }
                    sw.WriteLine();
                }
            }
            int totalPixels = image.GetLength(0) * image.GetLength(1);
            int countedPixels = pixelCounts.Values.Sum();
            Console.WriteLine($"✅ Total Pixels: {totalPixels}");
            Console.WriteLine($"✅ Counted Pixels: {countedPixels}");
            Console.WriteLine($"✅ Regions Found: {pixelCounts.Count}");
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
        public static Vertix[,] SegmentationLogic(Vertix[,] graph, List<Edge> edges)
        {
            edges = SortEdges2(edges);

            foreach (var item in edges)
            {
                double diff = item.Weight;

                Vertix v1 = graph[item.fromVertix.x, item.fromVertix.y];
                Vertix v2 = graph[item.toVertix.x, item.toVertix.y];

                var parent1 = data.Find(v1);
                var parent2 = data.Find(v2);

                if (parent1 == parent2)
                    continue;

                double intensityC1 = parent1.Component.MaxInternalWeight;
                double intensityC2 = parent2.Component.MaxInternalWeight;
                double threshold1 = (double)data.K / parent1.Component.VertixCount;
                double threshold2 = (double)data.K / parent2.Component.VertixCount;
                double min = Math.Min(intensityC1 + threshold1, intensityC2 + threshold2);

                if (min >= diff)
                {
                    data.Union(v1, v2, diff);
                }
            }

            Dictionary<Vertix, int> componentIds = new Dictionary<Vertix, int>();
            int compCounter = 1;

            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    var root = data.Find(graph[i, j]);

                    if (!componentIds.ContainsKey(root))
                    {
                        componentIds[root] = compCounter++;
                    }

                    graph[i, j].Component.ComponentId = componentIds[root];
                }
            }

            return graph;
        }

        public static RGBPixel[,] CombineAndVisualize(Vertix[,] redGraph, Vertix[,] greenGraph, Vertix[,] blueGraph,out Dictionary<int, int> regionPixelCounts)
        {
            int height = redGraph.GetLength(0);
            int width = redGraph.GetLength(1);
            RGBPixel[,] result = new RGBPixel[height, width];
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

