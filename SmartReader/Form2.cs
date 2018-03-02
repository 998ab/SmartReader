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

        QuestionSym qs = new QuestionSym();
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
            if (textBox1.Text == "")
                MessageBox.Show("Ведите имя образа");
            else
            {
                //Если образ с таким именем уже есть
                if (getExistByName(obr, textBox1.Text))
                {
                    int id = getIdByName(textBox1.Text);
                    //obr[id] <- это образ который нужно сравнить
                    //obr[id].map <- карта образа  |   .count <- кол-во образов


                    //твой код...

                    //Должно быть в конце
                    addCountById(id); //Добавит +1 к числу образов
                    listWrite(); // Перезапишет лист в файл

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


        #region Методы для работы со списком
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

        void addCountById(int id)
        {
            obr[id].count = obr[id].count++;
        } 
        #endregion


        private void Form2_Load(object sender, EventArgs e)
        {

            imgForm = new Bitmap(200, 200);

            if (File.Exists(path))
            {
                //Заполнение списка данными из файла
                obr = JsonConvert.DeserializeObject<List<obraz>>(File.ReadAllText(path));
                
                //Добавление элементов в listBox
                foreach (var item in obr)
                {
                    listBox1.Items.Add(item.name);
                }

            }
            else { File.Create(path); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //сранение
            int max = 0; string name = "";
            for (int i = 0; i < obr.Count; i++)
            {
                int per = compare(obr[i].map);//,obr[i].count);
                if (per > max) { max = per; name = obr[i].name; }
            }

            // MessageBox.Show(name + " " + max);

            if (MessageBox.Show("Это символ \"" +name + "\" ?", "Сравнение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // user clicked yes
            }
            else
            {
                //Пользователи нажал нет

                //qs.OpenShow(this, key);
                qs.FormClosed += Qs_FormClosed;
                qs.Show();
            }
        }

        private void Qs_FormClosed(object sender, FormClosedEventArgs e)
        {
           string key = qs.s;
            int[] map = new int[100];
            int count;
            // MessageBox.Show(qs.s);
            foreach (var item in obr)
            {

                if (item.name == key)
                {
                    map = item.map;
                    count = item.count;
                    int imgcount;
                    for (int i = 0; i < smallImg.Height; i++)
                    {
                        for (int j = 0; j < smallImg.Width; j++)
                        {
                            if (smallImg.GetPixel(j, i) == Color.FromArgb(255,0,0,0)) imgcount = 10; else imgcount = 0;
                            map[i * 10 + j] = (map[i * 10 + j] + imgcount)/2;
                        }
                    }
                    obr.Remove(item);
                    obr.Add(new obraz(count + 1, key, map));
                    listWrite();
                    break;
                    //obr.Add(new obraz(count + 1, key, map));
                }
            }
            //obr.Remove();
            

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

            for (int y = 1; y < (img.Height / n + 1); y++)
            {
                for (int x = 1; x < (img.Width / n + 1); x++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        for (int j = 0; j < n; j++)
                        {
                            if (img.GetPixel((x * n) - i - 1, (y * n) - j - 1) == Color.FromArgb(255, 0, 0, 0)) { blackPixel = true; }
                        }

                    }

                    if (blackPixel) smallImg.SetPixel(x - 1, y - 1, Color.Black);

                    blackPixel = false;
                }
            }

            return smallImg;
        }

        

        private void listBox1_Click(object sender, EventArgs e)
        {
            int[,] mass = new int[10, 10];
            int[] t = obr[getIdByName(listBox1.Items[listBox1.SelectedIndex].ToString())].map;
            string s = "";
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    mass[x, y] = t[x + y * 10];
                    s += mass[x, y];
                    if (mass[x, y] > 9) s += " "; else s += "   ";
                }
                s += "\n";
            }
            label1.Text = s;
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

                    //if (smallImg.GetPixel(x, y) == Color.FromArgb(255, 0, 0, 0) && mass[x, y] > 0)
                    //{
                    //    pohoshe+=mass[x,y];
                    //}
                    //if (smallImg.GetPixel(x, y) == Color.FromArgb(0, 0, 0, 0) && mass[x, y] == 0)
                    //{
                    //    pohoshe+=0;
                    //}
                    if(smallImg.GetPixel(x,y) == Color.FromArgb(255, 0, 0, 0))
                    {
                        pohoshe += (mass[x, y] + mass[x,y])/2;
                    }
                    else
                    {
                        pohoshe += mass[x, y] * 10;
                    }
                }
            }
            return pohoshe;
        }


    }
}
