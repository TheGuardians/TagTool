using BlamCore.Cache;
using BlamCore.Common;
using BlamCore.TagDefinitions;
using System.Collections.Generic;

namespace TagTool.Commands.Unicode
{
    class RemoveStringCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private MultilingualUnicodeStringList Definition { get; set; }

        public RemoveStringCommand(GameCacheContext cacheContext, CachedTagInstance tag, MultilingualUnicodeStringList definition) :
            base(CommandFlags.None,
                
                "RemoveString",
                "Removes a string entry from the multilingual_unicode_string_list definition.",
                
                "RemoveString <StringID>",

                "Removes a string entry from the multilingual_unicode_string_list definition.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var stringID = CacheContext.GetStringId(args[0]);

            var newDefinition = new MultilingualUnicodeStringList
            {
                Data = new byte[0],
                Strings = new List<LocalizedStringBlock>()
            };

            foreach (var oldString in Definition.Strings)
            {
                if (oldString.StringID == stringID)
                    continue;

                var newString = new LocalizedStringBlock
                {
                    Offsets = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                    StringID = oldString.StringID,
                    StringIDStr = oldString.StringIDStr
                };

                for (var i = 0; i < 12; i++)
                {
                    if (oldString.Offsets[i] == -1)
                        continue;

                    newDefinition.SetString(newString, (GameLanguage)i, Definition.GetString(oldString, (GameLanguage)i));
                }

                newDefinition.Strings.Add(newString);
            }

            Definition.Data = newDefinition.Data;
            Definition.Strings = newDefinition.Strings;

            return true;
        }
    }
}