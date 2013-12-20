using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding.Html
{
    public class CustomHtmlAnchor : HtmlAnchor
    {
        public CustomHtmlAnchor()
        {

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (writer == null)
                return;

            if (this.Controls.Count > 0 && this.Controls[0].Controls.Count > 0 || this.Controls.Count > 1
                || (this.Parent != null && (this.Parent.Controls.Count > 0 && this.Parent.Controls[0].Controls.Count > 1 || this.Parent.Controls.Count > 1)))
            {
                writer.WriteLine(Environment.NewLine);
                base.Render(writer);
                writer.Write(Environment.NewLine);
            }
            else
                base.Render(writer);
        }
    }
}
