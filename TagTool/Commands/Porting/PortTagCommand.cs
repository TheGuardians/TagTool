using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Damage;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.Tags;
using TagTool.Shaders;
using TagTool.Tags.Definitions;
using TagTool.Serialization;
using System.Text.RegularExpressions;
using TagTool.IO;
using TagTool.Cache.HaloOnline;

namespace TagTool.Commands.Porting
{
    public partial class PortTagCommand : Command
	{
		private GameCacheHaloOnlineBase CacheContext { get; }
		private GameCache BlamCache;
		private RenderGeometryConverter GeometryConverter { get; }

		private Dictionary<Tag, List<string>> ReplacedTags = new Dictionary<Tag, List<string>>();

        private Dictionary<int, CachedTag> PortedTags = new Dictionary<int, CachedTag>();
        private Dictionary<uint, StringId> PortedStringIds = new Dictionary<uint, StringId>();

		private List<Tag> RenderMethodTagGroups = new List<Tag> { new Tag("rmbk"), new Tag("rmcs"), new Tag("rmd "), new Tag("rmfl"), new Tag("rmhg"), new Tag("rmsh"), new Tag("rmss"), new Tag("rmtr"), new Tag("rmw "), new Tag("rmrd"), new Tag("rmct") };
		private List<Tag> EffectTagGroups = new List<Tag> { new Tag("beam"), new Tag("cntl"), new Tag("ltvl"), new Tag("decs"), new Tag("prt3") };

        private DirectoryInfo TempDirectory { get; } = new DirectoryInfo(Path.GetTempPath());

		private static readonly string[] DoNotReplaceGroups = new[]
		{
			"glps",
			"glvs",
			"vtsh",
			"pixl",
			"rmdf",
			"rmt2"
		};

		private readonly Dictionary<Tag, CachedTag> DefaultTags = new Dictionary<Tag, CachedTag> { };

		public PortTagCommand(GameCacheHaloOnlineBase cacheContext, GameCache blamCache) :
			base(true,

				"PortTag",
				PortTagCommand.GetPortingFlagsDescription(),
				"PortTag [Options] <Tag>",
				"")
		{
			CacheContext = cacheContext;
			BlamCache = blamCache;
			GeometryConverter = new RenderGeometryConverter(cacheContext, blamCache);

			foreach (var tagType in TagDefinition.Types.Keys)
                DefaultTags[tagType] = CacheContext.TagCache.FindFirstInGroup(tagType);
		}

