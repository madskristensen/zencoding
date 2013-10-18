using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ZenCoding
{
    public static class HtmlElementFactory
    {
        public static HtmlControl CloneElement(this HtmlControl element, int count)
        {
            HtmlControl control = HtmlElementFactory.Create(element.TagName.Increment(count));

            foreach (var attribute in element.Attributes.Keys)
            {
                string attr = (string)attribute;
                control.Attributes[attr] = element.Attributes[attr].Increment(count);

                if (attr == "class")
                {
                    control.Attributes[attr] = control.Attributes[attr].Increment(count);
                }
            }

            if (!string.IsNullOrEmpty(element.ID))
            {
                control.ID = element.ID.Increment(count);
            }

            if (element.Controls.Count == 1)
            {
                LiteralControl literal = element.Controls[0] as LiteralControl;
                if (literal != null)
                    control.Controls.Add(new LiteralControl(literal.Text.Increment(count)));
            }

            var lorem = element as LoremControl;
            if (lorem != null)
            {
                lorem.InnerText = lorem.Generate(count);
            }
           
            return control;
        }

        public static string Increment(this string text, int count)
        {
            MatchCollection matches = Regex.Matches(text, @"(\$+)");
            
            foreach (Match match in matches)
            {
                text = text.Replace(match.Value, (count + 1).ToString().PadLeft(match.Value.Length, '0'));
            }

            return text;
        }

        public static HtmlControl Create(string tagName)
        {
            if (tagName.StartsWith("lorem", System.StringComparison.Ordinal))
            {
                return new LoremControl(tagName);
            }
            else if (tagName.StartsWith("pix", System.StringComparison.Ordinal))
            {
                return new LoremPixel(tagName);
            }

            switch (tagName)
            {
                case "":
                    return new EmptyHtmlControl();

                case "a":
                    HtmlAnchor a = new HtmlAnchor();
                    a.Attributes["href"] = "";
                    return a;

                case "button":
                case "btn":
                    return new HtmlButton();

                case "input":
                    CustomHtmlInput input = new CustomHtmlInput();
                    input.Attributes["value"] = "";
                    input.Attributes["type"] = "";
                    return input;

                case "img":
                    var img = new HtmlImage();
                    img.Attributes["src"] = string.Empty;
                    img.Attributes["alt"] = string.Empty;
                    return img;

                case "source":
                case "src":
                    return new HtmlGenericSelfClosing("source");
                    
                case "meta":
                    return new HtmlMeta();

                case "link":
                    return new HtmlLink();

                case "abbr":
                case "acronym":
                    HtmlGenericControl abbr = new HtmlGenericControl("acronym");
                    abbr.Attributes["title"] = string.Empty;
                    return abbr;

                case "area":
                    BlockHtmlControl area = new BlockHtmlControl("area");
                    area.Attributes["shape"] = string.Empty;
                    area.Attributes["coords"] = string.Empty;
                    area.Attributes["href"] = string.Empty;
                    area.Attributes["alt"] = string.Empty;                    
                    return area;

                case "iframe":
                case "ifr":
                    HtmlGenericControl iframe = new HtmlGenericControl("iframe");
                    iframe.Attributes["src"] = string.Empty;
                    iframe.Attributes["frameborder"] = "0";
                    return iframe;

                case "param":
                    HtmlGenericSelfClosing param = new HtmlGenericSelfClosing("param");
                    param.Attributes["name"] = string.Empty;
                    param.Attributes["value"] = string.Empty;
                    return param;

                case "section":
                case "sect" :
                    return new BlockHtmlControl("section");

                case "article":
                case "art":
                    return new BlockHtmlControl("article");

                case "hgroup":
                case "hgr":
                    return new BlockHtmlControl("hgroup");

                case "header":
                case "hdr":
                    return new BlockHtmlControl("header");

                case "footer":
                case "ftr":
                    return new BlockHtmlControl("footer");

                case "adr":
                    return new BlockHtmlControl("address");

                case "dlg":
                    return new BlockHtmlControl("dialog");

                case "bq":
                    return new HtmlGenericControl("blockquote");

                case "acr":
                    return new HtmlGenericControl("acronym");

                case "prog":
                    return new HtmlGenericControl("progress");

                case "figure":
                case "fig":
                    return new BlockHtmlControl("figure");

                case "emb":
                    return new HtmlGenericControl("embed");

                case "object":
                case "obj":
                    return new BlockHtmlControl("object");

                case "cap":
                    return new HtmlGenericControl("caption");

                case "colg":
                    return new HtmlGenericControl("colgroup");

                case "fset":
                    return new BlockHtmlControl("fieldset");

                case "leg":
                    return new BlockHtmlControl("legend");

                case "optg":
                    return new HtmlGenericControl("optgroup");

                case "opt":
                    return new HtmlGenericControl("option");

                case "datag":
                    return new HtmlGenericControl("datagrid");

                case "datal":
                    return new HtmlGenericControl("datalist");

                case "textarea":
                case "tarea":
                    HtmlTextArea textarea = new HtmlTextArea();
                    textarea.ID = string.Empty;
                    textarea.Attributes["cols"] = string.Empty;
                    textarea.Attributes["rows"] = string.Empty;
                    return textarea;

                case "kg":
                    return new HtmlGenericControl("keygen");

                case "out":
                    return new HtmlGenericControl("output");

                case "html":
                case "head":
                case "body":                
                case "div":
                case "table":
                case "tr":                
                case "p":
                    return new BlockHtmlControl(tagName);

                case "br":
                case "hr":
                    return new HtmlGenericSelfClosing(tagName);
            }

            return new HtmlGenericControl(tagName);
        }

        public static Control CreateDoctypes(string part, ref List<Control> current)
        {
            ContentPlaceHolder root = new ContentPlaceHolder();

            string second = part.Substring(part.IndexOf(':') + 1);
            root.Controls.Add(new LiteralControl(GetDoctype(second)));

            HtmlControl html = HtmlElementFactory.Create("html");

            if (!second.StartsWith("x"))
            {
                html.Attributes["lang"] = "en";
            }
            else
            {
                html.Attributes["xmlns"] = "http://www.w3.org/1999/xhtml";
                html.Attributes["xml:lang"] = "en";
            }

            root.Controls.Add(html);

            HtmlControl head = HtmlElementFactory.Create("head");
            HtmlControl title = HtmlElementFactory.Create("title");
            HtmlControl meta = ShortcutHelper.Parse("meta:utf");
            head.Controls.Add(title);
            head.Controls.Add(meta);
            html.Controls.Add(head);

            HtmlControl body = HtmlElementFactory.Create("body");
            html.Controls.Add(body);
            current = new List<Control>() { body };

            return root;
        }

        private static string GetDoctype(string key)
        {
            switch (key)
            {
                case "4t":
                    return "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01 Transitional//EN\" \"http://www.w3.org/TR/html4/loose.dtd\">";

                case "4s":
                    return "<!DOCTYPE HTML PUBLIC \"-//W3C//DTD HTML 4.01//EN\" \"http://www.w3.org/TR/html4/strict.dtd\">";

                case "xt":
                    return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";

                case "xs":
                    return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";

                case "xxs":
                    return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">";

                case "5":
                    return "<!DOCTYPE html>";
            }

            return null;
        }
    }
}
