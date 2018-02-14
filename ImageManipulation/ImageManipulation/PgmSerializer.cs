using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public class PgmSerializer : IImageSerialization
    {
        private string formatSpec = "P2";

        public string Serialize(Image img)
        {
            return "";
        }

        /// <summary>
        /// Converts a string (usually from file) to an Image object
        /// </summary>
        /// <param name="imgData">the image data in string format</param>
        /// <returns></returns>
        public Image Parse(string imgData)
        {
            string[] lines = imgData.Split
                (new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            if(!lines[0].ToLower().Equals(formatSpec.ToLower()))
            {
                throw new InvalidDataException("Expected format : " +
                    formatSpec + ", Actual format : " + lines[0]);
            }

            string[] metadata = lines.Skip(1) //skip format specifier
                .TakeWhile(line => line.ElementAt(0).Equals('#'))
                .Select(comment => comment = comment.Substring(1)) //skip #
                .ToArray();

            int[] size = lines[metadata.Length + 1].Split(' ')
                .Select(num => int.Parse(num))
                .ToArray();

            int maxRange = int.Parse(lines[metadata.Length + 2]);

            int[] pixelsData = lines.Skip(metadata.Length + 3) //skip metadata and stuff
                .Select(line => line.Split(' ')) //gives a collection of collection
                .Select(nums => nums.Select(num => int.Parse(num)))
                // parse each collection of collection
                .SelectMany(list => list.ToArray()).ToArray();
            //flatten the array

            if(pixelsData.Length != size[0] * size[1])
            {
                throw new InvalidDataException("Expected size : " +
                    size[0] * size[1] + ", Actual format : "
                    + pixelsData.Length);
            }

            Pixel[,] pixels = new Pixel[size[0], size[1]];
            for(int i = 0; i < pixelsData.Length; i++)
            {
                pixels[i % pixels.GetLength(0),
                    i / pixels.GetLength(0)] =
                    new Pixel(pixelsData[i]);
            }

            return new Image(metadata.Aggregate
                ((whole, nextComment) => //sum each element in array
                whole += Environment.NewLine + nextComment), maxRange,
                pixels);
        }
    }
}
