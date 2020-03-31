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

            Dictionary<CachedTag, long> rmt2Values = new Dictionary<CachedTag, long>();
            ShaderSorter test = new ShaderSorter();
            foreach (var tag in Cache.TagCache.NonNull())
            {
                if (tag.IsInGroup("rmt2") && tag.Name.Contains(@"shaders\shader_template"))
                {
                    // filter rmts2 here
                    rmt2Values.Add(tag, Sorter.GetValue(test, Sorter.GetTemplateOptions(tag.Name)));
                }
            }

            var testValues = new List<int> { 1, 2, 0, 1, 7, 0, 0, 0, 2, 0 };
            var targetValue = Sorter.GetValue(test, testValues);
            long bestValue = long.MaxValue;
            CachedTag bestTag = null;

            foreach(var pair in rmt2Values)
            {
                if( Math.Abs(pair.Value - targetValue) < bestValue && !pair.Key.Name.Contains("ms30"))
                {
                    bestValue = Math.Abs(pair.Value - targetValue);
                    bestTag = pair.Key;
                }
            }

            Console.WriteLine($"Closest tag to jungle cliffs with options and value {targetValue}");
            test.PrintOptions(testValues);
            Console.WriteLine($"is tag {bestTag.Name} with options and value {bestValue + targetValue}");
            test.PrintOptions(Sorter.GetTemplateOptions(bestTag.Name));

            /*
            string filename = "test";
            BlamModelFile geometryFormat = new BlamModelFile();

            using (var stream = Cache.OpenCacheRead())
            {
                //objects\gear\human\industrial\toolbox_small\toolbox_small
                var tag = Cache.TagCache.GetTag(@"objects\vehicles\warthog\warthog", "mode"); // objects\vehicles\warthog\warthog
                var mode = Cache.Deserialize<RenderModel>(stream, tag);
                var resource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(mode.Geometry.Resource);
                mode.Geometry.SetResourceBuffers(resource);

                geometryFormat.InitGen3(Cache, mode);

                using (var modelStream = new FileStream($"3dsmax/{filename}.bmf", FileMode.Create))
                using (var writer = new EndianWriter(modelStream))
                {
                    geometryFormat.SerializeToFile(writer);
                }
            }*/

            return true;
        }

        
    }
}

