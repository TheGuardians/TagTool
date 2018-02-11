using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Serialization;

namespace TagTool.Commands.Editing
{
    class SaveTagChangesCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private object Value { get; }

        public SaveTagChangesCommand(GameCacheContext cacheContext, CachedTagInstance tag, object value)
            : base(CommandFlags.Inherit,

                  "SaveTagChanges",
                  $"Saves changes made to the current {cacheContext.GetString(tag.Group.Name)} definition.",

                  "SaveTagChanges",

                  $"Saves changes made to the current {cacheContext.GetString(tag.Group.Name)} definition.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Value = value;
        }

        public override object Execute(List<string> args)
        {
            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                var context = new TagSerializationContext(stream, CacheContext, Tag);
                CacheContext.Serializer.Serialize(context, Value);
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}
