using System;
using System.Collections.Generic;
using TagTool.Tags;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x50, MaxVersion = Cache.CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x58, MinVersion = Cache.CacheVersion.HaloReach)]
    public class PixelShaderBlock : TagStructure
	{
        [TagField(MinVersion = Cache.CacheVersion.HaloReach)]
        public CompiledShaderFlags Flags;

        public byte[] XboxShaderBytecode;
        public byte[] PCShaderBytecode;

        public List<ShaderParameter> XboxParameters;
        public ShaderType XboxShaderType;
        [TagField(Flags = TagFieldFlags.Padding, Length = 0x3)]
        public byte[] Padding0;

        public List<ShaderParameter> PCParameters;
        public ShaderType PCShaderType;
        [TagField(Flags = TagFieldFlags.Padding, Length = 0x3)]
        public byte[] Padding1;

        public uint Gprs;

        [TagField(MinVersion = Cache.CacheVersion.HaloReach)]
        public int GlobalCachePixelShaderIndex;

        public PixelShaderReference XboxShaderReference;

        [Flags]
        public enum CompiledShaderFlags : uint
        {
            None = 0,
            RequiresConstantTable = 1 << 0,
        }
    }
}
