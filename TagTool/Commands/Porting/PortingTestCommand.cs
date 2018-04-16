using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TagTool.Tags;

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
            
            foreach (var item in BlamCache.IndexItems)
            {
                if (item.ClassCode == args[0] && item.Filename == args[1])
                {
                    blamInstance = item;
                    break;
                }
            }

            var blamContext = new CacheSerializationContext(BlamCache, blamInstance);

            var Value = BlamCache.Deserializer.Deserialize(blamContext, TagDefinition.Find(args[0]));

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
            
            foreach (var item in BlamCache.IndexItems)
            {
                if (item.ClassCode == args[0])
                {
                    var blamContext = new CacheSerializationContext(BlamCache, item);
                    var def = BlamCache.Deserializer.Deserialize(blamContext, TagDefinition.Find(args[0]));

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
            var blamContext = new CacheSerializationContext(BlamCache, blamWeapTag);
            Weapon blamWeap = BlamCache.Deserializer.Deserialize<Weapon>(blamContext);

            // Deserialize hlmt tag
            var blamContext2 = new CacheSerializationContext(BlamCache, BlamCache.IndexItems.Find(x => x.ID == blamWeap.Model.Index));
            var blamHlmt = BlamCache.Deserializer.Deserialize<Model>(blamContext2);

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
            var blamContext = new CacheSerializationContext(BlamCache, blamWeapTag);
            Weapon blamWeap = BlamCache.Deserializer.Deserialize<Weapon>(blamContext);

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

            var blamWeapName = args[1];
            var blamWeapTag = BlamCache.IndexItems.Find(x => x.Filename == blamWeapName);

            var portTagCommand = new PortTagCommand(CacheContext, BlamCache);
            
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
            var blamContext = new CacheSerializationContext(BlamCache, blamWeapTag);
            var blamWeap = BlamCache.Deserializer.Deserialize<Weapon>(blamContext);

            var blamContext2 = new CacheSerializationContext(BlamCache, BlamCache.IndexItems.Find(x => x.ID == blamWeap.Barrels[0].FiringEffects[0].FiringEffect2.Index));
            var blamEffe = BlamCache.Deserializer.Deserialize<Effect>(blamContext2);

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
    }
}