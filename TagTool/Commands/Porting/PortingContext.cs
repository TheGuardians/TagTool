//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TagTool.Cache;
//using TagTool.Common;
//using TagTool.Serialization;

//namespace TagTool.Commands.Porting
//{
//	public static class PortingConstants
//	{
//		private static readonly Tag[] RenderMethodTagGroups = new[]{ new Tag("rmbk"), new Tag("rmcs"), new Tag("rmd "), new Tag("rmfl"), new Tag("rmhg"), new Tag("rmsh"), new Tag("rmss"), new Tag("rmtr"), new Tag("rmw "), new Tag("rmrd"), new Tag("rmct") };
//		private static readonly Tag[] EffectTagGroups = new[] { new Tag("beam"), new Tag("cntl"), new Tag("ltvl"), new Tag("decs"), new Tag("prt3") };
//		private static readonly string[] DoNotReplaceGroups = new[]
//		{
//			"glps",
//			"glvs",
//			"vtsh",
//			"pixl",
//			"rmdf",
//			"rmt2"
//		};
//	}

//	public class PortingContext
//	{
//		private readonly HaloOnlineCacheContext CacheContext;
//		private readonly CacheFile BlamCache;
//		private readonly Dictionary<Tag, List<string>> ReplacedTags = new Dictionary<Tag, List<string>>();

//		private readonly Stream CacheStream;
//		private readonly Dictionary<ResourceLocation, Stream> ResourceStreams;

//		public CachedTagInstance GetTag<T>(string name) where T : TagStructure
//		{
//			throw new NotImplementedException();
//		}
//	}
//}
