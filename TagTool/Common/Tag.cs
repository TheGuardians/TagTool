using System;
using System.Text;

namespace TagTool.Common
{
    /// <summary>
    /// Represents a magic number which looks like a string.
    /// </summary>
    public struct Tag : IComparable<Tag>, IBlamType
	{
		#region Tags
		/// <summary>
		/// Tag: "ÿÿÿÿ" - The null tag representation.
		/// </summary>
		public static Tag NULL { get; } = new Tag("ÿÿÿÿ");

		/// <summary>
		/// Tag: "&lt;fx&gt;", Name: "sound_effect_template", Definition: "<see cref="TagTool.Tags.Definitions.SoundEffectTemplate"/>".
		/// </summary>
		public static Tag _FX_ { get; } = new Tag("<fx>");

		/// <summary>
		/// Tag: "achi", Name: "achievements", Definition: "<see cref="TagTool.Tags.Definitions.Achievements"/>".
		/// </summary>
		public static Tag ACHI { get; } = new Tag("achi");

		/// <summary>
		/// Tag: "adlg", Name: "ai_dialogue_globals", Definition: "<see cref="TagTool.Tags.Definitions.AiDialogueGlobals"/>".
		/// </summary>
		public static Tag ADLG { get; } = new Tag("adlg");

		/// <summary>
		/// Tag: "aigl", Name: "ai_globals", Definition: "<see cref="TagTool.Tags.Definitions.AiGlobals"/>".
		/// </summary>
		public static Tag AIGL { get; } = new Tag("aigl");

		/// <summary>
		/// Tag: "ant!", Name: "antenna", Definition: "<see cref="TagTool.Tags.Definitions.Antenna"/>".
		/// </summary>
		public static Tag ANT_ { get; } = new Tag("ant!");

		/// <summary>
		/// Tag: "argd", Name: "device_arg_device", Definition: "<see cref="TagTool.Tags.Definitions.DeviceArgDevice"/>".
		/// </summary>
		public static Tag ARGD { get; } = new Tag("argd");

		/// <summary>
		/// Tag: "armr", Name: "armor", Definition: "<see cref="TagTool.Tags.Definitions.Armor"/>".
		/// </summary>
		public static Tag ARMR { get; } = new Tag("armr");

		/// <summary>
		/// Tag: "arms", Name: "armor_sounds", Definition: "<see cref="TagTool.Tags.Definitions.ArmorSounds"/>".
		/// </summary>
		public static Tag ARMS { get; } = new Tag("arms");

		/// <summary>
		/// Tag: "beam", Name: "beam_system", Definition: "<see cref="TagTool.Tags.Definitions.BeamSystem"/>".
		/// </summary>
		public static Tag BEAM { get; } = new Tag("beam");

		/// <summary>
		/// Tag: "bink", Name: "bink", Definition: "<see cref="TagTool.Tags.Definitions.Bink"/>".
		/// </summary>
		public static Tag BINK { get; } = new Tag("bink");

		/// <summary>
		/// Tag: "bipd", Name: "biped", Definition: "<see cref="TagTool.Tags.Definitions.Biped"/>".
		/// </summary>
		public static Tag BIPD { get; } = new Tag("bipd");

		/// <summary>
		/// Tag: "bitm", Name: "bitmap", Definition: "<see cref="TagTool.Tags.Definitions.Bitmap"/>".
		/// </summary>
		public static Tag BITM { get; } = new Tag("bitm");

		/// <summary>
		/// Tag: "bkey", Name: "gui_button_key_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiButtonKeyDefinition"/>".
		/// </summary>
		public static Tag BKEY { get; } = new Tag("bkey");

		/// <summary>
		/// Tag: "bloc", Name: "crate", Definition: "<see cref="TagTool.Tags.Definitions.Crate"/>".
		/// </summary>
		public static Tag BLOC { get; } = new Tag("bloc");

		/// <summary>
		/// Tag: "bmp3", Name: "gui_bitmap_widget_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiBitmapWidgetDefinition"/>".
		/// </summary>
		public static Tag BMP3 { get; } = new Tag("bmp3");

		/// <summary>
		/// Tag: "bsdt", Name: "breakable_surface", Definition: "<see cref="TagTool.Tags.Definitions.BreakableSurface"/>".
		/// </summary>
		public static Tag BSDT { get; } = new Tag("bsdt");

		/// <summary>
		/// Tag: "cddf", Name: "collision_damage", Definition: "<see cref="TagTool.Tags.Definitions.CollisionDamage"/>".
		/// </summary>
		public static Tag CDDF { get; } = new Tag("cddf");

		/// <summary>
		/// Tag: "cfgt", Name: "cache_file_global_tags", Definition: "<see cref="TagTool.Tags.Definitions.CacheFileGlobalTags"/>".
		/// </summary>
		public static Tag CFGT { get; } = new Tag("cfgt");

		/// <summary>
		/// Tag: "cfxs", Name: "camera_fx_settings", Definition: "<see cref="TagTool.Tags.Definitions.CameraFxSettings"/>".
		/// </summary>
		public static Tag CFXS { get; } = new Tag("cfxs");

