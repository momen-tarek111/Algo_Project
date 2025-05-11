using ImageTemplate.Classes;
using ImageTemplate.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageTemplate
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;
        List<int> selectedRegions = new List<int>();
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            selectedRegions.Clear();
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            data.OriginImage = ImageMatrix;
            try
            {
                data.K=int.Parse(textBox1.Text);

            }
            catch
            {
                MessageBox.Show("Enter K Value");
                return;
            }
            //ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(MainFlow.First(ImageMatrix), pictureBox2);
        }
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            int label = data.FinalLabels[y, x];
            if (!selectedRegions.Contains(label))
            {
                selectedRegions.Add(label);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Height = data.OriginImage.GetLength(0);
            int width = data.OriginImage.GetLength(1);
            RGBPixel[,] FinailImage =new RGBPixel[Height,width]; 
            for (int i = 0; i < Height; i++)
            {
                for(int j = 0; j <width; j++)
                {
                    if (selectedRegions.Contains(data.FinalLabels[i, j]))
                    {
                        FinailImage[i, j].blue = data.OriginImage[i, j].blue;
                        FinailImage[i, j].green = data.OriginImage[i,j].green;
                        FinailImage[i,j].red=data.OriginImage[i,j].red;
                    }
                    else
                    {
                        FinailImage[i, j].blue = 255;
                        FinailImage[i, j].green = 255;
                        FinailImage[i, j].red = 255;
                    }
                }
            }
            ImageOperations.DisplayImage(FinailImage, pictureBox2);
        }

        private void nudMaskSize_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}