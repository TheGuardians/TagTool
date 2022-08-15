using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Audio;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Gen3;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Damage;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.Shaders;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Definitions.Common;
using System.Text.RegularExpressions;
using TagTool.IO;
using System.Collections.Concurrent;
using TagTool.Geometry.BspCollisionGeometry;

namespace TagTool.Commands.Porting
{
    public partial class PortTagCommand : Command
	{
		private GameCacheHaloOnlineBase CacheContext { get; }
		private GameCache BlamCache;
		private RenderGeometryConverter GeometryConverter { get; }

		private Dictionary<Tag, List<string>> ReplacedTags = new Dictionary<Tag, List<string>>();
        private Dictionary<CachedTag, object> CachedTagData = new Dictionary<CachedTag, object>();

        private Dictionary<int, CachedTag> PortedTags = new Dictionary<int, CachedTag>();
        private Dictionary<uint, StringId> PortedStringIds = new Dictionary<uint, StringId>();

		private List<Tag> RenderMethodTagGroups = new List<Tag> { new Tag("rmbk"), new Tag("rmcs"), new Tag("rmd "), new Tag("rmfl"), new Tag("rmhg"), new Tag("rmsh"), new Tag("rmss"), new Tag("rmtr"), new Tag("rmw "), new Tag("rmrd"), new Tag("rmct"), new Tag("rmgl") };
		private List<Tag> EffectTagGroups = new List<Tag> { new Tag("beam"), new Tag("cntl"), new Tag("ltvl"), new Tag("decs"), new Tag("prt3") };
        private readonly List<Tag> ResourceTagGroups = new List<Tag> { new Tag("snd!"), new Tag("bitm"), new Tag("Lbsp") }; // for null tag detection

        private DirectoryInfo TempDirectory { get; } = new DirectoryInfo(Path.GetTempPath());
        private BlockingCollection<Action> _deferredActions = new BlockingCollection<Action>();

        string[] argParameters = new string[0];

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

			argParameters = ParsePortingOptions(portingOptions);

			var initialStringIdCount = CacheContext.StringTableHaloOnline.Count;

            InitializeSoundConverter();
            CachedTagData.Clear();

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

                    WaitForPendingSoundConversion();
                    ProcessDeferredActions();
                    if (BlamCache is GameCacheGen3 gen3Cache)
                        gen3Cache.ResourceCacheGen3.ResourcePageCache.Clear();

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

			ProcessDeferredActions();

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