		/// <summary>
		/// Tag: "chad", Name: "chud_animation_definition", Definition: "<see cref="TagTool.Tags.Definitions.ChudAnimationDefinition"/>".
		/// </summary>
		public static Tag CHAD { get; } = new Tag("chad");

		/// <summary>
		/// Tag: "char", Name: "character", Definition: "<see cref="TagTool.Tags.Definitions.Character"/>".
		/// </summary>
		public static Tag CHAR { get; } = new Tag("char");

		/// <summary>
		/// Tag: "chdt", Name: "chud_definition", Definition: "<see cref="TagTool.Tags.Definitions.ChudDefinition"/>".
		/// </summary>
		public static Tag CHDT { get; } = new Tag("chdt");

		/// <summary>
		/// Tag: "chgd", Name: "chud_globals_definition", Definition: "<see cref="TagTool.Tags.Definitions.ChudGlobalsDefinition"/>".
		/// </summary>
		public static Tag CHGD { get; } = new Tag("chgd");

		/// <summary>
		/// Tag: "chmt", Name: "chocolate_mountain_new", Definition: "<see cref="TagTool.Tags.Definitions.ChocolateMountainNew"/>".
		/// </summary>
		public static Tag CHMT { get; } = new Tag("chmt");

		/// <summary>
		/// Tag: "cine", Name: "cinematic", Definition: "<see cref="TagTool.Tags.Definitions.Cinematic"/>".
		/// </summary>
		public static Tag CINE { get; } = new Tag("cine");

		/// <summary>
		/// Tag: "cisc", Name: "cinematic_scene", Definition: "<see cref="TagTool.Tags.Definitions.CinematicScene"/>".
		/// </summary>
		public static Tag CISC { get; } = new Tag("cisc");

		/// <summary>
		/// Tag: "clwd", Name: "cloth", Definition: "<see cref="TagTool.Tags.Definitions.Cloth"/>".
		/// </summary>
		public static Tag CLWD { get; } = new Tag("clwd");

		/// <summary>
		/// Tag: "cmoe", Name: "camo", Definition: "<see cref="TagTool.Tags.Definitions.Camo"/>".
		/// </summary>
		public static Tag CMOE { get; } = new Tag("cmoe");

		/// <summary>
		/// Tag: "cntl", Name: "contrail_system", Definition: "<see cref="TagTool.Tags.Definitions.ContrailSystem"/>".
		/// </summary>
		public static Tag CNTL { get; } = new Tag("cntl");

		/// <summary>
		/// Tag: "coll", Name: "collision_model", Definition: "<see cref="TagTool.Tags.Definitions.CollisionModel"/>".
		/// </summary>
		public static Tag COLL { get; } = new Tag("coll");

		/// <summary>
		/// Tag: "colo", Name: "color_table", Definition: "<see cref="TagTool.Tags.Definitions.ColorTable"/>".
		/// </summary>
		public static Tag COLO { get; } = new Tag("colo");

		/// <summary>
		/// Tag: "cprl", Name: "chud_widget_parallax_data", Definition: "<see cref="TagTool.Tags.Definitions.ChudWidgetParallaxData"/>".
		/// </summary>
		public static Tag CPRL { get; } = new Tag("cprl");

		/// <summary>
		/// Tag: "crea", Name: "creature", Definition: "<see cref="TagTool.Tags.Definitions.Creature"/>".
		/// </summary>
		public static Tag CREA { get; } = new Tag("crea");

		/// <summary>
		/// Tag: "crte", Name: "cortana_effect_definition", Definition: "<see cref="TagTool.Tags.Definitions.CortanaEffectDefinition"/>".
		/// </summary>
		public static Tag CRTE { get; } = new Tag("crte");

		/// <summary>
		/// Tag: "ctrl", Name: "device_control", Definition: "<see cref="TagTool.Tags.Definitions.DeviceControl"/>".
		/// </summary>
		public static Tag CTRL { get; } = new Tag("ctrl");

		/// <summary>
		/// Tag: "dctr", Name: "decorator_set", Definition: "<see cref="TagTool.Tags.Definitions.DecoratorSet"/>".
		/// </summary>
		public static Tag DCTR { get; } = new Tag("dctr");

		/// <summary>
		/// Tag: "decs", Name: "decal_system", Definition: "<see cref="TagTool.Tags.Definitions.DecalSystem"/>".
		/// </summary>
		public static Tag DECS { get; } = new Tag("decs");

		/// <summary>
		/// Tag: "draw", Name: "rasterizer_cache_file_globals", Definition: "<see cref="TagTool.Tags.Definitions.RasterizerCacheFileGlobals"/>".
		/// </summary>
		public static Tag DRAW { get; } = new Tag("draw");

		/// <summary>
		/// Tag: "drdf", Name: "damage_response_definition", Definition: "<see cref="TagTool.Tags.Definitions.DamageResponseDefinition"/>".
		/// </summary>
		public static Tag DRDF { get; } = new Tag("drdf");

