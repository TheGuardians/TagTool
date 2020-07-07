using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.HUD
{
    class RescaleHudTextCommand : Command
    {
        private GameCache Cache { get; }

        public RescaleHudTextCommand(GameCache cache)
            : base(true,

                  "RescaleHudText",
                  "Rescales chdt text widgets according to the specified scale",

                  "RescaleHudText [scale] [all]",
                  "Rescales chdt text widgets according to the specified scale")
        {
            Cache = cache;
        }

        static readonly List<int> fontIndices = new List<int> { 3, 4, 5, 9 };

        public override object Execute(List<string> args)
        {
            float scale = 0.7619047619047619f;
            bool applyToAll = false;

            if (args.Count > 0 && !float.TryParse(args[0], out scale))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");
            if (args.Count > 1)
                applyToAll = args[1] == "all";

            using (var stream = Cache.OpenCacheReadWrite())
            {
                foreach (var tag in Cache.TagCache.TagTable)
                {
                    if (tag != null && tag.Group.Tag == "chdt")
                    {
                        var chdt = Cache.Deserialize<ChudDefinition>(stream, tag);

                        foreach (var hudWidget in chdt.HudWidgets)
                        {
                            foreach (var textWidget in hudWidget.TextWidgets)
                            {
                                if (fontIndices.Contains((int)textWidget.Font) || applyToAll)
                                {
                                    foreach (var placementData in textWidget.PlacementData)
                                    {
                                        placementData.Scale.X *= scale;
                                        placementData.Scale.Y *= scale;
                                    }
                                }
                            }
                        }

                        Cache.Serialize(stream, tag, chdt);
                    }
                }
            }

            return true;
        }
    }
}
