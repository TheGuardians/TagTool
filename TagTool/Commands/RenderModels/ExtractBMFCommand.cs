using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tools.Geometry;
using TagTool.IO;

namespace TagTool.Commands.RenderModels
{
    class ExtractBMFCommand : Command
    {
        private GameCache Cache { get; }
        private RenderModel Definition { get; }

        public ExtractBMFCommand(GameCache cacheContext, RenderModel model)
            : base(true,

                  "ExtractBMF",
                  "Extracts the current render model definition to BMF. (See tools folder for the 3ds max scripts)",

                  "ExtractBMF <filename>.bmf",

                  "Extracts the current render model definition to BMF. (See tools folder for the 3ds max scripts) \n" +
                  "")
        {
            Cache = cacheContext;
            Definition = model;
        }
        
        public override object Execute(List<string> args)
        {
            string modelFileName;

            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            modelFileName = args[0];

            if (Definition.Geometry.Resource == null)
            {
                Console.WriteLine("Render model does not have a resource associated with it");
                return true;
            }

            BlamModelFile geometryFormat = new BlamModelFile();

            using (var stream = Cache.OpenCacheRead())
            {
                var resource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(Definition.Geometry.Resource);
                Definition.Geometry.SetResourceBuffers(resource, false);

                geometryFormat.InitGen3(Cache, Definition);

                using (var modelStream = new FileStream($"{modelFileName}.bmf", FileMode.Create))
                using (var writer = new EndianWriter(modelStream))
                {
                    geometryFormat.SerializeToFile(writer);
                }
            }
            return true;
        }
    }
}