		/// <summary>
		/// Tag: "dsrc", Name: "gui_datasource_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiDatasourceDefinition"/>".
		/// </summary>
		public static Tag DSRC { get; } = new Tag("dsrc");

		/// <summary>
		/// Tag: "effe", Name: "effect", Definition: "<see cref="TagTool.Tags.Definitions.Effect"/>".
		/// </summary>
		public static Tag EFFE { get; } = new Tag("effe");

		/// <summary>
		/// Tag: "effg", Name: "effect_globals", Definition: "<see cref="TagTool.Tags.Definitions.EffectGlobals"/>".
		/// </summary>
		public static Tag EFFG { get; } = new Tag("effg");

		/// <summary>
		/// Tag: "efsc", Name: "effect_scenery", Definition: "<see cref="TagTool.Tags.Definitions.EffectScenery"/>".
		/// </summary>
		public static Tag EFSC { get; } = new Tag("efsc");

		/// <summary>
		/// Tag: "eqip", Name: "equipment", Definition: "<see cref="TagTool.Tags.Definitions.Equipment"/>".
		/// </summary>
		public static Tag EQIP { get; } = new Tag("eqip");

		/// <summary>
		/// Tag: "flck", Name: "flock", Definition: "<see cref="TagTool.Tags.Definitions.Flock"/>".
		/// </summary>
		public static Tag FLCK { get; } = new Tag("flck");

		/// <summary>
		/// Tag: "foot", Name: "material_effects", Definition: "<see cref="TagTool.Tags.Definitions.MaterialEffects"/>".
		/// </summary>
		public static Tag FOOT { get; } = new Tag("foot");

		/// <summary>
		/// Tag: "forg", Name: "forge_globals_definition", Definition: "<see cref="TagTool.Tags.Definitions.ForgeGlobalsDefinition"/>".
		/// </summary>
		public static Tag FORG { get; } = new Tag("forg");

		/// <summary>
		/// Tag: "form", Name: "formation", Definition: "<see cref="TagTool.Tags.Definitions.Formation"/>".
		/// </summary>
		public static Tag FORM { get; } = new Tag("form");

		/// <summary>
		/// Tag: "gfxt", Name: "gfx_textures_list", Definition: "<see cref="TagTool.Tags.Definitions.GfxTexturesList"/>".
		/// </summary>
		public static Tag GFXT { get; } = new Tag("gfxt");

		/// <summary>
		/// Tag: "gint", Name: "giant", Definition: "<see cref="TagTool.Tags.Definitions.Giant"/>".
		/// </summary>
		public static Tag GINT { get; } = new Tag("gint");

		/// <summary>
		/// Tag: "glps", Name: "global_pixel_shader", Definition: "<see cref="TagTool.Tags.Definitions.GlobalPixelShader"/>".
		/// </summary>
		public static Tag GLPS { get; } = new Tag("glps");

		/// <summary>
		/// Tag: "glvs", Name: "global_vertex_shader", Definition: "<see cref="TagTool.Tags.Definitions.GlobalVertexShader"/>".
		/// </summary>
		public static Tag GLVS { get; } = new Tag("glvs");

		/// <summary>
		/// Tag: "goof", Name: "multiplayer_variant_settings_interface_definition", Definition: "<see cref="TagTool.Tags.Definitions.MultiplayerVariantSettingsInterfaceDefinition"/>".
		/// </summary>
		public static Tag GOOF { get; } = new Tag("goof");

		/// <summary>
		/// Tag: "gpdt", Name: "game_progression", Definition: "<see cref="TagTool.Tags.Definitions.GameProgression"/>".
		/// </summary>
		public static Tag GPDT { get; } = new Tag("gpdt");

		/// <summary>
		/// Tag: "grup", Name: "gui_group_widget_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiGroupWidgetDefinition"/>".
		/// </summary>
		public static Tag GRUP { get; } = new Tag("grup");

		/// <summary>
		/// Tag: "hlmt", Name: "model", Definition: "<see cref="TagTool.Tags.Definitions.Model"/>".
		/// </summary>
		public static Tag HLMT { get; } = new Tag("hlmt");

		/// <summary>
		/// Tag: "inpg", Name: "input_globals", Definition: "<see cref="TagTool.Tags.Definitions.InputGlobals"/>".
		/// </summary>
		public static Tag INPG { get; } = new Tag("inpg");

		/// <summary>
		/// Tag: "jmad", Name: "model_animation_graph", Definition: "<see cref="TagTool.Tags.Definitions.ModelAnimationGraph"/>".
		/// </summary>
		public static Tag JMAD { get; } = new Tag("jmad");

		/// <summary>
		/// Tag: "jmrq", Name: "sandbox_text_value_pair_definition", Definition: "<see cref="TagTool.Tags.Definitions.SandboxTextValuePairDefinition"/>".
		/// </summary>
		public static Tag JMRQ { get; } = new Tag("jmrq");

		/// <summary>
		/// Tag: "jpt!", Name: "damage_effect", Definition: "<see cref="TagTool.Tags.Definitions.DamageEffect"/>".
		/// </summary>
		public static Tag JPT_ { get; } = new Tag("jpt!");

