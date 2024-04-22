using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Tags.Definitions.Gen2
{
    // This is a place holder for now to make the generated definitions compile
    [TagStructure(Size=0x20)]
    public class VertexBuffer
    {
        [TagField(Length = 0x20)]
        public byte[] Data;
    }
}
