using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Damage;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.Tags;
using TagTool.Shaders;
using TagTool.Tags.Definitions;
using System.Text.RegularExpressions;
using TagTool.IO;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Gen3;
using TagTool.Commands.CollisionModels;

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

		private List<Tag> RenderMethodTagGroups = new List<Tag> { new Tag("rmbk"), new Tag("rmcs"), new Tag("rmd "), new Tag("rmfl"), new Tag("rmhg"), new Tag("rmsh"), new Tag("rmss"), new Tag("rmtr"), new Tag("rmw "), new Tag("rmrd"), new Tag("rmct"), new Tag("rmgl") };
		private List<Tag> EffectTagGroups = new List<Tag> { new Tag("beam"), new Tag("cntl"), new Tag("ltvl"), new Tag("decs"), new Tag("prt3") };
        private readonly List<Tag> ResourceTagGroups = new List<Tag> { new Tag("snd!"), new Tag("bitm"), new Tag("Lbsp") }; // for null tag detection

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

            foreach (var tagType in CacheContext.TagCache.TagDefinitions.Types.Keys)
                DefaultTags[tagType.Tag] = CacheContext.TagCache.FindFirstInGroup(tagType.Tag);
		}

		public override object Execute(List<string> args)
		{
			if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            var portingOptions = args.Take(args.Count - 1).ToList();
			ParsePortingOptions(portingOptions);

			var initialStringIdCount = CacheContext.StringTableHaloOnline.Count;
            /*
            if(CacheContext is GameCacheModPackage)
            {
                SetFlags(PortingFlags.Memory);
            }*/


			//
			// Convert Blam data to ElDorado data
			//

			var resourceStreams = new Dictionary<ResourceLocation, Stream>();

            try
            {
                using (var cacheStream = FlagIsSet(PortingFlags.Memory) ? new MemoryStream() : (Stream)CacheContext.OpenCacheReadWrite())
                using (var blamCacheStream = BlamCache.OpenCacheRead())
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
            }
            finally
            {

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

                Matcher.DeInit();
            }

			return true;
		}

        private bool TagIsValid(CachedTag blamTag, Stream blamCacheStream, out CachedTag resultTag)
        {
            resultTag = null;

            if (ResourceTagGroups.Contains(blamTag.Group.Tag))
            {
                // there is only a few cases here -- if geometry\animation references a null resource its tag is still valid
                
                if (blamTag.Group.Tag == "snd!")
                {
                    Sound sound = BlamCache.Deserialize<Sound>(blamCacheStream, blamTag);

                    if (BlamSoundGestalt == null)
                        BlamSoundGestalt = PortingContextFactory.LoadSoundGestalt(BlamCache, blamCacheStream);

                    var xmaFileSize = BlamSoundGestalt.GetFileSize(sound.SoundReference.PitchRangeIndex, sound.SoundReference.PitchRangeCount);
                    if (xmaFileSize < 0)
                        return false;

                    var soundResource = BlamCache.ResourceCache.GetSoundResourceDefinition(sound.Resource);
                    if (soundResource == null)
                        return false;

                    var xmaData = soundResource.Data.Data;
                    if (xmaData == null)
                        return false;
                }
                else if (blamTag.Group.Tag == "bitm")
                {
                    Bitmap bitmap = BlamCache.Deserialize<Bitmap>(blamCacheStream, blamTag);

                    for (int i = 0; i < bitmap.Images.Count; i++)
                    {
                        var image = bitmap.Images[i];

                        // need to assign resource reference to an object here -- otherwise it compiles strangely??
                        object bitmapResourceDefinition;

                        if (image.XboxFlags.HasFlag(TagTool.Bitmaps.BitmapFlagsXbox.UseInterleavedTextures))
                            bitmapResourceDefinition = BlamCache.ResourceCache.GetBitmapTextureInterleavedInteropResource(bitmap.InterleavedResources[image.InterleavedTextureIndex1]);
                        else
                            bitmapResourceDefinition = BlamCache.ResourceCache.GetBitmapTextureInteropResource(bitmap.Resources[i]);

                        if (bitmapResourceDefinition == null)
                            return false;
                    }
                }
                else if (blamTag.Group.Tag == "Lbsp")
                {
                    ScenarioLightmapBspData Lbsp = BlamCache.Deserialize<ScenarioLightmapBspData>(blamCacheStream, blamTag);

                    if (BlamCache.ResourceCache.GetRenderGeometryApiResourceDefinition(Lbsp.Geometry.Resource) == null)
                        return false;
                }
            }
            else if (RenderMethodTagGroups.Contains(blamTag.Group.Tag))
            {
                RenderMethod renderMethod = BlamCache.Deserialize<RenderMethod>(blamCacheStream, blamTag);

                string templateName = renderMethod.ShaderProperties[0].Template.Name;
                TagTool.Shaders.ShaderMatching.ShaderMatcherNew.Rmt2Descriptor.TryParse(templateName, out var rmt2Descriptor);

                foreach (var tag in CacheContext.TagCacheGenHO.TagTable)
                    if (tag != null && tag.Group.Tag == "rmt2" && (tag.Name.Contains(rmt2Descriptor.Type) || FlagIsSet(PortingFlags.GenerateShaders)))
                    {
                        if ((FlagIsSet(PortingFlags.Ms30) && tag.Name.StartsWith("ms30\\")) || (!FlagIsSet(PortingFlags.Ms30) && !tag.Name.StartsWith("ms30\\")))
                            return true;

                        else if (tag.Name.StartsWith("ms30\\"))
                            continue;
                    }

                // TODO: add code for "!MatchShaders" -- if a perfect match isnt found a null tag will be left in the cache

                // "ConvertTagInternal" isnt called so the default shader needs to be set here
                resultTag = GetDefaultShader(blamTag.Group.Tag, resultTag);
                return false;
            }
            else if (blamTag.Group.Tag == "glvs" || blamTag.Group.Tag == "glps" || blamTag.Group.Tag == "rmdf")
                return false; // these tags will be generated in the template generation code

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

                if (TagIsValid(blamTag, blamCacheStream, out result))
                { 
                    var oldFlags = Flags;
                    result = ConvertTagInternal(cacheStream, blamCacheStream, resourceStreams, blamTag);

                    if (result == null)
                        Console.WriteLine("WARNING: null tag allocated in cache");

                    Flags = oldFlags;
                }
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
                        CacheContext.TagCache.TryGetTag($"{defaultSoundName}.{groupTag}", out CachedTag result);
                        return result;
                    }
                    break;

                case "bipd":
                    if (!FlagIsSet(PortingFlags.Elites) && (blamTag.Name.Contains("elite") || blamTag.Name.Contains("dervish")))
                        return null;
                    break;

				case "shit": // use the global shit tag until shit tags are port-able
					if (CacheContext.TagCache.TryGetTag<ShieldImpact>(blamTag.Name, out var shitInstance) && !FlagIsSet(PortingFlags.Replace))
                        return shitInstance;
                    if (BlamCache.Version < CacheVersion.HaloOnline106708)
                        return CacheContext.Deserialize<RasterizerGlobals>(cacheStream, CacheContext.TagCache.GetTag<RasterizerGlobals>(@"globals\rasterizer_globals")).DefaultShieldImpact;
                    break;

                case "sncl" when BlamCache.Version > CacheVersion.HaloOnline700123:
                    return CacheContext.TagCache.GetTag<SoundClasses>(@"sound\sound_classes");

                case "rmdf":
                    return null;
				case "glvs":
                    return null;//CacheContext.TagCache.GetTag<GlobalVertexShader>(@"shaders\shader_shared_vertex_shaders");
				case "glps":
                    return null;// CacheContext.TagCache.GetTag<GlobalPixelShader>(@"shaders\shader_shared_pixel_shaders");
				case "rmct":
                    return CacheContext.TagCache.GetTag<Shader>(@"shaders\invalid");
				case "rmgl":
					return CacheContext.TagCache.GetTag<Shader>(@"levels\dlc\sidewinder\shaders\side_hall_glass03");
				case "rmt2":
                    // match rmt2 with current ones available, else return null
                    return FindClosestRmt2(cacheStream, blamCacheStream, blamTag);
			} 

			//
			// Check to see if the ElDorado tag exists
			//

			CachedTag edTag = null;

            TagGroupGen3 edGroup = (TagGroupGen3)blamTag.Group;

			if (!CacheContext.TagCache.TagDefinitions.TagDefinitionExists(blamTag.Group))
			{
                Console.WriteLine($"Tag group {blamTag.Group} does not exist in destination cache! Returning null!");
                return null;
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
                    if (FlagIsSet(PortingFlags.Replace) && !DoNotReplaceGroups.Contains(instance.Group.Tag.ToString()) 
                        && !DoNotReplaceGroupsCommand.UserDefinedDoNotReplaceGroups.Contains(instance.Group.Tag.ToString()))
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
                    if (BlamCache.Version >= CacheVersion.HaloReach)
                    {
                        sefc.GlobalFlags = sefc.GlobalFlagsReach;
                        sefc.GlobalHiddenFlags = sefc.GlobalHiddenFlagsReach;

                        foreach (var screenEffect in sefc.ScreenEffects)
                        {
                            screenEffect.Flags = screenEffect.FlagsReach;
                            screenEffect.Delay = screenEffect.DelayReach;
                            screenEffect.InputVariable = screenEffect.InputVariableReach;
                            screenEffect.RangeVariable = screenEffect.RangeVariableReach;
                            screenEffect.ObjectFalloff = screenEffect.ObjectFalloffReach;
                            screenEffect.Tron = screenEffect.TronReach;
                            screenEffect.RadialBlur = screenEffect.RadialBlurReach;
                            screenEffect.RadialBlurDirection = screenEffect.RadialBlurDirectionReach;
                            screenEffect.HorizontalBlur = screenEffect.HorizontalBlurReach;
                            screenEffect.VerticalBlur = screenEffect.VerticalBlurReach;
                            screenEffect.Unknown4 = screenEffect.Unknown4Reach;
                            screenEffect.HudTransparency = screenEffect.HudTransparencyReach;
                            screenEffect.FovIn = screenEffect.FovInReach;
                            screenEffect.FovOut = screenEffect.FovOutReach;
                        }
                    }
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
                    foreach (var screenEffect in sefc.ScreenEffects)
                    {
                        //convert flags
                        Enum.TryParse(screenEffect.Flags.ToString(), out screenEffect.Flags_HO);

                        if (screenEffect.InputVariable != null && screenEffect.InputVariable != StringId.Invalid)
                        {
                            //restore ODST stringid input variables using name field to store values
                            screenEffect.Name = ConvertStringId(screenEffect.InputVariable);

                            screenEffect.Flags_HO |= AreaScreenEffect.ScreenEffectBlock.FlagBits_HO.UseNameAsStringIDInput;
                            if (screenEffect.RangeVariable != null && screenEffect.RangeVariable != StringId.Invalid)
                            {
                                screenEffect.Flags_HO |= AreaScreenEffect.ScreenEffectBlock.FlagBits_HO.InvertStringIDInput;
                            }

                            //fixup for vision mode saved film sefc always displaying
                            if (BlamCache.StringTable.GetStringId("saved_film_vision_mode_intensity") == screenEffect.InputVariable)
                                screenEffect.Name = CacheContext.StringTable.GetStringId("flashlight_intensity");
                        }
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
                        foreach (var frame in shot.CameraFrames)
                        {
                            frame.NearFocalPlaneDistance *= -1.0f;
                            frame.FarFocalPlaneDistance *= -1.0f;

                            if (BlamCache.Version == CacheVersion.Halo3ODST)
                                frame.FOV *= 0.65535f; // fov change in ODST affected cisc too it seems
                        }
                    }
                    break;

                case CollisionModel coll:
                    if (BlamCache.Version == CacheVersion.HaloReach)
                    {
                        GenerateCollisionBSPCommand bspgeneration = new GenerateCollisionBSPCommand(ref coll);
                        bspgeneration.Execute(new List<string>());
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
                                    framesblock.Dynamicvalue1 = framesblock.Dynamicvalue2 * 1.5f;
                                    framesblock.Dynamicvalue2 = temp * 1.5f;
                                }
                            }
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
                            //fix weapon target tracking
                            if (weapon.Tracking > 0 || weapon.WeaponType == Weapon.WeaponTypeValue.Needler)
                            {
                                weapon.TargetTracking = new List<Unit.TargetTrackingBlock>{
                                    new Unit.TargetTrackingBlock{
                                        AcquireTime = (weapon.Tracking == Weapon.TrackingType.HumanTracking ? 1.0f : 0.0f),
                                        GraceTime = (weapon.WeaponType == Weapon.WeaponTypeValue.Needler ? 0.2f : 0.1f),
                                        DecayTime = (weapon.WeaponType == Weapon.WeaponTypeValue.Needler ? 0.0f : 0.2f),
                                        TrackingTypes = (weapon.Tracking == Weapon.TrackingType.HumanTracking ?
                                            new List<Unit.TargetTrackingBlock.TrackingType> {
                                                new Unit.TargetTrackingBlock.TrackingType{
                                                    TrackingType2 = CacheContext.StringTable.GetStringId("ground_vehicles")
                                                },
                                                new Unit.TargetTrackingBlock.TrackingType{
                                                    TrackingType2 = CacheContext.StringTable.GetStringId("flying_vehicles")
                                                },
                                            }
                                            :
                                            new List<Unit.TargetTrackingBlock.TrackingType> {
                                                new Unit.TargetTrackingBlock.TrackingType{
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
                        /*case Vehicle vehicle:
                            //fix vehicle weapon target tracking
                            if (vehicle.TargetTracking == null)
                                vehicle.TargetTracking = new List<Unit.TargetTrackingBlock>();
                            foreach (var weaponBlock in vehicle.Weapons)
                            {
                                if (weaponBlock.Weapon2 != null)
                                {
                                    var vehicleWeap = CacheContext.Deserialize<Weapon>(cacheStream, weaponBlock.Weapon2);
                                    if (vehicleWeap.Tracking > 0 && vehicleWeap.TargetTracking.Count > 0)
                                    {
                                        vehicle.TargetTracking.AddRange(vehicleWeap.TargetTracking);
                                    }
                                }
                            }
                            break;*/
                        case Biped biped:
                            // add bipeds filter to "target_main" (fixes needler tracking)
                            if (biped.Model != null)
                            {
                                var hlmt = CacheContext.Deserialize<Model>(cacheStream, biped.Model);

                                foreach (var target in hlmt.Targets)
                                {
                                    if (target.TargetFilter == StringId.Invalid && CacheContext.StringTable.GetString(target.MarkerName) == "target_main")
                                    {
                                        target.TargetFilter = CacheContext.StringTable.GetStringId("bipeds");
                                    }
                                }

                                CacheContext.Serialize(cacheStream, biped.Model, hlmt);
                            }
                            break;
                        default:
                            break;
                    };
                    if (FlagIsSet(PortingFlags.MPobject) && blamDefinition is GameObject obj && obj.MultiplayerObject.Count == 0)
                        obj.MultiplayerObject.Add(new GameObject.MultiplayerObjectBlock() { SpawnTime = 30, AbandonTime = 30 });

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

                case Particle particle:
                    if (BlamCache.Version == CacheVersion.Halo3Retail) // Shift all flags above 2 by 1.
                    {
                        int flagsH3 = (int)particle.FlagsH3;
                        particle.Flags = (Particle.FlagsValue)((flagsH3 & 0x3) + ((int)(flagsH3 & 0xFFFFFFFC) << 1));
                    }
                    else if (BlamCache.Version >= CacheVersion.HaloReach) // Shift all flags above 9 by 2
                    {
                        int flagsReach = particle.AppearanceFlags;
                        particle.AppearanceFlags = ((flagsReach & 0xFF) + ((int)(flagsReach & 0xFFFFFC00) >> 2));
                    }
                    // temp prevent odst prt3 using cheap shader as we dont have the entry point shader
                    if (particle.Flags.HasFlag(Particle.FlagsValue.UsesCheapShader))
                        particle.Flags &= ~Particle.FlagsValue.UsesCheapShader;
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

                case RenderMethodOption rmop when BlamCache.Version == CacheVersion.Halo3ODST || BlamCache.Version >= CacheVersion.HaloReach:
                    foreach (var block in rmop.Options)
                    {
                        if (BlamCache.Version == CacheVersion.Halo3ODST && block.RenderMethodExtern >= RenderMethodExtern.emblem_player_shoulder_texture)
                            block.RenderMethodExtern = (RenderMethodExtern)((int)block.RenderMethodExtern + 2);
                        if (BlamCache.Version >= CacheVersion.HaloReach)
                        {
                            // TODO
                        }
                    }
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
                        return GetDefaultShader(blamTag.Group.Tag, edTag);
                    else
                    {
                        // Verify that the ShaderMatcher is ready to use
                        if (!Matcher.IsInitialized)
                            Matcher.Init(CacheContext, BlamCache, cacheStream, blamCacheStream, FlagIsSet(PortingFlags.Ms30), FlagIsSet(PortingFlags.PefectShaderMatchOnly));

                        blamDefinition = ConvertShader(cacheStream, blamCacheStream, blamDefinition, blamTag, BlamCache.Deserialize(blamCacheStream, blamTag));
                        if (blamDefinition == null) // convert shader failed
                            return GetDefaultShader(blamTag.Group.Tag, edTag);
                    }
                    break;

                case ShaderCortana rmct:
                    if (!FlagIsSet(PortingFlags.MatchShaders))
                        ConvertShaderCortana(rmct, cacheStream, blamCacheStream, resourceStreams);
                    else // invalid for now, TODO: fix this up, rmct shouldnt be a special case
                        return GetDefaultShader(blamTag.Group.Tag, edTag);
                    break;
            }

            //
            // Finalize and serialize the new ElDorado tag definition
            //

            if (blamDefinition == null)
            {
                CacheContext.TagCacheGenHO.Tags[edTag.Index] = null;
                return null;
            }

			CacheContext.Serialize(cacheStream, edTag, blamDefinition);

			if (FlagIsSet(PortingFlags.Print))
				Console.WriteLine($"['{edTag.Group.Tag}', 0x{edTag.Index:X4}] {edTag.Name}.{(edTag.Group as TagGroupGen3).Name}");

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
                        if (prt3Definition.Flags.HasFlag(Particle.FlagsValue.HasAttachment)) // flag bit is always 7 -- this is a post porting fixup
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

				case Part part when BlamCache.Version < CacheVersion.Halo3Retail:
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
                case Part part:
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
                                        GearShiftSound = CacheContext.TagCache.GetTag<Vehicle>(@"sound\vehicles\warthog\warthog_shift")
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
                case CacheVersion.HaloReach:
                    value = damageReportingType.HaloReach.ToString();
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

            if (BlamCache.Version == CacheVersion.HaloReach)
                if (!Enum.TryParse(objectType.HaloReach.ToString(), out objectType.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());

            // todo: properly convert type
            if (BlamCache.Endianness != CacheContext.Endianness && BlamCache.Endianness == EndianFormat.BigEndian)
                objectType.Unknown2 = objectType.Unknown1;
            else if (BlamCache.Endianness != CacheContext.Endianness)
                objectType.Unknown1 = objectType.Unknown2;

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

            if (!CacheContext.TagCache.TryParseGroupTag(tagIdentifiers[1], out var groupTag))
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