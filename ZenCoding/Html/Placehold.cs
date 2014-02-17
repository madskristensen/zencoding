using System;
using System.Linq;
using System.Text;
using ZenCoding.Html;

namespace ZenCoding
{
    public class PlaceHold : PixelBase
    {
        private const string _url = "http://placehold.it/";
        private static readonly string[] _formats = new string[] { "gif", "jpg", "jpeg", "png" };

        public string Text { get; set; }
        public string Format { get; set; }
        public string Foreground { get; set; }
        public string Background { get; set; }

        public PlaceHold(string pixText)
        {
            Generate(pixText);
        }

        protected override void Initialize()
        {
            this.Foreground = "";
            this.Background = "";
            this.AssetWidth = this.AssetHeight = 30;
            this.Format = "";
            this.Text = "";
        }

        public override void Generate(string pixText)
        {
            string dimensions = "";
            string[] parts;

            parts = pixText == null ? new string[] { } : pixText.Split('-');

            try
            {
                if (parts.Length > 1)
                {
                    dimensions = parts[1];

                    SetDimensions(dimensions);

                    if (parts.Length > 2)
                    {
                        if (_formats.Contains(parts[2]))
                            this.Format = parts[2];
                        else
                            SetTextAndColors(parts[2]);

                        if (parts.Length > 3)
                            SetTextAndColors(parts[3]);

                        if (parts.Length > 4)
                            SetTextAndColors(parts[4]);
                    }
                }
            }
            catch
            { }

            this.AssetWidth = this.AssetWidth % 1921;
            this.AssetHeight = this.AssetHeight % 1921;

            SetPath();
        }

        private void SetTextAndColors(string slug)
        {
            if (string.IsNullOrEmpty(this.Text) && slug.Contains("="))
                this.Text = slug.Split('=')[1];
            else if ((slug.Length == 3 || slug.Length == 6) && slug.All(c => c.IsHex()))
                if (string.IsNullOrEmpty(this.Background))
                    this.Background = slug;
                else
                    this.Foreground = slug;
        }

        protected override void SetPath()
        {
            StringBuilder builder = new StringBuilder(_url + this.AssetWidth + "x" + this.AssetHeight + "/");

            if (!String.IsNullOrEmpty(this.Format))
                builder.Append(this.Format).Append("/");

            if (!String.IsNullOrEmpty(this.Background))
                builder.Append(this.Background).Append("/");

            if (!String.IsNullOrEmpty(this.Foreground))
                builder.Append(this.Foreground).Append("/");

            if (!String.IsNullOrEmpty(this.Text))
                builder.Append("&text=").Append(this.Text);

            this.Src = builder.ToString();
        }
    }
}
