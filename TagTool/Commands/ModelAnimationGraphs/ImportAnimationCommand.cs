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
    public class ImportAnimationCommand : Command
    {
        private GameCacheHaloOnlineBase CacheContext { get; }
        private ModelAnimationGraph Animation { get; set; }
        private ModelAnimationGraph.FrameType AnimationType = ModelAnimationGraph.FrameType.Base;
        private ModelAnimationTagResource.GroupMemberMovementDataType FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.None;
        private bool isWorldRelative { get; set; }
        private CachedTag Jmad { get; set; }
        bool NodesBuilt = false;
        private bool ScaleFix = false;

        public ImportAnimationCommand(GameCacheHaloOnlineBase cachecontext)
            : base(false,

                  "ImportAnimation",
                  "Import animation(s) into a new ModelAnimationGraph tag",

                  "ImportAnimation <filepath> <tagname>",

                  "Import animation(s) into a new ModelAnimationGraph tag from an animation in JMA/JMM/JMO/JMR/JMW/JMZ/JMT format")
        {
            CacheContext = cachecontext;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: <filepath> <tagname>
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            var argStack = new Stack<string>(args.AsEnumerable().Reverse());

            string directoryarg = argStack.Pop();
            string tagName = argStack.Pop();

            CachedTag tag;
            if (CacheContext.TagCache.TryGetTag(tagName + ".jmad", out tag))
                return new TagToolError(CommandError.OperationFailed, "Selected TagName already exists in the cache!");

            List<FileInfo> fileList = new List<FileInfo>();

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

            Console.WriteLine($"###Importing {fileList.Count} animation(s)...");
            NodesBuilt = false;

            //set up a new animation tag
            Animation = new ModelAnimationGraph
            {
                Animations = new List<ModelAnimationGraph.Animation>(),
                ResourceGroups = new List<ModelAnimationGraph.ResourceGroup>(),
                SkeletonNodes = new List<ModelAnimationGraph.SkeletonNode>()
            };

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

                if (!NodesBuilt)
                {
                    foreach(var node in importer.AnimationNodes)
                    {
                        StringId node_name = CacheContext.StringTable.GetStringId(node.Name);
                        if (node_name == StringId.Invalid)
                            node_name = CacheContext.StringTable.AddString(node.Name);

                        ModelAnimationGraph.SkeletonNode newnode = new ModelAnimationGraph.SkeletonNode
                        {
                            Name = node_name,
                            NextSiblingNodeIndex = node.NextSiblingNode,
                            FirstChildNodeIndex = node.FirstChildNode,
                            ParentNodeIndex = node.ParentNode
                        };
                        if (importer.AnimationNodes.IndexOf(node) == 0)
                            newnode.ModelFlags |= ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.LocalRoot;
                        Animation.SkeletonNodes.Add(newnode);
                    }
                    NodesBuilt = true;
                }

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

            tag = CacheContext.TagCacheGenHO.AllocateTag(CacheContext.TagCache.TagDefinitions.GetTagDefinitionType("jmad"), tagName);

            //write out the tag
            using (var stream = CacheContext.OpenCacheReadWrite())
            {
                CacheContext.Serialize(stream, tag, Animation);
            }
            CacheContext.SaveStrings();
            CacheContext.SaveTagNames();

            Console.WriteLine("Done!");
            return true;
        }        
    }
}
