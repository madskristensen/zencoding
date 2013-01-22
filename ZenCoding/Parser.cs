
namespace ZenCoding
{
    public class Parser
    {
        public string Parse(string zenSyntax, ZenType type)
        {
            switch (type)
            {
                case ZenType.CSS:
                    break;

                case ZenType.HTML:
                    HtmlParser parser = new HtmlParser();
                    return parser.Parse(zenSyntax.Trim());
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
