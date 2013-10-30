using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    public class CustomHtmlInput : HtmlGenericControl
    {
        public CustomHtmlInput()
        {
            this.TagName = "input";
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write(Environment.NewLine);

            writer.WriteBeginTag("input");

            writer.WriteAttribute("type", Attributes["type"]);

            foreach (var key in this.Attributes.Keys)
            {
                string attr = (string)key;

                if (attr != "name" && attr != "type")
                    writer.WriteAttribute(attr, Attributes[attr]);
            }

            writer.Write(HtmlTextWriter.SelfClosingTagEnd);
            writer.Write(Environment.NewLine);
        }
    }
}