		/// <summary>
		/// Tag: "Lbsp", Name: "scenario_lightmap_bsp_data", Definition: "<see cref="TagTool.Tags.Definitions.ScenarioLightmapBspData"/>".
		/// </summary>
		public static Tag LBSP { get; } = new Tag("Lbsp");

		/// <summary>
		/// Tag: "lens", Name: "lens_flare", Definition: "<see cref="TagTool.Tags.Definitions.LensFlare"/>".
		/// </summary>
		public static Tag LENS { get; } = new Tag("lens");

		/// <summary>
		/// Tag: "ligh", Name: "light", Definition: "<see cref="TagTool.Tags.Definitions.Light"/>".
		/// </summary>
		public static Tag LIGH { get; } = new Tag("ligh");

		/// <summary>
		/// Tag: "lsnd", Name: "sound_looping", Definition: "<see cref="TagTool.Tags.Definitions.SoundLooping"/>".
		/// </summary>
		public static Tag LSND { get; } = new Tag("lsnd");

		/// <summary>
		/// Tag: "lst3", Name: "gui_list_widget_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiListWidgetDefinition"/>".
		/// </summary>
		public static Tag LST3 { get; } = new Tag("lst3");

		/// <summary>
		/// Tag: "lswd", Name: "leaf_system", Definition: "<see cref="TagTool.Tags.Definitions.LeafSystem"/>".
		/// </summary>
		public static Tag LSWD { get; } = new Tag("lswd");

		/// <summary>
		/// Tag: "ltvl", Name: "light_volume_system", Definition: "<see cref="TagTool.Tags.Definitions.LightVolumeSystem"/>".
		/// </summary>
		public static Tag LTVL { get; } = new Tag("ltvl");

		/// <summary>
		/// Tag: "mach", Name: "device_machine", Definition: "<see cref="TagTool.Tags.Definitions.DeviceMachine"/>".
		/// </summary>
		public static Tag MACH { get; } = new Tag("mach");

		/// <summary>
		/// Tag: "matg", Name: "globals", Definition: "<see cref="TagTool.Tags.Definitions.Globals"/>".
		/// </summary>
		public static Tag MATG { get; } = new Tag("matg");

		/// <summary>
		/// Tag: "mdl3", Name: "gui_model_widget_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiModelWidgetDefinition"/>".
		/// </summary>
		public static Tag MDL3 { get; } = new Tag("mdl3");

		/// <summary>
		/// Tag: "mdlg", Name: "ai_mission_dialogue", Definition: "<see cref="TagTool.Tags.Definitions.AiMissionDialogue"/>".
		/// </summary>
		public static Tag MDLG { get; } = new Tag("mdlg");

		/// <summary>
		/// Tag: "mffn", Name: "muffin", Definition: "<see cref="TagTool.Tags.Definitions.Muffin"/>".
		/// </summary>
		public static Tag MFFN { get; } = new Tag("mffn");

		/// <summary>
		/// Tag: "mode", Name: "render_model", Definition: "<see cref="TagTool.Tags.Definitions.RenderModel"/>".
		/// </summary>
		public static Tag MODE { get; } = new Tag("mode");

		/// <summary>
		/// Tag: "mulg", Name: "multiplayer_globals", Definition: "<see cref="TagTool.Tags.Definitions.MultiplayerGlobals"/>".
		/// </summary>
		public static Tag MULG { get; } = new Tag("mulg");

		/// <summary>
		/// Tag: "nclt", Name: "new_cinematic_lighting", Definition: "<see cref="TagTool.Tags.Definitions.NewCinematicLighting"/>".
		/// </summary>
		public static Tag NCLT { get; } = new Tag("nclt");

		/// <summary>
		/// Tag: "obje", Name: "object", Definition: "<see cref="TagTool.Tags.Definitions.GameObject"/>".
		/// </summary>
		public static Tag OBJE { get; } = new Tag("obje");

		/// <summary>
		/// Tag: "pecp", Name: "particle_emitter_custom_points", Definition: "<see cref="TagTool.Tags.Definitions.ParticleEmitterCustomPoints"/>".
		/// </summary>
		public static Tag PECP { get; } = new Tag("pecp");

		/// <summary>
		/// Tag: "pdm!", Name: "podium_settings", Definition: "<see cref="TagTool.Tags.Definitions.PodiumSettings"/>".
		/// </summary>
		public static Tag PDM_ { get; } = new Tag("pdm!");

		/// <summary>
		/// Tag: "perf", Name: "performance_throttles", Definition: "<see cref="TagTool.Tags.Definitions.PerformanceThrottles"/>".
		/// </summary>
		public static Tag PERF { get; } = new Tag("perf");

		/// <summary>
		/// Tag: "phmo", Name: "physics_model", Definition: "<see cref="TagTool.Tags.Definitions.PhysicsModel"/>".
		/// </summary>
		public static Tag PHMO { get; } = new Tag("phmo");

		/// <summary>
		/// Tag: "pixl", Name: "pixel_shader", Definition: "<see cref="TagTool.Tags.Definitions.PixelShader"/>".
		/// </summary>
		public static Tag PIXL { get; } = new Tag("pixl");

