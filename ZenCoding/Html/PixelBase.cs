using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ZenCoding
{
    public abstract class PixelBase : CustomHtmlImage
    {
        private static readonly Regex _numberExtractor = new Regex("[^\\d]", RegexOptions.Compiled);

        protected abstract void SetPath();
        protected abstract void Initialize();
        protected abstract void ProcessParts(string pixText);

        protected PixelBase()
        {
            Width = Height = 30;
            Initialize();
        }

        public void Generate(string pixText)
        {
            ProcessParts(pixText);
            SetPath();
            Width = Height = -1;
        }

        protected void SetDimensions(string slug)
        {
            if (string.IsNullOrEmpty(slug))
                return;

            var dimensions = _numberExtractor.Split(slug);

            if (ValidateIntegers(dimensions))
            {
                Width = int.Parse(dimensions[0], CultureInfo.CurrentCulture);

                if (dimensions.Length == 1)
                    Height = Width;
                else
                    Height = int.Parse(dimensions[1], CultureInfo.CurrentCulture);
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
