using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    class ShortcutHelper
    {
        public static HtmlControl Parse(string zenSyntax)
        {
            string[] args = zenSyntax.TrimStart('+', '>', '^').Split(':');

            if (args.Length != 2)
                return null;

            HtmlControl element = HtmlElementFactory.Create(args[0]);
            AddAttributes(element, args[1]);

            return element;
        }
               

        private static void AddAttributes(HtmlControl element, string keyword)
        {
            switch (keyword)
            {
                // HTML
                case "xml":
                    element.Attributes["xmlns"] = "http://www.w3.org/1999/xhtml";
                    element.Attributes["xml:lang"] = "en";
                    break;

                // Link
                case "css":
                    element.Attributes["rel"] = "stylesheet";
                    element.Attributes["href"] = "";
                    element.Attributes["media"] = "all";
                    break;

                case "print":
                    element.Attributes["rel"] = "stylesheet";
                    element.Attributes["href"] = "";
                    element.Attributes["type"] = "print";
                    break;

                case "favicon":
                    element.Attributes["rel"] = "shortcut icon";
                    element.Attributes["href"] = "";
                    element.Attributes["media"] = "image/x-icon";
                    break;

                case "touch":
                    element.Attributes["rel"] = "apple-touch-icon";
                    element.Attributes["href"] = "";
                    break;

                case "rss":
                    element.Attributes["rel"] = "alternate";
                    element.Attributes["type"] = "application/rss+xml";
                    element.Attributes["title"] = "RSS";
                    element.Attributes["href"] = "";
                    break;

                case "atom":
                    element.Attributes["rel"] = "alternate";
                    element.Attributes["type"] = "application/atom+xml";
                    element.Attributes["title"] = "Atom";
                    element.Attributes["href"] = "";
                    break;

                // Meta
                case "utf":
                    element.Attributes["http-equiv"] = "content-type";
                    element.Attributes["content"] = "text/html;charset=UTF-8";
                    break;

                case "win":
                    element.Attributes["http-equiv"] = "content-type";
                    element.Attributes["content"] = "text/html;charset=win-1251";
                    break;

                case "compat":
                    element.Attributes["http-equiv"] = "X-UA-Compatible";
                    element.Attributes["content"] = "IE=7";
                    break;

                // Script
                case "src":
                    element.Attributes["src"] = "";
                    break;

                // A
                case "link":
                    element.Attributes["href"] = "http://";
                    break;

                case "mail":
                    element.Attributes["href"] = "mailto:";
                    break;

                // BDO
                case "r":
                    if (element.TagName == "bdo")
                        element.Attributes["dir"] = "rtl";
                    else if (element.TagName == "area")
                        element.Attributes["shape"] = "rect";
                    else if (element.TagName == "input")
                    {
                        element.Attributes["type"] = "radio";
                        element.Attributes["id"] = "";
                        element.Attributes.Remove("value");
                    }
                    break;

                case "l":
                    element.Attributes["dir"] = "ltr";
                    break;

                // Area
                case "d":
                    element.Attributes["shape"] = "default";
                    element.Attributes.Remove("coords");
                    break;

                case "c":
                    if (element.TagName == "area")
                    {
                        element.Attributes["shape"] = "circle";
                    }
                    else if (element.TagName == "input")
                    {
                        element.Attributes["type"] = "checkbox";
                        element.Attributes["id"] = "";
                        element.Attributes.Remove("value");
                    }
                    break;

                case "p":
                    if (element.TagName == "area")
                    {
                        element.Attributes["shape"] = "poly";
                    }
                    else if (element.TagName == "input")
                    {
                        element.Attributes["type"] = "password";
                        element.Attributes["id"] = "";
                    }
                    break;

                // Form
                case "get":
                    element.Attributes["action"] = "";
                    element.Attributes["method"] = "get";
                    break;

                case "post":
                    element.Attributes["action"] = "";
                    element.Attributes["method"] = "post";
                    break;

                // Input
                case "hidden":
                case "h":
                    element.Attributes["type"] = "hidden";
                    break;

                case "text":
                case "t":
                    element.Attributes["type"] = "text";
                    element.Attributes["id"] = "";
                    break;

                case "password":
                case "search":
                case "email":
                case "url":
                case "datetime":
                case "datetime-local":
                case "date":
                case "month":
                case "week":
                case "time":
                case "number":
                case "range":
                case "color":
                    element.Attributes["type"] = keyword;
                    element.Attributes["id"] = "";
                    break;

                case "checkbox":
                case "radio":
                    element.Attributes["type"] = keyword;
                    element.Attributes["id"] = "";
                    element.Attributes.Remove("value");
                    break;

                case "file":
                case "f":
                    element.Attributes["type"] = "file";
                    element.Attributes["id"] = "";
                    element.Attributes.Remove("value");
                    break;

                case "submit":
                case "s":
                    element.Attributes["type"] = "submit";
                    break;

                case "reset":
                    element.Attributes["type"] = "reset";
                    break;

                case "image":
                case "i":
                    element.Attributes["type"] = "image";
                    element.Attributes["src"] = "";
                    element.Attributes["alt"] = "";
                    break;

                case "button":
                case "b":
                    element.Attributes["type"] = "button";
                    break;
            }
        }
    }
}
