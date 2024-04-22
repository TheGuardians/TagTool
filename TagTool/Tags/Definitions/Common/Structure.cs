using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Tags.Definitions.Common
{
    [TagStructure(Size = 0x18)]
    public class StructureManifestBuildIdentifier : TagStructure
    {
        public int ManifestId0;
        public int ManifestId1;
        public int ManifestId2;
        public int ManifestId3;
        public int BuildIndex;
        public int StructureImporterVersion;
    }
}
