﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ZenCoding.Test
{
    [TestClass]
    public class InnerText
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void InnerTextSimple()
        {
            string result = _parser.Parse("a{Click here}", ZenType.HTML);
            string expected = "<a href=\"\">" + Environment.NewLine +
                              "Click here" + Environment.NewLine + 
                              "</a>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void InnerTextWithCounter()
        {
            string result = _parser.Parse("a[prop=val]{Click here $}", ZenType.HTML);
            string expected = "<a href=\"\" prop=\"val\">" + Environment.NewLine +
                              "Click here 1" + Environment.NewLine + 
                              "</a>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void InnerTextWithMultipleCounter()
        {
            string result = _parser.Parse("ul>li*2>a[prop=val][href=ost]{Click here $}", ZenType.HTML);
            string expected = "<ul>" + Environment.NewLine +
                                "<li>" + Environment.NewLine +
                                "<a href=\"ost\" prop=\"val\">" + Environment.NewLine +
                                "Click here 1" + Environment.NewLine + 
                                "</a>" + Environment.NewLine +
                                "</li>" + Environment.NewLine +
                                "<li>" + Environment.NewLine +
                                "<a href=\"ost\" prop=\"val\">" + Environment.NewLine +
                                "Click here 2" + Environment.NewLine + 
                                "</a>" + Environment.NewLine +
                                "</li>" + Environment.NewLine +
                              "</ul>";                

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void InnerTextMultiple()
        {
            string result = _parser.Parse("p>{Click }>a{here}+{ to continue}", ZenType.HTML);
            string expected = "<p>Click <a href=\"\">here</a> to continue</p>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void InnerTextMultiple2()
        {
            string result = _parser.Parse("p{Click }>a{here}+{ to continue}", ZenType.HTML);
            string expected = "<p>Click <a href=\"\">here</a> to continue</p>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void InnerTextSeparator()
        {
            string result = _parser.Parse("footer>p{Sample + text}", ZenType.HTML);
            string expected = "<footer><p>Sample + text</p></footer>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void InnerTextSeparator2()
        {
            string result = _parser.Parse("footer>p{Sample >> text}", ZenType.HTML);
            string expected = "<footer><p>Sample >> text</p></footer>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void InnerTextStar()
        {
            string result = _parser.Parse("span.required{*}", ZenType.HTML);
            string expected = "<span class=\"required\">*</span>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }
    }
}
