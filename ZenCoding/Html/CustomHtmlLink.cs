using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    public class CustomHtmlLink : HtmlLink
    {
        public CustomHtmlLink()
        {
        }

        protected override void RenderBeginTag(HtmlTextWriter writer)
        {
            if (writer == null)
                return;

            writer.Write(Environment.NewLine);
            base.RenderBeginTag(writer);
            writer.Write(Environment.NewLine);
        }
    }
}
