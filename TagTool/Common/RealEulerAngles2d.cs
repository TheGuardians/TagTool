using System;

namespace TagTool.Common
{
	public struct RealEulerAngles2d : IEquatable<RealEulerAngles2d>
	{
		private Angle _yaw;
		public float YawValue
		{
			get { return this._yaw.Radians; }
			set { this._yaw.Radians = value; }
		}
		public Angle Yaw
		{
			get { return this._yaw; }
			private set { this._yaw = value; }
		}

		private Angle _pitch;
		public float PitchValue
		{
			get { return this._pitch.Radians; }
			set { this._pitch.Radians = value; }
		}
		public Angle Pitch
		{
			get { return this._pitch; }
			private set { this._pitch = value; }
		}

		public RealEulerAngles2d(Angle yaw, Angle pitch)
        {
            this._yaw = yaw;
            this._pitch = pitch;
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
