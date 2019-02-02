using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Bitmaps.DDS
{
    [Flags]
    public enum DDSFlags : int
    {
        // ddsCaps field is valid.
        Caps = 0x00000001,     // default

        // dwHeight field is valid.
        Height = 0x00000002,

        // dwWidth field is valid.
        Width = 0x00000004,

        // ;Pitch is valid.
        Pitch = 0x00000008,

        // dwBackBufferCount is valid.
        BackBufferCount = 0x00000020,

        // dwZBufferBitDepth is valid.  (shouldnt be used in DDSURFACEDESC2)
        ZBufferBitDepth = 0x00000040,

        // dwAlphaBitDepth is valid.
        AlphaBitDepth = 0x00000080,

        // lpSurface is valid.
        LPSurface = 0x00000800,

        // ddpfPixelFormat is valid.
        PixelFormat = 0x00001000,

        // ddckCKDestOverlay is valid./
        CKDEstOverlay = 0x00002000,

        // ddckCKDestBlt is valid.
        CKDestBlt = 0x00004000,

        // ddckCKSrcOverlay is valid.
        CKSrcOverlay = 0x00008000,

        // ddckCKSrcBlt is valid.
        CKSrcBlt = 0x00010000,

        // dwMipMapCount is valid.
        MipMapCount = 0x00020000,

        // dwRefreshRate is valid
        RefreshRate = 0x00040000,

        // dwLinearSize is valid
        LinearSize = 0x00080000,

        // dwTextureStage is valid
        TextureStage = 0x00100000,

        // dwFVF is valid
        FVF = 0x00200000,

        // dwSrcVBHandle is valid
        SrcVBHandle = 0x00400000,

        // dwDepth is valid
        Depth = 0x00800000,

        // All input fields are valid.
        All = 0x00FFF9EE
    }
}
