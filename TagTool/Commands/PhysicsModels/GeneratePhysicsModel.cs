using TagTool.Geometry;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Geometry.Jms;

namespace TagTool.Commands.PhysicsModels
{
    class GeneratePhysicsModelCommand : Command
    {
        private GameCache Cache { get; }

        public GeneratePhysicsModelCommand(GameCache cache)
            : base(false,

                "GeneratePhysicsModel",
                "Physics model import command.",

                "GeneratePhysicsModel [mopp] <filepath> <tagname>",

                "Generates a physics model (phmo) tag from a JSON or JMS file.\n" +
                "For JSON: Created using Gurten's Blender exporter.\n" +
                "For JMS: Created using a custom exporter or tool.\n\n" +
                "The model used to generate your file should be 1/100th the size of your render model.\n" +
                "Node and Region data must be populated manually.\n")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2)
            {
                return new TagToolError(CommandError.ArgCount, "Not enough arguments.");
            }

            var path = args[args.Count - 2];
            var tagname = args.Last();
            var moppflag = args.Contains("mopp");

            // Determine the file type
            var extension = Path.GetExtension(path).ToLower();

            object result = null;
            using (var stream = Cache.OpenCacheReadWrite())
            {
                // Handle based on file extension
                switch (extension)
                {
                    case ".json":
                        var generator = new PhysicsModelGenerator(Cache);
                        result = generator.GeneratePhysicsModel(stream, path, tagname, moppflag, stream);
                        break;
                    case ".jms":
                        var JMSGenerator = new PhysicsModelGenerator(Cache);
                        JmsFormat jms = new JmsFormat();
                        // Create a FileInfo object from the path
                        FileInfo fileInfo = new FileInfo(path);
                        // Check if the file exists
                        if (!fileInfo.Exists)
                        {
                            return new TagToolError(CommandError.FileNotFound, $"File not found: {path}");
                        }
                        // Attempt to deserialize the JMS file
                        if (jms.TryRead(fileInfo))
                        {
                            // If deserialization is successful, proceed with generating the physics model from JMS
                            result = JMSGenerator.GeneratePhysicsModelFromJms(stream, jms, tagname, moppflag, null, stream);
                        }
                        else
                        {
                            // Handle the case where JMS deserialization fails
                            return new TagToolError(CommandError.CustomError, "Failed to deserialize JMS file.");
                        }
                        break;
                    default:
                        return new TagToolError(CommandError.ArgInvalid, "Unsupported file format.");
                }

                if (result is TagToolError error)
                {
                    return error;
                }

                var tag = (CachedTag)result;
                Console.WriteLine($"Successfully generated physics model! [Index: 0x{tag.Index:X4}] {tag.Group} {tag.Index}");
            }
            return true;
        }
    }
}