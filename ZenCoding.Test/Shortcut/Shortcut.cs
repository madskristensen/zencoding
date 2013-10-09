using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Text;

namespace ZenCoding.Test
{
    [TestClass]
    public class Shortcut
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void ShortcutHidden()
        {
            string result = _parser.Parse("input:h", ZenType.HTML);
            string expected = "<input type=\"hidden\" value=\"\" />";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ShortcutSearch()
        {
            string result = _parser.Parse("input:search", ZenType.HTML);
            string expected = "<input type=\"search\" value=\"\" id=\"\" />";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ShortcutChildren()
        {
            string result = _parser.Parse("input:search+div", ZenType.HTML);
            string expected = "<input type=\"search\" value=\"\" id=\"\" />\r\n<div></div>";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ShortcutComplex()
        {
            string result = _parser.Parse("html:xt>div#header>div#logo+ul#nav>li.item-$*5>a", ZenType.HTML);
            string expected = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
                              "<html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\">" +
                              "<head>" +
                                  "<title></title>" +
                                  "<meta http-equiv=\"content-type\" content=\"text/html;charset=UTF-8\" />" +
                              "</head>" +
                              "<body>" +
                                  "<div id=\"header\">" +
                                      "<div id=\"logo\"></div>" +
                                      "<ul id=\"nav\">" +
                                          "<li class=\"item-1\"><a href=\"\"></a></li>" +
                                          "<li class=\"item-2\"><a href=\"\"></a></li>" +
                                          "<li class=\"item-3\"><a href=\"\"></a></li>" +
                                          "<li class=\"item-4\"><a href=\"\"></a></li>" +
                                          "<li class=\"item-5\"><a href=\"\"></a></li>" +
                                      "</ul>" +
                                  "</div>" +
                              "</body>" +
                              "</html>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void ShortcutNested()
        {
            string result = _parser.Parse("form>input:t", ZenType.HTML);
            string expected = "<form>" + Environment.NewLine + "<input type=\"text\" value=\"\" id=\"\" />" + Environment.NewLine + "</form>";

            Assert.AreEqual(expected, result);
        }
    }
}
