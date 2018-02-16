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
        /// <summary>
        /// Loads images from folder
        /// 
        /// Write a method LoadFolder which takes as input a String representing 
        /// a directory path and returns an Image[]. This method should look at 
        /// all the ﬁles with the extension pnm or pgm inside a directory 
        /// (Hint: use the library method Directory.GetFiles) and create an Image[]
        /// based on them, using the serializers you created above, choosing the 
        /// correct one to use based on the extension of the ﬁle. To do the actual
        /// reading, you should use the StreamReader class.
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public Image[] LoadFolder(String path)
        {
            String[] images = Directory.GetFiles(path);


            return null;
        }

        /// <summary>
        /// 
        /// Saves images to folder
        /// 
        /// Write a method SaveFolder which takes as input a String representing a 
        /// directory path as well as an Image[] and a String format.This method should
        /// export every Image in the array to the folder given using the speciﬁed 
        /// format(assumed to be “pnm” or “pgm”). To do the actual writing, you should
        /// use the StreamWriter class.
        ///
        /// </summary>
        /// <param name="images"></param>
        /// <param name="path"></param>
        /// <param name="format"></param>
        public void SaveFolder(Image[] images, String path, String format)
        {
            if (!format.Equals("pnm") || !format.Equals("pgm"))
                throw new ArgumentException("Invalid format");

            
        }

    }
}
