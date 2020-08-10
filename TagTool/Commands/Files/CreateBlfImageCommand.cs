using System.Collections.Generic;
using System.IO;
using TagTool.BlamFile;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.IO;

namespace TagTool.Commands.Files
{
    class CreateBlfImageCommand : Command
    {
        public CreateBlfImageCommand()
            : base(true,

                  "CreateBlfImage",
                  "Creates a BLF from a JPEG image",

                  "CreateBlfImage <jpg path> <output blf>",
                  "Creates a BLF from a JPEG image\n" +
                  "Note: only little endian is currently supported")
        {
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            FileInfo file = new FileInfo(args[0]);
            if (!file.Exists)
                return new TagToolError(CommandError.ArgInvalid, $"\"{args[0]}\" does not exist at the specified path");

            FileInfo output = new FileInfo(args[1]);
            if (!output.Directory.Exists)
                output.Directory.Create();

            byte[] jpgImage = null;

            using (var stream = file.OpenRead())
            using (var reader = new EndianReader(stream))
            {
                if (stream.Length > 0)
                    jpgImage = reader.ReadBytes((int)stream.Length);
            }

            if (jpgImage == null || jpgImage.Length == 0)
                return new TagToolError(CommandError.CustomMessage, "Invalid image");

            CacheVersion version = CacheVersion.HaloOnline106708;

            Blf blf = new Blf(version)
            {
                StartOfFile = new BlfChunkStartOfFile
                {
                    Signature = "_blf",
                    Length = 0x30,
                    MajorVersion = 1,
                    MinorVersion = 2,
                    ByteOrderMarker = -2,
                    Unknown = 0,
                    InternalName = "",
                },
                ContentFlags = BlfFileContentFlags.StartOfFile | BlfFileContentFlags.MapImage | BlfFileContentFlags.EndOfFile,
                JpegImage = jpgImage,
                EndOfFile = new BlfChunkEndOfFile
                {
                    Signature = "_eof",
                    Length = 0x11,
                    MajorVersion = 1,
                    MinorVersion = 1,
                    AuthenticationDataSize = 0,
                    AuthenticationType = BlfAuthenticationType.None,
                }
            };

            blf.MapImage = new BlfMapImage
            {
                Signature = "mapi",
                Length = 0x14 + blf.JpegImage.Length,
                MajorVersion = 1,
                MinorVersion = 1,
                Unknown = 0,
                JpegSize = blf.JpegImage.Length,
            };

            using (var stream = output.Create())
            using (var writer = new EndianWriter(stream))
            {
                blf.Write(writer);
            }

            return true;
        }
    }
}
