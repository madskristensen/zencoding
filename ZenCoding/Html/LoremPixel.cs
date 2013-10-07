﻿using System;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    public class LoremPixel : HtmlImage
    {
        private static readonly string _url = "http://lorempixel.com/";
        private static readonly string[] _categories = new string[] { "abstract", "animals", "business", "cats", "city", "food", "nightlife", "fashion", "people", "nature", "sports", "technics", "transport" };

        public LoremPixel(string loremPixelText)
        {
            this.AssetWidth = this.AssetHeight = 30;
            this.Category = "";
            this.Text = "";
            this.IsGray = false;

            string dimensions = "", category = "", text = "";
            string[] parts;

            parts = loremPixelText.Split('-');

            try
            {
                if (parts.Length > 1)
                {
                    if (parts[1].Equals("g"))
                    {
                        this.IsGray = true;
                        dimensions = parts[2];
                    }
                    else
                    {
                        dimensions = parts[1];
                    }

                    string[] components = ExtractNumbers(dimensions);

                    if (IsInteger(components[0]) && IsInteger(components[1]))
                    {
                        this.AssetWidth = int.Parse(components[0]);
                        this.AssetHeight = int.Parse(components[1]);

                        if (parts.Length > 2)
                        {
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
                    }
                }
            }
            catch
            {

            }

            this.AssetWidth = this.AssetWidth % 1921;
            this.AssetHeight = this.AssetHeight % 1921;

            this.Src = GetLoremPixelPath();
        }

        private string GetLoremPixelPath()
        {
            Random random = new Random();
            int randomInt = random.Next(0, 10);
            StringBuilder builder = new StringBuilder(_url);
            if (this.IsGray)
            {
                builder.Append("g/");
            }
            builder.Append(this.AssetWidth).Append("/").Append(this.AssetHeight).Append("/");
            if (!String.IsNullOrEmpty(this.Category))
            {
                builder.Append(this.Category).Append("/").Append(randomInt).Append("/");
                if (!String.IsNullOrEmpty(this.Text))
                {
                    builder.Append(this.Text).Append("/");
                }
            }
            return builder.ToString();
        }

        public int AssetWidth { get; set; }

        public int AssetHeight { get; set; }

        public string Category { get; set; }

        public string Text { get; set; }

        public bool IsGray { get; set; }

        private string[] ExtractNumbers(string theValue)
        {
            return System.Text.RegularExpressions.Regex.Split(theValue, "[^\\d]");
        }

        public static bool IsInteger(string theValue)
        {
            try
            {
                Convert.ToInt32(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
