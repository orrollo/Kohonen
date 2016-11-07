using System;
using System.Collections.Generic;
using System.Threading;

namespace Kohonen
{
	public class KohonenMap
	{
		private List<List<Elem>> _map;
		private double _radius;
		private int _iterationCount = 2000;
		
		public double Eps1 = 0.0005;
		public double Value1 = 0.001;

		public Mutex mapMutex = new Mutex(false);

		//double value2 = 1.0;

		public KohonenMap(int width, int height)
		{
			Width = width;
			Height = height;
			ReInitMap();
		}

		public void ReInitMap()
		{
			_map = InitializeMap(Width, Height);
			var maxSize = Math.Max(Height, Width);
			_radius = Math.Sqrt(Math.Pow(maxSize >> 1, 2)*2)/2;
			Value1 = 0.001;
		}

		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public List<List<Elem>> Map
		{
			get { return _map; }
		}

		public double Radius
		{
			get { return _radius; }
		}

		public int IterationCount
		{
			get { return _iterationCount; }
			set { _iterationCount = value; }
		}

		protected static List<List<Elem>> InitializeMap(int width, int height)
		{
			var r = new Random();
			var newMap = new List<List<Elem>>();
			var temp = new List<Elem>();
			for(int i = 0; i < height; i++)
			{
				for(int j = 0; j < width; j++) temp.Add(new Elem(r, 0, 255));
				newMap.Add(new List<Elem>(temp));
				temp.Clear();
			}
			return newMap;
		}

		public void FillNetwork(List<Elem> controlSet)
		{
			var random = new Random();
			var currentRadius = _radius;
			for (var i = 0; i < _iterationCount; i++)
			{
				var k = /*r.Next(0, 7)*/ i%controlSet.Count;
				var controlElem = controlSet[k];

				//ищем BMU
				var bestI = -1;
				var bestJ = -1;
				var minX = Int32.MaxValue;
				var minY = Int32.MaxValue;
				var minZ = Int32.MaxValue;
				//int start_j = r.Next(0, map.Count - 1);
				//int start_m = r.Next(0, map[start_j].Count - 1);
				var bmus = new List<Tuple<int, int>>(1000);
				for (var curY = 0; curY < _map.Count; curY++)
					for (var curX = 0; curX < _map[curY].Count; curX++)
					{
						var elem = _map[curY][curX];
						var x = Math.Abs(elem.Vector[0] - controlElem.Vector[0]);
						var y = Math.Abs(elem.Vector[1] - controlElem.Vector[1]);
						var z = Math.Abs(elem.Vector[2] - controlElem.Vector[2]);
						//int sum = x + y + z;
						if (minX < x || minY < y || minZ < z) continue;
						if (x <= 10 && y <= 10 && z <= 10) bmus.Add(new Tuple<int, int>(curY, curX));
						minX = x;
						minY = y;
						minZ = z;
						bestI = curY;
						bestJ = curX;
					}

				if (bmus.Count != 0)
				{
					var ind = random.Next(0, bmus.Count - 1);
					bestI = bmus[ind].Item1;
					bestJ = bmus[ind].Item2;
				}

				if (mapMutex.WaitOne(5000))
				{
					try
					{
						for (var curY = 0; curY < _map.Count; curY++)
						{
							var elems = _map[curY];
							for (var curX = 0; curX < elems.Count; curX++)
							{
								if (curY == bestI && curX == bestJ) continue;
								var dy = curY - bestI;
								var dx = curX - bestJ;
								var distance = Math.Sqrt(dy*dy + dx*dx);
								if (!(distance < currentRadius)) continue;
								//функция степени соседства 0 < h < 1
								//double h = 1 / (1 + 0.5* distance) /* (current_radius * 0.005)*/;
								//double h = distance * (0.001);
								//double h = value1;
								var h = Math.Exp(-distance*0.03) /*- Math.Log(current_radius)*0.0001*/;
								var elem = elems[curX];

								var red = elem.Red;
								var green = elem.Green;
								var blue = elem.Blue;

								red = (int) Math.Ceiling(red + h*(controlElem.Vector[0] - red));
								green = (int) Math.Ceiling(green + h*(controlElem.Vector[1] - green));
								blue = (int) Math.Ceiling(blue + h*(controlElem.Vector[2] - blue));

								if (red == Int32.MinValue || green == Int32.MinValue || blue == Int32.MinValue)
									continue;

								elem.Vector = new[] {red, green, blue};
							}
						}
						Value1 -= Eps1;
						currentRadius -= _iterationCount/(_radius*100.0);
					}
					finally
					{
						mapMutex.ReleaseMutex();
					}
				}
				else
				{
					Thread.Sleep(100);
				}

				//Thread.Sleep(0);
			}
		}
	}
}