using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace ImageTemplate.Classes
{
    public static class RGBColor
    {
        public static (Vertix[,], Vertix[,], Vertix[,]) construncGraph(RGBPixel[,] image)
        {
            int n = image.GetLength(0);
            int m = image.GetLength(1);
            Vertix[,] verticesR= new Vertix[n, m];
            Vertix[,] verticesG= new Vertix[n, m];
            Vertix[,] verticesB = new Vertix[n, m];
            data.edgesR = new List<int>[256];
            data.edgesG = new List<int>[256];
            data.edgesB = new List<int>[256];
            int[] dx = { 0, 1, 1, 1 };
            int[] dy = { 1, 0, -1, 1 };
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (verticesR[i, j] == null)
                    {
                        verticesR[i, j] = new Vertix { x = i, y = j };
                        verticesR[i, j].Parent = verticesR[i, j];
                        verticesG[i, j] = new Vertix { x = i, y = j };
                        verticesG[i, j].Parent = verticesG[i, j];
                        verticesB[i, j] = new Vertix { x = i, y = j };
                        verticesB[i, j].Parent = verticesB[i, j];
                    }
                    for (int s = 0; s < dx.Length; s++)
                    {
                        int x = i + dx[s];
                        int y = j + dy[s];
                        if (x >= 0 && x < n && y >= 0 && y < m)
                        {
                            if (verticesR[x, y] == null)
                            {
                                verticesR[x, y] = new Vertix { x = x, y = y };
                                verticesR[x, y].Parent = verticesR[x, y];
                                verticesG[x, y] = new Vertix { x = x, y = y };
                                verticesG[x, y].Parent = verticesG[x, y];
                                verticesB[x, y] = new Vertix { x = x, y = y };
                                verticesB[x, y].Parent = verticesB[x, y];
                            }
                            int diff1 = Math.Abs(image[i, j].red -image[x, y].red);
                            int diff2 = Math.Abs(image[i, j].green - image[x, y].green);
                            int diff3 = Math.Abs(image[i, j].blue - image[x, y].blue);
                            if (data.edgesR[diff1] == null)
                                data.edgesR[diff1] = new List<int>();
                            if (data.edgesG[diff2] == null)
                                data.edgesG[diff2] = new List<int>();
                            if (data.edgesB[diff3] == null)
                                data.edgesB[diff3] = new List<int>();
                            data.edgesR[diff1].Add((j + (i * m)));
                            data.edgesR[diff1].Add((y + (x * m)));
                            data.edgesG[diff2].Add((j + (i * m)));
                            data.edgesG[diff2].Add((y + (x * m)));
                            data.edgesB[diff3].Add((j + (i * m)));
                            data.edgesB[diff3].Add((y + (x * m)));
                        }
                    }
                }
            }
            return (verticesR, verticesG, verticesB);
        }
    }
}
