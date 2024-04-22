using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.BlamFile;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Cache.Gen2
{
    public class StringTableGen2 : StringTable
    {
        public StringTableGen2(EndianReader reader, MapFile baseMapFile) : base()
        {
            Resolver = new StringIdResolverHalo2();

            //
            // Read offsets
            //

            var stringIDHeader = baseMapFile.Header.GetStringIDHeader();

            reader.SeekTo(stringIDHeader.IndicesOffset);

            int[] stringOffset = new int[stringIDHeader.Count];
            for (var i = 0; i < stringIDHeader.Count; i++)
            {
                stringOffset[i] = reader.ReadInt32();
                Add("");
            }

            reader.SeekTo(stringIDHeader.BufferOffset);

            EndianReader newReader = new EndianReader(new MemoryStream(reader.ReadBytes(stringIDHeader.BufferSize)), reader.Format);

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


        public override StringId AddString(string newString)
        {
            throw new NotImplementedException();
        }
    }
}