		public override object Execute(List<string> args)
		{
			if (args.Count < 1)
				return false;

			var portingOptions = args.Take(args.Count - 1).ToList();
			ParsePortingOptions(portingOptions);

			var initialStringIdCount = CacheContext.StringTableHaloOnline.Count;

			//
			// Convert Blam data to ElDorado data
			//

			var resourceStreams = new Dictionary<ResourceLocation, Stream>();

			using (var cacheStream = FlagIsSet(PortingFlags.Memory) ? new MemoryStream() : (Stream)CacheContext.OpenCacheReadWrite())
            using(var blamCacheStream = BlamCache.OpenCacheRead())
			{
				if (FlagIsSet(PortingFlags.Memory))
					using (var cacheFileStream = CacheContext.OpenCacheRead())
						cacheFileStream.CopyTo(cacheStream);

				var oldFlags = Flags;

				foreach (var blamTag in ParseLegacyTag(args.Last()))
				{
					ConvertTag(cacheStream, blamCacheStream, resourceStreams, blamTag);
					Flags = oldFlags;
				}

				if (FlagIsSet(PortingFlags.Memory))
					using (var cacheFileStream = CacheContext.OpenCacheReadWrite())
					{
						cacheFileStream.Seek(0, SeekOrigin.Begin);
						cacheFileStream.SetLength(cacheFileStream.Position);

						cacheStream.Seek(0, SeekOrigin.Begin);
						cacheStream.CopyTo(cacheFileStream);
					}
			}

            if (initialStringIdCount != CacheContext.StringTable.Count)
                CacheContext.SaveStrings();

			CacheContext.SaveTagNames();

			foreach (var entry in resourceStreams)
			{
				if (FlagIsSet(PortingFlags.Memory))
					using (var resourceFileStream = CacheContext.ResourceCaches.OpenCacheReadWrite(entry.Key))
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

        public CachedTag ConvertTag(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTag blamTag)
        {
            if (blamTag == null)
                return null;

            CachedTag result = null;
#if !DEBUG
            try
            {
#endif
                if (PortedTags.ContainsKey(blamTag.Index))
                    return PortedTags[blamTag.Index];

                var oldFlags = Flags;
                result = ConvertTagInternal(cacheStream, blamCacheStream, resourceStreams, blamTag);
                Flags = oldFlags;
#if !DEBUG
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine($"{e.GetType().Name} while porting '{blamTag.Name}.{blamTag.Group.Tag.ToString()}':");
                Console.WriteLine();
                throw e;
            }
#endif
            PortedTags[blamTag.Index] = result;
            return result;
        }

		public CachedTag ConvertTagInternal(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTag blamTag)
		{
			if (blamTag == null)
				return null;

			var groupTag = blamTag.Group.Tag;

			//
			// Handle tags that are not ready to be ported
			//

			switch (groupTag.ToString())
			{
                case "snd!":
                    if (!FlagIsSet(PortingFlags.Audio))
                    {
                        PortingConstants.DefaultTagNames.TryGetValue(groupTag, out string defaultSoundName);
                        CacheContext.TryGetTag($"{defaultSoundName}.{groupTag}", out CachedTag result);
                        return result;
                    }
                    break;

                case "bipd":
                    if (!FlagIsSet(PortingFlags.Elites) && (blamTag.Name.Contains("elite") || blamTag.Name.Contains("dervish")))
                        return null;
                    break;

				case "shit": // use the global shit tag until shit tags are port-able
					if (CacheContext.TryGetTag<ShieldImpact>(blamTag.Name, out var shitInstance) && !FlagIsSet(PortingFlags.Replace))
                        return shitInstance;
                    if (BlamCache.Version < CacheVersion.HaloOnline106708)
                        return CacheContext.Deserialize<RasterizerGlobals>(cacheStream, CacheContext.GetTag<RasterizerGlobals>(@"globals\rasterizer_globals")).DefaultShieldImpact;
                    break;

                case "sncl" when BlamCache.Version > CacheVersion.HaloOnline700123:
                    return CacheContext.GetTag<SoundClasses>(@"sound\sound_classes");

				case "glvs":
					return CacheContext.GetTag<GlobalVertexShader>(@"shaders\shader_shared_vertex_shaders");
				case "glps":
					return CacheContext.GetTag<GlobalPixelShader>(@"shaders\shader_shared_pixel_shaders");
				case "rmct":
					if (!HaloShaderGenerator.HaloShaderGenerator.LibraryLoaded)
					{
						return CacheContext.GetTag<Shader>(@"shaders\invalid");
					}
					break;
				case "rmt2":
					if (HaloShaderGenerator.HaloShaderGenerator.LibraryLoaded)
					{
						// discard cortana shaders
						if (blamTag.Name.ToLower().Contains("cortana_template"))
						{
							if (CacheContext.TryGetTag<RenderMethodTemplate>(blamTag.Name, out var rmt2Instance))
								return rmt2Instance;

							return null; // This will be generated in the shader post
						}
					}

                    // match rmt2 with current ones available, else return null
                    return FindClosestRmt2(cacheStream, blamCacheStream, blamTag);

                    break;
			} 

			//
			// Check to see if the ElDorado tag exists
			//

			CachedTag edTag = null;

			TagGroup edGroup = null;

			if (TagGroup.Instances.ContainsKey(groupTag))
			{
				edGroup = TagGroup.Instances[groupTag];
			}
			else
			{
				edGroup = new TagGroup(
					blamTag.Group.Tag,
					blamTag.Group.ParentTag,
					blamTag.Group.GrandparentTag,
					CacheContext.StringTable.GetStringId(BlamCache.StringTable.GetString(blamTag.Group.Name)));
			}

            var wasReplacing = FlagIsSet(PortingFlags.Replace);
			var wasNew = FlagIsSet(PortingFlags.New);
			var wasSingle = FlagIsSet(PortingFlags.Recursive);

            foreach (var instance in CacheContext.TagCache.TagTable)
            {
                if (instance == null || !instance.IsInGroup(groupTag) || instance.Name == null || instance.Name != blamTag.Name)
                    continue;

                if (instance.IsInGroup("rm  ") && !FlagIsSet(PortingFlags.Ms30) && instance.Name.StartsWith("ms30"))
                {
                    continue;
                }

                if (ReplacedTags.ContainsKey(groupTag) && ReplacedTags[groupTag].Contains(blamTag.Name))
                {
                    if (instance.Group.Tag == groupTag)
                        return instance;
                }
                else if (!FlagIsSet(PortingFlags.New))
                {
                    if (FlagIsSet(PortingFlags.Replace) && !DoNotReplaceGroups.Contains(instance.Group.Tag.ToString()))
                    {
                        if (!FlagIsSet(PortingFlags.Recursive))
                            ToggleFlags(PortingFlags.Replace | PortingFlags.Recursive);

                        edTag = instance;
                        break;
                    }
                    else
                    {
                        if (FlagIsSet(PortingFlags.Merge))
                        {
                            switch (groupTag.ToString())
                            {
                                case "char":
                                    MergeCharacter(cacheStream, blamCacheStream, resourceStreams, instance, blamTag);
                                    break;

                                case "mulg":
                                    MergeMultiplayerGlobals(cacheStream, blamCacheStream, resourceStreams, instance, blamTag);
                                    break;

                                case "unic":
                                    MergeMultilingualUnicodeStringList(cacheStream, blamCacheStream, resourceStreams, instance, blamTag);
                                    break;
                            }
                        }

                        if (!ReplacedTags.ContainsKey(groupTag))
                            ReplacedTags[groupTag] = new List<string>();

                        ReplacedTags[groupTag].Add(blamTag.Name);
                        return instance;
                    }
                }
            }

            if (FlagIsSet(PortingFlags.New) && !FlagIsSet(PortingFlags.Recursive) && wasSingle)
				ToggleFlags(PortingFlags.New | PortingFlags.Recursive);

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
				if (FlagIsSet(PortingFlags.UseNull))
				{
					var i = CacheContext.TagCache.TagTable.ToList().FindIndex(n => n == null);

					if (i >= 0)
						CacheContext.TagCacheGenHO.Tags[i] = (CachedTagHaloOnline)(edTag = (new CachedTagHaloOnline(i, edGroup)));
				}
				else
				{
					edTag = CacheContext.TagCache.AllocateTag(edGroup);
				}
			}

			edTag.Name = blamTag.Name;

            //
            // Load the Blam tag definition
            //

            object blamDefinition = BlamCache.Deserialize(blamCacheStream, blamTag);

			//
			// Perform pre-conversion fixups to the Blam tag definition
			//

			switch (blamDefinition)
			{
				case RenderModel mode when BlamCache.Version < CacheVersion.Halo3Retail:
					foreach (var material in mode.Materials)
						material.RenderMethod = null;
					break;

				case Scenario scenario when !FlagIsSet(PortingFlags.Squads):
					scenario.Squads = new List<Scenario.Squad>();
					break;

				case Scenario scenario when !FlagIsSet(PortingFlags.ForgePalette):
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

            ((TagStructure)blamDefinition).PreConvert(BlamCache.Version, CacheContext.Version);

			//
			// Perform automatic conversion on the Blam tag definition
			//

			blamDefinition = ConvertData(cacheStream, blamCacheStream, resourceStreams, blamDefinition, blamDefinition, blamTag.Name);

            //
            // Perform post-conversion fixups to Blam data
            //

            ((TagStructure)blamDefinition).PostConvert(BlamCache.Version, CacheContext.Version);

            switch (blamDefinition)
			{
				case AreaScreenEffect sefc:
					if (BlamCache.Version < CacheVersion.Halo3ODST)
					{
						sefc.GlobalHiddenFlags = AreaScreenEffect.HiddenFlagBits.UpdateThread | AreaScreenEffect.HiddenFlagBits.RenderThread;

						foreach (var screenEffect in sefc.ScreenEffects)
							screenEffect.HiddenFlags = AreaScreenEffect.HiddenFlagBits.UpdateThread | AreaScreenEffect.HiddenFlagBits.RenderThread;
                    }
                    if (sefc.ScreenEffects[0].Duration == 1.0f && sefc.ScreenEffects[0].MaximumDistance == 1.0f)
                    {
                        sefc.ScreenEffects[0].Duration = 1E+19f;
                        sefc.ScreenEffects[0].MaximumDistance = 1E+19f;
                    }
                    break;

				case Bitmap bitm:
					blamDefinition = ConvertBitmap(blamTag, bitm, resourceStreams);
					break;

				case CameraFxSettings cfxs:
					blamDefinition = ConvertCameraFxSettings(cfxs, blamTag.Name);
					break;

				case Character character:
                    blamDefinition = ConvertCharacter(cacheStream, character);
					break;

				case ChudDefinition chdt:
					blamDefinition = ConvertChudDefinition(chdt);
					break;

				case ChudGlobalsDefinition chudGlobals:
					blamDefinition = ConvertChudGlobalsDefinition(cacheStream, blamCacheStream, resourceStreams, chudGlobals);
					break;

                case CinematicScene cisc:
                    foreach (var shot in cisc.Shots)
                    {
                        foreach (var frame in shot.Frames)
                        {
                            frame.NearPlane *= -1.0f;
                            frame.FarPlane *= -1.0f;
                        }
                    }
                    break;

                case CortanaEffectDefinition crte:
                    foreach (var gravemindblock in crte.Gravemind)
                    {
                        foreach (var vignetteblock in gravemindblock.Vignette)
                        {
                            foreach (var dynamicvaluesblock in vignetteblock.DynamicValues)
                            {
                                foreach (var framesblock in dynamicvaluesblock.Frames)
                                {
                                    //fix inverted vignette
                                    float temp = framesblock.Dynamicvalue1; 
                                    framesblock.Dynamicvalue1 = framesblock.Dynamicvalue2;
                                    framesblock.Dynamicvalue2 = temp;
                                }
                            }
                        }
                    }
                    foreach (var postprocessblock in crte.PostProcessing)
                    {
                        foreach (var hueblock in postprocessblock.Hue)
                        {
                            //make red tentacles greenish brown
                            hueblock.Basevalue1 = 55.0f;
                        }
                    }
                    break;

				case Dialogue udlg:
					blamDefinition = ConvertDialogue(cacheStream, udlg);
					break;

                case Effect effe:
                    blamDefinition = FixupEffect(cacheStream, resourceStreams, effe, blamTag.Name);
                    break;

                case GameObject gameobject:
                    //fix AI object avoidance
                    if (gameobject.Model != null)
                    {
                        var childmodeltag = CacheContext.TagCache.GetTag(gameobject.Model.Index);
                        if (childmodeltag.DefinitionOffset > 0) //sometimes a tag that isn't ported yet can be referenced here, which causes a crash
                        {
                            var childmodel = CacheContext.Deserialize<Model>(cacheStream, childmodeltag);
                            if (childmodel.CollisionModel != null)
                            {
                                var childcollisionmodel = CacheContext.Deserialize<CollisionModel>(cacheStream, childmodel.CollisionModel);
                                if (childcollisionmodel.PathfindingSpheres.Count > 0)
                                {
                                    gameobject.PathfindingSpheres = new List<GameObject.PathfindingSphere>();
                                    for (var i = 0; i < childcollisionmodel.PathfindingSpheres.Count; i++)
                                    {
                                        gameobject.PathfindingSpheres.Add(new GameObject.PathfindingSphere
                                        {
                                            Node = childcollisionmodel.PathfindingSpheres[i].Node,
                                            Flags = (GameObject.PathfindingSphereFlags)childcollisionmodel.PathfindingSpheres[i].Flags,
                                            Center = childcollisionmodel.PathfindingSpheres[i].Center,
                                            Radius = childcollisionmodel.PathfindingSpheres[i].Radius
                                        });
                                    }
                                }
                            }
                        }
                    };

                    //all gameobjects are handled within this subswitch now
                    switch (gameobject)
                    {
                        case Weapon weapon:
                            //fix weapon firing looping sounds
                            foreach (var attach in weapon.Attachments)
                                if (attach.PrimaryScale == CacheContext.StringTable.GetStringId("primary_firing"))
                                    attach.PrimaryScale = CacheContext.StringTable.GetStringId("primary_rate_of_fire");
                            //fix weapon target tracking
                            if (weapon.Tracking > 0 || weapon.WeaponType == Weapon.WeaponTypeValue.Needler)
                            {
                                weapon.TargetTracking = new List<Weapon.TargetTrackingBlock>{
                                    new Weapon.TargetTrackingBlock{
                                        AcquireTime = (weapon.Tracking == Weapon.TrackingType.HumanTracking ? 1.0f : 0.0f),
                                        GraceTime = (weapon.WeaponType == Weapon.WeaponTypeValue.Needler ? 0.2f : 0.1f),
                                        DecayTime = (weapon.WeaponType == Weapon.WeaponTypeValue.Needler ? 0.0f : 0.2f),
                                        TrackingTypes = (weapon.Tracking == Weapon.TrackingType.HumanTracking ?
                                            new List<Weapon.TargetTrackingBlock.TrackingType> {
                                                new Weapon.TargetTrackingBlock.TrackingType{
                                                    TrackingType2 = CacheContext.StringTable.GetStringId("ground_vehicles")
                                                },
                                                new Weapon.TargetTrackingBlock.TrackingType{
                                                    TrackingType2 = CacheContext.StringTable.GetStringId("flying_vehicles")
                                                },
                                            }
                                            :
                                            new List<Weapon.TargetTrackingBlock.TrackingType> {
                                                new Weapon.TargetTrackingBlock.TrackingType{
                                                    TrackingType2 = CacheContext.StringTable.GetStringId("bipeds")
                                                },
                                        })
                                    }
                                };
                                if (weapon.Tracking == Weapon.TrackingType.HumanTracking)
                                {
                                    weapon.TargetTracking[0].TrackingSound = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"sound\weapons\missile_launcher\tracking_locking\tracking_locking.sound_looping")[0]);
                                    weapon.TargetTracking[0].LockedSound = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"sound\weapons\missile_launcher\tracking_locked\tracking_locked.sound_looping")[0]);                                      
                                }
                            }                    
                            break;
                        default:
                            break;
                    };                                     
                    break;

