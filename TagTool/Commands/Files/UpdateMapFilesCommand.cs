using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Files
{
    class UpdateMapFilesCommand : Command
    {
        private const int MapNameOffset = 0x01A4;
        private const int ScenarioPathOffset = 0x01C8;
        private const int ScenarioMapIdOffset = 0x2DEC;
        private const int ScenarioTagIndexOffset = 0x2DF0;

        private const int ContentNameOffset = 0x42D4;
        private const int ContentMapNameOffset = 0x43D4;
        private static readonly int[] ContentMapIdOffsets = new[] { 0x33CC, 0xBE60, 0xBE80 };

        public HaloOnlineCacheContext CacheContext { get; }

        public UpdateMapFilesCommand(HaloOnlineCacheContext cacheContext)
            : base(true,

                  "UpdateMapFiles",
                  "Updates the game's .map files to contain valid scenario indices.",

                  "UpdateMapFiles <MapInfo Directory>",

                  "Updates the game's .map files to contain valid scenario indices.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
            {
                return false;
            }

            var scenarioIndices = new Dictionary<int, (ScenarioMapType, int)>();

            using (var stream = CacheContext.OpenTagCacheRead())
            using (var reader = new BinaryReader(stream))
            {
                foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
                {
                    if (tag.HeaderOffset == -1)
                        continue;

                    reader.BaseStream.Position = tag.HeaderOffset + tag.DefinitionOffset;
                    var mapType = reader.ReadEnum<ScenarioMapType>();

                    reader.BaseStream.Position = tag.HeaderOffset + tag.DefinitionOffset + 0x8;
                    var mapId = reader.ReadInt32();

                    scenarioIndices[mapId] = (mapType, tag.Index);
                }
            }

            var mainmenuMapFile = CacheContext.Directory.GetFiles("mainmenu.map")[0];
            var guardianMapFile = CacheContext.Directory.GetFiles("guardian.map")[0];

            string[] odstmaps = { "c100", "c200", "h100", "l200", "l300", "sc100", "sc110", "sc120", "sc130", "sc140", "sc150" };
            var odstlist = new List<string>(odstmaps);

            foreach (var entry in scenarioIndices)
            {
                var scnrTag = CacheContext.GetTag(entry.Value.Item2);
                var scenarioPath = scnrTag?.Name ?? $"0x{entry.Value:X4}";

                var mapName = scenarioPath.Split('\\').Last();
                var mapFile = new FileInfo(Path.Combine(CacheContext.Directory.FullName, $"{mapName}.map"));

                var isodstmap = false;
                if (odstlist.Contains(mapName))
                    isodstmap = true;

                if (!mapFile.Exists)
                    mapFile = ((entry.Value.Item1 == ScenarioMapType.Multiplayer) ? guardianMapFile : mainmenuMapFile).CopyTo(mapFile.FullName);

                DirectoryInfo mapInfoDir = null;

                if (args.Count == 1)
                {
                    mapInfoDir = new DirectoryInfo(args[0]);
                }
                else
                {
                    if (Directory.Exists(Path.Combine(CacheContext.Directory.FullName, "info")))
                    {
                        mapInfoDir = new DirectoryInfo(Path.Combine(CacheContext.Directory.FullName, "info"));
                    }
                }

                using (var stream = mapFile.Open(FileMode.Open, FileAccess.ReadWrite))
                using (var reader = new BinaryReader(stream))
                using (var writer = new BinaryWriter(stream))
                {
                    if (reader.ReadInt32() != new Tag("head").Value)
                        continue;

                    stream.Position = MapNameOffset;
                    for (var i = 0; i < 32; i++)
                        writer.Write(i < mapName.Length ? mapName[i] : '\0');

                    stream.Position = ScenarioPathOffset;
                    for (var i = 0; i < 256; i++)
                        writer.Write(i < scenarioPath.Length ? scenarioPath[i] : '\0');

                    stream.Position = ScenarioMapIdOffset;
                    writer.Write(entry.Key);

                    stream.Position = ScenarioTagIndexOffset;
                    writer.Write(entry.Value.Item2);

                    bool using_mapinfo = false;

                    if (mapInfoDir != null)
                    {
                        var mapInfoFiles = mapInfoDir.GetFiles(mapName + ".mapinfo");

                        if (mapInfoFiles != null && mapInfoFiles.Length > 0)
                        {
                            var mapInfoFile = mapInfoFiles[0];
                            using_mapinfo = true;

                            using (var infoStream = mapInfoFile.OpenRead())
                            using (var infoReader = new BinaryReader(infoStream))
                            {
                                if (entry.Value.Item1 == ScenarioMapType.SinglePlayer)
                                {
                                    infoStream.Position = 0x0;
                                    stream.Position = 0x3390;
                                    writer.Write(infoReader.ReadBytes(0x3C)); //mapinfo header
                                    //stream.Position = 0x33cc;

                                    writer.Write(entry.Key); //scenario map id
                                    infoStream.Position = 0x40; //advance infostream past map id
                                    var zone = infoReader.ReadBytes(4);
                                    Array.Reverse(zone); //this must be swapped for the map file to work
                                    writer.Write(zone);

                                    //writer.Write(infoReader.ReadBytes(0x1120)); //cache file metadata
                                    stream.Position = 0x44f0;
                                    var infocontentbase = 0x1160;
                                    var contententry = 0;

                                    //each content entry is 0xF10 in ODST, 0xF08 in H3
                                    //There are 9 content entries in ODST and 3 in H3
                                    var contentnames = new byte[12][];
                                    var contentdescs = new byte[12][];
                                    if (isodstmap)
                                    {
                                        while (contententry < 9)
                                        {
                                            infoStream.Position = (infocontentbase + contententry * 0xf10);
                                            writer.Write(infoReader.ReadBytes(0x10));

                                            for (var i = 0; i < 12; i++)
                                                contentnames[i] = infoReader.ReadBytes(0x40);
                                            foreach (var contentnameUnicode in contentnames)
                                            {
                                                for (var c = 0; c < contentnameUnicode.Length; c += 2)
                                                {
                                                    var temp = contentnameUnicode[c];
                                                    contentnameUnicode[c] = contentnameUnicode[c + 1];
                                                    contentnameUnicode[c + 1] = temp;
                                                }
                                                writer.Write(contentnameUnicode);
                                            }

                                            for (var i = 0; i < 12; i++)
                                            {
                                                contentdescs[i] = infoReader.ReadBytes(0x100);
                                            }
                                            foreach (var contentdesc in contentdescs)
                                            {
                                                for (var c = 0; c < contentdesc.Length; c += 2)
                                                {
                                                    var temp = contentdesc[c];
                                                    contentdesc[c] = contentdesc[c + 1];
                                                    contentdesc[c + 1] = temp;
                                                }

                                                writer.Write(contentdesc);
                                            }
                                            contententry += 1;
                                        }
                                    }
                                    else
                                    {
                                        while (contententry < 3)
                                        {
                                            infoStream.Position = (infocontentbase + contententry * 0xf08);
                                            writer.Write(infoReader.ReadBytes(0x8));
                                            writer.Write(0); //padding
                                            writer.Write(0); //padding
                                            for (var i = 0; i < 12; i++)
                                                contentnames[i] = infoReader.ReadBytes(0x40);
                                            foreach (var contentnameUnicode in contentnames)
                                            {
                                                for (var c = 0; c < contentnameUnicode.Length; c += 2)
                                                {
                                                    var temp = contentnameUnicode[c];
                                                    contentnameUnicode[c] = contentnameUnicode[c + 1];
                                                    contentnameUnicode[c + 1] = temp;
                                                }
                                                writer.Write(contentnameUnicode);
                                            }

                                            for (var i = 0; i < 12; i++)
                                            {
                                                contentdescs[i] = infoReader.ReadBytes(0x100);
                                            }
                                            foreach (var contentdesc in contentdescs)
                                            {
                                                for (var c = 0; c < contentdesc.Length; c += 2)
                                                {
                                                    var temp = contentdesc[c];
                                                    contentdesc[c] = contentdesc[c + 1];
                                                    contentdesc[c + 1] = temp;
                                                }

                                                writer.Write(contentdesc);
                                            }
                                            contententry += 1;
                                        }
                                    }
                                }
                                var mapNames = new byte[12][];

                                infoStream.Position = 0x44;
                                for (var i = 0; i < 12; i++)
                                    mapNames[i] = infoReader.ReadBytes(0x40);

                                stream.Position = 0x33D4;
                                foreach (var mapNameUnicode in mapNames)
                                {
                                    for (var c = 0; c < mapNameUnicode.Length; c += 2)
                                    {
                                        var temp = mapNameUnicode[c];
                                        mapNameUnicode[c] = mapNameUnicode[c + 1];
                                        mapNameUnicode[c + 1] = temp;
                                    }

                                    writer.Write(mapNameUnicode);
                                }

                                var mapDescriptions = new byte[12][];

                                infoStream.Position = 0x344;
                                for (var i = 0; i < 12; i++)
                                {
                                    mapDescriptions[i] = infoReader.ReadBytes(0x100);
                                }

                                stream.Position = 0x36D4;
                                foreach (var mapDescription in mapDescriptions)
                                {
                                    for (var c = 0; c < mapDescription.Length; c += 2)
                                    {
                                        var temp = mapDescription[c];
                                        mapDescription[c] = mapDescription[c + 1];
                                        mapDescription[c + 1] = temp;
                                    }

                                    writer.Write(mapDescription);
                                }

                                stream.Position = 0xBD88;
                                writer.Write(mapNames[0], 0, 32);

                                var description = new string(mapDescriptions[0].Select(i => Convert.ToChar(i)).ToArray()).Replace("\0", "");

                                stream.Position = 0xBDA8;
                                writer.Write(description.ToArray());

                                for (var i = 0; i < (0x80 - description.Length); i++)
                                    writer.Write('\0');
                            }
                        }
                    }

                    if (entry.Value.Item1 == ScenarioMapType.Multiplayer)
                    {
                        var contentName = scenarioPath.StartsWith("levels\\dlc\\") ?
                            $"dlc_{mapName}" :
                            $"m_{mapName}";

                        stream.Position = ContentNameOffset;
                        for (var i = 0; i < 256; i++)
                            writer.Write(i < contentName.Length ? contentName[i] : '\0');

                        stream.Position = ContentMapNameOffset;
                        for (var i = 0; i < 256; i++)
                            writer.Write(i < mapName.Length ? mapName[i] : '\0');

                        foreach (var offset in ContentMapIdOffsets)
                        {
                            stream.Position = offset;
                            writer.Write(entry.Key);
                        }
                    }

                    if (using_mapinfo)
                    {
                        Console.WriteLine($"Scenario tag index for {mapFile.Name}: 0x{entry.Value.Item2:X4} (using map info)");
                    }
                    else
                    {
                        Console.WriteLine($"Scenario tag index for {mapFile.Name}: 0x{entry.Value.Item2:X4} (WARNING: not using map info)");
                    }

                }
            }

            Console.WriteLine("Done!");

            return true;
        }
    }
}