                    if (BlamCache.Platform != CachePlatform.MCC)
                    {
                        if (sound.SoundReference != null)
                        {
                            var xmaFileSize = BlamSoundGestalt.GetFileSize(sound.SoundReference.PitchRangeIndex, sound.SoundReference.PitchRangeCount, BlamCache.Platform);
                            if (xmaFileSize < 0)
                                return false;
                        }

                        var soundResource = BlamCache.ResourceCache.GetSoundResourceDefinition(sound.GetResource(BlamCache.Version, BlamCache.Platform));
                        if (soundResource == null)
                            return false;

                        var xmaData = soundResource.Data.Data;
                        if (xmaData == null)
                            return false;
                    }
                    else
                    {
                        if(sound.Resource.Gen3ResourceID == DatumHandle.None)
                        {
                            new TagToolWarning($"Invalid resource for sound {blamTag.Name}");
                            return false;
                        }
                    }
                }
                else if (blamTag.Group.Tag == "bitm")
                {
                    Bitmap bitmap = BlamCache.Deserialize<Bitmap>(blamCacheStream, blamTag);

                    for (int i = 0; i < bitmap.Images.Count; i++)
                    {
                        var image = bitmap.Images[i];

                        // need to assign resource reference to an object here -- otherwise it compiles strangely??
                        object bitmapResourceDefinition;

                        if (image.XboxFlags.HasFlag(TagTool.Bitmaps.BitmapFlagsXbox.Xbox360UseInterleavedTextures))
                            bitmapResourceDefinition = BlamCache.ResourceCache.GetBitmapTextureInterleavedInteropResource(bitmap.InterleavedHardwareTextures[image.InterleavedInterop]);
                        else
                            bitmapResourceDefinition = BlamCache.ResourceCache.GetBitmapTextureInteropResource(bitmap.HardwareTextures[i]);

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
                if(TagTool.Shaders.ShaderMatching.ShaderMatcherNew.Rmt2Descriptor.TryParse(templateName, out var rmt2Descriptor))
                {
                    foreach (var tag in CacheContext.TagCacheGenHO.TagTable)
                        if (tag != null && tag.Group.Tag == "rmt2" && (tag.Name.Contains(rmt2Descriptor.Type) || FlagIsSet(PortingFlags.GenerateShaders)))
                        {
                            if ((FlagIsSet(PortingFlags.Ms30) && tag.Name.StartsWith("ms30\\")) || (!FlagIsSet(PortingFlags.Ms30) && !tag.Name.StartsWith("ms30\\")))
                                return true;

                            else if (tag.Name.StartsWith("ms30\\"))
                                continue;
                        }
                };
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
                    new TagToolWarning($"null tag allocated for reference \"{blamTag.Name}.{blamTag.Group}\"");

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

        private void ProcessDeferredActions()
        {
            while(_deferredActions.TryTake(out Action action))
            {
                action();
            }
        }

        private void PreConvertReachDefinition(object definition)
        {
            if(definition is ScenarioStructureBsp sbsp)
            {
                if (!PortingOptions.Current.ReachDecorators)
                {
                    sbsp.Decorators.Clear();
                    foreach (var cluster in sbsp.Clusters)
                        cluster.DecoratorGrids.Clear();
                }
                
                foreach(var cluster in sbsp.Clusters)
                {
                    cluster.RuntimeDecalCount = 0;
                    cluster.RuntimeDecalStartIndex = -1;
                }
            }

            if(definition is Scenario scenario)
            {
                scenario.Bipeds.Clear();
                scenario.BipedPalette.Clear();
                //scenario.Vehicles.Clear();
                //scenario.VehiclePalette.Clear();
                //scenario.Weapons.Clear();
                //scenario.WeaponPalette.Clear();
                //scenario.Equipment.Clear();
                //scenario.EquipmentPalette.Clear();
                //scenario.Scenery.Clear();
                //scenario.SceneryPalette.Clear();
                scenario.Terminals.Clear();
                scenario.TerminalPalette.Clear();
                //scenario.Machines.Clear();
                //scenario.MachinePalette.Clear();
                //scenario.Controls.Clear();
                //scenario.ControlPalette.Clear();
                //scenario.Crates.Clear();
                //scenario.CratePalette.Clear();
                scenario.Giants.Clear();
                scenario.GiantPalette.Clear();
                //scenario.EffectScenery.Clear();
                //scenario.EffectSceneryPalette.Clear();
                //scenario.SoundScenery.Clear();
                //scenario.SoundSceneryPalette.Clear();

                scenario.Flocks.Clear();
                scenario.FlockPalette.Clear();
                scenario.Creatures.Clear();
                scenario.CreaturePalette.Clear();

                //scenario.LightVolumes.Clear();
                //scenario.LightVolumePalette.Clear();
                //scenario.DecalPalette.Clear();
                //scenario.Decals.Clear();

                scenario.Squads.Clear();
                scenario.SquadPatrols.Clear();
                scenario.SquadGroups.Clear();
                scenario.AiObjectives.Clear();
                scenario.AiUserHintData.Clear();
                scenario.Scripts.Clear();
                scenario.ScriptStrings = null;

                scenario.CharacterPalette.Clear();
                scenario.UnitSeatsMapping.Clear();
                scenario.MissionScenes.Clear();

                scenario.SkyParameters = null; // unused in reach, we will create a new one
                scenario.PerformanceThrottles = null;
                scenario.GamePerformanceThrottles = null;

                //scenario.ScenarioKillTriggers.Clear();
                scenario.ScenarioSafeTriggers.Clear();

                scenario.PlayerStartingProfile = new List<Scenario.PlayerStartingProfileBlock>() {
                    new Scenario.PlayerStartingProfileBlock() {
                        Name = "start_assault",
                        PrimaryWeapon = CacheContext.TagCache.GetTag(@"objects\weapons\rifle\assault_rifle\assault_rifle", "weap"),
                        PrimaryRoundsLoaded = 32,
                        PrimaryRoundsTotal = 108,
                        StartingFragGrenadeCount = 2
                    }
                };

                Dictionary<string, string> reachObjectives = new Dictionary<string, string>()
                {
                    {"objects\\multi\\models\\mp_hill_beacon\\mp_hill_beacon", "objects\\multi\\koth\\koth_hill_static"},
                    {"objects\\multi\\models\\mp_flag_base\\mp_flag_base", "objects\\multi\\ctf\\ctf_flag_return_area"},
                    {"objects\\multi\\models\\mp_circle\\mp_circle", "objects\\multi\\oddball\\oddball_ball_spawn_point"},
                    {"objects\\multi\\archive\\vip\\vip_boundary", "objects\\multi\\vip\\vip_destination_static"},
                    {"objects\\multi\\spawning\\respawn_zone","objects\\multi\\slayer\\slayer_respawn_zone"},
                    {"objects\\multi\\spawning\\initial_spawn_point","objects\\multi\\slayer\\slayer_initial_spawn_point"}
                };

                Dictionary<string, string> reachVehicles = new Dictionary<string, string>()
                {
                    {"objects\\vehicles\\human\\warthog\\warthog", "objects\\vehicles\\warthog\\warthog"},
                    {"objects\\vehicles\\human\\mongoose\\mongoose", "objects\\vehicles\\mongoose\\mongoose"},
                    {"objects\\vehicles\\human\\scorpion\\scorpion", "objects\\vehicles\\scorpion\\scorpion"},
                    {"objects\\vehicles\\human\\falcon\\falcon", "objects\\vehicles\\hornet\\hornet"},
                    {"objects\\vehicles\\covenant\\ghost\\ghost", "objects\\vehicles\\ghost\\ghost"},
                    {"objects\\vehicles\\covenant\\wraith\\wraith", "objects\\vehicles\\wraith\\wraith"},
                    {"objects\\vehicles\\covenant\\banshee\\banshee", "objects\\vehicles\\banshee\\banshee"},
                    {"objects\\vehicles\\human\\turrets\\machinegun\\machinegun", "objects\\weapons\\turret\\machinegun_turret\\machinegun_turret"},
                    {"objects\\vehicles\\covenant\\turrets\\plasma_turret\\plasma_turret_mounted", "objects\\weapons\\turret\\plasma_cannon\\plasma_cannon"},
                    {"objects\\vehicles\\covenant\\turrets\\shade\\shade", "objects\\vehicles\\shade\\shade"}
                };
		
                Dictionary<string, string> reachEquipment = new Dictionary<string, string>()
                {
                    {"objects\\equipment\\hologram\\hologram", "objects\\equipment\\hologram_equipment\\hologram_equipment"},
                    {"objects\\equipment\\active_camouflage\\active_camouflage", "objects\\equipment\\invisibility_equipment\\invisibility_equipment"}
                };

                ReplaceObjects(scenario.SceneryPalette, reachObjectives);
                ReplaceObjects(scenario.CratePalette, reachObjectives);
                ReplaceObjects(scenario.VehiclePalette, reachVehicles);
                ReplaceObjects(scenario.EquipmentPalette, reachEquipment);

                if (!FlagIsSet(PortingFlags.ReachMisc))
                {
                    CullNewObjects(scenario.SceneryPalette, scenario.Scenery, reachObjectives);
                    CullNewObjects(scenario.CratePalette, scenario.Crates, reachObjectives);
                }

                CullNewObjects(scenario.VehiclePalette, scenario.Vehicles, reachObjectives);
                CullNewObjects(scenario.WeaponPalette, scenario.Weapons, reachObjectives);
                CullNewObjects(scenario.EquipmentPalette, scenario.Equipment, reachObjectives);

                RemoveNullPlacements(scenario.SceneryPalette, scenario.Scenery);
                RemoveNullPlacements(scenario.CratePalette, scenario.Crates);
            }

            //if (definition is SkyAtmParameters skya)
            //{
            //    foreach (SkyAtmParameters.AtmosphereProperty atmProperty in skya.AtmosphereProperties)
            //    {
            //        atmProperty.Name = ConvertStringId(atmProperty.ReachName);
            //        atmProperty.FogColor = atmProperty.FogColorReach;
            //        atmProperty.UnknownFlags = 0;
            //        atmProperty.FogIntensityCyan = 1;
            //        atmProperty.FogIntensityMagenta = 1;
            //        atmProperty.FogIntensityYellow = 1;
            //    }
            //}

            if (definition is Model hlmt)
            {
                foreach (var variant in hlmt.Variants)
                    foreach (var item in variant.Objects)
                        if (item.ChildObject != null)
                            switch ((item.ChildObject.Group as TagGroupGen3).Name)
                            {
                                case "weapon":
                                case "equipment":
                                case "vehicle":
                                    item.ChildObject = null;
                                    break;
                            }
            }

            if (definition is GameObject obj) {
                foreach (var block in obj.MultiplayerObject)
                    if (block.SpawnedObject != null)
                        switch ((block.SpawnedObject.Group as TagGroupGen3).Name) {
                            case "weapon":
                            case "equipment":
                            case "vehicle":
                                block.SpawnedObject = null;
                                break;
                        }
            }

            if (definition is Effect effe) {
                foreach (var block in effe.Events)
                    foreach (var part in block.Parts)
                    {
                        string name = ((TagGroupGen3)part.Type.Group).Name;

                        if (name == "cheap_particle_emitter")
                            part.Type = null;
                        if (name == "decal_system")
                            part.Type = null;
                    }
            }
        }

        public void CullNewObjects<T>(List<Scenario.ScenarioPaletteEntry> palette, List<T> instanceList, Dictionary<string,string> replacements)
        {
            if (palette.Count() > 0)
            {
                foreach (Scenario.ScenarioPaletteEntry block in palette)
                    if (block.Object != null && !CacheContext.TagCache.TryGetTag($"{block.Object.Name}.{block.Object.Group}", out _))
                        block.Object = null;

                RemoveNullPlacements(palette, instanceList);
            }
        }

        public void ReplaceObjects(List<Scenario.ScenarioPaletteEntry> palette, Dictionary<string, string> replacements)
        {
            foreach (var block in palette)
            {
                if (block.Object != null)
                {
                    string name = block.Object.Name;
                    if (replacements.TryGetValue(name, out string result))
                        block.Object.Name = result;
                    else if (name.EndsWith("weak_anti_respawn_zone") || name.EndsWith("weak_respawn_zone") || name.EndsWith("danger_zone"))
                        block.Object = null;
                }
            }
        }

        public void RemoveNullPlacements<T>(List<Scenario.ScenarioPaletteEntry> palette, List<T> instanceList)
        {
            if (palette.Count() > 0)
            {
                List<int> indices = new List<int>();

                foreach (Scenario.ScenarioPaletteEntry block in palette)
                    if (block.Object == null)
                        foreach (var instance in instanceList)
                        {
                            if (!(instance is Scenario.EquipmentInstance) && (instance as Scenario.PermutationInstance).PaletteIndex == palette.IndexOf(block))
                                indices.Add(instanceList.IndexOf(instance));
                        }

                indices.Sort();
                indices.Reverse();
                for (int i = 0; i < indices.Count; i++)
                    instanceList.RemoveAt(indices[i]);
            }
        }

        public CachedTag ConvertTagInternal(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, CachedTag blamTag)
		{
            ProcessDeferredActions();

            if (blamTag == null)
				return null;

			var groupTag = blamTag.Group.Tag;

			// Handle tags that are undesired or not ready to be ported
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

                case "udlg":
                    if (!FlagIsSet(PortingFlags.Dialogue))
                    {
                        PortingConstants.DefaultTagNames.TryGetValue(groupTag, out string defaultUdlgName);
                        CacheContext.TagCache.TryGetTag($"{defaultUdlgName}.{groupTag}", out CachedTag result);
                        return result;
                    }
                    break;

                case "bipd":
                    if (!FlagIsSet(PortingFlags.Elites) && (blamTag.Name.Contains("elite") || blamTag.Name.Contains("dervish")))
                        return null;
                    break;
                case "char" when BlamCache.Version >= CacheVersion.HaloReach:
                    return null;

                case "sncl" when BlamCache.Version > CacheVersion.HaloOnline700123:
                    return CacheContext.TagCache.GetTag<SoundClasses>(@"sound\sound_classes");

                case "rmdf":
                    return null;
				case "glvs":
                    return null;//CacheContext.TagCache.GetTag<GlobalVertexShader>(@"shaders\shader_shared_vertex_shaders");
				case "glps":
                    return null;// CacheContext.TagCache.GetTag<GlobalPixelShader>(@"shaders\shader_shared_pixel_shaders");
				case "rmgl":
					return CacheContext.TagCache.GetTag<Shader>(@"levels\multi\s3d_avalanche\materials\s3d_avalanche_collision_material_55");
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

            if(BlamCache.Version >= CacheVersion.HaloReach)
            {
                PreConvertReachDefinition(blamDefinition);
            }

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
                    if (bsp.InstancedGeometryInstances != null)
                    {
                        foreach (var instance in bsp.InstancedGeometryInstances)
                            instance.Name = StringId.Invalid;
                    }
                    if (bsp.InstancedGeometryInstanceNames != null)
                    {
                        foreach (var instance in bsp.InstancedGeometryInstanceNames)
                            instance.Name = StringId.Invalid;
                    }
                    break;
                case ShieldImpact shit when BlamCache.Version < CacheVersion.HaloOnlineED:
                    shit = PreConvertShieldImpact(shit, BlamCache.Version, CacheContext);
                    // These won't convert automatically due to versioning
                    shit.Plasma.PlasmaNoiseBitmap1 = (CachedTag)ConvertData(cacheStream, blamCacheStream, resourceStreams, shit.Plasma.PlasmaNoiseBitmap1, null, shit.Plasma.PlasmaNoiseBitmap1.Name);
                    shit.Plasma.PlasmaNoiseBitmap2 = (CachedTag)ConvertData(cacheStream, blamCacheStream, resourceStreams, shit.Plasma.PlasmaNoiseBitmap2, null, shit.Plasma.PlasmaNoiseBitmap2.Name);
                    shit.ExtrusionOscillation.OscillationBitmap1 = shit.Plasma.PlasmaNoiseBitmap1;
                    shit.ExtrusionOscillation.OscillationBitmap2 = shit.Plasma.PlasmaNoiseBitmap2;
                    blamDefinition = shit;
                    break;
            }

            ((TagStructure)blamDefinition).PreConvert(BlamCache.Version, CacheContext.Version);

            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                if (blamDefinition is Scenario scnr)
                {
                    var lightmap = BlamCache.Deserialize<ScenarioLightmap>(blamCacheStream, scnr.Lightmap);
                    ConvertReachLightmap(cacheStream, blamCacheStream, resourceStreams, scnr.Lightmap.Name, lightmap);
                }
            }
           

			//
			// Perform automatic conversion on the Blam tag definition
			//

			blamDefinition = ConvertData(cacheStream, blamCacheStream, resourceStreams, blamDefinition, blamDefinition, blamTag.Name);

            //
            // Perform post-conversion fixups to Blam data
            //

            ((TagStructure)blamDefinition).PostConvert(BlamCache.Version, CacheContext.Version);

            bool isDeferred = false;

            switch (blamDefinition)
			{
				case AreaScreenEffect sefc:
					if (BlamCache.Version < CacheVersion.Halo3ODST)
					{
						sefc.GlobalHiddenFlags = AreaScreenEffect.HiddenFlagBits.UpdateThread | AreaScreenEffect.HiddenFlagBits.RenderThread;

						foreach (var screenEffect in sefc.ScreenEffects)
							screenEffect.HiddenFlags = AreaScreenEffect.HiddenFlagBits.UpdateThread | AreaScreenEffect.HiddenFlagBits.RenderThread;
                    }
                    if (sefc.ScreenEffects.Count > 0 && sefc.ScreenEffects[0].Lifetime == 1.0f && sefc.ScreenEffects[0].MaximumDistance == 1.0f)
                    {
                        sefc.ScreenEffects[0].Lifetime = 1E+19f;
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
                    //support bitmap conversion for HO generation caches
                    if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, BlamCache.Version))
                    {
                        for(var tex = 0; tex < bitm.HardwareTextures.Count; tex++)
                        {
                            var blamResourceDefinition = BlamCache.ResourceCache.GetBitmapTextureInteropResource(bitm.HardwareTextures[tex]);
                            bitm.HardwareTextures[tex] = CacheContext.ResourceCache.CreateBitmapResource(blamResourceDefinition);
                        }
                        blamDefinition = bitm;
                        break;
                    }
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
                                frame.FocalLength *= 0.65535f; // fov change in ODST affected cisc too it seems
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
                                    // scale (since this is chud)
                                    framesblock.Dynamicvalue1 *= 1.5f;
                                    framesblock.Dynamicvalue2 *= 1.5f;
                                }
                            }
                        }
                    }
                    break;

                case DamageEffect damageEffect:
                    blamDefinition = ConvertDamageEffect(damageEffect);
                    break;
                case DamageResponseDefinition damageResponse:
                    blamDefinition = ConvertDamageResponseDefinition(blamCacheStream, damageResponse);
                    break;

                case DecoratorSet decoratorSet when BlamCache.Version >= CacheVersion.HaloReach:
                    blamDefinition = ConvertDecoratorSetReach(decoratorSet);
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
                    if (FlagIsSet(PortingFlags.MPobject) && blamDefinition is GameObject obj)
                    {
                        if (obj.MultiplayerObject.Count == 0)
                        {
                            obj.MultiplayerObject.Add(new GameObject.MultiplayerObjectBlock() { DefaultSpawnTime = 30, DefaultAbandonTime = 30 });
                        }

                        if (argParameters.Count() > 0)
                        {
                            int.TryParse(argParameters[0], out int paletteIndex);
                            var objTagName = $"{edTag.Name}.{(edTag.Group as TagGroupGen3).Name}";

                            var paletteItemName = edTag.Name.Split('.').First().Split('\\').Last();
                            if (argParameters.Count() > 1)
                                paletteItemName = argParameters[1].Replace('-', ' ');

                            _deferredActions.Add(() =>
                            {
                                AddForgePaletteItem(cacheStream, objTagName, paletteIndex, paletteItemName);
                            });
                        }
                    }
                    break;

				case Globals matg:
					blamDefinition = ConvertGlobals(matg, cacheStream);
					break;

				case LensFlare lens:
					blamDefinition = ConvertLensFlare(lens);
					break;

                case Light ligh when BlamCache.Version >= CacheVersion.HaloReach:
                    {
                        ligh.DistanceDiffusion = 0.01f;
                        ligh.AngularSmoothness = ligh.MaxIntensityRangeReach;

                        if (ligh.GelBitmap != null)
                        {
                            ligh.Flags |= Light.LightFlags.AllowShadowsAndGels;
                            ligh.FrustumHeightScale = 1;
                            ligh.DistanceDiffusion = 0.0001f;
                            ligh.AngularSmoothness = 0;
                        }
                    }
                    break;

                case Model hlmt:
                    foreach (var target in hlmt.Targets)
                    {
                        if (BlamCache.Version <= CacheVersion.Halo3ODST)
                        {
                            if (target.LockOnFlags.Flags.HasFlag(Model.Target.TargetLockOnFlags.FlagsValue.LockedByHumanTracking))
                                target.TargetFilter = CacheContext.StringTable.GetStringId("flying_vehicles");
                            else if (target.LockOnFlags.Flags.HasFlag(Model.Target.TargetLockOnFlags.FlagsValue.LockedByPlasmaTracking))
                                target.TargetFilter = CacheContext.StringTable.GetStringId("bipeds");
                        }
                    }
                    if(BlamCache.Version >= CacheVersion.HaloReach)
                    {
                        if(hlmt.NewDamageInfo == null || hlmt.NewDamageInfo.Count == 0)
                            hlmt.NewDamageInfo = new List<Model.GlobalDamageInfoBlock>() { ConvertDamageInfoReach(hlmt.OmahaDamageInfo) };
                    }
                    break;
              
				case ModelAnimationGraph jmad:
					blamDefinition = ConvertModelAnimationGraph(cacheStream, blamCacheStream, resourceStreams, jmad);
					break;

				case MultilingualUnicodeStringList unic:
					blamDefinition = ConvertMultilingualUnicodeStringList(cacheStream, blamCacheStream, resourceStreams, unic);
					break;

                case Particle particle:
                    if (BlamCache.Version == CacheVersion.Halo3Retail) // Shift all flags above 5 by 1.
                    {
                        int flagsH3 = (int)particle.FlagsH3;
                        particle.Flags = (Particle.FlagsValue)((flagsH3 & 0x1F) + ((int)(flagsH3 & 0xFFFFFFE0) << 1));
                        //particle.Flags &= ~Particle.FlagsValue.DiesInWater; // h3 particles in odst seem to have this flag unset - does it behave differently?
                    }
                    else if (BlamCache.Version >= CacheVersion.HaloReach) // Shift all flags above 11 by 2
                    {
                        int flagsReach = (int)particle.AppearanceFlagsReach;
                        particle.AppearanceFlags = (Particle.AppearanceFlagsValue)((flagsReach & 0xFF) + ((flagsReach & 0x3F000) >> 2));
                    }
                    // temp prevent odst prt3 using cheap shader as we dont have the entry point shader
                    if (particle.Flags.HasFlag(Particle.FlagsValue.UseCheapShader))
                        particle.Flags &= ~Particle.FlagsValue.UseCheapShader;
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
                    foreach (var block in rmop.Parameters)
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
                    blamDefinition = ConvertGen3RenderModel(edTag, blamTag, mode);
					break;

				case Scenario scnr:
					blamDefinition = ConvertScenario(cacheStream, blamCacheStream, resourceStreams, scnr, blamTag.Name);
					break;

				case ScenarioLightmap sLdT:
                    if(BlamCache.Version < CacheVersion.HaloReach)
					    blamDefinition = ConvertScenarioLightmap(cacheStream, blamCacheStream, resourceStreams, blamTag.Name, sLdT);
                    //fixup lightmap bsp references
                    if (BlamCache.Version >= CacheVersion.HaloReach)
                    {
                        for (short i = 0; i < sLdT.PerPixelLightmapDataReferences.Count; i++)
                        {
                            if(sLdT.PerPixelLightmapDataReferences[i].LightmapBspData != null)
                            {
                                var lbsp = CacheContext.Deserialize<ScenarioLightmapBspData>(cacheStream, sLdT.PerPixelLightmapDataReferences[i].LightmapBspData);
                                lbsp.BspIndex = i;
                                CacheContext.Serialize(cacheStream, sLdT.PerPixelLightmapDataReferences[i].LightmapBspData, lbsp);
                            }
                        }
                    }
                    break;

				case ScenarioLightmapBspData Lbsp:
                    if (BlamCache.Version < CacheVersion.HaloReach)
                        blamDefinition = ConvertScenarioLightmapBspData(Lbsp);
					break;

				case ScenarioStructureBsp sbsp:
                    blamDefinition = ConvertScenarioStructureBsp(sbsp, edTag, resourceStreams);
					break;

                case Sound sound:
                    //support sound conversion for HO generation caches
                    if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, BlamCache.Version))
                    {
                        var blamResourceDefinition = BlamCache.ResourceCache.GetSoundResourceDefinition(sound.Resource);
                        sound.Resource = CacheContext.ResourceCache.CreateSoundResource(blamResourceDefinition);
                        blamDefinition = sound;
                        break;
                    }
                    isDeferred = true;
                    blamDefinition = ConvertSound(cacheStream, blamCacheStream, resourceStreams, sound, edTag, blamTag.Name, (SoundConversionResult result) =>
                    {
                        _deferredActions.Add(() =>
                        {
                            blamDefinition = FinishConvertSound(result);
                            CacheContext.Serialize(cacheStream, edTag, blamDefinition);
                    
                            if (FlagIsSet(PortingFlags.Print))
                                Console.WriteLine($"['{edTag.Group.Tag}', 0x{edTag.Index:X4}] {edTag.Name}.{(edTag.Group as TagGroupGen3).Name}");
                        });
                    });
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
                    Enum.TryParse(sily.ParameterH3.ToString(), out sily.Parameter);
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
                case ShaderCortana rmct:
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
            }

            //
            // Finalize and serialize the new ElDorado tag definition
            //

            if (blamDefinition == null)
            {
                CacheContext.TagCacheGenHO.Tags[edTag.Index] = null;
                return null;
            }

            if (!isDeferred)
            {
                CacheContext.Serialize(cacheStream, edTag, blamDefinition);

                if (FlagIsSet(PortingFlags.Print))
                    Console.WriteLine($"['{edTag.Group.Tag}', 0x{edTag.Index:X4}] {edTag.Name}.{(edTag.Group as TagGroupGen3).Name}");
            }

			return edTag;
		}

        private void AddForgePaletteItem(Stream cacheStream, string gameObjectName, int paletteCategory, string paletteItemName)
        {
            if (CacheContext.TagCache.TryGetCachedTag(@"multiplayer\forge_globals.forge_globals_definition", out CachedTag forge_globals))
                if (CacheContext.TagCache.TryGetCachedTag(gameObjectName, out CachedTag objectTag))
                {
                    var forg = CacheContext.Deserialize<ForgeGlobalsDefinition>(cacheStream, forge_globals);
                    forg.Palette.Add(new ForgeGlobalsDefinition.PaletteItem()
                    {
                        Name = paletteItemName,
                        Type = ForgeGlobalsDefinition.PaletteItemType.Prop,
                        CategoryIndex = (short)paletteCategory,
                        DescriptionIndex = -1,
                        MaxAllowed = 0,
                        Object = objectTag
                    });

                    CacheContext.Serialize(cacheStream, forge_globals, forg);
                }
        }

        private Effect FixupEffect(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, Effect effe, string blamTagName)
        {
            if (BlamCache.Platform == CachePlatform.MCC)
            {
                effe.Flags = effe.FlagsMCC.ConvertLexical<EffectFlags>();
            }

            foreach (var effectEvent in effe.Events)
            {
                if (BlamCache.Platform == CachePlatform.MCC)
                {
                    effectEvent.DurationBounds.Lower *= 2;
                    effectEvent.DurationBounds.Upper *= 2;
                }

                foreach (var particleSystem in effectEvent.ParticleSystems)
                {
                    if (BlamCache.Version < CacheVersion.Halo3ODST) //this value is inverted in ODST tags when compared to H3
                    {
                        particleSystem.NearRange = 1 / particleSystem.NearRange;
                    }

                    if(BlamCache.Version >= CacheVersion.HaloReach)
                    {
                        foreach(var emitter in particleSystem.Emitters)
                        {
                            // Needs to be implemented in the engine
                            if(emitter.EmissionShape >= Effect.Event.ParticleSystem.Emitter.EmissionShapeValue.BoatHullSurface)
                            {
                                switch (emitter.EmissionShape)
                                {
                                    case Effect.Event.ParticleSystem.Emitter.EmissionShapeValue.Cylinder:
                                        emitter.EmissionShape = Effect.Event.ParticleSystem.Emitter.EmissionShapeValue.Tube;
                                        break;
                                    default:
                                        new TagToolWarning($"Unsupported particle emitter shape '{emitter.EmissionShape}'. Using default.");
                                        emitter.EmissionShape = Effect.Event.ParticleSystem.Emitter.EmissionShapeValue.Sprayer;
                                        break;
                                }
                            }

                            if (!Enum.TryParse(emitter.ParticleMovement.FlagsReach.ToString(), out emitter.ParticleMovement.Flags))
                                throw new FormatException(BlamCache.Version.ToString());
                        }
                    }

                    // hack -- for some reason these emitters are killed when using gpu
                    if (BlamCache.Version == CacheVersion.Halo3Retail &&
                        (blamTagName == @"fx\cinematics\070la_waypoint_arrival\01\slipspace_rupture" ||
                        blamTagName == @"fx\cinematics\070la_waypoint_arrival\01\slipspace_rupture_carrier" ||
                        blamTagName == @"fx\cinematics\040lb_cov_flee\08\shot_1\slipspace_rupture" ||
                        blamTagName == @"fx\cinematics\100lb_hc_crash\shot_4\slipspace_rupture"))
                    {
                        particleSystem.Emitters[0].EmitterFlags &= ~Effect.Event.ParticleSystem.Emitter.FlagsValue.IsGpu;
                        particleSystem.Emitters[0].EmitterFlags |= Effect.Event.ParticleSystem.Emitter.FlagsValue.IsCpu;
                    }
                    if (BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        switch (blamTagName)
                        {
                            case @"fx\cinematics\c200\slipspace\slipspace_rupture" when CacheContext.StringTable.GetString(effectEvent.Name) == "rupture":
                            case @"fx\cinematics\l200_out\slipspace\slipspace_rupture" when CacheContext.StringTable.GetString(effectEvent.Name) == "rupture":
                                particleSystem.Emitters[0].EmitterFlags &= ~Effect.Event.ParticleSystem.Emitter.FlagsValue.IsGpu;
                                particleSystem.Emitters[0].EmitterFlags |= Effect.Event.ParticleSystem.Emitter.FlagsValue.IsCpu;
                                break;
                        }
                    }
                }
            }

            return effe;
        }

        private static object ConvertDecoratorSetReach(DecoratorSet decoratorSet)
        {
            switch (decoratorSet.RenderShaderReach)
            {
                case DecoratorSet.DecoratorShaderReach.BillboardWindDynamicLights:
                    decoratorSet.RenderShader = DecoratorSet.DecoratorShader.WindDynamicLights; // default
                    break;
                case DecoratorSet.DecoratorShaderReach.BillboardDynamicLights:
                    decoratorSet.RenderShader = DecoratorSet.DecoratorShader.StillDynamicLights; // no_wind
                    break;
                case DecoratorSet.DecoratorShaderReach.SolidMeshDynamicLights:
                    decoratorSet.RenderShader = DecoratorSet.DecoratorShader.StillDynamicLights; // no_wind
                    break;
                case DecoratorSet.DecoratorShaderReach.SolidMesh:
                    decoratorSet.RenderShader = DecoratorSet.DecoratorShader.StillSunLightOnly; // sun
                    break;
                case DecoratorSet.DecoratorShaderReach.UnderwaterDynamicLights:
                    decoratorSet.RenderShader = DecoratorSet.DecoratorShader.WavyDynamicLights; // wavy
                    break;
                case DecoratorSet.DecoratorShaderReach.VolumetricBillboardDynamicLights:
                    decoratorSet.RenderShader = DecoratorSet.DecoratorShader.ShadedDynamicLights; //shaded
                    break;
                case DecoratorSet.DecoratorShaderReach.VolumetricBillboardWindDynamicLights:
                    decoratorSet.RenderShader = DecoratorSet.DecoratorShader.WindDynamicLights; // unsupported: default + shaded
                    break;
            }

            int lodIndex = 0;
            decoratorSet.LodSettings.StartFade = decoratorSet.LodSettings.TransitionsReach[lodIndex].StartPoint;
            decoratorSet.LodSettings.EndFade = decoratorSet.LodSettings.TransitionsReach[lodIndex].EndPoint;           
            return decoratorSet;
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
                    hkpMoppCode.Data.Elements = HavokConverter.ConvertMoppCodes(BlamCache.Version, BlamCache.Platform, CacheContext.Version, hkpMoppCode.Data.Elements);
                    return hkpMoppCode;

                case PhysicsModel.PhantomTypeFlags phantomTypeFlags:
                    return ConvertPhantomTypeFlags(blamTagName, phantomTypeFlags);

                case PhysicsModel.Shape shape:
                    shape = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, shape, definition, blamTagName);
                    // might be from 3, had no reference
                    shape.ProxyCollisionGroup = shape.ProxyCollisionGroup > 2 ? (sbyte)(shape.ProxyCollisionGroup + 1) : shape.ProxyCollisionGroup;
                    return shape;

                case DamageReportingType damageReportingType:
					return ConvertDamageReportingType(damageReportingType);

                case GameObjectType gameObjectType:
					return ConvertGameObjectType(gameObjectType);

				case ObjectTypeFlags objectTypeFlags:
					return ConvertObjectTypeFlags(objectTypeFlags);

                case GameObject.MultiplayerObjectBlock multiplayer when BlamCache.Version >= CacheVersion.HaloReach:
                    {
                        multiplayer.Type = multiplayer.TypeReach.ConvertLexical<MultiplayerObjectType>();
                        multiplayer.Flags = multiplayer.FlagsReach.ConvertLexical<GameObject.MultiplayerObjectBlock.MultiplayerObjectFlags>();
                        multiplayer.DefaultSpawnTime = multiplayer.SpawnTimeReach;
                        multiplayer.DefaultAbandonTime = multiplayer.AbandonTimeReach;
                        if (multiplayer.DefaultSpawnTime == 0) multiplayer.DefaultSpawnTime = 30;
                        if (multiplayer.DefaultAbandonTime == 0) multiplayer.DefaultAbandonTime = 30;
                        multiplayer.BoundaryShape = multiplayer.ReachBoundaryShape;
                        multiplayer.SpawnTimerType = multiplayer.SpawnTimerTypeReach.ConvertLexical<MultiplayerObjectSpawnTimerType>();
                        return multiplayer;
                    }

                case BipedPhysicsFlags bipedPhysicsFlags:
					return ConvertBipedPhysicsFlags(bipedPhysicsFlags);

				case WeaponFlags weaponFlags:
					return ConvertWeaponFlags(weaponFlags);

                case BarrelFlags barrelflags:
                    return ConvertBarrelFlags(barrelflags);

                case Model.Target.TargetLockOnFlags targetflags:
                    return ConvertTargetFlags(targetflags);

                case RenderMaterial.PropertyType propertyType when BlamCache.Version < CacheVersion.Halo3Retail:
					if (!Enum.TryParse(propertyType.Halo2.ToString(), out propertyType.Halo3))
						throw new NotSupportedException(propertyType.Halo2.ToString());
					return propertyType;

				case RenderMethod renderMethod:
                    return ConvertStructure(cacheStream, blamCacheStream, resourceStreams, renderMethod, definition, blamTagName);

				case ScenarioObjectType scenarioObjectType:
					return ConvertScenarioObjectType(scenarioObjectType);

                case Scenario.MultiplayerObjectProperties scnrObj when BlamCache.Version >= CacheVersion.HaloReach:
                    {
                        scnrObj = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, scnrObj, definition, blamTagName);
                        scnrObj.BoundaryWidthRadius = scnrObj.BoundaryWidthRadiusReach;
                        scnrObj.BoundaryBoxLength = scnrObj.BoundaryBoxLengthReach;
                        scnrObj.BoundaryPositiveHeight = scnrObj.BoundaryPositiveHeightReach;
                        scnrObj.BoundaryNegativeHeight = scnrObj.BoundaryNegativeHeightReach;
                        scnrObj.RemappingPolicy = scnrObj.RemappingPolicyReach;

                        switch (scnrObj.MegaloLabel)
                        {
                            case "ctf_res_zone_away":
                            case "ctf_res_zone":
                            case "ctf_flag_return":
                            case "ctf":
                                scnrObj.EngineFlags |= GameEngineSubTypeFlags.CaptureTheFlag;
                                break;
                            case "slayer":
                                scnrObj.EngineFlags |= GameEngineSubTypeFlags.Slayer;
                                break;
                            case "oddball_ball":
                                scnrObj.EngineFlags |= GameEngineSubTypeFlags.Oddball;
                                break;
                            case "koth_hill":
                                scnrObj.EngineFlags |= GameEngineSubTypeFlags.KingOfTheHill;
                                break;
                            case "terr_object":
                                scnrObj.EngineFlags |= GameEngineSubTypeFlags.Territories;
                                break;
                            case "as_goal": // assault plant point
                            case "as_bomb": // assault bomb spawn
                            case "assault":
                                scnrObj.EngineFlags |= GameEngineSubTypeFlags.Assault;
                                break;
                            case "inf_spawn":
                            case "inf_haven":
                                scnrObj.EngineFlags |= GameEngineSubTypeFlags.Infection;
                                break;
                            case "stp_goal": // use these for juggernaut points
                                scnrObj.EngineFlags |= GameEngineSubTypeFlags.Juggernaut;
                                break;
                            case "stp_flag": // use these for VIP points
                            case "stockpile":
                                scnrObj.EngineFlags |= GameEngineSubTypeFlags.Vip;
                                break;
                            case "ffa_only":
                            case "team_only":
                            case "hh_drop_point":
                            case "none":
                                break;
                            default:
                                if (!string.IsNullOrEmpty(scnrObj.MegaloLabel))
                                    new TagToolWarning($"unknown megalo label: {scnrObj.MegaloLabel}");
                                break;
                        }

                        return data;
                    }

                case SoundClass soundClass:
					return soundClass.ConvertSoundClass(BlamCache.Version);

				case GuiTextWidgetDefinition guiTextWidget:
                    guiTextWidget = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, guiTextWidget, definition, blamTagName);
                    switch (BlamCache.Version)
                    {
                        case CacheVersion.Halo3Retail when BlamCache.Platform == CachePlatform.Original:
                            guiTextWidget.CustomFont = GetEquivalentValue(guiTextWidget.CustomFont, guiTextWidget.CustomFont_H3);
                            break;
                        case CacheVersion.Halo3Retail when BlamCache.Platform == CachePlatform.MCC:
                            guiTextWidget.CustomFont = GetEquivalentValue(guiTextWidget.CustomFont, guiTextWidget.CustomFont_H3MCC);
                            break;
                        case CacheVersion.Halo3ODST:
                            guiTextWidget.CustomFont = GetEquivalentValue(guiTextWidget.CustomFont, guiTextWidget.CustomFont_ODST);
                            break;
                    }
                    return guiTextWidget;

				case Array _:
				case IList _: // All arrays and List<T> implement IList, so we should just use that
					data = ConvertCollection(cacheStream, blamCacheStream, resourceStreams, data as IList, definition, blamTagName);
					return data;

                case CollisionGeometry collisionGeometry:
                    return ConvertCollisionBsp(collisionGeometry);

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
                case PlatformSignedValue _:
                case PlatformUnsignedValue _:
                    return data;

                default:
                    new TagToolWarning($"Unhandled type in `ConvertData`: {data.GetType().Name} (probably harmless).");
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
            }

            return data;
        }

        private T ConvertStructure<T>(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, T data, object definition, string blamTagName) where T : TagStructure
		{
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(data.GetType(), CacheContext.Version, CacheContext.Platform))
            {
                var attr = tagFieldInfo.Attribute;
                if (!CacheVersionDetection.TestAttribute(attr, BlamCache.Version, BlamCache.Platform))
                    continue;

                // skip the field if no conversion is needed
                if ((tagFieldInfo.FieldType.IsValueType && tagFieldInfo.FieldType != typeof(StringId)) || tagFieldInfo.FieldType == typeof(string))
                {
                    if(!tagFieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.GlobalMaterial))
                        continue;
                }
                   
                var oldValue = tagFieldInfo.GetValue(data);
                if (oldValue is null)
                    continue;

                // convert the field
                var newValue = ConvertData(cacheStream, blamCacheStream, resourceStreams, oldValue, definition, blamTagName);

                if(tagFieldInfo.Attribute.Flags.HasFlag(TagFieldFlags.GlobalMaterial))
                    newValue = ConvertGlobalMaterialTypeField(cacheStream, blamCacheStream, tagFieldInfo, newValue);

                tagFieldInfo.SetValue(data, newValue);
            }

            return UpgradeStructure(cacheStream, resourceStreams, data, definition, blamTagName);
		}

        private object ConvertGlobalMaterialTypeField(Stream cacheStream, Stream blamCacheStream, TagFieldInfo fieldinfo, object value)
        {
            // only enabled for reach, however it might be worth using for h3 and odst as a fallback
            if (BlamCache.Version < CacheVersion.HaloReach)
                return value;

            var globals = DeserializeTagCached<Globals>(CacheContext, cacheStream, CacheContext.TagCache.FindFirstInGroup<Globals>());
            var blamGlobals = DeserializeTagCached<Globals>(BlamCache, blamCacheStream, BlamCache.TagCache.FindFirstInGroup<Globals>());

            var materials = globals.Materials;
            var blamMaterials = BlamCache.Version >= CacheVersion.HaloReach ? blamGlobals.AlternateMaterials : blamGlobals.Materials;
            return ConvertInternal(value);

            object ConvertInternal(object val)
            {
                switch (val)
                {
                    case StringId stringId:
                        if (stringId != StringId.Invalid)
                            val = materials[FindMatchingMaterial(CacheContext.StringTable.GetString(stringId))].Name;
                        break;
                    case short index:
                        if (index != -1)
                        {
                            if (index < 0 || index >= blamMaterials.Count)
                            {
                                index = 0;
                                new TagToolWarning($"Global material type was out of range for '{fieldinfo.DeclaringType.FullName}.{fieldinfo.Name}', value: {index}");
                            }
                            else
                            {
                                val = FindMatchingMaterial(BlamCache.StringTable.GetString(blamMaterials[index].Name));
                            }
                        }
                        break;
                    case short[] indices:
                        for (int i = 0; i < indices.Length; i++)
                            indices[i] = (short)ConvertInternal(indices[i]);
                        break;
                    case StringId[] stringIds:
                        for (int i = 0; i < stringIds.Length; i++)
                            stringIds[i] = (StringId)ConvertInternal(stringIds[i]);
                        break;
                }
                return val;
            }

            short FindMatchingMaterial(string name)
            {
                var originalName = name;

                // we don't have wet materials
                if (name.StartsWith("wet_"))
                    name = name.Substring(4);

                // search for the name in the destination materials
                var matchIndex = (short)materials.FindIndex(x => CacheContext.StringTable.GetString(x.Name) == name);
                if (matchIndex != -1)
                {
                    if(name != originalName)
                        new TagToolWarning($"Failed to find global material type '{originalName}', using '{name}' instead");

                    return matchIndex;
                }
                    
                // we couldn't find it, find the index in the source materials
                var blamIndex = blamMaterials.FindIndex(x => BlamCache.StringTable.GetString(x.Name) == originalName);
                if (blamIndex == -1)
                {
                    if (!originalName.StartsWith("default"))
                        new TagToolWarning($"Failed to find global material type '{originalName}', using 'default_material'");
                    return 0;
                }

                // if it has a parent search for its name
                StringId parentName = blamMaterials[blamIndex].ParentName;
                if (parentName == StringId.Invalid)
                    return 0;

                // recurse
                matchIndex = FindMatchingMaterial(BlamCache.StringTable.GetString(parentName));

                // if we still can't find anything after walking up the hierarchy, use 'default_material'
                if (matchIndex == -1)
                    matchIndex = 0;

                name = CacheContext.StringTable.GetString(materials[matchIndex].Name);
                new TagToolWarning($"Failed to find global material type '{originalName}', using '{name}' instead");
                return matchIndex;
            }
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

        private object ConvertTargetFlags(Model.Target.TargetLockOnFlags target)
        {
            if (BlamCache.Version <= CacheVersion.Halo3ODST)
                if (!Enum.TryParse(target.Flags.ToString(), out target.Flags_HO))
                    throw new FormatException(BlamCache.Version.ToString());

            return target;
        }

        private PhysicsModel.PhantomTypeFlags ConvertPhantomTypeFlags(string tagName, PhysicsModel.PhantomTypeFlags flags)
        {
            switch (BlamCache.Version)
            {
                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                    if (flags.Halo2.ToString().Contains("Unknown"))
                    {
                        new TagToolWarning($"Disabling unknown phantom type flags ({flags.Halo2.ToString()})");
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
                        new TagToolWarning($"Found unknown phantom type flags ({flags.Halo3Retail.ToString()})");
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

		private TagFunction ConvertTagFunction(TagFunction function)
		{
			return TagFunction.ConvertTagFunction(CacheVersionDetection.IsLittleEndian(BlamCache.Version, BlamCache.Platform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian, function);
		}

        private GameObjectType ConvertGameObjectType(GameObjectType objectType)
		{
            if (BlamCache.Version <= CacheVersion.Halo2Vista)
                if (!Enum.TryParse(objectType.Halo2.ToString(), out objectType.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());

            if (BlamCache.Version == CacheVersion.Halo3Retail)
				if (!Enum.TryParse(objectType.Halo3Retail.ToString(), out objectType.Halo3ODST))
					throw new FormatException(BlamCache.Version.ToString());

            if (CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, BlamCache.Version))
                if (!Enum.TryParse(objectType.HaloOnline.ToString(), out objectType.Halo3ODST))
                    throw new FormatException(BlamCache.Version.ToString());

            if (BlamCache.Version >= CacheVersion.HaloReach)
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

            if (BlamCache.Version >= CacheVersion.HaloReach)
                if (!Enum.TryParse(objectType.HaloReach.ToString(), out objectType.Halo3ODST))
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
                    new TagToolError(CommandError.CustomError, $"Invalid regex: {tagSpecifier}");
                    return new List<CachedTag>();
                }
                return result;
            }

            if (tagSpecifier.Length == 0 || (!char.IsLetter(tagSpecifier[0]) && !tagSpecifier.Contains('*')) || !tagSpecifier.Contains('.'))
            {
                new TagToolError(CommandError.CustomError, $"Invalid tag name: {tagSpecifier}");
                return new List<CachedTag>();
            }

            var tagIdentifiers = tagSpecifier.Split('.');

            if (!CacheContext.TagCache.TryParseGroupTag(tagIdentifiers[1], out var groupTag))
            {
                new TagToolError(CommandError.CustomError, $"Invalid tag name: {tagSpecifier}");
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
                new TagToolError(CommandError.CustomError, $"Invalid tag name: {tagSpecifier}");
                return new List<CachedTag>();
            }

            return result;
        }

        private T DeserializeTagCached<T>(GameCache cache, Stream stream, CachedTag tag)
        {
            return (T)DeserializeTagCached(cache, stream, tag);
        }

        private object DeserializeTagCached(GameCache cache, Stream stream, CachedTag tag)
        {
            if (!CachedTagData.TryGetValue(tag, out object value))
                CachedTagData.Add(tag, value = cache.Deserialize(stream, tag));
            return value;
        }
    }
}
