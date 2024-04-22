using System.Collections.Generic;
using System.IO;
using TagTool.BlamFile;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.IO;

namespace TagTool.Commands.Files
{
    class ExtractBlfImageCommand : Command
    {
        public ExtractBlfImageCommand()
            : base(true,

                  "ExtractBlfImage",
                  "Extracts the image from a blf",

                  "ExtractBlfImage <blf path> <output jpg> [version]",
                  "Extracts the image from a blf")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2 || args.Count > 3)
                return new TagToolError(CommandError.ArgCount);

            FileInfo file = new FileInfo(args[0]);
            if (!file.Exists)
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\" does not exist at the specified path");

            FileInfo output = new FileInfo(args[1]);
            if (!output.Directory.Exists)
                output.Directory.Create();

            CacheVersion version = CacheVersion.Halo3Retail;
            CachePlatform cachePlatform = CachePlatform.Original;

            // todo: support little endian
            /*if (args.Count == 3)
            {
                if (CacheVersion.TryParse(args[2], out CacheVersion tempVersion))
                    version = tempVersion;
            }*/

            Blf blf = new Blf(version, cachePlatform);

            using (var stream = file.OpenRead())
            using (var reader = new EndianReader(stream))
            {
                if (version == CacheVersion.Halo3Retail || version == CacheVersion.Halo3ODST)
                    reader.Format = EndianFormat.BigEndian;
                if (!blf.Read(reader))
                    return new TagToolError(CommandError.CustomMessage, "Could not parse BLF");
            }

            if (!blf.ContentFlags.HasFlag(BlfFileContentFlags.MapImage) || blf.JpegImage == null || blf.JpegImage.Length == 0)
                return new TagToolError(CommandError.CustomMessage, "BLF does not contain image");

            using (var stream = output.Create())
            {
                stream.Write(blf.JpegImage, 0, blf.JpegImage.Length);
            }

            return true;
        }
    }
}
