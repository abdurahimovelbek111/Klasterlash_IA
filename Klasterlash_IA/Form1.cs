using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace Klasterlash_IA
{

    public partial class Form1 : Form
    {
        int k = 0;        
        List<List<int>> groups = new List<List<int>>();
        double [,]centers;
        public string path;
        public Form1()
        {
            InitializeComponent();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text|*.txt|All|*.*";
            openFileDialog.Title = "Text faylni tanlang";
            openFileDialog.InitialDirectory = @"C:\Users\Downloads";
            openFileDialog.ShowDialog();
            path = openFileDialog.FileName;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Read();
        }
        public void Read()
        {            
            int j = 1;
            using (StreamReader reader = new StreamReader(path))
            {               
                string ss = reader.ReadLine();
                int probil = 0, element = 0;
                for (int i = 0; i < ss.Length; i++)
                {
                    if(ss[i]==' ')
                    {
                        probil++;
                    }                   
                }
                element = reader.ReadLine().Length - probil;
                string satr = "";
                if (element>probil)
                {
                    for (int i = 1; i <=element+1; i++)
                    {
                        if(i==1)
                        {
                            satr += " " + "№";
                        }
                        else
                        {
                             satr +=" "+ (i-1);
                        }
                    }
                }
                else
                {
                    for (int i = 1; i <=element; i++)
                    {
                        satr +=" "+i;
                    }
                }                             
                string[] qatorr = satr.Split(' ');
                DataTable dataTable = new DataTable();
                foreach (var ustun in qatorr)
                {
                    if(ustun=="")
                    {
                        continue;
                    }
                    dataTable.Columns.Add(ustun);
                }
                string newLine;
                using(StreamReader reader1=new StreamReader(path))
                {
                    while ((newLine = reader1.ReadLine()) != null)
                    {                       
                        newLine = j.ToString() + " " + newLine;
                        j++;
                        DataRow dataRow = dataTable.NewRow();
                        string[] values = newLine.Split(' ');
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i] == "")
                            {
                                continue;
                            }
                            else
                            {
                                dataRow[i] = values[i];
                            }
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                    reader1.Close();
                    dataGridView1.DataSource = dataTable;
                }               
            }
        }
        public void Hisobla()
        {    
                Klaster();
                Markaz();
        }
        public double Masofa(int group, int tanlanma)
        {
            double s = 0;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                s += Math.Pow(double.Parse(dataGridView1.Rows[tanlanma].Cells[i].Value.ToString()) - centers[group, i], 2);
            }
            return Math.Sqrt(s);
        }
        public void Markaz()
        {
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < dataGridView1.Rows.Count; j++)
                {
                    centers[i, j] = 0;
                }
            }
            for (int j = 0; j < groups.Count; j++)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (i <dataGridView1.Columns.Count)
                        centers[groups[j][0], groups[j][1]] += (double.Parse(dataGridView1.Rows[groups[j][0]].Cells[i].Value.ToString()) + centers[groups[j][0], groups[j][1]]) / 2;
                }
            }
        }     
        public void Klaster()
        {            
           groups.Clear();   
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                List<int> te = new List<int>();
                int x = 0;
                double mas = 0;
                for (int j = 0; j < k; j++)
                {
                    if (j == 0)
                    {
                        x = 0;
                        mas = Masofa(j, i);
                    }
                    else
                        if (mas > Masofa(j, i))
                    {
                        mas = Masofa(j,i);
                        x = j;
                    }                   
                }
                te.Add(x);
                te.Add(i);
                groups.Add(te);               
            }

        }
        public void Chiqar()
        {           
            DataTable dataTable = new DataTable();
            string satr = "";
            for (int i = 1; i <= dataGridView1.Rows.Count; i++)
            {
                satr += $"{i}-Indeks" + " ";
            }
            string[] X = satr.Split(' ');
            foreach (var item in X)
            {
                dataTable.Columns.Add(item);
            }
            List<int> vs = new List<int>();
            int s = dataGridView1.Rows.Count;
            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < groups.Count; j++)
                {
                    if (i == groups[j][0])
                        vs.Add(groups[j][1]);
                }
                DataRow dataRow = dataTable.NewRow();
                int kk = 0;
                for (int g = 1; g <s; g++)
                {
                    if ((g-1) == 0)
                    {
                        dataRow[g-1] = i+1;
                    }
                    if ((g) <= vs.Count)
                    {
                        dataRow[g] = vs[kk];
                        kk++;
                    }
                    else
                    {
                        dataRow[g] = null;
                    }
                }
                kk = 0;
                dataTable.Rows.Add(dataRow);
                vs.Clear();
            }
            dataGridView2.DataSource = dataTable;
        }
       
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                MessageBox.Show("K kiritilmadi.");
            else
            {
                k = int.Parse(textBox1.Text);
                if (k < dataGridView1.Rows.Count)
                {
                    centers = new double[k, dataGridView1.Rows.Count];
                    for (int i = 0; i < k; i++)
                    {
                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                        {
                            if (j < dataGridView1.Columns.Count)
                                centers[i, j] = double.Parse(dataGridView1.Rows[i].Cells[j].Value.ToString());
                        }
                    }
                    Hisobla();
                    Chiqar();
                }

                else
                    MessageBox.Show("K kichik bo'lsin tanlanmalar sonidan!!!");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
