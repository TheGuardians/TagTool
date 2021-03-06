using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Strings
{
    class ListAllStringsCommand : Command
    {
        private GameCache Cache { get; }

        public ListAllStringsCommand(GameCache cache) : base(
            true,

            "ListAllStrings",
            "Scan unic tags to find a localized string",

            "ListAllStrings <language> [filter]",

            "Scans all unic tags to find the strings belonging to a language.\n" +
            "If a filter is specified, only strings containing the filter will be listed.\n" +
            "\n" +
            "Available languages:\n" +
            "\n" +
            "english, japanese, german, french, spanish, mexican, italian, korean,\n" +
            "chinese-trad, chinese-simp, portuguese, russian")
        {
            Cache = cache;
        }
        
        public override object Execute(List<string> args)
        {
            if (args.Count != 1 && args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            if (!ArgumentParser.TryParseEnum(args[0], out GameLanguage language))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");

            var filter = (args.Count == 2) ? args[1] : null;
            var found = false;

            using (var stream = Cache.OpenCacheRead())
            {
                foreach (var unicTag in Cache.TagCache.FindAllInGroup("unic"))
                {
                    var unic = Cache.Deserialize<MultilingualUnicodeStringList>(stream, unicTag);
                    var strings = LocalizedStringPrinter.PrepareForDisplay(unic, Cache.StringTable, unic.Strings, language, filter);

                    if (strings.Count == 0)
                        continue;

                    Console.WriteLine($"\nStrings found in {unicTag.Name}.unic:");
                    LocalizedStringPrinter.PrintStrings(strings);

                    found = true;
                }
            }

            if (!found)
                Console.WriteLine("No strings found.");

            return true;
        }
    }
}
