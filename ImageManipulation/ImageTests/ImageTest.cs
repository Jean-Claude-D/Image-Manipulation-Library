using System;
using ImageManipulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageTests
{
    [TestClass]
    public class ImageTest
    {
        [TestMethod]
        public void ToGreyTest()
        {
            Pixel[,] pixels = new Pixel[10, 10];
            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    pixels[i, j] = new Pixel(i + j, i + j, i + j);
                }
            }
            Image image = new Image("ey", 225, pixels);
            image.ToGrey();
        }

        [TestMethod]
        public void FlipTest()
        {
            Pixel[,] pixels = new Pixel[10, 10];
            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    pixels[i, j] = new Pixel(i + j, i + j, i + j);
                }
            }
            Image image = new Image("ey", 225, pixels);
            image.Flip(true);
        }

        [TestMethod]
        public void CropTest()
        {
            Pixel[,] pixels = new Pixel[10, 10];
            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    pixels[i, j] = new Pixel(i + j, i + j, i + j);
                }
            }
            Image image = new Image("ey", 225, pixels);
            image.Crop(0,0,5,5);
        }
    }
}
