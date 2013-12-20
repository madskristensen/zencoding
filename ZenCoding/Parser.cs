
namespace ZenCoding
{
    public class Parser
    {
        public string Parse(string zenSyntax, ZenType type)
        {
            if (zenSyntax == null)
                return null;

            switch (type)
            {
                case ZenType.HTML:
                    HtmlParser htmlParser = new HtmlParser();
                    return htmlParser.Parse(zenSyntax.Trim());

                case ZenType.CSS:
                    CssParser cssParser = new CssParser();
                    return cssParser.Parse(zenSyntax.Trim());
            }

            return null;
        }
    }

    public enum ZenType
    {
        HTML,
        CSS
    }
}
