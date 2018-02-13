using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public class Image
    {
        private Pixel[,] data;

        public String Metadata
        {
            get;
            private set; }

        public int MaxRange
        {
            get;
            private set;
        }

        /// <summary>
        /// Indexer for the Pixel array that makes up the whole image. Getter and setter both make deep copies.
        /// </summary>
        public Pixel this[int i, int j]
        {
            get
            {
                return new Pixel(data[i, j].Red, data[i, j].Green, data[i, j].Blue);
            }
            set
            {
                data[i, j] = new Pixel(value.Red, value.Green, value.Blue);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="maxRange"></param>
        /// <param name="pixel"></param>
        public Image(String metadata, int maxRange, Pixel[,] pixel)
        {
            if (maxRange < 0)  
                throw new ArgumentException("The max range cannot be negative");
            
            this.Metadata = metadata;
            this.MaxRange = maxRange;

            data = new Pixel[pixel.GetLength(0), pixel.GetLength(1)];
            for (int i = 0; i < pixel.GetLength(0); i++)
            {
                for (int j = 0; j < pixel.GetLength(1); j++)
                {
                    this[i,j] = pixel[i,j];
                }
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public int GetLength(int rank)
        {
            return data.GetLength(rank);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ToGrey()
        {
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j] = new Pixel(data[i, j].Grey()); 
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="horizontal"></param>
        public void Flip(Boolean horizontal)
        {
            if (horizontal)
            {
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    for (int j = 0; j < data.GetLength(1); j++)
                    {
                        Pixel temp = data[i, j];
                        data[i, j] = data[i, data.GetLength(0) - j];
                        data[i, data.GetLength(0) - j] = temp;                        
                    }
                }
            }

            else
            {
                for (int i = 0; i < data.GetLength(1); i++)
                {
                    for (int j = 0; j < data.GetLength(0); j++)
                    {
                        Pixel temp = data[j, i];
                        data[j, i] = data[data.GetLength(0) - j, i];
                        data[data.GetLength(0) - j, i] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        public void Crop(int startX, int startY, int endX, int endY)
        {
            if (startX > endX || startY > endY || startX < 0 || startY < 0 || endX > data.GetLength(0) || endY > data.GetLength(1))
                throw new ArgumentException("Invalid input");

            Pixel[,] temp = new Pixel[endX - startX, endY - startY];

            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {
                    temp[i, j] = data[startX, startY];
                    startY++;
                }
                startX++;
            }
            data = temp;
        }
    }
}
