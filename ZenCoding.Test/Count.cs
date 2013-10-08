using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ZenCoding.Test
{
    [TestClass]
    public class Count
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void CountSimple1()
        {
            string result = _parser.Parse("p*3", ZenType.HTML);
            string expected = "<p></p>" +
                              "<p></p>" +
                              "<p></p>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void CountSimple2()
        {
            string result = _parser.Parse("div#name>p*2", ZenType.HTML);
            string expected = "<div id=\"name\">" +
                              "<p></p>" +
                              "<p></p>" +
                              "</div>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void CountComplex()
        {
            string result = _parser.Parse("div#page>div.logo+ul#navigation>li*5>a", ZenType.HTML);
            string expected = "<div id=\"page\">" +
                                "<div class=\"logo\"></div>" +
                                "<ul id=\"navigation\">" +
                                    "<li><a href=\"\"></a></li>" +
                                    "<li><a href=\"\"></a></li>" +
                                    "<li><a href=\"\"></a></li>" +
                                    "<li><a href=\"\"></a></li>" +
                                    "<li><a href=\"\"></a></li>" +
                                "</ul>" +
                                "</div>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void CountClass()
        {
            string result = _parser.Parse("ul#name>li.item*3", ZenType.HTML);
            string expected = "<ul id=\"name\">" +
                                "<li class=\"item\"></li>" +
                                "<li class=\"item\"></li>" +
                                "<li class=\"item\"></li>" +
                              "</ul>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void CountMultiNesting()
        {
            string result = _parser.Parse("table>tr>td#idc$*4>img[onclick]", ZenType.HTML);
            string expected = "<table><tr>" +
                                "<td id=\"idc1\"><img src=\"\" alt=\"\" onclick=\"\" /></td>" +
                                "<td id=\"idc2\"><img src=\"\" alt=\"\" onclick=\"\" /></td>" +
                                "<td id=\"idc3\"><img src=\"\" alt=\"\" onclick=\"\" /></td>" +
                                "<td id=\"idc4\"><img src=\"\" alt=\"\" onclick=\"\" /></td>" +
                              "</tr></table>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void CountH1to6()
        {
            string result = _parser.Parse("h$[title=item$]{Header $}*2", ZenType.HTML);
            string expected = "<h1 title=\"item1\">Header 1</h1>" + Environment.NewLine +
                              "<h2 title=\"item2\">Header 2</h2>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CountPadLeft()
        {
            string result = _parser.Parse("h1[title=item$]{Header $$$}*2", ZenType.HTML);
            string expected = "<h1 title=\"item1\">Header 001</h1>" + Environment.NewLine +
                              "<h1 title=\"item2\">Header 002</h1>";

            Assert.AreEqual(expected, result);
        }
    }
}
