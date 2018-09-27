using System;

namespace TagTool.Common
{
	public struct RealEulerAngles2d : IEquatable<RealEulerAngles2d>, IBlamType
	{
		private Angle _yaw;
		public float YawValue
		{
			get { return _yaw.Radians; }
			set { _yaw.Radians = value; }
		}
		public Angle Yaw
		{
			get { return _yaw; }
			private set { _yaw = value; }
		}

		private Angle _pitch;
		public float PitchValue
		{
			get { return _pitch.Radians; }
			set { _pitch.Radians = value; }
		}
		public Angle Pitch
		{
			get { return _pitch; }
			private set { _pitch = value; }
		}

		public RealEulerAngles2d(Angle yaw, Angle pitch)
        {
            _yaw = yaw;
            _pitch = pitch;
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
