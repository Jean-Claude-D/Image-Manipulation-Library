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
        public void Parse_ValidFormatSpecifier()
        {
            PgmSerializer ser = new PgmSerializer();
            string imageStr = "P2" + Environment.NewLine +
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
