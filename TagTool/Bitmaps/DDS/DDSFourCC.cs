using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Bitmaps.DDS
{
    public static class DDSFourCC
    {
        /// <summary>
        /// Creates a FourCC value from a string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>The FourCC value.</returns>
        public static uint FromString(string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            var result = 0U;
            for (var i = bytes.Length - 1; i >= 0; i--)
            {
                result <<= 8;
                result |= bytes[i];
            }
            return result;
        }
    }
}
