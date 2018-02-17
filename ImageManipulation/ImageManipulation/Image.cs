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
        /// Constructor creates deep copy of pixel array
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="maxRange"></param>
        /// <param name="pixel"></param>
        public Image(String metadata, int maxRange, Pixel[,] pixel)
        {
            if (pixel == null)
                throw new ArgumentException("The pixel array cannot be null");

            if (metadata == null)
                metadata = "";

            if (maxRange < 0)  
                throw new ArgumentException("The max range cannot be negative");
            
            this.Metadata = metadata;
            this.MaxRange = maxRange;

            data = new Pixel[pixel.GetLength(0), pixel.GetLength(1)];
            for (int i = 0; i < pixel.GetLength(0); i++)
            {
                for (int j = 0; j < pixel.GetLength(1); j++)
                {
                    if (pixel[i, j] == null)
                        throw new ArgumentException("The pixel array is not filled to capacity");

                    if (pixel[i, j].Red > maxRange || pixel[i, j].Green > maxRange || pixel[i, j].Blue > maxRange)
                        throw new ArgumentException("The pixel " + pixel[i, j] + " does not respect given max range.");

                    this[i,j] = pixel[i,j];
                }
            } 
        }

        /// <summary>
        /// GetLength for image indexer
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public int GetLength(int rank)
        {
            return data.GetLength(rank);
        }

        /// <summary>
        /// Convert the image to gray
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
        /// Flips the image horizontally or vertically
        /// </summary>
        /// <param name="horizontal"></param>
        public void Flip(Boolean horizontal)
        {
            if (horizontal)
            {
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    for (int j = 0; j < data.GetLength(1)/2; j++)
                    {
                        Pixel temp = data[i, j];
                        data[i, j] = data[i, data.GetLength(1) - 1 - j];
                        data[i, data.GetLength(1) - 1 - j] = temp;                        
                    }
                }
            }

            else
            {
                for (int i = 0; i < data.GetLength(1); i++)
                {
                    for (int j = 0; j < data.GetLength(0)/2; j++)
                    {
                        Pixel temp = data[j, i];
                        data[j, i] = data[data.GetLength(0) - 1 - j, i];
                        data[data.GetLength(0) - 1 - j, i] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// Crops image to given size
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        public void Crop(int startX, int startY, int endX, int endY)
        {
            if (startX > endX || startY > endY || startX < 0 || startY < 0 
                || endX > data.GetLength(0) || endY > data.GetLength(1))
                throw new ArgumentException("Invalid input");

            Pixel[,] temp = new Pixel[endX - startX, endY - startY];

            int xcount = startX;
            int ycount = startY;

            for (int i = 0; i < temp.GetLength(0); i++)
            {
                for (int j = 0; j < temp.GetLength(1); j++)
                {                 
                    temp[i, j] = data[xcount, ycount];
                    ycount++;
                }
                ycount = startY;
                xcount++;
            }
            data = temp;
        }

        public override Boolean Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is Image))
            {
                return false;
            }
            Image img = (Image)obj;

            if (!this.Metadata.Equals(img.Metadata) || !this.MaxRange.Equals(img.MaxRange))
            {
                return false;
            }
            if (!this.GetLength(0).Equals(img.GetLength(0)) || !this.GetLength(1).Equals(img.GetLength(1)))
            { 
                return false;
            }

            for (int i = 0; i < this.GetLength(0); i++)
            {
                for (int j = 0; j < this.GetLength(1); j++)
                {
                    if (!this[i, j].Equals(img[i, j])) return false;
                }
            }
            return true;
        }
    }
}
