using System;
using System.Linq;
using System.Text;

namespace ZenCoding
{
    public class LoremPixel : PixelBase
    {
        private const string _url = "http://lorempixel.com/";
        private static readonly string[] _categories = new string[] { "abstract", "animals", "business", "cats", "city", "food", "nightlife", "fashion", "people", "nature", "sports", "technics", "transport" };

        public string Category { get; set; }
        public string Text { get; set; }
        public bool IsGray { get; set; }

        public LoremPixel(string pixText)
        {
            Generate(pixText);
        }

        protected override void Initialize()
        {
            this.Category = "";
            this.Text = "";
            this.IsGray = false;
        }

        protected override void ProcessParts(string pixText)
        {
            string[] parts = pixText.Split('-');

            if (parts.Length < 2)
                return;

            string dimensions = "", category = "", text = "";

            if (parts[1].Equals("g"))
            {
                this.IsGray = true;

                if (parts.Length > 2)
                    dimensions = parts[2];
            }
            else
            {
                dimensions = parts[1];
            }

            SetDimensions(dimensions);

            if (parts.Length < 3)
                return;

            if (this.IsGray)
            {
                category = parts[3];
                if (parts.Length > 4)
                {
                    text = parts[4];
                }
            }
            else
            {
                category = parts[2];
                if (parts.Length > 3)
                {
                    text = parts[3];
                }
            }
            if (_categories.Contains(category))
            {
                this.Category = category;
                this.Text = text;
            }
        }

        protected override void SetPath()
        {
            Random random = new Random();
            int randomInt = random.Next(0, 10);
            StringBuilder builder = new StringBuilder(_url);

            if (this.IsGray)
                builder.Append("g/");

            builder.Append(Width % 1921).Append("/").Append(Height % 1921).Append("/");

            if (!String.IsNullOrEmpty(this.Category))
            {
                builder.Append(this.Category).Append("/").Append(randomInt).Append("/");

                if (!String.IsNullOrEmpty(this.Text))
                    builder.Append(this.Text).Append("/");
            }

            this.Src = builder.ToString();
        }
    }
}
