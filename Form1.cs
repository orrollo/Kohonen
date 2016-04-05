using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;

namespace Kohonen
{
    public partial class Form1 : Form
    {
        List<List<Elem>> map;
        List<Elem> ControlSet;
        Bitmap bm;
        double radius;
        int IterationCount = 2000;
        Thread newThread1;
        Thread newThread2;
        const double eps1 = 0.0005;
        const double eps2 = 0.005;
        double value1 = 0.001;
        double value2 = 1.0;

        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bm;
            InitializeMap();
            InitializeControlSet();
            radius = Math.Sqrt(Math.Pow(Math.Max(pictureBox1.Height, pictureBox1.Width) / 2, 2) *2) / 2;
        }

        public void InitializeMap()
        {
            Random r = new Random();
            map = new List<List<Elem>>();
            List<Elem> temp = new List<Elem>();
            for(int i = 0; i < pictureBox1.Height; i++)
            {
                for(int j = 0; j < pictureBox1.Width; j++)
                {
                    Elem e = new Elem();
                    for (int k = 0; k < 3; k++)
                        e.Insert(r.Next(0, 255), k);
                    temp.Add(e);
                }
                map.Add(new List<Elem>(temp));
                temp.Clear();
            }
        }

        public void InitializeControlSet()
        {
            ControlSet = new List<Elem>();
            //красный
            ControlSet.Add(new Elem(new int[] {255, 0, 0}));
            //зеленый
            ControlSet.Add(new Elem(new int[] { 0, 128, 0 }));
            //синий
            ControlSet.Add(new Elem(new int[] { 0, 0, 255 }));
            //темно-зеленый
            ControlSet.Add(new Elem(new int[] { 0, 100, 0 }));
            //темно-синий
            ControlSet.Add(new Elem(new int[] { 0, 0, 139 }));
            //желтый
            ControlSet.Add(new Elem(new int[] { 255, 255, 0 }));
            //оранжевый
            ControlSet.Add(new Elem(new int[] { 255, 165, 0 }));
            //фиолетовый
            ControlSet.Add(new Elem(new int[] { 128, 0, 128 }));
        }

        public void FillNetwork()
        {
            Random r = new Random();
            double current_radius = radius;
            for(int i = 0; i < IterationCount; i++)
            {
                int k = /*r.Next(0, 7)*/ i % 8;
                Elem ControlElem = ControlSet[k];

                //ищем BMU
                int best_i = -1, best_j = -1, minX = Int32.MaxValue, minY = Int32.MaxValue, minZ = Int32.MaxValue;
                //int start_j = r.Next(0, map.Count - 1);
                //int start_m = r.Next(0, map[start_j].Count - 1);
                List<Tuple<int, int>> bmus = new List<Tuple<int, int>>(1000);
                for(int j = 0; j < map.Count; j++)
                    for (int m = 0; m < map[j].Count; m++)
                    {
                        int x = Math.Abs(map[j][m].Vector[0] - ControlElem.Vector[0]);
                        int y = Math.Abs(map[j][m].Vector[1] - ControlElem.Vector[1]);
                        int z = Math.Abs(map[j][m].Vector[2] - ControlElem.Vector[2]);
                        //int sum = x + y + z;
                        if (minX >= x && minY >= y && minZ >= z)
                        {
                            if (x <= 0 && x <= 10 && y <= 0 && y <= 10 && z <= 0 && z <= 10)
                                bmus.Add(new Tuple<int, int>(j, m));
                            minX = x;
                            minY = y;
                            minZ = z;
                            best_i = j;
                            best_j = m;
                        }
                    }

                if(bmus.Count != 0)
                {
                    int ind = r.Next(0, bmus.Count - 1);
                    best_i = bmus[ind].Item1;
                    best_j = bmus[ind].Item2;
                }
                for (int j = 0; j < map.Count; j++)
                    for (int m = 0; m < map[j].Count; m++)
                    {
                        if (j == best_i && m == best_j)
                            continue;
                        double distance = Math.Sqrt(Math.Pow(j - best_i, 2) + Math.Pow(m - best_j, 2));
                        if (distance < current_radius)
                        {
                            //функция степени соседства 0 < h < 1
                            //double h = 1 / (1 + 0.5* distance) /* (current_radius * 0.005)*/;
                            //double h = distance * (0.001);
                            //double h = value1;
                            double h = Math.Exp(-distance*0.03) /*- Math.Log(current_radius)*0.0001*/;
                            int red = map[j][m].Vector[0];
                            int green = map[j][m].Vector[1];
                            int blue = map[j][m].Vector[2];
                            red = (int)Math.Ceiling(red + h * (ControlElem.Vector[0] - red));
                            green = (int)Math.Ceiling(green + h * (ControlElem.Vector[1] - green));
                            blue = (int)Math.Ceiling(blue + h * (ControlElem.Vector[2] - blue));

                            if (red == Int32.MinValue || green == Int32.MinValue || blue == Int32.MinValue)
                                continue;

                            map[j][m].Vector = new int[] { red, green, blue };
                        }
                    }
                value1 -= eps1;
                current_radius -= (double)IterationCount / (radius * 100);
            }
        }

