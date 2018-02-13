using System;
using ImageManipulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageTests
{
    [TestClass]
    public class PixelTest
    {
        [TestMethod]
        public void PixelColor_OneInvalidValue()
        {
            Pixel pixel;
            Action action = delegate { pixel = new Pixel(256, 42, 42); };

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void PixelColor_OneNegativeValue()
        {
        }

        [TestMethod]
        public void PixelColor_AllValidValue()
        {
        }

        [TestMethod]
        public void PixelGrey_InvalidValue()
        {
        }

        [TestMethod]
        public void PixelGrey_NegativeValue()
        {
        }

        [TestMethod]
        public void PixelGrey_ValidValue()
        {
        }

        [TestMethod]
        public void Grey_AverageGivesIntegerValue()
        {
        }

        [TestMethod]
        public void Grey_AverageGivesFloatingValue()
        {
        }
    }
}
