using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ColorMine;
using ColorMine.ColorSpaces;
namespace SmartReader
{
    class Filters
    {
        public Bitmap Compress(Bitmap img)
        {
            int a=0, r=0, g=0, b=0, n=8;
            Bitmap smallImg = new Bitmap(img.Width/n, img.Height/n);
            Color clr = new Color();
            a = 0; r = 0; g = 0; b = 0;

            for (int y = 1; y < (img.Height/n +1); y++)
            {  
                for (int x = 1; x < (img.Width/n +1); x++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            clr = img.GetPixel(x*n-i, y*n-j); // Средний цвет из 16 (4х4 квадрат)
                            a += clr.A;
                            r += clr.R;
                            g += clr.G;
                            b += clr.B;
                        }
                        
                    }
                    a /= n * n;
                    r /= n * n;
                    g /= n * n;
                    b /= n * n;
                    clr = Color.FromArgb(a, r, g, b);  
                    smallImg.SetPixel(x -1, y -1, clr);
                    a = 0; r = 0; g = 0; b = 0;
                }
            }
            return smallImg;
        }

        public int Eqalizer(Bitmap img)
        {
            bool[,] matrix = new bool[img.Width, img.Height];
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    matrix[j, i] = true;
                }
            }
            int col = 0;
            int max = 0;
            Color max1 = new Color();
            Color clr = new Color();
            for (int y = 1; y < img.Height; y ++)
            {
                for (int x = 1; x < img.Width; x ++)
                {
                    clr = img.GetPixel(x, y);
                    if (matrix[x, y] == true)
                    {
                        for (int y1 = 0; y1 < img.Height; y1++)
                        {
                            for (int x1 = 0; x1 < img.Width; x1++)
                            {
                                if (matrix[x1, y1] == true)
                                {
                                    if (img.GetPixel(x1, y1).Equals(clr)) { col++; matrix[x1, y1] = false; }
                                }
                            }
                        }
                    }
                    if (col > max) { max = col; max1 = clr; }
                    col = 0;
                }

            }
            return col;
        }

        public Bitmap FindPopColors(Bitmap img,int i)
        {
            double index = 0;
            do
            {
                byte[,] color = new byte[img.Width,img.Height];
                int[,] col = new int[257,4];
                string s = "";
                Color clr = new Color();
                bool f = true;
                int n = 0;
                int[] max1 = new int[4];
                int[] max2 = new int[4];
                int[] max3 = new int[4];
                int a=0, r=0, g=0, b=0;
                for (int y = 1; y < img.Height; y++)
                {
                    for (int x = 1; x < img.Width - 1; x++)
                    {
                        color[x,y] = img.GetPixel(x, y).A;
                    }

                }
                for (int y = 1; y < img.Height; y++)
                {
                    for (int x = 1; x < img.Width - 1; x++)
                    {
                        col[color[x,y],1]++;
                        col[color[x, y], 2] = x;
                        col[color[x, y], 3] = y;
                    }

                }
                for (int x = 0; x < 256; x++)
                {
                    if (col[x,1] > max1[1]) { max2 = max1; max1[1] = col[x,1]; max1[2] = col[x,2]; max1[3] = col[x, 3]; }
                    else
                        if (col[x,1] > max2[1]) { max3 = max2; max2[1] = col[x,1];max2[2] = col[x, 2];max2[3] = col[x, 3]; }
                    else
                        if (col[x,1] > max3[1]) { max3[1] = col[x,1]; max3[2] = col[x, 2]; max3[3] = col[x, 3]; }
                }
                //index = (color[max2[2], max2[3]] - color[max3[2], max3[3]]) / ((color[max2[2], max2[3]] + color[max3[2], max3[3]]) / 2);
                if (color[max2[2], max2[3]]> color[max3[2], max3[3]])
                    {
                        index = color[max2[2], max2[3]] - color[max3[2], max3[3]];
                        f = true;
                    }
                    else
                    {
                        index = color[max3[2], max3[3]] - color[max2[2], max2[3]];
                        f = false;
                    }
                  if (index <100)
                    {
                        for (int y = 1; y < img.Height; y++)
                        {
                            for (int x = 1; x < img.Width - 1; x++)
                            {
                                if (f == false)
                                {
                                    if (img.GetPixel(x, y).A == color[max2[2], max2[3]])
                                    {
                                        clr = Color.FromArgb(color[max3[2], max3[3]], 0, 0, 0);
                                        img.SetPixel(x, y, clr);
                                    }
                                }
                                else
                                {
                                    if (img.GetPixel(x, y).A == color[max3[2], max3[3]])
                                    {
                                        clr = Color.FromArgb(color[max2[2], max2[3]], 0, 0, 0);
                                        img.SetPixel(x, y, clr);
                                    }
                                }
                            
                            }
                        }
                    }
            } while (index <100);
           // s = Convert.ToString(max1[1]) + " " + Convert.ToString(max2[1]) + " " + Convert.ToString(max3[1]);
            //MessageBox.Show(s);
            return img;
        }

        public Bitmap BW(Bitmap img)
        {
            int a = 0;
            Color clr = new Color();
            for (int y = 1; y < img.Height; y++)
            {
                for (int x = 1; x < img.Width; x++)
                {
                    clr = img.GetPixel(x, y);
                    a = (clr.R + clr.G + clr.B) / 3;
                    clr = Color.FromArgb(a, 0, 0, 0);
                    img.SetPixel(x, y, clr);
                }

            }
            return img;
        }

        public Bitmap DivTwo(Bitmap img)
        {
            Color clr = new Color();
            for (int y = 1; y < img.Height; y++)
            {
                for (int x = 1; x < img.Width; x++)
                {
                    if (img.GetPixel(x, y).A > 100) { clr = Color.FromArgb(255, 0, 0, 0); img.SetPixel(x, y, clr); }
                    else { clr = Color.FromArgb(0, 0, 0, 0); img.SetPixel(x, y, clr); }
                }

            }
            return img;
        }

        public Bitmap AddGraph(Bitmap img, Bitmap graph)
        {
            bool flagUp = true;
            bool flagDown = false;
            int n = 0;
            double[,] mass = new double[img.Height,2];
            double sred = 0;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    if (img.GetPixel(x, y).A == 0)
                    {
                        n++;
                        mass[y,0]++;
                    }

                }
                n = n * 10;
                if (n > img.Width) n = img.Width;
                n = (n / img.Width) * 40;
                n = 0;
            }
            for (int p = 0; p < 6; p++)
            {
                for (int i = 0; i < img.Height - 1; i ++)
                {
                    sred = (mass[i,0] + mass[i + 1,0]) / 2;
                    mass[i,0] = sred;
                    mass[i + 1,0] = sred;
                }
            }
            Color clr = Color.Yellow;
            for (int i = 0; i < img.Height - 1; i++)
            {
                Console.WriteLine(mass[i,0]);

                if (mass[i,0] > 39) mass[i,0] = 39;
                if (mass[i,0] > 0) { if (mass[i,0] < mass[i + 1,0]) { if (mass[i + 1,0] / mass[i,0] > 1.1) {clr = Color.Red; mass[i, 1] = 1; } else { clr = Color.Yellow; mass[i, 1] = 3; } } }
                if (mass[i,0] > 0) { if (mass[i,0] > mass[i + 1,0]) { if (mass[i,0] / mass[i + 1,0] > 1.1) {clr = Color.Blue; mass[i, 1] = 2; } else { clr = Color.Yellow; mass[i, 1] = 3; } } }
                for (int j = 0; j < mass[i,0]; j++)
                {
                        graph.SetPixel(j, i, clr);
                }
                
            }
            int test;
            for (int i = 0; i < img.Height - 1; i++)
            {
                if (mass[i, 1] == 1)
                {
                    test = i+((img.Height / 2)-1);
                    if (test > img.Height) test = img.Height-1;
                    for (int j = i + 5; j < test; j++)
                    {
                        if(mass[i,1] == 2 && mass[i+1,2] != 2)
                        {

                        }
                    }
                }
            }
            return graph;
        }
        
        public Bitmap FindPopColorsSmart(Bitmap img)
        {
           // byte[,] color = new byte[img.Width, img.Height];
            int[,] color = new int[257, 5];
            int n = 0;
            int p = 35;
            int n1 = 0;
            int index = 0;
            Color clrWhite = Color.FromArgb(0,0,0,0);
            Color clrBlack = Color.FromArgb(255, 0, 0, 0);
            bool f = false;

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    color[img.GetPixel(x, y).A, 1] = img.GetPixel(x, y).A; //Заполняю массив color
                    color[img.GetPixel(x, y).A, 2] ++;
                    color[img.GetPixel(x, y).A, 3] = x;
                    color[img.GetPixel(x, y).A, 4] = y;
                }

            }
            for (int i = 0; i < 255; i++)
            {
                Console.WriteLine(color[i, 1] + " " + color[i,2]);
            }
            for (int i = 0; i < 255; i++)
            {
                if (i + n + 1 < 256)  //проверка на перебор
                {
                    if (color[i + n, 2] > 10)//Если количество цвета[i] больше 0
                    {
                        if (color[i + n + 1, 2] > 10)  //Если количество цвета[i+1] больше 0
                        {
                            index = color[i + n + 1, 1] - color[i + n, 1];
                            if (index < p)
                            {
                                for (int y = 0; y < img.Height; y++)
                                {
                                    for (int x = 0; x < img.Width; x++)
                                    {
                                        //if (img.GetPixel(x, y).A == color[i + n + 1 + n1, 1])
                                        //{
                                        //    img.SetPixel(x, y, clrWhite);
                                        //}
                                        //else
                                        //{
                                            if (img.GetPixel(x, y).A == color[i + n, 1]) { img.SetPixel(x, y, clrWhite); }
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                f = true;
                                break;
                            }
                        }
                        else
                        {
                            do
                            {
                                n1++;
                                if (i + n + n1 + 1 > 255) { f = true; break; }
                            } while (color[i + n + n1 + 1, 2] <10);
                            if (f) break;
                            index = color[i + n + 1 + n1, 1] - color[i + n, 1];
                            if (index < p)
                            {
                                for (int y = 0; y < img.Height; y++)
                                {
                                    for (int x = 0; x < img.Width; x++)
                                    {
                                        //if (img.GetPixel(x, y).A == color[i + n + 1 + n1, 1])
                                        //{
                                        //    img.SetPixel(x, y, clrWhite);
                                        //}
                                        //else
                                        //{
                                            if (img.GetPixel(x, y).A == color[i + n, 1]) { img.SetPixel(x, y, clrWhite); }
                                        //}
                                    }
                                }
                            }
                            else
                            {
                                f = true;
                                break;
                            }
                            n += n1;
                            n1 = 0;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
            n = 0;
            return img;
        }
    }
}
