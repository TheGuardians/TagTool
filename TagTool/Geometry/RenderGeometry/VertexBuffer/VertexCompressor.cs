using TagTool.Common;

namespace TagTool.Geometry
{
    /// <summary>
    /// Compresses and decompresses vertex data.
    /// </summary>
    public class VertexCompressor
    {
        private readonly RenderGeometryCompression _info;
        private readonly float _xScale;
        private readonly float _yScale;
        private readonly float _zScale;
        private readonly float _uScale;
        private readonly float _vScale;

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexCompressor"/> class.
        /// </summary>
        /// <param name="info">The compression info to use.</param>
        public VertexCompressor(RenderGeometryCompression info)
        {
            _info = info;
            _xScale = info.X.Upper - info.X.Lower;
            _yScale = info.Y.Upper - info.Y.Lower;
            _zScale = info.Z.Upper - info.Z.Lower;
            _uScale = info.U.Upper - info.U.Lower;
            _vScale = info.V.Upper - info.V.Lower;
        }

        /// <summary>
        /// Compresses a position so that its components are between 0 and 1.
        /// </summary>
        /// <param name="pos">The position to compress.</param>
        /// <returns>The compressed position.</returns>
        public RealQuaternion CompressPosition(RealQuaternion pos)
        {
            var newX = (pos.I - _info.X.Lower) / _xScale;
            var newY = (pos.J - _info.Y.Lower) / _yScale;
            var newZ = (pos.K - _info.Z.Lower) / _zScale;
            return new RealQuaternion(newX, newY, newZ, pos.W);
        }

        /// <summary>
        /// Decompresses a position so that its components are in model space.
        /// </summary>
        /// <param name="pos">The position to decompress.</param>
        /// <returns>The decompressed position.</returns>
        public RealQuaternion DecompressPosition(RealQuaternion pos)
        {
            var newX = pos.I * _xScale + _info.X.Lower;
            var newY = pos.J * _yScale + _info.Y.Lower;
            var newZ = pos.K * _zScale + _info.Z.Lower;
            return new RealQuaternion(newX, newY, newZ, pos.W);
        }

        /// <summary>
        /// Compresses texture coordinates so that the components are between 0 and 1.
        /// </summary>
        /// <param name="uv">The texture coordinates to compress.</param>
        /// <returns>The compressed coordinates.</returns>
        public RealVector2d CompressUv(RealVector2d uv)
        {
            var newU = (uv.I - _info.U.Lower) / _uScale;
            var newV = (uv.J - _info.V.Lower) / _vScale;
            return new RealVector2d(newU, newV);
        }

        /// <summary>
        /// Decompresses texture coordinates.
        /// </summary>
        /// <param name="uv">The texture coordinates to decompress.</param>
        /// <returns>The decompressed coordinates.</returns>
        public RealVector2d DecompressUv(RealVector2d uv)
        {
            var newU = uv.I * _uScale + _info.U.Lower;
            var newV = uv.J * _vScale + _info.V.Lower;
            return new RealVector2d(newU, newV);
        }
    }
}
