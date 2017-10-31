using System.Text;

namespace TagTool.Commands
{
    public static class StringUtils
    {
        public static string ToSnakeCase(this string str)
        {
            var prevUpper = true;
            var result = "";

            foreach (var c in str)
            {
                if (char.IsUpper(c))
                {
                    result += prevUpper ? char.ToLower(c).ToString() : $"_{char.ToLower(c)}";
                    prevUpper = true;
                }
                else
                {
                    result += c;
                    prevUpper = false;
                }
            }

            return result;
        }

        public static string ToSpaced(this string str)
        {
            var prevUpper = true;
            var result = "";

            foreach (var c in str)
            {
                if (char.IsUpper(c))
                {
                    result += prevUpper ? c.ToString() : $" {c}";
                    prevUpper = true;
                }
                else
                {
                    result += c;
                    prevUpper = false;
                }
            }

            return result;
        }

        public static string Unescape(this string str)
        {
            var result = new StringBuilder();
            var escape = false;
            foreach (var ch in str)
            {
                if (!escape)
                {
                    if (ch == '\\')
                        escape = true;
                    else
                        result.Append(ch);
                    continue;
                }
                escape = false;
                switch (ch)
                {
                    case 'n':
                        result.Append('\n');
                        break;
                    case 'r':
                        result.Append('\r');
                        break;
                    case 't':
                        result.Append('\t');
                        break;
                    case 'q':
                        result.Append('"');
                        break;
                    case '\\':
                        result.Append('\\');
                        break;
                    default:
                        result.Append(ch);
                        break;
                }
            }
            return result.ToString();
        }
    }
}