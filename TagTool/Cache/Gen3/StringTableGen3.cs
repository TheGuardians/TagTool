using System;
using System.IO;
using TagTool.Common;
using TagTool.IO;
using TagTool.BlamFile;

namespace TagTool.Cache.Gen3
{
    public class StringTableGen3 : StringTable
    {
        public string StringKey = "";

        public StringTableGen3(EndianReader reader, MapFile baseMapFile) : base()
        {
            Version = baseMapFile.Version;
            
            var gen3Header = (CacheFileHeaderGen3)baseMapFile.Header;
            var stringIDHeader = gen3Header.GetStringIDHeader();

            var cachePlatform = baseMapFile.CachePlatform;

            if(cachePlatform == CachePlatform.Original)
            {
                switch (Version)
                {
                    case CacheVersion.Halo3Beta:
                        Resolver = new StringIdResolverHalo3Beta();
                        break;

                    case CacheVersion.Halo3Retail:
                        Resolver = new StringIdResolverHalo3();
                        break;

                    case CacheVersion.Halo3ODST:
                        Resolver = new StringIdResolverHalo3ODST();
                        break;

                    case CacheVersion.HaloReach:
                        Resolver = new StringIdResolverHaloReach();
                        StringKey = "ILikeSafeStrings";
                        break;

                    default:
                        throw new NotSupportedException(CacheVersionDetection.GetBuildName(Version, cachePlatform));
                }
            }
            else if(cachePlatform == CachePlatform.MCC)
            {
                switch (Version)
                {
                    case CacheVersion.Halo3Retail:
                        Resolver = new StringIdResolverHalo3MCC();
                        break;

                    case CacheVersion.Halo3ODST:
                        Resolver = new StringIdResolverHalo3ODSTMCC();
                        break;

                    case CacheVersion.HaloReach:
                        Resolver = new StringIdResolverHaloReachMCC();
                        break;

                    default:
                        throw new NotSupportedException(CacheVersionDetection.GetBuildName(Version, cachePlatform));
                }
            }

            var sectionTable = gen3Header.SectionTable;

            // means no strings
            if (sectionTable != null && sectionTable.Sections[(int)CacheFileSectionType.StringSection].Size == 0)
                return;

            uint stringIdIndexTableOffset;
            uint stringIdBufferOffset;
            if (Version > CacheVersion.Halo3Beta)
            {
                stringIdIndexTableOffset = sectionTable.GetOffset(CacheFileSectionType.StringSection, stringIDHeader.IndicesOffset);
                stringIdBufferOffset = sectionTable.GetOffset(CacheFileSectionType.StringSection, stringIDHeader.BufferOffset);
            }
            else
            {
                stringIdIndexTableOffset = stringIDHeader.IndicesOffset;
                stringIdBufferOffset = stringIDHeader.BufferOffset;
            }
            

            //
            // Read offsets
            //

            reader.SeekTo(stringIdIndexTableOffset);

            int[] stringOffset = new int[stringIDHeader.Count];
            for (var i = 0; i < stringIDHeader.Count; i++)
            {
                stringOffset[i] = reader.ReadInt32();
                Add("");
            }

            reader.SeekTo(stringIdBufferOffset);

            EndianReader newReader;

            if (StringKey == "")
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(stringIDHeader.BufferSize)), reader.Format);
            else
                newReader = new EndianReader(reader.DecryptAesSegment(stringIDHeader.BufferSize, StringKey), reader.Format);

            //
            // Read strings
            //

            for (var i = 0; i < stringOffset.Length; i++)
            {
                if (stringOffset[i] == -1)
                {
                    this[i] = "<null>";
                    continue;
                }

                newReader.SeekTo(stringOffset[i]);
                this[i] = newReader.ReadNullTerminatedString();
            }
            newReader.Close();
            newReader.Dispose();
        }

        /*
         * To resize the stringId Buffer in Gen3 caches, there are a few values to update. The map file section table needs to be updated
         * The 2 addresses for the buffer and index table in the map file header.
         */

        public override StringId AddString(string newString)
        {
            throw new NotImplementedException();
        }
    }
}
