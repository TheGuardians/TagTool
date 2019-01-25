using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Bitmaps.DDS
{
    [Flags]
    public enum DDSComplexityFlags : int
    {
        // Optional; must be used on any file that contains more than one surface (a mipmap, a cubic environment map, or mipmapped volume texture).
        Complex = 0x00000008,

        // Optional; should be used for a mipmap.
        MipMap = 0x00400000,

        // Required
        Texture = 0x00001000
    }
}
