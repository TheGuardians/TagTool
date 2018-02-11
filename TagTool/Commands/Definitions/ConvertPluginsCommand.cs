using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.Layouts;

namespace TagTool.Commands.Definitions
{
    class ConvertPluginsCommand : Command
    {
        private GameCacheContext CacheContext { get; }

        public ConvertPluginsCommand(GameCacheContext cacheContext)
            : base(CommandFlags.Inherit,

                  "ConvertPlugins",
                  "Convert Assembly plugins to tag layout structures",

                  "ConvertPlugins <input dir> <output type> <output dir>",

                  "Only plugins for groups that are actually used in the tag cache will be converted.\n" +
                  "Layouts will be written to the output directory in the chosen format.\n" +
                  "\n" +
                  "Supported output types: c#, c++")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 3)
                return false;
            var inDir = args[0];
            var type = args[1];
            var outDir = args[2];
            TagLayoutWriter writer;
            switch (type)
            {
                case "c#":
                    writer = new CSharpLayoutWriter();
                    break;
                case "c++":
                    writer = new CppLayoutWriter();
                    break;
                default:
                    return false;
            }
            Directory.CreateDirectory(outDir);

            // For each tag whose tag group hasn't been processed yet, load its
            // plugin into a TagLayout and then write it using the layout
            // writer for the output type. We need an actual tag reference in
            // order to look up the group name without using a static table.
            var processedGroups = new HashSet<Tag>();
            var numConflicts = 0;
            foreach (var tag in CacheContext.TagCache.Index.NonNull().Where(tag => !processedGroups.Contains(tag.Group.Tag)))
            {
                processedGroups.Add(tag.Group.Tag);

                // Get the plugin path and skip it if it doesn't exist
                var pluginFileName = SanitizeGroupTagName(tag.Group.Tag.ToString()) + ".xml";
                var pluginPath = Path.Combine(inDir, pluginFileName);
                if (!File.Exists(pluginPath))
                {
                    Console.Error.WriteLine("WARNING: No plugin found for the '{0}' tag group", tag.Group.Tag);
                    continue;
                }

                Console.WriteLine("Converting {0}...", pluginFileName);

                // Load the plugin into a layout
                AssemblyPluginLoadResults loadedPlugin;
                var groupName = CacheContext.GetString(tag.Group.Name);
                using (var reader = XmlReader.Create(pluginPath))
                    loadedPlugin = AssemblyPluginLoader.LoadPlugin(reader, groupName, tag.Group.Tag);

                // Warn the user about conflicts
                numConflicts += loadedPlugin.Conflicts.Count;
                foreach (var conflict in loadedPlugin.Conflicts)
                    Console.WriteLine("WARNING: Field \"{0}\" at offset 0x{1:X} in block \"{2}\" conflicts!", conflict.Name, conflict.Offset, conflict.Block ?? "(root)");

                // Write it
                var outPath = Path.Combine(outDir, writer.GetSuggestedFileName(loadedPlugin.Layout));
                writer.WriteLayout(loadedPlugin.Layout, outPath);
            }
            Console.WriteLine("Successfully converted {0} plugins!", processedGroups.Count);
            if (numConflicts > 0)
                Console.WriteLine("However, {0} conflicts were found. You MUST fix these yourself!", numConflicts);
            return true;
        }

        public static string SanitizeGroupTagName(string name)
        {
            // http://stackoverflow.com/questions/309485/c-sharp-sanitize-file-name
            var regex = string.Format(@"(\.+$)|([{0}])", new string(Path.GetInvalidFileNameChars()));
            return Regex.Replace(name, regex, "_").Trim();
        }
    }
}
