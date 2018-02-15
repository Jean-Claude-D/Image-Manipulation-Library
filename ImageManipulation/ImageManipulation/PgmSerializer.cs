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

        /// <summary>
        /// Converts an Image object to a string
        /// so it can be written to a file
        /// </summary>
        /// <param name="img">the Image to be converted</param>
        /// <returns>the serialized Image</returns>
        public string Serialize(Image img)
        {
            if(Object.ReferenceEquals(img, null))
            {
                throw new ArgumentException
                    ("img cannot be null");
            }

            StringBuilder imgStr = new StringBuilder
                (formatSpec + Environment.NewLine);

            //splitting each line of the comment
            string[] comments =
                img.Metadata.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);
            foreach(string comment in comments)
            {
                imgStr.Append(commentTag +
                    comment + Environment.NewLine);
            }

            int width = img.GetLength(0);
            int height = img.GetLength(1);

            imgStr.Append(width + " " + height
                + Environment.NewLine);

            imgStr.Append
                (img.MaxRange + Environment.NewLine);

            //Append the pixel data
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    imgStr.Append(img[j, i].Grey());

                    if(j + 1 < width)
                    {
                        imgStr.Append(' ');
                    }
                }
                if (i + 1 < height)
                {
                    imgStr.Append(Environment.NewLine);
                }
            }

            return imgStr.ToString();
        }

        /// <summary>
        /// Converts a string (usually from file) to an Image object
        /// </summary>
        /// <param name="imgData">the image data in string format</param>
        /// <returns>the deserialized Image</returns>
        public Image Parse(string imgData)
        {
            string formatRgx = '^' + formatSpec + '$';
            string commentRgx = '^' + commentTag + ".+$";
            string sizeRgx = @"^\d+ \d+$";
            string rangeRgx = @"^\d+$";
            string pixelRgx = @"^(:?\d+ ?)+$";

            //each line in the "file"
            string[] lines = imgData.Split
                (new string[] { Environment.NewLine },
                StringSplitOptions.None);

            //first line should be format specifier
            checkFormat(lines[0], true, 0, formatRgx);
            //second line should be either comment or size specifier
            checkOneFormat(lines[1], true, 1, commentRgx, sizeRgx);

            string[] metadata = lines.Skip(1) //skip format specifier
                .TakeWhile(line => checkFormat(line, false, -1, commentRgx))
                //take only metadata lines, line numbering is irrelevant here
                .Select(comment => comment = comment.Substring(1)) //remove #
                .ToArray();

            //line after comments should be size specifier
            checkFormat(lines[metadata.Length + 1], true,
                metadata.Length + 1, sizeRgx);

            int[] size = lines[metadata.Length + 1].Split(' ')
                .Select(num => int.Parse(num))
                .ToArray();

            //line after size specifier should be range
            checkFormat(lines[metadata.Length + 2], true,
                metadata.Length + 2, rangeRgx);

            int maxRange = int.Parse(lines[metadata.Length + 2]);

            //the rest of the string should be filled with pixels
            for(int i = metadata.Length + 3; i < lines.Length; i++)
            {
                checkFormat(lines[i], true, i, pixelRgx);
            }

            int[] pixelsData = lines.Skip(metadata.Length + 3) //skip metadata and stuff
                .Select(line => line.Split(' ').Select(num => int.Parse(num)))
                //parse each collection of collection
                .SelectMany(list => list.ToArray()).ToArray();
                //flatten the array
            
            //verify maxRange of each pixel
            for(int i = 0; i < pixelsData.Length; i++)
            {
                if(pixelsData[i] > maxRange)
                {
                    throw new InvalidDataException
                        ("Pixel " + i + " expected at most : " +
                        maxRange + Environment.NewLine + "Actual : " +
                        pixelsData[i]);
                }
            }

            //checking amount of pixel corresponds to specified size
            if(pixelsData.Length != size[0] * size[1])
            {
                throw new InvalidDataException("Expected size : " +
                    size[0] * size[1] + ", Actual size : "
                    + pixelsData.Length);
            }

            //filling the resulting Pixel rectangular array
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


        private bool checkFormat(string actualStr, bool throws , int lineNum, string expectedRgx)
        {
            if (!Regex.IsMatch(actualStr, expectedRgx))
            {
                if(throws)
                {
                    throw new InvalidDataException
                    ("Line " + lineNum + " expected : " + expectedRgx + Environment.NewLine +
                    "Actual : " + actualStr);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool checkOneFormat(string actualStr, bool throws, int lineNum, params string[] expectedRgx)
        {
            string expectedAllStr = "";

            foreach (string rgx in expectedRgx)
            {
                if (Regex.IsMatch(actualStr, rgx))
                {
                    return true;
                }

                expectedAllStr += rgx + Environment.NewLine;
            }

            if(throws)
            {
                throw new InvalidDataException
                        ("Line " + lineNum + " expected : " + expectedAllStr +
                        "Actual : " + actualStr);
            }
            else
            {
                return false;
            }
        }
    }
}
