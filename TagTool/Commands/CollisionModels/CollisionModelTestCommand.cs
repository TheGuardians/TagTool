using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.CollisionModels
{
    class CollisionModelTestCommand : Command
    {
        private GameCacheHaloOnlineBase Cache { get; }

        public CollisionModelTestCommand(GameCacheHaloOnlineBase cache)
            : base(false,
                  
                  "CollisionModelTest",
                  "Collision geometry import command (Test)",
                  
                  "CollisionModelTest <filepath>|<dirpath> <index>|<new> [force]",
                  
                  "Insert a collision_geometry tag compiled from Halo1 CE Tool.\n" +
                  "A file path can be specified to load from a single Halo 1 coll tag or a directory name " +
                  "for a directory with many coll tags can be loaded as separate BSPs.\n" +
                  "A tag-index can be specified to override an existing tag, or 'new' can be used to create a new tag.\n" +
                  "Tags that are not type- 'coll' will not be overridden unless the third argument is specified- 'force'.")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: filepath, <new>|<tagIndex>
            if (args.Count < 2)
            {
                return false;
            }

            CachedTag tag;

            // optional argument: forces overwriting of tags that are not type: coll
            var b_force = (args.Count >= 3 && args[2].ToLower().Equals("force"));

            if (args[1].ToLower().Equals("new") && TagGroup.Instances.TryGetValue("coll", out var collGroup))
            {
                tag = Cache.TagCacheGenHO.AllocateTag(collGroup);
            }
            else
            {
                if (!Cache.TryGetTag(args[1], out tag))
                    return false;
            }

            if (!b_force && !tag.IsInGroup("coll"))
            {
                Console.WriteLine("Tag to override was not of class- 'coll'. Use third argument- 'force' to inject into this tag.");
                return false;
            }

            string filepath = args[0];
            string[] fpaths = null;
            CollisionModel coll = null;
            bool b_singleFile = Path.GetExtension(filepath).Equals(".model_collision_geometry")
                && !Directory.Exists(filepath);

            var modelbuilder = new CollisionGeometryBuilder();
            int n_objects = 1;

            if (!b_singleFile)
            {
                fpaths = Directory.GetFiles(filepath, "*.model_collision_geometry");

                if (fpaths.Length == 0)
                {
                    Console.WriteLine("No Halo 1 coll tags in directory: \"{0}\"", filepath);
                    return false;
                }

                filepath = fpaths[0];
                n_objects = fpaths.Length;
            }
            
            Console.WriteLine(
                (n_objects == 1 ? "Loading coll tag..." : "Loading coll tags..."), n_objects);

            if (!modelbuilder.ParseFromFile(filepath))
                return false;

            coll = modelbuilder.Build();

            if (coll == null)
            {
                Console.WriteLine("Builder produced null result.");
                return false;
            }

            if (!b_singleFile)
            {
                for (int i = 1; i < fpaths.Length; ++i)
                {
                    if (!modelbuilder.ParseFromFile(fpaths[i]))
                        return false;

                    var coll2 = modelbuilder.Build();

                    if (coll2 == null)
                    {
                        Console.WriteLine("Builder produced null result.");
                        return false;
                    }

                    coll.Regions.Add(coll2.Regions[0]);
                }
            }

            using (var stream = Cache.OpenCacheReadWrite())
            {
                Cache.Serialize(stream, tag, coll);
            }

            Console.WriteLine(n_objects == 1 ? "Added 1 collision." : "Added {0} collisions in one tag.", n_objects);
            Console.Write("Successfully imported coll to: ");
            TagPrinter.PrintTagShort(tag);

            return true;
        }
    }
}