		/// <summary>
		/// Tag: "play", Name: "cache_file_resource_layout_table", Definition: "<see cref="TagTool.Tags.Definitions.CacheFileResourceLayoutTable"/>".
		/// </summary>
		public static Tag PLAY { get; } = new Tag("play");

		/// <summary>
		/// Tag: "pmdf", Name: "particle_model", Definition: "<see cref="TagTool.Tags.Definitions.ParticleModel"/>".
		/// </summary>
		public static Tag PMDF { get; } = new Tag("pmdf");

		/// <summary>
		/// Tag: "pmov", Name: "particle_physics", Definition: "<see cref="TagTool.Tags.Definitions.ParticlePhysics"/>".
		/// </summary>
		public static Tag PMOV { get; } = new Tag("pmov");

		/// <summary>
		/// Tag: "pphy", Name: "point_physics", Definition: "<see cref="TagTool.Tags.Definitions.PointPhysics"/>".
		/// </summary>
		public static Tag PPHY { get; } = new Tag("pphy");

		/// <summary>
		/// Tag: "proj", Name: "projectile", Definition: "<see cref="TagTool.Tags.Definitions.Projectile"/>".
		/// </summary>
		public static Tag PROJ { get; } = new Tag("proj");

		/// <summary>
		/// Tag: "prt3", Name: "particle", Definition: "<see cref="TagTool.Tags.Definitions.Particle"/>".
		/// </summary>
		public static Tag PRT3 { get; } = new Tag("prt3");

		/// <summary>
		/// Tag: "rasg", Name: "rasterizer_globals", Definition: "<see cref="TagTool.Tags.Definitions.RasterizerGlobals"/>".
		/// </summary>
		public static Tag RASG { get; } = new Tag("rasg");

		/// <summary>
		/// Tag: "rm  ", Name: "render_method", Definition: "<see cref="TagTool.Tags.Definitions.RenderMethod"/>".
		/// </summary>
		public static Tag RM__ { get; } = new Tag("rm  ");

		/// <summary>
		/// Tag: "rmbk", Name: "shader_black", Definition: "<see cref="TagTool.Tags.Definitions.ShaderBlack"/>".
		/// </summary>
		public static Tag RMBK { get; } = new Tag("rmbk");

		/// <summary>
		/// Tag: "rmcs", Name: "shader_custom", Definition: "<see cref="TagTool.Tags.Definitions.ShaderCustom"/>".
		/// </summary>
		public static Tag RMCS { get; } = new Tag("rmcs");

		/// <summary>
		/// Tag: "rmct", Name: "shader_cortana", Definition: "<see cref="TagTool.Tags.Definitions.ShaderCortana"/>".
		/// </summary>
		public static Tag RMCT { get; } = new Tag("rmct");

		/// <summary>
		/// Tag: "rmd ", Name: "shader_decal", Definition: "<see cref="TagTool.Tags.Definitions.ShaderDecal"/>".
		/// </summary>
		public static Tag RMD_ { get; } = new Tag("rmd ");

		/// <summary>
		/// Tag: "rmdf", Name: "render_method_definition", Definition: "<see cref="TagTool.Tags.Definitions.RenderMethodDefinition"/>".
		/// </summary>
		public static Tag RMDF { get; } = new Tag("rmdf");

		/// <summary>
		/// Tag: "rmfl", Name: "shader_foliage", Definition: "<see cref="TagTool.Tags.Definitions.ShaderFoliage"/>".
		/// </summary>
		public static Tag RMFL { get; } = new Tag("rmfl");

		/// <summary>
		/// Tag: "rmhg", Name: "shader_halogram", Definition: "<see cref="TagTool.Tags.Definitions.ShaderHalogram"/>".
		/// </summary>
		public static Tag RMHG { get; } = new Tag("rmhg");

		/// <summary>
		/// Tag: "rmop", Name: "render_method_option", Definition: "<see cref="TagTool.Tags.Definitions.RenderMethodOption"/>".
		/// </summary>
		public static Tag RMOP { get; } = new Tag("rmop");

		/// <summary>
		/// Tag: "rmsh", Name: "shader", Definition: "<see cref="TagTool.Tags.Definitions.Shader"/>".
		/// </summary>
		public static Tag RMSH { get; } = new Tag("rmsh");

		/// <summary>
		/// Tag: "rmss", Name: "shader_screen", Definition: "<see cref="TagTool.Tags.Definitions.ShaderScreen"/>".
		/// </summary>
		public static Tag RMSS { get; } = new Tag("rmss");

		/// <summary>
		/// Tag: "rmt2", Name: "render_method_template", Definition: "<see cref="TagTool.Tags.Definitions.RenderMethodTemplate"/>".
		/// </summary>
		public static Tag RMT2 { get; } = new Tag("rmt2");

		/// <summary>
		/// Tag: "rmtr", Name: "shader_terrain", Definition: "<see cref="TagTool.Tags.Definitions.ShaderTerrain"/>".
		/// </summary>
		public static Tag RMTR { get; } = new Tag("rmtr");

