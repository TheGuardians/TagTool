using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Legacy.Base;
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
                { "shadowfixtest", "Hack/fix a weapon or forge object's shadow mehs." }
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
                case "shadowfixtest": return ShadowFixTest(args);
                default:
                    Console.WriteLine($"Invalid command: {name}");
                    Console.WriteLine($"Available commands: {commandsList.Count}");
                    foreach (var a in commandsList)
                        Console.WriteLine($"{a.Key}: {a.Value}");
                    return false;
            }
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
                    case CacheVersion.Halo3Retail:
                        BlamCache = new TagTool.Legacy.Halo3Retail.CacheFile(blamCacheFile, version);
                        break;

                    case CacheVersion.Halo3ODST:
                        BlamCache = new TagTool.Legacy.Halo3ODST.CacheFile(blamCacheFile, version);
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
        
        public bool ShadowFixTest(List<string> args)
        {
            var instance1 = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);

            Model tag1;

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, instance1);
                tag1 = CacheContext.Deserializer.Deserialize<Model>(edContext);
            }

            tag1.CollisionRegions.Add(new Model.CollisionRegion { Permutations = new List<Model.CollisionRegion.Permutation> { new Model.CollisionRegion.Permutation() } });

            Console.WriteLine("Serializing...");
            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context1 = new TagSerializationContext(stream, CacheContext, instance1);
                CacheContext.Serializer.Serialize(context1, tag1);
            }

            RenderModel tag2;

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, tag1.RenderModel);
                tag2 = CacheContext.Deserializer.Deserialize<RenderModel>(edContext);
            }

            var resourceContext2 = new ResourceSerializationContext(tag2.Geometry.Resource);

            var def2 = CacheContext.Deserializer.Deserialize<RenderGeometryApiResourceDefinition>(resourceContext2);

            def2.IndexBuffers.Add(new D3DPointer<IndexBufferDefinition>
            {
                Address = 0,
                UnusedC = 0,
                Definition = new IndexBufferDefinition
                {
                    Format = IndexBufferFormat.TriangleStrip,
                    Data = new TagData
                    {
                        Size = 0x6,
                        Address = def2.IndexBuffers[0].Definition.Data.Address
                    }
                }
            });

            def2.VertexBuffers.Add(new D3DPointer<VertexBufferDefinition>
            {
                Definition = new VertexBufferDefinition
                {
                    Count = 3,
                    VertexSize = 0x38,
                    Data = new TagData
                    {
                        Size = 0xA8,
                        Address = def2.VertexBuffers[0].Definition.Data.Address
                    }
                }
            });

            def2.VertexBuffers.Add(new D3DPointer<VertexBufferDefinition>
            {
                Definition = new VertexBufferDefinition
                {
                    Count = 3,
                    VertexSize = 0x38,
                    Data = new TagData
                    {
                        Size = 0xA8,
                        Address = def2.VertexBuffers[1].Definition.Data.Address
                    }
                }
            });

            var context = new ResourceSerializationContext(tag2.Geometry.Resource);
            CacheContext.Serializer.Serialize(context, def2);

            tag2.Geometry.Meshes.Add(new Mesh
            {
                VertexBuffers = new ushort[] { (ushort)(def2.VertexBuffers.Count - 2), 0xFFFF, 0xFFFF, (ushort)(def2.VertexBuffers.Count - 1), 0xFFFF, 0xFFFF, 0xFFFF, 0xFFFF },
                IndexBuffers = new ushort[] { (ushort)(def2.IndexBuffers.Count - 1), 0xFFFF },
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
                        Type = 2,
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
                        VertexCount = 0,
                    }
                }
            });

            tag2.Regions.Add(new RenderModel.Region { Permutations = new List<RenderModel.Region.Permutation> { new RenderModel.Region.Permutation { MeshIndex = (short)(tag2.Geometry.Meshes.Count - 1) } } });

            // def2.IndexBuffers.Add(def1.IndexBuffers[0]);
            // def2.IndexBuffers.Last().Definition.Data.Address = def2.IndexBuffers[0].Definition.Data.Address;

            // def2.VertexBuffers.Add(def1.VertexBuffers[0]);
            // def2.VertexBuffers[2].Definition.Data.Address = def2.VertexBuffers[0].Definition.Data.Address;
            // def2.VertexBuffers.Add(def1.VertexBuffers[1]);
            // def2.VertexBuffers[3].Definition.Data.Address = def2.VertexBuffers[1].Definition.Data.Address;


            Console.WriteLine("Serializing...");
            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context2 = new TagSerializationContext(stream, CacheContext, tag1.RenderModel);
                CacheContext.Serializer.Serialize(context2, tag2);
            }

            Console.WriteLine("Done.");

            return true;
        }

    }
}