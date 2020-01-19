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
                    throw new NotSupportedException(CacheVersionDetection.GetBuildName(Version));
            }

            reader.SeekTo(baseMapFile.Header.StringIDsIndicesOffset);
            int[] indices = new int[baseMapFile.Header.StringIDsCount];
            for (var i = 0; i < baseMapFile.Header.StringIDsCount; i++)
            {
                indices[i] = reader.ReadInt32();
                Add("");
            }

            reader.SeekTo(baseMapFile.Header.StringIDsBufferOffset);

            EndianReader newReader;

            if (StringKey == "")
            {
                newReader = new EndianReader(new MemoryStream(reader.ReadBytes(baseMapFile.Header.StringIDsBufferSize)), reader.Format);
            }
            else
            {
                reader.BaseStream.Position = baseMapFile.Header.StringIDsBufferOffset;
                newReader = new EndianReader(reader.DecryptAesSegment(baseMapFile.Header.StringIDsBufferSize, StringKey), reader.Format);
            }

            for (var i = 0; i < indices.Length; i++)
            {
                if (indices[i] == -1)
                {
                    this[i] = "<null>";
                    continue;
                }

                newReader.SeekTo(indices[i]);

                int length;
                if (i == indices.Length - 1)
                    length = baseMapFile.Header.StringIDsBufferSize - indices[i];
                else
                    length = (indices[i + 1] != -1)
                        ? indices[i + 1] - indices[i]
                        : indices[i + 2] - indices[i];

                if (length == 1)
                {
                    this[i] = "";
                    continue;
                }

                this[i] = newReader.ReadString(length);
            }
            newReader.Close();
            newReader.Dispose();
        }

        public override StringId AddString(string newString)
        {
            throw new NotImplementedException();
        }
    }
}
