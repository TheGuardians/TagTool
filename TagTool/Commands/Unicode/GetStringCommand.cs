using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Unicode
{
    public class GetStringCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private MultilingualUnicodeStringList Definition { get; }

        public GetStringCommand(GameCache cache, CachedTag tag, MultilingualUnicodeStringList unic)
            : base(true,

                  "GetString",
                  "Gets the value of a string.",

                  "GetString <language> <string_id>",

                  "Gets the value of a string.")
        {
            Cache = cache;
            Tag = tag;
            Definition = unic;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            var languageName = args[0];
            
            if (!ArgumentParser.TryParseEnum(args[0], out GameLanguage language))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");

            var stringIdStr = args[1];
            var stringIdIndex = Cache.StringTable.IndexOf(stringIdStr);
            if (stringIdIndex < 0)
            {
                Console.WriteLine("Unable to find stringID \"{0}\".", stringIdStr);
                return true;
            }

            var stringId = Cache.StringTable.GetStringId(stringIdIndex);
            if (stringId == StringId.Invalid)
                return new TagToolError(CommandError.OperationFailed, "Failed to resolve the StringId");

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
