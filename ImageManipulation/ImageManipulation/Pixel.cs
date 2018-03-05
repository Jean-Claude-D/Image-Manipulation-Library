using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageManipulation
{
    /// @DesJC
    /// <summary>
    /// Represents a single pixel in an image
    /// </summary>
    public class Pixel
    {
        /// <summary>
        /// Range to check valid pixel intensities
        /// </summary>
        private const int _expectedInclusiveMax = 255;
        private const int _expectedInclusiveMin = 0;

        /// <summary>
        /// Red color intensity of this Pixel object
        /// </summary>
        public int Red
        {
            get;
            private set;
        }

        /// <summary>
        /// Green color intensity of this Pixel object
        /// </summary>
        public int Green
        {
            get;
            private set;
        }

        /// <summary>
        /// Blue color intensity of this Pixel object
        /// </summary>
        public int Blue
        {
            get;
            private set;
        }

        /// <summary>
        /// Constructs a new Pixel object with color intensities
        /// </summary>
        /// <param name="red">the red color intensity</param>
        /// <param name="green">the green color intensity</param>
        /// <param name="blue">the blue color intensity</param>
        public Pixel(int red, int green, int blue)
        {
            if(!isInRange(red))
            {
                throw new ArgumentException(
                    getIntensityExceptionMessage("red", red));
            }
            else if(!isInRange(green))
            {
                throw new ArgumentException(
                    getIntensityExceptionMessage("green", green));
            }
            else if(!isInRange(blue))
            {
                throw new ArgumentException(
                    getIntensityExceptionMessage("blue", blue));
            }

            this.Red = red;
            this.Green = green;
            this.Blue = blue;
        }

        /// <summary>
        /// Constructs a new Pixel object with identical intensities
        /// for each color. This results in a greyscale pixel
        /// </summary>
        /// <param name="intensity">the intensity for each color</param>
        public Pixel(int intensity) : this(intensity, intensity, intensity)
        { }

        /// <summary>
        /// Gets a well formatted exception message for
        /// an invalid pixel color intensity
        /// </summary>
        /// <param name="colorName">the name of the color with an invalid intensity</param>
        /// <param name="pixelValue">the intensity</param>
        /// <returns>A string exception message</returns>
        private string getIntensityExceptionMessage(string colorName, int pixelValue)
        {
            return string.Format("Illegal pixel intensity," +
                "{0} ({1}) must be inclusively between {2} and {3}",
                colorName, pixelValue, _expectedInclusiveMin,
                _expectedInclusiveMax);
        }

        /// <summary>
        /// Checks whether the given pixel intensity is
        /// within the range specified by Pixel class
        /// </summary>
        /// <param name="intensity">the tested pixel intensity</param>
        /// <returns>true if the given pixel intensity is
        /// within the range, false otherwise</returns>
        private bool isInRange(int intensity)
        {
            return intensity >= _expectedInclusiveMin
                && intensity <= _expectedInclusiveMax;
        }

        /// <summary>
        /// Returns the grey intensity for
        /// this Pixel object
        /// </summary>
        /// <returns>An average of this Pixel's
        /// each color's intensity</returns>
        public int Grey()
        {
            return (this.Red + this.Green + this.Blue) / 3;
        }

        /// <summary>
        /// Checks whether this Pixel is equal to obj or not
        /// </summary>
        /// <param name="obj">the Object to compare with</param>
        /// <returns>true if this Pixel is equal to obj,
        /// false otherwise</returns>
        public override bool Equals(object obj)
        {
            if(Object.ReferenceEquals(obj, null) ||
                !(obj is Pixel))
            {
                return false;
            }
            else
            {
                Pixel pixelObj = obj as Pixel;
                return (this.Red == pixelObj.Red) &&
                    (this.Green == pixelObj.Green) &&
                    (this.Blue == pixelObj.Blue);
            }
        }
    }
}
