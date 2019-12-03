using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Editing;

namespace TagTool.Common
{
    public struct RealBoundingBox : IBlamType
    {
        public Bounds<float> XBounds, YBounds, ZBounds;

        public RealBoundingBox(Bounds<float> xBounds, Bounds<float> yBounds, Bounds<float> zBounds)
        {
            XBounds = xBounds;
            YBounds = yBounds;
            ZBounds = zBounds;
        }

        public RealBoundingBox(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            XBounds = new Bounds<float>(minX, maxX);
            YBounds = new Bounds<float>(minY, maxY);
            ZBounds = new Bounds<float>(minZ, maxZ);
        }

        public float Length =>
            (float)Math.Sqrt(
                Math.Pow(XBounds.Upper - XBounds.Lower, 2) +
                Math.Pow(YBounds.Upper - YBounds.Lower, 2) +
                Math.Pow(ZBounds.Upper - ZBounds.Lower, 2));

        public override string ToString() => $"{{" +
            $"{{X: {XBounds.Lower}, {XBounds.Upper}}}, " +
            $"{{Y: {YBounds.Lower}, {YBounds.Upper}}}, " +
            $"{{Z: {ZBounds.Lower}, {ZBounds.Upper}}}}} ";

        public bool TryParse(HaloOnlineCacheContext cacheContext, List<string> args, out IBlamType result, out string error)
        {
            result = null;

            if (args.Count != 6)
            {
                error = $"{args.Count} arguments supplied; should be 6";
                return false;
            }
            else if (!float.TryParse(args[0], out float minX))
            {
                error = $"Unable to parse \"{args[0]}\" (minX) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[1], out float maxX))
            {
                error = $"Unable to parse \"{args[1]}\" (maxX) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[2], out float minY))
            {
                error = $"Unable to parse \"{args[2]}\" (minY) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[3], out float maxY))
            {
                error = $"Unable to parse \"{args[3]}\" (maxY) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[4], out float minZ))
            {
                error = $"Unable to parse \"{args[4]}\" (minZ) as `float`.";
                return false;
            }
            else if (!float.TryParse(args[5], out float maxZ))
            {
                error = $"Unable to parse \"{args[5]}\" (maxZ) as `float`.";
                return false;
            }
            else
            {
                result = new RealBoundingBox(minX, maxX, minY, maxY, minZ, maxZ);
                error = null;
                return true;
            }
        }
    }
}
