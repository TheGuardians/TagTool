using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;
using TagTool.IO;
using TagTool.BlamFile;

namespace TagTool.Cache.Gen3
{
    public class LocalesTableGen3
    {
        public static List<LocaleTable> CreateLocalesTable(EndianReader reader, MapFile baseMapFile, TagCacheGen3 tagCache)
        {
            CachedTagGen3 matg = null;
            foreach (var tag in tagCache.Tags)
                if (tag.IsInGroup("matg"))
                {
                    matg = tag;
                    break;
                }

            List<LocaleTable> localesTable = new List<LocaleTable>();
            string localesKey = "";
            uint localeGlobalsOffset = 0;
            uint localeGlobalsSize = 0;
            uint matgOffset = matg.DefinitionOffset;
            var interop = baseMapFile.Header.GetInterop();

            switch (baseMapFile.Version)
            {
                case CacheVersion.Halo3Retail:
                    localesKey = "";
                    localeGlobalsOffset = 452;
                    localeGlobalsSize = 68;
                    break;
                case CacheVersion.Halo3ODST:
                    localesKey = "";
                    localeGlobalsOffset = 508;
                    localeGlobalsSize = 68;
                    break;
                case CacheVersion.HaloReach:
                    localesKey = "BungieHaloReach!";
                    localeGlobalsOffset = 656;
                    localeGlobalsSize = 68;
                    break;
            }

            foreach (var language in Enum.GetValues(typeof(GameLanguage)))
            {
                LocaleTable table = new LocaleTable();

                reader.SeekTo(matgOffset + localeGlobalsOffset + ((int)language * localeGlobalsSize));

                var localeCount = reader.ReadInt32();
                var tableSize = reader.ReadInt32();
                var indexOffset = (int)(reader.ReadInt32() + interop.UnknownBaseAddress);
                var tableOffset = (int)(reader.ReadInt32() + interop.UnknownBaseAddress);

                reader.SeekTo(indexOffset);
                var indices = new int[localeCount];

                for (var i = 0; i < localeCount; i++)
                {
                    table.Add(new CacheLocalizedStringTest(reader.ReadInt32(), "", i));
                    indices[i] = reader.ReadInt32();
                }

                reader.SeekTo(tableOffset);

                EndianReader newReader = null;

                if (localesKey == "")
                {
                    newReader = new EndianReader(new MemoryStream(reader.ReadBytes(tableSize)), EndianFormat.BigEndian);
                }
                else
                {
                    reader.BaseStream.Position = tableOffset;
                    newReader = new EndianReader(reader.DecryptAesSegment(tableSize, localesKey));
                }

                for (var i = 0; i < indices.Length; i++)
                {
                    if (indices[i] == -1)
                    {
                        table[i].String = "<null>";
                        continue;
                    }

                    newReader.SeekTo(indices[i]);

                    int length;
                    if (i == indices.Length - 1)
                        length = tableSize - indices[i];
                    else
                        length = (indices[i + 1] != -1)
                            ? indices[i + 1] - indices[i]
                            : indices[i + 2] - indices[i];

                    if (length == 1)
                    {

                        table[i].String = "<blank>";
                        continue;
                    }
                    table[i].String = newReader.ReadString(length);
                }

                newReader.Close();
                newReader.Dispose();

                localesTable.Add(table);
            }

            return localesTable;
        }
    }

}
