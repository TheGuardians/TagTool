namespace TagTool.Common
{
    public struct RealRectangle3d
    {
        public float X0;
        public float X1;
        public float Y0;
        public float Y1;
        public float Z0;
        public float Z1;

        public RealRectangle3d(float x0, float x1, float y0, float y1, float z0, float z1)
        {
            X0 = x0;
            X1 = x1;
            Y0 = y0;
            Y1 = y1;
            Z0 = z0;
            Z1 = z1;
        }
    }
}
