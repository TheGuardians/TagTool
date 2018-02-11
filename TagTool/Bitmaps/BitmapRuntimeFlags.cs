using System;

namespace TagTool.Bitmaps
{
    [Flags]
    public enum BitmapRuntimeFlags : short
    {
        None = 0,

        /// <summary>
        /// Affects how height maps are converted to bump maps
        /// </summary>
        BitmapIsTiled = 1 << 0,

        /// <summary>
        /// Uses a sharper (and noisier) method of calculating bump maps from height maps
        /// </summary>
        UseLessBlurryBumpMap = 1 << 1,

        /// <summary>
        /// Lets the compressor use dithering
        /// </summary>
        DitherWhenCompressing = 1 << 2,

        /// <summary>
        /// Repopulates the manual sequences with random sprites upon reimport
        /// </summary>
        GenerateRandomSprites = 1 << 3,

        /// <summary>
        /// FOR INTERNAL USE ONLY - DO NOT MODIFY
        /// </summary>
        UsingTagInteropAndTagResource = 1 << 4,

        /// <summary>
        /// If you have an alpha channel but do not care about it, set this flag
        /// </summary>
        IgnoreAlphaChannel = 1 << 5,

        /// <summary>
        /// If your alpha channel represents transparency (alpha blend or alpha-test only),
        /// set this bit to stop color bleeding on edges
        /// </summary>
        AlphaChannelStoresTransparency = 1 << 6,

        /// <summary>
        /// This will artificially thicken the alpha channel in mip maps, which can keep your
        /// bitmap from disappearing in the distance when you are using alpha test
        /// </summary>
        PreserveAlphaChannelInMipmapsForAlphaTest = 1 << 7,

        /// <summary>
        /// This bitmap will always be demand loaded, only supported by UI
        /// </summary>
        OnlyUseOnDemand = 1 << 8,

        /// <summary>
        /// Generate a polygonal bounding box around the non-empty pixels to save fill rate cost
        /// </summary>
        GenerateTightBounds = 1 << 9,

        /// <summary>
        /// Unchecked, tight bounds are generated from the color channel
        /// </summary>
        TightBoundsFromAlphaChannel = 1 << 10,

        /// <summary>
        /// Bitmap will have data split between medium and low optional sections
        /// </summary>
        DoNotGenerateRequiredSection = 1 << 11,

        /// <summary>
        /// Allows use of "max resolution" on bitmaps that have multiple frames, but may be buggy...?
        /// </summary>
        ApplyMaxResolutionAfterSlicing = 1 << 12,

        /// <summary>
        /// Generate a set of polygonal bounding boxes for various alpha black points
        /// </summary>
        GenerateBlackPointTightBounds = 1 << 13,

        /// <summary>
        /// Apply cosine power-weighted cone filter to entire cubemap instead of filtering each face as separate 2D image
        /// </summary>
        PreFilterCubemaps = 1 << 14
    }
}
