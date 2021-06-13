using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "planar_fog_parameters", Tag = "pfpt", Size = 0x88)]
    public class PlanarFogParameters : TagStructure
    {
        public PlanarFogFlags Flags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public float FogThickness00To10;
        public float PerVertexFogThicknessModulation; // (only for transparents)
        public float FullFogDepth; // world units
        public RealRgbColor FogColor;
        public float FogColorIntensity;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag PaletteTexture;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag PatchyTexture;
        public RealRgbColor PatchyColor;
        public float PatchyColorIntensity;
        public float PatchyTextureTileSize; // world units
        public float PatchyDistanceBetweenSheets; // world units
        public float PatchyZBufferFadeFactor;
        public float PatchyDistanceFalloffStart; // world units
        public float PatchyDistanceFalloffPower; // world units
        public float PatchyDensity;
        public float PatchySurfaceDepth; // world units
        public float PatchyFadeRange; // world units
        public RealVector3d PatchyWindDirection; // world units
        public float MaxFogDrawDistance; // world units
        public float PatchyFadeStartDistance; // world units
        public float PatchyFadeEndDistance; // world units
        
        [Flags]
        public enum PlanarFogFlags : ushort
        {
            EnablePatchyEffect = 1 << 0,
            EnableColorPalette = 1 << 1,
            EnableAlphaPalette = 1 << 2,
            RenderOnly = 1 << 3
        }
    }
}
