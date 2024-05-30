using System;
using System.Collections.Generic;

namespace TagTool.Commands.Porting.Gen2
{
    public enum Platform
    {
        Xbox,
        Vista,
        MCC,
        All
    }

    public enum ShaderType
    {
        Shader,
        ShaderCustom,
        ShaderDecal,
        ShaderFoliage,
        ShaderHalogram,
        ShaderScreen,
        ShaderTerrain,
        ShaderWater
    }

    public class Gen2ShaderTemplateMapBase
    {
        public string Name { get; }
        public ShaderType Type { get; set; }

        // Use Tuple for properties: (int index, string value, Platform platform)
        public List<(Platform platform, int Index, string Value)> Bitmaps { get; }
        public List<(Platform platform, int Index, string Value)> PixelConstants { get; }
        public List<(Platform platform, int Index, string Value)> VertexConstants { get; }

        public Gen2ShaderTemplateMapBase(string name)
        {
            Name = name;
            Bitmaps = new List<(Platform platform, int Index, string Value)>();
            PixelConstants = new List<(Platform platform, int Index, string Value)>();
            VertexConstants = new List<(Platform platform, int Index, string Value)>();
        }
    }
}
