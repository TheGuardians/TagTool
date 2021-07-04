using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Commands.Common;

namespace TagTool.Commands.Shaders
{
    class UseShaderCacheCommand : Command
    {
        public static GameCacheHaloOnline ShaderCache = null;

        public UseShaderCacheCommand() :
            base(true,

                "UseShaderCache",
                "Specify a directory to store shader cache",

                "UseShaderCache <Directory>",
                "")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var directory = args[0];

            var tagCacheFile = new FileInfo(Path.Combine(directory, "tags.dat"));

            if (!tagCacheFile.Exists)
            {
                Console.Write("Shader cache does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return new TagToolError(CommandError.YesNoSyntax);

                if (answer.StartsWith("y"))
                {
                    tagCacheFile.Directory.Create();
                    ShaderCache = new GameCacheHaloOnline(tagCacheFile.Directory);
                }
            }

            if (tagCacheFile.Directory.Exists)
            {
                ShaderCache = (GameCacheHaloOnline)GameCache.Open(tagCacheFile);
                Console.WriteLine("Shader cache set successfully");
                return true;
            }
            else
            {
                return new TagToolError(CommandError.DirectoryNotFound, $"\"{directory}\"");
            }
        }
    }
}
