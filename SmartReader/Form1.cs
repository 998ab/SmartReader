using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SmartReader
{
    public partial class Form1 : Form
    {
        private Bitmap image;
        private Bitmap smallImage;
        private Bitmap graph;
        private Bitmap difference;
        private int f = 1;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog(); //создание диалогового окна для выбора файла
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG;*.JEPG)|*.BMP;*.JPG;*.GIF;*.PNG;*.JEPG|All files (*.*)|*.*"; //формат загружаемого файла
            if (open_dialog.ShowDialog() == DialogResult.OK) //если в окне была нажата кнопка "ОК"
            {
                try
                {
                    image = new Bitmap(open_dialog.FileName);
                    this.pictureBox1.Size = image.Size;
                    pictureBox1.Image = image;
                    pictureBox1.Invalidate();
                    //smallImage.SetResolution(image.Size.Width / 8, image.Size.Height/8);
                    smallImage = image;
                    difference = image;
                    
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки файла.");
                }
            }
        }
        Filters filters = new Filters();
        private void button2_Click(object sender, EventArgs e)
        {
            smallImage = filters.Compress(image);                                                         // Запускаем фильтр сжатия разрешения
            pictureBox2.Image = smallImage;                                                               
            pictureBox2.Invalidate();
            label1.Text = Convert.ToString(smallImage.Width) + " " + Convert.ToString(smallImage.Height); //Выводим разрешение
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // MessageBox.Show(Convert.ToString(filters.Eqalizer(smallImage))); //filters.Eqalizer(smallImage);
            Filters fl = new Filters();
            difference = smallImage;
            smallImage = fl.FindPopColors(smallImage,1);
            pictureBox2.Image = smallImage;
            pictureBox2.Invalidate();
        }
        public int i = 10;
        private void button4_Click(object sender, EventArgs e)
        {

            //i = 5;
            //label1.Text =  Convert.ToString(filters.FindPopColors(smallImage,i));
           // label1.Text = 
            //filters.FindPopColors(smallImage, i);
            //pictureBox2.Image = smallImage;
            //pictureBox2.Invalidate();

        }

        private void button5_Click(object sender, EventArgs e)
        {
           // difference = smallImage;
            smallImage = filters.BW(smallImage);
            pictureBox2.Image = smallImage;
            pictureBox2.Invalidate();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //difference = smallImage;
            smallImage = filters.DivTwo(smallImage);
            pictureBox2.Image = smallImage;
            pictureBox2.Invalidate();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            graph = new Bitmap(40, smallImage.Height);
            graph = filters.AddGraph(smallImage,graph);
            pictureBox3.Image = graph;
            pictureBox3.Invalidate();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            difference = smallImage;
            smallImage = filters.FindPopColorsSmart(smallImage);
            pictureBox2.Image = smallImage;
            pictureBox2.Invalidate();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            // f *= -1;
            //if (f>0)
            //{
            //  pictureBox2.Image = smallImage;
            //  pictureBox2.Invalidate();
            //}
            //else
            //{
            //if (image == difference) MessageBox.Show("LLLOOOLLL");
            //pictureBox2.Image = null;
            //pictureBox1.Invalidate();
            //    pictureBox2.Image = difference;
            //    pictureBox2.Invalidate();
                
            //}
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
