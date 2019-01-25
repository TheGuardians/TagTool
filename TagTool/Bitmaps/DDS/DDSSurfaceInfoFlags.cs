using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Bitmaps.DDS
{
    [Flags]
    public enum DDSSurfaceInfoFlags
    {
        // Required for a cube map.
        CubeMap = 0x00000200,

        // Required when these surfaces are stored in a cube map.
        CubeMapPositiveX = 0x00000400,

        // Required when these surfaces are stored in a cube map.
        CubeMapNegativeX = 0x00000800,

        // Required when these surfaces are stored in a cube map.
        CubeMapPositiveY = 0x00001000,

        // Required when these surfaces are stored in a cube map.
        CubeMapNegativeY = 0x00002000,

        // Required when these surfaces are stored in a cube map.
        CubeMapPositiveZ = 0x00004000,

        // Required when these surfaces are stored in a cube map.
        CubeMapNegativeZ = 0x00008000,

        // Bitwise OR of all cubemap side flags + CubeMap flag.
        CubeMapAllFaces = 0x0000FE00,

        // Required for a volume texture.
        Volume = 0x00200000
    }
}
