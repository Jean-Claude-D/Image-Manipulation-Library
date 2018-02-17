using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    public class ImageUtilities
    {
        IImageSerialization pnm = new PnmSerializer();
        IImageSerialization pgm = new PgmSerializer();

        /// <summary>
        /// Loads images from folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Image[] LoadFolder(String path)
        {
            if (path == null)
                throw new ArgumentException("Path cannot be null");

            if (!Directory.Exists(path))
                throw new ArgumentException("Path was not found");

            List<Image> imageList = new List<Image>();
            String[] files = Directory.GetFiles(path);

            foreach (String file in files)
            {
                using (StreamReader str = new StreamReader(file))
                {
                    if (Path.GetExtension(file).Equals(".pnm"))
                        imageList.Add(pnm.Parse(str.ReadLine()));

                    if (Path.GetExtension(file).Equals(".pgm"))
                        imageList.Add(pgm.Parse(str.ReadLine()));
                }
            }
            return imageList.ToArray();
        }

        /// <summary>
        /// Saves images to folder
        /// </summary>
        /// <param name="images"></param>
        /// <param name="path"></param>
        /// <param name="format"></param>
        public void SaveFolder(Image[] images, String path, String format)
        {
            if (!format.Equals("pnm") || !format.Equals("pgm"))
                throw new ArgumentException("Invalid format");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            IImageSerialization sFormat = format.Equals("pnm") ? pnm : pgm;
            this.SaveFormat(images, path, sFormat, format);
        }

        private void SaveFormat(Image[] images, String path, IImageSerialization format, String ext)
        {
            int filenum = 1;
            foreach (Image img in images)
            {
                using (StreamWriter str = new StreamWriter(new FileStream(path + "image" + filenum + "." + ext, FileMode.Create, FileAccess.Write)))

                {
                    String data = format.Serialize(img);
                    str.WriteLine(data);
                    filenum++;
                }
            } 
        }
    }
}

