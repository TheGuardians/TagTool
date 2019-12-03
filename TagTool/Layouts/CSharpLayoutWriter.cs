using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;

namespace TagTool.Layouts
{
    public class CSharpLayoutWriter : TagLayoutWriter
    {
        public override string GetSuggestedFileName(TagLayout layout)
        {
            return string.Format("{0}.cs", layout.Name.ToPascalCase());
        }

        public override void WriteLayout(TagLayout layout, TextWriter writer)
        {
            WriteHeader(writer);

            var name = layout.Name.ToPascalCase();
            var builder = new ClassBuilder(writer, 1);
            builder.Begin(name, layout.Size, 0, layout.GroupTag);
            layout.Accept(builder);
            builder.End();

            WriteFooter(writer);
        }

        private static void WriteHeader(TextWriter writer)
        {
            // Write the C# header
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.Linq;");
            writer.WriteLine("using System.Text;");
            writer.WriteLine("using System.Threading.Tasks;");
            writer.WriteLine("using HaloOnlineTagTool.Common;");
            writer.WriteLine("using HaloOnlineTagTool.Resources;");
            writer.WriteLine("using HaloOnlineTagTool.Serialization;");
            writer.WriteLine();
            writer.WriteLine("namespace HaloOnlineTagTool.TagStructures");
            writer.WriteLine("{");
        }

        private static void WriteFooter(TextWriter writer)
        {
            writer.WriteLine("}");
        }

        private class ClassBuilder : ITagLayoutFieldVisitor
        {
            private readonly TextWriter _writer;
            private string _indent;
            private int _indentLevel;
            private int _arrayCount = 1;
            private int _stringLength;
            private bool _short;
            private uint _align;
            private readonly Queue<string> _subBlocks = new Queue<string>();
            private readonly Dictionary<string, int> _nameCounts = new Dictionary<string, int>(); 

            public ClassBuilder(TextWriter writer, int indent)
            {
                _writer = writer;
                SetIndent(indent);
            }

            public void Begin(string name, uint size, uint align, Tag groupTag)
            {
                // TagStructureAttribute
                _writer.Write("{0}[TagStructure(", _indent);
                if (groupTag.Value != 0)
                    _writer.Write("Class = \"{0}\", ", groupTag);
                _writer.Write("Size = 0x{0:X}", size);
                if (align > 4)
                    _writer.Write(", Align = 0x{0:X}", align);
                _writer.WriteLine(")]");

                _writer.WriteLine("{0}public class {1}", _indent, name);
                _writer.WriteLine("{0}{{", _indent);
                SetIndent(_indentLevel + 1);
                _nameCounts[name] = 1;
            }

            public void Visit(BasicTagLayoutField field)
            {
                _short = (field.Type == BasicFieldType.ShortTagReference);
                _align = field.DataAlign;
                AddElement(GetTypeName(field.Type), field.Name);
                _align = 0;
                _short = false;
            }

            public void Visit(ArrayTagLayoutField field)
            {
                _arrayCount = field.Count;
                field.UnderlyingField.Accept(this);
                _arrayCount = 1;
            }

            public void Visit(EnumTagLayoutField field)
            {
                using (var enumWriter = new StringWriter())
                {
                    var typeName = BuildEnum(field.Layout, enumWriter);
                    _subBlocks.Enqueue(enumWriter.ToString());
                    AddElement(typeName, field.Name);
                }
            }

            public void Visit(StringTagLayoutField field)
            {
                _stringLength = field.Size;
                AddElement("string", field.Name);
                _stringLength = 0;
            }

            public void Visit(TagBlockTagLayoutField field)
            {
                string className;
                if (field.ElementLayout.Name != field.Name)
                    className = MakeName(field.ElementLayout.Name);
                else
                    className = MakeName(field.ElementLayout.Name + " Block");
                using (var blockWriter = new StringWriter())
                {
                    var blockBuilder = new ClassBuilder(blockWriter, _indentLevel);
                    blockBuilder.Begin(className, field.ElementLayout.Size, field.DataAlign, field.ElementLayout.GroupTag);
                    field.ElementLayout.Accept(blockBuilder);
                    blockBuilder.End();
                    _subBlocks.Enqueue(blockWriter.ToString());
                }
                var blockType = string.Format("List<{0}>", className);
                AddElement(blockType, field.Name);
            }

