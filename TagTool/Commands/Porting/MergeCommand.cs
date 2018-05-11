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
			var blamContext = new CacheSerializationContext(null, null);
			var hoContext = new TagSerializationContext(null, null, null);

			// find blam globals.
			var blam_adlg_tag = BlamCache.IndexItems.Find(item => item.ClassCode == "adlg");
			var blam_matg_tag = BlamCache.IndexItems.Find(item => item.ClassCode == "matg");
			var blam_mulg_tag = BlamCache.IndexItems.Find(item => item.ClassCode == "mulg");

			// find halo-online globals.
			var ho_adlg_tag = CacheContext.TagCache.Index.FindFirstInGroup(new Tag("adlg"));
			var ho_matg_tag = CacheContext.TagCache.Index.FindFirstInGroup(new Tag("matg"));
			var ho_mulg_tag = CacheContext.TagCache.Index.FindFirstInGroup(new Tag("mulg"));

			using (var tagsStream = CacheContext.OpenTagCacheReadWrite())
			{
				// deserialize blam globals.
				blamContext = new CacheSerializationContext(BlamCache, blam_adlg_tag);
				var blam_adlg = BlamCache.Deserializer.Deserialize<AiDialogueGlobals>(blamContext);
				blamContext = new CacheSerializationContext(BlamCache, blam_matg_tag);
				var blam_matg = BlamCache.Deserializer.Deserialize<Globals>(blamContext);
				blamContext = new CacheSerializationContext(BlamCache, blam_mulg_tag);
				var blam_mulg = BlamCache.Deserializer.Deserialize<MultiplayerGlobals>(blamContext);

				// deserialize halo-online globals.
				hoContext = new TagSerializationContext(tagsStream, CacheContext, ho_adlg_tag);
				var ho_adlg = CacheContext.Deserializer.Deserialize<AiDialogueGlobals>(hoContext);
				hoContext = new TagSerializationContext(tagsStream, CacheContext, ho_matg_tag);
				var ho_matg = CacheContext.Deserializer.Deserialize<Globals>(hoContext);
				hoContext = new TagSerializationContext(tagsStream, CacheContext, ho_mulg_tag);
				var ho_mulg = CacheContext.Deserializer.Deserialize<MultiplayerGlobals>(hoContext);

				// merge blam globals into halo-online globals
				ho_adlg = MergeAiDialogueGlobals(ho_adlg, blam_adlg);
				ho_matg = MergeGlobals(ho_matg, blam_matg);
				ho_mulg = MergeMultiplayerGlobals(ho_mulg, blam_mulg);

				// serialize halo-online globals
				hoContext = new TagSerializationContext(tagsStream, CacheContext, ho_adlg_tag);
				CacheContext.Serialize(hoContext, ho_adlg);
				hoContext = new TagSerializationContext(tagsStream, CacheContext, ho_matg_tag);
				CacheContext.Serialize(hoContext, ho_matg);
				hoContext = new TagSerializationContext(tagsStream, CacheContext, ho_mulg_tag);
				CacheContext.Serialize(hoContext, ho_mulg);
			}

			return true;
		}
	}
}