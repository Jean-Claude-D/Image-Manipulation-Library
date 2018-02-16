using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageManipulation;
using System.IO;

namespace ImageTests
{
    [TestClass]
    public class PnmSerializerTest
    {
        private static PnmSerializer ser = new PnmSerializer();

        [TestMethod]
        public void Serialize_EmptyComment()
        {
            Image original = new Image("", 255, getValidPixelArray());
            Image result;

            result = ser.Parse(ser.Serialize(original));

            pringPixels(original);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            pringPixels(result);

            Assert.AreEqual(original.Metadata, result.Metadata);
            Assert.AreEqual(original.MaxRange, result.MaxRange);
            for (int i = 0; i < original.GetLength(0); i++)
            {
                for (int j = 0; j < original.GetLength(1); j++)
                {
                    Assert.AreEqual(original[i, j], result[i, j]);
                }
            }
        }

        [TestMethod]
        public void Serialize_WithComment()
        {
            string comment = "This is my comment" + Environment.NewLine +
                "It goes in an Image file" + Environment.NewLine +
                "And is multi-line" + Environment.NewLine +
                "Four lines to be exact";
            Image original = new Image(comment, 255, getValidPixelArray());
            Image result;

            result = ser.Parse(ser.Serialize(original));

            pringPixels(original);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            pringPixels(result);

            Assert.AreEqual(original.Metadata, result.Metadata);
            Assert.AreEqual(original.MaxRange, result.MaxRange);
            for (int i = 0; i < original.GetLength(0); i++)
            {
                for (int j = 0; j < original.GetLength(1); j++)
                {
                    Assert.AreEqual(original[i, j], result[i, j]);
                }
            }
        }

        [TestMethod]
        public void Serialize_NullImage()
        {
            Image img = null;
            string result = null;
            Action serializeNullImage = delegate { result = ser.Serialize(img); };

            Assert.ThrowsException<ArgumentException>(serializeNullImage);
        }

        private Pixel[,] getValidPixelArray()
        {
            Pixel[,] pixels = new Pixel[9, 9];
            Random rand = new Random();

            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    pixels[i, j] = new Pixel(rand.Next(0, 256),
                        rand.Next(0, 256), rand.Next(0, 256));
                }
            }

            return pixels;
        }

        private void pringPixels(Image img)
        {
            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    Console.Write(img[i, j].Red + "\t" +
                        img[i, j].Green + "\t" + img[i, j].Blue + "\t");
                }
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void Parse_ValidParameters()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + getValidPixelData();
            Image result = null;

            result = ser.Parse(imageStr);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Parse_InvalidFormatSpecifier()
        {
            string imageStr = "P0" + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_NoFormatSpecifier()
        {
            string imageStr = getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_MultipleFormatSpecifier()
        {
            string imageStr = "P3" + "P3" + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_SpacedFormatSpecifier()
        {
            string imageStr = "  P3 " + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_WrongLineFormatSpecifier()
        {
            string imageStr = Environment.NewLine + "P3" + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_InCommentFormatSpecifier()
        {
            string imageStr = "# This is a stub comment" + Environment.NewLine +
                "P3" + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_NoComment()
        {
            string imageStr =
                getValidFormatSpecifier() + getValidPixelData();
            Image result = null;

            result = ser.Parse(imageStr);

            Assert.AreEqual(result.Metadata, string.Empty);
        }

        [TestMethod]
        public void Parse_OneCommentLine()
        {
            string comment = "This is my single line comment";
            string imageStr = getValidFormatSpecifier() + '#' +
                comment + Environment.NewLine + getValidPixelData();
            Image result = null;

            result = ser.Parse(imageStr);

            Assert.AreEqual(result.Metadata, comment);
        }

        [TestMethod]
        public void Parse_UnCommentedCommentLine()
        {
            string comment = "This is my single line comment";
            string imageStr = getValidFormatSpecifier() +
                comment + Environment.NewLine + getValidPixelData();
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_CommentOnSameLineAsFormatSpecifier()
        {
            string imageStr = "P3 #This is my single line comment" +
                Environment.NewLine + getValidPixelData();
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_BlankLineBetweenFormatSpecifierAndComment()
        {
            string imageStr = getValidFormatSpecifier() +
                Environment.NewLine + getValidMetadata() +
                getValidPixelData();
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_BlankLineBetweenCommentAndWidthHeight()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + Environment.NewLine +
                getValidPixelData();
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_NoSizeSpecification()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "255" + Environment.NewLine +
                "0 1 1 0 1 1" + Environment.NewLine + "0 1 1 0 1 1";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_OnlyWidthSpecification()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "2" + Environment.NewLine +
                "255" + Environment.NewLine + "0 1 1 0 1 1" +
                Environment.NewLine + "0 1 1 0 1 1";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_MultipleSizeSpecification()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "2 2 2" + Environment.NewLine +
                "255" + Environment.NewLine + "0 1 2 0 1 2" +
                Environment.NewLine + "0 1 2 0 1 2";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_InvalidSizeSpecification()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + getValidPixelData() +
                Environment.NewLine + "0 1 2 0 1 2";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_MaxRangeTooSmall()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "2 2" + Environment.NewLine +
                "10" + Environment.NewLine + "0 15 11 12 1 2" + Environment.NewLine +
                "0 0 0 0 0 0";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_NoMaxRange()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "2 2" + Environment.NewLine +
                "0 1 2 1 2 1" + Environment.NewLine + "0 1 2 0 1 2";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_TwoMaxRanges()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "2 2" + Environment.NewLine +
                "10 10" + Environment.NewLine + "0 1 2 1 2 1" + Environment.NewLine +
                "0 1 2 0 1 2";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_WeirdPixelFormat()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "4 4" + Environment.NewLine +
                "10" + Environment.NewLine +
                "0 1 2 0 1 2 0 1 2 0 1 2 0 1 2" + Environment.NewLine +
                "0 1 2" + Environment.NewLine +
                "0 1 2 0 1 2 0 1 2 0 1 2" + Environment.NewLine +
                "0 1 2 0 1 2" + Environment.NewLine +
                "0 1 2 0 1 2 0 1 2" + Environment.NewLine +
                "0 1 2";
            Image result = ser.Parse(imageStr);

            Assert.IsNotNull(result);
        }

        private string getValidFormatSpecifier()
        {
            return "P3" + Environment.NewLine;
        }

        private string getValidMetadata()
        {
            return "# This is my favorite image" + Environment.NewLine +
                "# It is used for testing purposes" + Environment.NewLine +
                "# It is in the valid format" + Environment.NewLine;
        }

        private string getValidPixelData()
        {
            return "2 2" + Environment.NewLine +
                "255" + Environment.NewLine +
                "0 1 2 0 1 2" + Environment.NewLine +
                "0 1 2 0 1 2";
        }
    }
}
