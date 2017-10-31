using System.Text;

namespace TagTool.Layouts
{
    /// <summary>
    /// Provides utility functions for working with naming conventions.
    /// </summary>
    public static class NamingConvention
    {
        /// <summary>
        /// Converts a name to pascal case, where words always begin with a capital letter.
        /// </summary>
        /// <param name="name">The name to convert.</param>
        /// <returns>The name converted to pascal case.</returns>
        public static string ToPascalCase(string name)
        {
            var result = new StringBuilder();
            var uppercase = true;
            foreach (var ch in name)
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
    }
}