        public void DrawMap()
        {

            PBInvalidateDelegate PBI = new PBInvalidateDelegate( PBInvalidate);
            GetMapDelegate gmd = new GetMapDelegate( GetMap);
            SetBitmapDelegate sbd = new SetBitmapDelegate(SetBitmap);
            GetPBWidthDelegate PBwidth = new GetPBWidthDelegate(GetPBWidth);
            GetPBHeightDelegate PBheight = new GetPBHeightDelegate(GetPBHeight);
            int width = (int)Invoke(PBwidth);
            int height = (int)Invoke(PBheight);
            while (true)
            {
                Bitmap b = new Bitmap(width, height);
                List<List<Elem>> curMap = Invoke(gmd) as List<List<Elem>>;
                for (int i = 0; i < curMap.Count; i++)
                    for (int j = 0; j < curMap[i].Count; j++)
                        b.SetPixel(j, i, Color.FromArgb(curMap[i][j].Vector[0], curMap[i][j].Vector[1], curMap[i][j].Vector[2]));
                Invoke(sbd, b);
                Invoke(PBI);
            }
        }

        delegate void PBInvalidateDelegate();
        delegate List<List<Elem>> GetMapDelegate();
        delegate void SetBitmapDelegate(Bitmap b);
        delegate int GetPBWidthDelegate();
        delegate int GetPBHeightDelegate();

        public void PBInvalidate()
        {
            if(pictureBox1 != null)
                pictureBox1.Invalidate();
        }

        public List<List<Elem>> GetMap()
        {
            return map;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(newThread1 != null)
                newThread1.Abort();
            if(newThread2 != null)
                newThread2.Abort();
        }

        private void SetBitmap(Bitmap b)
        {
            bm = new Bitmap(b);
            pictureBox1.Image = bm;
        }

        private int GetPBWidth()
        {
            return pictureBox1.Width;
        }

        private int GetPBHeight()
        {
            return pictureBox1.Height;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            toolStripMenuItem1.Enabled = false;
            newThread1 = new Thread(new ThreadStart(DrawMap));
            newThread1.IsBackground = true;
            newThread1.Start();
            newThread2 = new Thread(new ThreadStart(FillNetwork));
            newThread2.IsBackground = true;
            newThread2.Start();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (newThread1 != null)
                newThread1.Abort();
            if (newThread2 != null)
                newThread2.Abort();
            InitializeMap();

            newThread1 = new Thread(new ThreadStart(DrawMap));
            newThread1.IsBackground = true;
            newThread1.Start();
            newThread2 = new Thread(new ThreadStart(FillNetwork));
            newThread2.IsBackground = true;
            newThread2.Start();
        }

        private void остановитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (newThread1 != null)
                newThread1.Abort();
            if (newThread2 != null)
                newThread2.Abort();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            bm.Save(saveFileDialog1.FileName);
        }
    }
}
