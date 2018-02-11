using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.TagDefinitions;

namespace TagTool.Commands.Unicode
{
    class GetStringCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private MultilingualUnicodeStringList Definition { get; }

        public GetStringCommand(GameCacheContext cacheContext, CachedTagInstance tag, MultilingualUnicodeStringList unic)
            : base(CommandFlags.Inherit,

                  "GetString",
                  "Gets the value of a string.",

                  "GetString <language> <string_id>",

                  "Gets the value of a string.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = unic;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var languageName = args[0];
            
            if (!ArgumentParser.TryParseEnum(args[0], out GameLanguage language))
                return false;

            var stringIdStr = args[1];
            var stringIdIndex = CacheContext.StringIdCache.Strings.IndexOf(stringIdStr);
            if (stringIdIndex < 0)
            {
                Console.WriteLine("Unable to find stringID \"{0}\".", stringIdStr);
                return true;
            }

            var stringId = CacheContext.GetStringId(stringIdIndex);
            if (stringId == StringId.Null)
            {
                Console.WriteLine("Failed to resolve the stringID.");
                return true;
            }

            var localizedStr = Definition.Strings.FirstOrDefault(s => s.StringID == stringId);
            if (localizedStr == null)
            {
                Console.WriteLine("Unable to find unicode string \"{0}\"", stringIdStr);
                return true;
            }

            Console.WriteLine(Definition.GetString(localizedStr, language));

            return true;
        }
    }
}
