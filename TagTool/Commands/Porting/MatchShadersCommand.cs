using TagTool.Cache;
using TagTool.Commands;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;

namespace TagTool.Commands.Porting
{
    public class MatchShadersCommand : Command
    {
        public GameCacheContext CacheContext { get; }
        public CacheFile BlamCache { get; }
        public static int rmPreset = 0x0;
        public static int rmt2Preset = 0x0;
        public static bool newOnly = false;
        public static bool existingOnly = false;
        public static bool rmt2MatchOnly = false;
        public static bool ms23Only = false;
        public static bool ms30Only = false;
        public static bool noBumpMaps = false;
        public static bool forceFunctions = false;
        public static bool debugMode = false;
        public static string tagNameShort = "";
        public static List<string> csvQueue = new List<string>();
        public static List<string> edBitmaps = new List<string>();
        public static List<string> h3Bitmaps = new List<string>();
        public static List<string> edArguments = new List<string>();
        public static List<string> h3Arguments = new List<string>();
        public static Dictionary<string, float[]> edArgPreset;

        public MatchShadersCommand(GameCacheContext cacheContext, CacheFile blamCache) :
            base(CommandFlags.Inherit,

                "MatchShaders",
                "MatchShaders [option] <ED mode or sbsp tag> [Blam Tagname]",
                "MatchShaders",
                "MatchShaders [option] <ED mode or sbsp tag> [Blam Tagname]" +
                "Blam tagname is required if only the tag index is provided." +
                "Options: " +
                "replace: don't reuse previously ported shaders, port anew." +
                "existing: use only existing tags, without porting. Warning: previously ported tags will be used if they exist." +
                "match: match shaders by rmt2 name, don't improvise to use shaders with similar functions.")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }

        public static CachedTagInstance PortShaderTag(Stream stream, GameCacheContext cacheContext, CacheFile blamCache, CacheFile.IndexItem h3Tag)
        {
            WriteLine($"[{h3Tag.ClassCode}] {h3Tag.Filename} processing...");

            var tag = PortTagReference(cacheContext, blamCache, h3Tag.ID);

            if (tag != null)
            {
                WriteLine($"Found existing for {h3Tag.Filename}");
                return tag;
            }
            
            return MatchShader7(stream, cacheContext, blamCache, h3Tag);
        }

        public override object Execute(List<string> args)
        {
            if (args.Count == 0)
                return false;

            if (!CacheContext.TagNames.ContainsKey(0x3AB0))
                CacheContext.TagNames.Add(0x3ab0, @"objects\levels\forge\shaders\fw_global\fw_grid");
            else
                CacheContext.TagNames[0x3AB0] = @"objects\levels\forge\shaders\fw_global\fw_grid";

            CacheFile.IndexItem h3Tag = null;
            int removeArgs = 0;
            var tagName = "";

            // Redirect if only one shader needs to be ported
            if (args[0].ToLower() == "porttag")
            {
                if (args.Count != 3)
                    return false;

                args.RemoveAt(0);

                foreach (var instance in BlamCache.IndexItems)
                {
                    if (instance.ClassCode == args[0] && instance.Filename == args[1])
                    {
                        h3Tag = instance;
                        break;
                    }
                }

                if (h3Tag == null)
                    return false;

                using (var stream = CacheContext.OpenTagCacheReadWrite())
                    return PortShaderTag(stream, CacheContext, BlamCache, h3Tag);
            }

            foreach (var a in args)
            {
                switch (a)
                {
                    case "newOnly":
                        WriteLine($"Info: New shaders only.", true);
                        newOnly = true;
                        removeArgs++;
                        break;
                    case "existing":
                        WriteLine($"Info: using only existing shaders.", true);
                        existingOnly = true;
                        rmt2MatchOnly = false;
                        removeArgs++;
                        break;
                    case "rmt2match":
                        WriteLine($"Info: using only shaders with matching rmt2 tag.", true);
                        rmt2MatchOnly = true;
                        existingOnly = false;
                        removeArgs++;
                        break;
                    case "ms23":
                        WriteLine($"Info: using only shaders from ms23 and not ms30.", true);
                        ms23Only = true;
                        ms30Only = false;
                        removeArgs++;
                        break;
                    case "ms30":
                        WriteLine($"Info: using only shaders from ms23 and not ms30.", true);
                        ms30Only = true;
                        ms23Only = false;
                        removeArgs++;
                        break;
                    case "nobumpmaps":
                        WriteLine($"Info: nulling bumpmaps.", true);
                        noBumpMaps = true;
                        removeArgs++;
                        break;
                    case "debug":
                        WriteLine($"Info: writing debug info to console.", true);
                        debugMode = true;
                        removeArgs++;
                        break;
                }
            }

            args.RemoveRange(0, removeArgs);

            var tag = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);

            if (tag == null)
                return false;

            var tagGroup = tag.Group.Tag;

            if (args.Count == 1)
            {
                if (!CacheContext.TagNames.ContainsKey(tag.Index))
                {
                    WriteLine($"ERROR: Only named ED tags can be used without providing the blam tagname.");
                    return false;
                }
                else
                    tagName = CacheContext.TagNames[tag.Index];
            }
            else if (args.Count == 2)
                tagName = args[1];

            foreach (var instance in BlamCache.IndexItems)
            {
                if ((instance.ClassCode == tagGroup.ToString()) && (instance.Filename == tagName))
                {
                    h3Tag = instance;
                    break;
                }
            }

            if (h3Tag == null)
            {
                WriteLine($"ERROR: Blam tag does not exist: {tagName}.{tagGroup}");
                return true;
            }

            tagNameShort = tagName.Substring(tagName.LastIndexOf("\\") + 1, tagName.Length - tagName.LastIndexOf("\\") - 1);
            
            var blamContext = new CacheSerializationContext(BlamCache, h3Tag);

            object definition = null;

            var renderMaterials = new List<RenderMaterial>();

            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                definition = CacheContext.Deserializer.Deserialize(new TagSerializationContext(stream, CacheContext, tag), TagDefinition.Find(tag.Group.Tag));

                if (tag.Group.Tag.ToString() == "sbsp")
                {
                    var blamDefinition = BlamCache.Deserializer.Deserialize<ScenarioStructureBspMaterials>(blamContext);

                    RestoreH3Shaders(stream, CacheContext, BlamCache, blamDefinition.Materials);
                    renderMaterials = blamDefinition.Materials;
                }
                else if (tag.Group.Tag.ToString() == "mode")
                {
                    var blamDefinition = BlamCache.Deserializer.Deserialize<RenderModel>(blamContext);

                    RestoreH3Shaders(stream, CacheContext, BlamCache, blamDefinition.Materials);
                    renderMaterials = blamDefinition.Materials;
                }
                
