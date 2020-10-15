using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ProtectMethods
{
    public partial class Form1 : Form
    {
        static int NOD (int x, int y)
        {
            while (x != y)
            {
                if (x > y) x -= y;
                else y -= x;
            }
            return x;
        }
        static int getInverse(int a)
        {
            int result = 1;

            for (int i = 1; i <= arr.Length; i++)
            {
                if ((a * i) % (arr.Length) == 1)
                {
                    result = i;
                }
            }

            return result;
        }
        //char[] arr = Enumerable.Range(0, 32).Select((x, i) => (char)('а' + i)).ToArray();
        static char[] arr = //"абвгдеёжзийклмнопрстуфхцчшщьыъэюя".ToCharArray();
            Enumerable.Range('А', 32)
                .Concat(Enumerable.Range('а', 32))
                .Concat(Enumerable.Range(' ', 1))
                .Concat((new int[] { '?', '!', '.', ':', '-', '_', '(', ')', ',' }))
                .Select(x => (char)x)
                .ToArray();
        public Form1()
        {
            InitializeComponent();
            /*for (int i = 0; i < arr.Length; i++)
                textBox4.Text += arr[i] + " - " + i + ";";*/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string newString = "";
            StreamReader f = new StreamReader("text.txt");
            if (radioButton1.Checked)
            {
                bool check = false;
                int a = Convert.ToInt32(textBox2.Text);
                do
                {
                    a = Convert.ToInt32(textBox2.Text);
                    if (NOD(a, arr.Length) != 1) { MessageBox.Show("Некорректный коэффициент"); textBox2.Clear(); }
                    else check = true;
                }
                while (!check);
                int b = Convert.ToInt32(textBox3.Text);
                string text = String.Empty;
                if (!String.IsNullOrEmpty(textBox1.Text))
                    text = textBox1.Text;
                else
                {
                    while (!f.EndOfStream)
                    {
                        text += f.ReadLine();
                    }
                    textBox1.Text = text;
                    f.Close();
                }
                foreach (char ch in text)
                {
                    int ind = Array.IndexOf(arr, ch);
                    ind = (ind * a + b) % arr.Length;
                    newString += arr[ind % arr.Length];
                }
            }
            else if (radioButton2.Checked)
            {
                string text = String.Empty;
                string gamma = textBox5.Text;
                if (!String.IsNullOrEmpty(textBox1.Text))
                    text = textBox1.Text;
                else
                {
                    while (!f.EndOfStream)
                    {
                        text += f.ReadLine() + " ";
                    }
                    textBox1.Text = text;
                    f.Close();
                }
                for (int i = 0; i < text.Length; i++)
                {
                    int ind = Array.IndexOf(arr, text[i]);
                    int gInd = Array.IndexOf(arr, gamma[i % gamma.Length]);
                    newString += arr[(ind + gInd) % arr.Length];
                }
            }
            StreamWriter write = new StreamWriter("encrypt.txt");
            write.WriteLine(newString);
            write.Close();
            textBox4.Text = newString;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string newString = "";
            StreamReader f = new StreamReader("encrypt.txt");
            if (radioButton1.Checked)
            {
                textBox1.Clear();
                int a = Convert.ToInt32(textBox2.Text);
                int b = Convert.ToInt32(textBox3.Text);
                string text = String.Empty;
                if (!String.IsNullOrEmpty(textBox1.Text))
                    text = textBox4.Text;
                else
                {
                    while (!f.EndOfStream)
                    {
                        text += f.ReadLine();
                    }
                    textBox4.Text = text;
                    f.Close();
                }
                foreach (char ch in text)
                {
                    int ind = Array.IndexOf(arr, ch);
                    ind = (ind - b) * getInverse(a) % arr.Length;
                    if (ind < 0) ind += arr.Length;
                    newString += arr[ind];
                }
            }
            else if (radioButton2.Checked)
            {
                string text = String.Empty;
                string gamma = textBox5.Text;
                if (!String.IsNullOrEmpty(textBox4.Text))
                    text = textBox4.Text;
                else
                {
                    while (!f.EndOfStream)
                    {
                        text += f.ReadLine();
                    }
                    textBox4.Text = text;
                    f.Close();
                }
                for (int i = 0; i < text.Length; i++)
                {
                    int ind = Array.IndexOf(arr, text[i]);
                    int gInd = Array.IndexOf(arr, gamma[i % gamma.Length]);
                    newString += arr[(ind - gInd + arr.Length) % arr.Length];
                }
            }
            StreamWriter write = new StreamWriter("decrypt.txt");
            write.WriteLine(newString);
            write.Close();
            textBox1.Text = newString;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Text = "Афинный";
            label1.Visible = label2.Visible = textBox2.Visible = textBox3.Visible = true;
            label5.Visible = textBox5.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox2.Text = "Гаммирование";
            label1.Visible = label2.Visible = textBox2.Visible = textBox3.Visible = false;
            label5.Visible = textBox5.Visible = true;
        }
    }
}
