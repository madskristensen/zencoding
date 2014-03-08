using System;
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
            if (writer == null)
                return;

            base.RenderChildren(writer);

            var count = this.Parent.Controls.Count;

            if (count < 2)
                return;

            var index = this.Parent.Controls.IndexOf(this);

            if ((index == 0 && this.Parent.Controls[index + 1] is EmptyHtmlControl) || index > 0 &&
                ((index == --count && this.Parent.Controls[count] is EmptyHtmlControl) ||
                (this.Parent.Controls[index - 1] is EmptyHtmlControl || this.Parent.Controls[index + 1] is EmptyHtmlControl)))
                writer.WriteLine(Environment.NewLine);
        }
    }
}