            public void End()
            {
                // Put tag block definitions at the end
                while (_subBlocks.Count > 0)
                {
                    _writer.WriteLine();
                    _writer.Write(_subBlocks.Dequeue());
                }
                SetIndent(_indentLevel - 1);
                _writer.WriteLine("{0}}}", _indent);
            }

            private void AddElement(string type, string name)
            {
                if (_arrayCount > 1)
                    _writer.WriteLine("{0}[TagField(Count = {1}{2})] public {3}[] {4};", _indent, _arrayCount, _short ? ", Short = true" : "", type, MakeName(name));
                else if (_stringLength > 0)
                    _writer.WriteLine("{0}[TagField(Length = {1})] public {2} {3};", _indent, _stringLength, type, MakeName(name));
                else if (_align > 4)
                    _writer.WriteLine("{0}[TagField(DataAlign = 0x{1:X})] public {2} {3};", _indent, _align, type, MakeName(name));
                else
                    _writer.WriteLine("{0}{1}public {2} {3};", _indent, _short ? "[TagField(Flags = Short)] " : "", type, MakeName(name));
            }

            private string BuildEnum(EnumLayout layout, TextWriter writer)
            {
                var enumName = MakeName(layout.Name + " Value");
                var valueNameCounts = new Dictionary<string, int>();
                writer.WriteLine("{0}public enum {1} : {2}", _indent, enumName, GetTypeName(layout.UnderlyingType));
                writer.WriteLine("{0}{{", _indent);
                SetIndent(_indentLevel + 1);
                var nextValue = 0;
                foreach (var val in layout.Values)
                {
                    if (val.Value == nextValue)
                        writer.WriteLine("{0}{1},", _indent, MakeName(val.Name, valueNameCounts));
                    else
                        writer.WriteLine("{0}{1} = {2},", _indent, MakeName(val.Name, valueNameCounts), val.Value);
                    nextValue = val.Value + 1;
                }
                SetIndent(_indentLevel - 1);
                writer.WriteLine("{0}}}", _indent);
                return enumName;
            }

            private static string GetTypeName(BasicFieldType type)
            {
                switch (type)
                {
                    case BasicFieldType.Int8:
                        return "sbyte";
                    case BasicFieldType.UInt8:
                        return "byte";
                    case BasicFieldType.Int16:
                        return "short";
                    case BasicFieldType.UInt16:
                        return "ushort";
                    case BasicFieldType.Int32:
                        return "int";
                    case BasicFieldType.UInt32:
                        return "uint";
                    case BasicFieldType.Float32:
                        return "float";
                    case BasicFieldType.Vector2:
                        return "Vector2";
                    case BasicFieldType.Vector3:
                        return "Vector3";
                    case BasicFieldType.Vector4:
                        return "Vector4";
                    case BasicFieldType.Angle:
                        return "Angle";
                    case BasicFieldType.StringID:
                        return "StringID";
                    case BasicFieldType.TagReference:
                    case BasicFieldType.ShortTagReference:
                        return "TagInstance";
                    case BasicFieldType.DataReference:
                        return "byte[]";
                    case BasicFieldType.ResourceReference:
                        return "ResourceReference";
                    default:
                        throw new ArgumentException("Unrecognized basic field type " + type);
                }
            }

            private string MakeName(string name)
            {
                return MakeName(name, _nameCounts);
            }

            private static string MakeName(string name, Dictionary<string, int> nameCounts)
            {
                // Convert the name to pascal case, and if it's unique, then return it
                // Otherwise, increment the name's use count, append it to the name, and try again
                var result = name.ToPascalCase();

                while (true)
                {
                    if (!nameCounts.TryGetValue(result, out int count))
                    {
                        nameCounts[result] = 1;
                        return result;
                    }
                    count++;
                    nameCounts[result] = count;
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