		/// <summary>
		/// Tag: "rmw ", Name: "shader_water", Definition: "<see cref="TagTool.Tags.Definitions.ShaderWater"/>".
		/// </summary>
		public static Tag RMW_ { get; } = new Tag("rmw ");

		/// <summary>
		/// Tag: "rmzo", Name: "shader_zonly", Definition: "<see cref="TagTool.Tags.Definitions.ShaderZonly"/>".
		/// </summary>
		public static Tag RMZO { get; } = new Tag("rmzo");

		/// <summary>
		/// Tag: "rwrd", Name: "render_water_ripple", Definition: "<see cref="TagTool.Tags.Definitions.RenderWaterRipple"/>".
		/// </summary>
		public static Tag RWRD { get; } = new Tag("rwrd");

		/// <summary>
		/// Tag: "sLdT", Name: "scenario_lightmap", Definition: "<see cref="TagTool.Tags.Definitions.ScenarioLightmap"/>".
		/// </summary>
		public static Tag SLDT { get; } = new Tag("sLdT");

		/// <summary>
		/// Tag: "sbsp", Name: "scenario_structure_bsp", Definition: "<see cref="TagTool.Tags.Definitions.ScenarioStructureBsp"/>".
		/// </summary>
		public static Tag SBSP { get; } = new Tag("sbsp");

		/// <summary>
		/// Tag: "scen", Name: "scenery", Definition: "<see cref="TagTool.Tags.Definitions.Scenery"/>".
		/// </summary>
		public static Tag SCEN { get; } = new Tag("scen");

		/// <summary>
		/// Tag: "scn3", Name: "gui_screen_widget_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiScreenWidgetDefinition"/>".
		/// </summary>
		public static Tag SCN3 { get; } = new Tag("scn3");

		/// <summary>
		/// Tag: "scnr", Name: "scenario", Definition: "<see cref="TagTool.Tags.Definitions.Scenario"/>".
		/// </summary>
		public static Tag SCNR { get; } = new Tag("scnr");

		/// <summary>
		/// Tag: "sddt", Name: "structure_design", Definition: "<see cref="TagTool.Tags.Definitions.StructureDesign"/>".
		/// </summary>
		public static Tag SDDT { get; } = new Tag("sddt");

		/// <summary>
		/// Tag: "sefc", Name: "area_screen_effect", Definition: "<see cref="TagTool.Tags.Definitions.AreaScreenEffect"/>".
		/// </summary>
		public static Tag SEFC { get; } = new Tag("sefc");

		/// <summary>
		/// Tag: "sfx+", Name: "sound_effect_collection", Definition: "<see cref="TagTool.Tags.Definitions.SoundEffectCollection"/>".
		/// </summary>
		public static Tag SFX_ { get; } = new Tag("sfx+");

		/// <summary>
		/// Tag: "sgp!", Name: "sound_global_propagation", Definition: "<see cref="TagTool.Tags.Definitions.SoundGlobalPropagation"/>".
		/// </summary>
		public static Tag SGP_ { get; } = new Tag("sgp!");

		/// <summary>
		/// Tag: "shit", Name: "shield_impact", Definition: "<see cref="TagTool.Tags.Definitions.ShieldImpact"/>".
		/// </summary>
		public static Tag SHIT { get; } = new Tag("shit");

		/// <summary>
		/// Tag: "siin", Name: "simulation_interpolation", Definition: "<see cref="TagTool.Tags.Definitions.SimulationInterpolation"/>".
		/// </summary>
		public static Tag SIIN { get; } = new Tag("siin");

		/// <summary>
		/// Tag: "sily", Name: "text_value_pair_definition", Definition: "<see cref="TagTool.Tags.Definitions.TextValuePairDefinition"/>".
		/// </summary>
		public static Tag SILY { get; } = new Tag("sily");

		/// <summary>
		/// Tag: "skn3", Name: "gui_skin_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiSkinDefinition"/>".
		/// </summary>
		public static Tag SKN3 { get; } = new Tag("skn3");

		/// <summary>
		/// Tag: "skya", Name: "sky_atm_parameters", Definition: "<see cref="TagTool.Tags.Definitions.SkyAtmParameters"/>".
		/// </summary>
		public static Tag SKYA { get; } = new Tag("skya");

		/// <summary>
		/// Tag: "smdt", Name: "survival_mode_globals", Definition: "<see cref="TagTool.Tags.Definitions.SurvivalModeGlobals"/>".
		/// </summary>
		public static Tag SMDT { get; } = new Tag("smdt");

		/// <summary>
		/// Tag: "sncl", Name: "sound_classes", Definition: "<see cref="TagTool.Tags.Definitions.SoundClasses"/>".
		/// </summary>
		public static Tag SNCL { get; } = new Tag("sncl");

		/// <summary>
		/// Tag: "snd!", Name: "sound", Definition: "<see cref="TagTool.Tags.Definitions.Sound"/>".
		/// </summary>
		public static Tag SND_ { get; } = new Tag("snd!");

