using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace TagTool.Extensions
{
    public static class VectorExtensions
    {
        // initializes a vector, padding according to SIMD width (4 x86, 8 x64)
        public static Vector<float> InitializeVector(float[] a)
        {
            float[] vector = new float[Vector<float>.Count];
            vector[0] = a[0];
            vector[1] = a[1];
            vector[2] = a[2];
            if (a.Length > 3)
                vector[3] = a[3];
            return new Vector<float>(vector);
        }
        public static Vector<float> InitializeVector(float a)
        {
            float[] vector = new float[Vector<float>.Count];
            for (int i = 0; i < vector.Length; i++)
                vector[i] = a;
            return new Vector<float>(vector);
        }

        public static Vector<float> InitializeVector() => new Vector<float>(new float[Vector<float>.Count]);

        public static Vector<float> Subtract4d(Vector<float> a, float b) => a - InitializeVector(b);

        public static float Dot3d(Vector<float> a, Vector<float> b)
        {
            return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
        }

        public static Vector<float> Convert4dTo3d(Vector<float> a) => a;
        //{
        //    return new Vector<float>(new float[] { a[0], a[1], a[2] });
        //}
    }
}
