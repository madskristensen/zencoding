using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ZenParser = ZenCoding.HtmlParser;

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
            Assert.IsFalse(ZenParser.IsValid(string.Empty));
            Assert.IsFalse(ZenParser.IsValid("ost ost"));
            Assert.IsFalse(ZenParser.IsValid("div<ost"));
            Assert.IsFalse(ZenParser.IsValid("div!"));
            Assert.IsFalse(ZenParser.IsValid("div["));
            Assert.IsFalse(ZenParser.IsValid("div[]]"));
            Assert.IsFalse(ZenParser.IsValid("ul "));
            Assert.IsFalse(ZenParser.IsValid("{display"));
            Assert.IsFalse(ZenParser.IsValid("@for"));
            Assert.IsFalse(ZenParser.IsValid("div{text}}"));
            Assert.IsFalse(ZenParser.IsValid("div[text]]>div"));
            Assert.IsFalse(ZenParser.IsValid("asp:literal"));
            Assert.IsFalse(ZenParser.IsValid("ASP:LITERAL"));
            Assert.IsTrue(ZenParser.IsValid("!ion-list"));
        }

        [TestMethod]
        public void Validate2()
        {
            var result = new ZenCoding.Parser().Parse(string.Empty, ZenType.HTML);

            Assert.AreEqual(string.Empty, result);
        }
    }
}
