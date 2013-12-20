using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding.Html
{
    public class CustomHtmlImage : HtmlImage
    {
        public CustomHtmlImage()
        {

        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (writer == null)
                return;

            if (this.Controls.Count > 0 && this.Controls[0].Controls.Count > 0 || this.Controls.Count > 1
                || (this.Parent != null && (this.Parent.Controls.Count > 0 && this.Parent.Controls[0].Controls.Count > 1 || this.Parent.Controls.Count > 1)))
            {
                writer.WriteLine(Environment.NewLine);
                base.RenderBeginTag(writer);
                writer.Write(Environment.NewLine);
            }
            else
                base.RenderBeginTag(writer);
        }
    }

}