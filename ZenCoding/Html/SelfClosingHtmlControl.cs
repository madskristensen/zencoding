using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    internal class HtmlGenericSelfClosing : HtmlGenericControl
    {
        public HtmlGenericSelfClosing(string tag)
            : base(tag)
        {
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write(HtmlTextWriter.TagLeftChar + this.TagName);
            Attributes.Render(writer);
            writer.Write(HtmlTextWriter.SelfClosingTagEnd);
        }

        //public override ControlCollection Controls
        //{
        //    get { throw new Exception("Self-closing tag cannot have child controls"); }
        //}

        //public override string InnerHtml
        //{
        //    get { return String.Empty; }
        //    set { throw new Exception("Self-closing tag cannot have inner content"); }
        //}

        //public override string InnerText
        //{
        //    get { return String.Empty; }
        //    set { throw new Exception("Self-closing tag cannot have inner content"); }
        //}
    }

}
