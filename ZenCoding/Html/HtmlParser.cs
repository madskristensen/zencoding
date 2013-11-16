using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ZenCoding
{
    public class HtmlParser
    {
        private static char[] _attr = new[] { '#', '.', '[', '{' };
        private static char[] _elem = new[] { '>', '+', '^' };
        private static Regex _shortcuts = new Regex(@"^([\w]+):([\w]+)$", RegexOptions.Compiled);

        public static bool IsValid(string zenSyntax)
        {
            if (zenSyntax.Length == 0 || zenSyntax.StartsWith("asp:", StringComparison.OrdinalIgnoreCase))
                return false;

            int indexSpace = zenSyntax.IndexOf(' ');
            if (indexSpace > -1 && (indexSpace < zenSyntax.IndexOfAny(new[] { '[', '{', '"' }) || indexSpace > zenSyntax.LastIndexOfAny(new[] { ']', '}', '"' })))
                return false;

            if (zenSyntax.Contains("{") || zenSyntax.Contains("}"))
            {
                if (zenSyntax.Count(c => c == '{') != zenSyntax.Count(c => c == '}'))
                    return false;
            }

            if (zenSyntax.Contains("<") || zenSyntax.Contains("|") || zenSyntax.Contains("@"))
                return false;

            char last = zenSyntax.Last();
            if (!char.IsLetterOrDigit(last) && last != ']' && last != '}' && last != '+' && !char.IsWhiteSpace(last))
                return false;

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
                result = result.Replace(string.Format("<span class=\"__group{0}\">", groupId), resolvedGroups[groupId]);

            return result;
        }

        public string ParseGroup(string zenSyntax)
        {
            if (!IsValid(zenSyntax))
                return string.Empty;

            Control root = new Control();
            List<string> parts = GetSubParts(zenSyntax, _elem);

            AdjustImplicitTagNames(parts);

            if (!IsValidHtmlElements(parts))
                return string.Empty;

            List<Control> current = new List<Control>() { root };

            HandleDoctypes(ref root, parts, ref current);
            BuildControlTree(parts, current);

            return RenderControl(root);
        }

        private static void HandleDoctypes(ref Control root, List<string> parts, ref List<Control> current)
        {
            if (parts[0] == "html:4t" || parts[0] == "html:4s" || parts[0] == "html:xt" || parts[0] == "html:xs" || parts[0] == "html:xxs" || parts[0] == "html:5")
            {
                root = HtmlElementFactory.CreateDoctypes(parts[0], ref current);
                parts.RemoveAt(0);
            }
        }

        private static void AdjustImplicitTagNames(List<string> parts)
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i][0] == '#' || parts[i][0] == '.')
                {
                    parts[i] = "div" + parts[i];
                }
                else if (_elem.Contains(parts[i][0]) && (parts[i][1] == '#' || parts[i][1] == '.'))
                {
                    parts[i] = parts[i].Insert(1, "div");
                }
            }
        }

        private bool IsValidHtmlElements(List<string> parts)
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
                    !firstElement.StartsWith("lorem") &&
                    !firstElement.StartsWith("pix") &&
                    firstElement != "h$")

                    return false;
            }

            return true;
        }

        private void BuildControlTree(List<string> parts, List<Control> current)
        {
            foreach (string part in parts)
            {
                string name;
                int count = GetCountAndName(part, out name, _attr);
                List<Control> list = new List<Control>();

                for (int i = 0; i < count; i++)
                {
                    HtmlControl element = GenerateElement(part, name, i);

                    for (int c = 0; c < current.Count; c++)
                    {
                        int increment = current.Count == 1 ? i : c;

                        Control control = current[c];

                        var clone = element.CloneElement(increment);

                        if (part[0] == '+')
                        {
                            control.Parent.Controls.Add(clone);
                            c = current.Count;
                        }
                        else if (part[0] == '^')
                        {
                            AdjustClimbUp(part, control, clone);
                            c = current.Count;
                        }
                        else
                        {
                            control.Controls.Add(clone);
                        }

                        list.Add(clone);
                    }
                }

                current = list;
            }
        }

        private static void AdjustClimbUp(string part, Control control, HtmlControl clone)
        {
            Control parent = control;
            int climbs = part.Count(a => a == '^');

            for (int i = 0; i <= climbs; i++)
            {
                if (parent.Parent == null)
                    break;

                parent = parent.Parent;
            }

            parent.Controls.Add(clone);
        }

        private HtmlControl GenerateElement(string part, string name, int i)
        {
            HtmlControl element = null;

            if (!_shortcuts.IsMatch(name))
            {
                element = CreateElementWithAttributes(part, name, i);
            }
            else
            {
                element = ShortcutHelper.Parse(part);
            }
            return element;
        }

        private HtmlControl CreateElementWithAttributes(string part, string name, int count)
        {
            HtmlControl element = HtmlElementFactory.Create(name);
            List<string> subParts = GetSubParts(part, _attr);

            foreach (string subPart in subParts)
            {
                // Class
                if (subPart[0] == '.')
                {
                    AddClass(element, subPart, count);
                }
                // ID
                else if (subPart[0] == '#')
                {
                    AddId(element, subPart, count);
                }
                else if (subPart[0] == '[')
                {
                    AddAttributes(element, subPart, count);
                }
                else if (subPart[0] == '{')
                {
                    AddInnerText(element, subPart, count);
                }
            }

            return element;
        }

        private void AddInnerText(HtmlControl element, string subPart, int i)
        {
            string clean = subPart.Substring(1, subPart.IndexOf('}') - 1);

            LiteralControl lit = new LiteralControl(clean);
            element.Controls.Add(lit);

        }

        private void AddAttributes(HtmlControl element, string attribute, int count)
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
                        ((doubleCount == 1) && part.EndsWith("\"")) ||
                        ((singleCount == 1) && part.EndsWith("'")))
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

        private void AddId(HtmlControl element, string part, int count)
        {
            int index = part.IndexOf('*');
            string clean = part;

            if (index > 0)
            {
                clean = clean.Substring(0, index);
            }

            element.ID = clean.TrimStart(_attr);
        }

        private void AddClass(HtmlControl element, string className, int count)
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

        private int GetCountAndName(string part, out string cleanPart, char[] symbols)
        {
            int index = part.IndexOf('*');
            int count = 1;

            if (index > -1 && part.Length > index && !char.IsNumber(part[index + 1]))
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

            cleanPart = Regex.Replace(cleanPart, "{([^}]+)}", string.Empty);

            return count;
        }

        private List<string> GetSubParts(string zenSyntax, char[] symbols)
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

        private List<string> GetParts(string zenSyntax, char[] symbols)
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
            using (StringWriter stringWriter = new StringWriter())
            using (XhtmlTextWriter htmlTextWriter = new XhtmlTextWriter(stringWriter))
            {
                control.RenderControl(htmlTextWriter);

                return Regex.Replace(HttpUtility.HtmlDecode(stringWriter.ToString())
                                         .Trim(Environment.NewLine.ToArray()),
                                     "([" + Environment.NewLine + "]+)", Environment.NewLine); // Replace multiple linefeeds with single.
            }
        }
    }
}