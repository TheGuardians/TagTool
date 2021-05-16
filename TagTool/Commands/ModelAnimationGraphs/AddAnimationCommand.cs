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
        private bool BaseFix = false;
        private bool CameraFix = false;
        private bool ScaleFix = false;

        public AddAnimationCommand(GameCache cachecontext, ModelAnimationGraph animation, CachedTag jmad)
            : base(false,

                  "AddAnimation",
                  "Add an animation to a ModelAnimationGraph tag",

                  "AddAnimation [basefix] [camerafix] [scalefix] <filepath>",

                  "Add an animation to a ModelAnimationGraph tag from an animation in JMA/JMM/JMO/JMR/JMW/JMZ/JMT format")
        {
            CacheContext = cachecontext;
            Animation = animation;
            Jmad = jmad;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: <filepath>
            if (args.Count < 1 || args.Count > 3)
                return new TagToolError(CommandError.ArgCount);

            var argStack = new Stack<string>(args.AsEnumerable().Reverse());

            BaseFix = false;
            CameraFix = false;
            ScaleFix = false;
            while (argStack.Count > 1)
            {
                var arg = argStack.Peek();
                switch (arg.ToLower())
                {
                    case "basefix":
                        BaseFix = true;
                        argStack.Pop();
                        break;
                    case "camerafix":
                        CameraFix = true;
                        argStack.Pop();
                        break;
                    case "scalefix":
                        ScaleFix = true;
                        argStack.Pop();
                        break;
                    default:
                        return new TagToolError(CommandError.ArgInvalid);
                }
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

            Console.WriteLine($"###Adding {fileList.Count} animation(s)...");

            foreach (var filepath in fileList)
            {
                string file_extension = filepath.Extension;

                AnimationType = ModelAnimationGraph.FrameType.Base;
                isWorldRelative = false;
                FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.None;

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
                        new TagToolError(CommandError.CustomError, $"Filetype {file_extension.ToUpper()} not recognized!");
                        return false;
                }

                //get or create stringid for animation block name
                string file_name = Path.GetFileNameWithoutExtension(filepath.FullName).Replace(' ', ':');
                StringId animation_name = CacheContext.StringTable.GetStringId(file_name);
                if (animation_name == StringId.Invalid)
                    animation_name = CacheContext.StringTable.AddString(file_name);

                //create new importer class and import the source file
                var importer = new AnimationImporter();
                importer.ScaleFix = ScaleFix;
                if (!importer.Import(filepath.FullName))
                    continue;

                if(importer.Version >= 16394)
                {
                    string errormessage = "Only Halo:CE animation files are currently supported because newer versions offer no benefits but add node-space complications. " + 
                        "Please export your animations to Halo:CE format (JMA Version < 16394) and try importing again.";
                    return new TagToolError(CommandError.CustomError, errormessage);
                }

                //fixup Base node position/rotation/scale
                if (BaseFix)
                    FixupBaseNode(importer);

                //add camera_control node at position 0, useful for Halo:CE animations
                if (CameraFix)
                    AddCameraNode(importer);

                //Adjust imported nodes to ensure that they align with the jmad
                AdjustImportedNodes(importer);

                //process node data in advance of serialization
                importer.ProcessNodeFrames((GameCacheHaloOnlineBase)CacheContext, AnimationType, FrameInfoType);

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

        public void FixupBaseNode(AnimationImporter importer)
        {
            var imported_nodes = importer.AnimationNodes;

            int basenode_index = imported_nodes.FindIndex(x => x.Name.Equals("base"));
            if (basenode_index != -1)
            {
                //fixup base node
                foreach (var Frame in imported_nodes[basenode_index].Frames)
                {
                    Frame.Rotation = new RealQuaternion(0, 0, 0, 1);
                    Frame.Translation = new RealPoint3d(0, 0, 0);
                    Frame.Scale = 1.0f;
                }
            }
        }

        public void AddCameraNode(AnimationImporter importer)
        {
            var imported_nodes = importer.AnimationNodes;
            int camera_index = imported_nodes.FindIndex(x => x.Name.Equals("camera_control"));
            if (camera_index != -1)
            {
                new TagToolError(CommandError.CustomError, "You already have a camera_control node! Skipping camerafix...");
                return;
            }
            AnimationImporter.AnimationNode newnode = new AnimationImporter.AnimationNode
            {
                Name = "camera_control",
                Frames = new List<AnimationImporter.AnimationFrame>(),
                hasStaticRotation = true,
                hasStaticTranslation= true
            };
            for(int i = 0; i < importer.frameCount; i++)
            {
                newnode.Frames.Add(new AnimationImporter.AnimationFrame());
            }
            importer.AnimationNodes.Add(newnode);
        }
    }
}
