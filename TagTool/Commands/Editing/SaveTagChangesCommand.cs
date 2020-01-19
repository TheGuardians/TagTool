using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Commands.Editing
{
    class SaveTagChangesCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private object Value { get; }

        public SaveTagChangesCommand(GameCache cache, CachedTag tag, object value)
            : base(true,

                  "SaveTagChanges",
                  $"Saves changes made to the current {cache.StringTable.GetString(tag.Group.Name)} definition.",

                  "SaveTagChanges",

                  $"Saves changes made to the current {cache.StringTable.GetString(tag.Group.Name)} definition.")
        {
            Cache = cache;
            Tag = tag;
            Value = value;
        }

        public override object Execute(List<string> args)
        {
            using (var stream = Cache.OpenCacheReadWrite())
                Cache.Serialize(stream, Tag, Value);

            Console.WriteLine("Done!");

            return true;
        }
    }
}
