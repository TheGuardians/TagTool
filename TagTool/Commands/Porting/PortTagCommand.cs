using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;
using TagTool.Damage;
using TagTool.Audio;

namespace TagTool.Commands.Porting
{
    public partial class PortTagCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }
        private RenderGeometryConverter GeometryConverter { get; }

        private Dictionary<Tag, List<string>> ReplacedTags = new Dictionary<Tag, List<string>>();

		// <index, pre-conversion rmt2>
		private Dictionary<int, RenderMethodTemplate> OldRenderMethodTemplates = new Dictionary<int, RenderMethodTemplate> { };
		// <tag_name, post-conversion pixl>
		private Dictionary<string, PixelShader> NewPixelShaders = new Dictionary<string, PixelShader> { };

		private List<Tag> RenderMethodTagGroups = new List<Tag> { new Tag("rmbk"), new Tag("rmcs"), new Tag("rmd "), new Tag("rmfl"), new Tag("rmhg"), new Tag("rmsh"), new Tag("rmss"), new Tag("rmtr"), new Tag("rmw "), new Tag("rmrd"), new Tag("rmct") };
        private List<Tag> EffectTagGroups = new List<Tag> { new Tag("beam"), new Tag("cntl"), new Tag("ltvl"), new Tag("decs"), new Tag("prt3") };
        private List<Tag> OtherTagGroups = new List<Tag> { /*new Tag("effe"), new Tag("foot"),*/ new Tag("shit"), new Tag("sncl") };

        private bool IsReplacing = false;
        private bool IsRecursive = true;
        private bool IsNew = false;
        private bool UseNull = false;
        private bool NoAudio = false;
        private bool NoElites = false;
        private bool NoForgePalette = false;
        private bool UseShaderTest = false;
        private bool MatchShaders = true;
        private bool ConvertScripts = true;
        private bool NoSquads = false;

        public PortTagCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(CommandFlags.Inherit,

                "PortTag",
                "Ports a tag from the current cache file. Options are : noaudio | noreplace | noelites | noforgepalette | replace | new | single | nonnull" + Environment.NewLine + Environment.NewLine +
                "Replace: Use existing matching tag names if available." + Environment.NewLine +
                "New: Create a new tag after the last index." + Environment.NewLine +
                "Single: Port a new tag without any reference." + Environment.NewLine +
                "UseNull: Port a tag using nulled tag indices where available." + Environment.NewLine +
                "No option: Ports a tag if its name is not present in the tag names.",

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

            while (args.Count > 1)
            {
                switch (args[0].ToLower())
                {
					case "new": IsNew = true; break;
					case "noaudio": NoAudio = true; break;
					case "noelites": NoElites = true; break;
					case "noforgepalette": NoForgePalette = true; break;
					case "noreplace": IsReplacing = false; break;
					case "noscripts": ConvertScripts = false; break;
					case "noshaders": MatchShaders = false; break;
					case "nosquads": NoSquads = true; break;
					case "replace": IsReplacing = true; break;
					case "shadertest": UseShaderTest = true; MatchShaders = false; break;
					case "single": IsRecursive = false; IsNew = true; break;
					case "usenull": UseNull = true; break;
					default: throw new NotImplementedException(args[0]);
				}
                args.RemoveAt(0);
            }
            
            var initialStringIdCount = CacheContext.StringIdCache.Strings.Count;

            //
            // Convert Blam data to ElDorado data
            //

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
                foreach (var blamTag in ParseLegacyTag(args[0]))
                    ConvertTag(cacheStream, blamTag);

            if (initialStringIdCount != CacheContext.StringIdCache.Strings.Count)
                using (var stringIdCacheStream = CacheContext.OpenStringIdCacheReadWrite())
                    CacheContext.StringIdCache.Save(stringIdCacheStream);

            CacheContext.SaveTagNames();

            return true;
        }

        private List<CacheFile.IndexItem> ParseLegacyTag(string tagSpecifier)
        {
            if (tagSpecifier.Length == 0 || (!char.IsLetter(tagSpecifier[0]) && !tagSpecifier.Contains('*')) || !tagSpecifier.Contains('.'))
                throw new Exception($"Invalid tag name: {tagSpecifier}");

			Console.WriteLine(tagSpecifier);
            var tagIdentifiers = tagSpecifier.Split('.');

            var groupTag = ArgumentParser.ParseGroupTag(CacheContext.StringIdCache, tagIdentifiers[1]);
            if (groupTag == Tag.Null)
                throw new Exception($"Invalid tag name: {tagSpecifier}");

            var tagName = tagIdentifiers[0];

            List<CacheFile.IndexItem> result = new List<CacheFile.IndexItem>();

			// find the CacheFile.IndexItem(s)
			if (tagName == "*") result = BlamCache.IndexItems.FindAll(
				item => item != null && groupTag == item.ClassCode);
			else result.Add( BlamCache.IndexItems.Find(
				item => item != null && groupTag == item.ClassCode && tagName == item.Filename));

			if (result.Count == 0)
                Console.WriteLine($"Invalid tag name: {tagSpecifier}");

            return result;
        }

        public CachedTagInstance ConvertTag(Stream cacheStream, CacheFile.IndexItem blamTag)
        {
            if (blamTag == null)
                return null;
            
            //
            // Check to see if the ElDorado tag exists
            //

            var groupTagChars = new char[] { ' ', ' ', ' ', ' ' };
            for (var i = 0; i < blamTag.ClassCode.Length; i++)
                groupTagChars[i] = blamTag.ClassCode[i];

            var groupTag = new Tag(new string(groupTagChars));

            CachedTagInstance edTag = null;

            if ((groupTag == "snd!") && NoAudio)
                return null;

            var wasReplacing = IsReplacing;
            var wasNew = IsNew;
            
            if (NoElites && groupTag == "bipd" && blamTag.Filename.Contains("elite"))
                return null;

            if (!IsNew || groupTag == "glps" || groupTag == "glvs" || groupTag == "rmdf")
            {
                foreach (var instance in CacheContext.TagCache.Index.FindAllInGroup(groupTag))
                {
                    if (instance == null || !CacheContext.TagNames.ContainsKey(instance.Index))
                        continue;

                    if (CacheContext.TagNames[instance.Index] == blamTag.Filename)
                    {
                        if (IsReplacing)
                            edTag = instance;
                        else
                        {
                            edTag = instance;
                            Console.WriteLine($"[Group: '{edTag.Group.Tag}', Index: 0x{edTag.Index:X4}] {CacheContext.TagNames[edTag.Index]}.{CacheContext.GetString(edTag.Group.Name)}");
                            return edTag;
                        }
                    }
                }
            }

            //
            // Check to see if the tag was already replaced (if replacing)
            //

            if (ReplacedTags.ContainsKey(groupTag) && ReplacedTags[groupTag].Contains(blamTag.Filename))
            {
                var entries = CacheContext.TagNames.Where(i => i.Value == blamTag.Filename);

                foreach (var entry in entries)
                {
                    var tagInstance = CacheContext.GetTag(entry.Key);

                    if (tagInstance.Group.Tag == groupTag)
                    {
                        edTag = tagInstance;
                        Console.WriteLine($"[Group: '{edTag.Group.Tag}', Index: 0x{edTag.Index:X4}] {CacheContext.TagNames[edTag.Index]}.{CacheContext.GetString(edTag.Group.Name)}");
                        return edTag;
                    }
                }
            }

            //
            // If isReplacing is true, check current tags if there is an existing instance to replace
            //

            if (IsReplacing)
            {
                var listEntries = CacheContext.TagNames.Where(i => i.Value == blamTag.Filename);

                foreach (var entry in listEntries)
                {
                    var tagInstance = CacheContext.GetTag(entry.Key);

                    if (tagInstance.Group.Tag == groupTag)
                    {
                        edTag = tagInstance;
                        //If not recursive, use existing tags
                        if (!IsRecursive)
                            IsReplacing = false;
                        break;
                    }
                }
            }
            
            var replacedTags = ReplacedTags.ContainsKey(groupTag) ?
                (ReplacedTags[groupTag] ?? new List<string>()) :
                new List<string>();

            replacedTags.Add(blamTag.Filename);
            ReplacedTags[groupTag] = replacedTags;
            
            //
            // Handle shaders that do not exist (either in code or in tags)
            //
            
            if (groupTag == "rmcs")
                return CacheContext.GetTagInstance<Shader>(@"shaders\invalid"); // there are no rmcs tags in ms23, disable completely for now
            
            switch (groupTag.ToString())
            {
                case "rmw ": // Until water vertices port, always null water shaders to prevent the screen from turning blue. Can return 0x400F when fixed
                    return null;

                case "rmct": // Cortana shaders have no example in HO, they need a real port
                    return CacheContext.GetTagInstance<Shader>(@"objects\characters\masterchief\shaders\mp_masterchief_rubber");

                case "rmbk": // Unknown, black shaders don't exist in HO, only in ODST, might be just complete blackness
                    return CacheContext.GetTagInstance<Shader>(@"objects\characters\masterchief\shaders\mp_masterchief_rubber");
            }

            //
            // Handle shader tags when not porting or matching shaders
            //

            if ((RenderMethodTagGroups.Contains(groupTag) || EffectTagGroups.Contains(groupTag)) &&
                (!UseShaderTest && !MatchShaders))
            {
                switch (groupTag.ToString())
                {
                    case "beam":
                        return CacheContext.GetTagInstance<BeamSystem>(@"objects\weapons\support_high\spartan_laser\fx\firing_3p");

                    case "cntl":
                        return CacheContext.GetTagInstance<ContrailSystem>(@"objects\weapons\pistol\needler\fx\projectile");

                    case "decs":
                        return CacheContext.GetTagInstance<DecalSystem>(@"fx\decals\impact_plasma\impact_plasma_medium\hard");

                    case "ltvl":
                        return CacheContext.GetTagInstance<LightVolumeSystem>(@"objects\weapons\pistol\plasma_pistol\fx\charged\projectile");

                    case "prt3":
                        return CacheContext.GetTagInstance<Particle>(@"fx\particles\energy\sparks\impact_spark_orange");

                    case "rmd ":
                        return CacheContext.GetTagInstance<ShaderDecal>(@"objects\gear\human\military\shaders\human_military_decals");

                    case "rmfl":
                        return CacheContext.GetTagInstance<ShaderFoliage>(@"levels\multi\riverworld\shaders\riverworld_tree_leafa");

                    case "rmhg":
                        return CacheContext.GetTagInstance<ShaderHalogram>(@"objects\ui\shaders\editor_gizmo");

                    case "rmtr":
                        return CacheContext.GetTagInstance<ShaderTerrain>(@"levels\multi\riverworld\shaders\riverworld_ground");

					case "rmcs":
                    case "rmrd":
                    case "rmsh":
                    case "rmss":
                        return CacheContext.GetTagInstance<Shader>(@"objects\characters\masterchief\shaders\mp_masterchief_rubber");

                }
            }

            //
            // Handle tags that are not ready to be ported
            //

            if (OtherTagGroups.Contains(groupTag))
            {
                switch (groupTag.ToString())
                {
                    case "effe":
                        return CacheContext.GetTagInstance<Effect>(@"objects\characters\grunt\fx\grunt_birthday_party");

                    case "foot":
                        return CacheContext.GetTagInstance<MaterialEffects>(@"fx\material_effects\objects\characters\masterchief");

                    case "shit":
                        return CacheContext.GetTagInstance<ShieldImpact>(@"globals\global_shield_impact_settings");

                    case "sncl":
                        return CacheContext.GetTagInstance<SoundClasses>(@"sound\sound_classes");
                }    
            }

            //
            // Allocate Eldorado Tag
            //

            if (edTag == null && UseNull)
            {
				var i = CacheContext.TagCache.Index.ToList().FindIndex(n => n == null);
				CacheContext.TagCache.Index[i] = edTag = new CachedTagInstance(i, TagGroup.Instances[groupTag]);
            }

            if (edTag == null)
            {
                TagGroup edGroup = null;

                if (TagGroup.Instances.ContainsKey(groupTag))
                {
                    edGroup = TagGroup.Instances[groupTag];
                }
                else
                {
                    var blamGroup = BlamCache.IndexItems.ClassList[blamTag.ClassIndex];

                    groupTagChars = new char[] { ' ', ' ', ' ', ' ' };
                    for (var i = 0; i < blamGroup.ClassCode.Length; i++)
                        groupTagChars[i] = blamGroup.ClassCode[i];

                    var tag1 = new Tag(new string(groupTagChars));

                    groupTagChars = new char[] { ' ', ' ', ' ', ' ' };
                    for (var i = 0; i < blamGroup.Parent.Length; i++)
                        groupTagChars[i] = blamGroup.Parent[i];

                    var tag2 = new Tag(new string(groupTagChars));

                    groupTagChars = new char[] { ' ', ' ', ' ', ' ' };
                    for (var i = 0; i < blamGroup.Parent2.Length; i++)
                        groupTagChars[i] = blamGroup.Parent2[i];

                    var tag3 = new Tag(new string(groupTagChars));

                    edGroup = new TagGroup(tag1, tag2, tag3, CacheContext.GetStringId(BlamCache.Strings.GetItemByID(blamGroup.StringID)));
                }

                edTag = CacheContext.TagCache.AllocateTag(edGroup);
            }

            CacheContext.TagNames[edTag.Index] = blamTag.Filename;

            //
            // Load the Blam tag definition
            //

            Console.WriteLine($"Porting {blamTag.Filename}.{groupTag.ToString()}");

            var blamContext = new CacheSerializationContext(BlamCache, blamTag);
            var blamDefinition = BlamCache.Deserializer.Deserialize(blamContext, TagDefinition.Find(groupTag));

            //
            // Perform pre-conversion fixups to the Blam tag definition
            //

            switch (blamDefinition)
            {
                case ChudGlobalsDefinition chgd:
                    chgd.HudShaders.Clear();
                    for (int hudGlobalsIndex = 0; hudGlobalsIndex < chgd.HudGlobals.Count; hudGlobalsIndex++)
                        chgd.HudGlobals[hudGlobalsIndex].HudSounds.Clear();
                    break;

                case Scenario scenario when NoSquads:
                    scenario.Squads = new List<Scenario.Squad>();
					break;
				case Scenario scenario when NoForgePalette:
                    scenario.SandboxEquipment.Clear();
                    scenario.SandboxGoalObjects.Clear();
                    scenario.SandboxScenery.Clear();
                    scenario.SandboxSpawning.Clear();
                    scenario.SandboxTeleporters.Clear();
                    scenario.SandboxVehicles.Clear();
                    scenario.SandboxWeapons.Clear();
                    break;

                case ScenarioStructureBsp bsp:
                    foreach (var instance in bsp.InstancedGeometryInstances)
                        instance.Name = StringId.Invalid;
                    break;
            }
            
            //
            // Perform automatic conversion on the Blam tag definition
            //

            blamDefinition = ConvertData(cacheStream, blamDefinition, blamDefinition, blamTag.Filename);

            //
            // Perform post-conversion fixups to Blam data
            //

            switch (blamDefinition)
            {
				case AreaScreenEffect sefc when blamTag.Filename == "levels\\ui\\mainmenu\\sky\\ui":
					foreach (var screenEffect in sefc.ScreenEffects)
						screenEffect.MaximumDistance = screenEffect.Duration = float.MaxValue;
					break;

				case Bitmap bitm:
                    blamDefinition = ConvertBitmap(bitm);
                    break;

                case Character character:
                    blamDefinition = ConvertCharacter(character);
                    break;

                case ChudDefinition chdt:
                    blamDefinition = ConvertChudDefinition(chdt);
                    break;

                case ChudGlobalsDefinition chudGlobals:
                    blamDefinition = ConvertChudGlobalsDefinition(cacheStream, chudGlobals, blamTag, edTag);
                    break;

                case Cinematic cine:
                    blamDefinition = ConvertCinematic(cine);
                    break;

                case CinematicScene cisc:
                    blamDefinition = ConvertCinematicScene(cisc);
                    break;

                case CortanaEffectDefinition crte:
                    blamDefinition = ConvertCortanaEffect(crte);
                    break;

                case Dialogue udlg:
                    blamDefinition = ConvertDialogue(cacheStream, udlg);
                    break;

                case Effect effect when BlamCache.Version == CacheVersion.Halo3Retail:
					foreach (var even in effect.Events)
						foreach (var particleSystem in even.ParticleSystems)
							particleSystem.Unknown7 = 1.0f / particleSystem.Unknown7;
					break;

				case GlobalPixelShader glps when UseShaderTest:
					blamDefinition = ConvertGlobalPixelShader(glps);
					break;

                case Globals matg:
                    blamDefinition = ConvertGlobals(matg, cacheStream);
                    break;

                case GlobalVertexShader glvs when UseShaderTest:
					blamDefinition = ConvertGlobalVertexShader(glvs);
					break;

                case LensFlare lens:
                    blamDefinition = ConvertLensFlare(lens);
                    break;

                case ModelAnimationGraph jmad:
                    blamDefinition = ConvertModelAnimationGraph(cacheStream, jmad);
                    break;

                case MultilingualUnicodeStringList unic:
                    blamDefinition = ConvertMultilingualUnicodeStringList(unic);
                    break;

                case Particle particle when BlamCache.Version == CacheVersion.Halo3Retail:
                    // Shift all flags above 2 by 1.
                    particle.Flags = (particle.Flags & 0x3) + ((int)(particle.Flags & 0xFFFFFFFC) << 1);
                    break;

                case PhysicsModel phmo:
                    blamDefinition = ConvertPhysicsModel(phmo);
                    break;

                case PixelShader pixl when UseShaderTest:
					blamDefinition = ConvertPixelShader(pixl, blamTag);
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

                case Scenario scnr:
                    blamDefinition = ConvertScenario(scnr, blamTag.Filename);
                    break;

                case ScenarioLightmap sLdT:
                    blamDefinition = ConvertScenarioLightmap(cacheStream, blamTag.Filename, sLdT);
                    break;

                case ScenarioLightmapBspData Lbsp:
                    blamDefinition = ConvertScenarionLightmapBspData(Lbsp);
                    break;

                case ScenarioStructureBsp sbsp:
                    blamDefinition = ConvertScenarioStructureBsp(sbsp, edTag);
                    break;

                case SkyAtmParameters skya:
                    // Decrease secondary fog intensity (it's quite sickening in ms23)
                    // foreach (var atmosphere in skya.AtmosphereProperties)
                        // atmosphere.FogIntensity2 /= 36.0f;
                    break;

                case Sound sound:
                    blamDefinition = ConvertSound(sound);
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

                case VertexShader vtsh when UseShaderTest:
                    blamDefinition = ConvertVertexShader(vtsh);
                    break;

                // Fix shotgun reloading
                case Weapon weapon when blamTag.Filename == "objects\\weapons\\rifle\\shotgun\\shotgun":
                    weapon.Unknown24 = 1 << 16;
					break;
				case Weapon weapon when blamTag.Filename.EndsWith("\\weapon\\warthog_horn"):
                    foreach (var attach in weapon.Attachments)
                            attach.PrimaryScale = CacheContext.GetStringId("primary_rate_of_fire");
                    break;
            }

            //
            // Finalize and serialize the new ElDorado tag definition
            //
            
            if (blamDefinition == null) //If blamDefinition is null, return null tag.
            {
                Console.WriteLine($"Something happened when converting  {blamTag.Filename.Substring(Math.Max(0, blamTag.Filename.Length - 30))}, returning null tag reference.");
                CacheContext.TagNames.Remove(edTag.Index);
                CacheContext.TagCache.Index[edTag.Index] = null;
                return null;
            }

            var edContext = new TagSerializationContext(cacheStream, CacheContext, edTag);
            CacheContext.Serializer.Serialize(edContext, blamDefinition);
            CacheContext.SaveTagNames(); // Always save new tagnames in case of a crash

            Console.WriteLine($"['{edTag.Group.Tag}', 0x{edTag.Index:X4}] {CacheContext.TagNames[edTag.Index]}.{CacheContext.GetString(edTag.Group.Name)}");

            return edTag;
        }

        private object ConvertData(Stream cacheStream, object data, object definition, string blamTagName)
        {
            if (data == null)
                return null;

            var type = data.GetType();

            if (type.IsPrimitive)
                return data;

            switch (data)
            {
				case BipedPhysicsFlags bipedPhysicsFlags:
					return ConvertBipedPhysicsFlags(bipedPhysicsFlags);

				// case CachedTagInstance tag when !IsRecursive:
				// 	return null;
				// case CachedTagInstance tag when tag != null && !IsNew || !IsReplacing:
				// 	tag = PortTagReference(tag.Index);
				// 	return tag;
				// case CachedTagInstance tag:
				// 	tag = PortTagReference(tag.Index);
				// 	return ConvertTag(cacheStream, BlamCache.IndexItems.Find(i => i.ID == ((CachedTagInstance)data).Index));

				case CachedTagInstance tag:
					if (IsRecursive == false)
						return null;
					tag = PortTagReference(tag.Index);
					if (tag != null && !(IsNew || IsReplacing))
						return tag;
					return ConvertTag(cacheStream, BlamCache.IndexItems.Find(i => i.ID == ((CachedTagInstance)data).Index));

				case CollisionMoppCode collisionMopp:
					collisionMopp.Data = ConvertCollisionMoppData(collisionMopp.Data);
					return collisionMopp;

				case DamageReportingType damageReportingType:
					return ConvertDamageReportingType(damageReportingType);

				case GameObjectType gameObjectType:
					return ConvertGameObjectType(gameObjectType);

				case ObjectTypeFlags objectTypeFlags:
					return ConvertObjectTypeFlags(objectTypeFlags);

                case Mesh.Part part when BlamCache.Version < CacheVersion.Halo3Retail:
                    if (!Enum.TryParse(part.TypeOld.ToString(), out part.TypeNew))
                        throw new NotSupportedException(part.TypeOld.ToString());
                    break;

				case RenderGeometry renderGeometry when definition is ScenarioStructureBsp sbsp && BlamCache.Version >= CacheVersion.Halo3Retail:
					return GeometryConverter.Convert(cacheStream, renderGeometry);
				case RenderGeometry renderGeometry when definition is RenderModel mode && BlamCache.Version >= CacheVersion.Halo3Retail:
					return GeometryConverter.Convert(cacheStream, renderGeometry);
				case RenderGeometry renderGeometry when BlamCache.Version >= CacheVersion.Halo3Retail:
					return GeometryConverter.Convert(cacheStream, renderGeometry);

                case RenderMaterial.PropertyType propertyType when BlamCache.Version < CacheVersion.Halo3Retail:
                    if (!Enum.TryParse(propertyType.Halo2.ToString(), out propertyType.Halo3))
                        throw new NotSupportedException(propertyType.Halo2.ToString());
                    break;

                case RenderMaterial.Property property when BlamCache.Version < CacheVersion.Halo3Retail:
                    property.IntValue = property.ShortValue;
                    break;

                case RenderMethod renderMethod when MatchShaders:
					ConvertData(cacheStream, renderMethod.ShaderProperties[0].ShaderMaps, renderMethod.ShaderProperties[0].ShaderMaps, blamTagName);
					return ConvertRenderMethod(cacheStream, renderMethod, blamTagName);
				case RenderMethod renderMethod when !MatchShaders && type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0:
					data = ConvertStructure(cacheStream, data, type, definition, blamTagName);
					return ConvertRenderMethodGenerated(cacheStream, renderMethod, blamTagName);

                case RenderModel renderModel when BlamCache.Version < CacheVersion.Halo3Retail:
                    foreach (var material in renderModel.Materials)
                        material.RenderMethod = CacheContext.GetTagInstance<Shader>(@"shaders\invalid");
                    data = ConvertGen2RenderModel(renderModel);
                    break;

                case ScenarioObjectType scenarioObjectType:
					return ConvertScenarioObjectType(scenarioObjectType);

				case SoundClass soundClass:
					return soundClass.ConvertSoundClass(BlamCache.Version);

				case StringId stringId:
					return ConvertStringId(stringId);

				case TagFunction tagFunction:
					return ConvertTagFunction(tagFunction);
			}

            if (type.IsArray)
                return ConvertArray(cacheStream, (Array)data, definition, blamTagName);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return ConvertList(cacheStream, data, type, definition, blamTagName);

            if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
                return ConvertStructure(cacheStream, data, type, definition, blamTagName);

            return data;
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

        private StringId ConvertStringId(StringId stringId)
        {
            var value = BlamCache.Strings.GetString(stringId);
            var edStringId = CacheContext.StringIdCache.GetStringId(stringId.Set, value);

            if ((stringId != StringId.Invalid) && (edStringId != StringId.Invalid))
                return edStringId;

            if (((stringId != StringId.Invalid) && (edStringId == StringId.Invalid)) || !CacheContext.StringIdCache.Contains(value))
                CacheContext.StringIdCache.AddString(value);

            return CacheContext.GetStringId(value);
        }

        private Array ConvertArray(Stream cacheStream, Array array, object definition, string blamTagName)
        {
            if (array.GetType().GetElementType().IsPrimitive)
                return array;

            for (var i = 0; i < array.Length; i++)
            {
                var oldValue = array.GetValue(i);
                var newValue = ConvertData(cacheStream, oldValue, definition, blamTagName);
                array.SetValue(newValue, i);
            }

            return array;
        }

        private object ConvertList(Stream cacheStream, object list, Type type, object definition, string blamTagName)
        {
            if (type.GenericTypeArguments[0].IsPrimitive)
                return list;

            var count = (int)type.GetProperty("Count").GetValue(list);

            var getItem = type.GetMethod("get_Item");
            var setItem = type.GetMethod("set_Item");

            for (var i = 0; i < count; i++)
            {
                var oldValue = getItem.Invoke(list, new object[] { i });
                var newValue = ConvertData(cacheStream, oldValue, definition, blamTagName);
                setItem.Invoke(list, new object[] { i, newValue });
            }

            return list;
        }

        private object ConvertStructure(Stream cacheStream, object data, Type type, object definition, string blamTagName)
        {
            var enumerator = new TagFieldEnumerator(new TagStructureInfo(type, CacheContext.Version));

            while (enumerator.Next())
            {
                var oldValue = enumerator.Field.GetValue(data);
                var newValue = ConvertData(cacheStream, oldValue, definition, blamTagName);
                enumerator.Field.SetValue(data, newValue);
            }

            return data;
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
                var chars = new char[] { ' ', ' ', ' ', ' ' };
                for (var i = 0; i < instance.ClassCode.Length; i++)
                    chars[i] = instance.ClassCode[i];

                var tags = CacheContext.TagCache.Index.FindAllInGroup(new string(chars));

                foreach (var tag in tags)
                {
                    if (!CacheContext.TagNames.ContainsKey(tag.Index))
                        continue;

                    if (instance.Filename == CacheContext.TagNames[tag.Index] && tag.Index < maxIndex)
                        return tag;
                }
            }

            return null;
        }
    }
}