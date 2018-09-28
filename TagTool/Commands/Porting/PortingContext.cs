using System.Collections.Generic;
using TagTool.Common;

namespace TagTool.Commands.Porting
{
    public static class PortingConstants
	{
		public static readonly IReadOnlyList<Tag> RenderMethodGroups = new[] 
		{
			Tag.RMBK,
			Tag.RMCS,
			Tag.RMCT,
			Tag.RMD_,
			Tag.RMFL,
			Tag.RMHG,
			Tag.RWRD,
			Tag.RMSH,
			Tag.RMSS,
			Tag.RMTR,
			Tag.RMW_,
		};

		public static readonly IReadOnlyList<Tag> EffectGroups = new[] 
		{
			Tag.BEAM,
			Tag.CNTL,
			Tag.DECS,
			Tag.LTVL,
			Tag.PRT3,
		};

		public static readonly IReadOnlyList<Tag> DoNotReplaceGroups = new[]
		{
			Tag.GLPS,
			Tag.GLVS,
			Tag.PIXL,
			Tag.RMDF,
			Tag.RMT2,
			Tag.VTSH,
		};

		public static readonly IReadOnlyList<Tag> UnreadyGroups = new[]
		{
			Tag.GLPS,
			Tag.GLVS,
			Tag.RMCS,
			Tag.RMCT,
			Tag.RMDF,
			Tag.RMHG,
			Tag.RMT2,
			Tag.RMW_,
			Tag.SHIT,
			Tag.SNCL,
		};

		public static readonly IReadOnlyDictionary<Tag, string> DefaultTagNames = new Dictionary<Tag, string>
		{
			{ Tag.BEAM, @"objects\weapons\support_high\spartan_laser\fx\firing_3p" },
			{ Tag.BITM, @"shaders\default_bitmaps\bitmaps\gray_50_percent_linear" },
			{ Tag.CNTL, @"objects\weapons\pistol\needler\fx\projectile" },
			{ Tag.DECS, @"fx\decals\impact_plasma\impact_plasma_medium\hard" },
			{ Tag.GLPS, @"shaders\shader_shared_pixel_shaders" },
			{ Tag.GLVS, @"shaders\shader_shared_vertex_shaders" },
			{ Tag.LTVL, @"objects\weapons\pistol\plasma_pistol\fx\charged\projectile" },
			{ Tag.PRT3, @"fx\particles\energy\sparks\impact_spark_orange" },
			{ Tag.RMBK, @"shaders\invalid" },
			{ Tag.RMCS, @"shaders\invalid" },
			{ Tag.RMCT, @"shaders\invalid" },
			{ Tag.RMD_, @"objects\gear\human\military\shaders\human_military_decals" },
			{ Tag.RMDF, @"shaders\shader" },
			{ Tag.RMFL, @"levels\multi\riverworld\shaders\riverworld_tree_leafa" },
			{ Tag.RMHG, @"objects\ui\shaders\editor_gizmo" },
			{ Tag.RWRD, @"shaders\invalid" },
			{ Tag.RMSH, @"sound\sound_classes" },
			{ Tag.RMSS, @"shaders\invalid" },
			{ Tag.RMTR, @"levels\multi\riverworld\shaders\riverworld_ground" },
			{ Tag.RMW_, @"levels\multi\riverworld\shaders\riverworld_water_rough" },
			{ Tag.SHIT, @"globals\global_shield_impact_settings" },
			{ Tag.SNCL, @"sound\sound_classes" },
			{ Tag.SND_, @"sound\materials\tough\rubber\rubber_small" },
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
