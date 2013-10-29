﻿using System;
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
            writer.Write(Environment.NewLine);
            base.RenderBeginTag(writer);
        }

        protected override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            writer.Write(Environment.NewLine);
        }
    }
}
