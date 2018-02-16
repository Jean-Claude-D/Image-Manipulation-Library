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
            Pixel[,] pixels = { { new Pixel(111, 222, 222), new Pixel(123,134,115) },
                {new Pixel(134,111,123), new Pixel(222,123,167) } };

            Image result = new Image("ey", 255, pixels);
            result.ToGrey();

            Pixel[,] pixel = { { new Pixel(185), new Pixel(124) },
                {new Pixel(122), new Pixel(170) } };

            Image expected = new Image("ey", 255, pixel);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FlipHorizontalEvenTest()
        {
            Pixel[,] pixels = { { new Pixel(185), new Pixel(124) },
                                 { new Pixel(122), new Pixel(170) } };

            Image result = new Image("ey", 255, pixels);
            result.Flip(true);

            Pixel[,] pixel = { { new Pixel(124), new Pixel(185) },
                                 { new Pixel(170),  new Pixel(122) } };

            Image expected = new Image("ey", 255, pixel);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FlipHorizontalUnevenTest()
        {
            Pixel[,] pixels = { { new Pixel(185), new Pixel(124), new Pixel(170) },
                                 { new Pixel(122), new Pixel(170), new Pixel(124) } };

            Image result = new Image("ey", 255, pixels);
            result.Flip(true);

            Pixel[,] pixel = { { new Pixel(170), new Pixel(124), new Pixel(185) },
                                 { new Pixel(124), new Pixel(170),  new Pixel(122) } };

            Image expected = new Image("ey", 255, pixel);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FlipVerticalEvenTest()
        {
            Pixel[,] pixels = { { new Pixel(185), new Pixel(124) },
                                 { new Pixel(122), new Pixel(170) } };

            Image result = new Image("ey", 255, pixels);
            result.Flip(false);

            Pixel[,] pixel = { { new Pixel(122), new Pixel(170) },
                                 { new Pixel(185), new Pixel(124)} };
            
            Image expected = new Image("ey", 255, pixel);
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void FlipVerticalUnevenTest()
        {
            Pixel[,] pixels = { { new Pixel(124), new Pixel(185) },
                                 { new Pixel(170),  new Pixel(122) },
                                    { new Pixel(170),  new Pixel(122) }};

            Image result = new Image("ey", 255, pixels);
            result.Flip(false);

            Pixel[,] pixel = { { new Pixel(170),  new Pixel(122) },
                                 { new Pixel(170),  new Pixel(122) },
                                    { new Pixel(124), new Pixel(185) } };

            Image expected = new Image("ey", 255, pixel);
            Assert.AreEqual(expected, result);
        }


        [TestMethod]
        public void CropTestCorner()
        {
            Pixel[,] pixels = new Pixel[10, 10];
            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    pixels[i, j] = new Pixel(111);
                }
            }
            Image result = new Image("ey", 225, pixels);
            result.Crop(0,0,5,5);

            Pixel[,] pixel = new Pixel[5, 5];
            for (int i = 0; i < pixel.GetLength(0); i++)
            {
                for (int j = 0; j < pixel.GetLength(1); j++)
                {
                    pixel[i, j] = new Pixel(111);
                }
            }
            Image expected = new Image("ey", 225, pixel);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CropMiddleTest()
        {
            Pixel[,] pixel = { { new Pixel(124), new Pixel(185), new Pixel(170), new Pixel(122) },
                                { new Pixel(170), new Pixel(122), new Pixel(222), new Pixel(115) },
                                { new Pixel(123), new Pixel(167), new Pixel(170), new Pixel(122) },
                                { new Pixel(170), new Pixel(122), new Pixel(124), new Pixel(185) },
                                { new Pixel(124), new Pixel(185), new Pixel(170), new Pixel(122) },
                                { new Pixel(170), new Pixel(122), new Pixel(222), new Pixel(115) },
                                { new Pixel(123), new Pixel(167), new Pixel(170), new Pixel(122) },
                                { new Pixel(170), new Pixel(122), new Pixel(124), new Pixel(185) }};

            Image result = new Image("ey", 255, pixel);
            result.Crop(1, 1, 6, 3);

            Pixel[,] pixels = { 
                                { new Pixel(122), new Pixel(222) },
                                { new Pixel(167), new Pixel(170) },
                                { new Pixel(122), new Pixel(124) },
                                { new Pixel(185), new Pixel(170) },
                                { new Pixel(122), new Pixel(222) } };

            Image expected = new Image("ey", 255, pixels);
            Assert.AreEqual(expected, result);
        }
    }
}
