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
        private ModelAnimationTagResource.GroupMemberMovementDataType FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.None;
        private bool isWorldRelative { get; set; }
        private CachedTag Jmad { get; set; }
        private bool ReachFixup = false;

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
            if (args.Count < 1 || args.Count > 2)
                return new TagToolError(CommandError.ArgCount);

            var argStack = new Stack<string>(args.AsEnumerable().Reverse());

            if (argStack.Count == 2)
            {
                if (argStack.Pop().ToLower() == "reachfix")
                    ReachFixup = true;
                else
                    return new TagToolError(CommandError.ArgInvalid);
            }

            List<FileInfo> fileList = new List<FileInfo>();

            string directoryarg = argStack.Pop();

            if (Directory.Exists(directoryarg))
            {
                foreach (var file in Directory.GetFiles(directoryarg))
                {
                    fileList.Add(new FileInfo(file));
                }
            }
            else if (File.Exists(directoryarg))
            {
                fileList.Add(new FileInfo(directoryarg));
            }
            else
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

            Console.WriteLine($"###Adding {fileList.Count} animation(s)...");

            foreach (var filepath in fileList)
            {
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
                        FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy;
                        break;
                    case ".JMT":
                        FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dyaw;
                        Console.WriteLine("###WARNING: Advanced Movement data not currently supported, animation may not display properly!");
                        break;
                    case ".JMZ":
                        FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.dx_dy_dz_dyaw;
                        Console.WriteLine("###WARNING: Advanced Movement data not currently supported, animation may not display properly!");
                        break;
                    default:
                        Console.WriteLine($"###ERROR: Filetype {file_extension.ToUpper()} not recognized!");
                        return false;
                }

                //get or create stringid for animation block name
                string file_name = Path.GetFileNameWithoutExtension(filepath.FullName).Replace(' ', ':');
                StringId animation_name = CacheContext.StringTable.GetStringId(file_name);
                if (animation_name == StringId.Invalid)
                    animation_name = CacheContext.StringTable.AddString(file_name);

                //create new importer class and import the source file
                var importer = new AnimationImporter();
                if (!importer.Import(filepath.FullName))
                    continue;

                if(importer.Version >= 16394)
                {
                    string errormessage = "###ERROR: Only Halo:CE animation files are currently supported because newer versions offer no benefits but add node-space complications. " + 
                        "Please export your animations to Halo:CE format (JMA Version < 16394) and try importing again.";
                    return new TagToolError(CommandError.OperationFailed, errormessage);
                }

                //fixup Reach FP animations
                if (ReachFixup)
                    FixupReachFP(importer);

                //Adjust imported nodes to ensure that they align with the jmad
                AdjustImportedNodes(importer);

                //process node data in advance of serialization
                importer.ProcessNodeFrames((GameCacheHaloOnlineBase)CacheContext, ModelList, AnimationType, FrameInfoType);

                //Check the nodes to verify that this animation can be imported to this jmad
                //if (!importer.CompareNodes(Animation.SkeletonNodes, (GameCacheHaloOnlineBase)CacheContext))
                //    return false;

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
                        FrameInfoType = (AnimationMovementDataType)FrameInfoType,
                        BlendScreen = -1,
                        DesiredCompression = ModelAnimationGraph.Animation.CompressionValue.BestAccuracy,
                        CurrentCompression = ModelAnimationGraph.Animation.CompressionValue.BestAccuracy,
                        FrameCount = (short)importer.frameCount,
                        NodeCount = (sbyte)importer.AnimationNodes.Count,
                        NodeListChecksum = (int)(importer.CalculateNodeListChecksum(0)),
                        Unknown2 = 5, //don't know what these do, but set usual values
                        Unknown3 = 6,
                        Heading = new RealVector3d(1, 0, 0),
                        PreviousVariantSibling = -1,
                        NextVariantSibling = -1,
                        ResourceGroupIndex = (short)(Animation.ResourceGroups.Count - 1),
                        ResourceGroupMemberIndex = 0,
                    }
                };

                if (isWorldRelative)
                    AnimationBlock.AnimationData.InternalFlags |= ModelAnimationGraph.Animation.InternalFlagsValue.WorldRelative;

                Animation.Animations.Add(AnimationBlock);
                Console.WriteLine($"Added {file_name} successfully!");
            }
            //save changes to the current tag
            CacheContext.SaveStrings();
            using (Stream cachestream = CacheContext.OpenCacheReadWrite())
            {
                CacheContext.Serialize(cachestream, Jmad, Animation);
            }

            Console.WriteLine("Done!");
            return true;
        }

        public void AdjustImportedNodes(AnimationImporter importer)
        {
            //now order imported nodes according to jmad nodes
            List<AnimationImporter.AnimationNode> newAnimationNodes = new List<AnimationImporter.AnimationNode>();
            foreach (var skellynode in Animation.SkeletonNodes)
            {
                string nodeName = CacheContext.StringTable.GetString(skellynode.Name);
                int matching_index = importer.AnimationNodes.FindIndex(x => x.Name.Equals(nodeName));
                if (matching_index == -1)
                {
                    Console.WriteLine($"###WARNING: No node matching '{nodeName}' found in imported file! Will proceed with blank data for missing node");
                    newAnimationNodes.Add(new AnimationImporter.AnimationNode() { Name = nodeName, FirstChildNode = skellynode.FirstChildNodeIndex, NextSiblingNode = skellynode.NextSiblingNodeIndex, ParentNode = skellynode.ParentNodeIndex });
                }
                else
                {
                    AnimationImporter.AnimationNode matching_node = importer.AnimationNodes[matching_index];
                    matching_node.FirstChildNode = skellynode.FirstChildNodeIndex;
                    matching_node.NextSiblingNode = skellynode.NextSiblingNodeIndex;
                    matching_node.ParentNode = skellynode.ParentNodeIndex;
                    newAnimationNodes.Add(matching_node);
                }
            }

            //set importer animation nodes to newly sorted list
            importer.AnimationNodes = newAnimationNodes;
        }

        public void FixupReachFP(AnimationImporter importer)
        {
            var imported_nodes = importer.AnimationNodes;

            int basenode_index = imported_nodes.FindIndex(x => x.Name.Equals("base"));
            if (basenode_index != -1)
            {
                //fixup rotated base node
                foreach (var Frame in imported_nodes[basenode_index].Frames)
                {
                    Frame.Rotation = new RealQuaternion(0, 0, 0, 1);
                }
            }

            //fix weapon IK marker
            if (Animation.Modes.Count > 0)
            {
                if (Animation.Modes[0].WeaponClass.Count > 0)
                {
                    if (Animation.Modes[0].WeaponClass[0].WeaponIk.Count > 0)
                    {
                        Animation.Modes[0].WeaponClass[0].WeaponIk[0].AttachToMarker = CacheContext.StringTable.GetStringId("left_hand_spartan_fp");
                    }
                }
            }
        }
    }
}
