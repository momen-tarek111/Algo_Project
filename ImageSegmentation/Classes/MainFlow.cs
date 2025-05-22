using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ImageTemplate.Classes
{
 
    public static class MainFlow
    {
        public static RGBPixel[,] First(RGBPixel [,] image)
        {
            Vertix[,] verticesR;
            Vertix[,] verticesG;
            Vertix[,] verticesB;
            Dictionary<int, int> pixelCounts;
            Stopwatch timer = Stopwatch.StartNew();
            (verticesR, verticesG, verticesB) = RGBColor.construncGraph(image);
            Parallel.Invoke(
                () =>
                {
                    verticesG = SegmentationLogic(verticesG, data.edgesG);
                },
                () =>
                {
                    verticesR = SegmentationLogic(verticesR, data.edgesR);
                },
                () =>
                {
                    verticesB = SegmentationLogic(verticesB, data.edgesB);
                }
            );
            RGBPixel[,] outputImage = CombineAndVisualize(verticesR, verticesG, verticesB, out pixelCounts);
            data.time = timer.ElapsedMilliseconds;
            Dictionary<int, int> sortedDict = SortByValueDescending(pixelCounts).ToDictionary(pair => pair.Key, pair => pair.Value);
            timer.Stop();
            DictionaryWriter.WriteValuesToFile(sortedDict, "C:\\Users\\karas\\Desktop\\output_project_algo.txt", "C:\\Users\\karas\\Desktop\\Time.txt");
            return outputImage;
        }
        public static Vertix[,] SegmentationLogic(Vertix[,] graph, List<int>[] edges)
        {
            Stopwatch timer = Stopwatch.StartNew();
            int height = graph.GetLength(0);
            int width = graph.GetLength(1);
           
            for (int i = 0; i < 256; i++)
            {
                if (edges[i] == null)
                    continue;

                for(int j=0;j<edges[i].Count;j+=2)
                {
                    Vertix v1 = graph[(edges[i][j] / width), ((edges[i][j]) - ((edges[i][j] / width) * width))];
                    Vertix v2 = graph[(edges[i][j+1] / width), ((edges[i][j+1]) - ((edges[i][j+1] / width) * width))];
                    var parent1 = data.Find(v1);
                    var parent2 = data.Find(v2);
                    if (parent1 == parent2)
                        continue;

                    double intensityC1 = parent1.Component.MaxInternalWeight;
                    double intensityC2 = parent2.Component.MaxInternalWeight;
                    double threshold1 = (double)data.K / parent1.Component.VertixCount;
                    double threshold2 = (double)data.K / parent2.Component.VertixCount;
                    double min = Math.Min(intensityC1 + threshold1, intensityC2 + threshold2);

                    if (min >= i)
                    {
                        data.Union(v1, v2, i);
                    }
                }
            }
            //Dictionary<Vertix, int> componentIds = new Dictionary<Vertix, int>();
            //int compCounter = 1;
            //for (int i = 0; i < graph.GetLength(0); i++)
            //{
            //    for (int j = 0; j < graph.GetLength(1); j++)
            //    {
            //        var root = data.Find(graph[i, j]);

            //        if (!componentIds.ContainsKey(root))
            //        {
            //            componentIds[root] = compCounter++;
            //        }

            //        graph[i, j].Component.ComponentId = componentIds[root];
            //    }
            //}
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
                    else
                    {
                        redGraph[i, j].Component.ComponentId = data.Find(redGraph[i, j]).Component.ComponentId;
                        greenGraph[i, j].Component.ComponentId = data.Find(greenGraph[i, j]).Component.ComponentId;
                        blueGraph[i, j].Component.ComponentId = data.Find(blueGraph[i, j]).Component.ComponentId;
                    }
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
                            redGraph[nx, ny].Component.ComponentId = data.Find(redGraph[nx, ny]).Component.ComponentId;
                            greenGraph[nx, ny].Component.ComponentId = data.Find(greenGraph[nx, ny]).Component.ComponentId;
                            blueGraph[nx, ny].Component.ComponentId = data.Find(blueGraph[nx, ny]).Component.ComponentId;
                            redGraph[x, y].Component.ComponentId = data.Find(redGraph[x, y]).Component.ComponentId;
                            greenGraph[x, y].Component.ComponentId = data.Find(greenGraph[x, y]).Component.ComponentId;
                            blueGraph[x, y].Component.ComponentId = data.Find(blueGraph[x, y]).Component.ComponentId;
                            if
                            (
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
        
        
        #region Merge Insertion sort
       
        public static List<KeyValuePair<int, int>> SortByValueDescending(Dictionary<int, int> dict)
        {
            var list = dict.ToList();
            var temp = new KeyValuePair<int, int>[list.Count];
            MergeInsertionSort(list, temp, 0, list.Count - 1);
            return list;
        }

        private static void MergeInsertionSort(List<KeyValuePair<int, int>> list, KeyValuePair<int, int>[] temp, int left, int right)
        {
            if (right - left <= 32)
            {
                InsertionSort(list, left, right);
                return;
            }
            int mid = (left + right) / 2;
            MergeInsertionSort(list, temp, left, mid);
            MergeInsertionSort(list, temp, mid + 1, right);
            Merge(list, temp, left, mid, right);
        }

        private static void InsertionSort(List<KeyValuePair<int, int>> list, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                var key = list[i];
                int j = i - 1;

                while (j >= left && list[j].Value < key.Value) // Descending
                {
                    list[j + 1] = list[j];
                    j--;
                }
                list[j + 1] = key;
            }
        }

        private static void Merge(List<KeyValuePair<int, int>> list, KeyValuePair<int, int>[] temp, int left, int mid, int right)
        {
            int i = left, j = mid + 1, k = left;

            while (i <= mid && j <= right)
            {
                temp[k++] = (list[i].Value >= list[j].Value) ? list[i++] : list[j++];
            }

            while (i <= mid) temp[k++] = list[i++];
            while (j <= right) temp[k++] = list[j++];

            for (int idx = left; idx <= right; idx++)
                list[idx] = temp[idx];
        }
        #endregion 

    }
}

