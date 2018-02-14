using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ImageManipulation
{
    public class PgmSerializer : IImageSerialization
    {
        private string formatSpec = "P2";
        private string commentTag = "#";

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
            string formatRgx = '^' + formatSpec + '$';
            string commentRgx = "^" + commentTag + ".+$";
            string sizeRgx = @"^\d+\e\d+$";
            string rangeRgx = @"^\d+$";
            string pixelRgx = @"^{\d+\e?}+$";

            //each line in the "file"
            string[] lines = imgData.Split
                (new string[] { Environment.NewLine },
                StringSplitOptions.None);

            checkFormat(lines[0], formatRgx);

            if (! Regex.IsMatch(lines[0], formatRgx))
            {
                throw new InvalidDataException
                    ("Expected : " + formatRgx + Environment.NewLine + "" +
                    "Actual : " + );
            }
            // second line should either contain a comment or the image size
            else if(!Regex.IsMatch(lines[1], commentRgx) &&
                !Regex.IsMatch(lines[1], sizeRgx))
            {
                throw new InvalidDataException("Expected : \'" +
                    commentTag + "\' or WIDTH HEIGHT Actual : " + lines[0]);
            }

            string[] metadata = lines.Skip(1) //skip format specifier
                .TakeWhile(line => line.ElementAt(0).ToString().Equals(commentTag))
                .Select(comment => comment = comment.Substring(1)) //skip #
                .ToArray();

            if (lines[metadata.Length + 1].Equals(string.Empty))
            {
                throw new InvalidDataException
                    ("Expected : WIDTH HEIGHT Actual : "+ lines[0]);
            }

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

            return new Image(string.Join(Environment.NewLine, metadata),
                maxRange, pixels);
        }


        private void checkFormat(string actualStr, params string[] expectedRgx)
        {
            foreach(string rgx in expectedRgx)
            {
                if (!Regex.IsMatch(actualStr, rgx))
                {
                    throw new InvalidDataException
                        ("Expected : " + expectedRgx + Environment.NewLine +
                        "Actual : " + actualStr);
                }
            }
        }
    }
}
