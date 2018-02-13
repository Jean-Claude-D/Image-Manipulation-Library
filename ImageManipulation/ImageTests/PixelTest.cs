﻿using System;
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
            Pixel pixel;
            Action action = delegate { pixel = new Pixel(-256, 42, 42); };

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void PixelColor_AllValidValue()
        {
            Pixel pixel;

            pixel = new Pixel(42, 42, 42);

            Assert.IsNotNull(pixel);
        }

        [TestMethod]
        public void PixelGrey_InvalidValue()
        {
            Pixel pixel;
            Action action = delegate { pixel = new Pixel(256); };

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void PixelGrey_NegativeValue()
        {
            Pixel pixel;
            Action action = delegate { pixel = new Pixel(-256); };

            Assert.ThrowsException<ArgumentException>(action);
        }

        [TestMethod]
        public void PixelGrey_ValidValue()
        {
            Pixel pixel;

            pixel = new Pixel(42);

            Assert.IsNotNull(pixel);
        }

        [TestMethod]
        public void Grey_AverageGivesIntegerValue()
        {
            int expected = 25;
            int result;
            Pixel pixel = new Pixel(expected + 10, expected - 10, expected);

            result = pixel.Grey();

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Grey_AverageGivesFloatingValue()
        {
            int expected = 25;
            int result;
            Pixel pixel = new Pixel(expected + 10, expected - 10, expected + 1);

            result = pixel.Grey();

            Assert.AreEqual(expected, result);
        }
    }
}
