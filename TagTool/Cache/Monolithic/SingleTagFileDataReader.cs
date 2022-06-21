using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Commands.Common;

namespace TagTool.Cache.Monolithic
{
    public class SingleTagFileDataReader
    {
        private uint TagId;
        private TagLayout Layout;
        private MemoryStream OutputStream = new MemoryStream();
        private DataSerializationContext SerializationContext;
        private List<SingleTagFileDataFixup> DataFixups = new List<SingleTagFileDataFixup>();
        private ISingleTagFilePersistContext PersistContext;

        public SingleTagFileDataReader(uint tagId, TagLayout layout, ISingleTagFilePersistContext persistContext)
        {
            TagId = tagId;
            Layout = layout;
            PersistContext = persistContext;
        }

        public byte[] ReadAndFixupData(PersistChunkReader reader, out uint mainStructOffset)
        {
            var chunk = reader.ReadNextChunk();
            if (chunk.Header.Signature != "bdat")
                throw new Exception("Invalid chunk signature");

            var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);

            OutputStream = new MemoryStream();
            var outputWriter = new EndianWriter(new MemoryStream(), chunkReader.Format);
            SerializationContext = new DataSerializationContext(outputWriter);

            uint offset = ReadBlock(chunkReader, chunkReader, Layout.RootBlock);

            SerializationContext.EndSerialize(null, OutputStream.ToArray(), offset);
            mainStructOffset = SerializationContext.MainStructOffset;

            return OutputStream.ToArray();
        }

        private uint ReadBlock(PersistChunkReader dataReader, PersistChunkReader reader, TagBlockDefinition definition)
        {
            var chunk = reader.ReadNextChunk();
            if (chunk.Header.Signature != "tgbl")
                throw new Exception("Invalid tag block chunk signature");

            var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
            var blockheader = chunkReader.Deserialize<TagBlockChunkHeader>();

            var data = chunkReader.ReadBytes(definition.Struct.Size * blockheader.ElementCount);
            var newDataReader = new PersistChunkReader(new MemoryStream(data), chunkReader.Format);

            if (!chunkReader.EOF)
            {
                var oldFixups = DataFixups;
                DataFixups = new List<SingleTagFileDataFixup>();

                for (int i = 0; i < blockheader.ElementCount; i++)
                    ReadStruct(newDataReader, chunkReader, definition.Struct);

                var writer = new EndianWriter(new MemoryStream(data), dataReader.Format);
                foreach (var fixup in DataFixups)
                {
                    writer.BaseStream.Position = fixup.Offset;
                    fixup.Apply(PersistContext, writer);
                }

                DataFixups = oldFixups;
            }

            var block = SerializationContext.CreateBlock();
            block.Writer.WriteBlock(data);
            uint offset = block.Finalize(OutputStream);

            return offset;
        }

        private void ReadStruct(PersistChunkReader dataReader, PersistChunkReader reader, TagStructDefinition definition)
        {
            var chunk = reader.ReadNextChunk();
            if (chunk.Header.Signature != "tgst")
                throw new Exception("Invalid tag struct chunk signature");

            if (chunk.Header.Size == 0)
                return;

            var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
            foreach (var field in definition.Fields)
            {
                var offset = (uint)dataReader.Position;

                if (field.FieldType == TagFieldType.Block)
                {
                    uint dataOffset = ReadBlock(dataReader, chunkReader, (TagBlockDefinition)field.Definition);
                    DataFixups.Add(new TagBlockFixup() { Offset = (uint)dataReader.Position, DataOffset = dataOffset });
                }
                else if (field.FieldType == TagFieldType.Struct)
                {
                    ReadStruct(dataReader, chunkReader, (TagStructDefinition)field.Definition);
                }
                else if (field.FieldType == TagFieldType.TagReference)
                {
                    ReadTagReference(dataReader, chunkReader);
                }
                else if (field.FieldType == TagFieldType.StringId)
                {
                    ReadStringId(dataReader, chunkReader);
                }
                else if (field.FieldType == TagFieldType.OldStringId)
                {
                    ReadStringId(dataReader, chunkReader);
                }
                else if (field.FieldType == TagFieldType.Data)
                {
                    ReadTagData(dataReader, chunkReader);
                }
                else if (field.FieldType == TagFieldType.PageableResource)
                {
                    ReadPageableResource(dataReader, chunkReader, (TagResourceDefinition)field.Definition);
                }

                dataReader.SeekTo(offset);
                dataReader.Skip(Layout.GetfieldSize(field));
            }
        }

