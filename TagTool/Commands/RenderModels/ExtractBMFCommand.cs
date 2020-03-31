using TagTool.Cache;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Serialization;
using Assimp;
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

            if(args.Count == 1)
            {
                modelFileName = args[0];
            }
            else
                return false;

            if (Definition.Geometry.Resource == null)
            {
                Console.WriteLine("Render model does not have a resource associated with it");
                return true;
            }

            BlamModelFile geometryFormat = new BlamModelFile();

            using (var stream = Cache.OpenCacheRead())
            {
                var resource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(Definition.Geometry.Resource);
                Definition.Geometry.SetResourceBuffers(resource);

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