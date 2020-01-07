using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Direct3D.D3D9
{
    // TODO: fill the values using the resource definition and the bitmap tag definition.
    public enum D3DFormat : byte
    {
        D3DFMT_UNKNOWN = 0x0, // All A8, A8Y8, Y8, A8R8G8B8 are contained in this one...
        D3DFMT_DXT1 = 0x31,
        D3DFMT_ATI2 = 0x32,
        D3DFMT_DXT3 = 0x33,
        D3DFMT_DXT4 = 0x34,
        D3DFMT_DXT5 = 0x35
    }
}
