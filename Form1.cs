using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Kohonen
{
	public partial class Form1 : Form
    {
        List<Elem> ControlSet;

	    KohonenMap kohMap;

        Bitmap bm;

        Thread _newThread1;
        Thread _newThread2;

		//const double eps2 = 0.005;

		//double value1 = 0.001;
		//double value2 = 1.0;

        public Form1()
        {
	        InitializeComponent();
	        InitializeCustom();
        }

	    private void InitializeCustom()
	    {
			InitializeImage();
			
			Elem.ElemSize = 3;
			ControlSet = InitializeControlSet();
		    kohMap = new KohonenMap(pictureBox1.Width, pictureBox1.Height);
			
			//map = KohonenMap.InitializeMap(width, height);
			//var maxSize = Math.Max(height, width);
		    //radius = Math.Sqrt(Math.Pow(maxSize >> 1, 2)*2)/2;
	    }

	    private void InitializeImage()
	    {
		    bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
		    pictureBox1.Image = bm;
	    }

	    public List<Elem> InitializeControlSet()
        {
            var set = new List<Elem>();
            //красный
            set.Add(new Elem(255, 0, 0));
            //зеленый
            set.Add(new Elem(0, 128, 0));
            //синий
            set.Add(new Elem(0, 0, 255));
            //темно-зеленый
            set.Add(new Elem(0, 100, 0));
            //темно-синий
            set.Add(new Elem(0, 0, 139));
            //желтый
            set.Add(new Elem(255, 255, 0));
            //оранжевый
            set.Add(new Elem(255, 165, 0));
            //фиолетовый
            set.Add(new Elem(128, 0, 128));
		    return set;
        }

        public void FillNetwork()
        {
	        kohMap.FillNetwork(ControlSet);
        }

	    public void DrawMap()
        {

            var PBI = new PBInvalidateDelegate( PBInvalidate);
            var gmd = new GetMapDelegate( GetMap);
            var sbd = new SetBitmapDelegate(SetBitmap);
			var getPBWidth = new GetPBWidthDelegate(GetPBWidth);
            var getPBheight = new GetPBHeightDelegate(GetPBHeight);
		    var getMutex = new GetMutexDelegate(GetMutex);

			var width = (int)Invoke(getPBWidth);
			var height = (int)Invoke(getPBheight);

            while (true)
            {
                var bitmap = new Bitmap(width, height);
                var curMap = Invoke(gmd) as List<List<Elem>>;
	            var mutex = Invoke(getMutex) as Mutex;
				//lock (curMap)
				if (mutex.WaitOne(5000))
				{
					try
					{
						for (int i = 0; i < curMap.Count; i++)
						{
							var list = curMap[i];
							for (int j = 0; j < list.Count; j++)
							{
								var red = list[j].Vector[0];
								var green = list[j].Vector[1];
								var blue = list[j].Vector[2];
								var color = Color.FromArgb(red, green, blue);
								bitmap.SetPixel(j, i, color);
							}
						}
					}
					finally
					{
						mutex.ReleaseMutex();
					}
					Invoke(sbd, bitmap);
					Invoke(PBI);
				}
				//Thread.Sleep(0);
            }
        }

		private Mutex GetMutex()
		{
			return kohMap.mapMutex;
		}

		delegate void PBInvalidateDelegate();
        delegate List<List<Elem>> GetMapDelegate();
        delegate void SetBitmapDelegate(Bitmap b);
        delegate int GetPBWidthDelegate();
        delegate int GetPBHeightDelegate();
		delegate Mutex GetMutexDelegate();

	    public void PBInvalidate()
	    {
		    if (pictureBox1 == null) return;
		    if (pictureBox1.InvokeRequired)
		    {
			    var method = new Action(() => pictureBox1.Invalidate());
			    pictureBox1.Invoke(method);
		    }
		    else
		    {
				pictureBox1.Invalidate();
		    }
			
	    }

	    public List<List<Elem>> GetMap()
        {
            return kohMap.Map;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_newThread1 != null)
                _newThread1.Abort();
            if(_newThread2 != null)
                _newThread2.Abort();
        }

	    private void SetBitmap(Bitmap b)
	    {
		    if (pictureBox1.InvokeRequired)
		    {
			    var fn = new Action(() =>
				    {
					    bm = new Bitmap(b);
					    pictureBox1.Image = bm;
				    });
			    pictureBox1.Invoke(fn);
		    }
		    else
		    {
				bm = new Bitmap(b);
			    pictureBox1.Image = bm;
		    }
	    }

	    private int GetPBWidth()
        {
            return pictureBox1.Width;
        }

        private int GetPBHeight()
        {
            return pictureBox1.Height;
        }

        private void tsStartClick(object sender, EventArgs e)
        {
	        tsStart.Enabled = false;
			kohMap = new KohonenMap(pictureBox1.Width, pictureBox1.Height);
			CreateThreads();
        }

	    private void CreateThreads()
	    {
		    _newThread1 = new Thread(DrawMap) {IsBackground = true};
		    _newThread1.Start();
		    _newThread2 = new Thread(FillNetwork) {IsBackground = true};
		    _newThread2.Start();
	    }

	    private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                menuStrip.Show(Cursor.Position);
            }
        }

        private void tsRepeatClick(object sender, EventArgs e)
        {
            KillThreads();
	        //map = KohonenMap.InitializeMap(pictureBox1.Width, pictureBox1.Height);
			kohMap = new KohonenMap(pictureBox1.Width, pictureBox1.Height);

			CreateThreads();
			//newThread1 = new Thread(DrawMap) { IsBackground = true };
			//newThread1.Start();
			//newThread2 = new Thread(FillNetwork) {IsBackground = true};
			//newThread2.Start();
        }

	    private void KillThreads()
	    {
		    if (_newThread1 != null) _newThread1.Abort();
		    if (_newThread2 != null) _newThread2.Abort();
	    }

	    private void tsStopClick(object sender, EventArgs e)
        {
			//if (newThread1 != null) newThread1.Abort();
			//if (newThread2 != null) newThread2.Abort();
			KillThreads();
		}

        private void tsSaveClick(object sender, EventArgs e)
        {
            saveFile.ShowDialog();
        }

        private void saveFileOk(object sender, CancelEventArgs e)
        {
            bm.Save(saveFile.FileName);
        }
    }
}
