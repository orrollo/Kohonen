using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kohonen
{
    public class Elem
    {
        private int[] vector;

        public Elem()
        {
            vector = new int[3];
        }

        public Elem(int[] vec)
        {
            vector = new int[3];
            vector[0] = vec[0];
            vector[1] = vec[1];
            vector[2] = vec[2];
        }

        public Elem(Elem elem)
        {
            vector = new int[3];
            vector[0] = elem.vector[0];
            vector[1] = elem.vector[1];
            vector[2] = elem.vector[2];
        }

        public void Insert(int value, int pos)
        {
            if (pos >= 0 && pos < 3)
            {
                vector[pos] = value;
            }
        }

        public int[] Vector
        {
            get { return vector; }
            set { vector = value; }
        }

    }
}
