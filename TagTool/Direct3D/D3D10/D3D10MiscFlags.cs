using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.BDirect3D.D3D10
{
    /// <summary>
    /// Miscellaneous D3D10 flags.
    /// </summary>
    [Flags]
    public enum D3D10MiscFlags
    {
        None = 0,

        /// <summary>
        /// Indicates that a 2D texture is also a cubemap.
        /// </summary>
        TextureCube = 4
    }
}
