using System;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public class TagPersistLayout
    {
        public TagPersistLayoutHeader LayoutHeader;

        public StringBuffer StringBuffer;
        public int[] StringOffsets;
        public TagPersistStringList[] StringLists;
        public TagPersistFieldType[] FieldTypes;
        public TagPersistField[] Fields;
        public TagPersistBlockDefinition[] Blocks;
        public TagPersistStructDefinition[] Structs;
        public TagPersistArrayDefinition[] Arrays;
        public TagPersistResourcerDefinition[] ResourceDefinitions;
        public TagPersistInteropDefinition[] InteropDefinitions;
        public int[] CustomSearchBlockNames;
        public int[] DataDefinitionNames;

        public TagPersistLayout(TagPersistLayoutHeader layoutHeader, PersistChunkReader reader)
        {
            LayoutHeader = layoutHeader;
            ReadChunks(reader);
        }

        public void ReadChunks(PersistChunkReader reader)
        {
            foreach (var chunk in reader.ReadChunks())
            {
                var chunkReader = new PersistChunkReader(chunk.Stream, reader.Format);
                switch (chunk.Header.Signature.ToString())
                {
                    case "str*":
                        StringBuffer = new StringBuffer(chunkReader.ReadBytes(chunk.Header.Size));
                        break;
                    case "sz+x":
                        ReadStringOffsets(chunkReader);
                        break;
                    case "sz[]":
                        ReadStringLists(chunkReader);
                        break;
                    case "csbn":
                        ReadCustomSearchBlockNames(chunkReader);
                        break;
                    case "dtnm":
                        ReadDataDefinitionNames(chunkReader);
                        break;
                    case "tgft":
                        ReadFieldTypes(chunkReader);
                        break;
                    case "gras":
                        ReadFields(chunkReader);
                        break;
                    case "blv2":
                        ReadBlocks(chunkReader);
                        break;
                    case "stv2":
                        ReadStructs(chunkReader);
                        break;
                    case "arr!":
                        ReadArrays(chunkReader);
                        break;
                    case "rcv2":
                        ReadResourceDefinitions(chunkReader);
                        break;
                    case "]==[":
                        ReadInteropDefinitions(chunkReader);
                        break;
                }
            }
        }

        private void ReadStringOffsets(PersistChunkReader chunkReader)
        {
            StringOffsets = new int[LayoutHeader.StringOffsetsCount];
            for (int i = 0; i < StringOffsets.Length; i++)
                StringOffsets[i] = chunkReader.ReadInt32();
        }

        private void ReadStringLists(PersistChunkReader chunkReader)
        {
            StringLists = new TagPersistStringList[LayoutHeader.StringListCount];
            for (int i = 0; i < StringLists.Length; i++)
                StringLists[i] = new TagPersistStringList(chunkReader);
        }

        private void ReadFieldTypes(PersistChunkReader chunkReader)
        {
            FieldTypes = new TagPersistFieldType[LayoutHeader.FieldTypesCount];
            for (int i = 0; i < FieldTypes.Length; i++)
                FieldTypes[i] = new TagPersistFieldType(chunkReader);
        }

        private void ReadFields(PersistChunkReader chunkReader)
        {
            Fields = new TagPersistField[LayoutHeader.FieldCount];
            for (int i = 0; i < Fields.Length; i++)
                Fields[i] = new TagPersistField(chunkReader);
        }

        private void ReadBlocks(PersistChunkReader chunkReader)
        {
            Blocks = new TagPersistBlockDefinition[LayoutHeader.BlockCount];
            for (int i = 0; i < Blocks.Length; i++)
                Blocks[i] = new TagPersistBlockDefinition(chunkReader);
        }

        private void ReadStructs(PersistChunkReader chunkReader)
        {
            Structs = new TagPersistStructDefinition[LayoutHeader.StructCount];
            for (int i = 0; i < Structs.Length; i++)
                Structs[i] = new TagPersistStructDefinition(chunkReader);
        }

        private void ReadArrays(PersistChunkReader chunkReader)
        {
            Arrays = new TagPersistArrayDefinition[LayoutHeader.ArrayCount];
            for (int i = 0; i < Arrays.Length; i++)
                Arrays[i] = new TagPersistArrayDefinition(chunkReader);
        }

        private void ReadCustomSearchBlockNames(PersistChunkReader chunkReader)
        {
            CustomSearchBlockNames = new int[LayoutHeader.CustomSearchBlockNamesCount];
            for (int i = 0; i < CustomSearchBlockNames.Length; i++)
                CustomSearchBlockNames[i] = chunkReader.ReadInt32();
        }

        private void ReadDataDefinitionNames(PersistChunkReader chunkReader)
        {
            DataDefinitionNames = new int[LayoutHeader.DataDefinitionNamesCount];
            for (int i = 0; i < DataDefinitionNames.Length; i++)
                DataDefinitionNames[i] = chunkReader.ReadInt32();
        }

        private void ReadResourceDefinitions(PersistChunkReader chunkReader)
        {
            ResourceDefinitions = new TagPersistResourcerDefinition[LayoutHeader.ResourceDefinitionCount];
            for (int i = 0; i < ResourceDefinitions.Length; i++)
                ResourceDefinitions[i] = new TagPersistResourcerDefinition(chunkReader);
        }

        private void ReadInteropDefinitions(PersistChunkReader chunkReader)
        {
            InteropDefinitions = new TagPersistInteropDefinition[LayoutHeader.InteropDefinitionCount];
            for (int i = 0; i < InteropDefinitions.Length; i++)
                InteropDefinitions[i] = new TagPersistInteropDefinition(chunkReader);
        }

        public class TagPersistBlockDefinition
        {
            public int NameOffset;
            public int MaxElementCount;
            public int StructIndex;

            public TagPersistBlockDefinition(PersistChunkReader reader)
            {
                NameOffset = reader.ReadInt32();
                MaxElementCount = reader.ReadInt32();
                StructIndex = reader.ReadInt32();
            }
        }

        public class TagPersistStringList
        {
            public int NameOffset;
            public int StringCount;
            public int FirstStringIndex;

            public TagPersistStringList(PersistChunkReader reader)
            {
                NameOffset = reader.ReadInt32();
                StringCount = reader.ReadInt32();
                FirstStringIndex = reader.ReadInt32();
            }
        }

        public class TagPersistFieldType
        {
            public int NameOffset;
            public int Size;
            public int Unknown;

            public TagPersistFieldType(PersistChunkReader reader)
            {
                NameOffset = reader.ReadInt32();
                Size = reader.ReadInt32();
                Unknown = reader.ReadInt32();
            }
        }

        public class TagPersistField
        {
            public int NameOffset;
            public int FieldTypeIndex;
            public int Definition;

            public TagPersistField(PersistChunkReader reader)
            {
                NameOffset = reader.ReadInt32();
                FieldTypeIndex = reader.ReadInt32();
                Definition = reader.ReadInt32();
            }
        }

        public class TagPersistStructDefinition
        {
            public Guid UniqueId;
            public int NameOffset;
            public int FirstFieldIndex;

            public TagPersistStructDefinition(PersistChunkReader reader)
            {
                UniqueId = new Guid(reader.ReadBytes(16));
                NameOffset = reader.ReadInt32();
                FirstFieldIndex = reader.ReadInt32();
            }
        }

        public class TagPersistArrayDefinition
        {
            public int NameOffset;
            public int Count;
            public int StructIndex;

            public TagPersistArrayDefinition(PersistChunkReader reader)
            {
                NameOffset = reader.ReadInt32();
                Count = reader.ReadInt32();
                StructIndex = reader.ReadInt32();
            }
        }

        public class TagPersistResourcerDefinition
        {
            public int NameOffset;
            public int Unknown4;
            public int StructIndex;

            public TagPersistResourcerDefinition(PersistChunkReader reader)
            {
                NameOffset = reader.ReadInt32();
                Unknown4 = reader.ReadInt32();
                StructIndex = reader.ReadInt32();
            }
        }

        public class TagPersistInteropDefinition
        {
            public int NameOffset;
            public int StructIndex;
            public int Unknown8;
            public int UnknownC;
            public int Unknown10;
            public int Unknown14;

            public TagPersistInteropDefinition(PersistChunkReader reader)
            {
                NameOffset = reader.ReadInt32();
                StructIndex = reader.ReadInt32();
                Unknown8 = reader.ReadInt32();
                UnknownC = reader.ReadInt32();
                Unknown10 = reader.ReadInt32();
                Unknown14 = reader.ReadInt32();
            }
        }
    }

    [TagStructure(Size = 0x34)]
    public class TagPersistLayoutHeader : TagStructure
    {
        public int RootBlockIndex;
        public int StringCount;
        public int StringOffsetsCount;
        public int StringListCount;
        public int CustomSearchBlockNamesCount;
        public int DataDefinitionNamesCount;
        public int ArrayCount;
        public int FieldTypesCount;
        public int FieldCount;
        public int StructCount;
        public int BlockCount;
        public int ResourceDefinitionCount;
        public int InteropDefinitionCount;
    }
}
