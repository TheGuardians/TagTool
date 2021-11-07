using System;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x90, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Size = 0x98, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)] // might not be correct
    [TagStructure(Size = 0x50, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Size = 0x58, MinVersion = CacheVersion.HaloReach)]
    public class PixelShaderBlock : TagStructure
	{
        [TagField(MinVersion = Cache.CacheVersion.HaloReach)]
        public CompiledShaderFlags Flags;

        // bytecode and constant tables are two separate arrays indexed by platform
        public byte[] XboxShaderBytecode;
        public byte[] PCShaderBytecode;
        [TagField(Platform = CachePlatform.MCC)]
        public byte[] DurangoShaderBytecode;

        public ShaderConstantTable XBoxConstantTable;
        public ShaderConstantTable PCConstantTable;
        [TagField(Platform = CachePlatform.MCC)]
        public ShaderConstantTable DurangoConstantTable;

        public uint Gprs;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int GlobalCachePixelShaderIndex;

        public PixelShaderReference XboxShaderReference;

        [Flags]
        public enum CompiledShaderFlags : uint
        {
            None = 0,
            RequiresConstantTable = 1 << 0,
        }

        public ShaderConstantTable GetConstantTable(CacheVersion version, CachePlatform platform)
        {
            if(CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, version) || platform == CachePlatform.MCC)
                return PCConstantTable;
            else
                return XBoxConstantTable;
        }

        public byte[] GetBytecode(CacheVersion version, CachePlatform platform)
        {
            if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, version) || platform == CachePlatform.MCC)
                return PCShaderBytecode;
            else
                return XboxShaderBytecode;
        }
    }
}
