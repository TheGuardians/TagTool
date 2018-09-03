using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Damage;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.Serialization;
using TagTool.Shaders;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    public partial class PortTagCommand : Command
    {
        private const PortingFlags DefaultFlags = PortingFlags.Recursive | PortingFlags.Scripts | PortingFlags.MatchShaders;

        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache;
        private RenderGeometryConverter GeometryConverter { get; }
        private PortingFlags Flags { get; set; }

        private Dictionary<Tag, List<string>> ReplacedTags = new Dictionary<Tag, List<string>>();

        private List<Tag> RenderMethodTagGroups = new List<Tag> { new Tag("rmbk"), new Tag("rmcs"), new Tag("rmd "), new Tag("rmfl"), new Tag("rmhg"), new Tag("rmsh"), new Tag("rmss"), new Tag("rmtr"), new Tag("rmw "), new Tag("rmrd"), new Tag("rmct") };
        private List<Tag> EffectTagGroups = new List<Tag> { new Tag("beam"), new Tag("cntl"), new Tag("ltvl"), new Tag("decs"), new Tag("prt3") };

        private static readonly string[] DoNotReplaceGroups = new[]
        {
            "glps",
            "glvs",
            "vtsh",
            "pixl",
            "rmdf",
            "rmt2"
        };

        [Flags]
        public enum PortingFlags
        {
            None,
            Replace = 1 << 0,
            Recursive = 1 << 1,
            New = 1 << 2,
            UseNull = 1 << 3,
            NoAudio = 1 << 4,
            NoElites = 1 << 5,
            NoForgePalette = 1 << 6,
            NoSquads = 1 << 7,
            Scripts = 1 << 8,
            ShaderTest = 1 << 9,
            MatchShaders = 1 << 10,
            Memory = 1 << 11,
            NoRmhg = 1 << 12,
            NoMs30 = 1 << 13
        }

        public PortTagCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(true,

                "PortTag",

                "Ports a tag from the current cache file. Options are:" + Environment.NewLine +
                "    Replace, Recursive, Single, New, UseNull, NoAudio, NoElites, NoForgePalette, " + 
                "NoSquads, Scripts, NoScripts, ShaderTest, MatchShaders, NoShaders" + Environment.NewLine + Environment.NewLine +

                "Replace: Replace tags of the same name when porting." + Environment.NewLine +
                "Recursive: Recursively port all tag references available." + Environment.NewLine +
                "Single: Port the specified tag instance using only existing tag references if available." + Environment.NewLine +
                "New: Create a new tag after the last index." + Environment.NewLine +
                "UseNull: Port a tag using nulled tag indices where available." + Environment.NewLine +
                "NoAudio: Ports everything except for audio tags unless existing tags are available." + Environment.NewLine +
                "NoElites: Ports everything except elite bipeds." + Environment.NewLine +
                "NoForgePalette: Clears the forge palette of any scenario tag when porting." + Environment.NewLine +
                "NoSquads: Clears the squads palette of any scenario tag when porting." + Environment.NewLine +
                "Scripts: Ports and adjusts scripts where possible." + Environment.NewLine +
                "NoScripts: Clears the scripts of any scenario tag when porting." + Environment.NewLine +
                "ShaderTest: TBD." + Environment.NewLine +
                "MatchShaders: Attempts to match any shader tags using existing render method tags when porting." + Environment.NewLine +
                "NoShaders: Uses default shader tags when porting." + Environment.NewLine +
                "Memory: Keeps cache in memory until the porting process is complete.",

                "PortTag [Options] <Tag>",

                "")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            GeometryConverter = new RenderGeometryConverter(cacheContext, blamCache);
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return false;

            Flags = DefaultFlags;

            var flagNames = Enum.GetNames(typeof(PortingFlags)).Select(name => name.ToLower());
            var flagValues = Enum.GetValues(typeof(PortingFlags)) as PortingFlags[];

            while (args.Count > 1)
            {
                var arg = args[0].ToLower();

                switch (arg)
                {
                    case "single":
                        Flags &= ~PortingFlags.Recursive;
                        break;

                    case "noscripts":
                        Flags &= ~PortingFlags.Scripts;
                        break;

                    case "noshaders":
                        Flags &= ~PortingFlags.MatchShaders;
                        break;

                    case "replace":
                        Flags |= PortingFlags.Replace;
                        break;

                    default:
                        {
                            if (!flagNames.Contains(arg))
                            {
                                Flags = DefaultFlags;
                                throw new FormatException($"Invalid {typeof(PortingFlags).FullName}: {args[0]}");
                            }

                            for (var i = 0; i < flagNames.Count(); i++)
                                if (arg == flagNames.ElementAt(i))
                                    Flags |= flagValues[i];
                        }
                        break;
                }

                args.RemoveAt(0);
            }

            var initialStringIdCount = CacheContext.StringIdCache.Strings.Count;

            //
            // Convert Blam data to ElDorado data
            //

            var resourceStreams = new Dictionary<ResourceLocation, Stream>();

            using (var cacheStream = Flags.HasFlag(PortingFlags.Memory) ? new MemoryStream() : (Stream)CacheContext.OpenTagCacheReadWrite())
            {
                if (Flags.HasFlag(PortingFlags.Memory))
                    using (var cacheFileStream = CacheContext.OpenTagCacheRead())
                        cacheFileStream.CopyTo(cacheStream);

                var oldFlags = Flags;

                foreach (var blamTag in ParseLegacyTag(args[0]))
                {
                    ConvertTag(cacheStream, resourceStreams, blamTag);
                    Flags = oldFlags;
                }

                if (Flags.HasFlag(PortingFlags.Memory))
                    using (var cacheFileStream = CacheContext.OpenTagCacheReadWrite())
                    {
                        cacheFileStream.Seek(0, SeekOrigin.Begin);
                        cacheFileStream.SetLength(cacheFileStream.Position);

                        cacheStream.Seek(0, SeekOrigin.Begin);
                        cacheStream.CopyTo(cacheFileStream);
                    }
            }

            if (initialStringIdCount != CacheContext.StringIdCache.Strings.Count)
                using (var stringIdCacheStream = CacheContext.OpenStringIdCacheReadWrite())
                    CacheContext.StringIdCache.Save(stringIdCacheStream);

            CacheContext.SaveTagNames();

            foreach (var entry in resourceStreams)
            {
                if (Flags.HasFlag(PortingFlags.Memory))
                    using (var resourceFileStream = CacheContext.OpenResourceCacheReadWrite(entry.Key))
                    {
                        resourceFileStream.Seek(0, SeekOrigin.Begin);
                        resourceFileStream.SetLength(resourceFileStream.Position);

                        entry.Value.Seek(0, SeekOrigin.Begin);
                        entry.Value.CopyTo(resourceFileStream);
                    }

                entry.Value.Close();
            }

            return true;
        }

        private List<CacheFile.IndexItem> ParseLegacyTag(string tagSpecifier)
        {
            if (tagSpecifier.Length == 0 || (!char.IsLetter(tagSpecifier[0]) && !tagSpecifier.Contains('*')) || !tagSpecifier.Contains('.'))
                throw new Exception($"Invalid tag name: {tagSpecifier}");

            var tagIdentifiers = tagSpecifier.Split('.');

            if (!CacheContext.TryParseGroupTag(tagIdentifiers[1], out var groupTag))
                throw new Exception($"Invalid tag name: {tagSpecifier}");

            var tagName = tagIdentifiers[0];

            List<CacheFile.IndexItem> result = new List<CacheFile.IndexItem>();

            // find the CacheFile.IndexItem(s)
            if (tagName == "*") result = BlamCache.IndexItems.FindAll(
                item => item != null && groupTag == item.GroupTag);
            else result.Add(BlamCache.IndexItems.Find(
                item => item != null && groupTag == item.GroupTag && tagName == item.Name));

            if (result.Count == 0)
                throw new Exception($"Invalid tag name: {tagSpecifier}");

            return result;
        }

        public CachedTagInstance ConvertTag(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CacheFile.IndexItem blamTag)
        {
            CachedTagInstance result = null;
#if !DEBUG
            try
            {
#endif
            var oldFlags = Flags;
            result = ConvertTagInternal(cacheStream, resourceStreams, blamTag);
            Flags = oldFlags;
#if !DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine($"{e.GetType().Name} while porting '{blamTag.Name}.{blamTag.GroupName}':");
                Console.WriteLine($"\t{e.Message}");
                Console.WriteLine();
                throw e;
            }
#endif
            return result;
        }

        public CachedTagInstance ConvertTagInternal(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CacheFile.IndexItem blamTag)
        {
            if (blamTag == null)
                return null;

            var groupTag = blamTag.GroupTag;

            //
            // Handle tags that are not ready to be ported
            //

            switch (groupTag.ToString())
            {
                case "shit": // use the global shit tag until shit tags are port-able
                    return CacheContext.GetTag<ShieldImpact>(@"globals\global_shield_impact_settings");

                case "sncl": // always use the default sncl tag
                    return CacheContext.GetTag<SoundClasses>(@"sound\sound_classes");

                case "rmw ": // Until water vertices port, always null water shaders to prevent the screen from turning blue. Can return 0x400F when fixed
                    return CacheContext.GetTag<ShaderWater>(@"levels\multi\riverworld\shaders\riverworld_water_rough");

                case "rmcs": // there are no rmcs tags in ms23, disable completely for now

                case "rmbk": // Unknown, black shaders don't exist in HO, only in ODST, might be just complete blackness
                    return CacheContext.GetTag<Shader>(@"shaders\invalid");

                    //TODO: Someday we might be able to generate these, but for now lets just use the standard vertex shaders
                case "glvs":
                    return CacheContext.GetTag<GlobalVertexShader>(@"shaders\shader_shared_vertex_shaders");
                case "glps":
                    return CacheContext.GetTag<GlobalPixelShader>(@"shaders\shader_shared_pixel_shaders");
                case "rmct":
                    if(!HaloShaderGenerator.HaloShaderGenerator.LibraryLoaded)
                    {
                        return CacheContext.GetTag<Shader>(@"shaders\invalid");
                    }
                    break;
                case "rmt2":
                    if (!HaloShaderGenerator.HaloShaderGenerator.LibraryLoaded)
                    {
                        // discard cortana shaders
                        if (blamTag.Name.ToLower().Contains("cortana_template"))
                        {
                            if (CacheContext.TryGetTag<RenderMethodTemplate>(blamTag.Name, out var rmt2Instance))
                                return rmt2Instance;

                            return null; // This will be generated in the shader post
                        }
                    }
                    // unsupported shaders use default behavior
                    break;

                case "rmhg" when Flags.HasFlag(PortingFlags.NoRmhg): // rmhg have register indexing issues currently
                    if (CacheContext.TryGetTag<ShaderHalogram>(blamTag.Name, out var rmhgInstance))
                        return rmhgInstance;
                    return CacheContext.GetTag<ShaderHalogram>(@"objects\ui\shaders\editor_gizmo");

                // Don't port rmdf tags when using ShaderTest (MatchShaders doesn't port either but that's handled elsewhere).
                case "rmdf" when Flags.HasFlag(PortingFlags.ShaderTest) && CacheContext.TagNames.ContainsValue(blamTag.Name) && BlamCache.Version >= CacheVersion.Halo3Retail:
                    return CacheContext.GetTag<RenderMethodDefinition>(blamTag.Name);
                case "rmdf" when Flags.HasFlag(PortingFlags.ShaderTest) && !CacheContext.TagNames.ContainsValue(blamTag.Name) && BlamCache.Version >= CacheVersion.Halo3Retail:
                    Console.WriteLine($"WARNING: Unable to locate `{blamTag.Name}.rmdf`; using `shaders\\shader.rmdf` instead.");
                    return CacheContext.GetTag<RenderMethodDefinition>(@"shaders\shader");
            }

            //
            // Handle shader tags when not porting or matching shaders
            //

            if ((RenderMethodTagGroups.Contains(groupTag) || EffectTagGroups.Contains(groupTag)) &&
                (!Flags.HasFlag(PortingFlags.ShaderTest) && !Flags.HasFlag(PortingFlags.MatchShaders)))
            {
                switch (groupTag.ToString())
                {
                    case "beam":
                        return CacheContext.GetTag<BeamSystem>(@"objects\weapons\support_high\spartan_laser\fx\firing_3p");

                    case "cntl":
                        return CacheContext.GetTag<ContrailSystem>(@"objects\weapons\pistol\needler\fx\projectile");

                    case "decs":
                        return CacheContext.GetTag<DecalSystem>(@"fx\decals\impact_plasma\impact_plasma_medium\hard");

                    case "ltvl":
                        return CacheContext.GetTag<LightVolumeSystem>(@"objects\weapons\pistol\plasma_pistol\fx\charged\projectile");

                    case "prt3":
                        return CacheContext.GetTag<Particle>(@"fx\particles\energy\sparks\impact_spark_orange");

                    case "rmd ":
                        return CacheContext.GetTag<ShaderDecal>(@"objects\gear\human\military\shaders\human_military_decals");

                    case "rmfl":
                        return CacheContext.GetTag<ShaderFoliage>(@"levels\multi\riverworld\shaders\riverworld_tree_leafa");

                    case "rmtr":
                        return CacheContext.GetTag<ShaderTerrain>(@"levels\multi\riverworld\shaders\riverworld_ground");

                    case "rmrd":
                    case "rmsh":
                    case "rmss":
                        return CacheContext.GetTag<Shader>(@"shaders\invalid");
                }
            }

            //
            // Check to see if the ElDorado tag exists
            //

            CachedTagInstance edTag = null;

            TagGroup edGroup = null;

            if (TagGroup.Instances.ContainsKey(groupTag))
            {
                edGroup = TagGroup.Instances[groupTag];
            }
            else
            {
                edGroup = new TagGroup(
                    blamTag.GroupTag,
                    blamTag.ParentGroupTag,
                    blamTag.GrandparentGroupTag,
                    CacheContext.GetStringId(blamTag.GroupName));
            }

            if ((groupTag == "snd!") && Flags.HasFlag(PortingFlags.NoAudio))
                return null;

            var wasReplacing = Flags.HasFlag(PortingFlags.Replace);
            var wasNew = Flags.HasFlag(PortingFlags.New);
            var wasSingle = Flags.HasFlag(PortingFlags.Recursive);

            if (Flags.HasFlag(PortingFlags.NoElites) && groupTag == "bipd" && (blamTag.Name.Contains("elite") || blamTag.Name.Contains("dervish")))
                return null;

            if (ReplacedTags.ContainsKey(groupTag) && ReplacedTags[groupTag].Contains(blamTag.Name))
            {
                foreach (var entry in CacheContext.TagNames.Where(i => i.Value == blamTag.Name))
                {
                    var instance = CacheContext.GetTag(entry.Key);

                    if (instance.IsInGroup("rm  ") && (Flags & PortingFlags.NoMs30) != 0)
                    {
                        var rm = CacheContext.Deserialize<RenderMethod>(cacheStream, instance);
                        var rmt2 = CacheContext.Deserialize<RenderMethodTemplateFast>(cacheStream, rm.ShaderProperties[0].Template);

                        if (rmt2.VertexShader?.Index >= 0x4455 || rmt2.PixelShader?.Index >= 0x4455)
                            continue;
                    }

                    if (instance.Group.Tag == groupTag)
                        return edTag = instance;
                }
            }
            else if (!Flags.HasFlag(PortingFlags.New))
            {
                foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup(groupTag))
                {
                    if (instance == null || !CacheContext.TagNames.ContainsKey(instance.Index))
                        continue;

                    if (instance.IsInGroup("rm  ") && (Flags & PortingFlags.NoMs30) != 0)
                    {
                        var rm = CacheContext.Deserialize<RenderMethod>(cacheStream, instance);
                        var rmt2 = CacheContext.Deserialize<RenderMethodTemplateFast>(cacheStream, rm.ShaderProperties[0].Template);

                        if (rmt2.VertexShader?.Index >= 0x4455 || rmt2.PixelShader?.Index >= 0x4455)
                            continue;
                    }

                    if (CacheContext.TagNames[instance.Index] == blamTag.Name)
                    {
                        if (Flags.HasFlag(PortingFlags.Replace) && !DoNotReplaceGroups.Contains(instance.Group.Tag.ToString()))
                        {
                            if (!Flags.HasFlag(PortingFlags.Recursive) && wasSingle)
                            {
                                Flags &= ~PortingFlags.Replace;
                                Flags |= PortingFlags.Recursive;
                            }

                            edTag = instance;
                            break;
                        }
                        else return edTag = instance;
                    }
                }
            }

            if (Flags.HasFlag(PortingFlags.New) && !Flags.HasFlag(PortingFlags.Recursive))
            {
                Flags &= ~PortingFlags.New;
                Flags |= PortingFlags.Recursive;
            }

            //
            // If isReplacing is true, check current tags if there is an existing instance to replace
            //

            var replacedTags = ReplacedTags.ContainsKey(groupTag) ?
                (ReplacedTags[groupTag] ?? new List<string>()) :
                new List<string>();

            replacedTags.Add(blamTag.Name);
            ReplacedTags[groupTag] = replacedTags;

            //
            // Allocate Eldorado Tag
            //

            if (edTag == null)
            {
                if (Flags.HasFlag(PortingFlags.UseNull))
                {
                    var i = CacheContext.TagCache.Index.ToList().FindIndex(n => n == null);

                    if (i >= 0)
                        CacheContext.TagCache.Index[i] = edTag = new CachedTagInstance(i, edGroup);
                }
                else
                {
                    edTag = CacheContext.TagCache.AllocateTag(edGroup);
                }
            }

            CacheContext.TagNames[edTag.Index] = blamTag.Name;

            //
            // Load the Blam tag definition
            //

            var blamContext = new CacheSerializationContext(ref BlamCache, blamTag);
            var blamDefinition = BlamCache.Deserializer.Deserialize(blamContext, TagDefinition.Find(groupTag));

            //
            // Perform pre-conversion fixups to the Blam tag definition
            //

            switch (blamDefinition)
            {
                case RenderModel mode when BlamCache.Version < CacheVersion.Halo3Retail:
                    foreach (var material in mode.Materials)
                        material.RenderMethod = null;
                    break;

                case Scenario scenario when Flags.HasFlag(PortingFlags.NoSquads):
                    scenario.Squads = new List<Scenario.Squad>();
                    break;
                case Scenario scenario when Flags.HasFlag(PortingFlags.NoForgePalette):
                    scenario.SandboxEquipment.Clear();
                    scenario.SandboxGoalObjects.Clear();
                    scenario.SandboxScenery.Clear();
                    scenario.SandboxSpawning.Clear();
                    scenario.SandboxTeleporters.Clear();
                    scenario.SandboxVehicles.Clear();
                    scenario.SandboxWeapons.Clear();
                    break;

                case ScenarioStructureBsp bsp: // named instanced geometry instances, useless unless we decompile bsp's
                    foreach (var instance in bsp.InstancedGeometryInstances)
                        instance.Name = StringId.Invalid;
                    break;
            }

            //
            // Perform automatic conversion on the Blam tag definition
            //

            blamDefinition = ConvertData(cacheStream, resourceStreams, blamDefinition, blamDefinition, blamTag.Name);

            //
            // Perform post-conversion fixups to Blam data
            //

            switch (blamDefinition)
            {
                case AreaScreenEffect sefc:
                    if (BlamCache.Version < CacheVersion.Halo3ODST)
                    {
                        sefc.GlobalHiddenFlags = AreaScreenEffect.HiddenFlagBits.UpdateThread | AreaScreenEffect.HiddenFlagBits.RenderThread;

                        foreach (var screenEffect in sefc.ScreenEffects)
                            screenEffect.HiddenFlags = AreaScreenEffect.HiddenFlagBits.UpdateThread | AreaScreenEffect.HiddenFlagBits.RenderThread;
                    }
                    if (blamTag.Name == @"levels\ui\mainmenu\sky\ui")
                    {
                        sefc.ScreenEffects[0].Unknown4 = 1E-19f;
                        sefc.ScreenEffects[0].Duration = 1E-19f;
                    }
                    break;

                case Bitmap bitm:
                    blamDefinition = ConvertBitmap(bitm, resourceStreams);
                    break;

                case CameraFxSettings cfxs:
                    blamDefinition = ConvertCameraFxSettings(cfxs);
                    break;

                case Character character:
                    blamDefinition = ConvertCharacter(character);
                    break;

                case ChudDefinition chdt:
                    blamDefinition = ConvertChudDefinition(chdt);
                    break;

                case ChudGlobalsDefinition chudGlobals:
                    blamDefinition = ConvertChudGlobalsDefinition(chudGlobals);
                    break;

                case Cinematic cine:
                    blamDefinition = ConvertCinematic(cine);
                    break;

                case CinematicScene cisc:
                    blamDefinition = ConvertCinematicScene(cisc);
                    break;

                case Dialogue udlg:
                    blamDefinition = ConvertDialogue(cacheStream, udlg);
                    break;

                case Effect effect when BlamCache.Version == CacheVersion.Halo3Retail:
                    //foreach (var even in effect.Events)
                    //foreach (var particleSystem in even.ParticleSystems)
                    //particleSystem.Unknown7 = 1.0f / particleSystem.Unknown7;
                    break;

                case Globals matg:
                    blamDefinition = ConvertGlobals(matg, cacheStream);
                    break;

                case LensFlare lens:
                    blamDefinition = ConvertLensFlare(lens);
                    break;

                case ModelAnimationGraph jmad:
                    blamDefinition = ConvertModelAnimationGraph(cacheStream, resourceStreams, jmad);
                    break;

                case MultilingualUnicodeStringList unic:
                    blamDefinition = ConvertMultilingualUnicodeStringList(cacheStream, resourceStreams, unic);
                    break;

                case Particle particle when BlamCache.Version == CacheVersion.Halo3Retail:
                    // Shift all flags above 2 by 1.
                    particle.Flags = (particle.Flags & 0x3) + ((int)(particle.Flags & 0xFFFFFFFC) << 1);
                    break;

                // If there is no valid resource in the prtm tag, null the mode itself to prevent crashes
                case ParticleModel particleModel when BlamCache.Version >= CacheVersion.Halo3Retail && particleModel.Geometry.Resource.Page.Index == -1:
                    blamDefinition = null;
                    break;

                case PhysicsModel phmo:
                    blamDefinition = ConvertPhysicsModel(phmo);
                    break;

                case Projectile proj:
                    blamDefinition = ConvertProjectile(proj);
                    break;

                case RasterizerGlobals rasg:
                    blamDefinition = ConvertRasterizerGlobals(rasg);
                    break;

                // If there is no valid resource in the mode tag, null the mode itself to prevent crashes (engineer head, harness)
                case RenderModel mode when BlamCache.Version >= CacheVersion.Halo3Retail && mode.Geometry.Resource.Page.Index == -1:
                    blamDefinition = null;
                    break;

                case RenderModel renderModel when BlamCache.Version < CacheVersion.Halo3Retail:
                    blamDefinition = ConvertGen2RenderModel(edTag, renderModel, resourceStreams);
                    break;

                case Scenario scnr:
                    blamDefinition = ConvertScenario(cacheStream, resourceStreams, scnr, blamTag.Name);
                    break;

                case ScenarioLightmap sLdT:
                    blamDefinition = ConvertScenarioLightmap(cacheStream, resourceStreams, blamTag.Name, sLdT);
                    break;

                case ScenarioLightmapBspData Lbsp:
                    blamDefinition = ConvertScenarioLightmapBspData(Lbsp);
                    break;

                case ScenarioStructureBsp sbsp:
                    blamDefinition = ConvertScenarioStructureBsp(sbsp, edTag, resourceStreams);
                    break;

                case SkyAtmParameters skya:
                    // Decrease secondary fog intensity (it's quite sickening in ms23)
                    // foreach (var atmosphere in skya.AtmosphereProperties)
                    // atmosphere.FogIntensity2 /= 36.0f;
                    break;

                case Sound sound:
                    blamDefinition = ConvertSound(cacheStream, resourceStreams, sound, blamTag.Name);
                    break;

                case SoundLooping lsnd:
                    blamDefinition = ConvertSoundLooping(lsnd);
                    break;

                case SoundMix snmx:
                    blamDefinition = ConvertSoundMix(snmx);
                    break;

                case StructureDesign sddt:
                    blamDefinition = ConvertStructureDesign(sddt);
                    break;

                case Style style:
                    blamDefinition = ConvertStyle(style);
                    break;

                // Fix shotgun reloading
                case Weapon weapon when blamTag.Name == "objects\\weapons\\rifle\\shotgun\\shotgun":
                    weapon.Unknown24 = 1 << 16;
                    break;
                case Weapon weapon when blamTag.Name.EndsWith("\\weapon\\warthog_horn"):
                    foreach (var attach in weapon.Attachments)
                        attach.PrimaryScale = CacheContext.GetStringId("primary_rate_of_fire");
                    break;

                case ShaderCortana shader_cortana:
                    ConvertShaderCortana(shader_cortana, cacheStream, resourceStreams);
                    break;
            }

            //
            // Finalize and serialize the new ElDorado tag definition
            //

            if (blamDefinition == null) //If blamDefinition is null, return null tag.
            {
                CacheContext.TagNames.Remove(edTag.Index);
                CacheContext.TagCache.Index[edTag.Index] = null;
                return null;
            }

            CacheContext.Serialize(cacheStream, edTag, blamDefinition);

            Console.WriteLine($"['{edTag.Group.Tag}', 0x{edTag.Index:X4}] {CacheContext.TagNames[edTag.Index]}.{CacheContext.GetString(edTag.Group.Name)}");

            return edTag;
        }

        private object ConvertData(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, object data, object definition, string blamTagName)
        {
            if (data == null)
                return null;

            var type = data.GetType();

            if (type.IsPrimitive)
                return data;

            switch (data)
            {
                case TagFunction tagFunction:
                    return ConvertTagFunction(tagFunction);

                case StringId stringId:
                    {
                        if (stringId == StringId.Invalid)
                            return stringId;

                        var value = BlamCache.Version < CacheVersion.Halo3Retail ?
                            BlamCache.Strings.GetItemByID((int)(stringId.Value & 0xFFFF)) :
                            BlamCache.Strings.GetString(stringId);

                        var edStringId = BlamCache.Version < CacheVersion.Halo3Retail ?
                            CacheContext.GetStringId(value) :
                            CacheContext.StringIdCache.GetStringId(stringId.Set, value);

                        if ((stringId != StringId.Invalid) && (edStringId != StringId.Invalid))
                            return edStringId;

                        if (((stringId != StringId.Invalid) && (edStringId == StringId.Invalid)) || !CacheContext.StringIdCache.Contains(value))
                            return CacheContext.StringIdCache.AddString(value);

                        return StringId.Invalid;
                    }

                case CachedTagInstance tag:
                    {
                        if (!Flags.HasFlag(PortingFlags.Recursive))
                        {
                            foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup(tag.Group))
                            {
                                if (instance == null || !CacheContext.TagNames.ContainsKey(instance.Index))
                                    continue;

                                if (CacheContext.TagNames[instance.Index] == blamTagName)
                                    return instance;
                            }

                            return null;
                        }

                        tag = PortTagReference(tag.Index);

                        if (tag != null && !(Flags.HasFlag(PortingFlags.New) || Flags.HasFlag(PortingFlags.Replace)))
                            return tag;

                        return ConvertTag(cacheStream, resourceStreams, BlamCache.IndexItems.Find(i => i.ID == ((CachedTagInstance)data).Index));
                    }

                case CollisionMoppCode collisionMopp:
                    collisionMopp.Data = ConvertCollisionMoppData(collisionMopp.Data);
                    return collisionMopp;

                case DamageReportingType damageReportingType:
                    return ConvertDamageReportingType(damageReportingType);

                case GameObjectType gameObjectType:
                    return ConvertGameObjectType(gameObjectType);

                case ObjectTypeFlags objectTypeFlags:
                    return ConvertObjectTypeFlags(objectTypeFlags);

                case BipedPhysicsFlags bipedPhysicsFlags:
                    return ConvertBipedPhysicsFlags(bipedPhysicsFlags);

                case WeaponFlags weaponFlags:
                    return ConvertWeaponFlags(weaponFlags);

                case RenderMaterial.PropertyType propertyType when BlamCache.Version < CacheVersion.Halo3Retail:
                    if (!Enum.TryParse(propertyType.Halo2.ToString(), out propertyType.Halo3))
                        throw new NotSupportedException(propertyType.Halo2.ToString());
                    break;

                case RenderMethod renderMethod when Flags.HasFlag(PortingFlags.MatchShaders):
                    ConvertData(cacheStream, resourceStreams, renderMethod.ShaderProperties[0].ShaderMaps, renderMethod.ShaderProperties[0].ShaderMaps, blamTagName);
                    return ConvertRenderMethod(cacheStream, resourceStreams, renderMethod, blamTagName);

                case ScenarioObjectType scenarioObjectType:
                    return ConvertScenarioObjectType(scenarioObjectType);

                case SoundClass soundClass:
                    return soundClass.ConvertSoundClass(BlamCache.Version);
            }

            if (type.IsArray)
                return ConvertArray(cacheStream, resourceStreams, (Array)data, definition, blamTagName);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return ConvertList(cacheStream, resourceStreams, data, type, definition, blamTagName);

            if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
            {
                data = ConvertStructure(cacheStream, resourceStreams, data, type, definition, blamTagName);

                switch (data)
                {
                    case RenderGeometry renderGeometry when BlamCache.Version >= CacheVersion.Halo3Retail:
                        return GeometryConverter.Convert(cacheStream, renderGeometry, resourceStreams, Flags);

                    case Mesh.Part part when BlamCache.Version < CacheVersion.Halo3Retail:
                        if (!Enum.TryParse(part.TypeOld.ToString(), out part.TypeNew))
                            throw new NotSupportedException(part.TypeOld.ToString());
                        break;

                    case RenderMaterial.Property property when BlamCache.Version < CacheVersion.Halo3Retail:
                        property.IntValue = property.ShortValue;
                        break;

                    case Model.GlobalDamageInfoBlock newDamageInfo:
                        return ConvertNewDamageInfo(newDamageInfo);
                }
            }

            return data;
        }

        private Array ConvertArray(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, Array array, object definition, string blamTagName)
        {
            if (array.GetType().GetElementType().IsPrimitive)
                return array;

            for (var i = 0; i < array.Length; i++)
            {
                var oldValue = array.GetValue(i);
                var newValue = ConvertData(cacheStream, resourceStreams, oldValue, definition, blamTagName);
                array.SetValue(newValue, i);
            }

            return array;
        }

        private object ConvertList(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, object list, Type type, object definition, string blamTagName)
        {
            if (type.GenericTypeArguments[0].IsPrimitive)
                return list;

            var count = (int)type.GetProperty("Count").GetValue(list);

            var getItem = type.GetMethod("get_Item");
            var setItem = type.GetMethod("set_Item");

            for (var i = 0; i < count; i++)
            {
                var oldValue = getItem.Invoke(list, new object[] { i });
                var newValue = ConvertData(cacheStream, resourceStreams, oldValue, definition, blamTagName);
                setItem.Invoke(list, new object[] { i, newValue });
            }

            return list;
        }

        private object ConvertStructure(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, object data, Type type, object definition, string blamTagName)
        {
            using (var enumerator = ReflectionCache.GetTagFieldEnumerator(type, CacheContext.Version))
            {
                while (enumerator.Next())
                {
                    var oldValue = enumerator.Field.GetValue(data);
                    var newValue = ConvertData(cacheStream, resourceStreams, oldValue, definition, blamTagName);
                    enumerator.Field.SetValue(data, newValue);
                }
            }
            return data;
        }

        private Model.GlobalDamageInfoBlock ConvertNewDamageInfo(Model.GlobalDamageInfoBlock newDamageInfo)
        {
            if (!Enum.TryParse(newDamageInfo.CollisionDamageReportingTypeOld.HaloOnline.ToString(), out newDamageInfo.CollisionDamageReportingTypeNew))
                newDamageInfo.CollisionDamageReportingTypeNew = Model.GlobalDamageInfoBlock.DamageReportingTypeNew.Guardians;

            if (!Enum.TryParse(newDamageInfo.ResponseDamageReportingTypeOld.HaloOnline.ToString(), out newDamageInfo.ResponseDamageReportingTypeNew))
                newDamageInfo.ResponseDamageReportingTypeNew = Model.GlobalDamageInfoBlock.DamageReportingTypeNew.Guardians;

            return newDamageInfo;
        }

        private ObjectTypeFlags ConvertObjectTypeFlags(ObjectTypeFlags objectTypeFlags)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                if (!Enum.TryParse(objectTypeFlags.Halo3Retail.ToString(), out objectTypeFlags.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());
            }

            return objectTypeFlags;
        }

        private BipedPhysicsFlags ConvertBipedPhysicsFlags(BipedPhysicsFlags bipedPhysicsFlags)
        {
            if (!Enum.TryParse(bipedPhysicsFlags.Halo3Retail.ToString(), out bipedPhysicsFlags.Halo3Odst))
                throw new FormatException(BlamCache.Version.ToString());

            return bipedPhysicsFlags;
        }

        private object ConvertWeaponFlags(WeaponFlags weaponFlags)
        {
            if (weaponFlags.OldFlags.HasFlag(WeaponFlags.OldWeaponFlags.WeaponUsesOldDualFireErrorCode))
                weaponFlags.OldFlags &= ~WeaponFlags.OldWeaponFlags.WeaponUsesOldDualFireErrorCode;

            if (!Enum.TryParse(weaponFlags.OldFlags.ToString(), out weaponFlags.NewFlags))
                throw new FormatException(BlamCache.Version.ToString());

            return weaponFlags;
        }

        private DamageReportingType ConvertDamageReportingType(DamageReportingType damageReportingType)
        {
            string value = null;

            switch (BlamCache.Version)
            {
                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                    value = damageReportingType.Halo2Retail.ToString();
                    break;

                case CacheVersion.Halo3ODST:
                    value = damageReportingType.Halo3ODST.ToString();
                    break;

                case CacheVersion.Halo3Retail:
                    value = damageReportingType.Halo3Retail.ToString();
                    break;
            }

            if (value == null || !Enum.TryParse(value, out damageReportingType.HaloOnline))
                throw new NotSupportedException(value ?? CacheContext.Version.ToString());

            return damageReportingType;
        }

        private TagFunction ConvertTagFunction(TagFunction function)
        {
            return TagFunction.ConvertTagFunction(function);
        }

        private GameObjectType ConvertGameObjectType(GameObjectType objectType)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
                if (!Enum.TryParse(objectType.Halo3Retail.ToString(), out objectType.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());

            return objectType;
        }

        private ScenarioObjectType ConvertScenarioObjectType(ScenarioObjectType objectType)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
                if (!Enum.TryParse(objectType.Halo3Retail.ToString(), out objectType.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());

            return objectType;
        }

        private CachedTagInstance PortTagReference(int index, int maxIndex = 0xFFFF)
        {
            if (index == -1)
                return null;

            var instance = BlamCache.IndexItems.Find(i => i.ID == index);

            if (instance != null)
            {
                var tags = CacheContext.TagCache.Index.FindAllInGroup(instance.GroupTag);

                foreach (var tag in tags)
                {
                    if (!CacheContext.TagNames.ContainsKey(tag.Index))
                        continue;

                    if (instance.Name == CacheContext.TagNames[tag.Index] && tag.Index < maxIndex)
                        return tag;
                }
            }

            return null;
        }
    }
}