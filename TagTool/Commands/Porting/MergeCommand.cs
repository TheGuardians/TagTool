using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Serialization;
using TagTool.ShaderDecompiler;
using TagTool.Tags.Definitions;
using TagTool.Direct3D.Functions;
using TagTool.Direct3D.Enums;
using TagTool.Common;
using System.Linq;

namespace TagTool.Commands.Porting
{
	public partial class MergeCommand : Command
	{
		private CacheFile BlamCache { get; }
		private GameCacheContext CacheContext { get; }

		public MergeCommand(GameCacheContext cacheContext, CacheFile blamCache) : base(
			CommandFlags.None,

			"MergeGlobalTags",
			"Merges global BLAM! tags into existing HO global tags.",

			"MergeGlobalTags",

			"")
		{
			CacheContext = cacheContext;
			BlamCache = blamCache;
		}

		public override object Execute(List<string> args)
		{
			// initialize serialization contexts.
			var tagsContext = new TagSerializationContext(null, null, null);

			// find global tags
			var matg_tags = CacheContext.TagCache.Index.FindAllInGroup(new Tag("matg")).ToList();
			var mulg_tags = CacheContext.TagCache.Index.FindAllInGroup(new Tag("mulg")).ToList();

			using (var tagsStream = CacheContext.OpenTagCacheReadWrite())
			{
				var matgs = new List<Globals> { };
				var mulgs = new List<MultiplayerGlobals> { };

				// deserialize halo-online globals.
				foreach (var matg_tag in matg_tags)
				{
					tagsContext = new TagSerializationContext(tagsStream, CacheContext, matg_tag);
					matgs.Add(CacheContext.Deserializer.Deserialize<Globals>(tagsContext));
				}
				foreach (var mulg_tag in mulg_tags)
				{
					tagsContext = new TagSerializationContext(tagsStream, CacheContext, mulg_tag);
					mulgs.Add(CacheContext.Deserializer.Deserialize<MultiplayerGlobals>(tagsContext));
				}

				// merge global tags into the first global tag
				var matg = MergeGlobals(matgs);
				var mulg = MergeMultiplayerGlobals(mulgs);

				// serialize global tags
				tagsContext = new TagSerializationContext(tagsStream, CacheContext, matg_tags[0]);
				CacheContext.Serialize(tagsContext, matg);
				tagsContext = new TagSerializationContext(tagsStream, CacheContext, mulg_tags[0]);
				CacheContext.Serialize(tagsContext, mulg);
			}

			return true;
		}
	}
}