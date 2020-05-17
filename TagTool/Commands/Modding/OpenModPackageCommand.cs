using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Cache.HaloOnline;
using System;
using TagTool.Commands.Tags;
using System.IO;
using TagTool.Cache.Gen3;
using TagTool.IO;

namespace TagTool.Commands.Modding
{
    class OpenModPackageCommand : Command
    {
        private readonly GameCacheHaloOnlineBase Cache;
        private CommandContextStack ContextStack { get; }

        public OpenModPackageCommand(CommandContextStack contextStack, GameCacheHaloOnlineBase cache) :
            base(true,

                "OpenModPackage",
                "Create context for an existing mod package.",

                "OpenModPackage <.pak path>",

                "Create context for an existing mod package.")
        {
            ContextStack = contextStack;
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var file = new FileInfo(args[0]);

            if (!file.Exists)
                return false;

            Console.WriteLine("Initializing cache...");
            GameCacheModPackage modCache = new GameCacheModPackage(Cache, file);
            Console.WriteLine("done!");

            ContextStack.Push(TagCacheContextFactory.Create(ContextStack, modCache,
                $"{modCache.BaseModPackage.Metadata.Name}.pak"));

            return true;

        }
    }
}