				case Globals matg:
					blamDefinition = ConvertGlobals(matg, cacheStream);
					break;

				case LensFlare lens:
					blamDefinition = ConvertLensFlare(lens);
					break;

                case Model hlmt:
                    foreach (var target in hlmt.Targets)
                    {
                        if (target.Flags.HasFlag(Model.Target.FlagsValue.LockedByHumanTracking))
                            target.TargetFilter = CacheContext.StringTable.GetStringId("flying_vehicles");
                        else if (target.Flags.HasFlag(Model.Target.FlagsValue.LockedByPlasmaTracking))
                            target.TargetFilter = CacheContext.StringTable.GetStringId("bipeds");
                    }
                    break;
              
				case ModelAnimationGraph jmad:
					blamDefinition = ConvertModelAnimationGraph(cacheStream, blamCacheStream, resourceStreams, jmad);
					break;

				case MultilingualUnicodeStringList unic:
					blamDefinition = ConvertMultilingualUnicodeStringList(cacheStream, blamCacheStream, resourceStreams, unic);
					break;

				case Particle particle when BlamCache.Version == CacheVersion.Halo3Retail:
					// Shift all flags above 2 by 1.
					particle.Flags = (particle.Flags & 0x3) + ((int)(particle.Flags & 0xFFFFFFFC) << 1);
					break;

