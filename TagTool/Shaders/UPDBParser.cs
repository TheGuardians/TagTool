using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Shaders
{
    class UPDBParser
    {
        // Primary Block
        public Int32 Magic { get; internal set; }
        public Int32 UDPSize { get; internal set; }
        public Int32 TagBlockSize { get; internal set; }
        public Int32 Unknown1 { get; internal set; }
        public Int32 Unknown2 { get; internal set; }
        public Int32 UnknownBlockOffset { get; internal set; }
        public Int32 GroupBlockOffset { get; internal set; }
        public Int32 Unknown3 { get; internal set; }
        public Int32 Unknown4 { get; internal set; }
        public Int32 Unknown5 { get; internal set; }
        public Int32 FilenameLength { get; internal set; }
        public String Filename { get; internal set; }

        // Unknown Block

        // Group Block
        public Int32 ConstantBlockSize { get; internal set; }
        public Int32 ShaderFileSize { get; internal set; }
        public Int32 Unknown6 { get; internal set; }
        public Int32 Unknown7 { get; internal set; }
        public Int32 Unknown8 { get; internal set; }
        public Int32 Unknown9 { get; internal set; }
        public Int32 NumGroupA { get; internal set; }
        public Int32 NumGroupB { get; internal set; }
        public Int32 NumGroupC { get; internal set; }

        public List<GroupABlock> GroupA = null;
        public List<GroupBBlock> GroupB = null;
        public List<GroupCBlock> GroupC = null;

        public UPDBParser(IEnumerable<byte> _data)
        {
            if (_data == null) return;
            var data = _data.GetType() == typeof(byte[]) ? _data as byte[] : _data.ToArray();
            using (MemoryStream stream = new MemoryStream(data, false))
            using (EndianReader reader = new EndianReader(stream, EndianFormat.BigEndian))
            {
                stream.Seek(0, SeekOrigin.Begin);
                Magic = reader.ReadInt32();
                UDPSize = reader.ReadInt32();
                TagBlockSize = reader.ReadInt32();
                Unknown1 = reader.ReadInt32();
                Unknown2 = reader.ReadInt32();
                UnknownBlockOffset = reader.ReadInt32();
                GroupBlockOffset = reader.ReadInt32();
                Unknown3 = reader.ReadInt32();
                Unknown4 = reader.ReadInt32();
                Unknown5 = reader.ReadInt32();
                FilenameLength = reader.ReadInt32();
                var filename_bytes = reader.ReadBytes(FilenameLength);
                Filename = Encoding.UTF8.GetString(filename_bytes);

                if(UnknownBlockOffset != 0)
                {
                    stream.Seek(UnknownBlockOffset, SeekOrigin.Begin);
                    // Do stuff but I have no idea what to do with this data
                }
                
                stream.Seek(GroupBlockOffset, SeekOrigin.Begin);
                ConstantBlockSize = reader.ReadInt32();
                ShaderFileSize = reader.ReadInt32();
                {
                    Unknown6 = reader.ReadInt32();
                    Unknown7 = reader.ReadInt32();
                    Unknown8 = reader.ReadInt32();
                    Unknown9 = reader.ReadInt32();
                }
                NumGroupA = reader.ReadInt32();
                NumGroupB = reader.ReadInt32();
                NumGroupC = reader.ReadInt32();

                GroupA = GroupABlock.ReadGroup(reader, NumGroupA);
                GroupB = GroupBBlock.ReadGroup(reader, NumGroupB);
                GroupC = GroupCBlock.ReadGroup(reader, NumGroupC);

            }


        }

        public struct GroupABlock
        {
            Int32[] Data;

            private GroupABlock(BinaryReader reader)
            {
                Data = new Int32[] { reader.ReadInt32() };
            }

            public static List<GroupABlock> ReadGroup(BinaryReader reader, int count)
            {
                List<GroupABlock> list = new List<GroupABlock>();
                for (var i = 0; i < count; i++)
                    list.Add(new GroupABlock(reader));
                return list;
            }
        }

        public struct GroupBBlock
        {
            Int32[] Data;

            private GroupBBlock(BinaryReader reader)
            {
                Data = new Int32[] { reader.ReadInt32() };
            }

            public static List<GroupBBlock> ReadGroup(BinaryReader reader, int count)
            {
                List<GroupBBlock> list = new List<GroupBBlock>();
                for (var i = 0; i < count; i++)
                    list.Add(new GroupBBlock(reader));
                return list;
            }
        }

        public struct GroupCBlock
        {
            Int32[] Data;

            private GroupCBlock(BinaryReader reader)
            {
                Data = new Int32[] { reader.ReadInt32(), reader.ReadInt32() };
            }

            public static List<GroupCBlock> ReadGroup(BinaryReader reader, int count)
            {
                List<GroupCBlock> list = new List<GroupCBlock>();
                for (var i = 0; i < count; i++)
                    list.Add(new GroupCBlock(reader));
                return list;
            }
        }
    }
}
