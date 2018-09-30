using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders.ShaderMatching
{

    class ShaderTemplateItem
    {
        public int rmt2TagIndex;
        public int rmdfValuesMatchingCount = 0;
        public int mapsCountEd;
        public int argsCountEd;
        public int mapsCountBm;
        public int argsCountBm;
        public int mapsCommon;
        public int argsCommon;
        public int mapsUncommon;
        public int argsUncommon;
        public int mapsMissing;
        public int argsMissing;
    }

    [TagStructure(Name = "render_method_template", Tag = "rmt2")]
    public class RenderMethodTemplateFast : TagStructure // used to deserialize as fast as possible
    {
        public CachedTagInstance VertexShader;
        public CachedTagInstance PixelShader;

        [TagField(Length = 10)]
        public uint[] Unknown00;

        public List<Argument> Arguments;

        [TagField(Length = 6)]
        public uint[] Unknown02;

        public List<ShaderMap> ShaderMaps;

        [TagStructure]
        public class Argument : TagStructure
        {
            public StringId Name;
        }

        [TagStructure]
        public class ShaderMap : TagStructure
        {
            public StringId Name;
        }
    }

    [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x20)]
    public class RenderMethodFast : TagStructure
    {
        public CachedTagInstance BaseRenderMethod;
        public List<RenderMethod.RenderMethodDefinitionOptionIndex> Unknown;
    }
}
