using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Tags.Definitions;

namespace TagTool.Tags
{
    public static class TagDefinition
    {
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

        /// <summary>
        /// Finds the structure type corresponding to a group tag.
        /// </summary>
        /// <param name="groupTag">The string representation of the group tag of the group to search for.</param>
        /// <returns>The structure type if found, or <c>null</c> otherwise.</returns>
        public static Type Find(string groupTag) => Find(new Tag(groupTag));

        /// <summary>
        /// Checks to see if a tag definition exists.
        /// </summary>
        /// <param name="groupTag">The group tag of the tag definition.</param>
        /// <returns>true if the tag definition exists.</returns>
        public static bool Exists(Tag groupTag) => Types.ContainsKey(groupTag);

        /// <summary>
        /// Checks to see if a tag definition exists.
        /// </summary>
        /// <param name="groupTag">The group tag of the tag definition.</param>
        /// <returns>true if the tag definition exists.</returns>
        public static bool Exists(string groupTag) => Exists(new Tag(groupTag));

        public static readonly Dictionary<Tag, Type> Types = new Dictionary<Tag, Type>
        {
            { new Tag("<fx>"), typeof(SoundEffectTemplate) },
            { new Tag("achi"), typeof(Achievements) },
            { new Tag("adlg"), typeof(AiDialogueGlobals) },
            { new Tag("aigl"), typeof(AiGlobals) },
            { new Tag("ant!"), typeof(Antenna) },
            { new Tag("argd"), typeof(DeviceArgDevice) },
            { new Tag("armr"), typeof(Armor) },
            { new Tag("arms"), typeof(ArmorSounds) },
            { new Tag("beam"), typeof(BeamSystem) },
            { new Tag("bink"), typeof(Bink) },
            { new Tag("bipd"), typeof(Biped) },
            { new Tag("bitm"), typeof(Bitmap) },
            { new Tag("bkey"), typeof(GuiButtonKeyDefinition) },
            { new Tag("bloc"), typeof(Crate) },
            { new Tag("bmp3"), typeof(GuiBitmapWidgetDefinition) },
            { new Tag("bsdt"), typeof(BreakableSurface) },
            { new Tag("cddf"), typeof(CollisionDamage) },
            { new Tag("cfgt"), typeof(CacheFileGlobalTags) },
            { new Tag("cfxs"), typeof(CameraFxSettings) },
            { new Tag("chad"), typeof(ChudAnimationDefinition) },
            { new Tag("char"), typeof(Character) },
            { new Tag("chdt"), typeof(ChudDefinition) },
            { new Tag("chgd"), typeof(ChudGlobalsDefinition) },
            { new Tag("chmt"), typeof(ChocolateMountainNew) },
            { new Tag("cine"), typeof(Cinematic) },
            { new Tag("cisc"), typeof(CinematicScene) },
            { new Tag("clwd"), typeof(Cloth) },
            { new Tag("cmoe"), typeof(Camo) },
            { new Tag("cntl"), typeof(ContrailSystem) },
            { new Tag("coll"), typeof(CollisionModel) },
            { new Tag("colo"), typeof(ColorTable) },
            { new Tag("cprl"), typeof(ChudWidgetParallaxData) },
            { new Tag("crea"), typeof(Creature) },
            { new Tag("crte"), typeof(CortanaEffectDefinition) },
            { new Tag("ctrl"), typeof(DeviceControl) },
            { new Tag("dctr"), typeof(DecoratorSet) },
            { new Tag("decs"), typeof(DecalSystem) },
            { new Tag("draw"), typeof(RasterizerCacheFileGlobals) },
            { new Tag("drdf"), typeof(DamageResponseDefinition) },
            { new Tag("dsrc"), typeof(GuiDatasourceDefinition) },
            { new Tag("effe"), typeof(Effect) },
            { new Tag("effg"), typeof(EffectGlobals) },
            { new Tag("efsc"), typeof(EffectScenery) },
            { new Tag("eqip"), typeof(Equipment) },
            { new Tag("flck"), typeof(Flock) },
            { new Tag("foot"), typeof(MaterialEffects) },
            { new Tag("forg"), typeof(ForgeGlobalsDefinition) },
            { new Tag("form"), typeof(Formation) },
            { new Tag("gfxt"), typeof(GfxTexturesList) },
            { new Tag("gint"), typeof(Giant) },
            { new Tag("glps"), typeof(GlobalPixelShader) },
            { new Tag("glvs"), typeof(GlobalVertexShader) },
            { new Tag("goof"), typeof(MultiplayerVariantSettingsInterfaceDefinition) },
            { new Tag("gpdt"), typeof(GameProgression) },
            { new Tag("grup"), typeof(GuiGroupWidgetDefinition) },
            { new Tag("hlmt"), typeof(Model) },
            { new Tag("inpg"), typeof(InputGlobals) },
            { new Tag("jmad"), typeof(ModelAnimationGraph) },
            { new Tag("jmrq"), typeof(SandboxTextValuePairDefinition) },
            { new Tag("jpt!"), typeof(DamageEffect) },
            { new Tag("Lbsp"), typeof(ScenarioLightmapBspData) },
            { new Tag("lens"), typeof(LensFlare) },
            { new Tag("ligh"), typeof(Light) },
            { new Tag("lsnd"), typeof(SoundLooping) },
            { new Tag("lst3"), typeof(GuiListWidgetDefinition) },
            { new Tag("lswd"), typeof(LeafSystem) },
            { new Tag("ltvl"), typeof(LightVolumeSystem) },
            { new Tag("mach"), typeof(DeviceMachine) },
            { new Tag("matg"), typeof(Globals) },
            { new Tag("mdl3"), typeof(GuiModelWidgetDefinition) },
            { new Tag("mdlg"), typeof(AiMissionDialogue) },
            { new Tag("mffn"), typeof(Muffin) },
            { new Tag("mode"), typeof(RenderModel) },
            { new Tag("mulg"), typeof(MultiplayerGlobals) },
            { new Tag("nclt"), typeof(NewCinematicLightning) },
            { new Tag("pecp"), typeof(ParticleEmitterCustomPoints) },
            { new Tag("pdm!"), typeof(PodiumSettings) },
            { new Tag("perf"), typeof(PerformanceThrottles) },
            { new Tag("phmo"), typeof(PhysicsModel) },
            { new Tag("pixl"), typeof(PixelShader) },
            { new Tag("play"), typeof(CacheFileResourceLayoutTable) },
            { new Tag("pmdf"), typeof(ParticleModel) },
            { new Tag("pmov"), typeof(ParticlePhysics) },
            { new Tag("pphy"), typeof(PointPhysics) },
            { new Tag("proj"), typeof(Projectile) },
            { new Tag("prt3"), typeof(Particle) },
            { new Tag("rasg"), typeof(RasterizerGlobals) },
            { new Tag("rmbk"), typeof(ShaderBlack) },
            { new Tag("rmcs"), typeof(ShaderCustom) },
            { new Tag("rmct"), typeof(ShaderCortana) },
            { new Tag("rmd "), typeof(ShaderDecal) },
            { new Tag("rmdf"), typeof(RenderMethodDefinition) },
            { new Tag("rmfl"), typeof(ShaderFoliage) },
            { new Tag("rmhg"), typeof(ShaderHalogram) },
            { new Tag("rmop"), typeof(RenderMethodOption) },
            { new Tag("rmsh"), typeof(Shader) },
            { new Tag("rmss"), typeof(ShaderScreen) },
            { new Tag("rmt2"), typeof(RenderMethodTemplate) },
            { new Tag("rmtr"), typeof(ShaderTerrain) },
            { new Tag("rmw "), typeof(ShaderWater) },
            { new Tag("rmzo"), typeof(ShaderZonly) },
            { new Tag("rwrd"), typeof(RenderWaterRipple) },
            { new Tag("sLdT"), typeof(ScenarioLightmap) },
            { new Tag("sbsp"), typeof(ScenarioStructureBsp) },
            { new Tag("scen"), typeof(Scenery) },
            { new Tag("scn3"), typeof(GuiScreenWidgetDefinition) },
            { new Tag("scnr"), typeof(Scenario) },
            { new Tag("sddt"), typeof(StructureDesign) },
            { new Tag("sefc"), typeof(AreaScreenEffect) },
            { new Tag("sfx+"), typeof(SoundEffectCollection) },
            { new Tag("sgp!"), typeof(SoundGlobalPropagation) },
            { new Tag("shit"), typeof(ShieldImpact) },
            { new Tag("siin"), typeof(SimulationInterpolation) },
            { new Tag("sily"), typeof(TextValuePairDefinition) },
            { new Tag("skn3"), typeof(GuiSkinDefinition) },
            { new Tag("skya"), typeof(SkyAtmParameters) },
            { new Tag("smdt"), typeof(SurvivalModeGlobals) },
            { new Tag("sncl"), typeof(SoundClasses) },
            { new Tag("snd!"), typeof(Sound) },
            { new Tag("snde"), typeof(SoundEnvironment) },
            { new Tag("snmx"), typeof(SoundMix) },
            { new Tag("spk!"), typeof(SoundDialogueConstants) },
            { new Tag("sqtm"), typeof(SquadTemplate) },
            { new Tag("ssce"), typeof(SoundScenery) },
            { new Tag("styl"), typeof(Style) },
            { new Tag("sus!"), typeof(SoundUiSounds) },
            { new Tag("term"), typeof(Terminal) },
            { new Tag("trak"), typeof(CameraTrack) },
            { new Tag("trdf"), typeof(TextureRenderList) },
            { new Tag("txt3"), typeof(GuiTextWidgetDefinition) },
            { new Tag("udlg"), typeof(Dialogue) },
            { new Tag("ugh!"), typeof(SoundCacheFileGestalt) },
            { new Tag("uise"), typeof(UserInterfaceSoundsDefinition) },
            { new Tag("unic"), typeof(MultilingualUnicodeStringList) },
            { new Tag("vehi"), typeof(Vehicle) },
            { new Tag("vfsl"), typeof(VFilesList) },
            { new Tag("vmdx"), typeof(VisionMode) },
            { new Tag("vtsh"), typeof(VertexShader) },
            { new Tag("wacd"), typeof(GuiWidgetAnimationCollectionDefinition) },
            { new Tag("wclr"), typeof(GuiWidgetColorAnimationDefinition) },
            { new Tag("weap"), typeof(Weapon) },
            { new Tag("wezr"), typeof(GameEngineSettingsDefinition) },
            { new Tag("wgan"), typeof(GuiWidgetAnimationDefinition) },
            { new Tag("wgtz"), typeof(UserInterfaceGlobalsDefinition) },
            { new Tag("wigl"), typeof(UserInterfaceSharedGlobalsDefinition) },
            { new Tag("wind"), typeof(Wind) },
            { new Tag("wpos"), typeof(GuiWidgetPositionAnimationDefinition) },
            { new Tag("wrot"), typeof(GuiWidgetRotationAnimationDefinition) },
            { new Tag("wscl"), typeof(GuiWidgetScaleAnimationDefinition) },
            { new Tag("wspr"), typeof(GuiWidgetSpriteAnimationDefinition) },
            { new Tag("wtuv"), typeof(GuiWidgetTextureCoordinateAnimationDefinition) },
            { new Tag("zone"), typeof(CacheFileResourceGestalt) }
        };
    }
}
