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
            //
            // Read offsets
            //

            reader.SeekTo(baseMapFile.Header.StringIDsIndicesOffset);

            int[] stringOffset = new int[baseMapFile.Header.StringIDsCount];
            for (var i = 0; i < baseMapFile.Header.StringIDsCount; i++)
            {
                stringOffset[i] = reader.ReadInt32();
                Add("");
            }

            reader.SeekTo(baseMapFile.Header.StringIDsBufferOffset);

            EndianReader newReader = new EndianReader(new MemoryStream(reader.ReadBytes(baseMapFile.Header.StringIDsBufferSize)), reader.Format);

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
