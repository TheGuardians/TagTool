using System;
using System.Collections.Generic;
using System.IO;
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

            if (!Directory.Exists(directory))
            {
                Console.Write("Destination directory does not exist. Create it? [y/n] ");
                var answer = Console.ReadLine().ToLower();

                if (answer.Length == 0 || !(answer.StartsWith("y") || answer.StartsWith("n")))
                    return new TagToolError(CommandError.YesNoSyntax);

                if (answer.StartsWith("y"))
                    Directory.CreateDirectory(directory);
                else
                    return true;
            }

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
                bitmapName += ".dds";
                var outPath = Path.Combine(ddsOutDir, bitmapName);

                var ddsFile = BitmapExtractor.ExtractBitmap(Cache, Bitmap, i);

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
    }
}
