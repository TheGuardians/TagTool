using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "tag_package_manifest", Tag = "pach", Size = 0xC)]
    public class TagPackageManifest : TagStructure
    {
        public List<TagPatchBlockStruct> Patches;
        
        [TagStructure(Size = 0x20)]
        public class TagPatchBlockStruct : TagStructure
        {
            public CachedTag NewTag;
            public CachedTag TagToOverride;
        }
    }
}
