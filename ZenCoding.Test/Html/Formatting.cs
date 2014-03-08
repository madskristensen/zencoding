using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            string result = _parser.Parse("img", ZenType.HTML);
            string expected = "<img src=\"\" alt=\"\" />";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Formatting2()
        {
            string result = _parser.Parse("div>a*2+img^input", ZenType.HTML);

            string expected = "<div>" + Environment.NewLine +
                              "<a href=\"\"></a>" + Environment.NewLine +
                              "<a href=\"\"></a>" + Environment.NewLine +
                              "<img src=\"\" alt=\"\" />" + Environment.NewLine +
                              "</div>" + Environment.NewLine +
                              "<input type=\"\" value=\"\" />";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Formatting3()
        {
            string result = _parser.Parse("input+param", ZenType.HTML);
            string expected = "<input type=\"\" value=\"\" />" + Environment.NewLine + "<param name=\"\" value=\"\" />";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Formatting4()
        {
            string result = _parser.Parse("p>br+br", ZenType.HTML);
            string expected = "<p>" + Environment.NewLine + "<br /><br />" + Environment.NewLine + "</p>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Formatting5()
        {
            string result = _parser.Parse("section>img", ZenType.HTML);
            string expected = "<section><img src=\"\" alt=\"\" /></section>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Formatting6()
        {
            string result = _parser.Parse("ul>li>a*3", ZenType.HTML);
            string expected = "<ul>" + Environment.NewLine + "<li>" +
                    Environment.NewLine + "<a href=\"\"></a>" +
                    Environment.NewLine + "<a href=\"\"></a>" +
                    Environment.NewLine + "<a href=\"\"></a>" +
                    Environment.NewLine + "</li>" + Environment.NewLine + "</ul>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Formatting7()
        {
            string result = _parser.Parse("ul>li*3>{some text}", ZenType.HTML);
            string expected = "<ul>" +
                    Environment.NewLine + "<li>some text</li>" +
                    Environment.NewLine + "<li>some text</li>" +
                    Environment.NewLine + "<li>some text</li>" +
                    Environment.NewLine + "</ul>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Formatting8()
        {
            string result = _parser.Parse("ul>li*3>lorem", ZenType.HTML);
            string expected = "<ul>" +
                    Environment.NewLine + "<li>random lorem text</li>" +
                    Environment.NewLine + "<li>random lorem text</li>" +
                    Environment.NewLine + "<li>random lorem text</li>" +
                    Environment.NewLine + "</ul>";

            Assert.AreEqual(expected.Split(Environment.NewLine.ToCharArray()).Length, result.Split(Environment.NewLine.ToCharArray()).Length);
        }

    }
}
