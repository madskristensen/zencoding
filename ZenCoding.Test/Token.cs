using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace ZenCoding.Test
{
    [TestClass]
    public class Token
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void TokenClassSimple()
        {
            string result = _parser.Parse("p.name-$*3", ZenType.HTML);
            string expected = "<p class=\"name-1\"></p>" +
                              "<p class=\"name-2\"></p>" +
                              "<p class=\"name-3\"></p>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TokenClassComplex()
        {
            string result = _parser.Parse("select>option.item-$*3", ZenType.HTML);
            string expected = "<select>" +
                              "<option class=\"item-1\"></option>" +
                              "<option class=\"item-2\"></option>" +
                              "<option class=\"item-3\"></option>" +
                              "</select>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TokenIdSimple()
        {
            string result = _parser.Parse("p#name-$*3", ZenType.HTML);
            string expected = "<p id=\"name-1\"></p>" +
                              "<p id=\"name-2\"></p>" +
                              "<p id=\"name-3\"></p>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TokenIdComplex()
        {
            string result = _parser.Parse("select>option#item-$*3", ZenType.HTML);
            string expected = "<select>" +
                              "<option id=\"item-1\"></option>" +
                              "<option id=\"item-2\"></option>" +
                              "<option id=\"item-3\"></option>" +
                              "</select>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }
    }
}
