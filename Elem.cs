using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kohonen
{
    public class Elem
    {
		public static int ElemSize
		{
			get
			{
				return _elemSize ?? 3;
			}
			set 
			{ 
				if (_elemSize != null && value != _elemSize) 
					throw new ArgumentException("ElemSize is already set");
				_elemSize = value;
			}
		}

        private int[] _vector;
		private static int? _elemSize = null;

	    public int Red
	    {
		    get
		    {
				if (ElemSize != 3 || _vector == null || _vector.Length != 3)
					throw new ArgumentException("not a color map element");
				return _vector[0];
		    }
	    }

		public int Green
		{
			get
			{
				if (ElemSize != 3 || _vector == null || _vector.Length != 3)
					throw new ArgumentException("not a color map element");
				return _vector[1];
			}
		}

		public int Blue
		{
			get
			{
				if (ElemSize != 3 || _vector == null || _vector.Length != 3)
					throw new ArgumentException("not a color map element");
				return _vector[1];
			}
		}

		public Elem()
        {
            _vector = new int[ElemSize];
        }

		public Elem(Random rnd, int lower, int upper) : this()
		{
			for (int i = 0; i < _vector.Length; i++) _vector[i] = rnd.Next(lower, upper);
		}

		public Elem(params int[] vec)
		{
			if (vec == null || vec.Length == 0 || vec.Length != ElemSize) 
				throw new ArgumentException("bad data vector");
			CopyVector(vec);
		}

		//public Elem(int[] vec)
		//{
		//    vector = new int[3];
		//    vector[0] = vec[0];
		//    vector[1] = vec[1];
		//    vector[2] = vec[2];
		//}

        public Elem(Elem elem)
        {
	        CopyVector(elem.Vector);
        }

	    private void CopyVector(int[] sourceArray)
	    {
		    _vector = new int[ElemSize];
		    Array.Copy(sourceArray, _vector, ElemSize);
	    }

	    public void Insert(int value, int pos)
	    {
		    if (pos < 0 || pos >= Vector.Length) throw new ArgumentException("bad index value");
		    _vector[pos] = value;
	    }

	    public int[] Vector
        {
            get { return _vector; }
            set
            {
	            if (value == null || value.Length != ElemSize) 
					throw new ArgumentException("bad data vector");
				_vector = value;
            }
        }

    }
}
