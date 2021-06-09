using TagTool.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;

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

				"Generates a physics model (phmo) tag from a JSON created using Gurten's Blender exporter.\n" +
				"The model used to generate your JSON should be 1/100th the size of your render model.\n" +
				"The Blender plugin can be found here: https://github.com/Gurten/Assembly/releases \n\n" +

				"By default, a new tag with the specified name is created. If the name already exists, a new phmo will overwrite it.\n\n" +
                  
				"Node and Region data must be populated manually.\n")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            var path = "";
            var tagname = "";
            bool createNew = true;
            bool moppflag = false;

            switch (args.Count)
            {
                case 2:
                    path = args[0];
                    tagname = args[1];
                    break;
                case 3:
                    path = args[0];
                    if (args[1] == "mopp")
                        moppflag = true;
                    tagname = args[2];
                    break;
                default:
                    return new TagToolError(CommandError.ArgCount);
            }

            if (Environment.Is64BitProcess) // JsonMoppNet uses Havok 6.5.0, which is x86 only
            {
                return new TagToolError(CommandError.CustomError, "JsonMoppNet uses Havok 6.5.0, which is x86 only. Compile TagTool in 32bit (x86) to use this command!");
            }

            if (!tagname.EndsWith(".physics_model") && tagname.Contains("."))
                tagname = tagname.Substring(0, tagname.IndexOf('.')) + ".physics_model";
            else if (!tagname.EndsWith(".physics_model"))
                tagname += ".physics_model";

            if (Cache.TagCache.TryGetCachedTag(tagname, out CachedTag tag))
                createNew = false;

            var modelbuilder = new PhysicsModelBuilder();
            if (!modelbuilder.ParseFromFile(path, moppflag))
                return new TagToolError(CommandError.FileIO, "The physics model builder could not parse the specified file");

            //modelbuilder must also make a node for the physics model
            var phmo = modelbuilder.Build();

            if (phmo == null)
                return new TagToolError(CommandError.OperationFailed, "The built physics model was null!");

            using (var stream = Cache.OpenCacheReadWrite())
            {
                if (createNew)
                {
                    //original code duplicated an existing tag, trashcan phmo, from index: 
                    //Cache.TagCache.DuplicateTag(stream, Cache.TagCache.GetTagByIndex(0x4436));
                    tag = Cache.TagCache.AllocateTag(Cache.TagCache.TagDefinitions.GetTagGroupFromTag(new Tag("phmo")), tagname.Substring(0, tagname.IndexOf('.')));
                    if (tag == null)
                    {
                        new TagToolError(CommandError.CustomError, "Tag allocation failed!");
                        return true;
                    }
                }

                Cache.Serialize(stream, tag, phmo);

                if (Cache is GameCacheHaloOnlineBase)
                {
                    var hoCache = Cache as GameCacheHaloOnlineBase;
                    hoCache.SaveTagNames();
                }

                //set general phmo fixes and defaults
                var phmoDef = Cache.Deserialize<PhysicsModel>(stream, tag);

                phmoDef.Mass = 1;
                phmoDef.LowFrequencyDeactivationScale = 1;
                phmoDef.HighFrequencyDeactivationScale = 1;

                //set phmo fields supplied as arguments
                //  if (Enum.TryParse<PhysicsModel.RigidBody.MotionTypeValue>(motionType, out var temp))
                //      foreach (PhysicsModel.RigidBody rigidBody in phmoDef.RigidBodies)
                //          rigidBody.MotionType = temp;
                //  else
                //      return new TagToolError(CommandError.CustomError, "Motion type not recognized!\n" +
                //          "Motion type is set to Box (applicable for most situations) unless you specify otherwise.\n" +
                //          "Alternatives: Sphere, StabilizedSphere, StabilizedBox, Keyframed, Fixed");

                foreach (PhysicsModel.RigidBody rigidBody in phmoDef.RigidBodies)
                {
                    rigidBody.NoPhantomPowerAltRigidBody = -1;
                    rigidBody.AngularDampening = 0.05f;
                    rigidBody.InertiaTensorX = new RealVector3d(0.1f, 0, 0);
                    rigidBody.InertiaTensorY = new RealVector3d(0, 0.1f, 0);
                    rigidBody.InertiaTensorZ = new RealVector3d(0, 0, 0.1f);
                }

                Cache.Serialize(stream, tag, phmoDef);
            }

            Console.Write("Successfully generated physics model! Some manual tweaking may be necessary.\n");
            Console.WriteLine($"[Index: 0x{tag.Index:X4}] {tag}");

            return true;
        }
    }
}