                var context = new TagSerializationContext(stream, CacheContext, tag);
                if (tag.Group.Tag.ToString() == "sbsp")
                {
                    var def = (ScenarioStructureBsp)definition;
                    def.Materials = renderMaterials;

                    CacheContext.Serializer.Serialize(context, def);
                }
                else if (tag.Group.Tag.ToString() == "mode")
                {
                    var def = (RenderModel)definition;
                    def.Materials = renderMaterials;

                    CacheContext.Serializer.Serialize(context, def);
                }
            }

            if (debugMode)
            {
                var fileOut = new FileInfo($"ShadersOutput_{tagNameShort}.cs");
                if (File.Exists($"ShadersOutput_{tagNameShort}.cs"))
                    fileOut.Delete();

                using (var csvStream = fileOut.OpenWrite())
                using (var csvWriter = new StreamWriter(csvStream))
                {
                    foreach (var a in csvQueue)
                    {
                        csvStream.Position = csvStream.Length;
                        csvWriter.WriteLine(a);
                    }
                }
            }

            return true;
        }

        public static List<RenderMaterial> RestoreH3Shaders(Stream stream, GameCacheContext cacheContext, CacheFile blamCache, List<RenderMaterial> materials)
        {
            int i = -1;
            WriteLine("Default shaders:");
            foreach (var material in materials)
            {
                i++;
                if ((uint)material.RenderMethod.Index != 0xFFFFFFFF)
                {

                    var h3RenderMethod = blamCache.IndexItems.GetItemByID(material.RenderMethod.Index);
                    WriteLine($"[{i:D3}] [{h3RenderMethod.ClassCode}] {h3RenderMethod.Filename}");
                }
                else
                {
                    WriteLine($"[{i:D3}] [NULL]");
                }
            }

            i = -1;
            foreach (var material in materials)
            {
                i++;
                rmt2Preset = 0;
                rmPreset = 0;
                forceFunctions = false;
                edArgPreset = new Dictionary<string, float[]>();

                if (material.RenderMethod == null || material.RenderMethod.Index == -1)
                    continue;

                var renderMethodGroup = material.RenderMethod.Group.ToString();
                var h3RenderMethod = blamCache.IndexItems.GetItemByID(material.RenderMethod.Index);

                WriteLine("");
                WriteLine($"[{i:D3}] [{h3RenderMethod.ClassCode}] {h3RenderMethod.Filename}");

                // Hardcoded tag indexes or rmt2 tags, called presets
                GetShaderPresets(cacheContext, h3RenderMethod.Filename);

                // Disable rmbk and rmw for now
                switch (renderMethodGroup)
                {
                    case "rmbk":
                        material.RenderMethod = cacheContext.GetTag(0x3AB0);
                        continue;

                    case "rmw ":
                        material.RenderMethod = null;
                        continue;

                    case "rmhg": // maybe not. funny results occur half time
                        forceFunctions = true;
                        break;
                }

                // rmPreset and rmt2Preset possibly have a value now.
                if (rmPreset != 0x0)
                {
                    WriteLine($"[{i:D3}] [Using [{cacheContext.GetTag(rmPreset).Group}] 0x{rmPreset:X4}] {h3RenderMethod.Filename}");
                    material.RenderMethod = cacheContext.GetTag(rmPreset);
                    continue;
                }

                if (rmt2Preset != 0x0)
                {
                    WriteLine($"[{i:D3}] [Using preset rmt2 0x{rmt2Preset:X4}] {h3RenderMethod.Filename}");
                    material.RenderMethod = null;
                    goto label1;
                }

                material.RenderMethod = PortTagReference(cacheContext, blamCache, material.RenderMethod.Index);

                if (newOnly)
                    material.RenderMethod = null;

                if (existingOnly)
                {
                    if (material.RenderMethod == null)
                    {
                        material.RenderMethod = cacheContext.GetTag(0x3AB0);
                        WriteLine($"[{i:D3}] [{"No matches. Using 0x3AB0",-26}] {h3RenderMethod.Filename}");
                    }
                    continue;
                }

                label1:
                if (material.RenderMethod == null)
                {
                    material.RenderMethod = MatchShader7(stream, cacheContext, blamCache, h3RenderMethod);

                    if (material.RenderMethod == null)
                    {
                        material.RenderMethod = cacheContext.GetTag(0x3AB0);
                        WriteLine($"[{i:D3}] [{"Port failed. Using 0x3AB0",-26}] {h3RenderMethod.Filename}");
                    }
                    else
                    {
                        WriteLine($"[{i:D3}] [{"Using a new ported shader",-26}] {cacheContext.TagNames[material.RenderMethod.Index]}.{material.RenderMethod.Group.Tag}", true);
                    }
                }
                else
                {
                    WriteLine($"[{i:D3}] [{"Using existing shader",-26}] {cacheContext.TagNames[material.RenderMethod.Index]}.{material.RenderMethod.Group.Tag}", true);
                }
            }

            return materials;
        }

        public static RenderMethod ConvertDefinition(string tagClass, object h3Definition)
        {
            switch (tagClass)
            {
                case "cntl":
                    var cntl = (ContrailSystem)h3Definition;
                    return cntl.Contrail[0].RenderMethod;
                case "decs":
                    var decs = (DecalSystem)h3Definition;
                    return decs.DecalSystem2[0].RenderMethod;
                case "ltvl":
                    var ltvl = (LightVolumeSystem)h3Definition;
                    return ltvl.LightVolume[0].RenderMethod;
                case "prt3":
                    var prt3 = (Particle)h3Definition;
                    return prt3.RenderMethod;
                case "beam":
                    var beam = (BeamSystem)h3Definition;
                    return beam.Beam[0].RenderMethod;
                case "rmsh":
                    var rmsh = (Shader)h3Definition;
                    return rmsh;
                case "rmd":
                    var rmd = (ShaderDecal)h3Definition;
                    return rmd;
                case "rmhg":
                    var rmhg = (ShaderHalogram)h3Definition;
                    return rmhg;
                case "rmtr":
                    var rmtr = (ShaderTerrain)h3Definition;
                    return rmtr;
                case "rmcs":
                    var rmcs = (ShaderCustom)h3Definition;
                    return rmcs;
                case "rmfl":
                    var rmfl = (ShaderFoliage)h3Definition;
                    return rmfl;
                case "rmss":
                    var rmss = (ShaderScreen)h3Definition;
                    return rmss;
                case "rmw":
                    var rmw = (ShaderWater)h3Definition;
                    return rmw;
                case "rmzo":
                    var rmzo = (ShaderZonly)h3Definition;
                    return rmzo;
                // case "rmbk": // Disabled previously
                //     var rmbk = (ShaderBlack)h3Definition;
                //     return rmbk;

                default:
                    WriteLine($"ERROR: shader class/group not supported: {tagClass}");
                    return null;
            }

        }

        public static CachedTagInstance MatchShader7(Stream stream, GameCacheContext CacheContext, CacheFile BlamCache, CacheFile.IndexItem h3Tag)
        {
            var h3ShaderTag = BlamCache.IndexItems.GetItemByID(h3Tag.ID);
            var blamContext = new CacheSerializationContext(BlamCache, h3ShaderTag);

            var blamTagGroupChars = new char[] { ' ', ' ', ' ', ' ' };
            for (var i = 0; i < h3Tag.ClassCode.Length; i++)
                blamTagGroupChars[i] = h3Tag.ClassCode[i];

            object h3Definition = null;

            if (h3Tag.ClassCode == "rmd")
                h3Definition = BlamCache.Deserializer.Deserialize<ShaderDecal>(blamContext);
            // rmw was disabled previously
            // else if (h3Tag.ClassCode == "rmw")
            //     h3Definition = blamDeserializer.Deserialize<ShaderWater>(blamContext);
            else
                h3Definition = BlamCache.Deserializer.Deserialize(blamContext, TagDefinition.Find(h3Tag.ClassCode));

            var h3Shader = ConvertDefinition(h3Tag.ClassCode, h3Definition);

            if (h3Shader == null)
                return null;

            if (h3Shader.ShaderProperties.Count == 0)
            {
                WriteLine("ERROR: h3Shader.ShaderProperties = 0. Exception not implemented.");
                return null; // not implemented yet
            }

            // Deserialize blam rmt2
            var h3Rmt2Instance = BlamCache.IndexItems.GetItemByID(h3Shader.ShaderProperties[0].Template.Index);
            blamContext = new CacheSerializationContext(BlamCache, h3Rmt2Instance);
            var h3Rmt2 = BlamCache.Deserializer.Deserialize<RenderMethodTemplate>(blamContext);

            // Check for errors
            if (h3Rmt2 == null)
            {
                WriteLine($"This tag could not be deserialized: {h3Shader}, true");
                return null;
            }

            if (h3Rmt2.Arguments.Count == 0 || h3Rmt2.ShaderMaps.Count == 0)
            {
                WriteLine($"ERROR: Not implemented: h3Rmt2.Arguments.Count == 0 || h3Rmt2.ShaderMaps.Count == 0; Input:{h3Shader}");
                return null;
            }

            // Find the shader that uses this rmt2 tag. Proceed only for non hardcoded rmt2 tags
            if (rmPreset == 0 && rmt2Preset != 0)
            {
                foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup(new string(blamTagGroupChars)))
                {
                    if (instance.Dependencies.Contains(rmt2Preset))
                    {
                        rmPreset = instance.Index;
                        break;
                    }
                }
            }

            // Get a list of blam shader's bitmaps and arguments. rebundant
            h3Bitmaps = new List<string>();
            foreach (var a in h3Rmt2.ShaderMaps)
                h3Bitmaps.Add(BlamCache.Strings.GetItemByID((int)a.Name.Value));

            h3Arguments = new List<string>();
            foreach (var a in h3Rmt2.Arguments)
                h3Arguments.Add(BlamCache.Strings.GetItemByID((int)a.Name.Value));

            // Search for rmt2 tags with the same name; only search if no hardcoded values are provided
            // no matches found returns 0
            if (rmt2Preset == 0 && rmPreset == 0)
                rmt2Preset = MatchRenderMethodTemplateByName(CacheContext, BlamCache, h3Shader.ShaderProperties[0].Template);

            // Search for rmt2 tags with same or similar bitmaps count
            // no matches found returns 0
            if (rmt2Preset == 0 && rmPreset == 0)
                rmt2Preset = MatchRenderMethodTemplate2(stream, CacheContext, BlamCache, h3Bitmaps, h3Arguments, h3Rmt2Instance.Filename, h3Tag.Filename);

            // If this fails, return null which gets turned to 0x3AB0
            if (rmt2Preset == 0 && rmPreset == 0)
                return null;

            // Find the shader that uses this rmt2 tag. Proceed only for non hardcoded tags
            if (rmPreset == 0 && rmt2Preset != 0)
            {
                foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup(new string(blamTagGroupChars)))
                {
                    if (instance.Dependencies.Contains(rmt2Preset))
                    {
                        rmPreset = instance.Index;
                        break;
                    }
                }
            }

            // Check for errors
            if (rmPreset == 0)
            {
                WriteLine($"WARNING: Failed to find a shader with rmt2 0x{rmt2Preset:X4}", true);
                return CacheContext.GetTag(0x3AB0);
            }

            var edTag = CacheContext.GetTag(rmPreset);

            object edDefinition = null;
            
            edDefinition = CacheContext.Deserializer.Deserialize(new TagSerializationContext(stream, CacheContext, edTag), TagDefinition.Find(edTag.Group.Tag));

            // Check for errors
            if (edDefinition == null)
            {
                WriteLine($"WARNING: This tag could not be deserialized: 0x{rmPreset:X4}", true);
                return CacheContext.GetTag(0x3AB0);
            }

            var edShader = ConvertDefinition(edTag.Group.Tag.ToString(), edDefinition);

            // Check for errors
            if (edShader == null)
                return null;

            // Bigass check
            if (edShader.ShaderProperties.Count == 0)
                throw new Exception();

            // Deserialize ED rmt2.
            RenderMethodTemplate edRmt2;

            var edContext = new TagSerializationContext(stream, CacheContext, edShader.ShaderProperties[0].Template);
            edRmt2 = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(edContext);

            // Check for errors
            if (edRmt2 == null)
            {
                WriteLine($"ERROR: This tag could not be deserialized: 0x{rmPreset:X4}", true);
                return null;
            }

            // Get a list of ed shader's bitmaps and arguments. rebundant
            edBitmaps = new List<string>();
            foreach (var a in edRmt2.ShaderMaps)
                edBitmaps.Add(CacheContext.StringIdCache.GetString(a.Name));

            edArguments = new List<string>();
            foreach (var a in edRmt2.Arguments)
                edArguments.Add(CacheContext.StringIdCache.GetString(a.Name));

            /* Port the ShaderProperties tagblock with bitmaps and arguments
            var teloSgader = edShader.ShaderProperties[0];
            foreach (var a in teloSgader.ShaderMaps)
                if (a.Bitmap == null)
                    ;*/

            edShader.ShaderProperties[0] = PortShaderProperty(stream, CacheContext, BlamCache, h3Shader.ShaderProperties[0], edShader.ShaderProperties[0]);

            /*foreach (var a in edShader.ShaderProperties[0].ShaderMaps)
                if (a.Bitmap == null)
                    ;*/

            // Debug write to file
            if (debugMode)
            {
                WriteLine($"Bitmaps:");
                for (int edBitmI = 0; edBitmI < edShader.ShaderProperties[0].ShaderMaps.Count; edBitmI++)
                {
                    var edBitmName = CacheContext.TagNames.ContainsKey(edShader.ShaderProperties[0].ShaderMaps[edBitmI].Bitmap.Index) ?
                                CacheContext.TagNames[edShader.ShaderProperties[0].ShaderMaps[edBitmI].Bitmap.Index] :
                                $"0x{edShader.ShaderProperties[0].ShaderMaps[edBitmI].Bitmap.Index}:X4";

                    if (!h3Bitmaps.Contains(edBitmaps[edBitmI]))
                        WriteLine($"[{edBitmI:D2}] [{"Extra ED",-12}] 0x{edShader.ShaderProperties[0].ShaderMaps[edBitmI].Bitmap.Index:X4} {edBitmName}");

                    for (int blamBitmI = 0; blamBitmI < h3Shader.ShaderProperties[0].ShaderMaps.Count; blamBitmI++)
                    {
                        if (!edBitmaps.Contains(h3Bitmaps[blamBitmI]))
                            WriteLine($"[{edBitmI:D2}] [{"Missing H3",-12}] ###### {BlamCache.IndexItems.GetItemByID(h3Shader.ShaderProperties[0].ShaderMaps[blamBitmI].Bitmap.Index).Filename}");

                        if (edBitmaps[edBitmI] == h3Bitmaps[blamBitmI])
                        {
                            var blamBitmName = BlamCache.IndexItems.GetItemByID(h3Shader.ShaderProperties[0].ShaderMaps[blamBitmI].Bitmap.Index).Filename;

                            WriteLine($"[{edBitmI:D2}] {"",-12} 0x{edShader.ShaderProperties[0].ShaderMaps[edBitmI].Bitmap.Index:X4} {edBitmName}");
                            WriteLine($"[{edBitmI:D2}] {"",-12} ###### {blamBitmName}");
                        }
                    }
                }

                WriteLine($"Arguments:");
                for (int edBitmI = 0; edBitmI < edRmt2.Arguments.Count; edBitmI++)
                {
                    for (int blamBitmI = 0; blamBitmI < h3Rmt2.Arguments.Count; blamBitmI++)
                    {
                        var blamBitmapName = h3Arguments[blamBitmI];
                        var edBitmapName = edArguments[edBitmI];

                        if (edBitmapName == blamBitmapName)
                        {
                            var a = edShader.ShaderProperties[0].Arguments[edBitmI];
                            var b = h3Shader.ShaderProperties[0].Arguments[blamBitmI];
                            WriteLine($"ED:{edBitmapName} {a.Arg1}, {a.Arg2}, {a.Arg3}, {a.Arg4}");
                            WriteLine($"H3:{blamBitmapName} {b.Arg1}, {b.Arg2}, {b.Arg3}, {b.Arg4}");
                        }
                    }
                }
            }

            CachedTagInstance newTag = null;
            if (newTag == null)
                newTag = CacheContext.TagCache.AllocateTag(TagGroup.Instances[edTag.Group.Tag]);

            var context = new TagSerializationContext(stream, CacheContext, newTag);
            CacheContext.Serializer.Serialize(context, edDefinition);

            if (CacheContext.TagNames.ContainsKey(newTag.Index))
                CacheContext.TagNames[newTag.Index] = h3Tag.Filename;
            else
                CacheContext.TagNames.Add(newTag.Index, h3Tag.Filename);

            return newTag;
        }

        public static int MatchRenderMethodTemplateByName(GameCacheContext CacheContext, CacheFile BlamCache, CachedTagInstance blamRmt2)
        {
            // Get blamrmt2 name
            // Loop trough all ED tags till the ed filename contains blam rmt2 filename
            // Requires the tags to be named. Otherwise it will pick rmt2 tags based on bitmaps count
            // Would be good to check if the tags are named, if not, name all rmt2 tags.

            int edRmt2index = 0;

            var blamTagname = BlamCache.IndexItems.GetItemByID(blamRmt2.Index).Filename;

            WriteLine($"{blamTagname} (blam rmt2)");
            // Find rmt2 tags with the same name
            // 0x263A shaders\shader_templates\_0_0_0_1_0_0_1_3_0_0_0
            foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
            {
                if (!CacheContext.TagNames.ContainsKey(instance.Index))
                    continue;

                var edTagname = CacheContext.TagNames[instance.Index];

                if (blamTagname == edTagname)
                {
                    WriteLine($"{CacheContext.TagNames[instance.Index]} same rmt2 name");
                    return instance.Index;
                }
            }

            if (rmt2MatchOnly)
                return edRmt2index;

            // Find rmt2 tags with the same name except an extra _0 at the end
            // rmt2 name +1 digit seem to be ms23 shaders
            // 0x263A shaders\shader_templates\_0_0_0_1_0_0_1_3_0_0_0 ms23
            // 0x4D3F shaders\shader_templates\_0_0_0_1_0_0_1_3_0_0_0_0 ms30
            if (ms30Only) // rmt2 name +2 digits seem to be exclussive ms30 shaders
                goto label1;

            foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
            {
                if (!CacheContext.TagNames.ContainsKey(instance.Index))
                    continue;

                var edTagname = CacheContext.TagNames[instance.Index];

                if (blamTagname == edTagname.Substring(0, edTagname.Length - 2))
                {
                    WriteLine($"{CacheContext.TagNames[instance.Index]} Similar rmt2 name (-1)");
                    return instance.Index;
                }
            }

            // Find rmt2 tags with the same name except an extra _0_0 at the end
            // 0x2809 shaders\halogram_templates\_0_1_1_0_0_0_0
            // 0x5580 shaders\halogram_templates\_0_1_1_0_0_0_0_0_0 ; ok this is false, wtf me
            if (ms23Only)
                return edRmt2index;

            label1:
            foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
            {
                if (!CacheContext.TagNames.ContainsKey(instance.Index))
                    continue;

                var edTagname = CacheContext.TagNames.ContainsKey(instance.Index) ? CacheContext.TagNames[instance.Index] : throw new Exception("ERROR: Rmt2 tags are not named.");

                if (blamTagname == edTagname.Substring(0, edTagname.Length - 4))
                {
                    WriteLine($"{CacheContext.TagNames[instance.Index]} Similar rmt2 name (-2) (ms30)");
                    return instance.Index;
                }
            }

            WriteLine($"{blamTagname} None Found");

            // If no results are found, return 0.
            return edRmt2index;
        }

        public static void GetRmt2Info(Stream stream, GameCacheContext cacheContext)
        {
            if (cacheContext.Rmt2TagsInfo.Count == 0)
            {
                foreach (var instance in cacheContext.TagCache.Index.FindAllInGroup("rmt2"))
                {
                    var edContext = new TagSerializationContext(stream, cacheContext, instance);
                    var edRmt2 = cacheContext.Deserializer.Deserialize<RenderMethodTemplateEssentials>(edContext);

                    List<string> edBitmaps = new List<string>();
                    List<string> edArgs = new List<string>();

                    foreach (var ShaderMap in edRmt2.ShaderMaps)
                        edBitmaps.Add(cacheContext.StringIdCache.GetString(ShaderMap.Name));

                    foreach (var Argument in edRmt2.Arguments)
                        edArgs.Add(cacheContext.StringIdCache.GetString(Argument.Name));

                    cacheContext.Rmt2TagsInfo.Add(instance.Index, new List<List<string>> { edBitmaps, edArgs });
                }
            }
        }

        public static int MatchRenderMethodTemplate2(Stream stream, GameCacheContext cacheContext, CacheFile blamCache, List<string> h3Bitmaps, List<string> h3Args, string blamRmt2Name, string blamShaderName)
        {
            // Make a new dictionary with rmt2 of the same shader type
            List<ShaderTemplateItem> edRmt2SameType = new List<ShaderTemplateItem>();

            // Make a new dictionary with rmt2 with all the common bitmaps
            List<ShaderTemplateItem> edRmt2CommonBitmapsList = new List<ShaderTemplateItem>();

            // get blam rmt2 name/type to compare to ed rmt2 and use the same shader type, narrows down results nicely
            string blamRmt2Type = blamRmt2Name.Substring(
                blamRmt2Name.IndexOf("\\") + 1,
                blamRmt2Name.LastIndexOf("\\") - 8);

            // Loop only once trough all ED rmt2 tags and store them globally, + their bitmaps and arguments lists
            GetRmt2Info(stream, cacheContext);

            foreach (var instance in cacheContext.Rmt2TagsInfo)
            {
                if (!cacheContext.TagNames.ContainsKey(instance.Key))
                    continue;

                if (!cacheContext.TagNames[instance.Key].Contains(blamRmt2Type))
                    continue;

                int commonBitmaps = 0;
                int commonArgs = 0;
                int uncommonBitmaps = 0;
                int uncommonArgs = 0;
                int h3MissingBitmaps = 0;
                int h3MissingArgs = 0;

                foreach (var a in h3Bitmaps)
                {
                    if (instance.Value[(int)ShaderTemplateValueType.Bitmap].Contains(a))
                        commonBitmaps++;
                }

                foreach (var a in h3Args)
                {
                    if (instance.Value[(int)ShaderTemplateValueType.Parameter].Contains(a))
                        commonArgs++;
                }

                foreach (var a in h3Bitmaps)
                {
                    if (!instance.Value[(int)ShaderTemplateValueType.Bitmap].Contains(a))
                        h3MissingBitmaps++;
                }

                foreach (var a in h3Args)
                {
                    if (!instance.Value[(int)ShaderTemplateValueType.Parameter].Contains(a))
                        h3MissingArgs++;
                }

                foreach (var a in instance.Value[(int)ShaderTemplateValueType.Bitmap])
                {
                    if (!h3Bitmaps.Contains(a))
                        uncommonBitmaps++;
                }

                foreach (var a in instance.Value[(int)ShaderTemplateValueType.Parameter])
                {
                    if (!h3Args.Contains(a))
                        uncommonArgs++;
                }

                edRmt2SameType.Add(new ShaderTemplateItem
                {
                    ShaderTemplateTagIndex = instance.Key,
                    EdBitmapTagIndex = instance.Value[(int)ShaderTemplateValueType.Bitmap].Count,
                    EdParameterIndex = instance.Value[(int)ShaderTemplateValueType.Parameter].Count,
                    H3BitmapTagIndex = h3Bitmaps.Count,
                    H3ParameterIndex = h3Args.Count,
                    CommonBitmapCount = commonBitmaps,
                    CommonParameterCount = commonArgs,
                    UncommonBitmapCount = uncommonBitmaps,
                    UncommonParameterCount = uncommonArgs,
                    H3MissingBitmapCount = h3MissingBitmaps,
                    H3MissingParameterCount = h3MissingArgs
                });

            }

            if (cacheContext.Rmt2TagsInfo.Count == 0)
                return 0;

            if (edRmt2SameType.Count == 0)
                return 0;

            WriteLine("Matching rmt2 by type:");
            WriteLine("[rmt2] index_ comBitm comArg uncomBitm uncomArg missBitm missArg edBitm edArg h3Bitm h3Arg nameMatch tagname");
            WriteLine("[rmt2] index_ CB CA UB UA MB MA EB EA HB HA NM name");

            if (edRmt2SameType.Count == 0)
                return 0;

            // Filter in the shaders with the highest count of common bitmaps
            var bestCommonBitmaps = edRmt2SameType.OrderBy(x => x.CommonBitmapCount).Last().CommonBitmapCount;

            List<ShaderTemplateItem> list = new List<ShaderTemplateItem>();
            foreach (var b in edRmt2SameType)
            {
                if (b.CommonBitmapCount == bestCommonBitmaps)
                {
                    list.Add(new ShaderTemplateItem
                    {
                        ShaderTemplateTagIndex = b.ShaderTemplateTagIndex,
                        EdBitmapTagIndex = b.EdBitmapTagIndex,
                        EdParameterIndex = b.EdParameterIndex,
                        H3BitmapTagIndex = b.H3BitmapTagIndex,
                        H3ParameterIndex = b.H3ParameterIndex,
                        CommonBitmapCount = b.CommonBitmapCount,
                        CommonParameterCount = b.CommonParameterCount,
                        UncommonBitmapCount = b.UncommonBitmapCount,
                        UncommonParameterCount = b.UncommonParameterCount,
                        H3MissingBitmapCount = b.H3MissingBitmapCount,
                        H3MissingParameterCount = b.H3MissingParameterCount
                    });
                }
            }

            // Filter in the shaders with the highest count of common arguments
            var bestCommonArguments = list.OrderBy(x => x.CommonParameterCount).Last().CommonParameterCount;

            List<ShaderTemplateItem> list2 = new List<ShaderTemplateItem>();
            foreach (var b in list)
            {
                if (b.CommonParameterCount == bestCommonArguments)
                {
                    list2.Add(new ShaderTemplateItem
                    {
                        ShaderTemplateTagIndex = b.ShaderTemplateTagIndex,
                        EdBitmapTagIndex = b.EdBitmapTagIndex,
                        EdParameterIndex = b.EdParameterIndex,
                        H3BitmapTagIndex = b.H3BitmapTagIndex,
                        H3ParameterIndex = b.H3ParameterIndex,
                        CommonBitmapCount = b.CommonBitmapCount,
                        CommonParameterCount = b.CommonParameterCount,
                        UncommonBitmapCount = b.UncommonBitmapCount,
                        UncommonParameterCount = b.UncommonParameterCount,
                        H3MissingBitmapCount = b.H3MissingBitmapCount,
                        H3MissingParameterCount = b.H3MissingParameterCount
                    });
                }
            }

            // Filter out ms30 shaders (warning, this will break if previously ported shaders get ported to tag index < 0x4455 (first ms30 map))
            List<ShaderTemplateItem> list3 = new List<ShaderTemplateItem>();
            foreach (var b in list2)
            {
                if (b.ShaderTemplateTagIndex < 0x4455)
                {
                    list3.Add(new ShaderTemplateItem
                    {
                        ShaderTemplateTagIndex = b.ShaderTemplateTagIndex,
                        EdBitmapTagIndex = b.EdBitmapTagIndex,
                        EdParameterIndex = b.EdParameterIndex,
                        H3BitmapTagIndex = b.H3BitmapTagIndex,
                        H3ParameterIndex = b.H3ParameterIndex,
                        CommonBitmapCount = b.CommonBitmapCount,
                        CommonParameterCount = b.CommonParameterCount,
                        UncommonBitmapCount = b.UncommonBitmapCount,
                        UncommonParameterCount = b.UncommonParameterCount,
                        H3MissingBitmapCount = b.H3MissingBitmapCount,
                        H3MissingParameterCount = b.H3MissingParameterCount
                    });
                }
            }

            // If it failed to find any ms23 shaders, revert to the previous list
            if (list3.Count == 0)
                list3 = list2;

            // Filter in the shaders with the fewest extra bitmaps
            var uncommonBitm = list3.OrderBy(x => x.UncommonBitmapCount).First().UncommonBitmapCount;

            List<ShaderTemplateItem> list4 = new List<ShaderTemplateItem>();

            foreach (var b in list3)
            {
                if (b.UncommonBitmapCount == uncommonBitm)
                {
                    list4.Add(new ShaderTemplateItem
                    {
                        ShaderTemplateTagIndex = b.ShaderTemplateTagIndex,
                        EdBitmapTagIndex = b.EdBitmapTagIndex,
                        EdParameterIndex = b.EdParameterIndex,
                        H3BitmapTagIndex = b.H3BitmapTagIndex,
                        H3ParameterIndex = b.H3ParameterIndex,
                        CommonBitmapCount = b.CommonBitmapCount,
                        CommonParameterCount = b.CommonParameterCount,
                        UncommonBitmapCount = b.UncommonBitmapCount,
                        UncommonParameterCount = b.UncommonParameterCount,
                        H3MissingBitmapCount = b.H3MissingBitmapCount,
                        H3MissingParameterCount = b.H3MissingParameterCount
                    });
                }
            }

            // Filter in the shaders with the fewest extra bitmaps
            var uncomArg = list4.OrderBy(x => x.UncommonParameterCount).First().UncommonParameterCount;

            List<ShaderTemplateItem> list5 = new List<ShaderTemplateItem>();

            foreach (var b in list4)
            {
                if (b.UncommonParameterCount == uncomArg)
                {
                    list5.Add(new ShaderTemplateItem
                    {
                        ShaderTemplateTagIndex = b.ShaderTemplateTagIndex,
                        EdBitmapTagIndex = b.EdBitmapTagIndex,
                        EdParameterIndex = b.EdParameterIndex,
                        H3BitmapTagIndex = b.H3BitmapTagIndex,
                        H3ParameterIndex = b.H3ParameterIndex,
                        CommonBitmapCount = b.CommonBitmapCount,
                        CommonParameterCount = b.CommonParameterCount,
                        UncommonBitmapCount = b.UncommonBitmapCount,
                        UncommonParameterCount = b.UncommonParameterCount,
                        H3MissingBitmapCount = b.H3MissingBitmapCount,
                        H3MissingParameterCount = b.H3MissingParameterCount
                    });
                }
            }

            // Filter out rmt2 with the closest tagname
            List<ShaderTemplateItem> list6 = new List<ShaderTemplateItem>();
            foreach (var b in list5)
            {
                var a = blamRmt2Name;
                var c = cacheContext.TagNames[b.ShaderTemplateTagIndex];

                var d0 = a.Split("\\".ToCharArray()).Last();
                var e0 = c.Split("\\".ToCharArray()).Last();

                var d = d0.Split("_".ToCharArray());
                var e = e0.Split("_".ToCharArray());

                int j = 0;
                if (d.Length == e.Length)
                    j = e.Length;
                else
                    j = d.Length < e.Length ? d.Length : e.Length;

                int k = 0;
                for (int i = 0; i < j; i++)
                    if (d[i].ToString() != "0")
                        if (d[i].ToString() == e[i].ToString())
                            k++;

                list6.Add(new ShaderTemplateItem
                {
                    ShaderTemplateTagIndex = b.ShaderTemplateTagIndex,
                    NameIndex = k,
                    EdBitmapTagIndex = b.EdBitmapTagIndex,
                    EdParameterIndex = b.EdParameterIndex,
                    H3BitmapTagIndex = b.H3BitmapTagIndex,
                    H3ParameterIndex = b.H3ParameterIndex,
                    CommonBitmapCount = b.CommonBitmapCount,
                    CommonParameterCount = b.CommonParameterCount,
                    UncommonBitmapCount = b.UncommonBitmapCount,
                    UncommonParameterCount = b.UncommonParameterCount,
                    H3MissingBitmapCount = b.H3MissingBitmapCount,
                    H3MissingParameterCount = b.H3MissingParameterCount
                });
            }

            var _0 = list6.OrderBy(x => x.NameIndex).Last();

            if (debugMode)
            {
                WriteLine($"[BLAM] index_ CB CA UB UA MB MA EB EA HB HA NM {blamRmt2Name}");
                WriteLine($"[rmt2] 0x{_0.ShaderTemplateTagIndex:X4} " +
                    $"{_0.CommonBitmapCount:D2} " +
                    $"{_0.CommonParameterCount:D2} " +
                    $"{_0.UncommonBitmapCount:D2} " +
                    $"{_0.UncommonParameterCount:D2} " +
                    $"{_0.H3MissingBitmapCount:D2} " +
                    $"{_0.H3MissingParameterCount:D2} " +
                    $"{_0.EdBitmapTagIndex:D2} " +
                    $"{_0.EdParameterIndex:D2} " +
                    $"{_0.H3BitmapTagIndex:D2} " +
                    $"{_0.H3ParameterIndex:D2} " +
                    $"{_0.NameIndex:D2} " +
                    $"{cacheContext.TagNames[_0.ShaderTemplateTagIndex]}");

                WriteLine("Worse matches but name is less similar:");

                foreach (var _1 in list6)
                {
                    WriteLine($"[rmt2] 0x{_1.ShaderTemplateTagIndex:X4} " +
                        $"{_1.CommonBitmapCount:D2} " +
                        $"{_1.CommonParameterCount:D2} " +
                        $"{_1.UncommonBitmapCount:D2} " +
                        $"{_1.UncommonParameterCount:D2} " +
                        $"{_1.H3MissingBitmapCount:D2} " +
                        $"{_1.H3MissingParameterCount:D2} " +
                        $"{_1.EdBitmapTagIndex:D2} " +
                        $"{_1.EdParameterIndex:D2} " +
                        $"{_1.H3BitmapTagIndex:D2} " +
                        $"{_1.H3ParameterIndex:D2} " +
                        $"{_1.NameIndex:D2} " +
                        $"{cacheContext.TagNames[_1.ShaderTemplateTagIndex]}");
                }
            }

            WriteLine($"MatchShaders porttag {blamRmt2Type} {blamShaderName}");
            WriteLine($"[##] [rmt2 match] 0x{_0.ShaderTemplateTagIndex:X4} {cacheContext.TagNames[_0.ShaderTemplateTagIndex]} {blamShaderName}");

            return _0.ShaderTemplateTagIndex;
        }

        public static Shader.ShaderProperty PortShaderProperty(Stream stream, GameCacheContext cacheContext, CacheFile blamCache, Shader.ShaderProperty h3Property, Shader.ShaderProperty edProperty)
        {
            // Loop trough all the common bitmaps, match or port
            int blamBitmI2 = -1;
            for (int edBitmI = 0; edBitmI < edProperty.ShaderMaps.Count; edBitmI++)
            {
                blamBitmI2 = 0;
                for (int blamBitmI = 0; blamBitmI < h3Property.ShaderMaps.Count; blamBitmI++)
                {
                    blamBitmI2++;
                    var blamBitmapName = h3Bitmaps[blamBitmI];
                    var edBitmapName = edBitmaps[edBitmI];

                    if (h3Bitmaps[blamBitmI] == "environment_map") // WARNING: DEBUG: remove cubemaps until their bitm porting code is finished
                    {
                        h3Property.ShaderMaps[blamBitmI].Bitmap = cacheContext.GetTag(DefaultBitmaps("environment_map"));
                        continue;
                    }

                    if (edBitmapName == blamBitmapName)
                    {
                        edProperty.ShaderMaps[edBitmI].BitmapFlags = h3Property.ShaderMaps[blamBitmI].BitmapFlags;
                        edProperty.ShaderMaps[edBitmI].Unknown = h3Property.ShaderMaps[blamBitmI].Unknown;
                        edProperty.ShaderMaps[edBitmI].BitmapIndex = h3Property.ShaderMaps[blamBitmI].BitmapIndex;
                        edProperty.ShaderMaps[edBitmI].Unknown2 = h3Property.ShaderMaps[blamBitmI].Unknown2;
                        edProperty.ShaderMaps[edBitmI].BitmapFlags = h3Property.ShaderMaps[blamBitmI].BitmapFlags;
                        edProperty.ShaderMaps[edBitmI].UnknownBitmapIndexEnable = h3Property.ShaderMaps[blamBitmI].UnknownBitmapIndexEnable;
                        edProperty.ShaderMaps[edBitmI].UvArgumentIndex = h3Property.ShaderMaps[blamBitmI].UvArgumentIndex;
                        edProperty.ShaderMaps[edBitmI].Unknown3 = h3Property.ShaderMaps[blamBitmI].Unknown3;
                        edProperty.ShaderMaps[edBitmI].Unknown4 = h3Property.ShaderMaps[blamBitmI].Unknown4;

                        // match/port bitmaps
                        var blamBitm = h3Property.ShaderMaps[blamBitmI].Bitmap;
                        var edBitm = edProperty.ShaderMaps[edBitmI].Bitmap;

                        edProperty.ShaderMaps[edBitmI].Bitmap = PortTagReference(cacheContext, blamCache, blamBitm.Index);

                        if (edProperty.ShaderMaps[edBitmI].Bitmap == null)
                        {
                            var tagname = blamCache.IndexItems.GetItemByID(blamBitm.Index).Filename;

                            var porttag = new PortTagCommand(cacheContext, blamCache);
                            try
                            {
                                CacheFile.IndexItem blamTag = null;

                                foreach (var tag in blamCache.IndexItems)
                                {
                                    if ((tag.ClassCode == "bitm") && (tag.Filename == tagname))
                                    {
                                        blamTag = tag;
                                        break;
                                    }
                                }

                                edProperty.ShaderMaps[edBitmI].Bitmap = new PortTagCommand(cacheContext, blamCache).ConvertTag(stream, blamTag);
                            }
                            catch
                            {
                                WriteLine($"ERROR: required bitmap failed to port. Using Default bitmap.", true);
                                edProperty.ShaderMaps[edBitmI].Bitmap = cacheContext.GetTag(0x02B7);
                            }

                            if (!cacheContext.TagNames.ContainsValue(tagname))
                            {
                                WriteLine($"ERROR: required bitmap failed to port. Using Default bitmap.", true);
                                edProperty.ShaderMaps[edBitmI].Bitmap = edBitm;
                                continue;
                            }

                            foreach (var tag in cacheContext.TagCache.Index.FindAllInGroup("bitm"))
                            {
                                var tagName = cacheContext.TagNames.ContainsKey(tag.Index) ? cacheContext.TagNames[tag.Index] : null;
                                if (tagName == tagname)
                                {
                                    edProperty.ShaderMaps[edBitmI].Bitmap = tag;
                                    break;
                                }
                            }

                            if (edProperty.ShaderMaps[edBitmI].Bitmap == null)
                                edProperty.ShaderMaps[edBitmI].Bitmap = cacheContext.GetTag(DefaultBitmaps(blamBitmapName));
                        }
                    }
                }
            }

            // WARNING: DEBUG: remove bumpmaps
            if (noBumpMaps)
            {
                for (int blamBitmI = 0; blamBitmI < edProperty.ShaderMaps.Count; blamBitmI++)
                {
                    if (edBitmaps[blamBitmI].Contains("bump"))
                    {
                        WriteLine($"DEBUG: set ShaderMaps[{blamBitmI}].ShaderMap to default bumpmap 0x4417");
                        edProperty.ShaderMaps[blamBitmI].Bitmap = cacheContext.GetTag(0x4417);
                    }
                }
            }

            // Loop trough every ED bitmap. If blam shader doesn't have this bitmap, replace it with a default bitmap, to prevent ED's original bitmaps from showing up.
            for (int edBitmI = 0; edBitmI < edBitmaps.Count; edBitmI++)
            {
                var edBitmapName = edBitmaps[edBitmI];

                if (!h3Bitmaps.Contains(edBitmapName))    // Should use named tags and not tag indexes
                    edProperty.ShaderMaps[edBitmI].Bitmap = cacheContext.GetTag(DefaultBitmaps(edBitmapName));
            }

            // 
            // Arguments. WARNING: common rmt2 don't have the same arguments count and order
            // 

            // Manage Arguments
            int edArgIndex = -1;
            foreach (var edArg in edArguments)
            {
                edArgIndex++;
                int blamArgIndex = -1;
                foreach (var blamArg in h3Arguments)
                {
                    blamArgIndex++;
                    if (edArg == blamArg)
                    {
                        edProperty.Arguments[edArgIndex] = h3Property.Arguments[blamArgIndex];
                    }
                }
            }

            // For arguments that are missing from H3, set the average argument values.
            edArgIndex = -1;
            foreach (var edArg in edArguments)
            {
                edArgIndex++;
                if (!h3Arguments.Contains(edArg))
                {
                    var a = DefaultArgument(edArguments[edArgIndex]);
                    edProperty.Arguments[edArgIndex].Arg1 = a.Alpha;
                    edProperty.Arguments[edArgIndex].Arg2 = a.Red;
                    edProperty.Arguments[edArgIndex].Arg3 = a.Green;
                    edProperty.Arguments[edArgIndex].Arg4 = a.Blue;
                }
            }

            //
            // Null tagblocks that are crash potential or until functions are reversed, and each tagblock is reversed/understood, because common shaders have different tagblocks count in shaderproperties
            // Reenable for potential positive effects such as moving leaves, animated lights
            // 

            edProperty.Unknown2 = h3Property.Unknown2;
            edProperty.BitmapTransparency = h3Property.BitmapTransparency;
            edProperty.Unknown7 = h3Property.Unknown7;
            edProperty.Unknown8 = h3Property.Unknown8;
            edProperty.Unknown9 = h3Property.Unknown9;
            edProperty.Unknown10 = h3Property.Unknown10;
            edProperty.Unknown11 = h3Property.Unknown11;
            edProperty.Unknown12 = h3Property.Unknown12;
            edProperty.Unknown13 = h3Property.Unknown13;
            edProperty.Unknown14 = h3Property.Unknown14;
            edProperty.Unknown15 = h3Property.Unknown15;
            edProperty.Unknown16 = h3Property.Unknown16;

            if (!forceFunctions)
            {
                // TODO: see which does what and which is the real function
                edProperty.Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>();
                edProperty.DrawModes = new List<RenderMethod.ShaderProperty.DrawMode>();
                edProperty.Unknown3 = new List<RenderMethod.ShaderProperty.UnknownBlock3>();
                edProperty.ArgumentMappings = new List<RenderMethod.ShaderProperty.ArgumentMapping>();
                edProperty.Functions = new List<RenderMethod.ShaderProperty.FunctionBlock>();
            }

            if (edArgPreset == null)
                edArgPreset = new Dictionary<string, float[]>();

            //  edArgPreset.Add("env_roughness_scale", new float[] { 0.1f, 0, 0, 0 });
            foreach (var a in edArgPreset)
            {
                int i = -1;
                foreach (var b in edArguments)
                {
                    i++;
                    if (a.Key == b)
                    {
                        edProperty.Arguments[i].Arg1 = a.Value[0];
                        edProperty.Arguments[i].Arg2 = a.Value[1];
                        edProperty.Arguments[i].Arg3 = a.Value[2];
                        edProperty.Arguments[i].Arg4 = a.Value[3];
                        break;
                    }
                }
            }

            // DEBUG
            var rmt2 = $"0x{edProperty.Template.Index:X4}";
            // csv($"[???] [rmt2] [Used bitmap matching ] {rmt2} {cacheContext.TagNames[edProperty.Template.Index]}");

            return edProperty;
        }

        public static CachedTagInstance PortTagReference(GameCacheContext cacheContext, CacheFile blamCache, int index, int maxIndex = 0xFFFF)
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

                    if (instance.Filename == cacheContext.TagNames[tag.Index] && tag.Index < maxIndex)
                        return tag;
                }
            }

            return null;
        }

        public static void WriteLine(string output, bool visible = false)
        {
            if (debugMode || visible)
                Console.WriteLine(output);

            csvQueue.Add(output);
        }

        public static void GetShaderPresets(GameCacheContext CacheContext, string tagname)
        {
            edArgPreset = new Dictionary<string, float[]>();
            /*if (tagname.Contains("ext_metal_trim_red"))
                ;*/

            switch (tagname)
            {
                case "Example":
                    // Use a forced shader tag index
                    rmPreset = 0x3AB0;

                    // Use a forced shader tag name
                    rmPreset = ArgumentParser.ParseTagName(CacheContext, @"levels\multi\guardian\shaders\guardian_metal_b").Index;

                    // Use a forced rmt2 tag index
                    rmt2Preset = 0x02A7;

                    // Use a forced rmt2 tag name
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_0_0_0_0_0_0_3_0_0_0.rmt2").Index;

                    // Use forced shader arguments TODO
                    edArgPreset.Add("roughness", new float[4] { 0f, 1f, 2f, 3.4f });

                    // // Use a forced shader without updating the bitmaps
                    // newTag = CacheContext.GetTag(0x35BB);
                    break;

                // Snowbound Skybox:
                case @"levels\multi\snowbound\sky\shaders\skydome":
                    rmPreset = 0x35BB;
                    edArgPreset.Add("albedo_color", new float[] { 0.254901975f, 0.384313732f, 1f, 0f });
                    break;

                case @"levels\multi\snowbound\sky\shaders\aurora":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_0_0_0_0_0_0_3_0_0_0.rmt2").Index;
                    break;

                case @"levels\multi\snowbound\shaders\plasma_spire_a":
                case @"levels\multi\snowbound\sky\shaders\sun_clouds":
                case @"levels\multi\snowbound\sky\shaders\dust_clouds":
                case @"levels\multi\snowbound\sky\shaders\clouds_cirrus":
                    // forceFunctions = true; // MatchShader7 would always bring the function if the H3 shader has one
                    break;

                // Snowbound:
                case @"levels\multi\snowbound\shaders\cov_battery":
                case @"levels\multi\snowbound\shaders\cov_metalplates_icy":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_1_0_0_1_2_0_1_0_0_0.rmt2").Index;
                    edArgPreset.Add("albedo_color", new float[] { 0.8f, 0.2f, 0.3f, 1f }); // 2
                    edArgPreset.Add("env_tint_color", new float[] { 0.05f, 0.05f, 0.4f, 1f }); // 23
                    break;

                case @"levels\multi\snowbound\shaders\terrain_snow":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\terrain_templates\_0_0_1_1_0_0.rmt2").Index;
                    edArgPreset.Add("diffuse_coefficient_m_0", new float[] { 2f, 0, 0, 0 }); // 14
                    break;

                case @"levels\multi\snowbound\shaders\cov_glass_window_opaque":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_3_0_3_1_4_0_0_0_1_0.rmt2").Index;
                    edArgPreset.Add("env_roughness_scale", new float[] { 0.1f, 0, 0, 0 });
                    edArgPreset.Add("env_tint_color", new float[] { 0.02f, 0.01f, 0, 0 });
                    break;

                // Chillout/ Cold Storage
                case @"levels\dlc\chillout\shaders\chillout_floodroom02":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_1_0_0_1_4_9_0_0_1_0.rmt2").Index;
                    break;

                case @"levels\dlc\chillout\shaders\chillout_trim_a":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_1_0_1_1_2_1_0_0_0_0.rmt2").Index;
                    break;

                case @"levels\dlc\chillout\shaders\chillout_floor_glass_a":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_0_0_1_5_2_0_1_0_0_0_0.rmt2").Index;
                    break;

                case @"levels\dlc\chillout\shaders\chillout_floodroom03":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_3_0_0_1_4_0_0_0_0_0.rmt2").Index;
                    break;

                // Weapons
                // compass 0x0EEA
                // tens 0x0EEC
                // ones 0x0EEB
                case @"objects\weapons\rifle\assault_rifle\shaders\ones":
                case @"objects\weapons\rifle\battle_rifle\shaders\ones":
                    rmPreset = 0x0EEB;
                    // forceFunctions = true;
                    break;
                case @"objects\weapons\rifle\assault_rifle\shaders\tens":
                case @"objects\weapons\rifle\battle_rifle\shaders\tens":
                    rmPreset = 0x0EEC;
                    // forceFunctions = true;
                    break;
                case @"objects\weapons\rifle\assault_rifle\shaders\compass":
                    rmPreset = 0x0EEA;
                    // forceFunctions = true;
                    break;

                case @"objects\weapons\rifle\assault_rifle\shaders\assault_rifle":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_1_0_1_2_0_1_0_0_0_0.rmt2").Index;
                    break;

                case @"objects\weapons\rifle\battle_rifle\shaders\battle_rifle":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_2_0_1_2_2_1_0_0_0_0.rmt2").Index;
                    break;

                case @"objects\weapons\rifle\smg\shaders\smg_metal":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_2_0_1_2_1_1_0_1_0_0.rmt2").Index;
                    break;

                case @"objects\weapons\rifle\shotgun\shaders\shotgun":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_2_0_1_2_1_1_0_1_0_0.rmt2").Index;
                    break;

                case @"objects\weapons\rifle\sniper_rifle\shaders\sniper_rifle_metal":
                    rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_2_0_1_2_1_1_0_1_0_0.rmt2").Index;
                    break;

                case @"levels\atlas\sc140\shaders\ext_metal_trim_red":
                case @"levels\atlas\sc140\shaders\ext_wall_column":
                case @"levels\atlas\sc140\shaders\glass_light_b_blue":
                case @"levels\atlas\sc140\shaders\glass_light_b_blue_off":
                case @"levels\atlas\sc140\shaders\mc_stairs_ext":
                case @"levels\atlas\sc140\shaders\wall_ext":
                case @"levels\atlas\sc140\shaders\wall_ext_2":
                case @"levels\atlas\sc140\shaders\wall_ext_center":
                case @"levels\atlas\shared\shaders\light_white":
                case @"levels\dlc\spacecamp\shaders\glass_light_a":
                case @"levels\dlc\spacecamp\shaders\glass_light_b_blue":
                case @"levels\dlc\spacecamp\shaders\glass_light_sign_d":
                    edArgPreset.Add("specular_coefficient", new float[] { 0, 0, 0, 0 });
                    break;

                default:
                    break;
            }

            if (tagname.Contains("\\z_"))
                rmPreset = 0x3AB0;
        }

        public static int DefaultBitmaps(string type)
        {
            switch (type)
            {
                case "base_map":
                case "base_map_m_0":
                case "base_map_m_1":
                case "base_map_m_2":
                case "detail_map":
                case "detail_map2":
                case "detail_map3":
                case "detail_map_a":
                case "detail_map_m_0":
                case "detail_map_m_1":
                case "detail_map_m_2":
                case "detail_map_m_3":
                case "overlay_detail_map":
                case "detail_map_overlay":
                case "alpha_map":
                case "blend_map":
                case "meter_map":
                case "subsurface_map":
                case "self_illum_detail_map":
                case "warp_map":
                    return 0x02B7;

                case "change_color_map":
                case "material_texture":
                case "noise_map_a":
                case "noise_map_b":
                case "overlay_map":
                case "bump_detail_mask_map":
                case "chameleon_mask_map":
                case "environment_map":
                case "specular_map":
                case "specular_mask_texture":
                case "transparence_map":
                    return 0x02B9;

                case "color_mask_map":
                    return 0x0375;

                case "bump_map":
                case "bump_map_m_0":
                case "bump_map_m_1":
                case "bump_map_m_2":
                case "bump_map_m_3":
                case "detail_bump_m_0":
                case "detail_bump_m_1":
                case "detail_bump_m_2":
                case "detail_bump_m_3":
                case "bump_detail_map":
                case "alpha_test_map":
                case "height_map":
                case "overlay_multiply_map":
                    return 0x0376; // WARNING engine's toast, anything bump map related is busted

                case "self_illum_map":
                    return 0x0377;

                case "detail_mask_a":
                    return 0x037A;

                case "alpha_mask_map":
                    return 0x037B;

                default:
                    Console.WriteLine($"WARNING: unknown bitmap type for Halo Online: {type}. Using 0x02B7");
                    return 0x02B7;
                    // throw new NotImplementedException($"Useless exception. Bitmaps table requires an update for {type}.");
                    // Better replace with return 0x02B7;
            }
        }

        public static RealArgbColor DefaultArgument(string arg)
        {
            switch (arg)
            {
                // Default argument values based on how frequently they appear in shaders, so I assumed it as an average argument value.
                case "add_color": return new RealArgbColor(0.6f, 0.6f, 0.6f, 0f);
                case "ambient_coefficient": return new RealArgbColor(0.1f, 0.1f, 0.1f, 0.1f);
                case "analytical_specular_coefficient": return new RealArgbColor(0.03f, 0.03f, 0.03f, 0.03f);
                case "analytical_specular_contribution": return new RealArgbColor(0.5f, 0.5f, 0.5f, 0.5f);
                case "analytical_specular_contribution_m_0": return new RealArgbColor(0.5f, 0.5f, 0.5f, 0.5f);
                case "analytical_specular_contribution_m_1": return new RealArgbColor(0.5f, 0.5f, 0.5f, 0.5f);
                case "analytical_specular_contribution_m_2": return new RealArgbColor(0.5f, 0.5f, 0.5f, 0.5f);
                case "analytical_specular_contribution_m_3": return new RealArgbColor(0.5f, 0.5f, 0.5f, 0.5f);
                case "animation_amplitude_horizontal": return new RealArgbColor(0.04f, 0.04f, 0.04f, 0.04f);
                case "antialias_tweak": return new RealArgbColor(0.025f, 0.025f, 0.025f, 0.025f);
                case "area_specular_coefficient": return new RealArgbColor(0.01f, 0.01f, 0.01f, 0.01f);
                case "area_specular_contribution_m_0": return new RealArgbColor(0.1f, 0.1f, 0.1f, 0.1f);
                case "area_specular_contribution_m_1": return new RealArgbColor(0.2f, 0.2f, 0.2f, 0.2f);
                case "area_specular_contribution_m_2": return new RealArgbColor(0.1f, 0.1f, 0.1f, 0.1f);
                case "area_specular_contribution_m_3": return new RealArgbColor(0.1f, 0.1f, 0.1f, 0.1f);
                case "bankalpha_infuence_depth": return new RealArgbColor(0.27f, 0.27f, 0.27f, 0.27f);
                case "base_map_m_3": return new RealArgbColor(15f, 30f, 0f, 0f);
                case "blend_mode": return new RealArgbColor(8f, 8f, 8f, 8f);
                case "bump_map_m_0": return new RealArgbColor(50f, 50f, 0f, 0f);
                case "bump_map_m_2": return new RealArgbColor(50f, 50f, 0f, 0f);
                case "bump_map_m_3": return new RealArgbColor(50f, 50f, 0f, 0f);
                case "chameleon_color0": return new RealArgbColor(0.627451f, 0.3098039f, 0.7803922f, 1f);
                case "chameleon_color1": return new RealArgbColor(0.3254902f, 0.2745098f, 0.8431373f, 1f);
                case "chameleon_color2": return new RealArgbColor(1f, 1f, 0.5843138f, 1f);
                case "chameleon_color3": return new RealArgbColor(0.5529412f, 0.7137255f, 0.572549f, 1f);
                case "chameleon_color_offset1": return new RealArgbColor(0.3333f, 0.3333f, 0.3333f, 0.3333f);
                case "chameleon_color_offset2": return new RealArgbColor(0.6666f, 0.6666f, 0.6666f, 0.6666f);
                case "channel_a": return new RealArgbColor(0.9254903f, 0.4862745f, 0.01960784f, 2.147059f);
                case "channel_b": return new RealArgbColor(0.2784314f, 0.04705883f, 0.04705883f, 2.411765f);
                case "channel_c": return new RealArgbColor(0.5490196f, 0.8588236f, 1f, 8f);
                case "choppiness_backward": return new RealArgbColor(3f, 3f, 3f, 3f);
                case "choppiness_forward": return new RealArgbColor(2f, 2f, 2f, 2f);
                case "choppiness_side": return new RealArgbColor(3f, 3f, 3f, 3f);
                case "color_medium": return new RealArgbColor(0f, 0f, 0f, 1f);
                case "color_sharp": return new RealArgbColor(0.2156863f, 0.6745098f, 1f, 1f);
                case "color_wide": return new RealArgbColor(0f, 0f, 0f, 1f);
                case "depth_fade_range": return new RealArgbColor(0.1f, 0.1f, 0.1f, 0.1f);
                case "detail_fade_a": return new RealArgbColor(0.2f, 0.2f, 0.2f, 0.2f);
                case "detail_map3": return new RealArgbColor(6.05f, 6.05f, 0.6f, 0f);
                case "detail_map_a": return new RealArgbColor(0.8140022f, 1.628004f, 43.13726f, 12.31073f);
                case "detail_map_m_2": return new RealArgbColor(100f, 100f, 0f, 0f);
                case "detail_multiplier_a": return new RealArgbColor(4.59479f, 4.59479f, 4.59479f, 4.59479f);
                case "detail_slope_scale_x": return new RealArgbColor(8f, 8f, 8f, 8f);
                case "detail_slope_scale_y": return new RealArgbColor(8f, 8f, 8f, 8f);
                case "detail_slope_scale_z": return new RealArgbColor(3f, 3f, 3f, 3f);
                case "detail_slope_steepness": return new RealArgbColor(0.3f, 0.3f, 0.3f, 0.3f);
                case "displacement_range_x": return new RealArgbColor(0.14f, 0.14f, 0.14f, 0.14f);
                case "displacement_range_y": return new RealArgbColor(0.07f, 0.07f, 0.07f, 0.07f);
                case "displacement_range_z": return new RealArgbColor(0.14f, 0.14f, 0.14f, 0.14f);
                case "distortion_scale": return new RealArgbColor(2f, 2f, 2f, 2f);
                case "edge_fade_edge_tint": return new RealArgbColor(0f, 0f, 0f, 1f);
                case "environment_specular_contribution_m_2": return new RealArgbColor(0.4f, 0.4f, 0.4f, 0.4f);
                case "foam_height": return new RealArgbColor(0.1f, 0.1f, 0.1f, 0.1f);
                case "foam_pow": return new RealArgbColor(2f, 2f, 2f, 2f);
                case "foam_texture": return new RealArgbColor(5f, 4f, 1f, 1f);
                case "foam_texture_detail": return new RealArgbColor(1.97f, 1.377f, 1f, 0f);
                case "fresnel_coefficient": return new RealArgbColor(0.25f, 0.25f, 0.25f, 0.25f);
                case "fresnel_color": return new RealArgbColor(0.5019608f, 0.5019608f, 0.5019608f, 1f);
                case "fresnel_color_environment": return new RealArgbColor(0.5019608f, 0.5019608f, 0.5019608f, 1f);
                case "fresnel_curve_steepness": return new RealArgbColor(5f, 5f, 5f, 5f);
                case "fresnel_curve_steepness_m_0": return new RealArgbColor(5f, 5f, 5f, 5f);
                case "fresnel_curve_steepness_m_1": return new RealArgbColor(5f, 5f, 5f, 5f);
                case "fresnel_curve_steepness_m_2": return new RealArgbColor(5f, 5f, 5f, 5f);
                case "fresnel_curve_steepness_m_3": return new RealArgbColor(5f, 5f, 5f, 5f);
                case "glancing_specular_power": return new RealArgbColor(10f, 10f, 10f, 10f);
                case "global_shape": return new RealArgbColor(2f, 2f, 2f, 2f);
                case "globalshape_infuence_depth": return new RealArgbColor(0.2f, 0.2f, 0.2f, 0.2f);
                case "height_scale": return new RealArgbColor(0.02f, 0.02f, 0.02f, 0.02f);
                case "layer_contrast": return new RealArgbColor(4f, 4f, 4f, 4f);
                case "layer_depth": return new RealArgbColor(0.3f, 0.3f, 0.3f, 0.3f);
                case "layers_of_4": return new RealArgbColor(4f, 4f, 4f, 4f);
                case "meter_color_off": return new RealArgbColor(0f, 0f, 0f, 1f);
                case "meter_color_on": return new RealArgbColor(0.1333333f, 1f, 0.1686275f, 1f);
                case "minimal_wave_disturbance": return new RealArgbColor(0.2f, 0.2f, 0.2f, 0.2f);
                case "normal_specular_power": return new RealArgbColor(10f, 10f, 10f, 10f);
                case "reflection_coefficient": return new RealArgbColor(30f, 30f, 30f, 30f);
                case "refraction_extinct_distance": return new RealArgbColor(30f, 30f, 30f, 30f);
                case "refraction_texcoord_shift": return new RealArgbColor(0.12f, 0.12f, 0.12f, 0.12f);
                case "rim_coefficient": return new RealArgbColor(0.5f, 0.5f, 0.5f, 0.5f);
                case "rim_fresnel_power": return new RealArgbColor(2f, 2f, 2f, 2f);
                case "rim_power": return new RealArgbColor(8f, 8f, 8f, 8f);
                case "rim_start": return new RealArgbColor(0.4f, 0.4f, 0.4f, 0.4f);
                case "rim_tint": return new RealArgbColor(0.3215686f, 0.3843138f, 0.5450981f, 1f);
                case "roughness": return new RealArgbColor(0.27f, 0.27f, 0.27f, 0.27f);
                case "self_illum_heat_color": return new RealArgbColor(0.2392157f, 0.6470588f, 1f, 1f);
                case "self_illum_intensity": return new RealArgbColor(3f, 3f, 3f, 3f);
                case "shadow_intensity_mark": return new RealArgbColor(0.5f, 0.5f, 0.5f, 0.5f);
                case "slope_range_x": return new RealArgbColor(1.39f, 1.39f, 1.39f, 1.39f);
                case "slope_range_y": return new RealArgbColor(0.84f, 0.84f, 0.84f, 0.84f);
                case "specular_power": return new RealArgbColor(25f, 25f, 25f, 25f);
                case "specular_power_m_0": return new RealArgbColor(10f, 10f, 10f, 10f);
                case "specular_power_m_1": return new RealArgbColor(10f, 10f, 10f, 10f);
                case "specular_power_m_2": return new RealArgbColor(10f, 10f, 10f, 10f);
                case "specular_power_m_3": return new RealArgbColor(10f, 10f, 10f, 10f);
                case "specular_tint_m_2": return new RealArgbColor(0.1764706f, 0.1372549f, 0.09411766f, 1f);
                case "subsurface_coefficient": return new RealArgbColor(0.9f, 0.9f, 0.9f, 0.9f);
                case "subsurface_propagation_bias": return new RealArgbColor(0.66f, 0.66f, 0.66f, 0.66f);
                case "sunspot_cut": return new RealArgbColor(0.01f, 0.01f, 0.01f, 0.01f);
                case "thinness_medium": return new RealArgbColor(16f, 16f, 16f, 16f);
                case "thinness_sharp": return new RealArgbColor(32f, 32f, 32f, 32f);
                case "thinness_wide": return new RealArgbColor(4f, 4f, 4f, 4f);
                case "transparence_coefficient": return new RealArgbColor(0.3f, 0.3f, 0.3f, 0.3f);
                case "transparence_normal_bias": return new RealArgbColor(-1f, -1f, -1f, -1f);
                case "transparence_tint": return new RealArgbColor(0.8705883f, 0.8470589f, 0.6941177f, 1f);
                case "vector_sharpness": return new RealArgbColor(1000f, 1000f, 1000f, 1000f);
                case "warp_amount": return new RealArgbColor(0.005f, 0.005f, 0.005f, 0.005f);
                case "warp_map": return new RealArgbColor(8f, 5f, 0f, 0f);
                case "water_color_pure": return new RealArgbColor(0.03529412f, 0.1333333f, 0.1294118f, 1f);
                case "water_diffuse": return new RealArgbColor(0.05490196f, 0.08627451f, 0.09803922f, 1f);
                case "water_murkiness": return new RealArgbColor(12f, 12f, 12f, 12f);
                case "watercolor_coefficient": return new RealArgbColor(0.35f, 0.35f, 0.35f, 0.35f);
                case "wave_displacement_array": return new RealArgbColor(1.7779f, 1.7779f, 0f, 0f);
                case "wave_height": return new RealArgbColor(0.5f, 0.5f, 0.5f, 0.5f);
                case "wave_height_aux": return new RealArgbColor(0.3f, 0.3f, 0.3f, 0.3f);
                case "wave_slope_array": return new RealArgbColor(0.7773f, 1.3237f, 0f, 0f);
                case "wave_visual_damping_distance": return new RealArgbColor(8f, 8f, 8f, 8f);

                case "albedo":
                case "albedo_blend":
                case "albedo_blend_with_specular_tint":
                case "albedo_specular_tint_blend":
                case "albedo_specular_tint_blend_m_0":
                case "albedo_specular_tint_blend_m_1":
                case "albedo_specular_tint_blend_m_2":
                case "albedo_specular_tint_blend_m_3":
                case "analytical_anti_shadow_control":
                case "area_specular_contribution":
                case "environment_map_coefficient":
                case "environment_specular_contribution_m_0":
                case "environment_specular_contribution_m_1":
                case "environment_specular_contribution_m_3":
                case "fog":
                case "frame_blend":
                case "fresnel_curve_bias":
                case "invert_mask":
                case "lighting":
                case "meter_value":
                case "no_dynamic_lights":
                case "order3_area_specular":
                case "primary_change_color_blend":
                case "refraction_depth_dominant_ratio":
                case "rim_fresnel_albedo_blend":
                case "rim_fresnel_coefficient":
                case "rim_maps_transition_ratio":
                case "self_illumination":
                case "specialized_rendering":
                case "specular_coefficient_m_3":
                case "subsurface_normal_detail":
                case "time_warp":
                case "time_warp_aux":
                case "use_fresnel_color_environment":
                case "warp_amount_x":
                case "warp_amount_y":
                case "waveshape":
                case "env_tint_color":
                    return new RealArgbColor(0f, 0f, 0f, 0f);

                case "alpha_map":
                case "alpha_mask_map":
                case "alpha_test_map":
                case "base_map":
                case "base_map_m_0":
                case "base_map_m_1":
                case "base_map_m_2":
                case "blend_map":
                case "bump_detail_map":
                case "bump_detail_mask_map":
                case "bump_map":
                case "bump_map_m_1":
                case "chameleon_mask_map":
                case "change_color_map":
                case "color_mask_map":
                case "detail_bump_m_0":
                case "detail_bump_m_1":
                case "detail_bump_m_2":
                case "detail_bump_m_3":
                case "detail_map":
                case "detail_map2":
                case "detail_map_m_0":
                case "detail_map_m_1":
                case "detail_map_m_3":
                case "detail_map_overlay":
                case "detail_mask_a":
                case "height_map":
                case "material_texture":
                case "meter_map":
                case "multiply_map":
                case "noise_map_a":
                case "noise_map_b":
                case "overlay_detail_map":
                case "overlay_map":
                case "overlay_multiply_map":
                case "self_illum_detail_map":
                case "self_illum_map":
                case "specular_mask_texture":
                    return new RealArgbColor(1f, 1f, 0f, 0f);

                case "albedo_color":
                case "albedo_color2":
                case "albedo_color3":
                case "ambient_tint":
                case "bump_detail_coefficient":
                case "chameleon_fresnel_power":
                case "depth_darken":
                case "diffuse_coefficient":
                case "diffuse_coefficient_m_0":
                case "diffuse_coefficient_m_1":
                case "diffuse_coefficient_m_2":
                case "diffuse_coefficient_m_3":
                case "diffuse_tint":
                case "edge_fade_center_tint":
                case "edge_fade_power":
                case "ending_uv_scale":
                case "env_roughness_scale":
                case "environment_map_specular_contribution":
                case "environment_map_tint":
                case "final_tint":
                case "fresnel_power":
                case "glancing_specular_tint":
                case "global_albedo_tint":
                case "intensity":
                case "modulation_factor":
                case "neutral_gray":
                case "normal_specular_tint":
                case "overlay_intensity":
                case "overlay_tint":
                case "rim_fresnel_color":
                case "self_illum_color":
                case "specular_coefficient":
                case "specular_coefficient_m_0":
                case "specular_coefficient_m_1":
                case "specular_coefficient_m_2":
                case "specular_tint":
                case "specular_tint_m_0":
                case "specular_tint_m_1":
                case "specular_tint_m_3":
                case "starting_uv_scale":
                case "subsurface_tint":
                case "texcoord_aspect_ratio":
                case "tint_color":
                case "transparence_normal_detail":
                case "u_tiles":
                case "v_tiles":
                    return new RealArgbColor(1f, 1f, 1f, 1f);

                default:
                    return new RealArgbColor(1, 1, 1, 1);
            }

        }

        public class ShaderTemplateItem
        {
            public int ShaderTemplateTagIndex;
            public int NameIndex;
            public int EdBitmapTagIndex;
            public int EdParameterIndex;
            public int H3BitmapTagIndex;
            public int H3ParameterIndex;
            public int CommonBitmapCount;
            public int CommonParameterCount;
            public int UncommonBitmapCount;
            public int UncommonParameterCount;
            public int H3MissingBitmapCount;
            public int H3MissingParameterCount;
        }

        public enum ShaderTemplateValueType : int
        {
            Bitmap = 0,
            Parameter = 1
        }

        public class ArgumentItem
        {
            public float Arg1 = 0.0f;
            public float Arg2 = 0.0f;
            public float Arg3 = 0.0f;
            public float Arg4 = 0.0f;
        }

        [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp")]
        public class ScenarioStructureBspMaterials // used to deserialize as fast as possible
        {
            [TagField(Padding = true, Length = 0xC0, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
            public byte[] Padding = new byte[0xC0];

            [TagField(Padding = true, Length = 0xC4, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public byte[] Padding3 = new byte[0xC4];

            [TagField(Padding = true, Length = 0xD0, MinVersion = CacheVersion.HaloOnline106708)]
            public byte[] Padding2 = new byte[0x10];

            public List<RenderMaterial> Materials = new List<RenderMaterial>();
        }

        [TagStructure(Name = "render_method_template", Tag = "rmt2")]
        public class RenderMethodTemplateEssentials // used to deserialize as fast as possible
        {
            [TagField(Length = 18)]
            public uint[] Unknown00 = new uint[18];

            public List<Argument> Arguments = new List<Argument>();

            [TagField(Length = 6)]
            public uint[] Unknown02 = new uint[6];

            public List<ShaderMap> ShaderMaps = new List<ShaderMap>();

            [TagStructure]
            public class Argument
            {
                public StringId Name = StringId.Invalid;
            }

            [TagStructure]
            public class ShaderMap
            {
                public StringId Name = StringId.Invalid;
            }
        }
    }
}