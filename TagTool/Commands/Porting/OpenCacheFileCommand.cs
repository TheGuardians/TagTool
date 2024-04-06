using TagTool.Cache;
using TagTool.Commands.Common;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Porting
{
    public class OpenCacheFileCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private GameCache Cache { get; }

        public OpenCacheFileCommand(CommandContextStack contextStack, GameCache cache)
            : base(false,

                  "OpenCacheFile",
                  "Opens a porting context with the specified cache file.",

                  "OpenCacheFile <Cache File>",

                  "Opens a porting context with the specified cache file.")
        {
            ContextStack = contextStack;
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            while (args.Count > 1)
            {
                switch (args[0].ToLower())
                {
                    case "memory":
                        break;

                    default:
                        return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");
                }

                args.RemoveAt(0);
            }

            var fileName = new FileInfo(args[0]);

            if (!fileName.Exists)
            {
                new TagToolError(CommandError.CustomError, $"Cache \"{fileName.FullName}\" does not exist.");
                return true;
            }
                
            Console.Write("Loading cache...");

            var blamCache = GetCache(fileName);
            if (blamCache == null)
                return false;

            ContextStack.Push(PortingContextFactory.Create(ContextStack, Cache, (GameCache)blamCache));

            Console.WriteLine("done.");

            return true;
        }

        private object GetCache(FileInfo fileName)
        {
            if (fileName.Extension == ".pak")
            {
                if (Cache is GameCacheModPackage)
                    return new GameCacheModPackage(((GameCacheModPackage)Cache).BaseCacheReference, fileName, false);
                else if (Cache is GameCacheHaloOnlineBase)
                    return new GameCacheModPackage((GameCacheHaloOnlineBase)Cache, fileName, false);
                else
                    return new TagToolError(CommandError.OperationFailed, "Mod package porting only allowed on ED base cache or mod package caches!");
            }
            else
                return GameCache.Open(fileName);
        }
    }
}

