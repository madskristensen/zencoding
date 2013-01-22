
//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Collections.Generic;
//using System.Text;
//using P = ZenCoding.Parser;

//namespace ZenCoding.Test
//{
//    [TestClass]
//    public class Grouping
//    {
//        private ZenCoding.Parser _parser;

//        [TestInitialize]
//        public void Initialize()
//        {
//            _parser = new ZenCoding.Parser();
//        }

//        [TestMethod]
//        public void GroupingSimple()
//        {
//            string result = _parser.Parse("div>(header>h1)+footer", ZenType.HTML);
//            string expected = "<div></div>" +
//                                "<div>" +
//                                  "<p><span></span><em></em></p>" +
//                                  "<blockquote></blockquote>" +
//                                "</div>";

//            Assert.AreEqual(expected, result);
//        }
//    }
//}
