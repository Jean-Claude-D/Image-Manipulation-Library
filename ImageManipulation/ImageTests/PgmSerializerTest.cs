using System;
using ImageManipulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ImageTests
{
    [TestClass]
    public class PgmSerializerTest
    {
        private static PgmSerializer ser = new PgmSerializer();
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
            string imageStr = "P2" + "P2" + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_SpacedFormatSpecifier()
        {
            string imageStr = "  P2 " + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_WrongLineFormatSpecifier()
        {
            string imageStr = Environment.NewLine + "P2" + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_InCommentFormatSpecifier()
        {
            string imageStr = "# This is a stub comment" + Environment.NewLine +
                "P2" + Environment.NewLine +
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
            string imageStr = "P2 #This is my single line comment" +
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
                "0 0" + Environment.NewLine + "0 0 ";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_OnlyWidthSpecification()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "2" + Environment.NewLine +
                "255" + Environment.NewLine + "0 0" +
                Environment.NewLine + "0 0 ";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_MultipleSizeSpecification()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "2 2 2" + Environment.NewLine +
                "255" + Environment.NewLine + "0 0" +
                Environment.NewLine + "0 0 ";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_InvalidSizeSpecification()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + getValidPixelData() +
                Environment.NewLine + "0 0";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_MaxRangeTooSmall()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "2 2" + Environment.NewLine +
                "10" + Environment.NewLine + "0 12" + Environment.NewLine +
                "0 0";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_NoMaxRange()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "2 2" + Environment.NewLine +
                "0 12" + Environment.NewLine + "0 0";
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_TwoMaxRanges()
        {
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + "2 2" + Environment.NewLine +
                "10 10" + Environment.NewLine + "0 12" + Environment.NewLine +
                "0 0";
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
                "0 0 0 0 0" + Environment.NewLine +
                "0" + Environment.NewLine +
                "0 0 0 0" + Environment.NewLine +
                "0 0" + Environment.NewLine +
                "0 0 0" + Environment.NewLine +
                "0";
            Image result = ser.Parse(imageStr);

            Assert.IsNotNull(result);
        }

        private string getValidFormatSpecifier()
        {
            return "P2" + Environment.NewLine;
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
                "0 0" + Environment.NewLine +
                "0 0";
        }
    }
}
