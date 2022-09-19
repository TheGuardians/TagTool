using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Bitmaps;
using TagTool.Bitmaps.DDS;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Bitmaps
{
    class ExtractBitmapCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private Bitmap Bitmap { get; }

        public ExtractBitmapCommand(GameCache cache, CachedTag tag, Bitmap bitmap)
            : base(false,

                  "ExtractBitmap",
                  "Extracts a bitmap to a file.",

                  "ExtractBitmap <output directory>",

                  "Extracts a bitmap to a file.")
        {
            Cache = cache;
            Tag = tag;
            Bitmap = bitmap;
        }

        public override object Execute(List<string> args)
        {
            string directory;

            if (args.Count == 1)
            {
                directory = args[0];
            }
            else if (args.Count == 0)
            {
                directory = "Bitmaps";
            }
            else
                return new TagToolError(CommandError.ArgCount);

            Directory.CreateDirectory(directory);

            var ddsOutDir = directory;
            string name;

            if(Tag.Name != null)
            {
                var split = Tag.Name.Split('\\');
                name = split[split.Length - 1];
            }
            else
                name = Tag.Index.ToString("X8");

            if (Bitmap.Images.Count > 1)
            {
                ddsOutDir = Path.Combine(directory, name);
                Directory.CreateDirectory(ddsOutDir);
            }

            for (var i = 0; i < Bitmap.Images.Count; i++)
            {
                var bitmapName = (Bitmap.Images.Count > 1) ? i.ToString() : name;

                var outPath = Path.Combine(ddsOutDir, bitmapName);
                var ddsFile = BitmapExtractor.ExtractBitmap(Cache, Bitmap, i);

                if (File.Exists(outPath + ".dds"))
                {
                    var bitmLongName = bitmapName + CreateLongName(Tag);
                    outPath = Path.Combine(ddsOutDir, bitmLongName);
                }

                outPath += ".dds";

                if (ddsFile == null || ddsFile.BitmapData is null)
                    return new TagToolError(CommandError.OperationFailed, "Invalid bitmap data");

                using(var fileStream = File.Open(outPath, FileMode.Create, FileAccess.Write))
                using(var writer = new EndianWriter(fileStream, EndianFormat.LittleEndian))
                {
                    ddsFile.Write(writer);
                }
            }

            Console.WriteLine("Done!");

            return true;
        }

        public string CreateLongName(CachedTag tag)
        {
            string concatenation = " (";
            List<string> split = tag.Name.Split('\\').ToList();
            string[] cutKeywords = { "objects", "levels", "multi", "dlc", "solo", "characters"};

            split.RemoveAt(split.Count - 1);
            foreach (string s in cutKeywords)
            {
                if (split.Contains(s))
                    split.RemoveAt(split.IndexOf(s));
            }

            concatenation += string.Join("_", split.ToArray()) + ")";

            return concatenation;
        }
    }
}
