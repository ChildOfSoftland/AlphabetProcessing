using System;
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
    public partial class Form2 : Form
    {
        int Lang;
        string[] Res;
        double[] p;
        List<char> Alpha;
        char[] message;

        public Form2(double[] otn_chast, List<char> alfavit, char[] c)
        {
            InitializeComponent();
            try
            {
                Lang = otn_chast.Count();
                Res = new string[Lang];
                //p = new double[R];
                Alpha = alfavit;
                p = otn_chast;
                message = c;
            }
            catch
            {
                MessageBox.Show("Данные или их часть не были введены!");
                return;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Fano(0, Lang-1);

            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.ColumnCount = Lang;
            dataGridView1.RowCount = 4;

            for (int i = 0; i < Lang; i++)
            {
                dataGridView1.Rows[0].Cells[i].Value = i.ToString();
                dataGridView1.Rows[1].Cells[i].Value = Alpha[i].ToString();
                dataGridView1.Rows[2].Cells[i].Value = p[i].ToString();
                dataGridView1.Rows[3].Cells[i].Value = Res[i].ToString();
            }

            double n = 0.0;
            for (int i = 0; i < Lang; i++)
            {
                n += p[i] * Res[i].ToString().Length;
            }

            label4.Text = n.ToString();

            double ent = Entrop(p);
            label6.Text = (ent / n).ToString();
        }

        public int Razdelenie(int L, int R)
        {
            int m;
            double schet1 = 0;
            for (int i = L; i <= R - 1; i++)
            {
                schet1 += p[i];
            }

            double schet2 = p[R];
            m = R;
            while (schet1 >= schet2)
            {
                m--;
                schet1 -= p[m];
                schet2 += p[m];
            }
            return m;
        }

        public void Fano(int L, int R)
        {
            int n;

            if (L < R)
            {

                n = Razdelenie(L, R);

                for (int i = L; i <= R; i++)
                {
                    if (i <= n) Res[i] += Convert.ToByte(0);
                    else Res[i] += Convert.ToByte(1);
                }

                Fano(L, n);
                Fano(n + 1, R);
            }
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

        private void Button1_Click(object sender, EventArgs e)
        {
            //Button2_Click(sender, e);
            string Kod = "";
            for (int i = 0; i < message.Length; i++)
            {
                Kod += Res[Alpha.IndexOf(message[i])];
            }
            textBox1.Text = Kod;
        }
    }
}
