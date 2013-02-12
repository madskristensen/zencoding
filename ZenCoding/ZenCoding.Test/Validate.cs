using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using P = ZenCoding.HtmlParser;

namespace ZenCoding.Test
{
    [TestClass]
    public class Validate
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void Validate1()
        {
            Assert.IsFalse(P.IsValid(string.Empty));
            Assert.IsFalse(P.IsValid("ost ost"));
            Assert.IsFalse(P.IsValid("div<ost"));
            Assert.IsFalse(P.IsValid("div!"));
            Assert.IsFalse(P.IsValid("div["));
            Assert.IsFalse(P.IsValid("div[]]"));
            Assert.IsFalse(P.IsValid("ul "));
            Assert.IsFalse(P.IsValid("{display"));
            Assert.IsFalse(P.IsValid("@for"));
            Assert.IsFalse(P.IsValid("div{text}}"));
            Assert.IsFalse(P.IsValid("div[text]]>div"));
            Assert.IsFalse(P.IsValid("asp:literal"));
            Assert.IsFalse(P.IsValid("ASP:LITERAL"));
        }

        [TestMethod]
        public void Validate2()
        {
            var result = new ZenCoding.Parser().Parse(string.Empty, ZenType.HTML);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void ValidateHtmlElements()
        {
            string result = _parser.Parse("for", ZenType.HTML);
            string expected = "";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void ValidateMultipleHtmlElements()
        {
            string result = _parser.Parse("div>for", ZenType.HTML);
            string expected = "";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }
    }
}
