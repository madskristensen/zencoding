using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            writer.Write(Environment.NewLine);
            base.Render(writer);
            writer.Write(Environment.NewLine);
        }
    }
}
