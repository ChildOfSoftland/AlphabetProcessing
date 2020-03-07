using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlphabetProcessing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static char[] c;
        static List<char> alfavit;
        static List<string> sochet;
        static double[] ver;

        private void Button3_Click(object sender, EventArgs e)
        {
            int n = textBox1.Text.Length;
            c = new char[n];
            c = textBox1.Text.ToCharArray();
            if (c.Length > 0)
            {
                MessageBox.Show("Сообщение прочитанно!");
            }
            else
            {
                MessageBox.Show("Сообщение не было прочитанно!");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            c = new char[0];
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("Message.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();
                    c = line.ToCharArray();
                }
            }
            catch
            {
                MessageBox.Show("Файл не найден!");
            }
            if (c.Length > 0)
            {
                MessageBox.Show("Сообщение прочитанно!");
            }
            else
            {
                MessageBox.Show("Сообщение не было прочитанно!");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            int[] chast = new int[0];
            double[] otn_chast = new double[0];
            alfavit = new List<char>();
            int[] chast2 = new int[0];
            double[] otn_chast2 = new double[0];
            sochet = new List<string>();

            if (c.Length <= 0)
            {
                MessageBox.Show("Сообщение не было прочитанно!");
                return;
            }

            for (int i = 0; i < c.Length; i++)
            {
                if (alfavit.Any() == false || alfavit.Contains(c[i]) == false)
                {
                    alfavit.Add(c[i]);
                    Array.Resize(ref chast, chast.Length + 1);
                    Array.Resize(ref otn_chast, otn_chast.Length + 1);
                }
                chast[alfavit.IndexOf(c[i])]++;
            }

            for (int i = 0; i < chast.Length; i++)
            {
                otn_chast[i] = (double)chast[i] / (double)c.Length;
            }

            int temp;
            double temp1;
            char temp2;
            for (int i = 0; i < chast.Length - 1; i++)
            {
                for (int j = i + 1; j < chast.Length; j++)
                {
                    if (chast[i] < chast[j])
                    {
                        temp = chast[i];
                        chast[i] = chast[j];
                        chast[j] = temp;
                        temp1 = otn_chast[i];
                        otn_chast[i] = otn_chast[j];
                        otn_chast[j] = temp1;
                        temp2 = alfavit[i];
                        alfavit[i] = alfavit[j];
                        alfavit[j] = temp2;
                    }
                }
            }

            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.ColumnCount = alfavit.Count;
            dataGridView1.RowCount = 3;

            for (int i = 0; i < alfavit.Count; i++)
            {
                dataGridView1.Rows[0].Cells[i].Value = alfavit[i].ToString();
                dataGridView1.Rows[1].Cells[i].Value = chast[i].ToString();
                dataGridView1.Rows[2].Cells[i].Value = otn_chast[i].ToString();
            }

            for (int i = 0; i < c.Length - 1; i++)
            {
                if (sochet.Any() == false || sochet.Contains(c[i].ToString() + c[i+1].ToString()) == false)
                {
                    sochet.Add(c[i].ToString() + c[i+1].ToString());
                    Array.Resize(ref chast2, chast2.Length + 1);
                    Array.Resize(ref otn_chast2, otn_chast2.Length + 1);
                }
                chast2[sochet.IndexOf(c[i].ToString() + c[i+1].ToString())]++;
            }

            for (int i = 0; i < chast2.Length; i++)
            {
                otn_chast2[i] = (double)chast2[i] / (double)c.Length;
            }

            string temp3;
            for (int i = 0; i < chast2.Length - 1; i++)
            {
                for (int j = i + 1; j < chast2.Length; j++)
                {
                    if (chast2[i] < chast2[j])
                    {
                        temp = chast2[i];
                        chast2[i] = chast2[j];
                        chast2[j] = temp;
                        temp1 = otn_chast2[i];
                        otn_chast2[i] = otn_chast2[j];
                        otn_chast2[j] = temp1;
                        temp3 = sochet[i];
                        sochet[i] = sochet[j];
                        sochet[j] = temp3;
                    }
                }
            }

            ver = otn_chast;

            dataGridView2.ColumnHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;

            dataGridView2.ColumnCount = sochet.Count;
            dataGridView2.RowCount = 3;

            for (int i = 0; i < sochet.Count; i++)
            {
                dataGridView2.Rows[0].Cells[i].Value = sochet[i].ToString();
                dataGridView2.Rows[1].Cells[i].Value = chast2[i].ToString();
                dataGridView2.Rows[2].Cells[i].Value = otn_chast2[i].ToString();
            }

            dataGridView1.Show();
            dataGridView2.Show();

            label5.Text = Entrop(otn_chast).ToString();
            label7.Text = Entrop(otn_chast2).ToString();
            label9.Text = Entrop2(otn_chast, otn_chast2).ToString();
            label11.Text = LenCode().ToString();
            label14.Text = Izbt0().ToString();
            label16.Text = IzbtP(otn_chast).ToString();
            label18.Text = IzbtS(otn_chast, otn_chast2).ToString();
            label20.Text = Izbt(otn_chast, otn_chast2).ToString();
        }

        static public double Entrop(double[] p)
        {
            double y = 0.0;
            for (int i = 0; i < p.Length; i++)
            {
                y += Math.Log(p[i], 2.0) * p[i];
            }
            y = -1.0 * y;
            return y;
        }

        static public double Entrop2(double[] p, double[] p2)
        {
            double y = 0.0;
            for (int i = 0; i < p.Length; i++)
            {
                for (int j = 0; j < p2.Length; j++)
                {
                    if (alfavit[i] == sochet[j].ToCharArray()[0])
                    y += Math.Log(p2[j], 2.0) * p[i] * p2[j];
                }
            }
            y = -1.0 * y;
            return y;
        }

        static public double LenCode()
        {
            return Math.Ceiling(Math.Log(c.Length, 2.0));
        }

        static public double Izbt0()
        {
            return 1 - (Math.Log(c.Length, 2.0) / Math.Ceiling(Math.Log(c.Length, 2.0)));
        }

        static public double IzbtP(double[] p)
        {
            return 1 - (Entrop(p) / Math.Log(c.Length, 2.0));
        }

        static public double IzbtS(double[] p, double[] p2)
        {
            return 1 - (Entrop2(p,p2) / Entrop(p));
        }
        static public double Izbt(double[] p, double[] p2)
        {
            return IzbtP(p) + IzbtS(p,p2) - IzbtP(p) * IzbtS(p, p2);
        }

        private void КоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 F2 = new Form2(ver, alfavit, c);
            F2.Show();
        }

        private void АлгоритмToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 F3 = new Form3(ver, alfavit, c);
            F3.Show();
        }
    }
}
