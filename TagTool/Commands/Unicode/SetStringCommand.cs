using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using System.Text.RegularExpressions;
using System.Globalization;
using TagTool.Commands.Strings;

namespace TagTool.Commands.Unicode
{
    public class SetStringCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private MultilingualUnicodeStringList Definition { get; }

        public SetStringCommand(GameCache cache, CachedTag tag, MultilingualUnicodeStringList unic)
            : base(false,

                  "SetString",
                  "Set the value of a string",

                  "SetString <language> <stringid> <value>",

                  "Sets the string associated with a stringID in a language.\n" +
                  "Remember to put the string value in quotes if it contains spaces.\n" +
                  "If the string does not exist, it will be added.")
        {
            Cache = cache;
            Tag = tag;
            Definition = unic;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 3)
                return new TagToolError(CommandError.ArgCount);
            if (!ArgumentParser.TryParseEnum(args[0], out GameLanguage language))
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");

            // Look up the stringID that was passed in
            var stringIdStr = args[1];
            var stringIdIndex = Cache.StringTable.IndexOf(stringIdStr);
            if (stringIdIndex < 0)
            {
                Console.WriteLine($"\"{stringIdStr}\" not found, creating a new StringID.");
                new StringIdCommand(Cache).Execute(new List<string>() { "add", stringIdStr });
                stringIdIndex = Cache.StringTable.IndexOf(stringIdStr);
            }
            var stringId = Cache.StringTable.GetStringId(stringIdIndex);
            if (stringId == StringId.Invalid)
            {
                return new TagToolError(CommandError.OperationFailed, "Failed to resolve the StringId");
            }
            var newValue = new Regex(@"\\[uU]([0-9A-F]{4})").Replace(args[2], match => ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());

            // Look up or create a localized string entry
            var localizedStr = Definition.Strings.FirstOrDefault(s => s.StringID == stringId);
            var added = false;
            if (localizedStr == null)
            {
                // Add a new string
                localizedStr = new LocalizedString { StringID = stringId, StringIDStr = stringIdStr };
                Definition.Strings.Add(localizedStr);
                added = true;
            }

            // Save the tag data
            Definition.SetString(localizedStr, language, newValue);

            if (added)
                Console.WriteLine("String added successfully.");
            else
                Console.WriteLine("String changed successfully.");

            return true;
        }
    }
}
