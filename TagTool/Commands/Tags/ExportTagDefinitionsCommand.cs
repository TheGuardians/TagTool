using BlamCore.Cache;
using BlamCore.Common;
using BlamCore.Serialization;
using BlamCore.TagDefinitions;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TagTool.Commands.Tags
{
    class ExportTagDefinitionsCommand : Command
    {
        private GameCacheContext CacheContext { get; }

        public ExportTagDefinitionsCommand(GameCacheContext cacheContext) :
            base(CommandFlags.Inherit,
                
                "ExportTagDefinitions",
                "Exports all internal tag definitions for use in ElDewrito.",
                
                "ExportTagDefinitions <Dest Dir>",
                "Exports all internal tag definitions for use in ElDewrito.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var destDir = new DirectoryInfo(args[0]);

            if (!destDir.Exists)
                destDir.Create();

            foreach (var entry in TagDefinition.Types)
            {
                var tagGroup = TagGroup.Instances[entry.Key];
                var tagGroupName = CacheContext.GetString(tagGroup.Name);
                var tagStructureInfo = new TagStructureInfo(entry.Value, CacheContext.Version);
                var enumerator = new TagFieldEnumerator(tagStructureInfo);

                using (var stream = File.Create(Path.Combine(destDir.FullName, $"{tagGroupName}.hpp")))
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine(@"#pragma once");
                    writer.WriteLine(@"#include <cstdint>");
                    writer.WriteLine(@"#include ..\\Tags.hpp");
                    writer.WriteLine();
                    writer.WriteLine(@"namespace Blam");
                    writer.WriteLine(@"{");
                    writer.WriteLine(@"    namespace Tags");
                    writer.WriteLine(@"    {");
                    
                    while (enumerator.Next())
                        PrintField(writer, enumerator);
                }
            }

            return true;
        }

        private string SanitizeName(string name)
        {
            // http://stackoverflow.com/questions/309485/c-sharp-sanitize-file-name
            var regex = string.Format(@"(\.+$)|([{0}])", new string(Path.GetInvalidFileNameChars()));
            return Regex.Replace(name, regex, "_").Trim();
        }

        private void PrintField(StreamWriter writer, TagFieldEnumerator enumerator)
        {
            var field = enumerator.Field;
            var type = field.FieldType;
            var name = SanitizeName(field.Name);
        }
    }
}
