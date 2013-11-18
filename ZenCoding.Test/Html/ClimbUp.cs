using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZenCoding.Test
{
    [TestClass]
    public class ClimbUp
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void ClimbUpSimple()
        {
            string result = _parser.Parse("div+div>p>span+em^bq", ZenType.HTML);
            string expected = "<div></div>" +
                                "<div>" +
                                  "<p><span></span><em></em></p>" +
                                  "<blockquote></blockquote>" +
                                "</div>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void ClimbUpException()
        {
            string result = _parser.Parse("em^^^^bq", ZenType.HTML);
            string expected = "<em></em><blockquote></blockquote>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void ClimbUpMultiple()
        {
            string result = _parser.Parse("div+div>p>span+em^^^bq", ZenType.HTML);
            string expected = "<div></div>" +
                                "<div>" +
                                  "<p><span></span><em></em></p>" +
                                "</div>" +
                                "<blockquote></blockquote>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void ClimbUpWithAttributes()
        {
            string result = _parser.Parse("p.test^p.cake", ZenType.HTML);
            string expected = "<p class=\"test\"></p>" +
                              "<p class=\"cake\"></p>";


            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void ClimbUpWithAttributes2()
        {
            string result = _parser.Parse("p.test>span.test^p.cake", ZenType.HTML);
            string expected = "<p class=\"test\">" +
                                "<span class=\"test\"></span>" +
                              "</p>" +
                              "<p class=\"cake\"></p>";


            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void ClimbUpWithCount()
        {
            string result = _parser.Parse("li*2^p", ZenType.HTML);
            string expected = "<li></li><li></li><p></p>";


            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void ClimbUpWithCount2()
        {
            string result = _parser.Parse("ul>li*2^p", ZenType.HTML);
            string expected = "<ul><li></li><li></li></ul><p></p>";


            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void ClimbUp2()
        {
            string result = _parser.Parse("div>ul>li^^p", ZenType.HTML);
            string expected = "<div><ul><li></li></ul></div><p></p>";


            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }
    }
}