		/// <summary>
		/// Tag: "snde", Name: "sound_environment", Definition: "<see cref="TagTool.Tags.Definitions.SoundEnvironment"/>".
		/// </summary>
		public static Tag SNDE { get; } = new Tag("snde");

		/// <summary>
		/// Tag: "snmx", Name: "sound_mix", Definition: "<see cref="TagTool.Tags.Definitions.SoundMix"/>".
		/// </summary>
		public static Tag SNMX { get; } = new Tag("snmx");

		/// <summary>
		/// Tag: "spk!", Name: "sound_dialogue_constants", Definition: "<see cref="TagTool.Tags.Definitions.SoundDialogueConstants"/>".
		/// </summary>
		public static Tag SPK_ { get; } = new Tag("spk!");

		/// <summary>
		/// Tag: "sqtm", Name: "squad_template", Definition: "<see cref="TagTool.Tags.Definitions.SquadTemplate"/>".
		/// </summary>
		public static Tag SQTM { get; } = new Tag("sqtm");

		/// <summary>
		/// Tag: "ssce", Name: "sound_scenery", Definition: "<see cref="TagTool.Tags.Definitions.SoundScenery"/>".
		/// </summary>
		public static Tag SSCE { get; } = new Tag("ssce");

		/// <summary>
		/// Tag: "styl", Name: "style", Definition: "<see cref="TagTool.Tags.Definitions.Style"/>".
		/// </summary>
		public static Tag STYL { get; } = new Tag("styl");

		/// <summary>
		/// Tag: "sus!", Name: "sound_ui_sounds", Definition: "<see cref="TagTool.Tags.Definitions.SoundUiSounds"/>".
		/// </summary>
		public static Tag SUS_ { get; } = new Tag("sus!");

		/// <summary>
		/// Tag: "term", Name: "device_terminal", Definition: "<see cref="TagTool.Tags.Definitions.Terminal"/>".
		/// </summary>
		public static Tag TERM { get; } = new Tag("term");

		/// <summary>
		/// Tag: "trak", Name: "camera_track", Definition: "<see cref="TagTool.Tags.Definitions.CameraTrack"/>".
		/// </summary>
		public static Tag TRAK { get; } = new Tag("trak");

		/// <summary>
		/// Tag: "trdf", Name: "texture_render_list", Definition: "<see cref="TagTool.Tags.Definitions.TextureRenderList"/>".
		/// </summary>
		public static Tag TRDF { get; } = new Tag("trdf");

		/// <summary>
		/// Tag: "txt3", Name: "gui_text_widget_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiTextWidgetDefinition"/>".
		/// </summary>
		public static Tag TXT3 { get; } = new Tag("txt3");

		/// <summary>
		/// Tag: "udlg", Name: "dialogue", Definition: "<see cref="TagTool.Tags.Definitions.Dialogue"/>".
		/// </summary>
		public static Tag UDLG { get; } = new Tag("udlg");

		/// <summary>
		/// Tag: "ugh!", Name: "sound_cache_file_gestalt", Definition: "<see cref="TagTool.Tags.Definitions.SoundCacheFileGestalt"/>".
		/// </summary>
		public static Tag UGH_ { get; } = new Tag("ugh!");

		/// <summary>
		/// Tag: "uise", Name: "user_interface_sounds_definition", Definition: "<see cref="TagTool.Tags.Definitions.UserInterfaceSoundsDefinition"/>".
		/// </summary>
		public static Tag UISE { get; } = new Tag("uise");

		/// <summary>
		/// Tag: "unic", Name: "multilingual_unicode_string_list", Definition: "<see cref="TagTool.Tags.Definitions.MultilingualUnicodeStringList"/>".
		/// </summary>
		public static Tag UNIC { get; } = new Tag("unic");

		/// <summary>
		/// Tag: "vehi", Name: "vehicle", Definition: "<see cref="TagTool.Tags.Definitions.Vehicle"/>".
		/// </summary>
		public static Tag VEHI { get; } = new Tag("vehi");

		/// <summary>
		/// Tag: "vfsl", Name: "vfiles_list", Definition: "<see cref="TagTool.Tags.Definitions.VFilesList"/>".
		/// </summary>
		public static Tag VFSL { get; } = new Tag("vfsl");

		/// <summary>
		/// Tag: "vmdx", Name: "vision_mode", Definition: "<see cref="TagTool.Tags.Definitions.VisionMode"/>".
		/// </summary>
		public static Tag VMDX { get; } = new Tag("vmdx");

		/// <summary>
		/// Tag: "vtsh", Name: "vertex_shader", Definition: "<see cref="TagTool.Tags.Definitions.VertexShader"/>".
		/// </summary>
		public static Tag VTSH { get; } = new Tag("vtsh");

		/// <summary>
		/// Tag: "wacd", Name: "gui_widget_animation_collection_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiWidgetAnimationCollectionDefinition"/>".
		/// </summary>
		public static Tag WACD { get; } = new Tag("wacd");

		/// <summary>
		/// Tag: "wclr", Name: "gui_widget_color_animation_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiWidgetColorAnimationDefinition"/>".
		/// </summary>
		public static Tag WCLR { get; } = new Tag("wclr");

