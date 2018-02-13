using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    class PgmSerializer : IImageSerialization
    {
        private string formatSpec = "P2";

        public string Serialize(Image img)
        {
            return "";
        }

        public Image Parse(string imgData)
        {
            string[] lines = imgData.Split
                (new string[] { System.Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            if(!lines[0].ToLower().Equals(formatSpec.ToLower()))
            {
                throw new InvalidDataException("Expected format : " +
                    formatSpec + ", Actual format : " + lines[0]);
            }

            string[] metadata = lines.Skip(1) //skip format specifier
                .TakeWhile(line => line.ElementAt(0).Equals('#'))
                .ToArray();

            int[] size = lines[metadata.Length + 1].Split(' ')
                .Select(num => int.Parse(num))
                .ToArray();

            int maxRange = int.Parse(lines[metadata.Length + 2]);

            return null;
        }
    }
}
