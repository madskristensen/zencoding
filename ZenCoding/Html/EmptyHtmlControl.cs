using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    public class EmptyHtmlControl : HtmlGenericControl
    {
        public EmptyHtmlControl()
        {
            TagName = string.Empty;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.RenderChildren(writer);
        }
    }
}
