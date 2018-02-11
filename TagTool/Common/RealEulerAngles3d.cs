using System;

namespace TagTool.Common
{
    public struct RealEulerAngles3d : IEquatable<RealEulerAngles3d>
    {
        public Angle Yaw { get; }

        public Angle Pitch { get; }

        public Angle Roll { get; }


        public RealEulerAngles3d(Angle yaw, Angle pitch, Angle roll)
        {
            Yaw = yaw;
            Pitch = pitch;
            Roll = roll;
        }

        public bool Equals(RealEulerAngles3d other) =>
            Yaw.Equals(other.Yaw) &&
            Pitch.Equals(other.Pitch) &&
            Roll.Equals(other.Roll);

        public override bool Equals(object obj) =>
            obj is RealEulerAngles3d ?
                Equals((RealEulerAngles3d)obj) :
            false;

        public static bool operator ==(RealEulerAngles3d a, RealEulerAngles3d b) =>
            a.Equals(b);

        public static bool operator !=(RealEulerAngles3d a, RealEulerAngles3d b) =>
            !a.Equals(b);

        public override int GetHashCode() =>
            13 * 17 + Yaw.GetHashCode()
               * 17 + Pitch.GetHashCode()
               * 17 + Roll.GetHashCode();

        public override string ToString() =>
            $"{{ Yaw: {Yaw}, Pitch: {Pitch}, Roll: {Roll} }}";
    }
}
