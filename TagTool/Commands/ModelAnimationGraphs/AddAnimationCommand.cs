using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using System.IO;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Commands.Common;
using TagTool.Animations;
using TagTool.Tags.Resources;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.ModelAnimationGraphs
{
    public class AddAnimationCommand : Command
    {
        private GameCache CacheContext { get; }
        private ModelAnimationGraph Animation { get; set; }
        private ModelAnimationGraph.FrameType AnimationType = ModelAnimationGraph.FrameType.Base;
        private bool isWorldRelative { get; set; }
        private CachedTag Jmad { get; set; }

        public AddAnimationCommand(GameCache cachecontext, ModelAnimationGraph animation, CachedTag jmad)
            : base(false,

                  "AddAnimation",
                  "Add an animation to a ModelAnimationGraph tag",

                  "AddAnimation <filepath>",

                  "Add an animation to a ModelAnimationGraph tag from an animation in JMA/JMM/JMO/JMR/JMW/JMZ/JMT format")
        {
            CacheContext = cachecontext;
            Animation = animation;
            Jmad = jmad;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: <filepath>
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            FileInfo filepath = new FileInfo(args[0]);
            if (!File.Exists(filepath.FullName))
                return new TagToolError(CommandError.FileNotFound);

            List<string> ModelList = new List<string>();
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine("Enter the tagname of each render model tag that this animation uses");
            Console.WriteLine("Enter a blank line to finish.");
            Console.WriteLine("------------------------------------------------------------------");
            for (string line; !String.IsNullOrWhiteSpace(line = Console.ReadLine());)
            {
                //remove any tag type info from tagname
                line = line.Split('.')[0];
                ModelList.Add(line);
            }

            string file_extension = filepath.Extension;

            switch (file_extension.ToUpper())
            {
                case ".JMM":
                    break;
                case ".JMW":
                    isWorldRelative = true;
                    break;
                case ".JMO":
                    AnimationType = ModelAnimationGraph.FrameType.Overlay;
                    break;
                case ".JMR":
                    AnimationType = ModelAnimationGraph.FrameType.Replacement;
                    break;
                case ".JMA":
                case ".JMT":
                case ".JMZ":
                    Console.WriteLine("###WARNING: Movement data not currently supported, animation may not display properly!");
                    break;
                default:
                    Console.WriteLine($"###ERROR: Filetype {file_extension.ToUpper()} not recognized!");
                    return false;
            }

            //get or create stringid for animation block name
            string file_name = Path.GetFileNameWithoutExtension(filepath.FullName).Replace(' ', ':');
            StringId animation_name = CacheContext.StringTable.GetStringId(file_name);
            if(animation_name == StringId.Invalid)
                animation_name = CacheContext.StringTable.AddString(file_name);

            //create new importer class and import the source file
            var importer = new AnimationImporter();
            importer.Import(filepath.FullName, (GameCacheHaloOnlineBase)CacheContext, ModelList, AnimationType);

            //Check the nodes to verify that this animation can be imported to this jmad
            if (!importer.CompareNodes(Animation.SkeletonNodes, (GameCacheHaloOnlineBase)CacheContext))
                return false;

            //build a new resource 
            ModelAnimationTagResource newResource = new ModelAnimationTagResource
            {
                GroupMembers = new TagTool.Tags.TagBlock<ModelAnimationTagResource.GroupMember>()
            };
            newResource.GroupMembers.Add(importer.SerializeAnimationData((GameCacheHaloOnlineBase)CacheContext));
            newResource.GroupMembers.AddressType = CacheAddressType.Definition;
            //serialize the new resource into the cache
            TagResourceReference resourceref = CacheContext.ResourceCache.CreateModelAnimationGraphResource(newResource);

            //add resource reference to the animation tag
            Animation.ResourceGroups.Add(new ModelAnimationGraph.ResourceGroup
            {
                ResourceReference = resourceref,
                MemberCount = 1
            });

            //serialize animation block values
            var AnimationBlock = new ModelAnimationGraph.Animation
            {
                Name = animation_name,
                AnimationData = new ModelAnimationGraph.Animation.SharedAnimationData
                {
                    AnimationType = AnimationType,
                    BlendScreen = -1,
                    DesiredCompression = ModelAnimationGraph.Animation.CompressionValue.BestAccuracy,
                    CurrentCompression = ModelAnimationGraph.Animation.CompressionValue.BestAccuracy,
                    FrameCount = (short)importer.frameCount,
                    NodeCount = (sbyte)importer.AnimationNodes.Count,
                    NodeListChecksum = importer.nodeChecksum,
                    Unknown2 = 5, //don't know what these do, but set usual values
                    Unknown3 = 6,
                    Heading = new RealVector3d(1,0,0),
                    PreviousVariantSibling = -1,
                    NextVariantSibling = -1,
                    ResourceGroupIndex = (short)(Animation.ResourceGroups.Count - 1),
                    ResourceGroupMemberIndex = 0,
                }
            };

            if(isWorldRelative)
                AnimationBlock.AnimationData.InternalFlags |= ModelAnimationGraph.Animation.InternalFlagsValue.WorldRelative;

            Animation.Animations.Add(AnimationBlock);

            //save changes to the current tag
            CacheContext.SaveStrings();
            using (Stream cachestream = CacheContext.OpenCacheReadWrite())
            {
                CacheContext.Serialize(cachestream, Jmad, Animation);
            }

            Console.WriteLine("Animation added successfully!");
            return true;
        }
    }
}
