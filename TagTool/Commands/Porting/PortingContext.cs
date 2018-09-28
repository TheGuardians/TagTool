using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Commands.Porting
{
    public static class PortingConstants
	{
		public static readonly IReadOnlyList<Tag> RenderMethodGroups = new[] 
		{
			new Tag("rmbk"),
			new Tag("rmcs"),
			new Tag("rmct"),
			new Tag("rmd "),
			new Tag("rmfl"),
			new Tag("rmhg"),
			new Tag("rmrd"),
			new Tag("rmsh"),
			new Tag("rmss"),
			new Tag("rmtr"),
			new Tag("rmw "),
		};

		public static readonly IReadOnlyList<Tag> EffectGroups = new[] 
		{
			new Tag("beam"),
			new Tag("cntl"),
			new Tag("decs"),
			new Tag("ltvl"),
			new Tag("prt3"),
		};

		public static readonly IReadOnlyList<Tag> DoNotReplaceGroups = new[]
		{
			new Tag("glps"),
			new Tag("glvs"),
			new Tag("pixl"),
			new Tag("rmdf"),
			new Tag("rmt2"),
			new Tag("vtsh"),
		};

		public static readonly IReadOnlyList<Tag> UnreadyGroups = new[]
		{
			new Tag("glps"),
			new Tag("glvs"),
			new Tag("rmcs"),
			new Tag("rmcs"),
			new Tag("rmct"),
			new Tag("rmdf"),
			new Tag("rmhg"),
			new Tag("rmt2"),
			new Tag("rmw "),
			new Tag("shit"),
			new Tag("sncl"),
		};

		public static readonly IReadOnlyDictionary<Tag, string> DefaultTagNames = new Dictionary<Tag, string>
		{
			{ new Tag("beam"), @"objects\weapons\support_high\spartan_laser\fx\firing_3p" },
			{ new Tag("bitm"), @"shaders\default_bitmaps\bitmaps\gray_50_percent_linear" },
			{ new Tag("cntl"), @"objects\weapons\pistol\needler\fx\projectile" },
			{ new Tag("decs"), @"fx\decals\impact_plasma\impact_plasma_medium\hard" },
			{ new Tag("glps"), @"shaders\shader_shared_pixel_shaders" },
			{ new Tag("glvs"), @"shaders\shader_shared_vertex_shaders" },
			{ new Tag("ltvl"), @"objects\weapons\pistol\plasma_pistol\fx\charged\projectile" },
			{ new Tag("prt3"), @"fx\particles\energy\sparks\impact_spark_orange" },
			{ new Tag("rmbk"), @"shaders\invalid" },
			{ new Tag("rmcs"), @"shaders\invalid" },
			{ new Tag("rmct"), @"shaders\invalid" },
			{ new Tag("rmd "), @"objects\gear\human\military\shaders\human_military_decals" },
			{ new Tag("rmdf"), @"shaders\shader" },
			{ new Tag("rmfl"), @"levels\multi\riverworld\shaders\riverworld_tree_leafa" },
			{ new Tag("rmhg"), @"objects\ui\shaders\editor_gizmo" },
			{ new Tag("rmrd"), @"shaders\invalid" },
			{ new Tag("rmsh"), @"sound\sound_classes" },
			{ new Tag("rmss"), @"shaders\invalid" },
			{ new Tag("rmtr"), @"levels\multi\riverworld\shaders\riverworld_ground" },
			{ new Tag("rmw "), @"levels\multi\riverworld\shaders\riverworld_water_rough" },
			{ new Tag("shit"), @"globals\global_shield_impact_settings" },
			{ new Tag("sncl"), @"sound\sound_classes" },
			{ new Tag("snd!"), @"sound\materials\tough\rubber\rubber_small" },
		};
	}

	//public class PortingContext
	//{
	//	private readonly HaloOnlineCacheContext CacheContext;
	//	private readonly Stream CacheStream;
	//	private readonly Dictionary<ResourceLocation, Stream> ResourceStreams;

	//	private readonly CacheFile BlamCache;

	//	private readonly Dictionary<Tag, List<string>> ReplacedTags;

	//	public CachedTagInstance GetTag<T>(string name) where T : TagStructure
	//	{
	//		throw new NotImplementedException();
	//	}

	//	public PortingContext(HaloOnlineCacheContext cacheContext, CacheFile blamCache)
	//	{
	//		CacheContext = cacheContext;
	//		CacheStream = null;
	//		ResourceStreams = new Dictionary<ResourceLocation, Stream> { };
	//		BlamCache = blamCache;
	//		ReplacedTags = new Dictionary<Tag, List<string>> { };
	//	}
	//}

	//public static class PortingHelpers
	//{
	//	public static bool ShouldPortGroup(PortingContext context, Tag groupTag)
	//	{
	//		if (PortingConstants.UnreadyGroups.Contains(groupTag))
	//			throw new NotImplementedException(); // return false;

	//		if (/*!FlagsAnySet(PortingFlags.ShaderTest | PortingFlags.MatchShaders) &&*/
	//			(PortingConstants.RenderMethodGroups.Contains(groupTag) ||
	//			PortingConstants.EffectGroups.Contains(groupTag)))
	//			throw new NotImplementedException(); // return false;

	//		if (/*!FlagIsSet(PortingFlags.Audio) &&*/ groupTag == "snd!")
	//			throw new NotImplementedException(); // return false;

	//		throw new NotImplementedException(); // return true;
	//	}

	//	public static CachedTagInstance GetDefaultTag(Tag groupTag)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}
}