        private void ReadPageableResource(PersistChunkReader dataReader, PersistChunkReader reader, TagResourceDefinition definition)
        {
            var chunk = reader.ReadNextChunk();
            var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
            if (chunk.Header.Signature == "tgxc") // xsynced
            {
                var resourceHandle = dataReader.ReadDatumIndex();
                var xsyncState = new TagResourceXSyncState(TagId, chunkReader);
                PersistContext.AddTagResource(resourceHandle, xsyncState);
            }
            else if (chunk.Header.Signature == "tg\0c")
            {
                // unused
            }
            else
            {
                new TagToolWarning($"Unsupported pageable resource chunk signature '{chunk.Header.Signature}'");
            }
        }

        private void ReadTagData(PersistChunkReader dataReader, PersistChunkReader reader)
        {
            var chunk = reader.ReadNextChunk();
            if (chunk.Header.Signature != "tgda")
                throw new Exception("Invalid tag data chunk signature");


            var chunkReader = new EndianReader(chunk.Stream, dataReader.Format);
            var data = chunkReader.ReadBytes(chunk.Header.Size);

            var block = SerializationContext.CreateBlock();
            block.Writer.WriteBlock(data);
            uint offset = block.Finalize(OutputStream);

            var address = new CacheAddress(CacheAddressType.Data, (int)offset);

            DataFixups.Add(new TagDataFixup() { Offset = (uint)dataReader.Position, DataOffset = address.Value, Size = data.Length });
        }
        private void ReadOldStringId(PersistChunkReader reader)
        {
            var chunk = reader.ReadNextChunk();
            if (chunk.Header.Signature != "tgsi")
                throw new Exception("Invalid tag reference chunk signature");

            if (chunk.Header.Size == 0)
                return;

            var chunkReader = new EndianReader(chunk.Stream, reader.Format);
            var fixup = new StringFixup();
            fixup.StringValue = chunkReader.ReadString(chunk.Header.Size);
            DataFixups.Add(fixup);
        }

        private void ReadStringId(PersistChunkReader dataReader, PersistChunkReader reader)
        {
            var chunk = reader.ReadNextChunk();
            if (chunk.Header.Signature != "tgsi")
                throw new Exception("Invalid tag reference chunk signature");

            if (chunk.Header.Size == 0)
                return;

            var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
            var str = chunkReader.ReadString(128);
            DataFixups.Add(new StringIdFixup() { Offset = (uint)dataReader.Position, StringValue = str });
        }

        private void ReadTagReference(PersistChunkReader dataReader, PersistChunkReader reader)
        {
            var chunk = reader.ReadNextChunk();
            if (chunk.Header.Signature != "tgrf")
                throw new Exception("Invalid tag reference chunk signature");

            if (chunk.Header.Size == 0)
                return;

            var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
            var fixup = new TagReferenceFixup();
            fixup.Offset = (uint)dataReader.Position;
            fixup.GroupTag = chunkReader.ReadTag();
            fixup.TagName = chunkReader.ReadString((int)chunkReader.Length - 4);
            DataFixups.Add(fixup);
        }

        abstract class SingleTagFileDataFixup
        {
            public uint Offset;
            public abstract void Apply(ISingleTagFilePersistContext context, EndianWriter writer);
        }

        class TagBlockFixup : SingleTagFileDataFixup
        {
            public uint DataOffset;

            public override void Apply(ISingleTagFilePersistContext context, EndianWriter writer)
            {
                writer.BaseStream.Position += 4;
                writer.Write(DataOffset);
            }
        }

        class TagDataFixup : SingleTagFileDataFixup
        {
            public uint DataOffset;
            public int Size;
            public override void Apply(ISingleTagFilePersistContext context, EndianWriter writer)
            {
                writer.Write(Size);
                writer.Write(0);
                writer.Write(0);
                writer.Write(DataOffset);
                writer.Write(0);
            }
        }

        class StringFixup : SingleTagFileDataFixup
        {
            public string StringValue;
            public override void Apply(ISingleTagFilePersistContext context, EndianWriter writer)
            {
                var bytes = Encoding.UTF8.GetBytes(StringValue);
                if (bytes.Length > 32)
                    throw new Exception("Max string length exceeded");

                writer.Write(bytes);
            }
        }

        class StringIdFixup : SingleTagFileDataFixup
        {
            public string StringValue;

            public override void Apply(ISingleTagFilePersistContext context, EndianWriter writer)
            {
                var stringId = context.AddStringId(StringValue);
                writer.Write(stringId.Value);
            }
        }

        class TagReferenceFixup : SingleTagFileDataFixup
        {
            public Tag GroupTag;
            public string TagName;

            public override void Apply(ISingleTagFilePersistContext context, EndianWriter writer)
            {
                var tag = context.GetTag(GroupTag, TagName);

                Tag groupTag = tag?.Group.Tag ?? Tag.Null;
                uint tagId = tag?.ID ?? uint.MaxValue;

                writer.Write(groupTag.Value);
                writer.Write(0);
                writer.Write(0);
                writer.Write(tagId);
            }
        }
    }
}
