using System;
using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Tags
{
    public abstract class TagDefinition : TagStructure
    {
        /// <summary>
        /// Gets the group tag of the tag definition.
        /// </summary>
        public abstract Tag GroupTag { get; }

        /// <summary>
        /// Gets the group name of the tag definition.
        /// </summary>
        public abstract string GroupName { get; }

        /// <summary>
        /// Finds the structure type corresponding to a group tag.
        /// </summary>
        /// <param name="groupTag">The group tag of the group to search for.</param>
        /// <returns>The structure type if found, or <c>null</c> otherwise.</returns>
        public static Type Find(Tag groupTag)
		{
			Types.TryGetValue(groupTag, out var result);
			return result;
		}

		public static bool TryFind(Tag groupTag, out Type result) =>
			Types.TryGetValue(groupTag, out result);

		/// <summary>
		/// Finds the structure type corresponding to a group.
		/// </summary>
		/// <param name="group">The group to search for.</param>
		/// <returns>The structure type if found, or <c>null</c> otherwise.</returns>
		public static Type Find(TagGroup group) => Find(group.Tag);

		public static bool TryFind(TagGroup group, out Type result) =>
			TryFind(group.Tag, out result);

		/// <summary>
		/// Finds the structure type corresponding to a group tag.
		/// </summary>
		/// <param name="group">The group name or group tag of the tag definition.</param>
		/// <returns>The structure type if found, or <c>null</c> otherwise.</returns>
		public static Type Find(string group)
		{
			foreach (var entry in Types)
			{
				var type = entry.Value;
				var attribute = TagStructure.GetTagStructureAttribute(type);
				if (attribute.Name == group)
					return type;
			}

			return Find(new Tag(group));
		}

		public static bool TryFind(string group, out Type result)
		{
			foreach (var entry in Types)
			{
				var type = entry.Value;
				var attribute = TagStructure.GetTagStructureAttribute(type);

				if (attribute.Name == group)
				{
					result = type;
					return true;
				}
			}

			return TryFind(new Tag(group), out result);
		}

		/// <summary>
		/// Checks to see if a tag definition exists.
		/// </summary>
		/// <param name="groupTag">The group tag of the tag definition.</param>
		/// <returns>true if the tag definition exists.</returns>
		public static bool Exists(Tag groupTag) => Types.ContainsKey(groupTag);

		/// <summary>
		/// Checks to see if a tag definition exists.
		/// </summary>
		/// <param name="group">The group name or group tag of the tag definition.</param>
		/// <returns>true if the tag definition exists.</returns>
		public static bool Exists(string group)
		{
			foreach (var entry in Types)
			{
				var type = entry.Value;
				var attribute = TagStructure.GetTagStructureAttribute(type);

				if (attribute.Name == group)
					return true;
			}

			return Exists(new Tag(group));
		}

		public static Tag GetGroupTag<T>()
		{
			var attribute = TagStructure.GetTagStructureAttribute(typeof(T));
			return new Tag(attribute.Tag);
		}

        public static readonly Dictionary<Tag, Type> Types = new Dictionary<Tag, Type>
        {
            { Tag._FX_, typeof(SoundEffectTemplate) },
            { Tag.ACHI, typeof(Achievements) },
            { Tag.ADLG, typeof(AiDialogueGlobals) },
            { Tag.AIGL, typeof(AiGlobals) },
            { Tag.ANT_, typeof(Antenna) },
            { Tag.ARGD, typeof(DeviceArgDevice) },
            { Tag.ARMR, typeof(Armor) },
            { Tag.ARMS, typeof(ArmorSounds) },
            { Tag.BEAM, typeof(BeamSystem) },
            { Tag.BINK, typeof(Bink) },
            { Tag.BIPD, typeof(Biped) },
            { Tag.BITM, typeof(Bitmap) },
            { Tag.BKEY, typeof(GuiButtonKeyDefinition) },
            { Tag.BLOC, typeof(Crate) },
            { Tag.BMP3, typeof(GuiBitmapWidgetDefinition) },
            { Tag.BSDT, typeof(BreakableSurface) },
            { Tag.CDDF, typeof(CollisionDamage) },
            { Tag.CFGT, typeof(CacheFileGlobalTags) },
            { Tag.CFXS, typeof(CameraFxSettings) },
            { Tag.CHAD, typeof(ChudAnimationDefinition) },
            { Tag.CHAR, typeof(Character) },
            { Tag.CHDT, typeof(ChudDefinition) },
            { Tag.CHGD, typeof(ChudGlobalsDefinition) },
            { Tag.CHMT, typeof(ChocolateMountainNew) },
            { Tag.CINE, typeof(Cinematic) },
            { Tag.CISC, typeof(CinematicScene) },
            { Tag.CLWD, typeof(Cloth) },
            { Tag.CMOE, typeof(Camo) },
            { Tag.CNTL, typeof(ContrailSystem) },
            { Tag.COLL, typeof(CollisionModel) },
            { Tag.COLO, typeof(ColorTable) },
            { Tag.CPRL, typeof(ChudWidgetParallaxData) },
            { Tag.CREA, typeof(Creature) },
            { Tag.CRTE, typeof(CortanaEffectDefinition) },
            { Tag.CTRL, typeof(DeviceControl) },
            { Tag.DCTR, typeof(DecoratorSet) },
            { Tag.DECS, typeof(DecalSystem) },
            { Tag.DRAW, typeof(RasterizerCacheFileGlobals) },
            { Tag.DRDF, typeof(DamageResponseDefinition) },
            { Tag.DSRC, typeof(GuiDatasourceDefinition) },
            { Tag.EFFE, typeof(Effect) },
            { Tag.EFFG, typeof(EffectGlobals) },
            { Tag.EFSC, typeof(EffectScenery) },
            { Tag.EQIP, typeof(Equipment) },
            { Tag.FLCK, typeof(Flock) },
            { Tag.FOOT, typeof(MaterialEffects) },
            { Tag.FORG, typeof(ForgeGlobalsDefinition) },
            { Tag.FORM, typeof(Formation) },
            { Tag.GFXT, typeof(GfxTexturesList) },
            { Tag.GINT, typeof(Giant) },
            { Tag.GLPS, typeof(GlobalPixelShader) },
            { Tag.GLVS, typeof(GlobalVertexShader) },
            { Tag.GOOF, typeof(MultiplayerVariantSettingsInterfaceDefinition) },
            { Tag.GPDT, typeof(GameProgression) },
            { Tag.GRUP, typeof(GuiGroupWidgetDefinition) },
            { Tag.HLMT, typeof(Model) },
            { Tag.INPG, typeof(InputGlobals) },
            { Tag.JMAD, typeof(ModelAnimationGraph) },
            { Tag.JMRQ, typeof(SandboxTextValuePairDefinition) },
            { Tag.JPT_, typeof(DamageEffect) },
            { Tag.LBSP, typeof(ScenarioLightmapBspData) },
            { Tag.LENS, typeof(LensFlare) },
            { Tag.LIGH, typeof(Light) },
            { Tag.LSND, typeof(SoundLooping) },
            { Tag.LST3, typeof(GuiListWidgetDefinition) },
            { Tag.LSWD, typeof(LeafSystem) },
            { Tag.LTVL, typeof(LightVolumeSystem) },
            { Tag.MACH, typeof(DeviceMachine) },
            { Tag.MATG, typeof(Globals) },
            { Tag.MDL3, typeof(GuiModelWidgetDefinition) },
            { Tag.MDLG, typeof(AiMissionDialogue) },
            { Tag.MFFN, typeof(Muffin) },
            { Tag.MODE, typeof(RenderModel) },
            { Tag.MULG, typeof(MultiplayerGlobals) },
            { Tag.NCLT, typeof(NewCinematicLighting) },
            { Tag.OBJE, typeof(GameObject) },
            { Tag.PECP, typeof(ParticleEmitterCustomPoints) },
            { Tag.PDM_, typeof(PodiumSettings) },
            { Tag.PERF, typeof(PerformanceThrottles) },
            { Tag.PHMO, typeof(PhysicsModel) },
            { Tag.PIXL, typeof(PixelShader) },
            { Tag.PLAY, typeof(CacheFileResourceLayoutTable) },
            { Tag.PMDF, typeof(ParticleModel) },
            { Tag.PMOV, typeof(ParticlePhysics) },
            { Tag.PPHY, typeof(PointPhysics) },
            { Tag.PROJ, typeof(Projectile) },
            { Tag.PRT3, typeof(Particle) },
            { Tag.RASG, typeof(RasterizerGlobals) },
            { Tag.RM__, typeof(RenderMethod) },
            { Tag.RMBK, typeof(ShaderBlack) },
            { Tag.RMCS, typeof(ShaderCustom) },
            { Tag.RMCT, typeof(ShaderCortana) },
            { Tag.RMD_, typeof(ShaderDecal) },
            { Tag.RMDF, typeof(RenderMethodDefinition) },
            { Tag.RMFL, typeof(ShaderFoliage) },
            { Tag.RMHG, typeof(ShaderHalogram) },
            { Tag.RMOP, typeof(RenderMethodOption) },
            { Tag.RMSH, typeof(Shader) },
            { Tag.RMSS, typeof(ShaderScreen) },
            { Tag.RMT2, typeof(RenderMethodTemplate) },
            { Tag.RMTR, typeof(ShaderTerrain) },
            { Tag.RMW_, typeof(ShaderWater) },
            { Tag.RMZO, typeof(ShaderZonly) },
            { Tag.RWRD, typeof(RenderWaterRipple) },
            { Tag.SLDT, typeof(ScenarioLightmap) },
            { Tag.SBSP, typeof(ScenarioStructureBsp) },
            { Tag.SCEN, typeof(Scenery) },
            { Tag.SCN3, typeof(GuiScreenWidgetDefinition) },
            { Tag.SCNR, typeof(Scenario) },
            { Tag.SDDT, typeof(StructureDesign) },
            { Tag.SEFC, typeof(AreaScreenEffect) },
            { Tag.SFX_, typeof(SoundEffectCollection) },
            { Tag.SGP_, typeof(SoundGlobalPropagation) },
            { Tag.SHIT, typeof(ShieldImpact) },
            { Tag.SIIN, typeof(SimulationInterpolation) },
            { Tag.SILY, typeof(TextValuePairDefinition) },
            { Tag.SKN3, typeof(GuiSkinDefinition) },
            { Tag.SKYA, typeof(SkyAtmParameters) },
            { Tag.SMDT, typeof(SurvivalModeGlobals) },
            { Tag.SNCL, typeof(SoundClasses) },
            { Tag.SND_, typeof(Sound) },
            { Tag.SNDE, typeof(SoundEnvironment) },
            { Tag.SNMX, typeof(SoundMix) },
            { Tag.SPK_, typeof(SoundDialogueConstants) },
            { Tag.SQTM, typeof(SquadTemplate) },
            { Tag.SSCE, typeof(SoundScenery) },
            { Tag.STYL, typeof(Style) },
            { Tag.SUS_, typeof(SoundUiSounds) },
            { Tag.TERM, typeof(Terminal) },
            { Tag.TRAK, typeof(CameraTrack) },
            { Tag.TRDF, typeof(TextureRenderList) },
            { Tag.TXT3, typeof(GuiTextWidgetDefinition) },
            { Tag.UDLG, typeof(Dialogue) },
            { Tag.UGH_, typeof(SoundCacheFileGestalt) },
            { Tag.UISE, typeof(UserInterfaceSoundsDefinition) },
            { Tag.UNIC, typeof(MultilingualUnicodeStringList) },
            { Tag.VEHI, typeof(Vehicle) },
            { Tag.VFSL, typeof(VFilesList) },
            { Tag.VMDX, typeof(VisionMode) },
            { Tag.VTSH, typeof(VertexShader) },
            { Tag.WACD, typeof(GuiWidgetAnimationCollectionDefinition) },
            { Tag.WCLR, typeof(GuiWidgetColorAnimationDefinition) },
            { Tag.WEAP, typeof(Weapon) },
            { Tag.WEZR, typeof(GameEngineSettingsDefinition) },
            { Tag.WGAN, typeof(GuiWidgetAnimationDefinition) },
            { Tag.WGTZ, typeof(UserInterfaceGlobalsDefinition) },
            { Tag.WIGL, typeof(UserInterfaceSharedGlobalsDefinition) },
            { Tag.WIND, typeof(Wind) },
            { Tag.WPOS, typeof(GuiWidgetPositionAnimationDefinition) },
            { Tag.WROT, typeof(GuiWidgetRotationAnimationDefinition) },
            { Tag.WSCL, typeof(GuiWidgetScaleAnimationDefinition) },
            { Tag.WSPR, typeof(GuiWidgetSpriteAnimationDefinition) },
            { Tag.WTUV, typeof(GuiWidgetTextureCoordinateAnimationDefinition) },
            { Tag.ZONE, typeof(CacheFileResourceGestalt) }
        };
    }
}