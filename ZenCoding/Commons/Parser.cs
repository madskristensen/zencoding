namespace ZenCoding
{
    public class Parser
    {
        IZenParser _parser;

        public string Parse(string zenSyntax, ZenType type)
        {
            if (zenSyntax == null)
                return null;

            switch (type)
            {
                case ZenType.HTML:
                    this._parser = new HtmlParser();
                    return this._parser.Parse(zenSyntax.Trim());
                case ZenType.CSS:
                    this._parser = new CssParser();
                    return this._parser.Parse(zenSyntax.Trim());
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
