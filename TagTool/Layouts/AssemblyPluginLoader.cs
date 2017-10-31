using BlamCore.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace TagTool.Layouts
{
    public class AssemblyPluginLoader
    {
        private static readonly PluralizationService Pluralization =
            PluralizationService.CreateService(new CultureInfo("en-US"));

        /// <summary>
        /// Parses an XML plugin into a <see cref="TagLayout"/>.
        /// </summary>
        /// <param name="reader">The XmlReader to read the plugin XML from.</param>
        /// <param name="name">The name to give the resulting layout.</param>
        /// <param name="groupTag">The group tag to give to the resulting layout.</param>
        /// <returns>The results from loading the plugin.</returns>
        public static AssemblyPluginLoadResults LoadPlugin(XmlReader reader, string name, Tag groupTag)
        {
            if (!reader.ReadToNextSibling("plugin"))
                throw new ArgumentException("The XML file is missing a <plugin> tag.");

            int baseSize = 0;
            if (reader.MoveToAttribute("baseSize"))
                baseSize = ParseInt(reader.Value);

            var loader = new AssemblyPluginLoader(null);
            loader._results.Layout = new TagLayout(name, (uint)baseSize, groupTag);
            loader.ReadElements(reader, true);
            return loader._results;
        }

        private AssemblyPluginLoadResults _results;
        private uint _nextOffset = 0;
        private string _blockName;

        private AssemblyPluginLoader(string blockName)
        {
            _results = new AssemblyPluginLoadResults();
            _blockName = blockName;
        }

        private void ReadElements(XmlReader reader, bool topLevel)
        {
            while (reader.Read())
            {
                if (reader.NodeType != XmlNodeType.Element) continue;
                if (topLevel)
                    HandleTopLevelElement(reader);
                else
                    HandleElement(reader);
            }
        }

        private void HandleTopLevelElement(XmlReader reader)
        {
            if (reader.Name == "revisions")
            {
                // TODO: Revision support?
                /*if (visitor.EnterRevisions())
                {
                    ReadRevisions(reader.ReadSubtree(), visitor);
                    visitor.LeaveRevisions();
                }
                else
                {
                    reader.Skip();
                }*/
                reader.Skip();
            }
            else
            {
                HandleElement(reader);
            }
        }

        private void HandleElement(XmlReader reader)
        {
            switch (reader.Name)
            {
                case "comment":
                    ReadComment(reader);
                    break;
                default:
                    HandleValueElement(reader, reader.Name);
                    break;
            }
        }

        private static void ReadComment(XmlReader reader)
        {
            /*string title = "Comment";

            if (reader.MoveToAttribute("title"))
                title = reader.Value;

            reader.MoveToElement();
            var xmlLineInfo = reader as IXmlLineInfo;
            if (xmlLineInfo == null) return;
            var pluginLine = (uint) xmlLineInfo.LineNumber;
            string text = reader.ReadElementContentAsString();
            layout.VisitComment(title, text, pluginLine);*/
        }

        /// <summary>
        /// Handles an element which describes how a value
        /// should be read from the cache file.
        /// </summary>
        /// <param name="reader">The XmlReader that read the element.</param>
        /// <param name="elementName">The element's name.</param>
        private void HandleValueElement(XmlReader reader, string elementName)
        {
            string name = "Unknown";
            uint offset = 0;
            var xmlLineInfo = reader as IXmlLineInfo;
            if (xmlLineInfo == null) return;
            var pluginLine = (uint) xmlLineInfo.LineNumber;
            bool visible = true;

            if (reader.MoveToAttribute("name"))
                name = reader.Value;
            if (reader.MoveToAttribute("offset"))
                offset = ParseUInt(reader.Value);
            if (reader.MoveToAttribute("visible"))
                visible = ParseBool(reader.Value);

            // If the offset isn't what it should be,
            // the field is probably overlapping something or there's a hole
            if (offset != _nextOffset)
                _results.Conflicts.Add(new AssemblyPluginFieldConflict(name, offset, _blockName));

            reader.MoveToElement();
            switch (elementName.ToLower()) // FIXME: Using ToLower() here violates XML standards
            {
                case "uint8":
                    _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.UInt8));
                    RegisterField(offset, 1);
                    break;
                case "int8":
                    _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.Int8));
                    RegisterField(offset, 1);
                    break;
                case "uint16":
                    _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.UInt16));
                    RegisterField(offset, 2);
                    break;
                case "int16":
                    _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.Int16));
                    RegisterField(offset, 2);
                    break;
                case "uint32":
                case "undefined":
                    if (name.Contains("Resource Reference Address"))
                        _results.Layout.Add(new BasicTagLayoutField(name.Replace(" Reference Address", ""), BasicFieldType.ResourceReference)); // hack
                    else
                        _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.UInt32));
                    RegisterField(offset, 4);
                    break;
                case "int32":
                    _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.Int32));
                    RegisterField(offset, 4);
                    break;
                case "float32":
                case "float":
                    _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.Float32));
                    RegisterField(offset, 4);
                    break;
                case "vector3":
                    _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.Vector3));
                    RegisterField(offset, 4 * 3);
                    break;
                case "degree":
                    _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.Angle));
                    RegisterField(offset, 4);
                    break;
                case "stringid":
                    _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.StringID));
                    RegisterField(offset, 4);
                    break;
                case "tagref":
                    ReadTagRef(reader, name, offset, visible, pluginLine);
                    break;

                case "range":
                    ReadRange(reader, name, offset, visible, pluginLine);
                    break;

                case "ascii":
                    ReadAscii(reader, name, offset, visible, pluginLine);
                    break;

                case "utf16":
                    ReadUtf16(reader, name, offset, visible, pluginLine);
                    break;

                case "bitfield8":
                    ReadBits(reader, name, BasicFieldType.UInt8);
                    RegisterField(offset, 1);
                    break;
                case "bitfield16":
                    ReadBits(reader, name, BasicFieldType.UInt16);
                    RegisterField(offset, 2);
                    break;
                case "bitfield32":
                    ReadBits(reader, name, BasicFieldType.Int32);
                    RegisterField(offset, 4);
                    break;

                case "enum8":
                    ReadOptions(reader, name, BasicFieldType.Int8);
                    RegisterField(offset, 1);
                    break;
                case "enum16":
                    ReadOptions(reader, name, BasicFieldType.Int16);
                    RegisterField(offset, 2);
                    break;
                case "enum32":
                    ReadOptions(reader, name, BasicFieldType.Int32);
                    RegisterField(offset, 4);
                    break;

                    //case "color8": case "colour8":
                    //case "color16": case "colour16":
                case "color":
                case "colour":
                    var format = ReadColorFormat(reader);
                    _results.Layout.AddRange(format.Select(ch => new BasicTagLayoutField(name + " " + ch, BasicFieldType.UInt8)));
                    RegisterField(offset, (uint)format.Length);
                    break;
                case "color24":
                case "colour24":
                    _results.Layout.AddRange("rgb".Select(ch => new BasicTagLayoutField(name + " " + ch, BasicFieldType.UInt8)));
                    RegisterField(offset, 3);
                    break;
                case "color32":
                case "colour32":
                    _results.Layout.AddRange("argb".Select(ch => new BasicTagLayoutField(name + " " + ch, BasicFieldType.UInt8)));
                    RegisterField(offset, 4);
                    break;
                case "colorf":
                case "colourf":
                    var formatf = ReadColorFormat(reader);
                    _results.Layout.AddRange(formatf.Select(ch => new BasicTagLayoutField(name + " " + ch, BasicFieldType.Float32)));
                    RegisterField(offset, (uint)formatf.Length * 4);
                    break;

                case "dataref":
                    ReadDataRef(reader, name, offset, visible, pluginLine);
                    break;

                case "reflexive":
                    ReadReflexive(reader, name, offset, visible, pluginLine);
                    break;

                case "raw":
                    ReadRaw(reader, name, offset, visible, pluginLine);
                    break;

                case "shader":
                    ReadShader(reader, name, offset, visible, pluginLine);
                    break;

                case "uniclist":
                    ReadUnicList(reader, name, offset, visible, pluginLine);
                    break;

                default:
                    throw new ArgumentException("Unknown element \"" + elementName + "\"." + PositionInfo(reader));
            }
        }

        /*private static void ReadRevisions(XmlReader reader)
        {
            reader.ReadStartElement();
            while (reader.ReadToFollowing("revision"))
                layout.VisitRevision(ReadRevision(reader));
        }

        private static PluginRevision ReadRevision(XmlReader reader)
        {
            string author = "";
            int version = 1;

            if (reader.MoveToAttribute("author"))
                author = reader.Value;
            if (reader.MoveToAttribute("version"))
                version = ParseInt(reader.Value);

            reader.MoveToElement();
            string description = reader.ReadElementContentAsString();
            return new PluginRevision(author, version, description);
        }*/

        private void ReadDataRef(XmlReader reader, string name, uint offset, bool visible,
            uint pluginLine)
        {
            string format = "bytes";

            if (reader.MoveToAttribute("format"))
                format = reader.Value;

            if (format != "bytes" &&
                format != "unicode" &&
                format != "asciiz")
                throw new ArgumentException("Invalid format. Must be either `bytes`, `unicode` or `asciiz`.");

            int align = 4;
            if (reader.MoveToAttribute("align"))
                align = ParseInt(reader.Value);

            _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.DataReference));
            RegisterField(offset, 0x14);
        }

        private void ReadRange(XmlReader reader, string name, uint offset, bool visible,
            uint pluginLine)
        {
            double min = 0.0;
            double max = 0.0;
            double largeChange = 0.0;
            double smallChange = 0.0;
            string type = "int32";

            if (reader.MoveToAttribute("min"))
                min = double.Parse(reader.Value);
            if (reader.MoveToAttribute("max"))
                max = double.Parse(reader.Value);
            if (reader.MoveToAttribute("smallStep"))
                smallChange = double.Parse(reader.Value);
            if (reader.MoveToAttribute("largeStep"))
                largeChange = double.Parse(reader.Value);
            if (reader.MoveToAttribute("type"))
                type = reader.Value.ToLower();

            _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.Int32)); // TODO: support types other than int32
            RegisterField(offset, 4);
        }

        private void ReadTagRef(XmlReader reader, string name, uint offset, bool visible,
            uint pluginLine)
        {
            bool showJumpTo = true;
            bool withClass = true;

            if (reader.MoveToAttribute("showJumpTo"))
                showJumpTo = ParseBool(reader.Value);
            if (reader.MoveToAttribute("withClass"))
                withClass = ParseBool(reader.Value);

            if (withClass)
            {
                _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.TagReference));
                RegisterField(offset, 0x10);
            }
            else
            {
                _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.ShortTagReference));
                RegisterField(offset, 0x4);
            }
        }

        private void ReadAscii(XmlReader reader, string name, uint offset, bool visible,
            uint pluginLine)
        {
            // Both "size" and "length" are accepted here because they are the same
            // with ASCII strings, but "size" should be preferred because it's less ambiguous
            // and <utf16> only supports "size"
            int size = 0;
            if (reader.MoveToAttribute("size") || reader.MoveToAttribute("length"))
                size = ParseInt(reader.Value);

            _results.Layout.Add(new StringTagLayoutField(name, size));
            RegisterField(offset, (uint)size);
        }

        private void ReadUtf16(XmlReader reader, string name, uint offset, bool visible,
            uint pluginLine)
        {
            int size = 0;
            if (reader.MoveToAttribute("size"))
                size = ParseInt(reader.Value);

            // TODO: Proper UTF-16 support (does HO use these?)
            _results.Layout.Add(new ArrayTagLayoutField(new BasicTagLayoutField(name, BasicFieldType.UInt8), size / 2));
            RegisterField(offset, (uint)size);
        }

        private void ReadBits(XmlReader reader, string name, BasicFieldType type)
        {
            XmlReader subtree = reader.ReadSubtree();

            var enumLayout = new EnumLayout(name, type);
            subtree.ReadStartElement();
            enumLayout.Add(new EnumValue("None", 0));
            while (subtree.ReadToNextSibling("bit"))
                enumLayout.Add(ReadBit(subtree));

            _results.Layout.Add(new EnumTagLayoutField(name, enumLayout));
        }

        private static EnumValue ReadBit(XmlReader reader)
        {
            string name = "Unknown";
            int value = 0;

            if (reader.MoveToAttribute("name"))
                name = reader.Value;
            if (reader.MoveToAttribute("index"))
                value = ParseInt(reader.Value);

            return new EnumValue(name, 1 << value);
        }

        private void ReadOptions(XmlReader reader, string name, BasicFieldType type)
        {
            XmlReader subtree = reader.ReadSubtree();

            var enumLayout = new EnumLayout(name, type);
            subtree.ReadStartElement();
            while (subtree.ReadToNextSibling("option"))
                enumLayout.Add(ReadOption(subtree));

            _results.Layout.Add(new EnumTagLayoutField(name, enumLayout));
        }

        private static EnumValue ReadOption(XmlReader reader)
        {
            string name = "Unknown";
            int value = 0;

            if (reader.MoveToAttribute("name"))
                name = reader.Value;
            if (reader.MoveToAttribute("value"))
                value = ParseInt(reader.Value);

            return new EnumValue(name, value);
        }

        private static string ReadColorFormat(XmlReader reader)
        {
            if (!reader.MoveToAttribute("format"))
                throw new ArgumentException("Color tags must have a format attribute." + PositionInfo(reader));

            string format = reader.Value.ToLower();

            if (format.Any(ch => ch != 'r' && ch != 'g' && ch != 'b' && ch != 'a'))
                throw new ArgumentException("Invalid color format: \"" + format + "\"" + PositionInfo(reader));

            return format;
        }

        private void ReadReflexive(XmlReader reader, string name, uint offset, bool visible,
            uint pluginLine)
        {
            if (!reader.MoveToAttribute("entrySize"))
                throw new ArgumentException("Reflexives must have an entrySize attribute." + PositionInfo(reader));

            uint entrySize = ParseUInt(reader.Value);
            int align = 4;
            if (reader.MoveToAttribute("align"))
                align = ParseInt(reader.Value);

            reader.MoveToElement();
            XmlReader subtree = reader.ReadSubtree();
            subtree.ReadStartElement();

            // Singularize the last word in the block name to get the layout name
            var words = name.Split(' ');
            words[words.Length - 1] = Pluralization.Singularize(words[words.Length - 1]);
            var layoutName = string.Join(" ", words);

            // Read the layout using a new loader (to keep track of conflicts)
            var loader = new AssemblyPluginLoader(name);
            loader._results.Layout = new TagLayout(layoutName, entrySize);
            loader.ReadElements(subtree, false);
            _results.Layout.Add(new TagBlockTagLayoutField(name, loader._results.Layout));
            _results.Conflicts.AddRange(loader._results.Conflicts); // Merge in conflicts from the block
            RegisterField(offset, 0xC);
        }

        private void ReadRaw(XmlReader reader, string name, uint offset, bool visible,
            uint pluginLine)
        {
            if (!reader.MoveToAttribute("size"))
                throw new ArgumentException("Raw data blocks must have a size attribute." + PositionInfo(reader));
            int size = ParseInt(reader.Value);

            _results.Layout.Add(new ArrayTagLayoutField(new BasicTagLayoutField(name, BasicFieldType.UInt8), size));
            RegisterField(offset, (uint)size);
        }

        private enum ShaderType
        {
            Pixel,
            Vertex
        }

        private void ReadShader(XmlReader reader, string name, uint offset, bool visible,
            uint pluginLine)
        {
            /*if (!reader.MoveToAttribute("type"))
                throw new ArgumentException("Shaders must have a type attribute." + PositionInfo(reader));

            ShaderType type;
            if (reader.Value == "pixel")
                type = ShaderType.Pixel;
            else if (reader.Value == "vertex")
                type = ShaderType.Vertex;
            else
                throw new ArgumentException("Invalid shader type \"" + reader.Value + "\"");*/
            reader.Skip();

            // TODO: Shader support
            _results.Layout.Add(new BasicTagLayoutField(name, BasicFieldType.UInt32));
            RegisterField(offset, 4);
        }

        private void ReadUnicList(XmlReader reader, string name, uint offset, bool visible,
            uint pluginLine)
        {
            if (!reader.MoveToAttribute("languages"))
                throw new ArgumentException("Unicode string lists must have a languages attribute." + PositionInfo(reader));
            int languages = ParseInt(reader.Value);

            _results.Layout.Add(new ArrayTagLayoutField(new BasicTagLayoutField(name, BasicFieldType.Int32), languages));
            RegisterField(offset, (uint)languages * 4);
        }

        private void RegisterField(uint fieldOffset, uint fieldSize)
        {
            _nextOffset = Math.Max(_nextOffset, fieldOffset + fieldSize);
        }

        private static string PositionInfo(XmlReader reader)
        {
            var info = reader as IXmlLineInfo;
            return info != null ? string.Format(" Line {0}, position {1}.", info.LineNumber, info.LinePosition) : "";
        }

        private static int ParseInt(string str)
        {
            if (str.StartsWith("0x"))
                return int.Parse(str.Substring(2), NumberStyles.HexNumber);
            if (str.StartsWith("-0x"))
                return -int.Parse(str.Substring(3), NumberStyles.HexNumber);
            return int.Parse(str);
        }

        private static uint ParseUInt(string str)
        {
            return str.StartsWith("0x") ? uint.Parse(str.Substring(2), NumberStyles.HexNumber) : uint.Parse(str);
        }

        private static bool ParseBool(string str)
        {
            return (str == "1" || str.ToLower() == "true");
        }
    }

    /// <summary>
    /// Contains the results from loading an Assembly plugin.
    /// </summary>
    public class AssemblyPluginLoadResults
    {
        public AssemblyPluginLoadResults()
        {
            Conflicts = new List<AssemblyPluginFieldConflict>();
        }

        /// <summary>
        /// The layout that was loaded.
        /// </summary>
        public TagLayout Layout { get; set; }

        /// <summary>
        /// Gets a list of any conflicts that occurred.
        /// Conflicts occur when a field is not at the offset it is expected to be at.
        /// </summary>
        public List<AssemblyPluginFieldConflict> Conflicts { get; private set; }
    }

    /// <summary>
    /// Contains information about a conflict in an Assembly plugin.
    /// </summary>
    public class AssemblyPluginFieldConflict
    {
        public AssemblyPluginFieldConflict(string name, uint offset, string block)
        {
            Name = name;
            Offset = offset;
            Block = block;
        }

        /// <summary>
        /// Gets the name of the field that caused the conflict.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the offset of the field that caused the conflict.
        /// </summary>
        public uint Offset { get; private set; }

        /// <summary>
        /// Gets the name of the block that the field is in. Can be <c>null</c> if the field is not in a tag block.
        /// </summary>
        public string Block { get; private set; }
    }
}