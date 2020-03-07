using System;
using System.Collections;
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
    public partial class Form3 : Form
    {
        int Lang;
        string[] Res;
        double[] p;
        List<char> Alpha;
        char[] message;

        public Form3(double[] otn_chast, List<char> alfavit, char[] c)
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
            double temp;
            for (int i = 0; i < p.Length - 1; i++)
            {
                for (int j = i + 1; j < p.Length; j++)
                {
                    if (p[i] > p[j])
                    {
                        temp = p[i];
                        p[i] = p[j];
                        p[j] = temp;
                    }
                }
            }

            string str = new string(message);
            HuffmanTree TR = new HuffmanTree(str, p);
            TR.getEncodingArray().CopyTo(Res,0);

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
            Button2_Click(sender, e);
            string Kod = "";
            for (int i = 0; i < message.Length; i++)
            {
                Kod += Res[Alpha.IndexOf(message[i])];
            }
            textBox1.Text = Kod;
        }
    }

    public class Node
    {
        private double frequence;//частота
        private char letter;//буква
        private Node leftChild;//левый потомок
        private Node rightChild;//правый потомок



        public Node(char letter, double frequence)
        { //собственно, конструктор
            this.letter = letter;
            this.frequence = frequence;
        }

        public Node() { }//перегрузка конструтора для безымянных узлов(см. выше в разделе о построении дерева Хаффмана)
        public void addChild(Node newNode)
        {//добавить потомка
            if (leftChild == null)//если левый пустой=> правый тоже=> добавляем в левый
                leftChild = newNode;
            else
            {
                if (leftChild.getFrequence() <= newNode.getFrequence()) //в общем, левым потомком
                    rightChild = newNode;//станет тот, у кого меньше частота
                else
                {
                    rightChild = leftChild;
                    leftChild = newNode;
                }
            }

            frequence += newNode.getFrequence();//итоговая частота
        }

        public Node getLeftChild()
        {
            return leftChild;
        }

        public Node getRightChild()
        {
            return rightChild;
        }

        public double getFrequence()
        {
            return frequence;
        }

        public char getLetter()
        {
            return letter;
        }

        public bool isLeaf()
        {//проверка на лист
            return leftChild == null && rightChild == null;
        }
    }


    class BinaryTree
    {
        private Node root;

        public BinaryTree()
        {
            root = new Node();
        }

        public BinaryTree(Node root)
        {
            this.root = root;
        }

        public double getFrequence()
        {
            return root.getFrequence();
        }

        public Node getRoot()
        {
            return root;
        }
    }

    class PriorityQueue
    {
        private List<BinaryTree> data;//список очереди
        private int nElems;//кол-во элементов в очереди

        public PriorityQueue()
        {
            data = new List<BinaryTree>();
            nElems = 0;
        }

        public void insert(BinaryTree newTree)
        {//вставка
            if (nElems == 0)
                data.Add(newTree);
            else
            {
                for (int i = 0; i < nElems; i++)
                {
                    if (data[i].getFrequence() > newTree.getFrequence())
                    {//если частота вставляемого дерева меньше 
                        data.Insert(i, newTree);//чем част. текущего, то cдвигаем все деревья на позициях справа на 1 ячейку                   
                        break;//затем ставим новое дерево на позицию текущего
                    }
                    if (i == nElems - 1)
                        data.Add(newTree);
                }
            }
            nElems++;//увеличиваем кол-во элементов на 1
        }

        public BinaryTree remove()
        {//удаление из очереди
            BinaryTree tmp = data[0];//копируем удаляемый элемент
            data.RemoveAt(0);//собственно, удаляем
            nElems--;//уменьшаем кол-во элементов на 1
            return tmp;//возвращаем удаленный элемент(элемент с наименьшей частотой)
        }

        public bool NotOne()
        {
            if (nElems > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class HuffmanTree
    {
        private int ENCODING_TABLE_SIZE; //длина кодировочной таблицы
        private String myString;//сообщение
        private BinaryTree huffmanTree;//дерево Хаффмана
        private double[] freqArray;//частотная таблица
        private String[] encodingArray;//кодировочная таблица


        //----------------constructor----------------------
        public HuffmanTree(String newString, double[] otn_chast)
        {
            ENCODING_TABLE_SIZE = otn_chast.Length;
            myString = newString;

            freqArray = new double[ENCODING_TABLE_SIZE];

            huffmanTree = getHuffmanTree();

            encodingArray = new String[ENCODING_TABLE_SIZE];
            fillEncodingArray(huffmanTree.getRoot(), "", "");
            freqArray = otn_chast;
        }

        //------------------------huffman tree creation------------------
        private BinaryTree getHuffmanTree()
        {
            PriorityQueue pq = new PriorityQueue();
            //алгоритм описан выше
            for (int i = 0; i < ENCODING_TABLE_SIZE; i++)
            {
                Node newNode = new Node((char)i, freqArray[i]);//то создать для него Node
                BinaryTree newTree = new BinaryTree(newNode);//а для Node создать BinaryTree
                pq.insert(newTree);//вставить в очередь
            }

            while (true)
            {
                BinaryTree tree1 = pq.remove();//извлечь из очереди первое дерево.

                try
                {
                    BinaryTree tree2 = pq.remove();//извлечь из очереди второе дерево

                    Node newNode = new Node();//создать новый Node
                    newNode.addChild(tree1.getRoot());//сделать его потомками два извлеченных дерева
                    newNode.addChild(tree2.getRoot());

                    pq.insert(new BinaryTree(newNode));
                }
                catch
                {//осталось одно дерево в очереди
                    return tree1;
                }

            }
        }

        //public BinaryTree GetTree()
        //{
        //    return huffmanTree;
        //}

        void fillEncodingArray(Node node, String codeBefore, String direction)
        {//заполнить кодировочную таблицу
            if (node.isLeaf())
            {
                encodingArray[(int)node.getLetter()] = codeBefore + direction;
            }
            else
            {
                fillEncodingArray(node.getLeftChild(), codeBefore + direction, "0");
                fillEncodingArray(node.getRightChild(), codeBefore + direction, "1");
            }
        }

        public String[] getEncodingArray()
        {
            return encodingArray;
        }
    }

    
}
