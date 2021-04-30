using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Shaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Gen3;
using TagTool.Commands.Common;

namespace TagTool.Commands.Files
{
    class GenerateAssemblyPluginsCommand : Command
    {

        public GenerateAssemblyPluginsCommand()
            : base(false,

                  "GenerateAssemblyPlugins",
                  "Converts tag definitions to Assembly Plugin files..",

                  "GenerateAssemblyPlugins [path]",

                  "If a path is specified, plugins will be saved to that directory.")
        {

        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return new TagToolError(CommandError.ArgCount);

            string path = null;

            if (args.Count == 1)
                path = args[0];
            var definitions = new TagDefinitionsGen3();
            foreach (KeyValuePair<TagGroup, Type> tagType in definitions.Gen3Types)
            {
                if (tagType.Key.Tag == "test")  // skip test definition as it contains unsuported bounds of type long and ulong
                    continue;

                foreach (KeyValuePair<CacheVersion, string> assemblyVersion in assemblyCacheVersions)
                {
                    if (path != null)
                    {
                        try
                        {
                            ConvertTagDefinition(tagType.Key.Tag, tagType.Value, assemblyVersion.Key, path);
                        }
                        catch (ArgumentException)
                        {
                            return new TagToolError(CommandError.CustomMessage, "Invalid path argument");
                        }
                    }
                    else
                        ConvertTagDefinition(tagType.Key.Tag, tagType.Value, assemblyVersion.Key);
                }
            }

            Console.Write("Done!");

            return true;
        }

        /// <summary>
        /// Stores data for a field in an assembly plugin and contains various functions to convert fields from BlamCore tag definitions.
        /// </summary>
        public class AssemblyPluginField
        {
            /// <summary>
            /// The field type.
            /// </summary>
            readonly AssemblyPluginFieldTypes type = AssemblyPluginFieldTypes.undefined;
            /// <summary>
            /// The field attributes. Eg, color format, string length.
            /// </summary>
            Dictionary<string, string> attributes = new Dictionary<string, string>() { { "name", "" }, { "visible", "true" } };
            /// <summary>
            /// The field children. Eg enum options, reflexive contents.
            /// </summary>
            List<AssemblyPluginField> children = new List<AssemblyPluginField>();

            /// <summary>
            /// Returns the size of the field in bytes.
            /// </summary>
            public int Size
            {
                get
                {
                    if (type == AssemblyPluginFieldTypes.ascii)
                        return Convert.ToInt32(attributes["length"], 16);

                    return assemblyPluginFieldTypeSizes.ContainsKey(type) ? assemblyPluginFieldTypeSizes[type] : 0;
                }
            }

            /// <summary>
            /// Returns the field's name attribute.
            /// </summary>
            public string Name
            {
                get
                {
                    return attributes["Name"];
                }
            }


            /// <summary>
            /// Field types recognized in Assembly.
            /// </summary>
            public enum AssemblyPluginFieldTypes
            {
                undefined,
                uint8,
                int8,
                uint16,
                int16,
                uint32,
                int32,
                float32,
                @float,
                vector3,
                degree,
                stringId,
                tagref,
                range,
                ascii,
                utf16,
                bitfield8,
                bitfield16,
                bitfield32,
                bit, //Not a field type
                enum8,
                enum16,
                enum32,
                option, //Not a field type
                color,
                colour,
                color24,
                colour24,
                color32,
                colour32,
                colorf,
                colourf,
                dataRef,
                reflexive,
                raw,
                comment,
                shader, //unused, reflexive instead
                uniclist //unused, reflexive instead
            }

            /// <summary>
            /// Contains AssemblyPluginType values for equevelant structure types available in BlamCore.
            /// Some assembly types are more complicated, such as bitfields which are enums with flags attribute or reflexives.
            /// </summary>
            static Dictionary<Type, AssemblyPluginFieldTypes> assemblyPluginTypeEquivalent = new Dictionary<Type, AssemblyPluginFieldTypes>()
            {
                {typeof(System.Byte), AssemblyPluginFieldTypes.uint8},
                {typeof(System.SByte), AssemblyPluginFieldTypes.int8},
                {typeof(System.UInt16), AssemblyPluginFieldTypes.uint16},
                {typeof(System.Int16), AssemblyPluginFieldTypes.int16},
                {typeof(System.UInt32), AssemblyPluginFieldTypes.uint32},
                {typeof(System.Int32), AssemblyPluginFieldTypes.int32},
                {typeof(System.Single), AssemblyPluginFieldTypes.float32},
                //{typeof(RealVector3d), AssemblyPluginFieldTypes.vector3},
                {typeof(RealPoint3d), AssemblyPluginFieldTypes.vector3},
                //{typeof(RealEulerAngles3d), AssemblyPluginFieldTypes.vector3},
                //{typeof(RealEulerAngles2d), AssemblyPluginFieldTypes.range},
                //{typeof(RealPoint2d), AssemblyPluginFieldTypes.range},
                {typeof(Angle), AssemblyPluginFieldTypes.degree},
                {typeof(StringId), AssemblyPluginFieldTypes.stringId},
                {typeof(CachedTag), AssemblyPluginFieldTypes.tagref},
                //{typeof(RealVector2d), AssemblyPluginFieldTypes.range},
                {typeof(RealArgbColor), AssemblyPluginFieldTypes.colorf },
                {typeof(RealRgbColor), AssemblyPluginFieldTypes.colorf },
                //{typeof(RgbColor), AssemblyPluginFieldTypes.color24},
                {typeof(ArgbColor), AssemblyPluginFieldTypes.color},
                {typeof(CachedTagData), AssemblyPluginFieldTypes.dataRef},
                {typeof(TagFunction), AssemblyPluginFieldTypes.dataRef },
                {typeof(VertexShaderReference), AssemblyPluginFieldTypes.shader},
                {typeof(PixelShaderReference), AssemblyPluginFieldTypes.shader },
                {typeof(Boolean), AssemblyPluginFieldTypes.int8 },
            };

