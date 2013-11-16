using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    class BlockHtmlControl : HtmlGenericControl
    {
        public BlockHtmlControl(string tagName)
        {
            TagName = tagName;
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.Write(Environment.NewLine);
            base.RenderBeginTag(writer);
            if (this.Controls.Count > 0 && this.Controls[0].Controls.Count > 1 || this.Controls.Count > 1)
            {
                writer.WriteLine(Environment.NewLine);
            }
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            if (this.Controls.Count > 0 && this.Controls[0].Controls.Count > 1 || this.Controls.Count > 1)
            {
                writer.WriteLine(Environment.NewLine);
            }
            base.RenderEndTag(writer);
            writer.Write(Environment.NewLine);
        }
    }
}
