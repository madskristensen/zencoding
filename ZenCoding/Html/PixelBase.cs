using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ZenCoding
{
    public abstract class PixelBase : CustomHtmlImage
    {
        private static readonly Regex _numberExtractor = new Regex("[^\\d]", RegexOptions.Compiled);

        public int AssetWidth { get; set; }
        public int AssetHeight { get; set; }

        protected PixelBase()
        {
            Initialize();
        }

        protected abstract void SetPath();
        protected abstract void Initialize();
        public abstract void Generate(string pixText);

        protected void SetDimensions(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return;

            var dimensions = _numberExtractor.Split(slug);

            if (ValidateIntegers(dimensions))
            {
                this.AssetWidth = int.Parse(dimensions[0], CultureInfo.CurrentCulture);

                if (dimensions.Length == 1)
                    this.AssetHeight = int.Parse(dimensions[0], CultureInfo.CurrentCulture);
                else
                    this.AssetHeight = int.Parse(dimensions[1], CultureInfo.CurrentCulture);
            }
        }

        private static bool ValidateIntegers(params string[] values)
        {
            foreach (var value in values)
            {
                try
                {
                    Convert.ToInt32(value, CultureInfo.CurrentCulture);
                }
                catch (AggregateException)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
