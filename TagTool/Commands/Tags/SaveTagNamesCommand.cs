using BlamCore.Cache;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Tags
{
    class SaveTagNamesCommand : Command
    {
        public GameCacheContext CacheContext { get; }

        public SaveTagNamesCommand(GameCacheContext cacheContext) :
            base(CommandFlags.Inherit,

                "SaveTagNames",
                "Saves the current tag names to the specified csv file.",

                "SaveTagNames [csv path]",

                "Saves the current tag names to the specified csv file.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return false;

            var csvFile = (args.Count == 1) ?
                new FileInfo(args[0]) :
                new FileInfo(Path.Combine(CacheContext.Directory.FullName, "tag_list.csv"));

            if (!csvFile.Directory.Exists)
                csvFile.Directory.Create();

            using (var csvWriter = new StreamWriter(csvFile.Create()))
                foreach (var entry in CacheContext.TagNames)
                    csvWriter.WriteLine($"0x{entry.Key:X8},{entry.Value}");

            return true;
        }
    }
}