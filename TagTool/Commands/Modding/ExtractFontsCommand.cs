using TagTool.Cache;
using TagTool.Commands.Common;
using System.Collections.Generic;
using System.IO;
using System;

namespace TagTool.Commands.Modding
{
    class ExtractFontsCommand : Command
    {
        public GameCacheModPackage Cache { get; }

        public ExtractFontsCommand(GameCacheModPackage cache) :
            base(true,

                "ExtractFonts",
                "Extract fonts from mod package to specified file.\n",

                "ExtractFonts <destination>",

                "Extract fonts from mod package to specified file.\n")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var modFonts = Cache.BaseModPackage.FontPackage;

            if (modFonts != null && modFonts.Length > 0)
            {
                var file = new FileInfo(args[0]);

                if (file.Exists)
                    file.Delete();

                using (var stream = file.Create())
                {
                    modFonts.Seek(0, SeekOrigin.Begin);
                    modFonts.CopyTo(stream);
                }
            }
            else
                Console.WriteLine("Mod package does not have a font package.");

            return true;
        }
    }
}