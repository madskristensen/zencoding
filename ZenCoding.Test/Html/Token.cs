using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZenCoding.Test
{
    [TestClass]
    public class Token
    {
        private ZenCoding.Parser _parser;

        [TestInitialize]
        public void Initialize()
        {
            _parser = new ZenCoding.Parser();
        }

        [TestMethod]
        public void TokenClassSimple()
        {
            string result = _parser.Parse("p.name-$*3", ZenType.HTML);
            string expected = "<p class=\"name-1\"></p>" +
                              "<p class=\"name-2\"></p>" +
                              "<p class=\"name-3\"></p>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TokenClassComplex()
        {
            string result = _parser.Parse("select>option.item-$*3", ZenType.HTML);
            string expected = "<select>" +
                              "<option class=\"item-1\"></option>" +
                              "<option class=\"item-2\"></option>" +
                              "<option class=\"item-3\"></option>" +
                              "</select>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TokenIdSimple()
        {
            string result = _parser.Parse("p#name-$*3", ZenType.HTML);
            string expected = "<p id=\"name-1\"></p>" +
                              "<p id=\"name-2\"></p>" +
                              "<p id=\"name-3\"></p>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TokenIdComplex()
        {
            string result = _parser.Parse("select>option#item-$*3", ZenType.HTML);
            string expected = "<select>" +
                              "<option id=\"item-1\"></option>" +
                              "<option id=\"item-2\"></option>" +
                              "<option id=\"item-3\"></option>" +
                              "</select>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TokenNestedMultipleChildren()
        {
            string result = _parser.Parse("ul>li*2>span+input", ZenType.HTML);
            string expected = "<ul>" +
                                  "<li>" +
                                      "<span></span>" +
                                      "<input type=\"\" value=\"\" />" +
                                  "</li>" +
                                  "<li>" +
                                  "<span></span>" +
                                      "<input type=\"\" value=\"\" />" +
                                   "</li>" +
                              "</ul>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }

        [TestMethod]
        public void TokenNestedMultipleChildrenComplex()
        {
            string result = _parser.Parse("ul[data-bind=\"foreach:customers\"]>li*3>span{Caption $$}*4+input[type=text data-bind=\"value:$$\"]+ul[data-bind=\"foreach:customers\"]>li*4>span{Caption $$}+input[type=text data-bind=\"value:$$\"]", ZenType.HTML);
            string expected = "<ul data-bind=\"foreach:customers\">" +
                                "<li>" +
                                    "<span>Caption 01</span>" +
                                    "<span>Caption 02</span>" +
                                    "<span>Caption 03</span>" +
                                    "<span>Caption 04</span>" +
                                    "<input type=\"text\" value=\"\" data-bind=\"value:01\" />" +
                                    "<ul data-bind=\"foreach:customers\">" +
                                        "<li><span>Caption 01</span><input type=\"text\" value=\"\" data-bind=\"value:01\" /></li>" +
                                        "<li><span>Caption 02</span><input type=\"text\" value=\"\" data-bind=\"value:02\" /></li>" +
                                        "<li><span>Caption 03</span><input type=\"text\" value=\"\" data-bind=\"value:03\" /></li>" +
                                        "<li><span>Caption 04</span><input type=\"text\" value=\"\" data-bind=\"value:04\" /></li>" +
                                    "</ul>" +
                                "</li>" +
                                "<li>" +
                                    "<span>Caption 01</span>" +
                                    "<span>Caption 02</span>" +
                                    "<span>Caption 03</span>" +
                                    "<span>Caption 04</span>" +
                                    "<input type=\"text\" value=\"\" data-bind=\"value:02\" />" +
                                    "<ul data-bind=\"foreach:customers\">" +
                                        "<li><span>Caption 01</span><input type=\"text\" value=\"\" data-bind=\"value:01\" /></li>" +
                                        "<li><span>Caption 02</span><input type=\"text\" value=\"\" data-bind=\"value:02\" /></li>" +
                                        "<li><span>Caption 03</span><input type=\"text\" value=\"\" data-bind=\"value:03\" /></li>" +
                                        "<li><span>Caption 04</span><input type=\"text\" value=\"\" data-bind=\"value:04\" /></li>" +
                                    "</ul>" +
                                "</li>" +
                                "<li>" +
                                    "<span>Caption 01</span>" +
                                    "<span>Caption 02</span>" +
                                    "<span>Caption 03</span>" +
                                    "<span>Caption 04</span>" +
                                    "<input type=\"text\" value=\"\" data-bind=\"value:03\" />" +
                                    "<ul data-bind=\"foreach:customers\">" +
                                        "<li><span>Caption 01</span><input type=\"text\" value=\"\" data-bind=\"value:01\" /></li>" +
                                        "<li><span>Caption 02</span><input type=\"text\" value=\"\" data-bind=\"value:02\" /></li>" +
                                        "<li><span>Caption 03</span><input type=\"text\" value=\"\" data-bind=\"value:03\" /></li>" +
                                        "<li><span>Caption 04</span><input type=\"text\" value=\"\" data-bind=\"value:04\" /></li>" +
                                    "</ul>" +
                                "</li>" +
                            "</ul>";

            Assert.AreEqual(expected, result.Replace(Environment.NewLine, string.Empty));
        }
    }
}
