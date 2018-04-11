using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x40)]
    public class RenderMethod
    {
        public CachedTagInstance BaseRenderMethod;
        public List<UnknownBlock> Unknown;
        public List<ImportDatum> ImportData;
        public List<ShaderProperty> ShaderProperties;
        public TagMapping.VariableTypeValue InputVariable;
        public TagMapping.VariableTypeValue RangeVariable;
        public TagMapping.OutputModifierValue OutputModifier;
        public TagMapping.VariableTypeValue OutputModifierInput;
        public float RuntimeMConstantValue;
        public ushort RuntimeMFlags;

        public TagMapping.ForceFlagsValue ForceFlags;

        [TagField(Padding = true, Length = 1)]
        public byte[] Unused;

        [TagStructure(Size = 0x2)]
        public class UnknownBlock
        {
            public short Unknown;
        }

        [TagStructure(Size = 0x3C)]
        public class ImportDatum
        {
            [TagField(Label = true)]
            public StringId MaterialType;
            public int Unknown;
            public CachedTagInstance Bitmap;
            public uint Unknown2;
            public int Unknown3;
            public short Unknown4;
            public short Unknown5;
            public short Unknown6;
            public short Unknown7;
            public short Unknown8;
            public short Unknown9;
            public uint Unknown10;
            public List<FunctionBlock> Functions;

            [TagStructure(Size = 0x24)]
            public class FunctionBlock
            {
                public int Unknown;
                [TagField(Label = true)]
                public StringId Name;
                public uint Unknown2;
                public uint Unknown3;
                public TagFunction Function = new TagFunction { Data = new byte[0] };
            }
        }

        [TagStructure(Size = 0x84)]
        public class ShaderProperty
        {
            public CachedTagInstance Template;
            public List<ShaderMap> ShaderMaps;
            public List<Argument> Arguments;
            public List<UnknownBlock1> Unknown;
            public uint Unknown2;
            public List<DrawMode> DrawModes;
            public List<UnknownBlock3> Unknown3;
            public List<ArgumentMapping> ArgumentMappings;
            public List<FunctionBlock> Functions;
            public int BitmapTransparency;
            public int Unknown7;
            public uint Unknown8;
            public short Unknown9;
            public short Unknown10;
            public short Unknown11;
            public short Unknown12;
            public short Unknown13;
            public short Unknown14;
            public short Unknown15;
            public short Unknown16;

            [TagStructure(Size = 0x18)]
            public class ShaderMap
            {
                [TagField(Label = true)]
                public CachedTagInstance Bitmap;
                public sbyte Unknown;
                public sbyte BitmapIndex;
                public sbyte Unknown2;
                public byte BitmapFlags;
                public sbyte UnknownBitmapIndexEnable;
                public sbyte UvArgumentIndex;
                public sbyte Unknown3;
                public sbyte Unknown4;
            }

            [TagStructure(Size = 0x10)]
            public class Argument
            {
                public float Arg1;
                public float Arg2;
                public float Arg3;
                public float Arg4;
            }

            [TagStructure(Size = 0x4)]
            public class UnknownBlock1
            {
                public uint Unknown;
            }

            [TagStructure(Size = 0x2)]
            public class DrawMode
            {
                public ushort DataHandle;
            }

            [TagStructure(Size = 0x6)]
            public class UnknownBlock3
            {
                public short DataHandleSampler;
                public short DataHandleUnknown;
                public short DataHandleVector;
            }

            [TagStructure(Size = 0x4)]
            public class ArgumentMapping
            {
                public short RegisterIndex;
                public byte FunctionIndex;
                public byte ArgumentIndex;
            }

            [TagStructure(Size = 0x24)]
            public class FunctionBlock
            {
                public int Unknown;
                [TagField(Label = true)]
                public StringId Name;
                public uint Unknown2;
                public uint Unknown3;
                public TagFunction Function = new TagFunction { Data = new byte[0] };
            }
        }
    }
}