using System;
using System.Text;

namespace TagTool.Cache.Monolithic
{
    public class StringBuffer
    {
        private byte[] _data;

        public StringBuffer(byte[] data)
        {
            _data = data;
        }

        public string GetString(int offset)
        {
            var builder = new StringBuilder();
            int i = 0;
            while (true)
            {
                if (_data[i + offset] == '\0')
                    break;
                builder.Append(Convert.ToChar(_data[i + offset]));
                i++;
            }
            return builder.ToString();
        }
    }
}
