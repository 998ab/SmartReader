using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartReader
{
    public partial class Form2 : Form
    {
        private bool f = false;
        private Bitmap imgForm ;
        private Bitmap smallImg;
        private int x = 0, y = 0;

        //Путь до файла с образами
        string path = Application.StartupPath + "\\file.json";

        //Dictionary<string, string> obr = new Dictionary<string, string>();
        List<obraz> obr = new List<obraz>();


        public Form2()
        {
            InitializeComponent();
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            x = e.X;
            y = e.Y;
            if (f && x > 0 && y > 0 && y<pictureBox1.Height && x<pictureBox1.Width)
            {
                imgForm.SetPixel(x, y, Color.FromArgb(255,0,0,0));
                pictureBox1.Image = imgForm;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            f = false;
        }
        

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            f = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            smallImg = compress(imgForm);
            pictureBox2.Image = smallImg;
            pictureBox2.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //obr.Add(textBox1.Text, getMassive(smallImg));
            //StreamWriter wr = new StreamWriter(path);
            //foreach (var item in obr)
            //{
            //    wr.WriteLine(item.Key + "|" + item.Value);
            //}
            //wr.Close();

            if (textBox1.Text == "")
                MessageBox.Show("Ведите имя образа");
            else
            {
                //Если образ с таким именем уже есть
                if (getExistByName(obr, textBox1.Text))
                {
                    //obr[getIdByName(textBox1.Text)] <- это образ который нужно сравнить
                    //obr[getIdByName(textBox1.Text)].map <- карта образа  |   .count <- кол-во образов
                }
                else
                {
                    //Добавляем новый образ в лист
                    obr.Add(new obraz(1, textBox1.Text, getMassiveNew(smallImg)));
                    //Запись списка в файл
                    listWrite();
                }
            }




        }

        string getMassive(Bitmap img)
        {
            string _out = "",s;
            int c = 0;
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    if (img.GetPixel(j, i) == Color.FromArgb(255, 0, 0, 0)) s = "10,";
                    else { s = "0,"; }
                    _out += s;c++;
                }
            }
            MessageBox.Show(img.Height + "  " + img.Width);
            return _out.Substring(0, _out.Length - 1);
        }


        //Преобразование сжатой картинки в массив
        int[] getMassiveNew(Bitmap img)
        {
            int[] _out = new int[100];
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    if (img.GetPixel(j, i) == Color.FromArgb(255, 0, 0, 0)) _out[i * 10 + j] = 10;
                    else { _out[i * 10 + j] = 0; }
                }
            }
            return _out;
        }


        //Проверка есть ли в списке образ с таким же именем
        bool getExistByName(List<obraz> list, string name)
        {
            bool _out = false;
            foreach (var item in list)
            {
                if (item.name == name) _out = true;
            }
            return _out;
        }

        //Запись списка образов в файл
        void listWrite()
        {
            StreamWriter wr = new StreamWriter(path);
            wr.Write(JsonConvert.SerializeObject(obr));
            wr.Close();
        }

        //Полуение Id образа в списке по его имени
        int getIdByName(string name)
        {
            int _out = 0;
            for (int i = 0; i < obr.Count; i++)
                if (obr[i].name == name) { _out = i; break; }
            return _out;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

            imgForm = new Bitmap(200, 200);

            if (File.Exists(path))
            {
                //Заполнение списка данными из файла
                obr = JsonConvert.DeserializeObject<List<obraz>>(File.ReadAllText(path));
                
                //Добавление элементов в listView
                foreach (var item in obr)
                {
                    listView1.Items.Add(item.name);
                }

            }
            else { File.Create(path); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //int[] t = toInt(obr["a"]);
            //MessageBox.Show(t.Count() + "");
            //int max = 0;
            //string name = "";
            //foreach (var item in obr)
            //{
            //    int t = compare(toInt(item.Value));
            //    if (t > max) { max = t; name = item.Key; }
            //}
            //MessageBox.Show(name);

            int max = 0; string name = "";
            for (int i = 0; i < obr.Count; i++)
            {
                int per = compare(obr[i].map);
                if (per > max) { max = per; name = obr[i].name; }
            }
            MessageBox.Show(name);


        }

        int[] toInt(string _in)
        {
            int[] _out = new int[100];
            for (int i = 0; i < 100; i++)
            {
                _out[i] = Convert.ToInt32(_in.Split(',')[i]);
            }
            return _out;

        }

        private Bitmap compress(Bitmap img)
        {
            bool blackPixel = false;
            int n = 20;
            Bitmap smallImg = new Bitmap(img.Width / n, img.Height / n);
            
            Color clr = new Color();

            for (int y = 1; y < (img.Height / n); y++)
            {
                for (int x = 1; x < (img.Width / n); x++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (img.GetPixel(x * n - i, y * n - j) == Color.FromArgb(255,0,0,0)) { blackPixel = true; }
                        }

                    }
                    
                    // smallImg.SetPixel(x, y, clr);
                    if (blackPixel)
                        smallImg.SetPixel(x, y, Color.Black);
                    
                    blackPixel = false;
                }
            }
            return smallImg;
        }

        int compare(int[] t)
        {
            int[,] mass = new int[10, 10];
            string s = "";
            for (int y = 0; y < smallImg.Height; y++)
            {
                for (int x = 0; x < smallImg.Width; x++)
                {
                    mass[x, y] = t[x + y * 10];
                    s += mass[x, y] + " ";
                }
                s += "\n";
            }
            int pohoshe = 0;
            for (int i = 0; i < t.Count(); i++)
            {
                textBox1.Text += t[i].ToString();
            }
            smallImg = compress(imgForm);
            for (int y = 0; y < smallImg.Height; y++)
            {
                for (int x = 0; x < smallImg.Width; x++)
                {
                    if (smallImg.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0) && mass[x, y] > 0)
                    {
                        pohoshe++;
                    }
                    if (smallImg.GetPixel(x, y) == Color.FromArgb(0, 0, 0, 0) && mass[x, y] == 0)
                    {
                        pohoshe++;
                    }
                }
            }
            return pohoshe;
        }


    }
}
