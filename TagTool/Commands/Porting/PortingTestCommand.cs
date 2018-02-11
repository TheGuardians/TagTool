using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.IO;
using TagTool.Legacy.Base;
using TagTool.Serialization;
using TagTool.TagDefinitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace TagTool.Commands.Porting
{
    public class PortingTestCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }

        public PortingTestCommand(GameCacheContext cacheContext, CacheFile blamCache) :
            base(CommandFlags.Inherit,

                "PortingTest",
                "A test command for porting-related actions.",

                "PortingTest [...]",

                "A test command. Used for various testing and temporary functionality.\n" +
                "Example: 'PortingTest UpdateMapFiles'")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count == 0)
                return false;

            var name = args[0].ToLower();
            args.RemoveAt(0);

            var commandsList = new Dictionary<string, string>
            {
                { "restoreweaponmodel"      , "Port and replace a weapon's fp and 3rd person model." },
                { "restoreweaponanimation"  , "Port and replace a weapon's fp animation." },
                { "restoreweaponfiringsound", "Port and replace a weapon's firing sound." },
                { "deserializeblam"         , "Deserialize a Blam tag and list all fields (edittag's listfields)" },
                { "createtag"               , "Creates a new tag of the specified tag group in the current tag cache." },
                { "updatemapfiles"          , "Updates the game's .map files to contain valid scenario tag indices." },
                { "listtags"                , "List all tags of a specified tag group." },
                { "listblamtags"            , "List all Blam tags of a specified tag group." },
                { "loopallblamtags"         , "Loop trough all tags. Source code modification required." },
                { "adjustscripts"           , "Adjust scripts based on a hardcoded setup. Only c100, c200, h100, l200, l300, sc140 are supported currently." },
                { "fixcisc"                 , "Double the framerate in cisc camera animation." },
            };

            switch (name)
            {
                case "restoreweaponmodel": return RestoreWeaponModel(args);
                case "restoreweaponanimation": return RestoreWeaponAnimation(args);
                case "restoreweaponfiringsound": return RestoreWeaponFiringSound(args);
                case "deserializeblam": return DeserializeBlam(args);
                case "createtag": return CreateTag(args);
                case "updatemapfiles": return UpdateMapFiles(args);
                case "listtags": return ListTags(args);
                case "listblamtags": return ListBlamTags(args);
                case "loopallblamtags": return LoopAllBlamTags(args);
                case "adjustscripts": return AdjustScripts(args);
                case "fixcisc": return FixCisc(args);
                default:
                    Console.WriteLine($"Invalid command: {name}");
                    Console.WriteLine($"Available commands: {commandsList.Count}");
                    foreach (var a in commandsList)
                        Console.WriteLine($"{a.Key}: {a.Value}");
                    return false;
            }
        }

        public bool ListTags(List<string> args)
        {
            var tags = CacheContext.TagCache.Index.NonNull().ToArray();

            List<string> groupsList = new List<string>();
            string matchName = "";

            if (args.Count > 2)
            {
                matchName = args.Last();
                args.RemoveAt(args.Count - 1);

                foreach (var a in args)
                    groupsList.Add(a);
            }

            foreach (var tag in tags)
            {
                var name = CacheContext.TagNames.ContainsKey(tag.Index) ? CacheContext.TagNames[tag.Index] : $"0x{tag.Index:X4}";

                if (args.Count == 2)
                    if (tag.Group.ToString() == args[0] && name.Contains(args[1]))
                        Console.WriteLine($"0x{tag.Index:X4},{name}.{tag.Group}");

                if (args.Count == 1)
                {
                    if (tag.Group.ToString() == args[0])
                        Console.WriteLine($"0x{tag.Index:X4},{name}.{tag.Group}");
                    if (args[0].Length > 4 && name.Contains(args[0]))
                        Console.WriteLine($"0x{tag.Index:X4},{name}.{tag.Group}");
                }

                if (args.Count == 0)
                    Console.WriteLine($"0x{tag.Index:X4},{name}.{tag.Group}");

                if (args.Count > 2)
                {
                    if (groupsList.Contains(tag.Group.ToString()) && name.Contains(matchName))
                        Console.WriteLine($"0x{tag.Index:X4},{name}.{tag.Group}");
                }
            }

            return true;
        }

        public bool DeserializeBlam(List<string> args)
        {
            if (args.Count != 2)
            {
                Console.WriteLine($"ERROR: usage: test DeserializeBlam matg globals\\globals");
                return false;
            }

            CacheFile.IndexItem blamInstance = null;

            var blamDeserializer = new TagDeserializer(BlamCache.Version);

            foreach (var item in BlamCache.IndexItems)
            {
                if (item.ClassCode == args[0] && item.Filename == args[1])
                {
                    blamInstance = item;
                    break;
                }
            }

            var blamContext = new CacheSerializationContext(CacheContext, BlamCache, blamInstance);

            var Value = blamDeserializer.Deserialize(blamContext, TagDefinition.Find(args[0]));

            var Structure = new TagStructureInfo(TagDefinition.Find(blamInstance.ClassCode));

            var match = false;
            var token = "";

            var enumerator = new TagFieldEnumerator(Structure);

            while (enumerator.Next())
            {
                if (enumerator.Attribute != null && enumerator.Attribute.Padding == true)
                    continue;

                var nameString = enumerator.Field.Name;

                if (match && !nameString.ToLower().Contains(token))
                    continue;

                var fieldType = enumerator.Field.FieldType;
                var fieldValue = enumerator.Field.GetValue(Value);

                var typeString =
                    fieldType.IsGenericType ?
                        $"{fieldType.Name}<{fieldType.GenericTypeArguments[0].Name}>" :
                    fieldType.Name;

                string valueString = "";

                try
                {
                    if (fieldValue == null)
                        valueString = "null";
                    else if (fieldType.GetInterface(typeof(IList).Name) != null)
                        valueString =
                            ((IList)fieldValue).Count != 0 ?
                                $"{{...}}[{((IList)fieldValue).Count}]" :
                            "null";
                    else if (fieldType == typeof(StringId))
                    {
                        int.TryParse(fieldValue.ToString().Split("x".ToCharArray())[1], NumberStyles.HexNumber, null, out int r);
                        valueString = BlamCache.Strings.GetItemByID(r);
                    }
                    else if (fieldType == typeof(CachedTagInstance))
                    {
                        var instance = (CachedTagInstance)fieldValue;

                        if ((uint)instance.Index != 0xFFFFFFFF)
                        {
                            var blamInstance2 = BlamCache.IndexItems.Find(x => x.ID == instance.Index);

                            valueString = $"{blamInstance2.Filename}.{blamInstance2.ClassCode}";
                        }
                    }
                    else
                        valueString = fieldValue.ToString();
                }
                catch (Exception e)
                {
                    valueString = $"<ERROR MESSAGE=\"{e.Message}\" />";
                }

                var fieldName = $"{enumerator.Field.DeclaringType.FullName}.{enumerator.Field.Name}".Replace("+", ".");

                Console.WriteLine("{0}: {1} = {2}", nameString, typeString, valueString);
            }
            
            return true;
        }

        public bool CreateTag(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;

            begin:
            var groupTagString = args[0];

            if (groupTagString.Length > 4)
            {
                Console.WriteLine($"ERROR: Invalid group tag: {groupTagString}");
                return true;
            }

            var groupTag = ArgumentParser.ParseGroupTag(CacheContext.StringIdCache, args[0]);

            if (groupTag == Tag.Null)
            {
                var chars = new char[] { ' ', ' ', ' ', ' ' };

                for (var i = 0; i < chars.Length; i++)
                    chars[i] = groupTagString[i];

                groupTag = new Tag(new string(chars));
            }

            if (!TagGroup.Instances.ContainsKey(groupTag))
            {
                Console.WriteLine($"ERROR: No tag group definition for group tag '{groupTag}'!");
                Console.Write($"(BE CAREFUL WITH THIS!!!) Define '{groupTag}' tag group? [y/n]: ");

                var answer = Console.ReadLine().ToLower();

                if (answer != "y" && answer != "yes")
                    return true;

                Console.WriteLine("Enter the tag group specification in the following format");
                Console.WriteLine("<group tag> [parent group tag] [grandparent group tag] <group name>:");
                Console.WriteLine();
                Console.Write($"{groupTag} specification> ");

                answer = Console.ReadLine();

                var groupArgs = ArgumentParser.ParseCommand(answer, out string redirect);

                switch (groupArgs.Count)
                {
                    case 2: new TagGroup(new Tag(groupArgs[0]), Tag.Null, Tag.Null, CacheContext.GetStringId(groupArgs[1])); break;
                    case 3: new TagGroup(new Tag(groupArgs[0]), new Tag(groupArgs[1]), Tag.Null, CacheContext.GetStringId(groupArgs[2])); break;
                    case 4: new TagGroup(new Tag(groupArgs[0]), new Tag(groupArgs[1]), new Tag(groupArgs[2]), CacheContext.GetStringId(groupArgs[3])); break;
                    default: return false;
                }

                goto begin;
            }

            CachedTagInstance instance = null;

            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                if (args.Count == 2)
                {
                    var tag = ArgumentParser.ParseTagSpecifier(CacheContext, args[1]);

                    if (tag == null)
                        return false;

                    var tagIndex = tag.Index;

                    if (tagIndex > CacheContext.TagCache.Index.Count)
                        return false;

                    if (tagIndex < CacheContext.TagCache.Index.Count)
                    {
                        if (CacheContext.TagCache.Index[tagIndex] != null)
                        {
                            var oldInstance = CacheContext.TagCache.Index[tagIndex];
                            CacheContext.TagCache.Index[tagIndex] = null;
                            CacheContext.TagCache.SetTagDataRaw(stream, oldInstance, new byte[] { });
                        }

                        instance = new CachedTagInstance(tagIndex, TagGroup.Instances[groupTag]);
                        CacheContext.TagCache.Index[tagIndex] = instance;
                    }
                }

                if (instance == null)
                    instance = CacheContext.TagCache.AllocateTag(TagGroup.Instances[groupTag]);

                var context = new TagSerializationContext(stream, CacheContext, instance);

                var data = Activator.CreateInstance(TagDefinition.Find(groupTag));
                CacheContext.Serializer.Serialize(context, data);
            }

            var tagName = CacheContext.TagNames.ContainsKey(instance.Index) ?
                CacheContext.TagNames[instance.Index] :
                $"0x{instance.Index:X4}";

            Console.WriteLine($"[Index: 0x{instance.Index:X4}, Offset: 0x{instance.HeaderOffset:X8}, Size: 0x{instance.TotalSize:X4}] {tagName}.{CacheContext.GetString(instance.Group.Name)}");

            return true;
        }

        public bool UpdateMapFiles(List<string> args)
        {
            var mapIndices = new Dictionary<int, int>();

            using (var cacheStream = CacheContext.OpenTagCacheRead())
            {
                foreach (var scnrTag in CacheContext.TagCache.Index.FindAllInGroup("scnr"))
                {
                    var tagContext = new TagSerializationContext(cacheStream, CacheContext, scnrTag);
                    
                    using (var tagStream = new MemoryStream(CacheContext.TagCache.ExtractTagRaw(cacheStream, scnrTag)))
                    using (var reader = new EndianReader(tagStream))
                    {
                        reader.BaseStream.Position = scnrTag.DefinitionOffset + 0x8;
                        var mapId = reader.ReadInt32();

                        mapIndices[mapId] = scnrTag.Index;
                    }
                }
            }

            foreach (var mapFile in CacheContext.Directory.GetFiles("*.map"))
            {
                try
                {
                    using (var stream = mapFile.Open(FileMode.Open, FileAccess.ReadWrite))
                    using (var reader = new BinaryReader(stream))
                    using (var writer = new BinaryWriter(stream))
                    {
                        if (reader.ReadInt32() != new Tag("head").Value)
                        {
                            Console.Error.WriteLine("Invalid map file");
                            return true;
                        }

                        reader.BaseStream.Position = 0x2DEC;
                        var mapId = reader.ReadInt32();

                        if (mapIndices.ContainsKey(mapId))
                        {
                            var mapIndex = mapIndices[mapId];

                            writer.BaseStream.Position = 0x2DF0;
                            writer.Write(mapIndex);

                            Console.WriteLine($"Scenario tag index for {mapFile.Name}: {mapIndex:X8}");
                        }
                    }
                }
                catch (IOException)
                {
                    Console.Error.WriteLine("Unable to open the map file for reading.");
                }
            }

            Console.WriteLine("Done!");

            return true;
        }

        public bool ListBlamTags(List<string> args)
        {
            var tags = BlamCache.IndexItems.ToArray();

            foreach (var tag in tags)
            {
                if (args.Count == 2)
                    if (tag.ClassCode.ToString() == args[0] && tag.Filename.Contains(args[1]))
                        Console.WriteLine($"[{tag.ClassCode}] {tag.Filename}");

                if (args.Count == 1)
                    if (tag.ClassCode.ToString() == args[0])
                        Console.WriteLine($"[{tag.ClassCode}] {tag.Filename}");

                if (args.Count == 0)
                    Console.WriteLine($"[{tag.ClassCode}] {tag.Filename}");
            }

            return true;
        }

        public bool LoopAllBlamTags(List<string> args)
        {
            if (args.Count != 1)
                return false;

            if (args[0].Length != 4)
                return false;

            var blamDeserializer = new TagDeserializer(BlamCache.Version);

            foreach (var item in BlamCache.IndexItems)
            {
                if (item.ClassCode == args[0])
                {
                    var blamContext = new CacheSerializationContext(CacheContext, BlamCache, item);
                    var def = blamDeserializer.Deserialize(blamContext, TagDefinition.Find(args[0]));

                    Console.WriteLine($"Deserialized [{item.ClassCode}] {item.Filename}");

                    var jmad = (ModelAnimationGraph)def;
                }
            }

            return true;
        }

        public bool RestoreWeaponModel(List<string> args)
        {
            if (args.Count != 1)
            {
                Console.WriteLine("Required: [1] weapon name");

                Console.WriteLine(@"Example: test RestoreWeaponModel objects\weapons\rifle\assault_rifle\assault_rifle");

                return false;
            }

            string blamWeapName = args[0];

            var blamDeserializer = new TagDeserializer(BlamCache.Version);

            var blamWeapTag = BlamCache.IndexItems.Find(x => x.Filename == blamWeapName);

            if (blamWeapTag == null)
            {
                Console.WriteLine($"{blamWeapName} does not exist.");
                return false;
            }

            // Check if ED has named tags, or has that weapon.
            if (!CacheContext.TagNames.ContainsValue(blamWeapName))
            {
                Console.WriteLine($"{blamWeapName} does not exist or is named incorrectly or tags are not named.");
                return false;
            }

            CachedTagInstance edWeapTag = null;

            edWeapTag = ArgumentParser.ParseTagSpecifier(CacheContext, $"{blamWeapName}.weap");

            if (edWeapTag == null)
            {
                Console.WriteLine($"{blamWeapName} does not exist or is named incorrectly or tags are not named.");
                return false;
            }

            string blamFPmodeName = null;
            string blam3DmodeName = null;
            string edFPmodeName = null;
            string ed3DmodeName = null;

            // Blam:
            // Deserialize weap tag
            var blamContext = new CacheSerializationContext(CacheContext, BlamCache, blamWeapTag);
            Weapon blamWeap = blamDeserializer.Deserialize<Weapon>(blamContext);

            // Deserialize hlmt tag
            var blamContext2 = new CacheSerializationContext(CacheContext, BlamCache, BlamCache.IndexItems.Find(x => x.ID == blamWeap.Model.Index));
            var blamHlmt = blamDeserializer.Deserialize<Model>(blamContext2);

            // Get blam FP mode name
            blamFPmodeName = BlamCache.IndexItems.Find(x => x.ID == blamWeap.FirstPerson[0].FirstPersonModel.Index).Filename;

            // Get blam 3D mode name
            blam3DmodeName = BlamCache.IndexItems.Find(x => x.ID == blamWeap.Model.Index).Filename;

            // ED:
            // Find weapon
            var edWeapInstance = ArgumentParser.ParseTagSpecifier(CacheContext, $"{blamWeapName}.weap");

            if (edWeapInstance == null)
                throw new Exception($"Failed to find ED weapon {blamWeapName}");

            // Deserialize weap tag
            Weapon edWeap;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edWeapInstance);
                edWeap = CacheContext.Deserializer.Deserialize<Weapon>(edContext);
            }

            // Deserialize hlmt tag
            Model edModel;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edWeap.Model);
                edModel = CacheContext.Deserializer.Deserialize<Model>(edContext);
            }

            // Cheap checks, only convert weapons that have a first person model, and third person model.
            // This will break on turrets, good because they need additional fixes.
            if (blamWeap.FirstPerson.Count == 0)
                throw new Exception($"Unsupported weapon due to missing first person model: {blamWeapName}");

            if (blamWeap.FirstPerson[0].FirstPersonModel == null || (uint)blamWeap.FirstPerson[0].FirstPersonModel.Index == 0xFFFFFFFF)
                throw new Exception($"Unsupported weapon due to missing first person model: {blamWeapName}");

            if (blamWeap.Model == null || (uint)blamWeap.Model.Index == 0xFFFFFFFF)
                throw new Exception($"Unsupported weapon due to missing third person model: {blamWeapName}");

            // Rename FP mode tag
            edFPmodeName = CacheContext.TagNames.ContainsKey(edWeap.FirstPerson[0].FirstPersonModel.Index) ? CacheContext.TagNames[edWeap.FirstPerson[0].FirstPersonModel.Index] : null;
            if (edFPmodeName != null)
                CacheContext.TagNames[edWeap.FirstPerson[0].FirstPersonModel.Index] = $"{CacheContext.TagNames[edWeap.FirstPerson[0].FirstPersonModel.Index]}_HO";

            // Rename 3D mode tag
            ed3DmodeName = CacheContext.TagNames.ContainsKey(edModel.RenderModel.Index) ? CacheContext.TagNames[edModel.RenderModel.Index] : null;
            if (ed3DmodeName != null)
                CacheContext.TagNames[edModel.RenderModel.Index] = $"{CacheContext.TagNames[edModel.RenderModel.Index]}_HO";

            // Port FP mode
            var portTagCommand = new PortTagCommand(CacheContext, BlamCache);
            portTagCommand.Execute(new List<string> { "replace", "mode", blamFPmodeName });
            // RestoreBlamShaderSet(new List<string> { "new", args[0], blamFPmodeName, blamFPmodeName + ".mode" });
            var matchShadersCommand = new MatchShadersCommand(CacheContext, BlamCache);
            matchShadersCommand.Execute(new List<string> { blamFPmodeName + ".mode" });

            // Port 3D mode
            portTagCommand.Execute(new List<string> { "replace", "mode", blam3DmodeName });
            // RestoreBlamShaderSet(new List<string> { args[0], blam3DmodeName, blam3DmodeName + ".mode" });
            matchShadersCommand.Execute(new List<string> { blam3DmodeName + ".mode" });

            // Set new models
            edWeap.FirstPerson[0].FirstPersonModel = ArgumentParser.ParseTagSpecifier(CacheContext, $"{blamFPmodeName}.mode");
            edModel.RenderModel = ArgumentParser.ParseTagSpecifier(CacheContext, $"{blam3DmodeName}.mode");

            if (edWeap.FirstPerson[0].FirstPersonModel == null)
            {
                Console.WriteLine($"Failed to find the ported mode tag: {blamFPmodeName}");
                return false;
            }

            if (edModel.RenderModel == null)
            {
                Console.WriteLine($"Failed to find the ported mode tag: {blamWeapName}");
                return false;
            }

            edModel.LodModel = null;

            // Serialize ED weap tag
            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edWeapInstance);
                CacheContext.Serializer.Serialize(context, edWeap);
            }

            // Serialize ED hlmt tag
            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edWeap.Model);
                CacheContext.Serializer.Serialize(context, edModel);
            }

            // DEBUG: Test
            var edFPmodeTag = edWeap.FirstPerson[0].FirstPersonModel;
            var ed3DmodeTag = edModel.RenderModel;

            edFPmodeName = CacheContext.TagNames.ContainsKey(edFPmodeTag.Index) ? CacheContext.TagNames[edFPmodeTag.Index] : null;
            ed3DmodeName = CacheContext.TagNames.ContainsKey(ed3DmodeTag.Index) ? CacheContext.TagNames[ed3DmodeTag.Index] : null;

            Console.WriteLine($"DEBUG: Test: FP: [{edFPmodeTag.Group}] 0x{edFPmodeTag.Index:X4} {edFPmodeName}");
            Console.WriteLine($"DEBUG: Test: 3D: [{ed3DmodeTag.Group}] 0x{ed3DmodeTag.Index:X4} {ed3DmodeName}");

            return true;
        }

        public bool RestoreWeaponAnimation(List<string> args)
        {
            if (args.Count != 1)
            {
                Console.WriteLine("Required:[1] weapon name (port only FP animation)");

                Console.WriteLine(@"Example: test RestoreWeaponAnimation D:\Halo\Cache\Halo3\sandbox.map objects\weapons\rifle\assault_rifle\assault_rifle");

                return false;
            }

            string blamWeapName = args[0];

            var blamDeserializer = new TagDeserializer(BlamCache.Version);

            var blamWeapTag = BlamCache.IndexItems.Find(x => x.Filename == blamWeapName);

            if (blamWeapTag == null)
            {
                Console.WriteLine($"{blamWeapName} does not exist.");
                return false;
            }

            // Check if ED has named tags, or has that weapon.
            if (!CacheContext.TagNames.ContainsValue(blamWeapName))
            {
                Console.WriteLine($"{blamWeapName} does not exist or is named incorrectly or tags are not named.");
                return false;
            }

            CachedTagInstance edWeapTag = null;

            edWeapTag = ArgumentParser.ParseTagSpecifier(CacheContext, $"{blamWeapName}.weap");

            if (edWeapTag == null)
            {
                Console.WriteLine($"{blamWeapName} does not exist or is named incorrectly or tags are not named.");
                return false;
            }

            string blamFPmodeName = null;
            string edFPmodeName = null;

            // Blam:
            // Deserialize weap tag
            var blamContext = new CacheSerializationContext(CacheContext, BlamCache, blamWeapTag);
            Weapon blamWeap = blamDeserializer.Deserialize<Weapon>(blamContext);

            // Get blam FP mode name
            blamFPmodeName = BlamCache.IndexItems.Find(x => x.ID == blamWeap.FirstPerson[0].FirstPersonAnimations.Index).Filename;

            // ED:
            // Find weapon
            var edWeapInstance = ArgumentParser.ParseTagSpecifier(CacheContext, $"{blamWeapName}.weap");

            if (edWeapInstance == null)
                throw new Exception($"Failed to find ED weapon {blamWeapName}");

            // Deserialize weap tag
            Weapon edWeap;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edWeapInstance);
                edWeap = CacheContext.Deserializer.Deserialize<Weapon>(edContext);
            }

            // This will break on turrets, good because they need additional fixes. Weapons that do not require animations port:
            // shotgun, smg, sniper, hammer, plasma pistol, spartan laser (maybe), fuel rod
            if (blamWeap.FirstPerson.Count == 0)
                throw new Exception($"Unsupported weapon due to missing first person animations: {blamWeapName}");

            if (blamWeap.FirstPerson[0].FirstPersonAnimations == null || (uint)blamWeap.FirstPerson[0].FirstPersonAnimations.Index == 0xFFFFFFFF)
                throw new Exception($"Unsupported weapon due to missing first person animations: {blamWeapName}");

            // Rename FP mode tag
            edFPmodeName = CacheContext.TagNames.ContainsKey(edWeap.FirstPerson[0].FirstPersonAnimations.Index) ? CacheContext.TagNames[edWeap.FirstPerson[0].FirstPersonAnimations.Index] : null;
            if (edFPmodeName != null)
                CacheContext.TagNames[edWeap.FirstPerson[0].FirstPersonAnimations.Index] = $"{CacheContext.TagNames[edWeap.FirstPerson[0].FirstPersonAnimations.Index]}_HO";

            // Port FP mode
            var portTagCommand = new PortTagCommand(CacheContext, BlamCache);
            portTagCommand.Execute(new List<string> { "replace", "jmad", blamFPmodeName });

            // Set new models
            edWeap.FirstPerson[0].FirstPersonAnimations = ArgumentParser.ParseTagSpecifier(CacheContext, $"{blamFPmodeName}.jmad");

            if (edWeap.FirstPerson[0].FirstPersonAnimations == null)
            {
                Console.WriteLine($"Failed to find the ported jmad tag: {blamFPmodeName}");
                return false;
            }

            // Serialize ED weap tag
            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edWeapInstance);
                CacheContext.Serializer.Serialize(context, edWeap);
            }

            // DEBUG: Test
            var edFPmodeTag = edWeap.FirstPerson[0].FirstPersonAnimations;

            edFPmodeName = CacheContext.TagNames.ContainsKey(edFPmodeTag.Index) ? CacheContext.TagNames[edFPmodeTag.Index] : null;

            Console.WriteLine($"DEBUG: Test: jmad: [{edFPmodeTag.Group}] 0x{edFPmodeTag.Index:X4} {edFPmodeName}");

            return true;
        }

        public bool RestoreWeaponFiringSound(List<string> args)
        {
            if (args.Count != 1)
            {
                Console.WriteLine("Required: [0] weapon name");

                Console.WriteLine(@"Example: test RestoreWeaponFiringSound D:\Halo\Cache\Halo3\sandbox.map objects\weapons\rifle\assault_rifle\assault_rifle");

                return false;
            }

            string blamWeapName = args[1];

            var blamDeserializer = new TagDeserializer(BlamCache.Version);

            var portTagCommand = new PortTagCommand(CacheContext, BlamCache);

            var blamWeapTag = BlamCache.IndexItems.Find(x => x.Filename == blamWeapName);

            if (blamWeapTag == null)
            {
                Console.WriteLine($"{blamWeapName} does not exist.");
                return false;
            }

            // Check if ED has named tags, or has that weapon.
            if (!CacheContext.TagNames.ContainsValue(blamWeapName))
            {
                Console.WriteLine($"{blamWeapName} does not exist or is named incorrectly or tags are not named.");
                return false;
            }

            CachedTagInstance edWeapTag = null;

            edWeapTag = ArgumentParser.ParseTagSpecifier(CacheContext, $"{blamWeapName}.weap");

            if (edWeapTag == null)
            {
                Console.WriteLine($"{blamWeapName} does not exist or is named incorrectly or tags are not named.");
                return false;
            }

            string blamSndName = null;
            string edSndName = null;

            // Blam:
            // Deserialize weap tag
            var blamContext = new CacheSerializationContext(CacheContext, BlamCache, blamWeapTag);
            var blamWeap = blamDeserializer.Deserialize<Weapon>(blamContext);

            var blamContext2 = new CacheSerializationContext(CacheContext, BlamCache, BlamCache.IndexItems.Find(x => x.ID == blamWeap.Barrels[0].FiringEffects[0].FiringEffect2.Index));
            var blamEffe = blamDeserializer.Deserialize<Effect>(blamContext2);

            // ED:
            // Find weapon
            var edWeapInstance = ArgumentParser.ParseTagSpecifier(CacheContext, $"{blamWeapName}.weap");

            if (edWeapInstance == null)
                throw new Exception($"Failed to find ED weapon {blamWeapName}");

            // Deserialize weap tag
            Weapon edWeap;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edWeapInstance);
                edWeap = CacheContext.Deserializer.Deserialize<Weapon>(edContext);
            }

            // Different for the smg compared to the rest of the weapons
            if (blamWeapTag.Filename.Contains("smg"))
            {
                for (int j = 1; j < 7; j++)
                {
                    blamSndName = BlamCache.IndexItems.Find(x => x.ID == blamWeap.Attachments[j].Attachment2.Index).Filename;

                    // Rename firing sound tag
                    edSndName = CacheContext.TagNames.ContainsKey(edWeap.Attachments[j].Attachment2.Index) ? CacheContext.TagNames[edWeap.Attachments[j].Attachment2.Index] : null;
                    if (edSndName != null)
                        CacheContext.TagNames[edWeap.Attachments[j].Attachment2.Index] = $"{CacheContext.TagNames[edWeap.Attachments[j].Attachment2.Index]}_HO";

                    portTagCommand.Execute(new List<string> { "lsnd", blamSndName });

                    edWeap.Attachments[j].Attachment2 = ArgumentParser.ParseTagSpecifier(CacheContext, blamSndName + ".lsnd");
                }

                // Serialize ED weap tag
                using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var context = new TagSerializationContext(stream, CacheContext, edWeapTag);
                    CacheContext.Serializer.Serialize(context, edWeap);
                }

                // DEBUG: verify if tags ported and are assigned
                foreach (var a in edWeap.Attachments)
                {
                    edSndName = CacheContext.TagNames.ContainsKey(a.Attachment2.Index) ? CacheContext.TagNames[a.Attachment2.Index] : null;

                    if (edSndName != null)
                        Console.WriteLine($"DEBUG: ported: [{a.Attachment2.Group}] 0x{a.Attachment2.Index:X4} {edSndName}");
                }

                return true;
            }

            // Cheap checks, only convert weapons that have a first person firing effect
            // This will break on turrets: sword, hammer, bomb, flag, ball, spartan laser
            if (blamWeap.Barrels.Count == 0)
                throw new Exception($"Unsupported weapon due to missing 'Barrels': {blamWeapName}");

            if (blamWeap.Barrels[0].FiringEffects.Count == 0)
                throw new Exception($"Unsupported weapon due to missing 'Barrels[0].FiringEffects': {blamWeapName}");

            if (blamWeap.Barrels[0].FiringEffects[0].FiringEffect2 == null || (uint)blamWeap.Barrels[0].FiringEffects[0].FiringEffect2.Index == 0xFFFFFFFF)
                throw new Exception($"Unsupported weapon due to missing 'Barrels[0].FiringEffects[0].FiringEffect2' tag ref: {blamWeapName}");

            Effect edEffe;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edWeap.Barrels[0].FiringEffects[0].FiringEffect2);
                edEffe = CacheContext.Deserializer.Deserialize<Effect>(edContext);
            }

            // Rename firing sound tag
            foreach (var a in edEffe.Events[0].Parts)
            {
                edSndName = CacheContext.TagNames.ContainsKey(a.Type.Index) ? CacheContext.TagNames[a.Type.Index] : null;
                if (edSndName != null)
                    CacheContext.TagNames[a.Type.Index] = $"{CacheContext.TagNames[a.Type.Index]}_HO";
            }

            // Port firing sound
            foreach (var a in blamEffe.Events[0].Parts)
            {
                // Get blam sound name
                blamSndName = BlamCache.IndexItems.Find(x => x.ID == a.Type.Index).Filename;

                portTagCommand.Execute(new List<string> { "snd!", blamSndName });
            }

            // Set new sounds
            int i = -1;
            int index = 0;
            if (blamWeapTag.Filename.Contains("needler"))
                index = 2;

            edEffe.Events[0].Parts = new List<Effect.Event.Part>();
            foreach (var a in blamEffe.Events[index].Parts)
            {
                a.RuntimeBaseGroupTag = a.Type.Group.Tag;

                edEffe.Events[index].Parts.Add(a);

                i++;

                // Get blam sound name
                blamSndName = BlamCache.IndexItems.Find(x => x.ID == a.Type.Index).Filename;

                edEffe.Events[index].Parts[i].Type = ArgumentParser.ParseTagSpecifier(CacheContext, $"{blamSndName}.snd!");

                if (edEffe.Events[index].Parts[i].Type == null || edEffe.Events[index].Parts[i].Type.Index > 0x7FFF)
                    throw new Exception($"edEffe.Events[0].Parts[{i}].Type is null, sound tag has not converted.");
            }

            // Serialize ED effe tag
            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edWeap.Barrels[0].FiringEffects[0].FiringEffect2);
                CacheContext.Serializer.Serialize(context, edEffe);
            }

            // DEBUG: verify if tags ported and are assigned
            foreach (var a in edEffe.Events[index].Parts)
            {
                edSndName = CacheContext.TagNames.ContainsKey(a.Type.Index) ? CacheContext.TagNames[a.Type.Index] : null;
                if (edSndName != null)
                    Console.WriteLine($"DEBUG: ported: [{a.Type.Group}] 0x{a.Type.Index:X4} {edSndName}");
            }

            return true;
        }

        public bool AdjustScripts(List<string> args)
        {
            Scenario edScnr = new Scenario();
            Scenario blamScnr = new Scenario();
            var PortTagCommand = new PortTagCommand(CacheContext, BlamCache);
            var blamDeserializer = new TagDeserializer(BlamCache.Version);
            var blamInstance = new CacheFile.IndexItem();

            if (args.Count == 0)
            {
                Console.WriteLine($"ERROR: ED scnr tagname is required.");
                return false;
            }

            var edTagSource = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);

            if (edTagSource == null)
            {
                Console.WriteLine($"ERROR: tag does not exist: {args[0]}");
                return false;
            }

            if (!CacheContext.TagNames.ContainsKey(edTagSource.Index))
            {
                Console.WriteLine($"ERROR: tag is not named {args[0]}");
                return false;
            }

            foreach (var item in BlamCache.IndexItems)
            {
                if (item.ClassCode == "scnr")
                {
                    blamInstance = item;
                    break;
                }
            }

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edTagSource);
                edScnr = CacheContext.Deserializer.Deserialize<Scenario>(edContext);
            }

            var blamContext = new CacheSerializationContext(CacheContext, BlamCache, blamInstance);
            blamScnr = blamDeserializer.Deserialize<Scenario>(blamContext);

            foreach (var line in DisabledScriptsString[CacheContext.TagNames[edTagSource.Index].Split("\\".ToCharArray()).Last()])
            {
                var items = line.Split(",".ToCharArray());

                int scriptIndex = Convert.ToInt32(items[0]);


                uint.TryParse(items[2], NumberStyles.HexNumber, null, out uint NextExpressionHandle);
                ushort.TryParse(items[3], NumberStyles.HexNumber, null, out ushort Opcode);
                byte.TryParse(items[4].Substring(0, 2), NumberStyles.HexNumber, null, out byte data0);
                byte.TryParse(items[4].Substring(2, 2), NumberStyles.HexNumber, null, out byte data1);
                byte.TryParse(items[4].Substring(4, 2), NumberStyles.HexNumber, null, out byte data2);
                byte.TryParse(items[4].Substring(6, 2), NumberStyles.HexNumber, null, out byte data3);

                edScnr.ScriptExpressions[scriptIndex].NextExpressionHandle = NextExpressionHandle;
                edScnr.ScriptExpressions[scriptIndex].Opcode = Opcode;
                edScnr.ScriptExpressions[scriptIndex].Data[0] = data0;
                edScnr.ScriptExpressions[scriptIndex].Data[1] = data1;
                edScnr.ScriptExpressions[scriptIndex].Data[2] = data2;
                edScnr.ScriptExpressions[scriptIndex].Data[3] = data3;
            }

            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edTagSource);
                CacheContext.Serializer.Serialize(context, edScnr);
            }

            Console.WriteLine($"Scripts changed.");

            return true;
        }

        public bool FixCisc(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var edTag = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);

            if (edTag == null)
                return false;

            CinematicScene cisc;
            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
                cisc = CacheContext.Deserializer.Deserialize<CinematicScene>(edContext);
            }

            foreach (var shot in cisc.Shots)
            {
                var newFrames = new List<CinematicScene.ShotBlock.FrameBlock>();
                
                for (int i = 0; i < shot.LoadedFrameCount; i++)
                {
                    newFrames.Add(shot.Frames[i]);

                    if (i + 1 == shot.LoadedFrameCount)
                        break;

                    newFrames.Add(new CinematicScene.ShotBlock.FrameBlock
                    {
                        Position = new RealPoint3d
                        (
                            (shot.Frames[i].Position.X + shot.Frames[i + 1].Position.X) / 2f,
                            (shot.Frames[i].Position.Y + shot.Frames[i + 1].Position.Y) / 2f,
                            (shot.Frames[i].Position.Z + shot.Frames[i + 1].Position.Z) / 2f
                        ),
                        Unknown1 = (shot.Frames[i].Unknown1 + shot.Frames[i + 1].Unknown1) / 2,
                        Unknown2 = (shot.Frames[i].Unknown2 + shot.Frames[i + 1].Unknown2) / 2,
                        Unknown3 = (shot.Frames[i].Unknown3 + shot.Frames[i + 1].Unknown3) / 2,
                        Unknown4 = (shot.Frames[i].Unknown4 + shot.Frames[i + 1].Unknown4) / 2,
                        Unknown5 = (shot.Frames[i].Unknown5 + shot.Frames[i + 1].Unknown5) / 2,
                        Unknown6 = (shot.Frames[i].Unknown6 + shot.Frames[i + 1].Unknown6) / 2,
                        Unknown7 = (shot.Frames[i].Unknown7 + shot.Frames[i + 1].Unknown7) / 2,
                        Unknown8 = (shot.Frames[i].Unknown8 + shot.Frames[i + 1].Unknown8) / 2,
                        FOV = (shot.Frames[i].FOV + shot.Frames[i + 1].FOV) / 2,
                        NearPlane = (shot.Frames[i].NearPlane + shot.Frames[i + 1].NearPlane) / 2,
                        FarPlane = (shot.Frames[i].FarPlane + shot.Frames[i + 1].FarPlane) / 2,
                        FocalDepth = (shot.Frames[i].FocalDepth + shot.Frames[i + 1].FocalDepth) / 2,
                        BlurAmount = (shot.Frames[i].BlurAmount + shot.Frames[i + 1].BlurAmount) / 2,

                    });
                }

                shot.Frames = newFrames;
                newFrames = new List<CinematicScene.ShotBlock.FrameBlock>();
                shot.LoadedFrameCount *= 2;

                break;

            }

            using (var stream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
            {
                var context = new TagSerializationContext(stream, CacheContext, edTag);
                CacheContext.Serializer.Serialize(context, cisc);
            }

            Console.WriteLine("Done.");

            return true;
        }

        private static Dictionary<string, List<string>> DisabledScriptsString = new Dictionary<string, List<string>>
        {
            // Format: Script expression tagblock index (dec), expression handle (salt + tagblock index), next expression handle (salt + tagblock index), opcode, data, 
            // expression type, value type, script expression name, original value, comment
            // Ideally this should use a dictionary with a list of script expressions per map name. I'm using a simple text format as this is how I dump scripts and modify them currently.

            ["c100"] = new List<string>
            {
                "00000293,E4980125,E48E011B,0000,00000000,Expression,FunctionName,begin,// E4860113",
                "00000444,E52F01BC,E53D01CA,030F,BD0130E5,Group,Void,unit_action_test_reset,// E53401C1",
                "00000495,E56201EF,E57001FD,030F,F00163E5,Group,Void,unit_action_test_reset,// E56701F4",
                "00000509,E57001FD,E5780205,0667,FE0171E5,Group,Void,chud_show_navpoint,// FFFFFFFF",
                "00000548,E5970224,E5A50232,030F,250298E5,Group,Void,unit_action_test_reset,// E59C0229",
                "00000562,E5A50232,E5B70244,0000,3302A6E5,Group,Void,begin,// FFFFFFFF",
                "00000622,E5E1026E,E5EF027C,030F,6F02E2E5,Group,Void,unit_action_test_reset,// E5E60273",
                "00000636,E5EF027C,E6090296,0000,7D02F0E5,Group,Void,begin,// FFFFFFFF",
                "00000715,E63E02CB,E64C02D9,030F,CC023FE6,Group,Void,unit_action_test_reset,// E64302D0",
                "00000729,E64C02D9,E66E02FB,0000,DA024DE6,Group,Void,begin,// FFFFFFFF",
                "00000852,E6C70354,E6D0035D,03F4,5503C8E6,Group,Void,sound_impulse_start,// E6CC0359",
                "00000884,E6E70374,FFFFFFFF,03F4,7503E8E6,Group,Void,sound_impulse_start,//E6EC0379",
                "00001572,E9970624,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E9790606",
                "00001043,E7860413,E79B0428,0112,140487E7,Group,Void,unit_enable_vision_mode,// E78B0418",
                "00001064,E79B0428,E7AD043A,0009,29049CE7,ScriptReference,Void,,// E79D042A",
                "00001328,E8A30530,E8B80545,0112,3105A4E8,Group,Void,unit_enable_vision_mode,// E8A80535",
                "00004033,F3340FC1,FFFFFFFF,0002,C20F35F3,Group,Void,if,// F3420FCF",
                "00013476,981834A4,982834B4,005A,A5341998,Group,Void,object_set_function_variable,// 982234AE cinematic_scripting_set_user_input_constraints",
                "00014308,9B5837E4,9B6837F4,005A,E537599B,Group,Void,object_set_function_variable,// 9B6237EE cinematic_scripting_set_user_input_constraints",
                "00020922,B52E51BA,B53D51C9,03A1,BB512FB5,Group,Void,cinematic_scripting_start_effect,// B53751C3 cinematic_scripting_set_user_input_constraints"
            },

            ["c200"] = new List<string>
            {
                "00000293,E4980125,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E4860113",
                "00000482,E55501E2,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E52F01BC",
                "00000532,E5870214,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E56201EF",
                "00000603,E5CE025B,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E5970224",
                "00000693,E62802B5,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E5E1026E",
                "00000802,E6950322,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E63E02CB",
                "00000852,E6C70354,E6D0035D,03F4,5503C8E6,Group,Void,sound_impulse_start,// E6CC0359",
                "00000884,E6E70374,FFFFFFFF,03F4,7503E8E6,Group,Void,sound_impulse_start,// E6EC0379",
                "00001043,E7860413,E79B0428,0112,140487E7,Group,Void,unit_enable_vision_mode,// E78B0418",
                "00001064,E79B0428,E7AD043A,0009,29049CE7,ScriptReference,Void,,// E79D042A",
                "00001328,E8A30530,E8B80545,0112,3105A4E8,Group,Void,unit_enable_vision_mode,// E8A80535",
                "00001572,E9970624,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E9790606"
            },

            ["sc140"] = new List<string>
            {
                "00000909,E700038D,E6F60383,0000,00000000,Expression,FunctionName,begin,// E6EE037B",
                "00001098,E7BD044A,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E7970424",
                "00001148,E7EF047C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E7CA0457",
                "00001219,E83604C3,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E7FF048C",
                "00001309,E890051D,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E84904D6",
                "00001418,E8FD058A,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E8A60533",
                "00001468,E92F05BC,E93805C5,03F4,BD0530E9,Group,Void,sound_impulse_start,// E93405C1",
                "00001500,E94F05DC,FFFFFFFF,03F4,DD0550E9,Group,Void,sound_impulse_start,// E95405E1",
                "00001659,E9EE067B,EA030690,0112,7C06EFE9,Group,Void,unit_enable_vision_mode,// E9F30680",
                "00001680,EA030690,EA1506A2,0012,910604EA,ScriptReference,Void,,// EA050692",
                "00001944,EB0B0798,EB2007AD,0112,99070CEB,Group,Void,unit_enable_vision_mode,// EB10079D",
                "00002188,EBFF088C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EBE1086E",
                "00005201,F7C41451,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// F7A2142F",
                "00015275,9F1F3BAB,9F293BB5,0192,AC3B209F,Group,Void,ai_force_active,// 9F233BAF",
                "00015458,9FD63C62,9FE03C6C,0192,633CD79F,Group,Void,ai_force_active,// 9FDA3C66",
                "00015771,A10F3D9B,A1193DA5,0192,9C3D10A1,Group,Void,ai_force_active,// A1133D9F",
                "00015966,A1D23E5E,A1DC3E68,0192,5F3ED3A1,Group,Void,ai_force_active,// A1D63E62",
                "00016351,A3533FDF,A35A3FE6,017F,E03F54A3,Group,Void,ai_allegiance,// A3573FE3",
                "00016405,A3894015,A3A44030,0002,16408AA3,Group,Void,if,// A390401C",
                "00016728,A4CC4158,A4D64162,0192,5941CDA4,Group,Void,ai_force_active,// A4D0415C",
                "00018114,AA3646C2,AA4046CC,0178,C34637AA,Group,Void,ai_magically_see,// AA3A46C6",
                "00018134,AA4A46D6,AA5346DF,0166,D7464BAA,Group,Void,ai_place,// AA4D46D9",
                "00021577,B7BD5449,B7C3544F,0333,4A54BEB7,Group,Void,switch_zone_set,// B7C0544C",
                "00001538,E9750602,FFFFFFFF,0376,030676E9,Group,Void,cinematic_skip_stop_internal,// E9770604",
                "00018057,A9FD4689,AA03468F,0015,8A46FEA9,Group,Void,sleep_forever,// AA00468C",
                
                // "00016605,A45140DD,A49B4127,0004,DE4052A4,Group,Void,set,// A45540E1" // remove intro cine
            },

            ["h100"] = new List<string>
            {
                "00000909,E700038D,E6F60383,0000,00000000,Expression,FunctionName,begin,// E6EE037B",
                "00001060,E7970424,E7A50432,030F,250498E7,Group,Void,unit_action_test_reset,// E79C0429",
                "00001111,E7CA0457,E7D80465,030F,5804CBE7,Group,Void,unit_action_test_reset,// E7CF045C",
                "00001125,E7D80465,E7E0046D,0667,6604D9E7,Group,Void,chud_show_navpoint,// FFFFFFFF",
                "00001164,E7FF048C,E80D049A,030F,8D0400E8,Group,Void,unit_action_test_reset,// E8040491",
                "00001178,E80D049A,E81F04AC,0000,9B040EE8,Group,Void,begin,// FFFFFFFF",
                "00001238,E84904D6,E85704E4,030F,D7044AE8,Group,Void,unit_action_test_reset,// E84E04DB",
                "00001252,E85704E4,E87104FE,0000,E50458E8,Group,Void,begin,// FFFFFFFF",
                "00001331,E8A60533,E8B40541,030F,3405A7E8,Group,Void,unit_action_test_reset,// E8AB0538",
                "00001336,E8AB0538,E8D60563,0002,3905ACE8,Group,Void,if,// E8D60563",
                "00001345,E8B40541,E8D60563,0000,4205B5E8,Group,Void,begin,// FFFFFFFF",
                "00001468,E92F05BC,E93805C5,03F4,BD0530E9,Group,Void,sound_impulse_start,// E93405C1",
                "00001500,E94F05DC,FFFFFFFF,03F4,DD0550E9,Group,Void,sound_impulse_start,// E95405E1",
                "00001659,E9EE067B,EA030690,0112,7C06EFE9,Group,Void,unit_enable_vision_mode,// E9F30680",
                "00001680,EA030690,EA1506A2,0012,910604EA,ScriptReference,Void,,// EA050692",
                "00001944,EB0B0798,EB2007AD,0112,99070CEB,Group,Void,unit_enable_vision_mode,// EB10079D",
                "00002188,EBFF088C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EBE1086E",
                "00005201,F7C41451,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// F7A2142F",
                "00012780,956031EC,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 954D31D9",
                "00012813,9581320D,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 956631F2",
                "00015198,9ED23B5E,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 9D5439E0",
                "00016273,A3053F91,A3113F9D,0002,923F06A3,Group,Void,if,// A30B3F97",
                "00016285,A3113F9D,A31D3FA9,0112,9E3F12A3,Group,Void,unit_enable_vision_mode,// A3173FA3",
                "00016606,A45240DE,A46940F5,0112,DF4053A4,Group,Void,unit_enable_vision_mode,// A45840E4",
                "00017061,A61942A5,A62342AF,0004,A6421AA6,Group,Void,set,// A61D42A9",
                "00018774,ACCA4956,ACD44960,013A,5749CBAC,Group,Void,device_set_power,// ACCE495A",
                "00019806,B0D24D5E,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B0BA4D46",
                "00019836,B0F04D7C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B0D84D64",
                "00019866,B10E4D9A,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B0F64D82",
                "00019896,B12C4DB8,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B1144DA0",
                "00020043,B1BF4E4B,B1C84E54,0014,4C4EC0B1,Group,Void,sleep,// B1C24E4E",
                "00020052,B1C84E54,B1D54E61,0014,554EC9B1,Group,Void,sleep,// B1CB4E57",
                "00020089,B1ED4E79,B1F94E85,0002,7A4EEEB1,Group,Void,if,// B1F34E7F",
                "00020113,B2054E91,B2104E9C,0014,924E06B2,Group,Void,sleep,// B2084E94",
                "00020847,B4E3516F,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B25A4EE6",
                "00020937,B53D51C9,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B5095195",
                "00020994,B5765202,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B54251CE",
                "00021051,B5AF523B,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B57B5207",
                "00021108,B5E85274,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B5B45240",
                "00021639,B7FB5487,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B7D75463",
                "00021815,B8AB5537,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B86554F1",
                "00022191,BA2356AF,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// B9D45660",
                "00022480,BB4457D0,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// BB03578F",
                "00022823,BC9B5927,FFFFFFFF,040D,28599CBC,Group,Void,vehicle_auto_turret,// BCA2592E",
                "00022920,BCFC5988,FFFFFFFF,040D,8959FDBC,Group,Void,vehicle_auto_turret,// BD03598F",
                "00023021,BD6159ED,FFFFFFFF,040D,EE5962BD,Group,Void,vehicle_auto_turret,// BD6859F4",
                "00023115,BDBF5A4B,FFFFFFFF,040D,4C5AC0BD,Group,Void,vehicle_auto_turret,// BDC65A52",
                "00023224,BE2C5AB8,FFFFFFFF,040D,B95A2DBE,Group,Void,vehicle_auto_turret,// BE335ABF",
                "00023357,BEB15B3D,FFFFFFFF,040D,3E5BB2BE,Group,Void,vehicle_auto_turret,// BEB85B44",
                "00023407,BEE35B6F,BEE85B74,01C9,705BE4BE,ScriptReference,Void,,// BEE55B71",
                "00023449,BF0D5B99,BEE35B6F,0000,00000000,Expression,FunctionName,begin,// BEDD5B69",
                "00024398,C2C25F4E,C2F05F7C,004F,4F5FC3C2,Group,Void,object_create_folder_anew,// C2C55F51",
                "00025402,C6AE633A,C6B2633E,0000,00000000,Expression,FunctionName,begin,// C6AF633B",
                "00025462,C6EA6376,C6EE637A,0000,00000000,Expression,FunctionName,begin,// C6EB6377",
                "00025508,C71863A4,C71C63A8,0000,00000000,Expression,FunctionName,begin,// C71963A5",
                "00025906,C8A66532,C84864D4,0000,00000000,Expression,FunctionName,begin,// C83B64C7",
                "00026011,C90F659B,C8B1653D,0000,00000000,Expression,FunctionName,begin,// C8AB6537",
                "00026116,C9786604,C91A65A6,0000,00000000,Expression,FunctionName,begin,// C91465A0",
                "00026221,C9E1666D,C983660F,0000,00000000,Expression,FunctionName,begin,// C97D6609",
                "00026326,CA4A66D6,C9EC6678,0000,00000000,Expression,FunctionName,begin,// C9E66672",
                "00026431,CAB3673F,CA5566E1,0000,00000000,Expression,FunctionName,begin,// CA4F66DB",
                "00027261,CDF16A7D,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// CDC06A4C",
                "00027330,CE366AC2,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// CDF76A83",
                "00027448,CEAC6B38,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// CE3C6AC8",
                "00027580,CF306BBC,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// CEB26B3E",
                "00036488,F1FC8E88,F19F8E2B,0000,00000000,Expression,FunctionName,begin,// F19C8E28",
                "00037912,F78C9418,F7959421,0002,19948DF7,Group,Void,if,// F792941E",
                "00038519,F9EB9677,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,//F9E49670",
                "00040775,82BC9F47,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 81CD9E58",
                "00040831,82F49F7F,FFFFFFFF,0014,809FF582,Group,Void,sleep,// 82F79F82",
                "00041280,84B5A140,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 83339FBE",
                "00041327,84E4A16F,84F6A181,0141,70A1E584,Group,Void,device_group_set_immediate,// 84E8A173",
                "00041542,85BBA246,85CDA258,0141,47A2BC85,Group,Void,device_group_set_immediate,// 85BFA24A",
                "00041751,868CA317,869EA329,0141,18A38D86,Group,Void,device_group_set_immediate,// 8690A31B",
                "00041972,8769A3F4,8774A3FF,0141,F5A36A87,Group,Void,device_group_set_immediate,// 876DA3F8",
                "00043346,8CC7A952,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,G:ai_wave_07_squad_02// 8C21A8AC",
            },

            ["l200"] = new List<string>
            {
                "00000909,E700038D,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E6EE037B",
                "00001060,E7970424,E7A50432,030F,250498E7,Group,Void,unit_action_test_reset,// E79C0429 bypass pda check",
                "00001111,E7CA0457,E7D80465,030F,5804CBE7,Group,Void,unit_action_test_reset,// E7CF045C",
                "00001125,E7D80465,E7E0046D,0667,6604D9E7,Group,Void,chud_show_navpoint,// FFFFFFFF",
                "00001164,E7FF048C,E80D049A,030F,8D0400E8,Group,Void,unit_action_test_reset,// E8040491",
                "00001178,E80D049A,E81F04AC,0000,9B040EE8,Group,Void,begin,// FFFFFFFF",
                "00001238,E84904D6,E85704E4,030F,D7044AE8,Group,Void,unit_action_test_reset,// E84E04DB",
                "00001252,E85704E4,E87104FE,0000,E50458E8,Group,Void,begin,// FFFFFFFF",
                "00001278,E87104FE,E8740501,0014,FF0472E8,Group,Void,sleep,",
                "00001331,E8A60533,E8B40541,030F,3405A7E8,Group,Void,unit_action_test_reset,// E8AB0538 not sure if i should bypass the begin",
                "00001345,E8B40541,E8D60563,0000,4205B5E8,Group,Void,begin,// FFFFFFFF",
                "00001468,E92F05BC,E93805C5,03F4,BD0530E9,Group,Void,sound_impulse_start,// E93405C1",
                "00001500,E94F05DC,FFFFFFFF,03F4,DD0550E9,Group,Void,sound_impulse_start,// E95405E1",
                "00001659,E9EE067B,EA030690,0112,7C06EFE9,Group,Void,unit_enable_vision_mode,// E9F30680",
                "00001680,EA030690,EA1506A2,0012,910604EA,ScriptReference,Void,,// EA050692",
                "00001944,EB0B0798,EB2007AD,0112,99070CEB,Group,Void,unit_enable_vision_mode,// EB10079D",
                "00002188,EBFF088C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EBE1086E",
                "00005201,F7C41451,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// F7A2142F",
                "00011278,8F822C0E,8F8C2C18,0141,0F2C838F,Group,Void,device_group_set_immediate,// 8F862C12",
                "00011307,8F9F2C2B,8FD02C5C,0014,2C2CA08F,Group,Void,sleep,// 8FA22C2E",
                "00011412,90082C94,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// 8FDD2C69",
                "00013111,96AB3337,96B1333D,0017,3833AC96,Group,Void,wake,// 96AE333A",
                "00013215,9713339F,971C33A8,0166,A0331497,Group,Void,ai_place,// 971633A2",
                "00013263,974333CF,974933D5,0017,D0334497,Group,Void,wake,// 974633D2",
                "00013377,97B53441,97BE344A,0166,4234B697,Group,Void,ai_place,// 97B83444",
                "00013428,97E83474,97EE347A,0017,7534E997,Group,Void,wake,// 97EB3477",
                "00013542,985A34E6,986034EC,0017,E7345B98,Group,Void,wake,// 985D34E9",
                "00013696,98F43580,98FD3589,0166,8135F598,Group,Void,ai_place,// 98F73583",
                "00013792,995435E0,995A35E6,0017,E1355599,Group,Void,wake,// 995735E3",
                "00013936,99E43670,99ED3679,0166,7136E599,Group,Void,ai_place,// 99E73673",
                "00013960,99FC3688,9A02368E,0017,8936FD99,Group,Void,wake,// 99FF368B",
                "00014093,9A81370D,9A8A3716,0166,0E37829A,Group,Void,ai_place,// 9A843710",
                "00014113,9A953721,9A9B3727,0017,2237969A,Group,Void,wake,// 9A983724",
                "00014250,9B1E37AA,9B2737B3,0166,AB371F9B,Group,Void,ai_place,// 9B2137AD",
                "00014270,9B3237BE,9B3B37C7,0166,BF37339B,Group,Void,ai_place,// 9B3537C1",
                "00014290,9B4637D2,9B4F37DB,0166,D337479B,Group,Void,ai_place,// 9B4937D5",
                "00014313,9B5D37E9,9B6337EF,0017,EA375E9B,Group,Void,wake,// 9B6037EC",
                "00014817,9D5539E1,9D5B39E7,04EC,E239569D,Group,Void,data_mine_set_mission_segment,// 9D5839E4",
                "00015167,9EB33B3F,9EBC3B48,001D,403BB49E,Group,Void,print,// 9EB63B42",
                "00015336,9F5C3BE8,FFFFFFFF,0016,E93B5D9F,Group,Void,sleep_until,// 9F663BF2",
                "00015361,9F753C01,9F7B3C07,04EC,023C769F,Group,Void,data_mine_set_mission_segment,// 9F783C04",
                "00015760,A1043D90,A10A3D96,04EC,913D05A1,Group,Void,data_mine_set_mission_segment,// A1073D93",
                "00016074,A23E3ECA,A2443ED0,04EC,CB3E3FA2,Group,Void,data_mine_set_mission_segment,// A2413ECD",
                "00016535,A40B4097,A411409D,04EC,98400CA4,Group,Void,data_mine_set_mission_segment,// A40E409A",
                "00016938,A59E422A,A5A74233,0166,2B429FA5,Group,Void,ai_place,// A5A1422D",
                "00017274,A6EE437A,A6F44380,04EC,7B43EFA6,Group,Void,data_mine_set_mission_segment,// A6F1437D",
                "00017728,A8B44540,A8BD4549,0014,4145B5A8,Group,Void,sleep,// A8B74543",
                "00018175,AA7346FF,AA794705,04EC,004774AA,Group,Void,data_mine_set_mission_segment,// AA764702",
                "00018437,AB794805,AB82480E,0166,06487AAB,Group,Void,ai_place,// AB7C4808",
                "00018846,AD12499E,AD1B49A7,0166,9F4913AD,Group,Void,ai_place,// AD1549A1",
                "00018866,AD2649B2,AD2F49BB,0166,B34927AD,Group,Void,ai_place,// AD2949B5",
                "00020192,B2544EE0,B25A4EE6,04EC,E14E55B2,Group,Void,data_mine_set_mission_segment,// B2574EE3",
                "00023448,BF0C5B98,BF155BA1,0158,995B0DBF,Group,Void,ai_dialogue_enable,// BF0F5B9B",
                "00024050,C1665DF2,FFFFFFFF,0169,F35D67C1,Group,Void,ai_cannot_die,// C16A5DF6",
                "00028696,D38C7018,D3957021,0371,19708DD3,Group,Void,fade_in,// D392701E",

                "00001538,E9750602,FFFFFFFF,0376,030676E9,Group,Void,cinematic_skip_stop_internal,// E9770604 prevent cinematic from looping; to fix properly",
                "00001555,E9860613,FFFFFFFF,0376,140687E9,Group,Void,cinematic_skip_stop_internal,// E9880615 prevent cinematic from looping; to fix properly",
                "00015667,A0A73D33,A0A23D2E,0000,00000000,Expression,FunctionName,begin,// A0933D1F // test: force cop to teleport to open the hatch",
            },

            ["l300"] = new List<string>
            {
                "00000891,E6EE037B,E6F50382,0016,7C03EFE6,Group,Void,sleep_until,// E6F60383",
                "00001060,E7970424,E7A50432,030F,250498E7,Group,Void,unit_action_test_reset,// E79C0429",
                "00001111,E7CA0457,E7D80465,030F,5804CBE7,Group,Void,unit_action_test_reset,// E7CF045C",
                "00001125,E7D80465,E7E0046D,0667,6604D9E7,Group,Void,chud_show_navpoint,// FFFFFFFF",
                "00001164,E7FF048C,E80D049A,030F,8D0400E8,Group,Void,unit_action_test_reset,// E8040491",
                "00001178,E80D049A,E81F04AC,0000,9B040EE8,Group,Void,begin,// FFFFFFFF",
                "00001238,E84904D6,E85704E4,030F,D7044AE8,Group,Void,unit_action_test_reset,// E84E04DB",
                "00001252,E85704E4,E87104FE,0000,E50458E8,Group,Void,begin,// FFFFFFFF",
                "00001331,E8A60533,E8B40541,030F,3405A7E8,Group,Void,unit_action_test_reset,// E8AB0538",
                "00001345,E8B40541,E8D60563,0000,4205B5E8,Group,Void,begin,// FFFFFFFF",
                "00001468,E92F05BC,E93805C5,03F4,BD0530E9,Group,Void,sound_impulse_start,// E93405C1",
                "00001500,E94F05DC,FFFFFFFF,03F4,DD0550E9,Group,Void,sound_impulse_start,// E95405E1",
                "00001659,E9EE067B,EA030690,0112,7C06EFE9,Group,Void,unit_enable_vision_mode,// E9F30680",
                "00001680,EA030690,EA1506A2,0012,910604EA,ScriptReference,Void,,// EA050692",
                "00001944,EB0B0798,EB2007AD,0112,99070CEB,Group,Void,unit_enable_vision_mode,// EB10079D",
                "00002188,EBFF088C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EBE1086E",
                "00005201,F7C41451,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// F7A2142F",
                "00020425,B33D4FC9,B3494FD5,0004,CA4F3EB3,Group,Void,set,// B3434FCF",
                "00020521,B39D5029,B3A75033,0169,2A509EB3,Group,Void,ai_cannot_die,// B3A1502D",
                "00020550,B3BA5046,B3C65052,0004,4750BBB3,Group,Void,set,// B3C0504C",
                "00020646,B41A50A6,B42450B0,0169,A7501BB4,Group,Void,ai_cannot_die,// B41E50AA",
                "00020675,B43750C3,B44350CF,0004,C45038B4,Group,Void,set,// B43D50C9",
                "00020771,B4975123,B4A1512D,0169,245198B4,Group,Void,ai_cannot_die,// B49B5127",
                "00020800,B4B45140,B4C0514C,0004,4151B5B4,Group,Void,set,// B4BA5146",
                "00020896,B51451A0,B51E51AA,0169,A15115B5,Group,Void,ai_cannot_die,// B51851A4",
                "00020925,B53151BD,B53D51C9,0004,BE5132B5,Group,Void,set,// B53751C3",
                "00021021,B591521D,B59B5227,0169,1E5292B5,Group,Void,ai_cannot_die,// B5955221",
                "00021050,B5AE523A,B5BA5246,0004,3B52AFB5,Group,Void,set,// B5B45240",
                "00021146,B60E529A,B61852A4,0169,9B520FB6,Group,Void,ai_cannot_die,// B612529E",
                "00021175,B62B52B7,B63752C3,0004,B8522CB6,Group,Void,set,// B63152BD",
                "00021271,B68B5317,B6955321,0169,18538CB6,Group,Void,ai_cannot_die,// B68F531B",
                "00021300,B6A85334,B6B45340,0004,3553A9B6,Group,Void,set,// B6AE533A",
                "00021396,B7085394,B712539E,0169,955309B7,Group,Void,ai_cannot_die,// B70C5398",
                "00021425,B72553B1,B73153BD,0004,B25326B7,Group,Void,set,// B72B53B7",
                "00021517,B781540D,B78B5417,0169,0E5482B7,Group,Void,ai_cannot_die,// B7855411",
                "00021546,B79E542A,B7AA5436,0004,2B549FB7,Group,Void,set,// B7A45430",
                "00021638,B7FA5486,B8045490,0169,8754FBB7,Group,Void,ai_cannot_die,// B7FE548A",
                "00021753,B86D54F9,B8775503,0169,FA546EB8,Group,Void,ai_cannot_die,// B87154FD",
                "00021795,B8975523,FFFFFFFF,0169,245598B8,Group,Void,ai_cannot_die,// B89B5527",
                "00022403,BAF75783,BB03578F,0004,8457F8BA,Group,Void,set,// BAFD5789",
                "00022523,BB6F57FB,BB795805,0169,FC5770BB,Group,Void,ai_cannot_die,// BB7357FF",
                "00022689,BC1558A1,BC1C58A8,017F,A25816BC,Group,Void,ai_allegiance,// BC1958A5",
                "00023456,BF145BA0,FFFFFFFF,01B1,A15B15BF,Group,Void,ai_set_objective,// BF185BA4",
                "00027973,D0B96D45,D0C36D4F,0169,466DBAD0,Group,Void,ai_cannot_die,// D0BD6D49",
                "00028614,D33A6FC6,D3486FD4,0016,C76F3BD3,Group,Void,sleep_until,// D3426FCE",
                "00028649,D35D6FE9,D3676FF3,0169,EA6F5ED3,Group,Void,ai_cannot_die,// D3616FED",
                "00028675,D3777003,D381700D,0192,047078D3,Group,Void,ai_force_active,// D37B7007",
                "00029030,D4DA7166,D4EF717B,0017,6771DBD4,Group,Void,wake,// D4DD7169",
                "00029086,D512719E,D52771B3,0166,9F7113D5,Group,Void,ai_place,// D51571A1",
                "00034658,EAD68762,EAEF877B,0006,6387D7EA,Group,Boolean,or,// EAE98775",
                "00034789,EB5987E5,EABD8749,0000,00000000,Expression,FunctionName,begin,// EABD8749 disable the whole thing for now",
                "00035083,EC7F890B,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EB5F87EB disable the whole thing for now",
                "00035212,ED00898C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EC858911 disable the whole thing for now",
                "00035397,EDB98A45,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// ED068992 disable the whole thing for now",
                "00035767,EF2B8BB7,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EF238BAF",
                "00035910,EFBA8C46,EFC08C4C,0333,478CBBEF,Group,Void,switch_zone_set,// EFBD8C49",
            },
        };
    }
}