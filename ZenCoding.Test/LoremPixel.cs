using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ZenCoding.Test
{
    [TestClass]
    public class LoremPixel
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void LoremPixel1()
        {
            string result = _parser.Parse("pix", ZenType.HTML);
            string expected = "<img src=\"http://lorempixel.com/30/30/\" alt=\"\" />";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LoremPixel2()
        {
            string result = _parser.Parse("pix-g", ZenType.HTML);
            string expected = "<img src=\"http://lorempixel.com/g/30/30/\" alt=\"\" />";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LoremPixel3()
        {
            string result = _parser.Parse("pix-120x1879-sports-SomeRandomText", ZenType.HTML);
            string expectedStart = "http://lorempixel.com/120/1879/sports/";
            string expectedEnd = "/SomeRandomText/";

            StringAssert.Contains(result, expectedStart);
            StringAssert.Contains(result, expectedEnd);
        }

        [TestMethod]
        public void LoremPixel4()
        {
            string result = _parser.Parse("pix-g-999x1920-animals-SomeRandomText", ZenType.HTML);
            
            string expectedStart = "http://lorempixel.com/g/999/1920/animals/";
            string expectedEnd = "/SomeRandomText/";

            StringAssert.Contains(result, expectedStart);
            StringAssert.Contains(result, expectedEnd);
        }

        [TestMethod]
        public void LoremPixel5()
        {
            string result = _parser.Parse("pix-20000x3599-SomeRandomText", ZenType.HTML);
            string expected = "<img src=\"http://lorempixel.com/790/1678/\" alt=\"\" />"; // the allowed bound is 0-1920
            
            Assert.AreEqual(expected, result);
        }
    }
}
