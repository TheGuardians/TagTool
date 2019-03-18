using TagTool.Cache;
using TagTool.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Porting
{
    public class OpenCacheFileCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private HaloOnlineCacheContext CacheContext { get; }

        public OpenCacheFileCommand(CommandContextStack contextStack, HaloOnlineCacheContext cacheContext)
            : base(false,

                  "OpenCacheFile",
                  "Opens a porting context with the specified cache file.",

                  "OpenCacheFile <Cache File>",

                  "Opens a porting context with the specified cache file.")
        {
            ContextStack = contextStack;
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            var memory = false;

            while (args.Count > 1)
            {
                switch (args[0].ToLower())
                {
                    case "memory":
                        memory = true;
                        break;

                    default:
                        throw new FormatException(args[0]);
                }

                args.RemoveAt(0);
            }

            var blamCacheFile = new FileInfo(args[0]);

            if (!blamCacheFile.Exists)
            {
                Console.WriteLine($"CacheFile {blamCacheFile.FullName} does not exist");
                return true;
            }
                
            Console.Write("Loading blam cache file...");

            CacheFile blamCache = null;

            using (var cacheStream = new FileStream(blamCacheFile.FullName, FileMode.Open, FileAccess.Read))
            {
                var reader = new EndianReader(cacheStream, EndianFormat.BigEndian);

                var head = reader.ReadInt32();

                if (head == 1684104552)
                    reader.Format = EndianFormat.LittleEndian;

                var v = reader.ReadInt32();

                switch (v)
                {
                    case 8: // Gen2
                        reader.SeekTo(36);
                        switch (reader.ReadInt32())
                        {
                            case 0: // Halo 2 Xbox
                                reader.SeekTo(288);
                                break;

                            case -1: // Halo 2 Vista
                                reader.SeekTo(300);
                                break;
                        }
                        break;

                    default: // Gen3+
                        reader.SeekTo(284);
                        break;
                }

                var version = CacheVersionDetection.GetFromBuildName(reader.ReadString(32));
                
                switch (version)
                {
                    case CacheVersion.Halo2Xbox:
                    case CacheVersion.Halo2Vista:
                        if (blamCacheFile.Name != "mainmenu.map")
                            new CacheFileGen2(CacheContext, new FileInfo(Path.Combine(blamCacheFile.Directory.FullName, "mainmenu.map")), version, memory);
                        if (blamCacheFile.Name != "shared.map")
                            new CacheFileGen2(CacheContext, new FileInfo(Path.Combine(blamCacheFile.Directory.FullName, "shared.map")), version, memory);
                        if (blamCacheFile.Name != "single_player_shared.map")
                            new CacheFileGen2(CacheContext, new FileInfo(Path.Combine(blamCacheFile.Directory.FullName, "single_player_shared.map")), version, memory);
                        blamCache = new CacheFileGen2(CacheContext, blamCacheFile, version, memory);
                        break;
                        
                    case CacheVersion.Halo3Retail:
                    case CacheVersion.Halo3ODST:
                    case CacheVersion.HaloReach:
                        blamCache = new CacheFileGen3(CacheContext, blamCacheFile, version, memory);
                        break;

                    default: // Same question here as above.
                        throw new NotSupportedException(CacheVersionDetection.GetBuildName(version));
                }
            }

            ContextStack.Push(PortingContextFactory.Create(ContextStack, CacheContext, blamCache));

            Console.WriteLine("done.");

            return true;
        }
    }
}

