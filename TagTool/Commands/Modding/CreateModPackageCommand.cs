using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Cache.HaloOnline;
using System;
using TagTool.Commands.Common;
using TagTool.Commands.Tags;
using System.IO;
using TagTool.Cache.Gen3;
using TagTool.IO;
using System.Threading;

namespace TagTool.Commands.Modding
{
    class CreateModPackageCommand : Command
    {
        private readonly GameCacheHaloOnline Cache;
        private CommandContextStack ContextStack { get; }

        public CreateModPackageCommand(CommandContextStack contextStack, GameCacheHaloOnline cache) :
            base(true,

                "CreateModPackage",
                "Create context for a mod package. Optionally have more than one tag cache and use unmanaged streams for 2gb + resources.",

                "CreateModPackage [# of tag caches, [large]] [testname]",

                "\t- [# of tag caches]: An integer used for when you want more than one isolated tag cache."
                + "\n\t- [large]: Used with a tag cache count to allow unmanaged streams for 2GB + resources."
                + "\n\t- [testname]: A name for a test mod package. Skips the standard dialog, used without other arguments.")
        {
            ContextStack = contextStack;
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 2)
                return new TagToolError(CommandError.ArgCount);

            int tagCacheCount = 1;
            bool useLargeStreams = false;
            bool useDialog = true;

            if (args.Count > 0)
            {
                if (!int.TryParse(args[0], System.Globalization.NumberStyles.Integer, null, out tagCacheCount))
                {
                    if (args.Count == 1 && !string.IsNullOrEmpty(args[0]))
                    {
                        useDialog = false;
                        tagCacheCount = 1;
                    }
                    else
                        return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\"");
                }

                if (args.Count == 2 && args[1].ToLower() == "large")
                    useLargeStreams = true;
            }
                

            Console.WriteLine("Initializing cache...");

            GameCacheModPackage modCache = new GameCacheModPackage(Cache, useLargeStreams);

            // create metadata here
            modCache.BaseModPackage.CreateDescription(IgnoreArgumentVariables, useDialog);

            if (!useDialog)
                modCache.BaseModPackage.Metadata.Name = args[0];

            // initialize mod package with current HO cache
            Console.WriteLine($"Building initial tag cache from reference...");

            modCache.BaseModPackage.CacheNames = new List<string>();
            modCache.BaseModPackage.TagCachesStreams = new List<ExtantStream>();
            modCache.BaseModPackage.TagCacheNames = new List<Dictionary<int, string>>();

            var referenceStream = new MemoryStream(); // will be reused by all base caches
            var writer = new EndianWriter(referenceStream, false);
            var modTagCache = new TagCacheHaloOnline(Cache.Version, referenceStream, modCache.BaseModPackage.StringTable);

            referenceStream.Seek(0, SeekOrigin.End);
            for (var tagIndex = 0; tagIndex < Cache.TagCache.Count; tagIndex++)
            {
                var srcTag = Cache.TagCache.GetTag(tagIndex);

                if (srcTag == null)
                {
                    modTagCache.AllocateTag(new TagGroupGen3());
                    continue;
                }

                var emptyTag = (CachedTagHaloOnline)modTagCache.AllocateTag(srcTag.Group, srcTag.Name);
                var cachedTagData = new CachedTagData
                {
                    Data = new byte[0],
                    Group = (TagGroupGen3)emptyTag.Group,
                };

                var headerSize = CachedTagHaloOnline.CalculateHeaderSize(cachedTagData);
                var alignedHeaderSize = (uint)((headerSize + 0xF) & ~0xF);
                emptyTag.HeaderOffset = referenceStream.Position;
                emptyTag.Offset = alignedHeaderSize;
                emptyTag.TotalSize = alignedHeaderSize;
                emptyTag.WriteHeader(writer, modTagCache.StringTableReference);
                StreamUtil.Fill(referenceStream, 0, (int)(alignedHeaderSize - headerSize));
            }
        
            modTagCache.UpdateTagOffsets(writer);
           

            Console.WriteLine("Done!");

            for (int i = 0; i < tagCacheCount; i++)
            {
                string name = "default";
                if (tagCacheCount > 1 || (args.Count == 1 && useLargeStreams))
                {
                    Console.WriteLine($"Enter the name of tag cache {i} (32 chars max):");
                    name = Console.ReadLine().Trim();
                    name = name.Length <= 32 ? name : name.Substring(0, 32);
                }
                else if (!useDialog)
                    name = modCache.BaseModPackage.Metadata.Name;

                Dictionary<int, string> tagNames = new Dictionary<int, string>();


                foreach (var tag in Cache.TagCache.NonNull())
                    tagNames[tag.Index] = tag.Name;



                referenceStream.Position = 0;
                var ms = referenceStream;
                if (i > 0)
                {
                    ms = new MemoryStream();
                    referenceStream.CopyTo(ms);
                    ms.Position = 0;
                }

                modCache.BaseModPackage.TagCachesStreams.Add(new ExtantStream(ms));
                modCache.BaseModPackage.CacheNames.Add(name);
                modCache.BaseModPackage.TagCacheNames.Add(tagNames);
            }

            modCache.SetActiveTagCache(0);

            ContextStack.Push(TagCacheContextFactory.Create(ContextStack, modCache,
                $"{modCache.BaseModPackage.Metadata.Name}.pak"));

            Program.ErrorCount = 0;
            Program.WarningCount = 0;
            Program._stopWatch.Start();

            return true;

        }
    }
}