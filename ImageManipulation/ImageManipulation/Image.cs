using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public class Image
    {
        private Pixel[,] _pixel;

        public String Metadata
        { get; private set; }

        public int MaxRange
        { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Pixel[,] Pixel
        {
            get
            {
                return this._pixel;
            }
            private set
            {
                Pixel[,] _pixel = new Pixel[value.GetLength(0), value.GetLength(1)];
                for (int i = 0; i < value.GetLength(0); i++)
                {
                    for (int j = 0; j < value.GetLength(1); j++)
                    {
                        _pixel[i, j] = new Pixel(value[i, j].Red, value[i, j].Green, value[i, j].Blue);
                    }
                } // nevermind, figure out indexer!!!
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
            this.Metadata = metadata;
            this.MaxRange = maxRange;

            Pixel[,] temp = new Pixel[pixel.GetLength(0), pixel.GetLength(1)];
            for (int i = 0; i < pixel.GetLength(0); i++)
            {
                for (int j = 0; j < pixel.GetLength(1); j++)
                {
                    temp[i, j] = new Pixel(pixel[i, j].Red, pixel[i, j].Green, pixel[i, j].Blue);
                }
            } //use setter now
        }

        /// <summary>
        /// 
        /// </summary>
        public void ToGrey()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="horizontal"></param>
        public void Flip(Boolean horizontal)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        public void Crop(int startX, int startY, int endX, int endY)
        { }
    }
}
