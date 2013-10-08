using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ZenCoding.Test
{
    [TestClass]
    public class Grouping
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void SimpleGroupingExample()
        {
            string result = _parser.Parse("div>(header>div)+section", ZenType.HTML);
            string expected = "<div>" +
                                "<header>" +
                                    "<div></div>" +
                                "</header>" +
                                "<section></section>" +
                              "</div>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));

        }

        [TestMethod]
        public void SubGroupingExample()
        {
            string result = _parser.Parse("div>(header>(div>span))+section", ZenType.HTML);
            string expected = "<div>" +
                                "<header>" +
                                    "<div>" +
                                        "<span></span>" +
                                    "</div>" +
                                "</header>" +
                                "<section></section>" +
                              "</div>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));

        }

        [TestMethod]
        public void MultipleGroupingExample()
        {
            string result = _parser.Parse("div>(header>div)+section>(ul>li*2>a)+footer>(div>span)", ZenType.HTML);
            string expected = "<div>" +
                                "<header>" +
                                    "<div></div>" +
                                "</header>" +
                                "<section>" +
                                    "<ul>" +
                                        "<li><a href=\"\"></a></li>" +
                                        "<li><a href=\"\"></a></li>" +
                                    "</ul>" +
                                    "<footer>" +
                                        "<div>" +
                                            "<span></span>" +
                                        "</div>" +
                                    "</footer>" +
                                "</section>" +
                              "</div>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }
    }
}
