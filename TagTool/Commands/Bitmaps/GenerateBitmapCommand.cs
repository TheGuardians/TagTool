using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Bitmaps;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags;
using TagTool.Bitmaps.DDS;
using TagTool.IO;
using System.Linq;

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

				  "GenerateBitmap [flags] <name or prefix> <path>",

				  "Creates bitm tag(s) with a specified name from DDS image(s) at the provided path."
                  + "\nIf <path> is a folder, a tag for each DDS within is created as <prefix>\\filename."
                  + "\nAny flags should be separated with commas (e.g. a,b,c)."

                  + "\n\nAvailable flags:"
                  + "\n\tsequence : imports all DDS with an integer prefix to the same tag as distinct images"
                  + "\n\talphaseq : same as above but added alphabetically"
                  + "\n\tlinear : sets the bitmap curve mode as Linear")
		{
			Cache = cache;
		}

		public override object Execute(List<string> args)
		{
			if (args.Count > 3 || args.Count < 2)
				return new TagToolError(CommandError.ArgCount);

            string imagePath = args[args.Count - 1];
            string tagname = args[args.Count - 2];

            bool batchImport = false;

            Tag groupTag = new Tag("bitm");
            TagGroup tagGroup = Cache.TagCache.TagDefinitions.GetTagGroupFromTag(groupTag);
            BitmapImageCurve curve = BitmapImageCurve.xRGB;

            // flags
            bool sequence = false;
            bool alphaseq = false;

            if (args.Count == 3)
            {
                var flags = args[0].ToLower().Split(',');

                foreach (string flag in flags)
                {
                    switch(flag)
                    {
                        case "sequence":
                            sequence = true;
                            break;
                        case "alphaseq":
                            alphaseq = sequence = true;
                            break;
                        case "linear":
                            curve = BitmapImageCurve.Linear;
                            break;
                    }
                }
            }

            List<FileInfo> fileList = new List<FileInfo>();

            if (Directory.Exists(imagePath))
            {
                batchImport = true;
                var files = Directory.GetFiles(imagePath, "*.dds");

                if (files == null || files.Count() == 0 )
                    return new TagToolError(CommandError.CustomError, $"Directory \"{imagePath}\" contains no DDS files.");

                foreach (var file in files)
                {
                    fileList.Add(new FileInfo(file));
                }
            }
            else if (File.Exists(imagePath))
            {
                fileList.Add(new FileInfo(imagePath));
            }
            else
                return new TagToolError(CommandError.FileNotFound, $"\"{imagePath}\"");

            // image sequence import (e.g. numbers_plate)
            if (sequence)
            {
                CachedTag instance = null;
                instance = AllocateBitmTag(tagname, instance, groupTag);

                using (var stream = Cache.OpenCacheReadWrite())
                {
                    var imageIndex = 0;
                    var bitmap = Cache.Deserialize<Bitmap>(stream, instance);
                    bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;

                    if (!alphaseq)
                    {
                        while (true)
                        {
                            var currentFile = fileList.FirstOrDefault(f => f.Name.StartsWith(imageIndex.ToString()));
                            if (currentFile == null)
                            {
                                if (imageIndex == 0)
                                    return new TagToolError(CommandError.CustomError, "Sequence element 0 not found.");
                                else
                                    break;
                            }

                            PrepareTagAndImport(groupTag, currentFile, imageIndex, bitmap, curve);
                            AddSequenceAndSprite(imageIndex, bitmap, currentFile.Name);

                            imageIndex++;
                        }
                    }
                    // importing a set of images as a sequence, but you don't care what order.
                    else
                    {
                        foreach (FileInfo file in fileList)
                        {
                            PrepareTagAndImport(groupTag, file, imageIndex, bitmap, curve);
                            AddSequenceAndSprite(imageIndex, bitmap, file.Name);

                            imageIndex++;
                        }
                    }

                    Cache.Serialize(stream, instance, bitmap);
                }

                PrintSuccess(tagname, instance);
            }
            // standard or batch import
            else
            {
                foreach (FileInfo file in fileList)
                {
                    CachedTag instance = null;

                    if (batchImport)
                    {
                        tagname = args[args.Count - 2];

                        if (!tagname.EndsWith("\\"))
                            tagname += "\\";

                        tagname += file.Name.Split('.')[0];
                    }

                    instance = AllocateBitmTag(tagname, instance, groupTag);

                    using (var stream = Cache.OpenCacheReadWrite())
                    {
                        var bitmap = Cache.Deserialize<Bitmap>(stream, instance);
                        bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;
                        PrepareTagAndImport(groupTag, file, 0, bitmap, curve);
                        Cache.Serialize(stream, instance, bitmap);
                    }

                    PrintSuccess(tagname, instance);
                }
            }

            return true;
		}



        private static void AddSequenceAndSprite(int imageIndex, Bitmap bitmap, string fileName)
        {
            bitmap.Sequences.Add(new Bitmap.Sequence
            {
                Name = fileName.Split('.')[0],
                BitmapCount = 1,
                Sprites = new List<Bitmap.Sequence.Sprite>
                {
                    new Bitmap.Sequence.Sprite
                    {
                        BitmapIndex = (short)imageIndex,
                        Right = 1,
                        Bottom = 1,
                        RegistrationPoint = new RealPoint2d(0.5f, 0.5f)
                    }
                }
            });
        }

        private void PrepareTagAndImport(Tag groupTag, FileInfo file, int imageIndex, Bitmap bitmap, BitmapImageCurve curve)
        {
            bitmap.Images.Add(new Bitmap.Image { Signature = groupTag });
            bitmap.HardwareTextures.Add(new TagResourceReference());

#if !DEBUG
			try
			{
#endif
            DDSFile dds = new DDSFile();

            using (var imageStream = File.OpenRead(file.FullName))
            using (var reader = new EndianReader(imageStream))
            {
                dds.Read(reader);
            }

            var bitmapTextureInteropDefinition = BitmapInjector.CreateBitmapResourceFromDDS(Cache, dds, curve);
            var reference = Cache.ResourceCache.CreateBitmapResource(bitmapTextureInteropDefinition);

            // set the tag data

            bitmap.HardwareTextures[imageIndex] = reference;
            bitmap.Images[imageIndex] = BitmapUtils.CreateBitmapImageFromResourceDefinition(bitmapTextureInteropDefinition.Texture.Definition.Bitmap);

#if !DEBUG
			}
			catch (Exception ex)
			{
			    return new TagToolError(CommandError.OperationFailed, "Importing image data failed: " + ex.Message);
			}
#endif
        }

        private CachedTag AllocateBitmTag(string tagname, CachedTag instance, Tag groupTag)
        {
            using (var stream = Cache.OpenCacheReadWrite())
            {
                if (instance == null)
                    instance = Cache.TagCache.AllocateTag(Cache.TagCache.TagDefinitions.GetTagGroupFromTag(groupTag), tagname);

                Cache.Serialize(stream, instance, Activator.CreateInstance(Cache.TagCache.TagDefinitions.GetTagDefinitionType(groupTag)));

                Cache.SaveTagNames();
            }

            return instance;
        }

        private static void PrintSuccess(string tagname, CachedTag instance)
        {
            Console.WriteLine("Image imported successfully.");
            var tagName = instance.Name ?? $"{tagname}";
            Console.WriteLine($"[Index: 0x{instance.Index:X4}] {tagname}.{instance.Group}");
        }
    }
}

