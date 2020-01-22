namespace TagTool.Geometry
{
    /// <summary>
    /// Precomputed radiance transfer (PRT) types using spherical harmonics as basis function.
    /// </summary>
    public enum PrtSHType : byte
    {
        None,
        /// <summary>
        /// SH order 0, 1 coefficent per vertex
        /// </summary>
        Ambient,
        /// <summary>
        /// SH order 1, 4 coefficients per vertex
        /// </summary>
        Linear,
        /// <summary>
        /// SH order 2, 9 coefficients per vertex
        /// </summary>
        Quadratic
    }
}
