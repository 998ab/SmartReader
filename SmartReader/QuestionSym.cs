using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartReader
{
    public partial class QuestionSym : Form
    {
        public string s = "aab";
        public QuestionSym()
        {
            InitializeComponent();
        }

        public void OpenShow(Form2 form, string name)
        {
            this.Show();
            s = name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            s = textBox1.Text;
            this.Close();
        }
    }
}
