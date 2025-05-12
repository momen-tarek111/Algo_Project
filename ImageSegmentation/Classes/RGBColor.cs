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

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    Vertix vR = new Vertix { x = i, y = j, Parent = null };
                    Vertix vG = new Vertix { x = i, y = j, Parent = null };
                    Vertix vB = new Vertix { x = i, y = j, Parent = null };

                    vR.Parent = vR;
                    vG.Parent = vG;
                    vB.Parent = vB;

                    verticesR[i, j] = vR;
                    verticesG[i, j] = vG;
                    verticesB[i, j] = vB;

                    for (int s = 0; s < 8; s++)
                    {
                        int x = i + data.dx[s];
                        int y = j + data.dy[s];

                        if (!data.isValid(n, m, x, y))
                            continue;

                        Vertix neighbor = new Vertix { x = x, y = y };

                        Edge edgeR = new Edge
                        {
                            fromVertix = vR,
                            toVertix = neighbor,
                            Weight = Math.Abs(image[i, j].red - image[x, y].red)
                        };
                        data.edgesR.Add(edgeR);
                        Edge edgeG = new Edge
                        {
                            fromVertix = vG,
                            toVertix = neighbor,
                            Weight = Math.Abs(image[i, j].green - image[x, y].green)
                        };
                        data.edgesG.Add(edgeG);
                        Edge edgeB = new Edge
                        {
                            fromVertix = vB,
                            toVertix = neighbor,
                            Weight = Math.Abs(image[i, j].blue - image[x, y].blue)
                        };
                        data.edgesB.Add(edgeB);
                    }
                }
            }

            return (verticesR, verticesG, verticesB);
        }
    }
}
