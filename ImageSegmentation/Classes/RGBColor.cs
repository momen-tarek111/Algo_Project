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
        public (Vertix[,], Vertix[,], Vertix[,]) construncGraph(RGBPixel[,] image)
        {
            int n = image.GetLength(0);
            int m = image.GetLength(1);
            Vertix[,] verticesR = new Vertix[n, m];
            Vertix[,] verticesG = new Vertix[n, m];
            Vertix[,] verticesB = new Vertix[n, m];
            bool [,]visited=new bool [n,m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    verticesR[i, j] = new Vertix { x = i, y = j };
                    verticesG[i, j] = new Vertix { x = i, y = j };
                    verticesB[i, j] = new Vertix { x = i, y = j };
                    verticesR[i, j].Parent = verticesR[i, j];
                    verticesG[i, j].Parent = verticesG[i, j];
                    verticesB[i, j].Parent = verticesB[i, j];
                }
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {

                    for (int s = 0; s < 8; s++)
                    {
                        int x = i + data.dx[s];
                        int y = j + data.dy[s];
                        if (!data.isValid(n, m, x, y))
                            continue;
                        if (visited[x, y])
                        {
                            continue;
                        }
                        
                        
                        // Add Red edge
                        Edge edgeR = new Edge
                        {
                            fromVertix = verticesR[i, j],
                            toVertix = verticesR[x, y],
                            Weight = Math.Abs(image[i, j].red - image[x, y].red)
                        };
                        data.edgesR.Add(edgeR);

                        // Add Green edge
                        Edge edgeG = new Edge
                        {
                            fromVertix = verticesG[i, j],
                            toVertix = verticesG[x, y],
                            Weight = Math.Abs(image[i, j].green - image[x, y].green)
                        };
                        data.edgesG.Add(edgeG);

                        // Add Blue edge
                        Edge edgeB = new Edge
                        {
                            fromVertix = verticesB[i, j],
                            toVertix = verticesB[x, y],
                            Weight = Math.Abs(image[i, j].blue - image[x, y].blue)
                        };
                        data.edgesB.Add(edgeB);
                    }
                    visited[i,j] = true;
                }
            }
            return (verticesR, verticesG, verticesB);
        }

    }
}
