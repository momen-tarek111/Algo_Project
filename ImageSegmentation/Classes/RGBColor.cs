using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ImageTemplate.Classes
{
    public class RGBColor : IColor
    {
        //public (Vertix[,], Vertix[,], Vertix[,]) construncGraph(RGBPixel[,] image)
        //{
        //    int n = image.GetLength(0);
        //    int m = image.GetLength(1);
        //    Vertix[,] verticesR = new Vertix[n, m];
        //    Vertix[,] verticesG = new Vertix[n, m];
        //    Vertix[,] verticesB = new Vertix[n, m];
        //    bool [,]visited=new bool [n,m];
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < m; j++)
        //        {
        //            verticesR[i, j] = new Vertix { x = i, y = j };
        //            verticesG[i, j] = new Vertix { x = i, y = j };
        //            verticesB[i, j] = new Vertix { x = i, y = j };
        //            verticesR[i, j].Parent = verticesR[i, j];
        //            verticesG[i, j].Parent = verticesG[i, j];
        //            verticesB[i, j].Parent = verticesB[i, j];
        //        }
        //    }
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < m; j++)
        //        {

        //            for (int s = 0; s < 8; s++)
        //            {
        //                int x = i + data.dx[s];
        //                int y = j + data.dy[s];
        //                if (!data.isValid(n, m, x, y))
        //                    continue;
        //                if (visited[x, y])
        //                {
        //                    continue;
        //                }


        //                // Add Red edge
        //                Edge edgeR = new Edge
        //                {
        //                    fromVertix = verticesR[i, j],
        //                    toVertix = verticesR[x, y],
        //                    Weight = Math.Abs(image[i, j].red - image[x, y].red)
        //                };
        //                data.edgesR.Add(edgeR);

        //                // Add Green edge
        //                Edge edgeG = new Edge
        //                {
        //                    fromVertix = verticesG[i, j],
        //                    toVertix = verticesG[x, y],
        //                    Weight = Math.Abs(image[i, j].green - image[x, y].green)
        //                };
        //                data.edgesG.Add(edgeG);

        //                // Add Blue edge
        //                Edge edgeB = new Edge
        //                {
        //                    fromVertix = verticesB[i, j],
        //                    toVertix = verticesB[x, y],
        //                    Weight = Math.Abs(image[i, j].blue - image[x, y].blue)
        //                };
        //                data.edgesB.Add(edgeB);
        //            }
        //            visited[i,j] = true;
        //        }
        //    }
        //    return (verticesR, verticesG, verticesB);
        //}


        //public (Vertix[,], Vertix[,], Vertix[,]) construncGraph(RGBPixel[,] image)
        //{
        //    int n = image.GetLength(0);
        //    int m = image.GetLength(1);

        //    // Initialize vertices
        //    var verticesR = InitializeVertices(n, m);
        //    var verticesG = InitializeVertices(n, m);
        //    var verticesB = InitializeVertices(n, m);

        //    // Pre-allocate edge lists
        //    int estimatedEdges = n * m * 4;
        //    data.edgesR = new List<Edge>(estimatedEdges);
        //    data.edgesG = new List<Edge>(estimatedEdges);
        //    data.edgesB = new List<Edge>(estimatedEdges);

        //    // Directions to check (right, down, down-left, down-right)
        //    int[] dx = { 0, 1, 1, 1 };
        //    int[] dy = { 1, 0, -1, 1 };

        //    // Process in parallel
        //    Parallel.Invoke(
        //        () => ProcessEdges(image, verticesR, data.edgesR, dx, dy, (p) => p.red),
        //        () => ProcessEdges(image, verticesG, data.edgesG, dx, dy, (p) => p.green),
        //        () => ProcessEdges(image, verticesB, data.edgesB, dx, dy, (p) => p.blue)
        //    );

        //    return (verticesR, verticesG, verticesB);
        //}
        //private Vertix[,] InitializeVertices(int n, int m)
        //{
        //    var vertices = new Vertix[n, m];
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < m; j++)
        //        {
        //            vertices[i, j] = new Vertix { x = i, y = j };
        //            vertices[i, j].Parent = vertices[i, j];
        //        }
        //    }
        //    return vertices;
        //}

        //private void ProcessEdges(RGBPixel[,] image, Vertix[,] vertices, List<Edge> edges,int[] dx, int[] dy, Func<RGBPixel, int> colorSelector)
        //{
        //    int n = image.GetLength(0);
        //    int m = image.GetLength(1);



        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < m; j++)
        //        {
        //            for (int s = 0; s < dx.Length; s++)
        //            {
        //                int x = i + dx[s];
        //                int y = j + dy[s];
        //                if (x >= 0 && x < n && y >= 0 && y < m)
        //                {
        //                    edges.Add(new Edge
        //                    {
        //                        fromVertix = vertices[i, j],
        //                        toVertix = vertices[x, y],
        //                        Weight = Math.Abs(colorSelector(image[i, j]) - colorSelector(image[x, y]))
        //                    });

        //                }
        //            }
        //        }
        //    }
        //}
        public (Vertix[,], Vertix[,], Vertix[,]) construncGraph(RGBPixel[,] image)
        {
            int n = image.GetLength(0);
            int m = image.GetLength(1);
            Vertix[,] verticesR= new Vertix[n, m];
            Vertix[,] verticesG= new Vertix[n, m];
            Vertix[,] verticesB = new Vertix[n, m];

            data.edgesR = new List<KeyValuePair<int, int>>[256];
            data.edgesG = new List<KeyValuePair<int, int>>[256];
            data.edgesB = new List<KeyValuePair<int, int>>[256];

            int[] dx = { 0, 1, 1, 1 };
            int[] dy = { 1, 0, -1, 1 };
            ProcessEdges(image, verticesR, data.edgesR, dx, dy, (p) => p.red);
            ProcessEdges(image, verticesG, data.edgesG, dx, dy, (p) => p.green);
            ProcessEdges(image, verticesB, data.edgesB, dx, dy, (p) => p.blue);
            //Parallel.Invoke(
            //    () => ProcessEdges(image, verticesR, data.edgesR, dx, dy, (p) => p.red),
            //    () => ProcessEdges(image, verticesG, data.edgesG, dx, dy, (p) => p.green),
            //    () => ProcessEdges(image, verticesB, data.edgesB, dx, dy, (p) => p.blue)
            //);
            return (verticesR, verticesG, verticesB);
        }
        private void ProcessEdges(RGBPixel[,] image, Vertix[,] vertices, List<KeyValuePair<int, int>>[] edges, int[] dx, int[] dy, Func<RGBPixel, int> colorSelector)
        {
            int n = image.GetLength(0);
            int m = image.GetLength(1);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (vertices[i,j]== null)
                    {
                        vertices[i,j]=new Vertix { x = i, y = j };
                        vertices[i, j].Parent = vertices[i, j];
                    }
                    for (int s = 0; s < dx.Length; s++)
                    {
                        int x = i + dx[s];
                        int y = j + dy[s];
                        if (x >= 0 && x < n && y >= 0 && y < m)
                        {
                            if (vertices[x, y] == null)
                            {
                                vertices[x, y] = new Vertix { x = x, y = y };
                                vertices[x, y].Parent = vertices[x, y];
                            }
                            int diff = Math.Abs(colorSelector(image[i, j]) - colorSelector(image[x, y]));
                            if (edges[diff] == null)
                                edges[diff] = new List<KeyValuePair<int, int>>();
                            edges[diff].Add(new KeyValuePair<int, int>((j + (i * m)), (y + (x * m))));
                        }
                    }
                }
            }
        }
    }
}