		/// <summary>
		/// Tag: "weap", Name: "weapon", Definition: "<see cref="TagTool.Tags.Definitions.Weapon"/>".
		/// </summary>
		public static Tag WEAP { get; } = new Tag("weap");

		/// <summary>
		/// Tag: "wezr", Name: "game_engine_settings_definition", Definition: "<see cref="TagTool.Tags.Definitions.GameEngineSettingsDefinition"/>".
		/// </summary>
		public static Tag WEZR { get; } = new Tag("wezr");

		/// <summary>
		/// Tag: "wgan", Name: "gui_widget_animation_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiWidgetAnimationDefinition"/>".
		/// </summary>
		public static Tag WGAN { get; } = new Tag("wgan");

		/// <summary>
		/// Tag: "wgtz", Name: "user_interface_globals_definition", Definition: "<see cref="TagTool.Tags.Definitions.UserInterfaceGlobalsDefinition"/>".
		/// </summary>
		public static Tag WGTZ { get; } = new Tag("wgtz");

		/// <summary>
		/// Tag: "wigl", Name: "user_interface_shared_globals_definition", Definition: "<see cref="TagTool.Tags.Definitions.UserInterfaceSharedGlobalsDefinition"/>".
		/// </summary>
		public static Tag WIGL { get; } = new Tag("wigl");

		/// <summary>
		/// Tag: "wind", Name: "wind", Definition: "<see cref="TagTool.Tags.Definitions.Wind"/>".
		/// </summary>
		public static Tag WIND { get; } = new Tag("wind");

		/// <summary>
		/// Tag: "wpos", Name: "gui_widget_position_animation_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiWidgetPositionAnimationDefinition"/>".
		/// </summary>
		public static Tag WPOS { get; } = new Tag("wpos");

		/// <summary>
		/// Tag: "wrot", Name: "gui_widget_rotation_animation_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiWidgetRotationAnimationDefinition"/>".
		/// </summary>
		public static Tag WROT { get; } = new Tag("wrot");

		/// <summary>
		/// Tag: "wscl", Name: "gui_widget_scale_animation_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiWidgetScaleAnimationDefinition"/>".
		/// </summary>
		public static Tag WSCL { get; } = new Tag("wscl");

		/// <summary>
		/// Tag: "wspr", Name: "gui_widget_sprite_animation_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiWidgetSpriteAnimationDefinition"/>".
		/// </summary>
		public static Tag WSPR { get; } = new Tag("wspr");

		/// <summary>
		/// Tag: "wtuv", Name: "gui_widget_texture_coordinate_animation_definition", Definition: "<see cref="TagTool.Tags.Definitions.GuiWidgetTextureCoordinateAnimationDefinition"/>".
		/// </summary>
		public static Tag WTUV { get; } = new Tag("wtuv");

		/// <summary>
		/// Tag: "zone", Name: "cache_file_resource_gestalt", Definition: "<see cref="TagTool.Tags.Definitions.CacheFileResourceGestalt"/>".
		/// </summary>
		public static Tag ZONE { get; } = new Tag("zone");
		#endregion

		/// <summary>
		/// Constructs a magic number from an integer.
		/// </summary>
		/// <param name="val">The integer.</param>
		public Tag(int val)
        {
            Value = val;
        }

        /// <summary>
        /// Constructs a magic number from a string.
        /// </summary>
        /// <param name="str">The string.</param>
        public Tag(string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            Value = 0;
            foreach (var b in bytes)
            {
                Value <<= 8;
                Value |= b;
            }
        }

        /// <summary>
        /// Constructs a magic number from a character array.
        /// </summary>
        /// <param name="input">The character array.</param>
        public Tag(char[] input)
        {
            var chars = new char[4] { ' ', ' ', ' ', ' ' };

            for (var i = 0; i < input.Length; i++)
                chars[i] = input[i];

            Value = 0;
            foreach (var c in chars)
            {
                Value <<= 8;
                Value |= c;
            }
        }

        /// <summary>
        /// Gets the value of the magic number as an integer. 
		/// THERE BE DRAGONS HERE: Do not set this manually outside of serialization.
        /// </summary>
        public int Value { get; set; }

		/// <summary>
		/// Converts the magic number into its string representation.
		/// </summary>
		/// <returns>The string that the magic number looks like.</returns>
		public override string ToString()
        {
            var i = 4;
            var chars = new char[4];
            var val = Value;
            while (val > 0)
            {
                i--;
                chars[i] = (char)(val & 0xFF);
                val >>= 8;
            }
            return (i < 4) ? new string(chars, i, chars.Length - i) : "";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Tag))
                return false;
            var other = (Tag)obj;
            return (Value == other.Value);
        }

        public static bool operator ==(Tag a, Tag b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Tag a, Tag b)
        {
            return !(a == b);
        }

        public static bool operator ==(Tag a, string b)
        {
            return a == new Tag(b);
        }

        public static bool operator !=(Tag a, string b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public int CompareTo(Tag other)
        {
            return Value - other.Value;
        }

	}
}
