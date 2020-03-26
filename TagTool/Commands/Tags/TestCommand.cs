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
using TagTool.Geometry;
using TagTool.BlamFile;
using TagTool.Tags.Definitions.Gen1;
using TagTool.Cache.HaloOnline;
using TagTool.Havok;
using System.Linq;
using System.IO.Compression;
using TagTool.Tools.Geometry;
using TagTool.Shaders.ShaderMatching;

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

            
            var mapFilesFolder = new DirectoryInfo(@"D:\Halo\Maps\Halo3");
            var outDir = new DirectoryInfo("CacheTest");
            if (!outDir.Exists)
                outDir.Create();


            //
            // Insert what test command you want below
            //


            //var file = new FileInfo(Path.Combine(mapFilesFolder.FullName, @"descent.map"));

            //var cache = GameCache.Open(file);

            var tag = Cache.TagCache.GetTag(@"shaders\shader_templates\_0_1_0_0_1_0_1_0_1_0_0", "rmt2");

            ShaderSorter test = new ShaderSorter();
            List<int> options = new List<int>();

            var optionStrings = tag.Name.Split('\\').ToList().Last().Split('_').ToList();
            optionStrings.RemoveAt(0);
            foreach(var optStr in optionStrings)
            {
                options.Add(int.Parse(optStr));
            }
            test.PrintOptions(options);

            /*
            string filename = "test";
            HaloGeometryFormat geometryFormat = new HaloGeometryFormat();
            HaloGeometryFormatHeader header = new HaloGeometryFormatHeader();

            using (var stream = Cache.OpenCacheRead())
            {
                var tag = Cache.TagCache.GetTag(@"objects\vehicles\warthog\warthog", "mode"); // objects\vehicles\warthog\warthog
                var mode = Cache.Deserialize<RenderModel>(stream, tag);
                var resource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(mode.Geometry.Resource);
                mode.Geometry.SetResourceBuffers(resource);

                geometryFormat.InitGen3(Cache, mode);

                using (var modelStream = new FileStream($"3dsmax/{filename}.hgf", FileMode.Create))
                using (var writer = new EndianWriter(modelStream))
                {
                    HaloGeometryFormat.SerializeToFile(writer, header, geometryFormat);
                }
            }*/

            return true;
        }

        
    }
}

