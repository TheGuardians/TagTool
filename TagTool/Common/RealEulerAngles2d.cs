using System;

namespace TagTool.Common
{
    public struct RealEulerAngles2d : IEquatable<RealEulerAngles2d>
    {
        public Angle Yaw { get; }

        public Angle Pitch { get; }

        public RealEulerAngles2d(Angle yaw, Angle pitch)
        {
            Yaw = yaw;
            Pitch = pitch;
        }

        public bool Equals(RealEulerAngles2d other) =>
            Yaw.Equals(other.Yaw) &&
            Pitch.Equals(other.Pitch);

        public override bool Equals(object obj) =>
            obj is RealEulerAngles2d ?
                Equals((RealEulerAngles2d)obj) :
            false;

        public static bool operator ==(RealEulerAngles2d a, RealEulerAngles2d b) =>
            a.Equals(b);

        public static bool operator !=(RealEulerAngles2d a, RealEulerAngles2d b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Yaw.GetHashCode()
               * 17 + Pitch.GetHashCode();

        public override string ToString() =>
            $"{{ Yaw: {Yaw}, Pitch: {Pitch} }}";
    }
}
