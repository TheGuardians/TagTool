using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.IO;

namespace TagTool.Commands.Files
{
    class ImportFontsCommand : Command
    {
        public GameCacheHaloOnlineBase Cache { get; }

        public ImportFontsCommand(GameCacheHaloOnlineBase cache)
            : base(true,

                  "ImportFonts",
                  "Import fonts into Halo Online cache.",

                  "ImportFonts <path to font_package.bin>",

                  "Import fonts into Halo Online cache.\n")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var file = new FileInfo(args[0]);
            
            if (!file.Exists)
                return new TagToolError(CommandError.FileNotFound);

            using (var stream = file.OpenRead())
            {
                Cache.SaveFonts(stream);
            }

            Console.WriteLine("Done!");
            return true;
        }

    }
}