            /// <summary>
            /// Sizes of types recognized in assembly. Some types don't have simple sizes, like strings which use their length attribute instead.
            /// </summary>
            static Dictionary<AssemblyPluginFieldTypes, int> assemblyPluginFieldTypeSizes = new Dictionary<AssemblyPluginFieldTypes, int>()
            {
                {AssemblyPluginFieldTypes.uint8, 1},
                {AssemblyPluginFieldTypes.int8, 1},
                {AssemblyPluginFieldTypes.uint16, 2},
                {AssemblyPluginFieldTypes.int16, 2},
                {AssemblyPluginFieldTypes.uint32, 4},
                {AssemblyPluginFieldTypes.int32, 4},
                {AssemblyPluginFieldTypes.float32, 4},
                {AssemblyPluginFieldTypes.@float, 4},
                {AssemblyPluginFieldTypes.undefined, 4}, //A float in Assembly
                {AssemblyPluginFieldTypes.vector3, 12},
                {AssemblyPluginFieldTypes.degree, 4},
                {AssemblyPluginFieldTypes.stringId, 4},
                {AssemblyPluginFieldTypes.tagref, 16},
                {AssemblyPluginFieldTypes.range, 8},
                {AssemblyPluginFieldTypes.bitfield8, 1},
                {AssemblyPluginFieldTypes.bitfield16, 2},
                {AssemblyPluginFieldTypes.bitfield32, 4},
                {AssemblyPluginFieldTypes.enum8, 1},
                {AssemblyPluginFieldTypes.enum16, 2},
                {AssemblyPluginFieldTypes.enum32, 4},
                //{AssemblyPluginFieldTypes.color, 4}, //I think the size of these depends on the format.
                //{AssemblyPluginFieldTypes.colour, 4}, //But we don't need them anyway.
                {AssemblyPluginFieldTypes.color24, 3},
                {AssemblyPluginFieldTypes.colour24, 3},
                {AssemblyPluginFieldTypes.color32, 4},
                {AssemblyPluginFieldTypes.colour32, 4},
                {AssemblyPluginFieldTypes.colorf, 12},
                {AssemblyPluginFieldTypes.colourf, 12},
                {AssemblyPluginFieldTypes.dataRef, 20},
                {AssemblyPluginFieldTypes.reflexive, 12},
                {AssemblyPluginFieldTypes.raw, 20},
                {AssemblyPluginFieldTypes.comment, 0 },
                {AssemblyPluginFieldTypes.shader, 4}
            };

            /// <summary>
            /// Creates an AssemblyPluginField of the given type, with the given name.
            /// </summary>
            /// <param name="type">The type of plugin field.</param>
            /// <param name="name">The name attrubute of the field.</param>
            public AssemblyPluginField(AssemblyPluginFieldTypes type, string name)
            {
                if (type == AssemblyPluginFieldTypes.bit || type == AssemblyPluginFieldTypes.option)
                    attributes.Remove("visible");

                this.type = type;

                if (name != null)
                    attributes["name"] = name;
                else
                    attributes["name"] = type.ToString();
            }

            /// <summary>
            /// Creates an AssemblyPluginField of the given type, with the given name, and adds the given attributes.
            /// </summary>
            /// <param name="type">The type of plugin field.</param>
            /// <param name="name">The name attrubute of the field.</param>
            /// <param name="attributes">Plugin field attributes such as string length or color format to add to the field.</param>
            public AssemblyPluginField(AssemblyPluginFieldTypes type, string name, Dictionary<string, string> attributes) : this(type, name)
            {
                //Merge attributes dictionary
                this.attributes = new Dictionary<string, string>[] { this.attributes, attributes }
                         .SelectMany(dict => dict)
                         .ToLookup(pair => pair.Key, pair => pair.Value)
                         .ToDictionary(group => group.Key, group => group.First());
            }

            /// <summary>
            /// Creates an AssemblyPluginField of the given type, with the given name, and adds the offset attribute.
            /// The size of the field is added to the offset, so that the converter can move to the next field offset.
            /// </summary>
            /// <param name="type">The type of plugin field.</param>
            /// <param name="name">The name attrubute of the field.</param>
            /// <param name="offset">The offset of the field. The size of the field is added, moving on to the next field.</param>
            public AssemblyPluginField(AssemblyPluginFieldTypes type, string name, ref int offset) : this(type, name)
            {
                attributes.Add("offset", "0x" + offset.ToString("X"));
                offset += Size;
            }

            /// <summary>
            /// Creates an AssemblyPluginField of the given type, with the given name, adds the offset attribute and the other attributes provided.
            /// The size of the field is added to the offset, so that the converter can move to the next field offset.
            /// </summary>
            /// <param name="type">The type of plugin field.</param>
            /// <param name="name">The name attrubute of the field.</param>
            /// <param name="offset">The offset of the field. The size of the field is added, moving on to the next field.</param>
            /// <param name="attributes">Plugin field attributes such as string length or color format to add to the field.</param>
            public AssemblyPluginField(AssemblyPluginFieldTypes type, string name, ref int offset, Dictionary<string, string> attributes) : this(type, name)
            {
                this.attributes.Add("offset", "0x" + offset.ToString("X"));

                this.attributes = new Dictionary<string, string>[] { this.attributes, attributes }
                         .SelectMany(dict => dict)
                         .ToLookup(pair => pair.Key, pair => pair.Value)
                         .ToDictionary(group => group.Key, group => group.First());

                offset += Size;
            }


