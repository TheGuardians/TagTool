using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Scripting;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using TagTool.Tags;

namespace TagTool.Commands.Files
{
    class TestCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private static bool debugConsoleWrite = true;
        private static List<string> csvQueue1 = new List<string>();
        private static List<string> csvQueue2 = new List<string>();

        public TestCommand(GameCacheContext cacheContext) :
            base(CommandFlags.Inherit,
                
                "Test",
                "A test command.",
                
                "Test",
                
                "A test command. Used for various testing and temporary functionality.\n" +
                "Example setinvalidmaterials: 'Test setinvalidmaterials <ED mode or sbsp tag>'. Set all materials to 0x101F shaders\\invalid. \n\n")
        {
            CacheContext = cacheContext;
        }
        
        public override object Execute(List<string> args)
        {
            if (args.Count == 0)
                return false;

            var name = args[0].ToLower();
            args.RemoveAt(0);

            var commandsList = new Dictionary<string, string>
            {
                { "scriptingxml", "scriptingxml" },
                { "lensunknown", "lensunknown" },
                { "setinvalidmaterials", "Set all materials to shaders\\invalid or 0x101F to a provided mode or sbsp tag." },
                { "namemodetags", "Name all mode tags based on" },
                { "dumpforgepalettecommands", "Read a scnr tag's forge palettes and dump as a tagtool commands script." },
                { "nametag", "Name all dependencies of a named tag using the same nameUsage: test nametag all shaders\\invalid." },
                { "listtags", "Listtags with a simplified output." },
                { "dumpcommandsscript", "Extract all the tags of a mode or sbsp tag (rmt2, rm--) and generate a commands script. WIP" },
                { "shadowfix", "Hack/fix a weapon or forge object's shadow mesh." },
                { "namermt2", "Name all rmt2 tags based on their parent render method." },
                { "comparetags", "Compare and dump differences between two tags. Works between this and a different ms23 cache." },
                { "findconicaleffects", "" },
            };

            switch (name)
            {
                case "scriptingxml": return ScriptingXml(args);
                case "lensunknown": return LensUnknown(args);
                case "setinvalidmaterials": return SetInvalidMaterials(args);
                case "dumpforgepalettecommands": return DumpForgePaletteCommands(args);
                case "nametag": return NameTag(args);
                case "listtags": return ListTags(args);
                case "dumpcommandsscript": return DumpCommandsScript(args);
                case "temp": return Temp(args);
                case "shadowfix": return ShadowFix(args);
                case "comparetags": return CompareTags(args);
                case "namermt2": return NameRmt2();
                case "findconicaleffects": return FindConicalEffects();
                default:
                    Console.WriteLine($"Invalid command: {name}");
                    Console.WriteLine($"Available commands: {commandsList.Count}");
                    foreach (var a in commandsList)
                        Console.WriteLine($"{a.Key}: {a.Value}");
                    return false;
            }
        }

        private bool FindConicalEffects()
        {
            using (var stream = CacheContext.TagCacheFile.OpenRead())
            using (var reader = new BinaryReader(stream))
            {
                for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
                {
                    var tag = CacheContext.GetTag(i);

                    if (tag == null || !tag.IsInGroup("effe"))
                        continue;

                    stream.Position = tag.HeaderOffset + tag.DefinitionOffset + 0x5C;
                    var conicalDistributionCount = reader.ReadInt32();

                    if (conicalDistributionCount <= 0)
                        continue;

                    var tagName = CacheContext.TagNames.ContainsKey(tag.Index) ?
                        $"0x{tag.Index:X4} - {CacheContext.TagNames[tag.Index]}" :
                        $"0x{tag.Index:X4}";

                    Console.WriteLine($"{tagName}.effect - {conicalDistributionCount} {(conicalDistributionCount == 1 ? "distribution" : "distributions")}");
                }
            }

            return true;
        }

        public void CsvDumpQueueToFile(List<string> in_, string file)
        {
            var fileOut = new FileInfo(file);
            if (File.Exists(file))
                File.Delete(file);

            int i = -1;
            using (var csvStream = fileOut.OpenWrite())
            using (var csvWriter = new StreamWriter(csvStream))
            {
                foreach (var a in in_)
                {
                    csvStream.Position = csvStream.Length;
                    csvWriter.WriteLine(a);
                    i++;
                }
            }
        }

        public static void Csv1(string in_)
        {
            csvQueue1.Add(in_);
            if (debugConsoleWrite)
                Console.WriteLine($"{in_}");
        }

        public static void Csv2(string in_)
        {
            csvQueue2.Add(in_);
            if (debugConsoleWrite)
                Console.WriteLine($"{in_}");
        }

        private CacheFile OpenCacheFile(string cacheArg)
        {
            FileInfo blamCacheFile = new FileInfo(cacheArg);

            // Console.WriteLine("Reading H3 cache file...");

            if (!blamCacheFile.Exists)
                throw new FileNotFoundException(blamCacheFile.FullName);

            CacheFile BlamCache = null;

            using (var fs = new FileStream(blamCacheFile.FullName, FileMode.Open, FileAccess.Read))
            {
                var reader = new EndianReader(fs, EndianFormat.BigEndian);

                var head = reader.ReadInt32();

                if (head == 1684104552)
                    reader.Format = EndianFormat.LittleEndian;

                var v = reader.ReadInt32();

                reader.SeekTo(284);
                var version = CacheVersionDetection.GetFromBuildName(reader.ReadString(32));

                switch (version)
                {
                    case CacheVersion.Halo2Xbox:
                    case CacheVersion.Halo2Vista:
                        BlamCache = new CacheFileGen2(CacheContext, blamCacheFile, version);
                        break;

                    case CacheVersion.Halo3Retail:
                    case CacheVersion.Halo3ODST:
                    case CacheVersion.HaloReach:
                        BlamCache = new CacheFileGen3(CacheContext, blamCacheFile, version);
                        break;

                    default:
                        throw new NotSupportedException(CacheVersionDetection.GetBuildName(version));
                }
            }

            BlamCache.LoadResourceTags();

            return BlamCache;
        }

