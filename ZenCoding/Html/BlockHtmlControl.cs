using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    class BlockHtmlControl : HtmlGenericControl
    {
        public BlockHtmlControl(string tagName)
        {
            TagName = tagName.Replace("!", "");
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (writer == null)
                return;

            writer.Write(Environment.NewLine);
            base.RenderBeginTag(writer);

            if (this.Controls.Count > 0 && this.Controls[0].Controls.Count > 1 || this.Controls.Count > 1)
            {
                writer.WriteLine(Environment.NewLine);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (writer == null)
                return;

            if (this.Controls.Count > 0 && this.Controls[0].Controls.Count > 1 || this.Controls.Count > 1)
            {
                writer.WriteLine(Environment.NewLine);
            }
            base.RenderEndTag(writer);
            writer.Write(Environment.NewLine);
        }
    }
}