            /// <summary>
            /// Common field types which can be converted to assembly plugin fields.
            /// </summary>
            public static class CommonFieldTypes
            {
                /// <summary>
                /// Returns the assembly plugin fields that represent a RealMatrix4x4
                /// </summary>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent RealMatrix4x3.</returns>
                public static List<AssemblyPluginField> RealMatrix4x3(string fieldName, ref int offset)
                {
                    return new List<AssemblyPluginField>
                {
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M11", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M12", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M13", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M21", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M22", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M23", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M31", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M32", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M33", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M41", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M42", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "M43", ref offset)
                };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a Rectangle2d
                /// </summary>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent Rectangle2d, top left bottom right.</returns>
                public static List<AssemblyPluginField> Rectangle2d(string fieldName, ref int offset)
                {
                    return new List<AssemblyPluginField>
                {
                    new AssemblyPluginField(AssemblyPluginFieldTypes.int16, fieldName + "Top", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.int16, fieldName + "Left", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.int16, fieldName + "Bottom", ref offset),
                    new AssemblyPluginField(AssemblyPluginFieldTypes.int16, fieldName + "Right", ref offset)
                };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent Bounds.
                /// </summary>
                /// <param name="type">The boundry value type.</param>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent Bounds, min and max.</returns>
                public static List<AssemblyPluginField> Bounds(AssemblyPluginFieldTypes type, string fieldName, ref int offset)
                {
                    return new List<AssemblyPluginField>()
                {
                    new AssemblyPluginField(type, fieldName + "Min", ref offset),
                    new AssemblyPluginField(type, fieldName + "Max", ref offset)
                };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a RealBoundingBox
                /// </summary>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent RealBoundingBox, bounds x y z.</returns>
                /// <seealso cref="Bounds(AssemblyPluginFieldTypes, string, ref int)"/>
                public static List<AssemblyPluginField> RealBoundingBox(string fieldName, ref int offset)
                {
                    List<AssemblyPluginField> ret = new List<AssemblyPluginField>();
                    ret.AddRange(Bounds(AssemblyPluginFieldTypes.@float, fieldName + "X", ref offset));
                    ret.AddRange(Bounds(AssemblyPluginFieldTypes.@float, fieldName + "Y", ref offset));
                    ret.AddRange(Bounds(AssemblyPluginFieldTypes.@float, fieldName + "Z", ref offset));
                    return ret;
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a Vector2.
                /// </summary>
                /// <param name="format">The names of the two fields. Eg x and y, i and j.</param>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent Vector2, two floats.</returns>
                public static List<AssemblyPluginField> Point2(string[] format, string fieldName, ref int offset)
                {
                    if (format.Count() != 2)
                        throw new ArgumentException("Invalid Point2 Format");

                    return new List<AssemblyPluginField>
                    {
                        new AssemblyPluginField(AssemblyPluginFieldTypes.int16, fieldName + format[0], ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.int16, fieldName + format[1], ref offset),
                    };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a Vector2.
                /// </summary>
                /// <param name="format">The names of the two fields. Eg x and y, i and j.</param>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent Vector2, two floats.</returns>
                public static List<AssemblyPluginField> Vector2(string[] format, string fieldName, ref int offset)
                {
                    if (format.Count() != 2)
                        throw new ArgumentException("Invalid Vector2 Format");

                    return new List<AssemblyPluginField>
                    {
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + format[0], ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + format[1], ref offset),
                    };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a Vector2.
                /// </summary>
                /// <param name="format">The names of the two fields. Eg x and y, i and j.</param>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent Vector2, two floats.</returns>
                public static List<AssemblyPluginField> Angle2(string[] format, string fieldName, ref int offset)
                {
                    if (format.Count() != 2)
                        throw new ArgumentException("Invalid Angle2 Format");

                    return new List<AssemblyPluginField>
                    {
                        new AssemblyPluginField(AssemblyPluginFieldTypes.degree, fieldName + format[0], ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.degree, fieldName + format[1], ref offset),
                    };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a Vector3.
                /// </summary>
                /// <param name="format">The names of the three fields. Eg x, y and z, i j and k.</param>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent Vector3, three floats.</returns>
                public static List<AssemblyPluginField> Vector3(string[] format, string fieldName, ref int offset)
                {
                    if (format.Count() != 3)
                        throw new ArgumentException("Invalid Vector3 Format");

                    return new List<AssemblyPluginField>
                    {
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + format[0], ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + format[1], ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + format[2], ref offset)
                    };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a Vector3.
                /// </summary>
                /// <param name="format">The names of the three fields. Eg x, y and z, i j and k.</param>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent Vector3, three floats.</returns>
                public static List<AssemblyPluginField> Angle3(string[] format, string fieldName, ref int offset)
                {
                    if (format.Count() != 3)
                        throw new ArgumentException("Invalid Angle3 Format");

                    return new List<AssemblyPluginField>
                    {
                        new AssemblyPluginField(AssemblyPluginFieldTypes.degree, fieldName + format[0], ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.degree, fieldName + format[1], ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.degree, fieldName + format[2], ref offset)
                    };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a RealPlane3d.
                /// </summary>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent RealPlane3d, I J K and Distance floats.</returns>
                public static List<AssemblyPluginField> RealPlane3d(string fieldName, ref int offset)
                {
                    return new List<AssemblyPluginField>
                    {
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "I", ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "J", ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "K", ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "Distance", ref offset)
                    };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a Quaternion.
                /// </summary>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent Quaternion, four floats, I J K and W.</returns>
                public static List<AssemblyPluginField> RealQuaternion(string fieldName, ref int offset)
                {
                    return new List<AssemblyPluginField>
                    {
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "I", ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "J", ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "K", ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.@float, fieldName + "W", ref offset)
                    };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a RgbaColor.
                /// </summary>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent RgbaColor, an rgb format color24 and a uint representing Alpha.</returns>
                public static List<AssemblyPluginField> RgbaColor(string fieldName, ref int offset)
                {
                    return new List<AssemblyPluginField>
                    {
                        new AssemblyPluginField(AssemblyPluginFieldTypes.color24, fieldName, ref offset, new Dictionary<string, string>() { { "format", "rgb" } }),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.uint8, fieldName + "Alpha", ref offset)
                    };
                }

                /// <summary>
                /// Returns the assembly plugin fields that represent a Argb Color.
                /// </summary>
                /// <param name="fieldName">The name of the field in BlamCore.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>A list of AssemblyPluginFields that represent RgbaColor, an uint representing alpha and a rgb format color24</returns>
                public static List<AssemblyPluginField> ArgbColor(string fieldName, ref int offset)
                {
                    return new List<AssemblyPluginField> // TODO: fix little endian
                    {
                        new AssemblyPluginField(AssemblyPluginFieldTypes.uint8, fieldName + "Alpha", ref offset),
                        new AssemblyPluginField(AssemblyPluginFieldTypes.color24, fieldName, ref offset, new Dictionary<string, string>() { { "format", "rgb" } })
                    };
                }



                /// <summary>
                /// Returns a list of assembly plugin fields representing fields in the given type.
                /// </summary>
                /// <param name="type">The structure to return fields for.</param>
                /// <param name="cacheVersion">The cache version to return fields for.</param>
                /// <param name="fieldName">The name of the structure reference. If no name can be given provide null rather than an empty string.</param>
                /// <param name="offset">The offset of the reference if applicable, or 0.</param>
                /// <returns>A list of AssemblyPluginFields</returns>
                public static List<AssemblyPluginField> ReferencedStructure(Type type, CacheVersion cacheVersion, string fieldName, ref int offset)
                {
                    ConvertTagStructure(type, cacheVersion, out List<AssemblyPluginField> referenceStructurePluginFields, out int size, ref offset);

                    //For inline classes,
                    //Add the name of the inline reference
                    if (fieldName != null)
                        foreach (AssemblyPluginField field in referenceStructurePluginFields)
                            field.attributes["name"] = fieldName + ":" + field.attributes["name"];

                    return referenceStructurePluginFields;
                }

                /// <summary>
                /// Returns a plugin field representing an enum type.
                /// </summary>
                /// <param name="enumType">The type of enum to return as an assembly field.</param>
                /// <param name="pluginType">The assembly enum type.</param>
                /// <param name="fieldName">The name of the field.</param>
                /// <param name="offset">The tag field offset.</param>
                /// <returns>An assembly plugin field with children detailing the enum values.</returns>
                public static AssemblyPluginField Enum(Type enumType, AssemblyPluginFieldTypes pluginType, string fieldName, ref int offset)
                {
                    bool isBitfield = (pluginType == AssemblyPluginFieldTypes.bitfield16 || pluginType == AssemblyPluginFieldTypes.bitfield32 || pluginType == AssemblyPluginFieldTypes.bitfield8);

                    AssemblyPluginField enumPLuginField = new AssemblyPluginField(GetAssemblyPluginFieldType(enumType), fieldName, ref offset);

                    int valueIndex = 0;

                    foreach (object value in System.Enum.GetValues(enumType))
                    {
                        var underlyingType = System.Enum.GetUnderlyingType(enumType);
                        var typeofvalue = value.GetType();  //(int)Convert.ChangeType(value, underlyingType)
                        if (isBitfield && Convert.ToInt64(value) == 0) //Skip the "None" value.
                            continue;

                        if (isBitfield)
                            enumPLuginField.children.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.bit, value.ToString(), new Dictionary<string, string>() { { "index", valueIndex.ToString() } }));
                        else
                            enumPLuginField.children.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.option, value.ToString(), new Dictionary<string, string>() { { "value", "0x" + ((int)Convert.ChangeType(value, typeof(int))).ToString("X") } }));

                        valueIndex++;
                    }

                    return enumPLuginField;
                }
            }


            /// <summary>
            /// Returns a list of assemblt plugin fields representing the BlamCore tag definition field type provided.
            /// </summary>
            /// <param name="fieldType">The type of field to convert to assembly fields.</param>
            /// <param name="tagFieldAttribute">The tag field attribute attached to the field, or null.</param>
            /// <param name="offset">The offset of the tag field.</param>
            /// <param name="cacheVersion">The cache version to return fields for.</param>
            /// <param name="fieldName">The name of the field in the BlamCore tag definition.</param>
            /// <returns>A list of AssemblyPluginFields representing the given field type.</returns>
            public static List<AssemblyPluginField> GetAssemblyPluginFields(Type fieldType, TagFieldAttribute tagFieldAttribute, ref int offset, CacheVersion cacheVersion, string fieldName)
            {
                List<AssemblyPluginField> assemblyPluginFields = new List<AssemblyPluginField>();
                AssemblyPluginFieldTypes assemblyPluginFieldType = GetAssemblyPluginFieldType(fieldType);

                if (assemblyPluginFieldType == AssemblyPluginFieldTypes.bitfield16 || assemblyPluginFieldType == AssemblyPluginFieldTypes.bitfield32 || assemblyPluginFieldType == AssemblyPluginFieldTypes.bitfield8
                    || assemblyPluginFieldType == AssemblyPluginFieldTypes.enum16 || assemblyPluginFieldType == AssemblyPluginFieldTypes.enum32 || assemblyPluginFieldType == AssemblyPluginFieldTypes.enum8)
                {
                    assemblyPluginFields.Add(CommonFieldTypes.Enum(fieldType, assemblyPluginFieldType, fieldName, ref offset));
                }
                else if (assemblyPluginFieldType == AssemblyPluginFieldTypes.shader)
                {
                    if (fieldType == typeof(VertexShaderReference))
                        assemblyPluginFields.Add(new AssemblyPluginField(assemblyPluginFieldType, fieldName, ref offset, new Dictionary<string, string>() { { "type", "vertex" } }));
                    else if (fieldType == typeof(PixelShaderReference))
                        assemblyPluginFields.Add(new AssemblyPluginField(assemblyPluginFieldType, fieldName, ref offset, new Dictionary<string, string>() { { "type", "pixel" } }));
                    else
                        throw new NotImplementedException("Shader type not supported.");
                }
                else if (assemblyPluginFieldType == AssemblyPluginFieldTypes.color || assemblyPluginFieldType == AssemblyPluginFieldTypes.colour)
                {
                    if (fieldType == typeof(ArgbColor))
                        assemblyPluginFields.AddRange(CommonFieldTypes.ArgbColor(fieldName, ref offset));
                    else
                        throw new NotImplementedException("This color needs implementing to the converter!");
                }
                else if (assemblyPluginFieldType == AssemblyPluginFieldTypes.colorf || assemblyPluginFieldType == AssemblyPluginFieldTypes.colourf)
                {
                    if (fieldType == typeof(RealArgbColor))
                    {
                        assemblyPluginFields.Add(new AssemblyPluginField(assemblyPluginFieldType, fieldName, ref offset, new Dictionary<string, string>() { { "format", "argb" } }));
                        offset += 4;    // argb float is 0x10
                    }
                    else if (fieldType == typeof(RealRgbColor))
                    {
                        assemblyPluginFields.Add(new AssemblyPluginField(assemblyPluginFieldType, fieldName, ref offset, new Dictionary<string, string>() { { "format", "rgb" } }));
                    }
                    else
                        throw new NotImplementedException("This color needs implementing to the converter!");
                }
                else if (assemblyPluginFieldType == AssemblyPluginFieldTypes.reflexive)
                {
                    Type elementType = null;
                    AssemblyPluginField reflexiveAssemblyPluginField = new AssemblyPluginField(assemblyPluginFieldType, fieldName, ref offset);
                    AssemblyPluginFieldTypes elementAssemblyPluginFieldType;

                    if (fieldType.IsGenericType && (fieldType.GetGenericTypeDefinition() == typeof(List<>) || fieldType.GetGenericTypeDefinition() == typeof(TagBlock<>)))
                    {
                        elementType = fieldType.GetGenericArguments()[0];
                    }
                    else
                    {
                        //this means the field has been marked as reflexive but isnt a list but is a reflexive...
                        //Which should never happen but if it did, this needs implementing.
                        throw new NotImplementedException();
                    }

                    elementAssemblyPluginFieldType = GetAssemblyPluginFieldType(elementType);

                    if (elementAssemblyPluginFieldType == AssemblyPluginFieldTypes.undefined && (elementType.IsClass || (elementType.IsValueType && !elementType.IsEnum)) && !elementType.IsGenericType)
                    {
                        int childClassOffset = 0; // here
                        reflexiveAssemblyPluginField.children = CommonFieldTypes.ReferencedStructure(elementType, cacheVersion, null, ref childClassOffset);
                        reflexiveAssemblyPluginField.attributes.Add("entrySize", "0x" + childClassOffset.ToString("X"));
                        assemblyPluginFields.Add(reflexiveAssemblyPluginField);
                    }
                    else
                    {
                        int childOffset = 0;
                        //Handles stuff like a list of tag references.
                        reflexiveAssemblyPluginField.children.AddRange(GetAssemblyPluginFields(elementType, tagFieldAttribute, ref childOffset, cacheVersion, null)); //This tagFieldAttribute arg should probably be null.
                        reflexiveAssemblyPluginField.attributes.Add("entrySize", "0x" + childOffset.ToString("X"));
                        assemblyPluginFields.Add(reflexiveAssemblyPluginField);
                    }
                }
                else if (assemblyPluginFieldType == AssemblyPluginFieldTypes.undefined)
                {
                    //This covers quite a few cases.
                    //Types like vectors that can't easily be converted to assembly types belong here.
                    if (fieldType == typeof(ulong))
                    {
                        assemblyPluginFields.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.uint32, fieldName + " 0", ref offset));
                        assemblyPluginFields.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.uint32, fieldName + " 1", ref offset));
                    }
                    else if (fieldType == typeof(long))
                    {
                        assemblyPluginFields.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.uint32, fieldName + " 0", ref offset));
                        assemblyPluginFields.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.int32, fieldName + " 1", ref offset));
                    }
                    else if (fieldType == typeof(string))
                    {
                        assemblyPluginFields.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.ascii, fieldName, ref offset, new Dictionary<string, string>() { { "length", "0x" + tagFieldAttribute.Length.ToString("X") } }));
                    }
                    else if (fieldType == typeof(Tag))
                    {
                        //<ascii name="Signature" offset="0x0" visible="true" length="0x4" />
                        assemblyPluginFields.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.ascii, fieldName, ref offset, new Dictionary<string, string>() { { "length", "0x4" } }));
                    }
                    //Handles bounds
                    else if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Bounds<>))
                    {
                        Type boundsValueType = fieldType.GetGenericArguments()[0];

                        //Only bounds of primitive and directly convertable types are supported at the moment.
                        if (assemblyPluginTypeEquivalent.ContainsKey(boundsValueType))
                            assemblyPluginFields.AddRange(CommonFieldTypes.Bounds(assemblyPluginTypeEquivalent[boundsValueType], fieldName, ref offset));
                        else
                            throw new NotImplementedException(boundsValueType.ToString() + " bounds are not supported.");
                    }
                    else if (fieldType == typeof(Point2d))
                        assemblyPluginFields.AddRange(CommonFieldTypes.Point2(new string[2] { "X", "Y" }, fieldName, ref offset));
                    else if (fieldType == typeof(RealBoundingBox))
                        assemblyPluginFields.AddRange(CommonFieldTypes.RealBoundingBox(fieldName, ref offset));
                    else if (fieldType == typeof(RealEulerAngles2d))
                        assemblyPluginFields.AddRange(CommonFieldTypes.Angle2(new string[2] { "Yaw", "Pitch" }, fieldName, ref offset));
                    else if (fieldType == typeof(RealEulerAngles3d))
                        assemblyPluginFields.AddRange(CommonFieldTypes.Angle3(new string[3] { "Yaw", "Pitch", "Roll" }, fieldName, ref offset));
                    else if (fieldType == typeof(RealPlane2d))
                        assemblyPluginFields.AddRange(CommonFieldTypes.Vector3(new string[3] { "I", "J", "Distance" }, fieldName, ref offset));
                    else if (fieldType == typeof(RealPlane3d))
                        assemblyPluginFields.AddRange(CommonFieldTypes.RealPlane3d(fieldName, ref offset));
                    else if (fieldType == typeof(RealPoint2d))
                        assemblyPluginFields.AddRange(CommonFieldTypes.Vector2(new string[2] { "X", "Y" }, fieldName, ref offset));
                    //else if (fieldType == typeof(RealPoint3d))
                    //    assemblyPluginFields.AddRange(Vector3(new string[3] { "X", "Y", "Z" }, fieldName, ref offset));
                    else if (fieldType == typeof(RealQuaternion))
                        assemblyPluginFields.AddRange(CommonFieldTypes.RealQuaternion(fieldName, ref offset));
                    else if (fieldType == typeof(RealVector2d))
                        assemblyPluginFields.AddRange(CommonFieldTypes.Vector2(new string[2] { "I", "J" }, fieldName, ref offset));
                    else if (fieldType == typeof(RealVector3d))
                        assemblyPluginFields.AddRange(CommonFieldTypes.Vector3(new string[3] { "I", "J", "K" }, fieldName, ref offset));
                    else if (fieldType == typeof(Rectangle2d))
                        assemblyPluginFields.AddRange(CommonFieldTypes.Rectangle2d(fieldName, ref offset));
                    else if (fieldType == typeof(RealMatrix4x3))
                        assemblyPluginFields.AddRange(CommonFieldTypes.RealMatrix4x3(fieldName, ref offset));
                    // Handles datum indices
                    else if (fieldType == typeof(DatumHandle))
                    {
                        assemblyPluginFields.AddRange(
                            cacheVersion > CacheVersion.Halo2Vista && cacheVersion < CacheVersion.HaloOnline106708 ?
                            new[]
                            {
                                new AssemblyPluginField(AssemblyPluginFieldTypes.uint16, fieldName + " Identifier", ref offset),
                                new AssemblyPluginField(AssemblyPluginFieldTypes.uint16, fieldName + " Index", ref offset)
                            } :
                            new[]
                            {
                                new AssemblyPluginField(AssemblyPluginFieldTypes.uint16, fieldName + " Index", ref offset),
                                new AssemblyPluginField(AssemblyPluginFieldTypes.uint16, fieldName + " Identifier", ref offset)
                            });
                    }
                    //Handles resource pointers
                    else if (fieldType == typeof(PageableResource))
                        assemblyPluginFields.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.uint32, fieldName, ref offset));
                    //Handles arrays of classes which are serialized/deserialized sequentially.
                    else if (fieldType.IsArray)
                    {
                        Type elementType = fieldType.GetElementType();
                        AssemblyPluginFieldTypes elementAssemblyPluginType = GetAssemblyPluginFieldType(elementType);
                        if (elementType.IsPrimitive && elementAssemblyPluginType != AssemblyPluginFieldTypes.undefined && elementAssemblyPluginType != AssemblyPluginFieldTypes.reflexive)
                        {
                            if (tagFieldAttribute.Length < 1)
                            {
                                assemblyPluginFields.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.dataRef, fieldName, ref offset));
                            }
                            else
                            {
                                for (int i = 0; i < tagFieldAttribute.Length; i++)
                                {
                                    if (tagFieldAttribute.Flags.HasFlag(Padding))
                                    {
                                        AssemblyPluginField assemblyPluginField = new AssemblyPluginField(elementAssemblyPluginType, fieldName + "Padding " + i.ToString(), ref offset);
                                        assemblyPluginField.attributes["visible"] = "false";
                                        assemblyPluginFields.Add(assemblyPluginField);
                                    }
                                    else
                                        assemblyPluginFields.Add(new AssemblyPluginField(elementAssemblyPluginType, fieldName + " " + i.ToString(), ref offset));
                                }
                            }
                        }
                        if (elementAssemblyPluginType == AssemblyPluginFieldTypes.undefined && (elementType.IsClass || (elementType.IsValueType && !elementType.IsEnum)) && !elementType.IsGenericType)
                        {
                            for (int i = 0; i < tagFieldAttribute.Length; i++)
                            {
                                List<AssemblyPluginField> structureFields = CommonFieldTypes.ReferencedStructure(elementType, cacheVersion, null, ref offset);
                                var comment = new AssemblyPluginField(AssemblyPluginFieldTypes.comment, fieldName);
                                comment.attributes["name"] = comment.attributes["name"] + " " + i.ToString();
                                assemblyPluginFields.Add(comment);
                                assemblyPluginFields.AddRange(structureFields);
                            }
                        }
                    }
                    //Handles classes or structs.
                    else if ((fieldType.IsClass || (fieldType.IsValueType && !fieldType.IsEnum)) && !fieldType.IsGenericType)
                    {
                        assemblyPluginFields.Add(new AssemblyPluginField(AssemblyPluginFieldTypes.comment, fieldName));
                        assemblyPluginFields.AddRange(CommonFieldTypes.ReferencedStructure(fieldType, cacheVersion, null, ref offset));
                    }
                    else
                        throw new NotImplementedException($"Undefined field type \"{fieldType}\" not implemented.");
                }
                else if (assemblyPluginFieldType == AssemblyPluginFieldTypes.tagref)
                {
                    if (tagFieldAttribute.Flags.HasFlag(Short))
                    {
                        assemblyPluginFields.Add(new AssemblyPluginField(assemblyPluginFieldType, fieldName, ref offset, new Dictionary<string, string>() { { "withClass", "false" } }));
                        offset -= 12;    
                    }
                    else
                    {
                        assemblyPluginFields.Add(new AssemblyPluginField(assemblyPluginFieldType, fieldName, ref offset));
                    }
                }
                else
                    assemblyPluginFields.Add(new AssemblyPluginField(assemblyPluginFieldType, fieldName, ref offset));

                return assemblyPluginFields;
            }

            /// <summary>
            /// Returns the assembly field type equivalent of a given type, or undefined if there isn't one.
            /// If the BlamCore field converts to multiple assembly fields this will return undefined.
            /// If the type cannot be resolved this will return undefined.
            /// </summary>
            /// <param name="fieldType">The field type in BlamCore.</param>
            /// <returns>The field type in Assembly.</returns>
            private static AssemblyPluginFieldTypes GetAssemblyPluginFieldType(Type fieldType)
            {
                AssemblyPluginFieldTypes assemblyPluginFieldType = AssemblyPluginFieldTypes.undefined;

                if (assemblyPluginTypeEquivalent.ContainsKey(fieldType))
                    assemblyPluginFieldType = assemblyPluginTypeEquivalent[fieldType];

                if (fieldType.IsEnum)
                {
                    bool bitfield = fieldType.GetCustomAttribute<FlagsAttribute>() != null;
                    Type underlyingType = fieldType.GetEnumUnderlyingType();

                    if (underlyingType == typeof(System.Int32) || underlyingType == typeof(System.UInt32))
                        assemblyPluginFieldType = bitfield ? AssemblyPluginFieldTypes.bitfield32 : AssemblyPluginFieldTypes.enum32;
                    else if (underlyingType == typeof(System.Int16) || underlyingType == typeof(System.UInt16))
                        assemblyPluginFieldType = bitfield ? AssemblyPluginFieldTypes.bitfield16 : AssemblyPluginFieldTypes.enum16;
                    else if (underlyingType == typeof(System.SByte) || underlyingType == typeof(System.Byte))
                        assemblyPluginFieldType = bitfield ? AssemblyPluginFieldTypes.bitfield8 : AssemblyPluginFieldTypes.enum8;
                }
                else if (fieldType.IsGenericType && (fieldType.GetGenericTypeDefinition() == typeof(List<>) || fieldType.GetGenericTypeDefinition() == typeof(TagBlock<>)))
                {
                    assemblyPluginFieldType = AssemblyPluginFieldTypes.reflexive;
                }

                return assemblyPluginFieldType;
            }

            /// <summary>
            /// Indent of the fields. This grows as the ToString method recurses through children.
            /// </summary>
            static string indent = "	";
            /// <summary>
            /// Converts the assembly plugin field to an XML Node suitable for a plugin.
            /// </summary>
            /// <returns><![CDATA[Returns the XML node of the field in the format <type name="{name}" offset="{offset}" {attributes}>{children}</type>]]></returns>
            public override string ToString()
            {

                

                attributes["name"] = ToSpaced(attributes["name"]);

                string formattedAttributes = "";
                foreach (KeyValuePair<string, string> attribute in attributes)
                {
                    formattedAttributes += String.Format(" {0}=\"{1}\"", attribute.Key, attribute.Value);
                }

                string nodeContents = "";
                foreach (AssemblyPluginField child in children)
                {
                    //indent += " ";
                    indent += "	";
                    nodeContents += child.ToString() + Environment.NewLine;
                    indent = indent.Substring(0, indent.Length - 1);
                }

                if (children.Count > 0)
                    nodeContents = Environment.NewLine + nodeContents + indent;

                string nodeFormat;

                if (children.Count <= 0)
                    nodeFormat = "{4}<{0}{1}/>";
                else
                    nodeFormat = "{4}<{0}{1}>{2}</{0}>";

                if (type == AssemblyPluginFieldTypes.comment)
                {
                    return $"{indent}<comment title=\"{ToSpaced(attributes["name"])}\"></comment>{Environment.NewLine}";
                }

                return String.Format(nodeFormat, type, formattedAttributes, nodeContents, Environment.NewLine, indent);
            }

            /// <summary>
            /// Adds spaces to a camel case string.
            /// </summary>
            /// <param name="string">The string to space.</param>
            /// <returns>Returns the string with spaces.</returns>
            /// <remarks>
            /// This method should really be moved. I wasn't sure where to put it. 
            /// It could use some tweaking to support symbols and acronyms too. 
            /// See CHGD for an example of the problem. -A
            /// </remarks>
            private static string ToSpaced(string @string)
            {
                var prevUpper = true;
                var result = "";

                foreach (var c in @string)
                {
                    if (char.IsUpper(c))
                    {
                        result += prevUpper ? c.ToString() : $" {c}";
                        prevUpper = true;
                    }
                    else
                    {
                        result += c;
                        prevUpper = false;
                    }
                }

                return result;
            }

            /// <summary>
            /// Converts a structure into assembly plugin fields.
            /// </summary>
            /// <param name="structureType">The structure type to find fields from.</param>
            /// <param name="cacheVersion">The cache version to return fields for.</param>
            /// <param name="pluginFields">The output fields.</param>
            /// <param name="size">The size of the structure.</param>
            public static void ConvertTagStructure(Type structureType, CacheVersion cacheVersion, out List<AssemblyPluginField> pluginFields, out int size)
            {
                int offset = 0;
                ConvertTagStructure(structureType, cacheVersion, out pluginFields, out size, ref offset);
            }

            /// <summary>
            /// BindingFlags to return only fields that aren't inherited. 
            /// </summary>
            const BindingFlags DeclaredOnlyLookup = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            /// <summary>
            /// Converts a structure into assembly plugin fields.
            /// </summary>
            /// <param name="structureType">The structure type to find fields from.</param>
            /// <param name="cacheVersion">The cache version to return fields for.</param>
            /// <param name="pluginFields">The output fields.</param>
            /// <param name="size">The size of the structure.</param>
            /// <param name="offset">The offset of the structure within the tag. Useful for structure references within structures.</param>
            public static void ConvertTagStructure(Type structureType, CacheVersion cacheVersion, out List<AssemblyPluginField> pluginFields, out int size, ref int offset)
            {
                pluginFields = new List<AssemblyPluginField>();

                FieldInfo[] structureFields = structureType.GetFields(DeclaredOnlyLookup);
                List<FieldInfo> inheritedFields = new List<FieldInfo>();


                List<Type> inheritedTypes = new List<Type>(); //Parent, Grandparent, Great-Grandparent...
                Type parentType = structureType.BaseType;

                while (parentType != typeof(TagStructure) && parentType != typeof(object) && parentType != null)
                {
                    inheritedTypes.Add(parentType);
                    parentType = parentType.BaseType;
                }

                inheritedTypes.Reverse();

                foreach (Type inheritedType in inheritedTypes)
                {
                    inheritedFields.AddRange(inheritedType.GetFields(DeclaredOnlyLookup));
                }

                foreach (FieldInfo fieldInfo in inheritedFields)
                {
                    //If the field isn't present in this cache version move on.
                    TagFieldAttribute tagFieldAttribute = fieldInfo.GetCustomAttributes<TagFieldAttribute>().Count() > 0 ? fieldInfo.GetCustomAttributes<TagFieldAttribute>().ElementAt(0) : new TagFieldAttribute();
                    if (!CacheVersionDetection.AttributeInCacheVersion(tagFieldAttribute, cacheVersion) || tagFieldAttribute.Flags.HasFlag(Runtime))
                        continue;

                    pluginFields.AddRange(GetAssemblyPluginFields(fieldInfo.FieldType, tagFieldAttribute, ref offset, cacheVersion, fieldInfo.Name));
                }

                foreach (FieldInfo fieldInfo in structureFields)
                {
                    //If the field isn't present in this cache version move on.
                    TagFieldAttribute tagFieldAttribute = fieldInfo.GetCustomAttributes<TagFieldAttribute>().Count() > 0 ? fieldInfo.GetCustomAttributes<TagFieldAttribute>().ElementAt(0) : new TagFieldAttribute();
                    if (!CacheVersionDetection.AttributeInCacheVersion(tagFieldAttribute, cacheVersion) || tagFieldAttribute.Flags.HasFlag(Runtime))
                        continue;

                    pluginFields.AddRange(GetAssemblyPluginFields(fieldInfo.FieldType, tagFieldAttribute, ref offset, cacheVersion, fieldInfo.Name));
                }

                size = offset;
            }
        }

        /// <summary>
        /// A list of assembly plugin game names for cache versions in BlamCore.
        /// If it's not here, it's not supported.
        /// </summary>
        static readonly Dictionary<CacheVersion, string> assemblyCacheVersions = new Dictionary<CacheVersion, string>()
        {
            {CacheVersion.HaloOnline106708, "HaloOnline" },
            {CacheVersion.Halo3Retail, "Halo3" },
            {CacheVersion.Halo3ODST, "ODST" },
        };

        /// <summary>
        /// Converts a tag definition to an assembly plugin.
        /// </summary>
        /// <remarks>
        /// The plugin is saved at <![CDATA[Plugins\{gameName}\{tagGroup}.xml]]>
        /// </remarks>
        /// <param name="tagGroup">The tag group, this is how assembly plugin files are named.</param>
        /// <param name="tagType">The type of tag to convert.</param>
        /// <param name="cacheVersion">The game to convert tag fields for.</param>
        public void ConvertTagDefinition(Tag tagGroup, Type tagType, CacheVersion cacheVersion)
        {
            if (!Directory.Exists("Plugins"))
                Directory.CreateDirectory("Plugins");

            ConvertTagDefinition(tagGroup, tagType, cacheVersion, "Plugins");
        }

        /// <summary>
        /// Converts a tag definition to an assembly plugin.
        /// </summary>
        /// <remarks>
        /// The plugin is saved at <![CDATA[{pluginsDirectory}\{gameName}\{tagGroup}.xml]]>
        /// </remarks>
        /// <param name="tagGroup">The tag group, this is how assembly plugin files are named.</param>
        /// <param name="tagType">The type of tag to convert.</param>
        /// <param name="cacheVersion">The game to convert tag fields for.</param>
        /// <param name="pluginsDirectory">The path to save the plugin files to.</param>
        public void ConvertTagDefinition(Tag tagGroup, Type tagType, CacheVersion cacheVersion, string pluginsDirectory)
        {
            if (!Directory.Exists(pluginsDirectory))
                throw new ArgumentException("Illegal plugins directory path!");

            string gameName = assemblyCacheVersions?[cacheVersion];

            //If no assembly game name was found the cache version is not supported.
            if (gameName == null)
                throw new ArgumentOutOfRangeException("Can't generate Assembly definition for game " + cacheVersion.ToString());

            //Do stuff.

#if DEBUG
            Console.WriteLine("Converting tag definition {0} for {1} to an assembly plugin at {3}\\{1}\\{2}.xml", tagGroup.ToString(), gameName, tagGroup.ToString().Replace('<', '_').Replace('>', '_'), pluginsDirectory);
#endif

            AssemblyPluginField.ConvertTagStructure(tagType, cacheVersion, out List<AssemblyPluginField> pluginFields, out int size);

            List<string> xmlNodesText = new List<string>
            {
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>",
                "<plugin game=\"" + gameName + "\" baseSize=\"0x" + size.ToString("X") + "\">",
                "	<!-- Automatically generated plugin -->",
                "	<revisions>",
                "		<revision author=\"TagTool\" version=\"1\">Generated plugin from TagTool definitions.</revision>",
                "	</revisions>"
            };

            foreach (AssemblyPluginField pluginField in pluginFields)
            {
                xmlNodesText.Add(pluginField.ToString());
            }

            xmlNodesText.Add("</plugin>");

            if (!Directory.Exists(pluginsDirectory + "\\" + gameName))
                Directory.CreateDirectory(pluginsDirectory + "\\" + gameName);

            File.WriteAllLines(String.Format("{0}\\{1}\\{2}.xml", pluginsDirectory, gameName, tagGroup.ToString().Replace('<', '_').Replace('>', '_')), xmlNodesText.ToArray());
        }
    }
}