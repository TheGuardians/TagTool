using System;

namespace TagTool.Bitmaps
{
    [Flags]
    public enum BitmapRuntimeFlags : short
    {
        None = 0,
        BitmapIsTiled = 1 << 0, // Affects how height maps are converted to bump maps
        UseLessBlurryBumpMap = 1 << 1, // Uses a sharper (and noisier) method of calculating bump maps from height maps
        DitherWhenCompressing = 1 << 2, // Lets the compressor use dithering
        GenerateRandomSprites = 1 << 3, // Repopulates the manual sequences with random sprites upon reimport
        UsingTagInteropAndTagResource = 1 << 4, // FOR INTERNAL USE ONLY - DO NOT MODIFY
        IgnoreAlphaChannel = 1 << 5, // If you have an alpha channel but do not care about it, set this flag
        AlphaChannelStoresTransparency = 1 << 6, // If your alpha channel represents transparency (alpha blend or alpha-test only), set this bit to stop color bleeding on edges
        PreserveAlphaChannelInMipmapsForAlphaTest = 1 << 7, // This will artificially thicken the alpha channel in mip maps, which can keep your bitmap from disappearing in the distance when you are using alpha test
        OnlyUseOnDemand = 1 << 8, // This bitmap will always be demand loaded, only supported by UI
        GenerateTightBounds = 1 << 9, // Generate a polygonal bounding box around the non-empty pixels to save fill rate cost
        TightBoundsFromAlphaChannel = 1 << 10, // Unchecked, tight bounds are generated from the color channel
        DoNotGenerateRequiredSection = 1 << 11, // Bitmap will have data split between medium and low optional sections
        ApplyMaxResolutionAfterSlicing = 1 << 12, // Allows use of "max resolution" on bitmaps that have multiple frames, but may be buggy...?
        GenerateBlackPointTightBounds = 1 << 13, // Generate a set of polygonal bounding boxes for various alpha black points
        PreFilterCubemaps = 1 << 14 // Apply cosine power-weighted cone filter to entire cubemap instead of filtering each face as separate 2D image
    }
}
