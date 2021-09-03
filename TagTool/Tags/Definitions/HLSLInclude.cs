using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "hlsl_include", Tag = "hlsl", Size = 0x14)]
    public class HlslInclude : TagStructure
    {
        public byte[] IncludeFile;
    }
}
