namespace TagTool.Bitmaps
{
    public enum BitmapCurveMode : sbyte
    {
        /// <summary>
        /// Will choose FAST if your bitmap is bright
        /// </summary>
        ChooseBest,

        /// <summary>
        /// Forces FAST mode, but causes banding in dark areas
        /// </summary>
        ForceFast,

        /// <summary>
        /// Chooses the best looking curve, probably slower
        /// </summary>
        ForcePretty
    }
}