using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Common
{
    public struct RealEulerAngles3d : IEquatable<RealEulerAngles3d>, IBlamType
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

		private Angle _roll;
		public float RollValue
		{
			get { return _roll.Radians; }
			set { _roll.Radians = value; }
		}
		public Angle Roll
		{
			get { return _roll; }
			private set { _roll = value; }
		}

		public RealEulerAngles3d(Angle yaw, Angle pitch, Angle roll)
        {
            _yaw = yaw;
            _pitch = pitch;
            _roll = roll;
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

        public bool TryParse(HaloOnlineCacheContext cacheContext, List<string> args, out IBlamType result, out string error)
        {
            result = null;
            if (args.Count != 3)
            {
                error = $"{args.Count} arguments supplied; should be 3";
                return false;
            }
            else if (!float.TryParse(args[0], out float yaw))
            {
                error = $"Unable to parse \"{args[0]}\" (yaw) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[1], out float pitch))
            {
                error = $"Unable to parse \"{args[1]}\" (pitch) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[1], out float roll))
            {
                error = $"Unable to parse \"{args[2]}\" (roll) as `float`.";
                return false;
            }
            else
            {
                result = new RealEulerAngles3d(
                    Angle.FromDegrees(yaw),
                    Angle.FromDegrees(pitch),
                    Angle.FromDegrees(roll));
                error = null;
                return true;
            }
        }
    }
}
