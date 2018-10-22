using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZenCoding.Test
{
    [TestClass]
    public class Parser
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void TestID()
        {
            string result = _parser.Parse("div#name", ZenType.HTML);
            Assert.AreEqual("<div id=\"name\"></div>", result);
        }

        [TestMethod]
        public void TestClass()
        {
            string result = _parser.Parse("div.name", ZenType.HTML);
            Assert.AreEqual("<div class=\"name\"></div>", result);
        }

        [TestMethod]
        public void TestMultipleClass()
        {
            string result = _parser.Parse("div.name.test", ZenType.HTML);
            Assert.AreEqual("<div class=\"name test\"></div>", result);
        }

        [TestMethod]
        public void TestMultipleClassAndIDs()
        {
            string result = _parser.Parse("div#name.one.two", ZenType.HTML);
            Assert.AreEqual("<div id=\"name\" class=\"one two\"></div>", result);
        }

        [TestMethod]
        public void TestClassId()
        {
            string result = _parser.Parse("form#search.wide", ZenType.HTML);
            string expected = "<form id=\"search\" class=\"wide\"></form>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestOneChild()
        {
            string result = _parser.Parse("div>span", ZenType.HTML);
            Assert.AreEqual("<div><span></span></div>", result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TestChildIdClass()
        {
            string result = _parser.Parse("ul#name>li.item", ZenType.HTML);
            string expected = "<ul id=\"name\">" +
                              "<li class=\"item\"></li>" +
                              "</ul>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TestSiblingSimple()
        {
            string result = _parser.Parse("p+p", ZenType.HTML);
            string expected = "<p></p>" +
                              "<p></p>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TestSiblingComplex()
        {
            string result = _parser.Parse("div#name>p.one+p.two", ZenType.HTML);
            string expected = "<div id=\"name\">" +
                              "<p class=\"one\"></p>" +
                              "<p class=\"two\"></p>" +
                              "</div>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TestHtml5AllowedTags()
        {
            string result = _parser.Parse("main#mainArea>section.search+section.results+h1-alternative[--data-breach=somevalue]", ZenType.HTML);
            string expected = "<main id=\"mainArea\">" +
                              "<section class=\"search\"></section>" +
                              "<section class=\"results\"></section>" +
                              "<h1-alternative --data-breach=\"somevalue\"></h1-alternative>" +
                              "</main>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }
    }
}
