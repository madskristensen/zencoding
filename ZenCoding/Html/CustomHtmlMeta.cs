using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    public class CustomHtmlMeta : HtmlMeta
    {
        public CustomHtmlMeta()
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(Environment.NewLine);
            base.Render(writer);
            writer.Write(Environment.NewLine);
        }
    }
}
