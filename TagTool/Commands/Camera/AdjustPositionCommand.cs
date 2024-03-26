using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Camera
{
    class AdjustPositionCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private CameraTrack Track { get; }

        public AdjustPositionCommand(GameCache cache, CachedTag tag, CameraTrack trak)
            : base(false,

                  "AdjustPosition",
                  "Shift a camera track's position by a specified vector.",

                  "AdjustPosition <i> <j> <k>",

                  "Shift all camera points by a specified amount in each direction.")
        {
            Cache = cache;
            Tag = tag;
            Track = trak;
        }

        public override object Execute(List<string> args)
        {
            if(args.Count != 3)
                return new TagToolError(CommandError.ArgCount);

            if (!float.TryParse(args[0], out float i))
                return new TagToolError(CommandError.CustomError, $"\"{args[0]}\" could not be parsed as a float.");

            if (!float.TryParse(args[1], out float j))
                return new TagToolError(CommandError.CustomError, $"\"{args[1]}\" could not be parsed as a float.");

            if (!float.TryParse(args[2], out float k))
                return new TagToolError(CommandError.CustomError, $"\"{args[2]}\" could not be parsed as a float.");

            foreach (var point in Track.ControlPoints)
            {
                point.Position.I += i;
                point.Position.J += j;
                point.Position.K += k;
            }

            Console.WriteLine("Applied.");
            return true;
        }
    }
}
