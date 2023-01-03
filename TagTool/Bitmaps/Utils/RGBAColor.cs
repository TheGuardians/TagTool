using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Bitmaps
{
    public struct RGBAColor
    {
        public byte R, G, B, A;

        public RGBAColor(byte Red, byte Green, byte Blue, byte Alpha)
        {
            R = Red;
            G = Green;
            B = Blue;
            A = Alpha;
        }
    }
}
