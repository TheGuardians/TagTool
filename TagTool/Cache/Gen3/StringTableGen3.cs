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

            switch (Version)
            {
                case CacheVersion.Halo3Retail:
                case CacheVersion.Halo3Beta:    // double check
                    Resolver = new StringIdResolverHalo3();
                    break;

                case CacheVersion.Halo3ODST:
                    Resolver = new StringIdResolverHalo3ODST();
                    break;

                case CacheVersion.HaloReach:
                    Resolver = new StringIdResolverHaloReach();
                    StringKey = "ILikeSafeStrings";
                    break;

                case CacheVersion.HaloReachMCC0824:
                case CacheVersion.HaloReachMCC0887:
                case CacheVersion.HaloReachMCC1035:
                case CacheVersion.HaloReachMCC1211:
                    Resolver = new StringIdResolverHaloReachMCC();
                    break;

                default:
                    throw new NotSupportedException(CacheVersionDetection.GetBuildName(Version));
            }

            var sectionTable = baseMapFile.Header.SectionTable;

            // means no strings
            if (sectionTable.Sections[(int)CacheFileSectionType.StringSection].Size == 0)
                return;

            var stringIdIndexTableOffset = sectionTable.GetOffset(CacheFileSectionType.StringSection, baseMapFile.Header.StringIDsIndicesOffset);
            var stringIdBufferOffset = sectionTable.GetOffset(CacheFileSectionType.StringSection, baseMapFile.Header.StringIDsBufferOffset);

            //
            // Read offsets
            //

            reader.SeekTo(stringIdIndexTableOffset);

            int[] stringOffset = new int[baseMapFile.Header.StringIDsCount];
            for (var i = 0; i < baseMapFile.Header.StringIDsCount; i++)
            {
                stringOffset[i] = reader.ReadInt32();
                Add("");
            }

            reader.SeekTo(stringIdBufferOffset);

            EndianReader newReader;

            if (StringKey == "")
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(baseMapFile.Header.StringIDsBufferSize)), reader.Format);
            else
                newReader = new EndianReader(reader.DecryptAesSegment(baseMapFile.Header.StringIDsBufferSize, StringKey), reader.Format);

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
