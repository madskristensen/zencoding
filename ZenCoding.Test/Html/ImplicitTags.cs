using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZenCoding.Test.Html
{
    [TestClass]
    public class ImplicitTags
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void SimpleImplicitTag()
        {
            string result = _parser.Parse(".footer>.copy", ZenType.HTML);
            string expected = "<div class=\"footer\">" +
                              "<div class=\"copy\">" +
                              "</div>" +
                              "</div>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void UnsortedListsAndListItems()
        {
            string result = _parser.Parse("ul>.class-name", ZenType.HTML);
            string expected = "<ul>" +
                              "<li class=\"class-name\">" +
                              "</li>" +
                              "</ul>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void UnsortedListsAndListItemsComplex()
        {
            string result = _parser.Parse("ul>.class-name*3+#print8+div+.point^div", ZenType.HTML);
            string expected = "<ul>" +
                              "<li class=\"class-name\">" +
                              "</li>" +
                              "<li class=\"class-name\">" +
                              "</li>" +
                              "<li class=\"class-name\">" +
                              "</li>" +
                              "<li id=\"print8\">" +
                              "</li>" +
                              "<div>" +
                              "</div>" +
                              "<li class=\"point\">" +
                              "</li>" +
                              "</ul>" +
                              "<div>" +
                              "</div>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void SortedListsAndListItems()
        {
            string result = _parser.Parse("ol>.class-name", ZenType.HTML);
            string expected = "<ol>" +
                              "<li class=\"class-name\">" +
                              "</li>" +
                              "</ol>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void SortedListsAndListItemsComplex()
        {
            string result = _parser.Parse("ol>.class-name*3+#print8+div+.point^div", ZenType.HTML);
            string expected = "<ol>" +
                              "<li class=\"class-name\">" +
                              "</li>" +
                              "<li class=\"class-name\">" +
                              "</li>" +
                              "<li class=\"class-name\">" +
                              "</li>" +
                              "<li id=\"print8\">" +
                              "</li>" +
                              "<div>" +
                              "</div>" +
                              "<li class=\"point\">" +
                              "</li>" +
                              "</ol>" +
                              "<div>" +
                              "</div>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void EmphasizedAndSpan()
        {
            string result = _parser.Parse("em>.class-name", ZenType.HTML);
            string expected = "<em>" +
                              "<span class=\"class-name\">" +
                              "</span>" +
                              "</em>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TableAndRow()
        {
            string result = _parser.Parse("table>.class-name", ZenType.HTML);
            string expected = "<table>" +
                              "<tr class=\"class-name\">" +
                              "</tr>" +
                              "</table>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void RowAndData()
        {
            string result = _parser.Parse("tr>.class-name", ZenType.HTML);
            string expected = "<tr>" +
                              "<td class=\"class-name\">" +
                              "</td>" +
                              "</tr>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TableAndRowAndData()
        {
            string result = _parser.Parse("table>.row-class-name>.data-class-name", ZenType.HTML);
            string expected = "<table>" +
                              "<tr class=\"row-class-name\">" +
                              "<td class=\"data-class-name\">" +
                              "</td>" +
                              "</tr>" +
                              "</table>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TableAndRowColumnsAndNestedMarkup()
        {
            string result = _parser.Parse("table>.a>.b>.c>.d>.e.f.g", ZenType.HTML);
            string expected = "<table>" +
                              "<tr class=\"a\">" +
                              "<td class=\"b\">" +
                              "<div class=\"c\">" +
                              "<div class=\"d\">" +
                              "<div class=\"e f g\">" +
                              "</div>" +
                              "</div>" +
                              "</div>" +
                              "</td>" +
                              "</tr>" +
                              "</table>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }
    }
}