        private ScriptValueType.Halo3ODSTValue ParseScriptValueType(string value)
        {
            foreach (var option in Enum.GetNames(typeof(ScriptValueType)))
                if (value.ToLower().Replace("_", "").Replace(" ", "") == option.ToLower().Replace("_", "").Replace(" ", ""))
                    return (ScriptValueType.Halo3ODSTValue)Enum.Parse(typeof(ScriptValueType.Halo3ODSTValue), option);

            throw new KeyNotFoundException(value);
        }

        private bool ScriptingXml(List<string> args)
        {
            if (args.Count != 0)
                return false;

            //
            // Load the lower-version scription xml file
            //


            Console.WriteLine();
            Console.WriteLine("Enter the path to the lower-version scripting xml:");
            Console.Write("> ");

            var xmlPath = Console.ReadLine();

            var xml = new XmlDocument();
            xml.Load(xmlPath);

            var scripts = new Dictionary<int, ScriptInfo>();

            foreach (XmlNode node in xml["BlamScript"]["functions"])
            {
                if (node.NodeType != XmlNodeType.Element)
                    continue;

                var script = new ScriptInfo(
                    ParseScriptValueType(node.Attributes["returnType"].InnerText),
                    node.Attributes["name"].InnerText);

                if (script.Name == "")
                    continue;

                if (node.HasChildNodes)
                {
                    foreach (XmlNode argumentNode in node.ChildNodes)
                    {
                        if (argumentNode.NodeType != XmlNodeType.Element)
                            continue;

                        script.Arguments.Add(new ScriptInfo.ArgumentInfo(ParseScriptValueType(argumentNode.Attributes["type"].InnerText)));
                    }
                }
                
                scripts[int.Parse(node.Attributes["opcode"].InnerText.Replace("0x", ""), NumberStyles.HexNumber)] = script;
            }

            Console.WriteLine();

            for (var opcode = 0; opcode < scripts.Keys.Max(); opcode++)
            {
                if (!scripts.ContainsKey(opcode))
                    continue;
                
                var script = scripts[opcode];

                if (script.Arguments.Count == 0)
                {
                    Console.WriteLine($"                [0x{opcode:X3}] = new ScriptInfo(ScriptValueType.{script.Type}, \"{script.Name}\"),");
                }
                else
                {
                    Console.WriteLine($"                [0x{opcode:X3}] = new ScriptInfo(ScriptValueType.{script.Type}, \"{script.Name}\")");
                    Console.WriteLine("                {");

                    foreach (var argument in script.Arguments)
                        Console.WriteLine($"                    new ScriptInfo.ArgumentInfo(ScriptValueType.{argument.Type}),");

                    Console.WriteLine("                },");
                }
            }

            Console.WriteLine();

            return true;
        }

