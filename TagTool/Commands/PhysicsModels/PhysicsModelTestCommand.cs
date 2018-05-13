using TagTool.Geometry;
using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Serialization;
using TagTool.Commands;
using TagTool.Common;
using TagTool.Commands.Common;

namespace TagTool.Commands.PhysicsModels
{
    class PhysicsModelTestCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public PhysicsModelTestCommand(HaloOnlineCacheContext cacheContext)
            : base(CommandFlags.None,
                  
                  "PhysicsModelTest",
                  "Physics model import command (Test)",
                  
                  "PhysicsModelTest <filepath> <index>|<new> [force]",
                  
                  "Imports a physics model from the file specified exported from Blender in JSON format.\n" +
                  "A tag-index can be specified to override an existing tag, or 'new' can be used to create a new tag.\n" +
            "Tags that are not type- 'phmo' will not be overridden unless the third argument is specified- 'force'. ")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: filepath, <new>|<tagIndex>
            if (args.Count < 2)
            {
                return false;
            }

            CachedTagInstance tag = null;
            bool b_duplicate;
            // optional argument: forces overwriting of tags that are not type: phmo
            bool b_force = (args.Count >= 3 && args[2].ToLower().Equals("force"));

            if (args[1].ToLower().Equals("new"))
            {
                b_duplicate = true;
            }
            else
            {
                tag = ArgumentParser.ParseTagSpecifier(CacheContext, args[1]);
                if (tag == null)
                {
                    return false;
                }
                b_duplicate = false;
            }

            if (!b_force && !b_duplicate && !tag.IsInGroup("phmo"))
            {
                Console.WriteLine("Tag to override was not of class- 'phmo'. Use third argument- 'force' to inject.");
                return false;
            }

            var filename = args[0];

            var modelbuilder = new PhysicsModelBuilder();
            if (!modelbuilder.ParseFromFile(filename))
            {
                return false;
            }
            //modelbuilder must also make a node for the physics model
            var phmo = modelbuilder.Build();

            if (phmo == null)
            {
                return false;
            }

            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {

                if (b_duplicate)
                {
                    //duplicate an existing tag, trashcan phmo
                    tag = CacheContext.TagCache.DuplicateTag(stream, CacheContext.TagCache.Index[0x4436]);
                    if (tag == null)
                    {
                        Console.WriteLine("Failed tag duplication.");
                        return false;
                    }
                }

                var context = new TagSerializationContext(stream, CacheContext, tag);
                CacheContext.Serializer.Serialize(context, phmo);

            }

            Console.Write("Successfully imported phmo to: ");

            TagPrinter.PrintTagShort(tag);

            return true;
        }
    }
}
