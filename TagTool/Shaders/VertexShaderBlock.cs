
using System;
using System.Collections.Generic;
﻿using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Shaders
{
    [TagStructure(Size = 0x90, Align = 0x8, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Size = 0x98, Align = 0x8, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)] // might not be correct
    [TagStructure(Size = 0x50, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x58, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
    public class VertexShaderBlock : TagStructure
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
        public int GlobalCachePixelShaderIndex; // pixl only

        public VertexShaderReference XboxShaderReference;

        [Flags]
        public enum CompiledShaderFlags : uint
        {
            None = 0,
            RequiresConstantTable = 1 << 0,
        }

        public ShaderConstantTable GetConstantTable(CacheVersion version, CachePlatform platform)
        {
            if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, version) || platform == CachePlatform.MCC)
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
