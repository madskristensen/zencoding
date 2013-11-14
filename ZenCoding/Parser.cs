
namespace ZenCoding
{
    public class Parser
    {
        public string Parse(string zenSyntax, ZenType type)
        {
            switch (type)
            {
                case ZenType.CSS:
                    CssParser cssParser = new CssParser();
                    return cssParser.Parse(zenSyntax.Trim());

                case ZenType.HTML:
                    HtmlParser htmlParser = new HtmlParser();
                    return htmlParser.Parse(zenSyntax.Trim());
            }

            return null;
        }
    }

    public enum ZenType
    {
        CSS,
        HTML
    }
}
