﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartReader
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения. Proverka sdfsdfsdf
        /// </summary>
        [STAThread]
        static void Main()
      {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
