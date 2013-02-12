
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace ZenCoding.Test
{
    [TestClass]
    public class Attributes
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void Attributes1()
        {
            string result = _parser.Parse("p[title]", ZenType.HTML);
            string expected = "<p title=\"\"></p>";

            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void Attributes2()
        {
            string result = _parser.Parse("td[colspan title]", ZenType.HTML);
            string expected = "<td colspan=\"\" title=\"\"></td>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Attributes3()
        {
            string result = _parser.Parse("td[colspan=2]", ZenType.HTML);
            string expected = "<td colspan=\"2\"></td>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Attributes4()
        {
            string result = _parser.Parse("span[title=\"Hello\" rel]", ZenType.HTML);
            string expected = "<span title=\"Hello\" rel=\"\"></span>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Attributes5()
        {
            string result = _parser.Parse("a[title=\"Hello world\"]", ZenType.HTML);
            string expected = "<a href=\"\" title=\"Hello world\"></a>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Attributes6()
        {
            string result = _parser.Parse("a[title=\"Hello world\" rel]", ZenType.HTML);
            string expected = "<a href=\"\" title=\"Hello world\" rel=\"\"></a>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AttributesAfterId()
        {
            string result = _parser.Parse("div#main[prop=val]", ZenType.HTML);
            string expected = "<div id=\"main\" prop=\"val\"></div>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AttributesMultiple()
        {
            string result = _parser.Parse("a[prop=val][href=ost] ", ZenType.HTML);
            string expected = "<a href=\"ost\" prop=\"val\"></a>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void AttributesHrefDefault()
        {
            string result = _parser.Parse("a[href=#] ", ZenType.HTML);
            string expected = "<a href=\"#\"></a>";

            Assert.AreEqual(expected, result);
        }
}
}