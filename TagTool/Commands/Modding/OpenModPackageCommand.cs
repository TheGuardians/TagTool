using TagTool.Cache;
using System.Collections.Generic;
using System;
using TagTool.Commands.Common;
using TagTool.Commands.Tags;
using System.IO;

namespace TagTool.Commands.Modding
{
    class OpenModPackageCommand : Command
    {
        private readonly GameCacheHaloOnlineBase Cache;
        private CommandContextStack ContextStack { get; }
        private GameCacheModPackage ModCache;
        private CommandContext Context;

        public OpenModPackageCommand(CommandContextStack contextStack, GameCacheHaloOnlineBase cache) :
            base(true,

                "OpenModPackage",
                "Create context for an existing mod package.",

                "OpenModPackage [large] <.pak path>",

                "Create context for an existing mod package.")
        {
            ContextStack = contextStack;
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            bool useLargeStreams = false;

            if (args.Count > 1 && args[0].ToLower() == "large")
            {
                useLargeStreams = true;
                args.RemoveAt(0);
            }

            if(args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            string path = args[0];

            if (Cache.Directory != null)
            { 
                path = Cache.Directory.FullName;
                path = path.Substring(0, path.Length - 4);

                if (!args[0].Contains("/") && !args[0].Contains("\\"))
                    path += "mods\\" + args[0];
                else
                    path = args[0];

                if (!args[0].Contains(".pak"))
                    path += ".pak";
            }

			var file = new FileInfo(path);

            if (!file.Exists)
                return new TagToolError(CommandError.FileNotFound, $"\"{args[0]}\"");

            Console.WriteLine("Initializing cache...");

            ModCache = new GameCacheModPackage(Cache, file, largeResourceStream: useLargeStreams);
            Context = TagCacheContextFactory.Create(ContextStack, ModCache, $"{ModCache.BaseModPackage.Metadata.Name}.pak");
            ContextStack.Push(Context);

            Console.WriteLine("Done!");

            return true;
        }
    }
}