        private bool LensUnknown(List<string> args)
        {
            if (args.Count != 0)
                return false;

            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("lens"))
                {
                    var context = new TagSerializationContext(cacheStream, CacheContext, instance);
                    var definition = CacheContext.Deserializer.Deserialize<LensFlare>(context);
                }
            }

            return true;
        }

        private static CachedTagInstance PortTagReference(GameCacheContext cacheContext, CacheFile blamCache, int index)
        {
            if (index == -1)
                return null;

            var instance = blamCache.IndexItems.Find(i => i.ID == index);

            if (instance != null)
            {
                var chars = new char[] { ' ', ' ', ' ', ' ' };
                for (var i = 0; i < instance.ClassCode.Length; i++)
                    chars[i] = instance.ClassCode[i];

                var tags = cacheContext.TagCache.Index.FindAllInGroup(new string(chars));

                foreach (var tag in tags)
                {
                    if (!cacheContext.TagNames.ContainsKey(tag.Index))
                        continue;

                    if (instance.Filename == cacheContext.TagNames[tag.Index])
                        return tag;
                }
            }

            return null;
        }

        public bool SetInvalidMaterials(List<string> args) // Set all mode or sbsp shaders to shaders\invalid 0x101F
        {
            Console.WriteLine("Required args: [0]ED tag; ");

            if (args.Count != 1)
                return false;

            string edTagArg = args[0];

            CachedTagInstance edTag = CacheContext.GetTag(0x0);
            try
            {
                edTag = ArgumentParser.ParseTagName(CacheContext, edTagArg);
            }
            catch
            {
                edTag = ArgumentParser.ParseTagSpecifier(CacheContext, edTagArg);
            }

            if (edTag.IsInGroup("mode"))
            {
                RenderModel edMode;
                using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                    edMode = CacheContext.Deserializer.Deserialize<RenderModel>(edContext);
                }

                foreach (var a in edMode.Materials)
                    a.RenderMethod = CacheContext.GetTag(0x101F);

                using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var context = new TagSerializationContext(stream, CacheContext, edTag);
                    CacheContext.Serializer.Serialize(context, edMode);
                }
            }

            else if (edTag.IsInGroup("sbsp"))
            {
                ScenarioStructureBsp instance;
                using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                    instance = CacheContext.Deserializer.Deserialize<ScenarioStructureBsp>(edContext);
                }

                foreach (var a in instance.Materials)
                    a.RenderMethod = CacheContext.GetTag(0x101F);

                Console.WriteLine("Nuked shaders.");

                using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var context = new TagSerializationContext(stream, CacheContext, edTag);
                    CacheContext.Serializer.Serialize(context, instance);
                }
            }

            return true;
        }
        
        public bool DumpForgePaletteCommands(List<string> args) // Dump all the forge lists of a scnr to use as tagtool commands. Mainly to reorder the items easily
        {
            Console.WriteLine("Required args: [0]ED scnr tag; ");

            if (args.Count != 1)
                return false;

            string edTagArg = args[0];

            CachedTagInstance edTag = CacheContext.GetTag(0x0);
            try
            {
                edTag = ArgumentParser.ParseTagName(CacheContext, edTagArg);
            }
            catch
            {
                edTag = ArgumentParser.ParseTagSpecifier(CacheContext, edTagArg);
            }

            Scenario instance;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                instance = CacheContext.Deserializer.Deserialize<Scenario>(edContext);
            }

            Console.WriteLine($"RemoveBlockElements SandboxEquipment 0 *");
            foreach (var a in instance.SandboxEquipment)
            {
                Console.WriteLine($"AddBlockElements SandboxEquipment 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField SandboxEquipment[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField SandboxEquipment[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField SandboxEquipment[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            string type = "SandboxWeapons";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxWeapons)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            type = "SandboxVehicles";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxVehicles)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            type = "SandboxScenery";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxScenery)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            type = "SandboxSpawning";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxSpawning)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            type = "SandboxTeleporters";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxTeleporters)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            type = "SandboxGoalObjects";
            Console.WriteLine($"RemoveBlockElements {type} 0 *");
            foreach (var a in instance.SandboxGoalObjects)
            {
                Console.WriteLine($"AddBlockElements {type} 1");
                if (CacheContext.TagNames.ContainsKey(a.Object.Index))
                    Console.WriteLine($"SetField {type}[*].Object {CacheContext.TagNames[a.Object.Index]}.{a.Object.Group}");
                else
                    Console.WriteLine($"SetField {type}[*].Object 0x{a.Object.Index:X4}");

                Console.WriteLine($"SetField {type}[*].Name {CacheContext.StringIdCache.GetString(a.Name)}");

                Console.WriteLine("");
            }

            return true;
        }

        public bool NameTag(List<string> args)
        {
            bool flag = args.Count != 2;
            bool result;
            if (flag)
            {
                Console.WriteLine("Description: Name all dependencies of a named tag using the same nameUsage: test nametag all shaders\\invalid");
                result = false;
            }
            else
            {
                CachedTagInstance cachedTagInstance = ArgumentParser.ParseTagSpecifier(this.CacheContext, args[1]);
                string text = this.CacheContext.TagNames.ContainsKey(cachedTagInstance.Index) ? this.CacheContext.TagNames[cachedTagInstance.Index] : null;
                bool flag2 = text == null;
                if (flag2)
                {
                    Console.WriteLine("ERROR: the provided tag is not named.");
                    result = false;
                }
                else
                {
                    bool flag3 = args[0] == "all";
                    if (flag3)
                    {
                        IEnumerable<CachedTagInstance> enumerable = this.CacheContext.TagCache.Index.FindDependencies(cachedTagInstance);
                        foreach (CachedTagInstance current in enumerable)
                        {
                            string arg_102_0 = this.CacheContext.TagNames.ContainsKey(current.Index) ? this.CacheContext.TagNames[current.Index] : null;
                        }
                        foreach (CachedTagInstance current2 in enumerable)
                        {
                            bool flag4 = !this.CacheContext.TagNames.ContainsKey(current2.Index);
                            if (flag4)
                            {
                                string arg_187_0 = this.CacheContext.TagNames.ContainsKey(current2.Index) ? this.CacheContext.TagNames[current2.Index] : null;
                            }
                        }
                        foreach (CachedTagInstance current3 in enumerable)
                        {
                            bool flag5 = !this.CacheContext.TagNames.ContainsKey(current3.Index);
                            if (flag5)
                            {
                                Console.WriteLine(string.Format("[{0}] 0x{1:X4} new tagname: {2}", current3.Group, current3.Index, text));
                                this.CacheContext.TagNames.Add(current3.Index, text);
                            }
                        }
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        public bool ListTags(List<string> args)
        {
            if (args.Count == 0)
            {
                var tags = CacheContext.TagCache.Index.NonNull().ToArray();

                foreach (var tag in tags)
                {
                    if (!CacheContext.TagNames.ContainsKey(tag.Index))
                        Console.WriteLine($"0x{tag.Index:X4} {tag.Group.Tag} {CacheContext.GetString(tag.Group.Name)}");
                    else
                        Console.WriteLine($"0x{tag.Index:X4} {CacheContext.TagNames[tag.Index]}.{CacheContext.GetString(tag.Group.Name)}");
                }
            }
            else
            {
                foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup(args[0]))
                {
                    if (!CacheContext.TagNames.ContainsKey(tag.Index))
                    {
                        if (args.Count == 1)
                            Console.WriteLine($"0x{tag.Index:X4} {tag.Group.Tag} {CacheContext.GetString(tag.Group.Name)}");
                    }

                    else if (CacheContext.TagNames.ContainsKey(tag.Index))
                    {
                        if (args.Count == 2)
                        {
                            if (CacheContext.TagNames[tag.Index].Contains(args[1]))
                                Console.WriteLine($"0x{tag.Index:X4} {CacheContext.TagNames[tag.Index]}.{CacheContext.GetString(tag.Group.Name)}");
                        }
                        else
                            Console.WriteLine($"0x{tag.Index:X4} {CacheContext.TagNames[tag.Index]}.{CacheContext.GetString(tag.Group.Name)}");
                    }
                }
            }

            return true;
        }
        
        public bool DumpCommandsScript(List<string> args)
        {
            // Role: extract all the tags of a mode or sbsp tag.
            // Extract all the shaders of that tag, rmt2, vtsh, pixl and bitmaps of all the shaders
            // Dump commands to make a mod out of it.
            // Dump commands to reimport into a new build.

            // rmdf, rmt2, vtsh, pixl, mode, shader tags NEED to be named.

            if (args.Count != 1)
            {
                Console.WriteLine("Required args: [0]tag");
                return false;
            }

            string edTagArg = args[0];
            string modName = edTagArg.Split("\\".ToCharArray()).Last();

            CachedTagInstance instance = CacheContext.GetTag(0x0);
            try
            {
                instance = ArgumentParser.ParseTagName(CacheContext, edTagArg);
            }
            catch
            {
                instance = ArgumentParser.ParseTagSpecifier(CacheContext, edTagArg);
            }

            if (instance.IsInGroup("mode"))
            {
                IEnumerable<CachedTagInstance> dependencies = CacheContext.TagCache.Index.FindDependencies(instance);

                List<string> commands = new List<string>();

                // Console.WriteLine("All deps:");
                foreach (var dep in dependencies)
                {
                    // To avoid porting a ton of existing textures, bitmaps under 0x5726 should be ignored

                    // For stability and first runs, extract all. Filter out potentially existing tags later.
                    // if (dep.Group.ToString() == "bitm" && dep.Index < 0x5726)
                    // {
                    //     // Ignore default bitmaps for now
                    // }

                    // These are common for all the shaders, so chances are small to see they get removed.
                    if (dep.Group.Tag == "rmdf" || dep.Group.Tag == "rmop" || dep.Group.Tag == "glps" || dep.Group.Tag == "glvs")
                        continue;

                    string depname = CacheContext.TagNames.ContainsKey(dep.Index) ? CacheContext.TagNames[dep.Index] : $"0x{dep.Index:X4}";
                    string exportedTagName = $"{dep.Index:X4}";

                    // if (!CacheContext.TagNames.ContainsKey(dep.Index))
                    //     throw new Exception($"0x{dep.Index:X4} isn't named.");

                    Console.WriteLine($"extracttag 0x{dep.Index:X4} {exportedTagName}.{dep.Group.Tag}");

                    commands.Add($"createtag cfgt");
                    commands.Add($"NameTag * {depname}");
                    commands.Add($"importtag * {exportedTagName}.{dep.Group.Tag}");

                    // Console.WriteLine($"createtag cfgt");
                    // Console.WriteLine($"NameTag * {depname}");
                    // Console.WriteLine($"importtag * {exportedTagName}.{dep.Group.Tag}");

                    // Console.WriteLine($"Echo If the program quits at this point, the tagname is invalid.");
                    // Console.WriteLine($"EditTag {depname}.{dep.Group.Tag}");
                    // Console.WriteLine($"Exit");
                    // Console.WriteLine($"Dumplog {modName}.log");
                }

                Console.WriteLine("");
                foreach (var a in commands)
                    Console.WriteLine(a);

                RenderModel modeTag;
                using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, instance);
                    modeTag = CacheContext.Deserializer.Deserialize<RenderModel>(edContext);
                }

                var modename = CacheContext.TagNames[instance.Index];

                List<CachedTagInstance> shadersList = new List<CachedTagInstance>();

                Console.WriteLine("");

                Console.WriteLine($"EditTag {modename}.{instance.Group.Tag}");

                int i = -1;
                foreach (var material in modeTag.Materials)
                {
                    i++;
                    var shadername = CacheContext.TagNames[material.RenderMethod.Index];
                    Console.WriteLine($"SetField Materials[{i}].RenderMethod {shadername}.{material.RenderMethod.Group.Tag}");

                    shadersList.Add(material.RenderMethod);
                }

                Console.WriteLine($"SaveTagChanges");
                Console.WriteLine($"ExitTo tags");

                foreach (var shaderInstance in shadersList)
                {
                    ShaderDecal shaderTag;
                    using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                    {
                        var edContext = new TagSerializationContext(cacheStream, CacheContext, shaderInstance);
                        shaderTag = CacheContext.Deserializer.Deserialize<ShaderDecal>(edContext);
                    }

                    var shaderName = CacheContext.TagNames[shaderInstance.Index];
                    var rmdfName = CacheContext.TagNames.ContainsKey(shaderTag.BaseRenderMethod.Index) ? CacheContext.TagNames[shaderTag.BaseRenderMethod.Index] : $"0x{shaderTag.BaseRenderMethod.Index:X4}";
                    var rmt2Name = CacheContext.TagNames[shaderTag.ShaderProperties[0].Template.Index];

                    // Manage rmt2
                    RenderMethodTemplate rmt2Tag;
                    using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                    {
                        var edContext = new TagSerializationContext(cacheStream, CacheContext, shaderTag.ShaderProperties[0].Template);
                        rmt2Tag = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(edContext);
                    }

                    var vtshName = CacheContext.TagNames[rmt2Tag.VertexShader.Index];
                    var pixlName = CacheContext.TagNames[rmt2Tag.PixelShader.Index];

                    Console.WriteLine("");
                    Console.WriteLine($"EditTag {rmt2Name}.rmt2");
                    Console.WriteLine($"SetField VertexShader {vtshName}.vtsh");
                    Console.WriteLine($"SetField PixelShader {pixlName}.pixl");
                    Console.WriteLine($"SaveTagChanges");
                    Console.WriteLine($"ExitTo tags");

                    // Manage bitmaps
                    int j = -1;

                    Console.WriteLine("");
                    Console.WriteLine($"EditTag {shaderName}.{shaderInstance.Group.Tag}");
                    Console.WriteLine($"SetField BaseRenderMethod {rmdfName}.rmdf");
                    Console.WriteLine($"SetField ShaderProperties[0].Template {rmt2Name}.rmt2");
                    foreach (var a in shaderTag.ShaderProperties[0].ShaderMaps)
                    {
                        j++;
                        var bitmapName = CacheContext.TagNames.ContainsKey(a.Bitmap.Index) ? CacheContext.TagNames[a.Bitmap.Index] : $"0x{a.Bitmap.Index:X4}";
                        Console.WriteLine($"SetField ShaderProperties[0].ShaderMaps[{j}].Bitmap {bitmapName}.bitm");
                    }
                    Console.WriteLine($"SaveTagChanges");
                    Console.WriteLine($"ExitTo tags");
                }

                Console.WriteLine("");
                Console.WriteLine($"SaveTagNames");
                Console.WriteLine($"Dumplog {modName}.log");
                Console.WriteLine($"Exit");
            }
            else
                throw new NotImplementedException();


            return true;
        }
        
        public bool Temp(List<string> args)
        {
            var tags = CacheContext.TagCache.Index.FindAllInGroup("rmt2");

            foreach (var tag in tags)
            {
                RenderMethodTemplate edRmt2;
                using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, tag);
                    edRmt2 = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(edContext);
                }

                Console.WriteLine($"A:{edRmt2.Arguments.Count:D2} S:{edRmt2.ShaderMaps.Count:D2} 0x{tag.Index:X4} ");
            }

            return true;
        }

        public bool ShadowFix(List<string> args)
        {
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var hlmtInstance = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);

                if (!hlmtInstance.IsInGroup("hlmt"))
                {
                    Console.WriteLine($"ERROR: tag group must be 'hlmt'. Supplied tag group was '{hlmtInstance.Group.Tag}'.");
                    return false;
                }

                var edContext = new TagSerializationContext(cacheStream, CacheContext, hlmtInstance);
                var hlmtDefinition = CacheContext.Deserializer.Deserialize<Model>(edContext);

                hlmtDefinition.CollisionRegions.Add(
                    new Model.CollisionRegion
                    {
                        Permutations = new List<Model.CollisionRegion.Permutation>
                        {
                            new Model.CollisionRegion.Permutation()
                        }
                    });

                edContext = new TagSerializationContext(cacheStream, CacheContext, hlmtInstance);
                CacheContext.Serializer.Serialize(edContext, hlmtDefinition);
                
                edContext = new TagSerializationContext(cacheStream, CacheContext, hlmtDefinition.RenderModel);
                var modeDefinition = CacheContext.Deserializer.Deserialize<RenderModel>(edContext);

                var resourceContext = new ResourceSerializationContext(modeDefinition.Geometry.Resource);
                var geometryResource = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext);

                geometryResource.IndexBuffers.Add(new D3DPointer<IndexBufferDefinition>
                {
                    Address = 0,
                    UnusedC = 0,
                    Definition = new IndexBufferDefinition
                    {
                        Format = IndexBufferFormat.TriangleStrip,
                        Data = new TagData
                        {
                            Size = 0x6,
                            Address = geometryResource.IndexBuffers[0].Definition.Data.Address
                        }
                    }
                });

                geometryResource.VertexBuffers.Add(new D3DPointer<VertexBufferDefinition>
                {
                    Definition = new VertexBufferDefinition
                    {
                        Count = 3,
                        VertexSize = 0x38,
                        Data = new TagData
                        {
                            Size = 0xA8,
                            Address = geometryResource.VertexBuffers[0].Definition.Data.Address
                        }
                    }
                });

                geometryResource.VertexBuffers.Add(new D3DPointer<VertexBufferDefinition>
                {
                    Definition = new VertexBufferDefinition
                    {
                        Count = 3,
                        VertexSize = 0x38,
                        Data = new TagData
                        {
                            Size = 0xA8,
                            Address = geometryResource.VertexBuffers[1].Definition.Data.Address
                        }
                    }
                });

                CacheContext.Serializer.Serialize(resourceContext, geometryResource);

                modeDefinition.Geometry.Meshes.Add(new Mesh
                {
                    VertexBuffers = new ushort[] { (ushort)(geometryResource.VertexBuffers.Count - 2), 0xFFFF, 0xFFFF, (ushort)(geometryResource.VertexBuffers.Count - 1), 0xFFFF, 0xFFFF, 0xFFFF, 0xFFFF },
                    IndexBuffers = new ushort[] { (ushort)(geometryResource.IndexBuffers.Count - 1), 0xFFFF },
                    Type = VertexType.Rigid,
                    PrtType = PrtType.Ambient,
                    IndexBufferType = PrimitiveType.TriangleStrip,
                    RigidNodeIndex = 0,
                    Parts = new List<Mesh.Part>
                    {
                        new Mesh.Part
                        {
                            TransparentSortingIndex = -1,
                            SubPartCount = 1,
                            Type = Mesh.Part.PartType.OpaqueShadowCasting,
                            Flags = Mesh.Part.PartFlags.PerVertexLightmapPart,
                            VertexCount = 3
                        },
                    },
                    SubParts = new List<Mesh.SubPart>
                    {
                        new Mesh.SubPart
                        {
                            FirstIndex = 0,
                            IndexCount = 3,
                            PartIndex = 0,
                            VertexCount = 0
                        }
                    }
                });

                modeDefinition.Regions.Add(
                    new RenderModel.Region
                    {
                        Permutations = new List<RenderModel.Region.Permutation>
                        {
                            new RenderModel.Region.Permutation
                            {
                                MeshIndex = (short)(modeDefinition.Geometry.Meshes.Count - 1)
                            }
                        }
                    });

                edContext = new TagSerializationContext(cacheStream, CacheContext, hlmtDefinition.RenderModel);
                CacheContext.Serializer.Serialize(edContext, modeDefinition);
            }

            return true;
        }
        
        
        public bool CompareTags(List<string> args)
        {
            // When comparing between different caches, compare by name or tag index
            // test compare "D:\Halo\Halo3\maps\tags.dat" levels\solo\005_intro\005_intro.scenario levels\solo\005_intro\005_intro.scenario

            // Get the tag with the same name from both caches
            // test compare "D:\Halo\Halo3\maps\tags.dat" levels\solo\005_intro\005_intro.scenario

            // Compare in the same cache between 2 named tags, or tag indexes
            // test compare levels\solo\005_intro\005_intro.scenario levels\solo\005_intro\005_intro_scripts.scenario

            debugConsoleWrite = false;
            var dumpMatch = false;

            if (args.Count < 2)
                return false;

            csvQueue1 = new List<string>();

            GameCacheContext CacheContext2 = null;

            if (args[0].Contains(".dat"))
            {
                CacheContext2 = new GameCacheContext(new FileInfo(args[0]).Directory);
                args.RemoveAt(0);
            }
            else
                CacheContext2 = CacheContext;

            var tag1 = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);
            if (tag1 == null)
            {
                Console.WriteLine($"ERROR: tag cannot be found in this cache: {args[0]}");
                return false;
            }

            CachedTagInstance tag2 = null;

            if (args.Count == 1)
            {
                tag2 = ArgumentParser.ParseTagSpecifier(CacheContext2, args[0]);
            }
            else
            {
                args.RemoveAt(0);
                tag2 = ArgumentParser.ParseTagSpecifier(CacheContext2, args[0]);
            }

            if (tag2 == null)
            {
                Console.WriteLine($"ERROR: tag cannot be found in the second cache: {args[0]}");
                return false;
            }

            object def1 = null;

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                def1 = CacheContext.Deserializer.Deserialize(new TagSerializationContext(cacheStream, CacheContext, tag1), TagDefinition.Find(tag1.Group.Tag));

            object def2 = null;

            using (var cacheStream = CacheContext2.OpenTagCacheReadWrite())
                def2 = CacheContext2.Deserializer.Deserialize(new TagSerializationContext(cacheStream, CacheContext2, tag2), TagDefinition.Find(tag2.Group.Tag));

            CompareBlocks(def1, def2, CacheContext, CacheContext2, "");

            if (csvQueue1.Count == 0)
            {
                Console.WriteLine($"No changes found.");
                return true;
            }
            else
            {
                Console.WriteLine($"Found differences.");
            }

            var tagname1 = CacheContext.TagNames.ContainsKey(tag1.Index) ? CacheContext.TagNames[tag1.Index] : $"0x{tag1.Index:X4}";
            var tagname2 = CacheContext2.TagNames.ContainsKey(tag2.Index) ? CacheContext2.TagNames[tag2.Index] : $"0x{tag2.Index:X4}";
            var filename1 = tagname1.Split("\\".ToCharArray()).Last();
            var filename2 = tagname2.Split("\\".ToCharArray()).Last();

            CsvDumpQueueToFile(csvQueue1, $"{tag1.Group}_{filename1}_diff.csv");
            if (dumpMatch)
                CsvDumpQueueToFile(csvQueue2, $"{tag1.Group}_{filename1}_match.csv");

            return true;
        }

        public static void CompareBlocks(object leftData, object rightData, GameCacheContext CacheContext, GameCacheContext CacheContext2, String name)
        {
            var dumpMatch = false;

            if (leftData == null || rightData == null)
                return;

            if (name.Contains("ResourcePageIndex"))
                return;

            var type = leftData.GetType();

            if (type == typeof(CachedTagInstance))
            {
                // If the objects are tags, then we've found a match
                var leftTag = (CachedTagInstance)leftData;
                var rightTag = (CachedTagInstance)rightData;

                var leftName = CacheContext.TagNames.ContainsKey(leftTag.Index) ? CacheContext.TagNames[leftTag.Index] : "";
                var rightName = CacheContext2.TagNames.ContainsKey(rightTag.Index) ? CacheContext2.TagNames[rightTag.Index] : "";

                if (leftName != rightName)
                {
                    Csv1($"{name,-120},{leftName},{rightName}");
                    return;
                }
                else
                    Csv2($"{name,-120},{leftName,-60},{rightName}");

                if (leftTag.Group.Tag != rightTag.Group.Tag)
                    Csv1($"{name,-120},{leftName}.{leftTag.Group.Tag,-20},{rightName}.{rightTag.Group.Tag}");
                else
                    Csv2($"{name,-120},{leftName}.{leftTag.Group.Tag,-60},{rightName}.{rightTag.Group.Tag}");
            }
            else if (type.IsArray)
            {
                if (type.GetElementType().IsPrimitive)
                {
                    switch (Type.GetTypeCode(type))
                    {
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                            break;

                        default:
                            break;
                    }

                    return;
                }

                // If the objects are arrays, then loop through each element
                var leftArray = (Array)leftData;
                var rightArray = (Array)rightData;

                if (leftArray.Length != rightArray.Length)
                {
                    Csv1($"{name,-120},{leftArray.Length,-20},{rightArray.Length}");
                    return;
                }
                else
                    Csv2($"{name,-120},{leftArray.Length,-60},{rightArray.Length}");

                for (var i = 0; i < leftArray.Length; i++)
                    CompareBlocks(leftArray.GetValue(i), rightArray.GetValue(i), CacheContext, CacheContext2, name);
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                if (type.GenericTypeArguments[0].IsPrimitive)
                {
                    Csv1($"{name,-120} : type.GenericTypeArguments().IsPrimitive");
                    return;
                }

                // If the objects are lists, then loop through each element
                var countProperty = type.GetProperty("Count");
                var leftCount = (int)countProperty.GetValue(leftData);
                var rightCount = (int)countProperty.GetValue(rightData);
                if (leftCount != rightCount) // If the sizes are different, we probably can't compare them
                {
                    Csv1($"{name,-120},{leftCount,-20},{rightCount}");
                    return;
                }
                else if (dumpMatch)
                    Csv2($"{name,-120},{leftCount,-60},{rightCount}");

                var getItem = type.GetMethod("get_Item");
                for (var i = 0; i < leftCount; i++)
                {
                    var leftItem = getItem.Invoke(leftData, new object[] { i });
                    var rightItem = getItem.Invoke(rightData, new object[] { i });
                    CompareBlocks(leftItem, rightItem, CacheContext, CacheContext2, $"{name}[{i}].");
                }
            }
            else if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
            {
                // The objects are structures
                var left = new TagFieldEnumerator(new TagStructureInfo(leftData.GetType(), CacheVersion.HaloOnline106708));
                var right = new TagFieldEnumerator(new TagStructureInfo(rightData.GetType(), CacheVersion.HaloOnline106708));
                while (left.Next() && right.Next())
                {
                    // Keep going on the left until the field is on the right
                    while (!CacheVersionDetection.IsBetween(CacheVersion.HaloOnline106708, left.Attribute.MinVersion, left.Attribute.MaxVersion))
                    {
                        if (!left.Next())
                            return; // probably unused
                    }

                    // Keep going on the right until the field is on the left
                    while (!CacheVersionDetection.IsBetween(CacheVersion.HaloOnline106708, right.Attribute.MinVersion, right.Attribute.MaxVersion))
                    {
                        if (!right.Next())
                            return;
                    }
                    if (left.Field.MetadataToken != right.Field.MetadataToken)
                        throw new InvalidOperationException("WTF, left and right fields don't match!");

                    // Process the fields
                    var leftFieldData = left.Field.GetValue(leftData);
                    var rightFieldData = right.Field.GetValue(rightData);
                    CompareBlocks(leftFieldData, rightFieldData, CacheContext, CacheContext2, $"{name}{left.Field.Name}");
                }
            }
            else if (type.IsEnum)
            {
                var a = leftData.ToString();
                var b = rightData.ToString();
                if (a != b)
                    Csv1($"{name,-120},{leftData,-20},{rightData}");
                else if (dumpMatch)
                    Csv2($"{name,-120},{leftData,-60},{rightData}");
            }
            else if (type.IsPrimitive)
            {
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.SByte:
                        if ((sbyte)leftData != (sbyte)rightData)
                            Csv1($"{name,-120},{(sbyte)leftData,-20},{(sbyte)rightData}");
                        else if (dumpMatch)
                            Csv2($"{name,-120},{(sbyte)leftData,-60},{(sbyte)rightData}");
                        break;
                    case TypeCode.Byte:
                        if ((byte)leftData != (byte)rightData)
                            Csv1($"{name,-120},{(byte)leftData,-20},{(byte)rightData}");
                        else if (dumpMatch)
                            Csv2($"{name,-120},{(byte)leftData,-60},{(byte)rightData}");
                        break;
                    case TypeCode.Int16:
                        if ((short)leftData != (short)rightData)
                            Csv1($"{name,-120},{(short)leftData,-20},{(short)rightData}");
                        else if (dumpMatch)
                            Csv2($"{name,-120},{(short)leftData,-60},{(short)rightData}");
                        break;
                    case TypeCode.UInt16:
                        if ((ushort)leftData != (ushort)rightData)
                            Csv1($"{name,-120},{(ushort)leftData,-20},{(ushort)rightData}");
                        else if (dumpMatch)
                            Csv2($"{name,-120},{(ushort)leftData,-60},{(ushort)rightData}");
                        break;
                    case TypeCode.Int32:
                        if ((int)leftData != (int)rightData)
                            Csv1($"{name,-120},{(int)leftData,-20},{(int)rightData}");
                        else if (dumpMatch)
                            Csv2($"{name,-120},{(int)leftData,-60},{(int)rightData}");
                        break;
                    case TypeCode.UInt32:
                        if ((uint)leftData != (uint)rightData)
                            Csv1($"{name,-120},{(uint)leftData,-20},{(uint)rightData}");
                        else if (dumpMatch)
                            Csv2($"{name,-120},{(uint)leftData,-60},{(uint)rightData}");
                        break;
                    case TypeCode.Single:
                        if ((float)leftData != (float)rightData)
                            Csv1($"{name,-120},{(float)leftData,-20},{(float)rightData}");
                        else if (dumpMatch)
                            Csv2($"{name,-120},{(float)leftData,-60},{(float)rightData}");
                        break;

                    default:
                        break;
                }
            }
        }
        
        public bool NameRmt2()
        {
            var validShaders = new List<string> { "rmsh", "rmtr", "rmhg", "rmfl", "rmcs", "rmss", "rmd ", "rmw ", "rmzo", "ltvl", "prt3", "beam", "decs", "cntl", "rmzo", "rmct", "rmbk" };
            var newlyNamedRmt2 = new List<int>();
            var type = "invalid";
            var rmt2Instance = -1;

            // Unname rmt2 tags
            foreach (var edInstance in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
                CacheContext.TagNames[edInstance.Index] = "blank";

            foreach (var edInstance in CacheContext.TagCache.Index.NonNull())
            {
                object rm = null;
                RenderMethod renderMethod = null;

                // ignore tag groups not in validShaders
                if (!validShaders.Contains(edInstance.Group.Tag.ToString()))
                    continue;

                // Console.WriteLine($"Checking 0x{edInstance:x4} {edInstance.Group.Tag.ToString()}");

                // Get the tagname type per tag group
                switch (edInstance.Group.Tag.ToString())
                {
                    case "rmsh": type = "shader"; break;
                    case "rmtr": type = "terrain"; break;
                    case "rmhg": type = "halogram"; break;
                    case "rmfl": type = "foliage"; break;
                    case "rmss": type = "screen"; break;
                    case "rmcs": type = "custom"; break;
                    case "prt3": type = "particle"; break;
                    case "beam": type = "beam"; break;
                    case "cntl": type = "contrail"; break;
                    case "decs": type = "decal"; break;
                    case "ltvl": type = "light_volume"; break;
                    case "rmct": type = "cortana"; break;
                    case "rmbk": type = "black"; break;
                    case "rmzo": type = "zonly"; break;
                    case "rmd ": type = "decal"; break;
                    case "rmw ": type = "water"; break;
                }

                switch (edInstance.Group.Tag.ToString())
                {
                    case "rmsh":
                    case "rmhg":
                    case "rmtr":
                    case "rmcs":
                    case "rmfl":
                    case "rmss":
                    case "rmct":
                    case "rmzo":
                    case "rmbk":
                    case "rmd ":
                    case "rmw ":
                        using (var cacheStream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                        {
                            var edContext = new TagSerializationContext(cacheStream, CacheContext, edInstance);
                            var rm2 = CacheContext.Deserializer.Deserialize<RenderMethodFast>(edContext);
                            renderMethod = new RenderMethod();
                            renderMethod.Unknown = rm2.Unknown;
                            if (renderMethod.Unknown.Count == 0)
                                continue;
                        }

                        foreach (var a in edInstance.Dependencies)
                            if (CacheContext.GetTag(a).Group.ToString() == "rmt2")
                                rmt2Instance = CacheContext.GetTag(a).Index;

                        if (rmt2Instance == 0)
                            throw new Exception();

                        NameRmt2Part(type, renderMethod, edInstance, rmt2Instance, newlyNamedRmt2);

                        continue;

                    default:
                        break;
                }

                using (var cacheStream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, edInstance);
                    rm = CacheContext.Deserializer.Deserialize(new TagSerializationContext(cacheStream, CacheContext, edInstance), TagDefinition.Find(edInstance.Group.Tag));
                }

                switch (edInstance.Group.Tag.ToString())
                {
                    case "prt3": var e = (Particle)rm; NameRmt2Part(type, e.RenderMethod, edInstance, e.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "beam": var a = (BeamSystem)rm; foreach (var f in a.Beam) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "cntl": var b = (ContrailSystem)rm; foreach (var f in b.Contrail) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "decs": var c = (DecalSystem)rm; foreach (var f in c.DecalSystem2) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "ltvl": var d = (LightVolumeSystem)rm; foreach (var f in d.LightVolume) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;

                    default:
                        break;
                }
            }


            return true;
        }


        private void NameRmt2Part(string type, RenderMethod renderMethod, CachedTagInstance edInstance, int rmt2Instance, List<int> newlyNamedRmt2)
        {
            if (renderMethod.Unknown.Count == 0) // invalid shaders, most likely caused by ported shaders
                return;

            if (newlyNamedRmt2.Contains(rmt2Instance))
                return;
            else
                newlyNamedRmt2.Add(rmt2Instance);

            var newTagName = $"shaders\\{type}_templates\\";

            var rmdfRefValues = "";

            for (int i = 0; i < renderMethod.Unknown.Count; i++)
            {
                if (edInstance.Group.Tag.ToString() == "rmsh" && i > 9) // for better H3/ODST name matching
                    break;

                if (edInstance.Group.Tag.ToString() == "rmhg" && i > 6) // for better H3/ODST name matching
                    break;

                rmdfRefValues = $"{rmdfRefValues}_{renderMethod.Unknown[i].Unknown}";
            }

            newTagName = $"{newTagName}{rmdfRefValues}";

            CacheContext.TagNames[rmt2Instance] = newTagName;
            // Console.WriteLine($"0x{rmt2Instance:X4} {newTagName}");
        }

        [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x20)]
        public class RenderMethodFast
        {
            public CachedTagInstance BaseRenderMethod;
            public List<RenderMethod.UnknownBlock> Unknown;
        }
    }
}