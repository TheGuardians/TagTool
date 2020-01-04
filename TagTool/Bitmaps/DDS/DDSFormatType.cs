using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Bitmaps.DDS
{
    /// <summary>
    /// DDS format types.
    /// </summary>
    public enum DDSFormatType
    {
        /// <summary>
        /// The texture contains RGB data.
        /// </summary>
        RGB,

        /// <summary>
        /// The texture contains YUV data.
        /// </summary>
        YUV,

        /// <summary>
        /// The texture contains luminance data.
        /// </summary>
        Luminance,

        /// <summary>
        /// The texture only contains an alpha channel.
        /// </summary>
        Alpha,

        /// <summary>
        /// The format should be determined by the texture's FourCC code or the D3D10 format.
        /// </summary>
        Other
    }
}
