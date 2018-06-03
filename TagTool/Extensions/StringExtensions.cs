using System.Text;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a name to pascal case, where words always begin with a capital letter.
        /// </summary>
        /// <param name="str">The string to convert.</param>
        /// <returns>The string converted to pascal case.</returns>
        public static string ToPascalCase(this string str)
        {
            var result = new StringBuilder();
            var uppercase = true;
            foreach (var ch in str)
            {
                if (!char.IsLetter(ch))
                {
                    // Reset the uppercase state on non-alpha characters
                    uppercase = true;
                }
                if (!char.IsLetterOrDigit(ch))
                {
                    // Ignore non-alphanumeric characters
                    continue;
                }
                if (result.Length == 0 && char.IsDigit(ch))
                {
                    // Prepend an underscore if the name begins with a digit
                    result.Append('_');
                }
                result.Append(uppercase ? char.ToUpperInvariant(ch) : char.ToLowerInvariant(ch));
                uppercase = false;
            }
            return result.ToString();
        }

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
            var first = true;
            var prevUpper = false;
            var result = "";

            foreach (var c in str)
            {
                if (char.IsUpper(c))
                {
                    result += first ? c.ToString() : prevUpper ? c.ToString() : $" {c}";
                    prevUpper = true;
                    first = false;
                }
                else if (first)
                {
                    result += char.ToUpper(c);
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

        public static bool TrySplit(this string str, char separator, out string[] result)
        {
            if (!str.Contains(separator.ToString()))
            {
                result = null;
                return false;
            }

            result = str.Split(separator);
            return true;
        }
    }
}
