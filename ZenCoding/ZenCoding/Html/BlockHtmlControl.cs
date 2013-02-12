using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            base.RenderBeginTag(writer);

            if (Controls.Count > 0)
                writer.Write(Environment.NewLine);
        }
    }
}
