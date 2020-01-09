using TagTool.Cache;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Bitmaps;
using TagTool.Tags.Resources;
using TagTool.Bitmaps.Utils;
using TagTool.Bitmaps.DDS;

namespace TagTool.Commands
{
    
    public class TestCommand : Command
    {
        GameCache Cache;

        public TestCommand(GameCache cache) : base(false, "Test", "Test", "Test", "Test")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;

            //
            // Insert what test command you want below
            //

            
            using(var stream = Cache.TagCache.OpenTagCacheRead())
            {
                foreach (var tag in Cache.TagCache.TagTable)
                {
                    if (tag.Group.Tag == "bitm")
                    {
                        var bitmap = Cache.Deserialize<Bitmap>(stream, tag);


                        if (!bitmap.Images[0].XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
                            continue;

                        for (int i = 0; i < bitmap.Images.Count; i++)
                        {
                            var baseBitmap = BitmapConverterNew.ConvertGen3Bitmap(Cache, bitmap, i);
                            if(baseBitmap == null)
                            {
                                Console.WriteLine($"Invalid data for {tag.Name}.bitm");
                                continue;
                            }
                            var ddsFile = new DDSFile(baseBitmap);
                            var tagName = tag.Name.Replace('\\', '_');
                            var fileName = $"Bitmaps\\{tagName}_{i}.dds";
                            var file = new FileInfo(fileName);

                            using (var fileStream = file.Create())
                            using (var writer = new EndianWriter(fileStream))
                            {
                                ddsFile.Write(writer);
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}

