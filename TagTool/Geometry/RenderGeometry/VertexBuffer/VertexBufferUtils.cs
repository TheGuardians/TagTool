using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;

namespace TagTool.Geometry
{
    public static class VertexBufferUtils
    {
        /// <summary> 
        /// Change basis [0,1] to [-1,1] uniformly
        /// </summary>
        public static float ConvertFromNormalBasis(float value)
        {
            value = VertexElementStream.Clamp(value, 0.0f, 1.0f);
            return 2.0f * (value - 0.5f);
        }

        /// <summary> 
        /// Change basis [-1,1] to [0,1] uniformly
        /// </summary>
        public static float ConvertToNormalBasis(float value)
        {
            value = VertexElementStream.Clamp(value);
            return (value / 2.0f) + 0.5f;
        }

        /// <summary>
        /// Modify value to account for some rounding error in gen3 conversion
        /// </summary>
        public static float FixRoundingShort(float value)
        {
            value = VertexElementStream.Clamp(value);
            if (value > 0 && value < 1)
            {
                value += 1.0f / 32767.0f;
                value = VertexElementStream.Clamp(value);
            }
            return value;
        }

        public static uint ConvertColorSpace(uint color)
        {
            uint c1 = color & 0xFF;
            uint c2 = (color >> 8) & 0xFF;
            uint c3 = (color >> 16) & 0xFF;
            uint c4 = (color >> 24) & 0xFF;
            c1 = (c1 + 0x7F) & 0xFF;
            c2 = (c2 + 0x7F) & 0xFF;
            c3 = (c3 + 0x7F) & 0xFF;
            c4 = (c4 + 0x7F) & 0xFF;

            return c1 | (c2 << 8) | (c3 << 16) | (c4 << 24);
        }

        public static float ConvertVectorSpace(float value)
        {
            return value <= 0.5 ? 2.0f * value : (value - 1.0f) * 2.0f;
        }

        public static RealVector3d ConvertVectorSpace(RealVector3d vector)
        {
            return new RealVector3d(ConvertVectorSpace(vector.I), ConvertVectorSpace(vector.J), ConvertVectorSpace(vector.K));
        }

        public static RealQuaternion ConvertVectorSpace(RealQuaternion vector)
        {
            return new RealQuaternion(ConvertVectorSpace(vector.I), ConvertVectorSpace(vector.J), ConvertVectorSpace(vector.K), vector.W);
        }

        /// <summary>
        /// Convert H3 normal to HO normal for tinyposition vertex
        /// </summary>
        public static RealQuaternion ConvertNormal(RealQuaternion normal)
        {
            return new RealQuaternion(normal.ToArray().Select(e => ConvertToNormalBasis(e)).Reverse());
        }

        /// <summary>
        /// Convert H3 position to HO position including rounding error for tinyposition vertex
        /// </summary>
        public static RealVector3d ConvertPositionShort(RealVector3d position)
        {
            return new RealVector3d(position.ToArray().Select(e => FixRoundingShort(ConvertFromNormalBasis(e))).ToArray());
        }
    }
}
