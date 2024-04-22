using System;
using System.Collections.Generic;
using TagTool.Tags;

namespace TagTool.Cache.Monolithic
{
    public class TagLayout
    {
        private TagPersistLayout PersistLayout;
        public Dictionary<int, TagFieldType> FieldTypeLookup;
        public Dictionary<TagFieldType, TagFieldTypeInfo> FieldTypeInfos;
        public TagBlockDefinition[] Blocks;
        public TagStructDefinition[] Structs;
        public TagField[] Fields;
        public StringListDefiniton[] StringLists;
        public TagArrayDefinition[] Arrays;
        public TagResourceDefinition[] ResourceDefinitions;
        public TagInteropDefinition[] InteropDefinitions;
        public int RootBlockIndex = -1;

        public TagBlockDefinition RootBlock => GetBlock(RootBlockIndex);
        public TagStructDefinition RootStruct => RootBlock.Struct;

        public TagLayout(TagPersistLayout persistLayout)
        {
            PersistLayout = persistLayout;
            Blocks = new TagBlockDefinition[persistLayout.Blocks.Length];
            Structs = new TagStructDefinition[persistLayout.Structs.Length];
            Fields = new TagField[persistLayout.Fields.Length];
            StringLists = new StringListDefiniton[persistLayout.StringLists.Length];
            Arrays = new TagArrayDefinition[persistLayout.Arrays.Length];
            ResourceDefinitions = new TagResourceDefinition[persistLayout.ResourceDefinitions.Length];
            InteropDefinitions = new TagInteropDefinition[persistLayout.InteropDefinitions.Length];
            RootBlockIndex = persistLayout.LayoutHeader.RootBlockIndex;
            BuildFieldTypeMapping();
        }

        public TagBlockDefinition GetBlock(int index)
        {
            if (Blocks[index] != null)
                return Blocks[index];

            var persistBlock = PersistLayout.Blocks[index];

            var blockLayout = new TagBlockDefinition();
            blockLayout.Name = PersistLayout.StringBuffer.GetString(persistBlock.NameOffset);
            blockLayout.Struct = GetStruct(persistBlock.StructIndex);

            Blocks[index] = blockLayout;
            return blockLayout;
        }

        public TagStructDefinition GetStruct(int index)
        {
            if (Structs[index] != null)
                return Structs[index];

            var persistStruct = PersistLayout.Structs[index];

            var structLayout = new TagStructDefinition();
            structLayout.Id = persistStruct.UniqueId;
            structLayout.Name = PersistLayout.StringBuffer.GetString(persistStruct.NameOffset);
            structLayout.Fields = new List<TagField>();

            int size = 0;
            int fieldIndex = persistStruct.FirstFieldIndex;
            while (true)
            {
                var field = GetField(fieldIndex++);
                if (field.FieldType == TagFieldType.TerminatorX)
                    break;

                field.Offset = size;
                size += GetfieldSize(field);
                structLayout.Fields.Add(field);
            }
            structLayout.Size = size;

            Structs[index] = structLayout;
            return structLayout;
        }

        public int GetfieldSize(TagField field)
        {
            switch (field.FieldType)
            {
                case TagFieldType.Struct:
                    {
                        var structDefinition = (TagStructDefinition)field.Definition;
                        return structDefinition.Size;
                    }
                case TagFieldType.Array:
                    {
                        var arrayDefinition = (TagArrayDefinition)field.Definition;
                        return arrayDefinition.Struct.Size * arrayDefinition.Count;
                    }
                case TagFieldType.Pad:
                case TagFieldType.Skip:
                    return (int)field.Definition;
                default:
                    return FieldTypeInfos[field.FieldType].Size;
            }
        }


        public StringListDefiniton GetStringList(int index)
        {
            if (StringLists[index] != null)
                return StringLists[index];

            var persistStringList = PersistLayout.StringLists[index];

            var stringList = new StringListDefiniton();
            stringList.Strings = new List<string>(persistStringList.StringCount);
            stringList.Name = PersistLayout.StringBuffer.GetString(persistStringList.NameOffset);
            for (int i = 0; i < persistStringList.StringCount; i++)
            {
                var str = PersistLayout.StringBuffer.GetString(PersistLayout.StringOffsets[persistStringList.FirstStringIndex + i]);
                stringList.Strings.Add(str);
            }
            StringLists[index] = stringList;

            return stringList;
        }

        public TagArrayDefinition GetArray(int index)
        {
            if (Arrays[index] != null)
                return Arrays[index];

            var persistArray = PersistLayout.Arrays[index];

            var array = new TagArrayDefinition();
            array.Name = PersistLayout.StringBuffer.GetString(persistArray.NameOffset);
            array.Struct = GetStruct(persistArray.StructIndex);
            array.Count = persistArray.Count;
            Arrays[index] = array;

            return array;
        }

        private object GetResourceDefinition(int index)
        {
            if (ResourceDefinitions[index] != null)
                return ResourceDefinitions[index];

            var persistDefinition = PersistLayout.ResourceDefinitions[index];

