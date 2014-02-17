using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    public class HtmlParser : IZenParser
    {
        private static char[] _attr = new[] { '#', '.', '[', '{' };
        private static char[] _elem = new[] { '>', '+', '^' };
        private static Regex _shortcuts = new Regex(@"^([\w]+):([\w]+)$", RegexOptions.Compiled);
        private static Regex _emptyingComponent = new Regex("{([^}]+)}", RegexOptions.Compiled);
        private static Regex _collapseMultipleLinefeeds = new Regex("([" + Environment.NewLine + "]+)", RegexOptions.Compiled);

        public static bool IsValid(string zenSyntax)
        {
            if (string.IsNullOrEmpty(zenSyntax) || zenSyntax.StartsWith("asp:", StringComparison.OrdinalIgnoreCase))
                return false;

            int indexSpace = zenSyntax.IndexOf(' ');

            if (indexSpace > -1 && (indexSpace < zenSyntax.IndexOfAny(new[] { '[', '{', '"' }) || indexSpace > zenSyntax.LastIndexOfAny(new[] { ']', '}', '"' })))
                return false;

            if ((zenSyntax.Contains("{") || zenSyntax.Contains("}")) && (zenSyntax.Count(c => c == '{') != zenSyntax.Count(c => c == '}')))
                return false;

            if (zenSyntax.Contains("<") || zenSyntax.Contains("|") || zenSyntax.Contains("@"))
                return false;

            if (!zenSyntax.StartsWith("place", StringComparison.CurrentCultureIgnoreCase))
            {
                char last = zenSyntax.Last();

                if (!char.IsLetterOrDigit(last) && last != ']' && last != '}' && last != '+' && !char.IsWhiteSpace(last))
                    return false;
            }

            if (zenSyntax.Count(z => z == ']') != zenSyntax.Count(z => z == '['))
                return false;

            return true;
        }

        public string Parse(string zenSyntax)
        {
            int groupId = 0;
            int groupIdx;
            var resolvedGroups = new List<string>();
            while ((groupIdx = zenSyntax.IndexOf('(')) != -1)
            {
                //Find the end of the group
                int stackBase;
                int i;
                for (i = groupIdx + 1, stackBase = 1; i < zenSyntax.Length && stackBase != 0; ++i)
                    switch (zenSyntax[i])
                    {
                        case '(':
                            stackBase++;
                            break;
                        case ')':
                            stackBase--;
                            break;
                    }

                //Bad formatting (could not match the group ending)
                if (stackBase != 0)
                    return string.Empty;

                //Remove the grouping chars and parse it recursively (enabling subgroups)
                string group = zenSyntax.Substring(groupIdx, i - groupIdx);
                string parsedGroup = Parse(group.Substring(1, group.Length - 2));

                //Add the parsed group to a list for last composition
                if (!resolvedGroups.Contains(parsedGroup))
                    resolvedGroups.Add(parsedGroup);

                //Replace the group with a "valid" element marked with the group id
                zenSyntax = zenSyntax.Replace(group, "span.__group" + groupId++);
            }

            //If there are no more groups
            string result = ParseGroup(zenSyntax);

            // Remove temporary span's end tags
            if (groupId - 1 >= 0)
                result = result.Replace("</span>", String.Empty);

            //Place the parsed groups on their respective positions
            for (--groupId; groupId >= 0; --groupId)
                result = result.Replace(string.Format(CultureInfo.CurrentCulture, "<span class=\"__group{0}\">", groupId), resolvedGroups[groupId]);

            return result;
        }

        public static string ParseGroup(string zenSyntax)
        {
            if (!IsValid(zenSyntax))
                return string.Empty;

            Control root = null;

            try
            {
                root = new Control();
                List<string> parts = GetSubParts(zenSyntax, _elem);

                AdjustImplicitTagNames(parts);

                if (!IsValidHtmlElements(parts))
                    return string.Empty;

                IEnumerable<Control> current = new Control[] { root };

                HandleDoctypes(ref root, parts, ref current);

                if (root == null) return null;

                BuildControlTree(CloneStack<string>(parts), current.First(), -1);

                return RenderControl(root);
            }
            finally
            {
                if (root != null)
                {
                    root.Dispose();
                }
            }
        }

        private static void HandleDoctypes(ref Control root, List<string> parts, ref IEnumerable<Control> current)
        {
            if (parts[0] == "html:4t" || parts[0] == "html:4s" || parts[0] == "html:xt" || parts[0] == "html:xs" || parts[0] == "html:xxs" || parts[0] == "html:5")
            {
                root = HtmlElementFactory.CreateDoctypes(parts[0], ref current);
                parts.RemoveAt(0);
            }
        }

        private static void AdjustImplicitTagNames(List<string> parts)
        {
            var currentDefault = "div";

            for (int i = 0; i < parts.Count; i++)
            {
                if (i != 0 && (parts[i - 1] == "ol" || parts[i - 1] == "ul") && parts[i][0] == '>')
                    currentDefault = "li";
                else if (i != 0 && parts[i - 1] == "em" && parts[i][0] == '>')
                    currentDefault = "span";
                else if (i != 0 && parts[i - 1] == "table" && parts[i][0] == '>')
                    currentDefault = "tr";
                else if (i != 0 && (parts[i - 1] == "tr" || parts[i - 1].StartsWith(">tr", StringComparison.CurrentCultureIgnoreCase)) && parts[i][0] == '>')
                    currentDefault = "td";
                else if ((parts[i][0] == '>' && currentDefault == "td") || (currentDefault != "div" && parts[i][0] == '^'))
                    currentDefault = "div";

                if (parts[i][0] == '#' || parts[i][0] == '.')
                {
                    parts[i] = currentDefault + parts[i];
                }
                else if (_elem.Contains(parts[i][0]) && (parts[i][1] == '#' || parts[i][1] == '.'))
                {
                    parts[i] = parts[i].Insert(1, currentDefault);
                }
            }
        }

        private static bool IsValidHtmlElements(List<string> parts)
        {
            foreach (string part in parts)
            {
                string firstElement = string.Empty;
                string clean = part.TrimStart(_elem);

                foreach (char c in clean)
                {
                    if (_attr.Contains(c) || c == '*' || c == ':')
                        break;

                    firstElement += c;
                }

                if (firstElement.Length > 0 &&
                    !ValidElements.List.Contains(firstElement) &&
                    !firstElement.StartsWith("lorem", StringComparison.Ordinal) &&
                    !firstElement.StartsWith("pix", StringComparison.Ordinal) &&
                    !firstElement.StartsWith("place", StringComparison.Ordinal) &&
                    firstElement != "h$")

                    return false;
            }

            return true;
        }

        private static void BuildControlTree(Stack<string> parts, Control control, int nestedCounter)
        {
            if (!parts.Any())
                return;

            string name;
            var part = parts.Pop();
            int count = GetCountAndName(part, out name, _attr);
            var htmlControl = GenerateElement(part, name);
            bool foundMaintainLevelSemaphore = false;
            bool foundLevelUpSemaphore = false;

            for (int i = 0; i < count; ++i)
            {
                var clone = htmlControl.CloneElement(count > 1 || nestedCounter == -1 ? i : nestedCounter);
                clone.SkinID = Guid.NewGuid().ToString();
                control.Controls.Add(clone);

                if (parts.Any() && parts.Peek()[0] == '+')
                {
                    foundMaintainLevelSemaphore = true;
                }
                else if (parts.Any() && parts.Peek()[0] == '^')
                {
                    foundLevelUpSemaphore = true;
                }
                else // must be ">" meaning; going down!
                {
                    if (count > 1) nestedCounter = i;

                    BuildControlTree(CloneStack<string>(parts), FindControlBySkinId(control, clone.SkinID), nestedCounter);

                    nestedCounter = -1;
                }
            }

            if (foundMaintainLevelSemaphore)
            {
                BuildControlTree(CloneStack<string>(parts), control, nestedCounter);
            }
            else if (foundLevelUpSemaphore)
            {
                Control parent = control;

                for (int j = 0; j < parts.Peek().Count(a => a == '^'); j++)
                {
                    if (parent.Parent == null)
                        break;

                    parent = parent.Parent;
                }

                BuildControlTree(CloneStack<string>(parts), parent, nestedCounter);
            }

            nestedCounter = -1;
        }

        private static Stack<T> CloneStack<T>(IEnumerable<T> collection)
        {
            return new Stack<T>(collection.Reverse<T>());
        }

        private static Control FindControlBySkinId(Control root, string id)
        {
            if (root.SkinID == id) return root;

            foreach (Control c in root.Controls)
            {
                Control t = FindControlBySkinId(c, id);

                if (t != null) return t;
            }

            return null;
        }

        private static HtmlControl GenerateElement(string part, string name)
        {
            HtmlControl element = null;

            if (!_shortcuts.IsMatch(name))
            {
                element = CreateElementWithAttributes(part, name);
            }
            else
            {
                element = ShortcutHelper.Parse(part);
            }
            return element;
        }

        private static HtmlControl CreateElementWithAttributes(string part, string name)
        {
            using (HtmlControl element = HtmlElementFactory.Create(name))
            {
                List<string> subParts = GetSubParts(part, _attr);

                foreach (string subPart in subParts)
                {
                    // Class
                    if (subPart[0] == '.')
                    {
                        AddClass(element, subPart);
                    }
                    // ID
                    else if (subPart[0] == '#')
                    {
                        AddId(element, subPart);
                    }
                    else if (subPart[0] == '[')
                    {
                        AddAttributes(element, subPart);
                    }
                    else if (subPart[0] == '{')
                    {
                        AddInnerText(element, subPart);
                    }
                }

                return element;
            }
        }

        private static void AddInnerText(HtmlControl element, string subPart)
        {
            string clean = subPart.Substring(1, subPart.IndexOf('}') - 1);

            LiteralControl lit = new LiteralControl(clean);
            element.Controls.Add(lit);

        }

        private static void AddAttributes(HtmlControl element, string attribute)
        {
            int start = attribute.IndexOf('[');
            int end = attribute.IndexOf(']');

            if (start > -1 && end > start)
            {
                string content = attribute.Substring(start + 1, end - start - 1);
                List<string> parts = content.Trim().Split(' ').ToList();

                for (int i = parts.Count - 1; i > 0; i--)
                {
                    string part = parts[i];
                    int singleCount = part.Count(c => c == '\'');
                    int doubleCount = part.Count(c => c == '"');

                    if (((singleCount > 1 || doubleCount > 1) && !part.Contains("=")) ||
                        ((doubleCount == 1) && part.EndsWith("\"", StringComparison.Ordinal)) ||
                        ((singleCount == 1) && part.EndsWith("'", StringComparison.Ordinal)))
                    {
                        parts[i - 1] += " " + part;
                        parts.RemoveAt(i);
                    }
                }

                foreach (string part in parts)
                {
                    string[] sides = part.Split('=');

                    if (sides.Length == 1)
                    {
                        element.Attributes[sides[0]] = string.Empty;
                    }
                    else
                    {
                        sides[1] = sides[1].Trim();
                        char firstChar = sides[1][0];
                        char lastChar = sides[1][sides[1].Length - 1];
                        if ((firstChar == '\'' || firstChar == '"') && firstChar == lastChar)
                        {
                            element.Attributes[sides[0]] = sides[1].Substring(1, sides[1].Length - 2);
                        }
                        else
                        {
                            element.Attributes[sides[0]] = sides[1];
                        }
                    }
                }
            }
        }

        private static void AddId(HtmlControl element, string part)
        {
            int index = part.IndexOf('*');
            string clean = part;

            if (index > 0)
            {
                clean = clean.Substring(0, index);
            }

            element.ID = clean.TrimStart(_attr);
        }

        private static void AddClass(HtmlControl element, string className)
        {
            string current = element.Attributes["class"];
            string clean = className.TrimStart(_attr);
            int index = clean.IndexOf('*');

            if (index > 0)
            {
                clean = clean.Substring(0, index);
            }

            element.Attributes["class"] = string.IsNullOrEmpty(current) ? clean : current += " " + clean;
            element.Attributes["class"] = element.Attributes["class"];
        }

        private static int GetCountAndName(string part, out string cleanPart, char[] symbols)
        {
            int index = part.IndexOf('*');
            int count = 1;

            if (index > -1 && part.Length > index + 1 && !char.IsNumber(part[index + 1]))
                index = -1;

            if (index > -1 && int.TryParse(part.Substring(index + 1), out count))
            {
                string[] subParts = part.Split(symbols, StringSplitOptions.RemoveEmptyEntries);
                cleanPart = subParts[0];

                if (index < cleanPart.Length)
                {
                    cleanPart = cleanPart.Substring(0, index);
                }

                cleanPart = cleanPart.Trim(_attr).Trim(_elem);
            }
            else
            {
                string[] subParts = part.Split(_attr, StringSplitOptions.RemoveEmptyEntries);
                cleanPart = subParts[0].TrimStart(_attr).Trim(_elem);
            }

            cleanPart = _emptyingComponent.Replace(cleanPart, string.Empty);

            return count;
        }

        private static List<string> GetSubParts(string zenSyntax, char[] symbols)
        {
            List<string> parts = GetParts(zenSyntax, symbols);

            for (int i = parts.Count - 1; i > 0; i--)
            {
                string part = parts[i];

                if ((part.Contains("]") && !part.Contains("[")) || (part.Contains("}") && !part.Contains("{")))
                {
                    parts[i - 1] += part;
                    parts.RemoveAt(i);
                }
            }

            return parts;
        }

        private static List<string> GetParts(string zenSyntax, char[] symbols)
        {
            List<string> parts = new List<string>();
            int index = 0;

            for (int i = 0; i < zenSyntax.Length; i++)
            {
                char c = zenSyntax[i];

                if (i > 0 && symbols.Contains(c))
                {
                    parts.Add(zenSyntax.Substring(index, i - index));
                    index = i;
                }
            }

            IEnumerable<string> final = new[] { zenSyntax.Substring(index) };

            if (final.ElementAt(0) == "+")
            {
                final = HandleTrailingPlus(parts.Last());
            }

            parts.AddRange(final);

            // Adjust for multiple ^ characters
            for (int i = parts.Count - 2; i >= 0; i--)
            {
                if (parts[i] == "^")
                {
                    parts[i] = '^' + parts[i + 1];
                    parts.RemoveAt(i + 1);
                }
            }

            return parts;
        }

        private static IEnumerable<string> HandleTrailingPlus(string last)
        {
            if (last == "ul" || last == "ol")
            {
                yield return ">li";
            }
            else if (last == "dl")
            {
                yield return ">dt";
                yield return "+dd";
            }
            else if (last == "tr")
            {
                yield return ">td";
            }
            else if (last == "map")
            {
                yield return ">area";
            }
            else if (last == "table")
            {
                yield return ">tr";
                yield return ">td";
            }
            else if (last == "select")
            {
                yield return ">option[value]";
            }
        }

        public static string RenderControl(Control control)
        {
            if (control == null)
                return null;

            using (StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture))
            using (XhtmlTextWriter htmlTextWriter = new XhtmlTextWriter(stringWriter))
            {
                control.RenderControl(htmlTextWriter);

                return _collapseMultipleLinefeeds.Replace(HttpUtility.HtmlDecode(stringWriter.ToString())
                                         .Trim(Environment.NewLine.ToArray()), Environment.NewLine); // Replace multiple linefeeds with single.
            }
        }
    }
}