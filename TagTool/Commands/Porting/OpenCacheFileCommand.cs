using TagTool.Cache;
using TagTool.Commands;
using TagTool.IO;
using TagTool.Legacy.Base;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Commands.Porting
{
    public class OpenCacheFileCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private GameCacheContext CacheContext { get; }

        public OpenCacheFileCommand(CommandContextStack contextStack, GameCacheContext cacheContext)
            : base(CommandFlags.None,

                  "OpenCacheFile",
                  "Opens a porting context on a cache file from H3B/H3/ODST.",

                  "OpenCacheFile <Cache File>",

                  "Opens a porting context on a cache file from H3B/H3/ODST.")
        {
            ContextStack = contextStack;
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var blamCacheFile = new FileInfo(args[0]);

            if (!blamCacheFile.Exists)
                throw new FileNotFoundException(blamCacheFile.FullName);

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
                        blamCache = new TagTool.Legacy.Halo2Xbox.CacheFile(blamCacheFile, version);
                        break;

                    case CacheVersion.Halo2Vista:
                        blamCache = new TagTool.Legacy.Halo2Vista.CacheFile(blamCacheFile, version);
                        break;

                    case CacheVersion.Halo3Retail:
                        blamCache = new TagTool.Legacy.Halo3Retail.CacheFile(blamCacheFile, CacheContext, version);
                        break;

                    case CacheVersion.Halo3ODST:
                        blamCache = new TagTool.Legacy.Halo3ODST.CacheFile(blamCacheFile, CacheContext, version);
                        break;

                    case CacheVersion.HaloReach:
                        blamCache = new TagTool.Legacy.HaloReach.CacheFile(blamCacheFile, CacheContext, version);
                        break;

                    default:
                        throw new NotSupportedException(CacheVersionDetection.GetBuildName(version));
                }
            }

            ContextStack.Push(PortingContextFactory.Create(ContextStack, CacheContext, blamCache));

            Console.WriteLine("done.");

            return true;
        }
    }
}

