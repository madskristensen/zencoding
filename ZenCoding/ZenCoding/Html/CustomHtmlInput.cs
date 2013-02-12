using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    class CustomHtmlInput : HtmlGenericControl
    {
        public CustomHtmlInput()
        {
            this.TagName = "input";
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //this will output the start of the img element - <img
            writer.WriteBeginTag("input");

            writer.WriteAttribute("type", Attributes["type"]);

            foreach (var key in this.Attributes.Keys)
            {
                string attr = (string)key;

                if (attr != "name" && attr != "type")
                    writer.WriteAttribute(attr, Attributes[attr]);
            }

            //this will output the src and alt attributes
            //writer.WriteAttribute("src", src);
            //writer.WriteAttribute("alt", alt);

            writer.Write(HtmlTextWriter.SelfClosingTagEnd);
        }
    }
}
