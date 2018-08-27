using System;

namespace TagTool.Common
{
    public struct RealEulerAngles3d : IEquatable<RealEulerAngles3d>
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

		private Angle _roll;
		public float RollValue
		{
			get { return this._roll.Radians; }
			set { this._roll.Radians = value; }
		}
		public Angle Roll
		{
			get { return this._roll; }
			private set { this._roll = value; }
		}

		public RealEulerAngles3d(Angle yaw, Angle pitch, Angle roll)
        {
            this._yaw = yaw;
            this._pitch = pitch;
            this._roll = roll;
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
