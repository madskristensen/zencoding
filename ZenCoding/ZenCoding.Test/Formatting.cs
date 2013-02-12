using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;
using P = ZenCoding.Parser;

namespace ZenCoding.Test
{
    [TestClass]
    public class Formatting
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void Formatting1()
        {
            string result = _parser.Parse("div>a*2", ZenType.HTML);
            string expected = "<div>" + Environment.NewLine +
                              "<a href=\"\"></a>" + Environment.NewLine +
                              "<a href=\"\"></a>" + Environment.NewLine +
                              "</div>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Formatting2()
        {
            string result = _parser.Parse("param", ZenType.HTML);
            string expected = "<param name=\"\" value=\"\" />";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Formatting3()
        {
            string result = _parser.Parse("p>br+br", ZenType.HTML);
            string expected = "<p><br /><br /></p>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void Formatting4()
        {
            string result = _parser.Parse("section>img", ZenType.HTML);
            string expected = "<section><img src=\"\" alt=\"\" /></section>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }
    }
}
