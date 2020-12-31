using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageMatrix.Source.Common
{
    public struct Matrix
    {
        public int Width { get; set; }
        public int Height { get; set; }

        private int[] Contents { get; set; }

        public Matrix(int width, int height, params int[] contents)
        {
            //Exceptions
            if (contents.Length > width * height)
                throw new ArgumentException("Too many items", "conents");
            if (contents.Length < width * height)
                throw new ArgumentException("Too few items", "contents");
            if (width + height == 0)
                throw new ArgumentException("There must be at least one row or colummn", "width and height");

            Width = width;
            Height = height;
            Contents = contents;
        }

        public Matrix(string textFormat)
        {
            Width = 0;
            Height = 0;

            //String To Matrix
            List<int> array = new List<int>();
            string number = "";
            int i = 0;
            foreach(char c in textFormat)
            {
                if ((c >= 48 && c <= 57) || c == '-')
                    number += c;
                else if(number.Length > 0)
                {
                    i++;
                    if (Width == 0)
                    {
                        Width = Convert.ToInt32(number);
                        number = "";
                        continue;
                    }

                    if (Height == 0)
                    {
                        Height = Convert.ToInt32(number);
                        number = "";
                        continue;
                    }

                    array.Add(Convert.ToInt32(number));
                    number = "";
                }
            }

            array.Add(Convert.ToInt32(number));
            Contents = array.ToArray();
        }
        public Matrix(int width, int height)
        {
            Width = width;
            Height = height;

            //Create a List and set all values to zero
            List<int> array = new List<int>(Width * Height);
            for(int i = 0; i < array.Capacity; i++)
            {
                array.Add(0);

            }

            Contents = array.ToArray();
        }

        public float GetValue(int x, int y)
        {
            return Contents[(y * Width) + x];
        }

        public void SetValue(int x, int y, int value)
        {
            Contents[(y * Width) + x] = value;
        }

        public int this[int x, int y]
        {
            get { return Contents[(y * Width) + x]; }
            set { Contents[(y * Width) + x] = value; }
        }

        public void PrintAllContents()
        {
            string row = "";
            for(int i = 0; i < Contents.Length; i++)
            {
                row += Contents[i] + " ";

                if ((i + 1) % Width == 0)
                {
                    Console.WriteLine(row);
                    row = "";
                }
            }
        }
    }
}
