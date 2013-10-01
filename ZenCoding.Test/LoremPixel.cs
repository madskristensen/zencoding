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
            string expected = "http://lorempixel.com/30/30";
            int index = result.LastIndexOf("/");
            result = (new string[] { result.Substring(0, index), result.Substring(index + 1) })[0];

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LoremPixel2()
        {
            string result = _parser.Parse("pix-g", ZenType.HTML);
            string expected = "http://lorempixel.com/g/30/30";
            int index = result.LastIndexOf("/");
            result = (new string[] { result.Substring(0, index), result.Substring(index + 1) })[0];

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LoremPixel3()
        {
            string result = _parser.Parse("pix-120x1879-sports-SomeRandomText", ZenType.HTML);
            string expected = "http://lorempixel.com/120/1879/sports/SomeRandomText";
            int index = result.LastIndexOf("/");
            result = (new string[] { result.Substring(0, index), result.Substring(index + 1) })[0];

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LoremPixel4()
        {
            string result = _parser.Parse("pix-g-999x1920-animals-SomeRandomText", ZenType.HTML);
            string expected = "http://lorempixel.com/g/999/1920/animals/SomeRandomText";
            int index = result.LastIndexOf("/");
            result = (new string[] { result.Substring(0, index), result.Substring(index + 1) })[0];

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void LoremPixel5()
        {
            string result = _parser.Parse("pix-20000x3599-SomeRandomText", ZenType.HTML);
            string expected = "http://lorempixel.com/g/800/1679/SomeRandomText"; // the allowed bound is 0-1920
            int index = result.LastIndexOf("/");
            result = (new string[] { result.Substring(0, index), result.Substring(index + 1) })[0];

            Assert.AreEqual(expected, result);
        }
    }
}
