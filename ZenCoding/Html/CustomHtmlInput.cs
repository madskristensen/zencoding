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
            if (writer == null)
                return;

            if (this.Controls.Count > 0 && this.Controls[0].Controls.Count > 0 || this.Controls.Count > 1
                || (this.Parent != null && (this.Parent.Controls.Count > 0 && this.Parent.Controls[0].Controls.Count > 1 || this.Parent.Controls.Count > 1)))
            {
                writer.WriteLine(Environment.NewLine);
            }

            writer.WriteBeginTag("input");

            writer.WriteAttribute("type", Attributes["type"]);

            foreach (var key in this.Attributes.Keys)
            {
                string attr = (string)key;

                if (attr != "name" && attr != "type")
                    writer.WriteAttribute(attr, Attributes[attr]);
            }

            writer.Write(HtmlTextWriter.SelfClosingTagEnd);
            if (this.Controls.Count > 0 && this.Controls[0].Controls.Count > 0 || this.Controls.Count > 1
                || (this.Parent != null && (this.Parent.Controls.Count > 0 && this.Parent.Controls[0].Controls.Count > 1 || this.Parent.Controls.Count > 1)))
            {
                writer.WriteLine(Environment.NewLine);
            }
        }
    }
}
