using System;
using ImageManipulation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ImageTests
{
    [TestClass]
    public class PgmSerializerTest
    {
        [TestMethod]
        public void Parse_ValidParameters()
        {
            PgmSerializer ser = new PgmSerializer();
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + getValidPixelData();
            Image result = null;

            result = ser.Parse(imageStr);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Parse_InvalidFormatSpecifier()
        {
            PgmSerializer ser = new PgmSerializer();
            string imageStr = "P0" + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_NoFormatSpecifier()
        {
            PgmSerializer ser = new PgmSerializer();
            string imageStr = getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_MultipleFormatSpecifier()
        {
            PgmSerializer ser = new PgmSerializer();
            string imageStr = "P2" + "P2" + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_SpacedFormatSpecifier()
        {
            PgmSerializer ser = new PgmSerializer();
            string imageStr = "  P2 " + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_WrongLineFormatSpecifier()
        {
            PgmSerializer ser = new PgmSerializer();
            string imageStr = Environment.NewLine + "P2" + Environment.NewLine +
                getValidMetadata() + getValidPixelData();
            Image result = null;
            Action parsing = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(parsing);
        }

        [TestMethod]
        public void Parse_InCommentFormatSpecifier()
        {
            PgmSerializer ser = new PgmSerializer();
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
            PgmSerializer ser = new PgmSerializer();
            string imageStr =
                getValidFormatSpecifier() + getValidPixelData();
            Image result = null;

            result = ser.Parse(imageStr);

            Assert.AreEqual(result.Metadata, string.Empty);
        }

        [TestMethod]
        public void Parse_OneCommentLine()
        {
            PgmSerializer ser = new PgmSerializer();
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
            PgmSerializer ser = new PgmSerializer();
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
            PgmSerializer ser = new PgmSerializer();
            string imageStr = "P2 #This is my single line comment" +
                Environment.NewLine + getValidPixelData();
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
        }

        [TestMethod]
        public void Parse_BlankLineBetweenFormatSpecifierAndComment()
        {
            PgmSerializer ser = new PgmSerializer();
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
            PgmSerializer ser = new PgmSerializer();
            string imageStr = getValidFormatSpecifier() +
                getValidMetadata() + Environment.NewLine +
                getValidPixelData();
            Image result = null;
            Action createInvalidImage = delegate { result = ser.Parse(imageStr); };

            Assert.ThrowsException<InvalidDataException>(createInvalidImage);
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
                "0 0" + Environment.NewLine;
        }
    }
}
