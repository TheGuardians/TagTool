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
                var name = @"objects\weapons\rifle\assault_rifle\bitmaps\assault_rifle";

                var tag = Cache.TagCache.GetTagByName(name, "bitm");
                if (tag.Group.Tag == "bitm")
                {
                    var bitmap = Cache.Deserialize<Bitmap>(stream, tag);

                    for (int i = 0; i < bitmap.Images.Count; i++)
                    {
                        var baseBitmap = BitmapConverterNew.ConvertGen3Bitmap(Cache, bitmap, i);
                        var ddsFile = new DDSFile(baseBitmap);

                        var fileName = $"Bitmaps\\test_port_{i}";
                        var file = new FileInfo(fileName);

                        using (var fileStream = file.Create())
                        using(var writer = new EndianWriter(fileStream))
                        {
                            ddsFile.Write(writer);
                        }
                    }
                }
            }

            return true;
        }
    }
}

