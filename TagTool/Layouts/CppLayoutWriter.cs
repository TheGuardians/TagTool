using System;
using System.Collections.Generic;
using System.IO;
using BlamCore.Common;

namespace TagTool.Layouts
{
    public class CppLayoutWriter : TagLayoutWriter
    {
        public override string GetSuggestedFileName(TagLayout layout)
        {
            return string.Format("{0}.hpp", NamingConvention.ToPascalCase(layout.Name));
        }

        public override void WriteLayout(TagLayout layout, TextWriter writer)
        {
            WriteHeader(writer);

            var name = NamingConvention.ToPascalCase(layout.Name);
            var builder = new ClassBuilder(writer, 2);
            builder.Begin(name, layout.Size, layout.GroupTag);
            layout.Accept(builder);
            builder.End();

            WriteFooter(writer);
        }

        private static void WriteHeader(TextWriter writer)
        {
            // Write the C++ header
            writer.WriteLine("#pragma once");
            writer.WriteLine("#include \"..\\Tags.hpp\"");
            writer.WriteLine();
            writer.WriteLine("namespace Blam");
            writer.WriteLine("{");
            writer.WriteLine("\tnamespace Tags");
            writer.WriteLine("\t{");
        }

        private static void WriteFooter(TextWriter writer)
        {
            writer.WriteLine("\t}");
            writer.WriteLine("}");
        }

        private class ClassBuilder : ITagLayoutFieldVisitor
        {
            private readonly TextWriter _writer;
            private string _indent;
            private int _indentLevel;
            private int _arrayCount = 1;
            private readonly StringWriter _fields = new StringWriter();
            private readonly Queue<string> _subBlocks = new Queue<string>();
            private readonly Dictionary<string, int> _nameCounts = new Dictionary<string, int>();
            private string _name;
            private uint _size;

            public ClassBuilder(TextWriter writer, int indent)
            {
                _writer = writer;
                SetIndent(indent);
            }

            public void Begin(string name, uint size, Tag groupTag)
            {
                _name = name;
                _size = size;
                if (groupTag.Value != 0)
                    _writer.WriteLine("{0}struct {1} : TagGroup<'{2}'>", _indent, name, groupTag);
                else
                    _writer.WriteLine("{0}struct {1}", _indent, name);
                _writer.WriteLine("{0}{{", _indent);
                SetIndent(_indentLevel + 1);
            }

            public void Visit(BasicTagLayoutField field)
            {
                AddElement(GetTypeName(field.Type), field.Name);
            }

            public void Visit(ArrayTagLayoutField field)
            {
                _arrayCount = field.Count;
                field.UnderlyingField.Accept(this);
                _arrayCount = 1;
            }

            public void Visit(EnumTagLayoutField field)
            {
                // TODO: Implement
                AddElement(GetTypeName(field.Layout.UnderlyingType), field.Name);
            }

            public void Visit(StringTagLayoutField field)
            {
                _arrayCount = field.Size;
                AddElement("char", field.Name);
                _arrayCount = 1;
            }

            public void Visit(TagBlockTagLayoutField field)
            {
                var className = MakeName(field.ElementLayout.Name);
                _writer.WriteLine("{0}struct {1};", _indent, className);
                using (var blockWriter = new StringWriter())
                {
                    var blockBuilder = new ClassBuilder(blockWriter, _indentLevel);
                    blockBuilder.Begin(className, field.ElementLayout.Size, field.ElementLayout.GroupTag);
                    field.ElementLayout.Accept(blockBuilder);
                    blockBuilder.End();
                    _subBlocks.Enqueue(blockWriter.ToString());
                }
                var type = string.Format("TagBlock<{0}>", className);
                AddElement(type, field.Name);
            }

            public void End()
            {
                if (_subBlocks.Count > 0)
                    _writer.WriteLine();
                _writer.Write(_fields.ToString());

                // Put tag block definitions at the end
                while (_subBlocks.Count > 0)
                {
                    _writer.WriteLine();
                    _writer.Write(_subBlocks.Dequeue());
                }

                SetIndent(_indentLevel - 1);
                _writer.WriteLine("{0}}};", _indent);
                _writer.WriteLine("{0}TAG_STRUCT_SIZE_ASSERT({1}, 0x{2:X});", _indent, _name, _size);
            }

            private void AddElement(string type, string name)
            {
                if (_arrayCount > 1)
                {
                    _fields.WriteLine("{0}{1} {2}[{3}];", _indent, type, MakeName(name), _arrayCount);
                }
                else
                {
                    _fields.WriteLine("{0}{1} {2};", _indent, type, MakeName(name));
                }
            }

            private static string GetTypeName(BasicFieldType type)
            {
                switch (type)
                {
                    case BasicFieldType.Int8:
                        return "int8_t";
                    case BasicFieldType.UInt8:
                        return "uint8_t";
                    case BasicFieldType.Int16:
                        return "int16_t";
                    case BasicFieldType.UInt16:
                        return "uint16_t";
                    case BasicFieldType.Int32:
                        return "int32_t";
                    case BasicFieldType.UInt32:
                        return "uint32_t";
                    case BasicFieldType.Float32:
                        return "float";
                    case BasicFieldType.Vector2:
                        throw new NotSupportedException("Vector2 is not supported");
                    case BasicFieldType.Vector3:
                        throw new NotSupportedException("Vector3 is not supported");
                    case BasicFieldType.Vector4:
                        throw new NotSupportedException("Vector4 is not supported");
                    case BasicFieldType.Angle:
                        return "float";
                    case BasicFieldType.StringID:
                        return "StringID";
                    case BasicFieldType.TagReference:
                        return "TagReference";
                    case BasicFieldType.DataReference:
                        return "TagData<uint8_t>";
                    case BasicFieldType.ResourceReference:
                        return "void*";
                    case BasicFieldType.ShortTagReference:
                        return "int TagReference";
                    default:
                        throw new ArgumentException("Unrecognized basic field type " + type);
                }
            }

            private string MakeName(string name)
            {
                // Convert the name to pascal case, and if it's unique, then return it
                // Otherwise, increment the name's use count, append it to the name, and try again
                var result = NamingConvention.ToPascalCase(name);
                while (true)
                {
                    if (!_nameCounts.TryGetValue(result, out int count))
                    {
                        _nameCounts[result] = 1;
                        return result;
                    }
                    count++;
                    _nameCounts[result] = count;
                    if (result.Length > 0 && char.IsDigit(result[result.Length - 1]))
                        result += '_'; // Prepend an underscore to the count if the name ends with a digit
                    result += count;
                }
            }

            private void SetIndent(int level)
            {
                _indentLevel = level;
                _indent = new string('\t', level);
            }
        }
    }
}
