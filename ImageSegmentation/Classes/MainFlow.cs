using FasterSort;
using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
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
            Stopwatch timer2 = Stopwatch.StartNew();
            (verticesR, verticesG, verticesB) = MakeGraph(image,new RGBColor());
            timer2.Stop();
            data.time3 = timer2.ElapsedMilliseconds;
            Stopwatch timer3 = Stopwatch.StartNew();
            //Parallel.Invoke(
            //() =>
            //    {
            //        MergeInsertionSort(data.edgesR);
            //    },
            //    () =>
            //    {
            //        MergeInsertionSort(data.edgesG);
            //    },
            //    () =>
            //    {
            //        MergeInsertionSort(data.edgesB);
            //    }
            //);
            timer3.Stop();
            data.time2 = timer3.ElapsedMilliseconds;
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
            Dictionary<int, int> pixelCounts;
            RGBPixel[,] outputImage = CombineAndVisualize(verticesR, verticesG, verticesB, out pixelCounts);


            timer.Stop();
            data.time = timer.ElapsedMilliseconds;
            //data.edgesR.Clear();
            //data.edgesG.Clear();
            //data.edgesB.Clear();
            data.counter = 0;
            DictionaryWriter.WriteValuesToFile(pixelCounts, "C:\\Users\\karas\\Desktop\\output_project_algo.txt", "C:\\Users\\karas\\Desktop\\Time.txt");
            return outputImage;
        }
        public static (Vertix[,], Vertix[,], Vertix[,]) MakeGraph(RGBPixel[,] image , IColor color)
        {
            return color.construncGraph(image);
        }
        
        #region Merge Insertion Sort
        public static void MergeInsertionSort(KeyValuePair<KeyValuePair<int, int>, double>[] array)
        {
            var temp = new KeyValuePair<KeyValuePair<int, int>, double>[array.Length];
            MergeInsertionSort(array, temp, 0, array.Length - 1);
        }

        private static void MergeInsertionSort(KeyValuePair<KeyValuePair<int, int>, double>[] arr,KeyValuePair<KeyValuePair<int, int>, double>[] temp, int left, int right)
        {
            if (right - left <= 50)
            {
                InsertionSort(arr, left, right);
                return;
            }

            int mid = (left + right) / 2;
            MergeInsertionSort(arr, temp, left, mid);
            MergeInsertionSort(arr, temp, mid + 1, right);
            Merge(arr, temp, left, mid, right);
        }

        private static void InsertionSort(KeyValuePair<KeyValuePair<int, int>, double>[] arr, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                var key = arr[i];
                int j = i - 1;
                while (j >= left && arr[j].Value > key.Value)
                {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
        }

        private static void Merge(KeyValuePair<KeyValuePair<int, int>, double>[] arr, KeyValuePair<KeyValuePair<int, int>, double>[] temp, int left, int mid, int right)
        {
            int i = left, j = mid + 1, k = left;

            while (i <= mid && j <= right)
            {
                temp[k++] = (arr[i].Value <= arr[j].Value) ? arr[i++] : arr[j++];
            }

            while (i <= mid) temp[k++] = arr[i++];
            while (j <= right) temp[k++] = arr[j++];

            for (int idx = left; idx <= right; idx++)
                arr[idx] = temp[idx];
        }
        #endregion
        #region Quick Insertion Sort
        public static void QuickInsertionSort(KeyValuePair<KeyValuePair<int, int>, double>[] arr)
        {
            const int INSERTION_SORT_THRESHOLD = 32;
            QuickInsertionSort(arr, 0, arr.Length - 1, INSERTION_SORT_THRESHOLD);
        }

        private static void QuickInsertionSort(KeyValuePair<KeyValuePair<int, int>, double>[] arr, int low, int high, int threshold)
        {
            while (low < high)
            {
                if (high - low + 1 <= threshold)
                {
                    InsertionSort(arr, low, high);
                    break;
                }

                int pivotIndex = Partition(arr, low, high);

                if (pivotIndex - low < high - pivotIndex)
                {
                    QuickInsertionSort(arr, low, pivotIndex - 1, threshold);
                    low = pivotIndex + 1;
                }
                else
                {
                    QuickInsertionSort(arr, pivotIndex + 1, high, threshold);
                    high = pivotIndex - 1;
                }
            }
        }

        private static int Partition(KeyValuePair<KeyValuePair<int, int>, double>[] arr, int low, int high)
        {
            var pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (arr[j].Value <= pivot.Value)
                {
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                }
            }

            (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
            return i + 1;
        }

        //private static void InsertionSort(KeyValuePair<KeyValuePair<int, int>, double>[] arr, int low, int high)
        //{
        //    for (int i = low + 1; i <= high; i++)
        //    {
        //        var key = arr[i];
        //        int j = i - 1;

        //        while (j >= low && arr[j].Value > key.Value)
        //        {
        //            arr[j + 1] = arr[j];
        //            j--;
        //        }

        //        arr[j + 1] = key;
        //    }
        //}
        #endregion

        public static Vertix[,] SegmentationLogic(Vertix[,] graph, KeyValuePair<KeyValuePair<int, int>, double>[] edges)
        {
            Stopwatch timer = Stopwatch.StartNew();
            int height = graph.GetLength(0);
            int width = graph.GetLength(1);
            //edges = SortEdges2(edges);
            //MergeInsertionSort(edges);
            //FasterSort.FasterSort.Sort(edges, Comparer<Edge>.Create((a, b) => a.Weight.CompareTo(b.Weight)));
            timer.Stop();
            data.time2=timer.ElapsedMilliseconds;
            foreach (var item in edges)
            {
                double diff = item.Value;
                
                Vertix v1 = graph[(item.Key.Key/ width), ((item.Key.Key)- ((item.Key.Key / width)*width))];
                Vertix v2 = graph[(item.Key.Value / width), ((item.Key.Value) - ((item.Key.Value / width)*width))];

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

