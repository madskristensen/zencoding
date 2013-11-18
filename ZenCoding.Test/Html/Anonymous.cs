using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZenCoding.Test
{
    [TestClass]
    public class Anonymous
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void Anonymous1()
        {
            string result = _parser.Parse("#name", ZenType.HTML);

            Assert.AreEqual("<div id=\"name\"></div>", result);
        }

        [TestMethod]
        public void Anonymous2()
        {
            string result = _parser.Parse("#name.item", ZenType.HTML);

            Assert.AreEqual("<div id=\"name\" class=\"item\"></div>", result);
        }

        [TestMethod]
        public void Anonymous3()
        {
            string result = _parser.Parse("footer+#name.item", ZenType.HTML);

            Assert.AreEqual("<footer></footer><div id=\"name\" class=\"item\"></div>", result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void Anonymous4()
        {
            string result = _parser.Parse("#name>.item", ZenType.HTML);

            Assert.AreEqual("<div id=\"name\"><div class=\"item\"></div></div>", result.Replace(Environment.NewLine, string.Empty));
        }
    }
}
