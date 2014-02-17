using System;
using System.Linq;
using System.Text;

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
            this.Format = "";
            this.Text = "";
        }

        protected override void ProcessParts(string pixText)
        {
            string[] parts = pixText.Split('-');

            if (parts.Length < 2)
                return;

            SetDimensions(parts[1]);

            if (parts.Length < 3)
                return;

            if (_formats.Contains(parts[2]))
                this.Format = parts[2];
            else
                SetTextAndColors(parts[2]);

            if (parts.Length > 3)
                SetTextAndColors(parts[3]);

            if (parts.Length > 4)
                SetTextAndColors(parts[4]);

            if (pixText.Last() == '-')
                this.Text += '-';
        }

        private void SetTextAndColors(string slug)
        {
            if (string.IsNullOrEmpty(this.Text) && slug.Contains("="))
                this.Text = string.Join("=", slug.Split('=').Skip(1));
            else if ((slug.Length == 3 || slug.Length == 6) && slug.All(c => c.IsHex()))
                if (string.IsNullOrEmpty(this.Background))
                    this.Background = slug;
                else
                    this.Foreground = slug;
        }

        protected override void SetPath()
        {
            StringBuilder builder = new StringBuilder(_url).Append(Width % 5480).Append("x").Append(Height % 2921).Append("/");

            if (!String.IsNullOrEmpty(this.Format))
                builder.Append(this.Format).Append("/");

            if (!String.IsNullOrEmpty(this.Background))
                builder.Append(this.Background).Append("/");

            if (!String.IsNullOrEmpty(this.Foreground))
                builder.Append(this.Foreground).Append("/");

            if (!String.IsNullOrEmpty(this.Text))
                builder.Remove(builder.Length - 1, 1).Append("&text=").Append(this.Text);

            this.Src = builder.ToString();
        }
    }
}
