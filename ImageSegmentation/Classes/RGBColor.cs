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
            
            Vertix[,] verticesR = new Vertix[image.GetLength(0), image.GetLength(1)];
            Vertix[,] verticesG = new Vertix[image.GetLength(0), image.GetLength(1)];
            Vertix[,] verticesB = new Vertix[image.GetLength(0), image.GetLength(1)];
            int n= image.GetLength(0);
            int m= image.GetLength(1);
            for (int i = 0; i < n; i++) {
                for (int j = 0; j < m; j++)
                {
                    Vertix vR =new Vertix();
                    Vertix vG = new Vertix();
                    Vertix vB = new Vertix();
                    vR.x = i;
                    vR.y = j;
                    vG.x = i;
                    vG.y = j;
                    vB.x = i;
                    vB.y = j;
                    for (int s = 0; s < 8; s++)
                    {
                        
                        Edge edgeR = new Edge();
                        Edge edgeG = new Edge();
                        Edge edgeB = new Edge();
                        Vertix newVertix=new Vertix();
                        int x=i+data.dx[s];
                        int y=i+data.dy[s];
                        if (!(data.isValid(n, m, x, y)))
                        {
                            continue;
                        }
                        newVertix.x = x;
                        newVertix.y = y;
                        edgeB.Vertix = newVertix;
                        edgeB.Weight = Math.Abs(image[i, j].blue - image[x,y].blue);
                        vB.Edges.Add(edgeB);
                        edgeR.Vertix = newVertix;
                        edgeR.Weight = Math.Abs(image[i, j].red - image[x, y].red);
                        vR.Edges.Add(edgeR);
                        edgeG.Vertix = newVertix;
                        edgeG.Weight = Math.Abs(image[i, j].green - image[x, y].green);
                        vG.Edges.Add(edgeG);
                    }
                    verticesR[i,j] = vR;
                    verticesG[i, j] = vG;
                    verticesB[i, j] = vB;
                }
            }
            return (verticesR,verticesG,verticesB);
        }
    }
}
