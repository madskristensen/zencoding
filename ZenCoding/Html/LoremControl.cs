using System.Linq;

namespace ZenCoding
{
    public class LoremControl : EmptyHtmlControl
    {
        private const string _raw = "lorem ipsum dolor sit amet, consectetur adipiscing elit fusce vel sapien elit in malesuada semper mi, id sollicitudin urna fermentum ut fusce varius nisl ac ipsum gravida vel pretium tellus tincidunt integer eu augue augue nunc elit dolor, luctus placerat scelerisque euismod, iaculis eu lacus nunc mi elit, vehicula ut laoreet ac, aliquam sit amet justo nunc tempor, metus vel placerat suscipit, orci nisl iaculis eros, a tincidunt nisi odio eget lorem nulla condimentum tempor mattis ut vitae feugiat augue cras ut metus a risus iaculis scelerisque eu ac ante fusce non varius purus aenean nec magna felis fusce vestibulum velit mollis odio sollicitudin lacinia aliquam posuere, sapien elementum lobortis tincidunt, turpis dui ornare nisl, sollicitudin interdum turpis nunc eget sem nulla eu ultricies orci praesent id augue nec lorem pretium congue sit amet ac nunc fusce iaculis lorem eu diam hendrerit at mattis purus dignissim vivamus mauris tellus, fringilla vel dapibus a, blandit quis erat vivamus elementum aliquam luctus etiam fringilla pretium sem vitae sodales mauris id nulla est praesent laoreet, metus vel auctor aliquam, eros purus vulputate leo, eget consequat neque quam id tellus duis ultricies tempor tortor, vitae dignissim ligula mattis nec in hac habitasse platea dictumst ut arcu enim, dictum quis ultrices id, sagittis eget nulla sed nunc mi, congue ut ultricies ac, varius a eros donec porttitor, libero fermentum fringilla laoreet, eros arcu sodales ante, ut dictum risus lectus vel quam integer ultricies, nunc eget elementum euismod, orci enim vestibulum orci, nec suscipit urna odio et tellus suspendisse suscipit orci sit amet sem venenatis nec lobortis sem suscipit nullam nec imperdiet velit mauris eu nisi a felis imperdiet porta at ac nulla vivamus faucibus felis nec dolor pretium eget pellentesque dolor suscipit maecenas vitae enim arcu, at tincidunt nunc pellentesque eleifend vulputate lacus, vel semper sem ornare sit amet proin sem sapien, auctor vel faucibus id, aliquet vitae ipsum etiam auctor ultricies ante, at dapibus urna viverra sed nullam mi arcu, tempor vitae interdum a, sodales non urna vestibulum dignissim auctor mauris, ac elementum purus fermentum vitae duis placerat laoreet risus, sit amet eleifend tellus lobortis non vivamus iaculis dapibus leo a ornare cras vel sem at felis convallis mollis posuere ultrices dolor sed tellus arcu, accumsan a consectetur sit amet, volutpat eget lorem phasellus quis ipsum orci integer sodales tincidunt nibh a elementum ut ac libero nec orci euismod euismod nec at nulla duis malesuada faucibus porta aliquam nec consequat eros sed porttitor placerat dolor, accumsan imperdiet neque ornare in aenean non elit non leo porta mattis mauris non dolor nunc, id congue odio donec tellus nisl, semper id consectetur vitae, dapibus dictum nisl morbi sed augue purus sed dictum diam convallis tortor interdum volutpat phasellus dapibus arcu sit amet neque vulputate sed elementum orci fringilla in hac habitasse platea dictumst maecenas ut dui diam curabitur adipiscing vestibulum libero, nec varius dui pulvinar eget vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; aliquam dui neque, varius eu laoreet vel";
        private static readonly string[] _array = _raw.Split(' ');
        private int _length = 30;

        public LoremControl(string loremText)
        {
            int length = 30;

            if (int.TryParse(loremText.Replace("lorem", string.Empty), out length))
            {
                _length = length;
            }

            this.InnerText = GetLoremText(_length);
            Length = _length;
        }

        public int Length { get; set; }

        private static string GetLoremText(int words)
        {
            string result = string.Join(" ", _array.Take(words));
            return Normalize(result);
        }

        public string Generate(int count)
        {
            var clean = _array.Skip(Length * (count + 1));

            if (Length > _array.Length)
            {
                clean = _array;
            }

            string result = string.Join(" ", clean.Take(Length)).Trim();

            return Normalize(result);
        }

        private static string Normalize(string result)
        {
            if (result.Length > 0)
            {
                string first = result.Substring(0, 1).ToUpperInvariant();
                result = first + result.Substring(1);
                result = result.TrimEnd(',', ';');

                if (!result.EndsWith("."))
                    result += ".";
            }

            return result;
        }
    }
}
