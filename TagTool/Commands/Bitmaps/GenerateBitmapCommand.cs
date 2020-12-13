using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TagTool.Bitmaps;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags;
using TagTool.Cache.HaloOnline;
using TagTool.Bitmaps.DDS;
using TagTool.IO;

namespace TagTool.Commands.Tags
{
	class GenerateBitmapCommand : Command
	{
		public GameCacheHaloOnlineBase Cache { get; }
		public object TagDefinition { get; private set; }

		public GenerateBitmapCommand(GameCacheHaloOnlineBase cache)
			: base(true,

				  "GenerateBitmap",
				  "Creates a new bitm tag with a specified name from a DDS image.",

				  "GenerateBitmap <desired name> <path to dds>",

				  "Creates a new bitm tag with a specified name from a DDS image.")
		{
			Cache = cache;
		}

		public override object Execute(List<string> args)
		{
			if (args.Count != 2)
				return new TagToolError(CommandError.ArgCount);

			var tagname = args[0];
			var imagePath = args[1];

			if (!File.Exists(imagePath))
				return new TagToolError(CommandError.FileNotFound, $"\"{imagePath}\"");

			CachedTag instance = null;
			var tagGroup = Cache.TagCache.TagDefinitions.GetTagGroupFromTag(new Tag("bitm"));

			var groupTag = new Tag("bitm");

			using (var stream = Cache.OpenCacheReadWrite())
			{
				if (instance == null)
					instance = Cache.TagCache.AllocateTag(Cache.TagCache.TagDefinitions.GetTagGroupFromTag(groupTag), args[0]);

				Cache.Serialize(stream, instance, Activator.CreateInstance(Cache.TagCache.TagDefinitions.GetTagDefinitionType(groupTag)));

				Cache.SaveTagNames();
			}

			using (var stream = Cache.OpenCacheReadWrite())
			{
				// importing
				var bitmap = Cache.Deserialize<Bitmap>(stream, instance);

				bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;
				bitmap.Images.Add(new Bitmap.Image { Signature = new Tag("bitm") });
				bitmap.Resources.Add(new TagResourceReference());

				var imageIndex = 0;
				BitmapImageCurve curve = BitmapImageCurve.xRGB;

#if !DEBUG
				try
				{
#endif
					DDSFile file = new DDSFile();

					using (var imageStream = File.OpenRead(imagePath))
					using (var reader = new EndianReader(imageStream))
					{
						file.Read(reader);
					}

					var bitmapTextureInteropDefinition = BitmapInjector.CreateBitmapResourceFromDDS(Cache, file, curve);
					var reference = Cache.ResourceCache.CreateBitmapResource(bitmapTextureInteropDefinition);

					// set the tag data

					bitmap.Resources[imageIndex] = reference;
					bitmap.Images[imageIndex] = BitmapUtils.CreateBitmapImageFromResourceDefinition(bitmapTextureInteropDefinition.Texture.Definition.Bitmap);

					Cache.Serialize(stream, instance, bitmap);
#if !DEBUG
				}
				catch (Exception ex)
				{
				    return new TagToolError(CommandError.OperationFailed, "Importing image data failed: " + ex.Message);
				}
#endif
					Console.WriteLine("Image imported successfully.");
			}

			var tagName = instance.Name ?? $"{tagname}";

			Console.WriteLine($"[Index: 0x{instance.Index:X4}] {tagname}.{instance.Group}");

			return true;
		}
	}
}

