using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            writer.Write(Environment.NewLine);
            base.RenderBeginTag(writer);
            writer.Write(Environment.NewLine);
        }
    }

}