            var definition = new TagResourceDefinition();
            definition.Name = PersistLayout.StringBuffer.GetString(persistDefinition.NameOffset);
            definition.Struct = GetStruct((int)persistDefinition.StructIndex);
            ResourceDefinitions[index] = definition;
            return definition;
        }

        private object GetApiInteropDefinition(int index)
        {
            if (InteropDefinitions[index] != null)
                return InteropDefinitions[index];

            var persistDefinition = PersistLayout.InteropDefinitions[index];

            var definition = new TagInteropDefinition();
            definition.Name = PersistLayout.StringBuffer.GetString(persistDefinition.NameOffset);
            definition.Struct = GetStruct(persistDefinition.StructIndex);
            InteropDefinitions[index] = definition;

            return definition;
        }

        public TagField GetField(int index)
        {
            if (Fields[index] != null)
                return Fields[index];

            var persistField = PersistLayout.Fields[index];

            var fieldName = PersistLayout.StringBuffer.GetString(persistField.NameOffset);
            var field = new TagField();

            field.FieldType = FieldTypeLookup[persistField.FieldTypeIndex];
            if (field.FieldType != TagFieldType.TerminatorX)
            {
                field.Name = fieldName;
                field.Definition = GetFieldDefinition(field.FieldType, persistField.Definition);
            }

            Fields[index] = field;
            return field;
        }

        public object GetFieldDefinition(TagFieldType fieldType, int definition)
        {
            switch (fieldType)
            {
                case TagFieldType.Struct:
                    return GetStruct(definition);
                case TagFieldType.Block:
                    return GetBlock(definition);
                case TagFieldType.CharEnum:
                case TagFieldType.ShortEnum:
                case TagFieldType.LongEnum:
                case TagFieldType.ByteFlags:
                case TagFieldType.WordFlags:
                case TagFieldType.LongFlags:
                    return GetStringList(definition);
                case TagFieldType.Array:
                    return GetArray(definition);
                case TagFieldType.CharBlockIndex:
                case TagFieldType.ShortBlockIndex:
                case TagFieldType.LongBlockIndex:
                case TagFieldType.ByteBlockFlags:
                case TagFieldType.WordBlockFlags:
                case TagFieldType.LongBlockFlags:
                    return definition;
                case TagFieldType.CustomCharBlockIndex:
                case TagFieldType.CustomShortBlockIndex:
                case TagFieldType.CustomLongBlockIndex:
                    return PersistLayout.StringBuffer.GetString(PersistLayout.CustomSearchBlockNames[definition]);
                case TagFieldType.Pad:
                case TagFieldType.Skip:
                    return definition;
                case TagFieldType.Data:
                    return PersistLayout.StringBuffer.GetString(PersistLayout.DataDefinitionNames[definition]);
                case TagFieldType.PageableResource:
                    return GetResourceDefinition((int)definition);
                case TagFieldType.ApiInterop:
                    return GetApiInteropDefinition((int)definition);
                default:
                    if (definition != 0)
                        throw new NotImplementedException();
                    return null;
            }
        }
        private void BuildFieldTypeMapping()
        {
            FieldTypeLookup = new Dictionary<int, TagFieldType>();
            FieldTypeInfos = new Dictionary<TagFieldType, TagFieldTypeInfo>();
            for (int i = 0; i < PersistLayout.FieldTypes.Length; i++)
            {
                var fieldType = PersistLayout.FieldTypes[i];
                var name = PersistLayout.StringBuffer.GetString(fieldType.NameOffset);
                var typeName = name.ToPascalCase();

                var info = new TagFieldTypeInfo();
                info.Type = (TagFieldType)Enum.Parse(typeof(TagFieldType), typeName, true);
                info.Size = fieldType.Size;
                info.Name = name;
                FieldTypeLookup.Add(i, info.Type);
                FieldTypeInfos.Add(info.Type, info);
            }
        }

        public class TagFieldTypeInfo
        {
            public TagFieldType Type;
            public int Size;
            public string Name;
        }
    }

    public class TagBlockDefinition
    {
        public string Name;
        public string BlockName;
        public TagStructDefinition Struct;
    }

    public class TagStructDefinition
    {
        public Guid Id;
        public string Name;
        public List<TagField> Fields;
        public int Size;
    }

    public class TagArrayDefinition
    {
        public string Name;
        public int Count;
        public TagStructDefinition Struct;
    }

    public class TagResourceDefinition
    {
        public string Name;
        public TagStructDefinition Struct;
    }

    public class TagInteropDefinition
    {
        public string Name;
        public TagStructDefinition Struct;
    }

    public class StringListDefiniton
    {
        public string Name;
        public List<string> Strings = new List<string>();
    }

    public class TagField
    {
        public TagFieldType FieldType;
        public string Name;
        public object Definition;
        public int Offset;
    }

    [TagStructure(Size = 0x8)]
    public class TagBlockChunkHeader : TagStructure
    {
        public int ElementCount;
        public int Unknown1;
    }
}