				case ParticleModel particleModel:
                    blamDefinition = ConvertParticleModel(edTag, blamTag, particleModel);
                    break;

				case PhysicsModel phmo:
					blamDefinition = ConvertPhysicsModel(edTag, phmo);
					break;

				case RasterizerGlobals rasg:
					blamDefinition = ConvertRasterizerGlobals(rasg);
					break;

                case RenderModel mode:
                    if (BlamCache.Version < CacheVersion.Halo3Retail)
                        blamDefinition = ConvertGen2RenderModel(edTag, mode, resourceStreams);
                    else
                        blamDefinition = ConvertGen3RenderModel(edTag, blamTag, mode);
					break;

				case Scenario scnr:
					blamDefinition = ConvertScenario(cacheStream, blamCacheStream, resourceStreams, scnr, blamTag.Name);
					break;

				case ScenarioLightmap sLdT:
					blamDefinition = ConvertScenarioLightmap(cacheStream, blamCacheStream, resourceStreams, blamTag.Name, sLdT);
					break;

				case ScenarioLightmapBspData Lbsp:
					blamDefinition = ConvertScenarioLightmapBspData(Lbsp);
					break;

				case ScenarioStructureBsp sbsp:
					blamDefinition = ConvertScenarioStructureBsp(sbsp, edTag, resourceStreams);
					break;

                case Sound sound:
					blamDefinition = ConvertSound(cacheStream, blamCacheStream, resourceStreams, sound, blamTag.Name);
					break;

                case SoundClasses sncl:
                    blamDefinition = ConvertSoundClasses(sncl, BlamCache.Version);
                    break;

                case SoundLooping lsnd:
                    blamDefinition = ConvertSoundLooping(lsnd);
					break;

				case SoundMix snmx:
					blamDefinition = ConvertSoundMix(snmx);
					break;

				case Style style:
					blamDefinition = ConvertStyle(style);
					break;

                case TextValuePairDefinition sily:
                    Enum.TryParse(sily.ParameterH3.ToString(), out sily.ParameterHO);
                    break;
                    
                case UserInterfaceSharedGlobalsDefinition wigl:
                    if (BlamCache.Version == CacheVersion.Halo3Retail)
                    {
                        wigl.UiWidgetBipeds = new List<UserInterfaceSharedGlobalsDefinition.UiWidgetBiped>
                        {
                            new UserInterfaceSharedGlobalsDefinition.UiWidgetBiped
                            {
                                AppearanceBipedName = "chief",
                                RosterPlayer1BipedName = "elite",
                            }
                        };
                    }
                    break;
            }

            //
            // Shader conversion
            //

            switch (blamDefinition)
            {
                case ShaderFoliage rmfl:
                case ShaderBlack rmbk:
                case ShaderTerrain rmtr:
                case ShaderCustom rmcs:
                case ShaderDecal rmd:
                case ShaderHalogram rmhg:
                case Shader rmsh:
                case ShaderScreen rmss:
                case ShaderWater rmw:
                case ShaderZonly rmzo:
                case ContrailSystem cntl:
                case Particle prt3:
                case LightVolumeSystem ltvl:
                case DecalSystem decs:
                case BeamSystem beam:
                    if (!FlagIsSet(PortingFlags.MatchShaders))
                        return GetDefaultShader(blamTag.Group.Tag);
                    else
                    {
                        blamDefinition = ConvertShader(cacheStream, blamCacheStream, blamDefinition, blamTag, BlamCache.Deserialize(blamCacheStream, blamTag));
                        if (blamDefinition == null) // convert shader failed
                            return GetDefaultShader(blamTag.Group.Tag);
                    }
                    break;

                case ShaderCortana rmct:
                    ConvertShaderCortana(rmct, cacheStream, blamCacheStream, resourceStreams);
                    break;
            }

            //
            // Finalize and serialize the new ElDorado tag definition
            //

            if (blamDefinition == null) //If blamDefinition is null, return null tag.
			{
				CacheContext.TagCacheGenHO.Tags[edTag.Index] = null;
				return null;
			}

			CacheContext.Serialize(cacheStream, edTag, blamDefinition);

			if (FlagIsSet(PortingFlags.Print))
				Console.WriteLine($"['{edTag.Group.Tag}', 0x{edTag.Index:X4}] {edTag.Name}.{CacheContext.StringTable.GetString(edTag.Group.Name)}");

