using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ZenCoding.Test
{
    [TestClass]
    public class Plus
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void Plus1()
        {
            string result = _parser.Parse("ul+", ZenType.HTML);

            Assert.AreEqual("<ul><li></li></ul>", result);
        }

        [TestMethod]
        public void PlusTr()
        {
            string result = _parser.Parse("tr+", ZenType.HTML);

            Assert.AreEqual("<tr><td></td></tr>", result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void PlusTable()
        {
            string result = _parser.Parse("table+", ZenType.HTML);

            Assert.AreEqual("<table><tr><td></td></tr></table>", result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void PlusDl()
        {
            string result = _parser.Parse("dl+", ZenType.HTML);

            Assert.AreEqual("<dl><dt></dt><dd></dd></dl>", result);
        }

        [TestMethod]
        public void PlusSelect()
        {
            string result = _parser.Parse("select+", ZenType.HTML);

            Assert.AreEqual("<select><option value=\"\"></option></select>", result);
        }

        [TestMethod]
        public void PlusMap()
        {
            string result = _parser.Parse("map+", ZenType.HTML);
            string expected = "<map><area shape=\"\" coords=\"\" href=\"\" alt=\"\"></area></map>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void PlusResetCount()
        {
            string result = _parser.Parse("li*2+p", ZenType.HTML);

            Assert.AreEqual("<li></li><li></li><p></p>", result.Replace(Environment.NewLine, ""));
        }
    }
}