			return edTag;
		}

        private Effect FixupEffect(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, Effect effe, string blamTagName)
        {
            foreach (var effectEvent in effe.Events)
            {
                foreach (var particleSystem in effectEvent.ParticleSystems)
                {
                    if (BlamCache.Version < CacheVersion.Halo3ODST) //this value is inverted in ODST tags when compared to H3
                    {
                        particleSystem.NearRange = 1 / particleSystem.NearRange;
                    }
                    if (particleSystem.Particle != null)// yucky hack-fix for some particles taking over the screen
                    {
                        var prt3Definition = CacheContext.Deserialize<Particle>(cacheStream, particleSystem.Particle);
                        if ((prt3Definition.Flags & (1 << 7)) != 0) // flag bit is always 7 -- this is a post porting fixup
                        {
                            foreach (var attachment in prt3Definition.Attachments)
                            {
                                if (attachment.Type != null && attachment.Type.Group.Tag == "effe")
                                {
                                    var attachmentEffe = CacheContext.Deserialize<Effect>(cacheStream, attachment.Type);
                                    foreach (var attEvent in attachmentEffe.Events)
                                    {
                                        if (attEvent.ParticleSystems.Count > 0)
                                        {
                                            // this prevents the particles attached effect from rendering at random sizes
                                            particleSystem.Emitters[0].EmitterFlags &= ~Effect.Event.ParticleSystem.Emitter.FlagsValue.ClampParticleVelocities;
                                            particleSystem.Emitters[0].EmitterFlags |= Effect.Event.ParticleSystem.Emitter.FlagsValue.ParticleEmittedInsideShape;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return effe;
        }

        public object ConvertData(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, object data, object definition, string blamTagName)
		{
			switch (data)
			{
                case TagResourceReference _:
                    return data;

				case StringId stringId:
					stringId = ConvertStringId(stringId);
					return stringId;

				case null:  // no conversion necessary
				case ValueType _:   // no conversion necessary
				case string _:  // no conversion necessary
					return data;

				case TagFunction tagFunction:
					return ConvertTagFunction(tagFunction);

				case CachedTag tag:
					{
						if (!FlagIsSet(PortingFlags.Recursive))
						{
							foreach (var instance in CacheContext.TagCache.FindAllInGroup(tag.Group.Tag))
							{
								if (instance == null || instance.Name == null)
									continue;

								if (instance.Name == tag.Name)
									return instance;
							}

							return null;
						}

						tag = PortTagReference(tag.Index);

						if (tag != null && !(FlagsAnySet(PortingFlags.New | PortingFlags.Replace)))
							return tag;

						return ConvertTag(cacheStream, blamCacheStream, resourceStreams, (CachedTag)data);
					}

                case TagHkpMoppCode hkpMoppCode:
                    hkpMoppCode.Data.Elements = HavokConverter.ConvertMoppCodes(BlamCache.Version, CacheContext.Version, hkpMoppCode.Data.Elements);
                    return hkpMoppCode;

                case PhysicsModel.PhantomTypeFlags phantomTypeFlags:
                    return ConvertPhantomTypeFlags(blamTagName, phantomTypeFlags);

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

                case BarrelFlags barrelflags:
                    return ConvertBarrelFlags(barrelflags);

                case Vehicle.VehicleFlagBits vehicleFlags:
                    return ConvertVehicleFlags(vehicleFlags);

                case Vehicle.HavokVehiclePhysicsFlags havokVehicleFlags:
                    return ConvertHavokVehicleFlags(havokVehicleFlags);

                case RenderMaterial.PropertyType propertyType when BlamCache.Version < CacheVersion.Halo3Retail:
					if (!Enum.TryParse(propertyType.Halo2.ToString(), out propertyType.Halo3))
						throw new NotSupportedException(propertyType.Halo2.ToString());
					return propertyType;

				case RenderMethod renderMethod:
                    return ConvertStructure(cacheStream, blamCacheStream, resourceStreams, renderMethod, definition, blamTagName);

				case ScenarioObjectType scenarioObjectType:
					return ConvertScenarioObjectType(scenarioObjectType);

				case SoundClass soundClass:
					return soundClass.ConvertSoundClass(BlamCache.Version);

				case Array _:
				case IList _: // All arrays and List<T> implement IList, so we should just use that
					data = ConvertCollection(cacheStream, blamCacheStream, resourceStreams, data as IList, definition, blamTagName);
					return data;

				case RenderGeometry renderGeometry when BlamCache.Version >= CacheVersion.Halo3Retail:
					renderGeometry = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, renderGeometry, definition, blamTagName);
					return renderGeometry;

				case Mesh.Part part when BlamCache.Version < CacheVersion.Halo3Retail:
					part = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, part, definition, blamTagName);
					if (!Enum.TryParse(part.TypeOld.ToString(), out part.TypeNew))
						throw new NotSupportedException(part.TypeOld.ToString());
					return part;

				case RenderMaterial.Property property when BlamCache.Version < CacheVersion.Halo3Retail:
					property = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, property, definition, blamTagName);
					property.IntValue = property.ShortValue;
					return property;

				case TagStructure tagStructure: // much faster to pattern match a type than to check for custom attributes.
					tagStructure = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, tagStructure, definition, blamTagName);
					return data;

				case PixelShaderReference _:
				case VertexShaderReference _:
					return null;

				default:
					Console.WriteLine($"WARNING: Unhandled type in `ConvertData`: {data.GetType().Name} (probably harmless).");
					break;
			}

			return data;
		}

        private IList ConvertCollection(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, IList data, object definition, string blamTagName)
		{
			// return early where possible
			if (data is null || data.Count == 0) 
				return data;

            if (data[0] == null)
                return data;

			var type = data[0].GetType();
			if ((type.IsValueType && type != typeof(StringId)) ||
				type == typeof(string))
				return data;
			
			// convert each element
			for (var i = 0; i < data.Count; i++)
			{
				var oldValue = data[i];
				var newValue = ConvertData(cacheStream, blamCacheStream, resourceStreams, oldValue, definition, blamTagName);
				data[i] = newValue;
			}

			return data;
		}

        private T UpgradeStructure<T>(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, T data, object definition, string blamTagName) where T : TagStructure
        {
            if (BlamCache.Version >= CacheVersion.Halo3Retail)
                return data;

            switch (data)
            {
                case Mesh.Part part:
                    if (!Enum.TryParse(part.TypeOld.ToString(), out part.TypeNew))
                        throw new NotSupportedException(part.TypeOld.ToString());
                    break;

                case RenderMaterial.Property property:
                    property.IntValue = property.ShortValue;
                    break;

                case Vehicle.VehicleSteeringControl steering:
                    steering.OverdampenCuspAngleNew = Angle.FromDegrees(steering.OverdampenCuspAngleOld);
                    break;

                case Vehicle vehi:
                    vehi.FlipOverMessageNew = ConvertStringId(vehi.FlipOverMessageOld);
                    vehi.FlipTimeNew = vehi.FlipTimeOld;
                    vehi.FlippingAngularVelocityRangeNew = vehi.FlippingAngularVelocityRangeOld;
                    vehi.HavokPhysicsNew = vehi.HavokPhysicsOld;

                    vehi.PhysicsTypes = new Vehicle.VehiclePhysicsTypes();

                    switch (vehi.PhysicsType)
                    {
                        case Vehicle.VehiclePhysicsType.HumanTank:
                            vehi.PhysicsTypes.HumanTank = new List<Vehicle.HumanTankPhysics>
                            {
                                new Vehicle.HumanTankPhysics
                                {
                                    ForwardArc = Angle.FromDegrees(100.0f),
                                    FlipWindow = 0.4f,
                                    PeggedFraction = 1.0f,
                                    MaximumLeftDifferential = vehi.MaximumLeftSlide,
                                    MaximumRightDifferential = vehi.MaximumRightSlide,
                                    DifferentialAcceleration = vehi.SlideAcceleration,
                                    DifferentialDeceleration = vehi.SlideDeceleration,
                                    MaximumLeftReverseDifferential = vehi.MaximumLeftSlide,
                                    MaximumRightReverseDifferential = vehi.MaximumRightSlide,
                                    DifferentialReverseAcceleration = vehi.SlideAcceleration,
                                    DifferentialReverseDeceleration = vehi.SlideDeceleration,
                                    Engine = new Vehicle.EnginePhysics
                                    {
                                        EngineMomentum = vehi.EngineMomentum,
                                        EngineMaximumAngularVelocity = vehi.EngineMaximumAngularVelocity,
                                        Gears = vehi.Gears,
                                        GearShiftSound = null
                                    },
                                    WheelCircumference = vehi.WheelCircumference,
                                    GravityAdjust = 0.45f
                                }
                            };
                            break;

                        case Vehicle.VehiclePhysicsType.HumanJeep:
                            vehi.PhysicsTypes.HumanJeep = new List<Vehicle.HumanJeepPhysics>
                            {
                                new Vehicle.HumanJeepPhysics
                                {
                                    Steering = vehi.Steering,
                                    Turning = new Vehicle.VehicleTurningControl
                                    {
                                        MaximumLeftTurn = vehi.MaximumLeftTurn,
                                        MaximumRightTurn = vehi.MaximumRightTurn,
                                        TurnRate = vehi.TurnRate
                                    },
                                    Engine = new Vehicle.EnginePhysics
                                    {
                                        EngineMomentum = vehi.EngineMomentum,
                                        EngineMaximumAngularVelocity = vehi.EngineMaximumAngularVelocity,
                                        Gears = vehi.Gears,
                                        GearShiftSound = CacheContext.GetTag<Vehicle>(@"sound\vehicles\warthog\warthog_shift")
                                    },
                                    WheelCircumference = vehi.WheelCircumference,
                                    GravityAdjust = 0.8f
                                }
                            };
                            break;

                        case Vehicle.VehiclePhysicsType.HumanBoat:
                            throw new NotSupportedException(vehi.PhysicsType.ToString());

                        case Vehicle.VehiclePhysicsType.HumanPlane:
                            vehi.PhysicsTypes.HumanPlane = new List<Vehicle.HumanPlanePhysics>
                            {
                                new Vehicle.HumanPlanePhysics
                                {
                                    MaximumForwardSpeed = vehi.MaximumForwardSpeed,
                                    MaximumReverseSpeed = vehi.MaximumReverseSpeed,
                                    SpeedAcceleration = vehi.SpeedAcceleration,
                                    SpeedDeceleration = vehi.SpeedDeceleration,
                                    MaximumLeftSlide = vehi.MaximumLeftSlide,
                                    MaximumRightSlide = vehi.MaximumRightSlide,
                                    SlideAcceleration = vehi.SlideAcceleration,
                                    SlideDeceleration = vehi.SlideDeceleration,
                                    MaximumUpRise = vehi.MaximumForwardSpeed,
                                    MaximumDownRise = vehi.MaximumForwardSpeed,
                                    RiseAcceleration = vehi.SpeedAcceleration,
                                    RiseDeceleration = vehi.SpeedDeceleration,
                                    FlyingTorqueScale = vehi.FlyingTorqueScale,
                                    AirFrictionDeceleration = vehi.AirFrictionDeceleration,
                                    ThrustScale = vehi.ThrustScale,
                                    TurnRateScaleWhenBoosting = 1.0f,
                                    MaximumRoll = Angle.FromDegrees(90.0f),
                                    SteeringAnimation = new Vehicle.VehicleSteeringAnimation
                                    {
                                        InterpolationScale = 0.9f,
                                        MaximumAngle = Angle.FromDegrees(15.0f)
                                    }
                                }
                            };
                            break;

                        case Vehicle.VehiclePhysicsType.AlienScout:
                            vehi.PhysicsTypes.AlienScout = new List<Vehicle.AlienScoutPhysics>
                            {
                                new Vehicle.AlienScoutPhysics
                                {
                                    Steering = vehi.Steering,
                                    MaximumForwardSpeed = vehi.MaximumForwardSpeed,
                                    MaximumReverseSpeed = vehi.MaximumReverseSpeed,
                                    SpeedAcceleration = vehi.SpeedAcceleration,
                                    SpeedDeceleration = vehi.SpeedDeceleration,
                                    MaximumLeftSlide = vehi.MaximumLeftSlide,
                                    MaximumRightSlide = vehi.MaximumRightSlide,
                                    SlideAcceleration = vehi.SlideAcceleration,
                                    SlideDeceleration = vehi.SlideDeceleration,
                                    Flags = Vehicle.VehicleScoutPhysicsFlags.None, // TODO
                                    DragCoefficient = 0.0f,
                                    ConstantDeceleration = 0.0f,
                                    TorqueScale = 1.0f,
                                    EngineGravityFunction = new Vehicle.AlienScoutGravityFunction
                                    {// TODO
                                        ObjectFunctionDamageRegion = StringId.Invalid,
                                        AntiGravityEngineSpeedRange = new Bounds<float>(0.0f, 0.0f),
                                        EngineSpeedAcceleration = 0.0f,
                                        MaximumVehicleSpeed = 0.0f
                                    },
                                    ContrailObjectFunction = new Vehicle.AlienScoutGravityFunction
                                    {// TODO
                                        ObjectFunctionDamageRegion = StringId.Invalid,
                                        AntiGravityEngineSpeedRange = new Bounds<float>(0.0f, 0.0f),
                                        EngineSpeedAcceleration = 0.0f,
                                        MaximumVehicleSpeed = 0.0f
                                    },
                                    GearRotationSpeed = new Bounds<float>(0.0f, 0.0f), // TODO
                                    SteeringAnimation = new Vehicle.VehicleSteeringAnimation
                                    {// TODO
                                        InterpolationScale = 0.0f,
                                        MaximumAngle = Angle.FromDegrees(0.0f)
                                    }
                                }
                            };
                            break;

                        case Vehicle.VehiclePhysicsType.AlienFighter:
                            vehi.PhysicsTypes.AlienFighter = new List<Vehicle.AlienFighterPhysics>
                            {
                                new Vehicle.AlienFighterPhysics
                                {
                                    Steering = vehi.Steering,
                                    Turning = new Vehicle.VehicleTurningControl
                                    {
                                        MaximumLeftTurn = vehi.MaximumLeftTurn,
                                        MaximumRightTurn = vehi.MaximumRightTurn,
                                        TurnRate = vehi.TurnRate
                                    },
                                    MaximumForwardSpeed = vehi.MaximumForwardSpeed,
                                    MaximumReverseSpeed = vehi.MaximumReverseSpeed,
                                    SpeedAcceleration = vehi.SpeedAcceleration,
                                    SpeedDeceleration = vehi.SpeedDeceleration,
                                    MaximumLeftSlide = vehi.MaximumLeftSlide,
                                    MaximumRightSlide = vehi.MaximumRightSlide,
                                    SlideAcceleration = vehi.SlideAcceleration,
                                    SlideDeceleration = vehi.SlideDeceleration,
                                    SlideAccelAgainstDirection = 1.0f,
                                    FlyingTorqueScale = vehi.FlyingTorqueScale,
                                    FixedGunOffset = vehi.FixedGunOffset,
                                    LoopTrickDuration = 1.8f,
                                    RollTrickDuration = 1.8f,
                                    ZeroGravitySpeed = 4.0f,
                                    FullGravitySpeed = 3.7f,
                                    StrafeBoostScale = 7.5f,
                                    OffStickDecelScale = 0.1f,
                                    CruisingThrottle = 0.75f,
                                    DiveSpeedScale = 0.0f
                                }
                            };
                            break;

                        case Vehicle.VehiclePhysicsType.Turret:
                            vehi.PhysicsTypes.Turret = new List<Vehicle.TurretPhysics>
                            {// TODO: Determine if these fields are used
                                new Vehicle.TurretPhysics()
                            };
                            break;
                    }
                    break;
            }

            return data;
        }

        private T ConvertStructure<T>(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, T data, object definition, string blamTagName) where T : TagStructure
		{
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(data.GetType(), CacheContext.Version))
            {
                var attr = tagFieldInfo.Attribute;

                if ((attr.Version != CacheVersion.Unknown && attr.Version == BlamCache.Version) ||
                    (attr.Version == CacheVersion.Unknown && CacheVersionDetection.IsBetween(BlamCache.Version, attr.MinVersion, attr.MaxVersion)))
                {
                    // skip the field if no conversion is needed
                    if ((tagFieldInfo.FieldType.IsValueType && tagFieldInfo.FieldType != typeof(StringId)) ||
                    tagFieldInfo.FieldType == typeof(string))
                        continue;

                    var oldValue = tagFieldInfo.GetValue(data);
                    if (oldValue is null)
                        continue;

                    // convert the field
                    var newValue = ConvertData(cacheStream, blamCacheStream, resourceStreams, oldValue, definition, blamTagName);
                    tagFieldInfo.SetValue(data, newValue);
                }
            }

            return UpgradeStructure(cacheStream, resourceStreams, data, definition, blamTagName);
		}

		private ObjectTypeFlags ConvertObjectTypeFlags(ObjectTypeFlags flags)
		{
            switch (BlamCache.Version)
            {
                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                    if (!Enum.TryParse(flags.Halo2.ToString(), out flags.Halo3ODST))
                        throw new FormatException(BlamCache.Version.ToString());
                    break;

                case CacheVersion.Halo3Retail:
                    if (!Enum.TryParse(flags.Halo3Retail.ToString(), out flags.Halo3ODST))
                        throw new FormatException(BlamCache.Version.ToString());
                    break;
            }

			return flags;
		}

        private BipedPhysicsFlags ConvertBipedPhysicsFlags(BipedPhysicsFlags flags)
        {
            switch (BlamCache.Version)
            {
                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                    if (!Enum.TryParse(flags.Halo2.ToString(), out flags.Halo3ODST))
                        throw new FormatException(BlamCache.Version.ToString());
                    break;

                case CacheVersion.Halo3Retail:
                    if (!Enum.TryParse(flags.Halo3Retail.ToString(), out flags.Halo3ODST))
                        throw new FormatException(BlamCache.Version.ToString());
                    break;
            }

            return flags;
        }

        private object ConvertWeaponFlags(WeaponFlags weaponFlags)
		{
            // TODO: refactor for Halo 2

			if (weaponFlags.OldFlags.HasFlag(WeaponFlags.OldWeaponFlags.WeaponUsesOldDualFireErrorCode))
				weaponFlags.OldFlags &= ~WeaponFlags.OldWeaponFlags.WeaponUsesOldDualFireErrorCode;

			if (!Enum.TryParse(weaponFlags.OldFlags.ToString(), out weaponFlags.NewFlags))
				throw new FormatException(BlamCache.Version.ToString());

			return weaponFlags;
        }

        private object ConvertBarrelFlags(BarrelFlags barrelflags)
        {
            //fire locked projectiles flag has been removed completely in HO
            if (barrelflags.Halo3.HasFlag(BarrelFlags.Halo3Value.FiresLockedProjectiles))
                barrelflags.Halo3 &= ~BarrelFlags.Halo3Value.FiresLockedProjectiles;

            if (!Enum.TryParse(barrelflags.Halo3.ToString(), out barrelflags.HaloOnline))
                throw new NotSupportedException(barrelflags.Halo3.ToString());

            return barrelflags;
        }

        private PhysicsModel.PhantomTypeFlags ConvertPhantomTypeFlags(string tagName, PhysicsModel.PhantomTypeFlags flags)
        {
            switch (BlamCache.Version)
            {
                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                    if (flags.Halo2.ToString().Contains("Unknown"))
                    {
                        Console.WriteLine($"WARNING: Disabling unknown phantom type flags ({flags.Halo2.ToString()})");
                        Console.WriteLine($"         in tag \"{tagName}.physics_model\"");

                        foreach (var flag in Enum.GetValues(typeof(PhysicsModel.PhantomTypeFlags.Halo2Bits)))
                            if (flag.ToString().StartsWith("Unknown") && flags.Halo2.HasFlag((PhysicsModel.PhantomTypeFlags.Halo2Bits)flag))
                                flags.Halo2 &= ~(PhysicsModel.PhantomTypeFlags.Halo2Bits)flag;
                    }
                    if (!Enum.TryParse(flags.Halo2.ToString(), out flags.Halo3ODST))
                        throw new FormatException(BlamCache.Version.ToString());
                    break;

                case CacheVersion.Halo3Retail:
                    if (flags.Halo3Retail.ToString().Contains("Unknown"))
                    {
                        Console.WriteLine($"WARNING: Found unknown phantom type flags ({flags.Halo3Retail.ToString()})");
                        Console.WriteLine($"         in tag \"{tagName}.physics_model\"");
                        /*
                        foreach (var flag in Enum.GetValues(typeof(PhysicsModel.PhantomTypeFlags.Halo3RetailBits)))
                            if (flag.ToString().StartsWith("Unknown") && flags.Halo3Retail.HasFlag((PhysicsModel.PhantomTypeFlags.Halo3RetailBits)flag))
                                flags.Halo3Retail &= ~(PhysicsModel.PhantomTypeFlags.Halo3RetailBits)flag;
                        */
                    }
                    if (!Enum.TryParse(flags.Halo3Retail.ToString(), out flags.Halo3ODST))
                        throw new FormatException(BlamCache.Version.ToString());
                    break;
            }

            return flags;
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
                    if (damageReportingType.Halo3ODST == DamageReportingType.Halo3ODSTValue.ElephantTurret)
                        value = DamageReportingType.HaloOnlineValue.GuardiansUnknown.ToString();
                    else
					    value = damageReportingType.Halo3ODST.ToString();
					break;

				case CacheVersion.Halo3Retail:
                    if (damageReportingType.Halo3Retail == DamageReportingType.Halo3RetailValue.ElephantTurret)
                        value = DamageReportingType.HaloOnlineValue.GuardiansUnknown.ToString();
                    else
                        value = damageReportingType.Halo3Retail.ToString();
					break;
			}

			if (value == null || !Enum.TryParse(value, out damageReportingType.HaloOnline))
				throw new NotSupportedException(value ?? CacheContext.Version.ToString());

			return damageReportingType;
		}

		private TagFunction ConvertTagFunction(TagFunction function)
		{
			return TagFunction.ConvertTagFunction(CacheVersionDetection.IsLittleEndian(BlamCache.Version) ? EndianFormat.LittleEndian : EndianFormat.BigEndian, function);
		}

        private Vehicle.VehicleFlagBits ConvertVehicleFlags(Vehicle.VehicleFlagBits flags)
        {
            if (BlamCache.Version <= CacheVersion.Halo2Vista)
            {
                var gen2Values = Enum.GetValues(typeof(Vehicle.VehicleFlagBits.Gen2Bits));
                var gen3Values = Enum.GetValues(typeof(Vehicle.VehicleFlagBits.Gen3Bits));

                flags.Gen3 = Vehicle.VehicleFlagBits.Gen3Bits.None;

                foreach (var gen2 in gen2Values)
                {
                    if (!flags.Gen2.HasFlag((Enum)gen2))
                        continue;

                    var wasSet = false;

                    foreach (var gen3 in gen3Values)
                    {
                        if (gen2.ToString() == gen3.ToString())
                        {
                            flags.Gen3 |= (dynamic)gen3;
                            wasSet = true;
                            break;
                        }
                    }

                    if (!wasSet)
                        Console.WriteLine($"WARNING: Vehicle flag not found in gen3: {gen2}");
                }

                if (!flags.Gen2.HasFlag(Vehicle.VehicleFlagBits.Gen2Bits.KillsRidersAtTerminalVelocity))
                    flags.Gen3 |= Vehicle.VehicleFlagBits.Gen3Bits.DoNotKillRidersAtTerminalVelocity;
            }

            return flags;
        }

        private Vehicle.HavokVehiclePhysicsFlags ConvertHavokVehicleFlags(Vehicle.HavokVehiclePhysicsFlags flags)
        {
            if (BlamCache.Version <= CacheVersion.Halo2Vista)
                if (!Enum.TryParse(flags.Gen2.ToString(), out flags.Gen3))
                    throw new FormatException(BlamCache.Version.ToString());

            return flags;
        }

        private GameObjectType ConvertGameObjectType(GameObjectType objectType)
		{
            if (BlamCache.Version <= CacheVersion.Halo2Vista)
                if (!Enum.TryParse(objectType.Halo2.ToString(), out objectType.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());

            if (BlamCache.Version == CacheVersion.Halo3Retail)
				if (!Enum.TryParse(objectType.Halo3Retail.ToString(), out objectType.Halo3ODST))
					throw new FormatException(BlamCache.Version.ToString());

			return objectType;
		}

		private ScenarioObjectType ConvertScenarioObjectType(ScenarioObjectType objectType)
        {
            if (BlamCache.Version <= CacheVersion.Halo2Vista)
                if (!Enum.TryParse(objectType.Halo2.ToString(), out objectType.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());

            if (BlamCache.Version == CacheVersion.Halo3Retail)
				if (!Enum.TryParse(objectType.Halo3Retail.ToString(), out objectType.Halo3ODST))
					throw new FormatException(BlamCache.Version.ToString());

			return objectType;
		}

		private StringId ConvertStringId(StringId stringId)
		{
			if (stringId == StringId.Invalid)
				return stringId;

            if (PortedStringIds.ContainsKey(stringId.Value))
                return PortedStringIds[stringId.Value];

			var value = BlamCache.StringTable.GetString(stringId);
            var edStringId = CacheContext.StringTable.GetStringId(value);


            if (edStringId != StringId.Invalid)
				return PortedStringIds[stringId.Value] = edStringId;

			if (edStringId == StringId.Invalid || !CacheContext.StringTable.Contains(value))
				return PortedStringIds[stringId.Value] = CacheContext.StringTable.AddString(value);

            return PortedStringIds[stringId.Value];
        }

		private CachedTag PortTagReference(int index, int maxIndex = 0xFFFF)
		{
			if (index == -1)
				return null;

            var blamTag = BlamCache.TagCache.GetTag(index);

            if (blamTag == null)
            {
                foreach (var instance in CacheContext.TagCache.TagTable)
                {
                    if (instance == null || !instance.IsInGroup(blamTag.Group.Tag) || instance.Name == null || instance.Name != blamTag.Name || instance.Index >= maxIndex)
                        continue;

                    return instance;
                }
            }

			return null;
		}

        private List<CachedTag> ParseLegacyTag(string tagSpecifier)
        {
            List<CachedTag> result = new List<CachedTag>();

            if (FlagIsSet(PortingFlags.Regex))
            {
                var regex = new Regex(tagSpecifier);
                result = BlamCache.TagCache.TagTable.ToList().FindAll(item => item != null && regex.IsMatch(item.ToString() + "." + item.Group.Tag));
                if (result.Count == 0)
                {
                    Console.WriteLine($"ERROR: Invalid regex: {tagSpecifier}");
                    return new List<CachedTag>();
                }
                return result;
            }

            if (tagSpecifier.Length == 0 || (!char.IsLetter(tagSpecifier[0]) && !tagSpecifier.Contains('*')) || !tagSpecifier.Contains('.'))
            {
                Console.WriteLine($"ERROR: Invalid tag name: {tagSpecifier}");
                return new List<CachedTag>();
            }

            var tagIdentifiers = tagSpecifier.Split('.');

            if (!CacheContext.TryParseGroupTag(tagIdentifiers[1], out var groupTag))
            {
                Console.WriteLine($"ERROR: Invalid tag name: {tagSpecifier}");
                return new List<CachedTag>();
            }

            var tagName = tagIdentifiers[0];

            // find the CacheFile.IndexItem(s)
            if (tagName == "*") result = BlamCache.TagCache.TagTable.ToList().FindAll(
                item => item != null && item.IsInGroup(groupTag));
            else result.Add(BlamCache.TagCache.TagTable.ToList().Find(
                item => item != null && item.IsInGroup(groupTag) && tagName == item.Name));

            if (result.Count == 0)
            {
                Console.WriteLine($"ERROR: Invalid tag name: {tagSpecifier}");
                return new List<CachedTag>();
            }

            return result;
        }
    }
}