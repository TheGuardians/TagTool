using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using System.IO;
using TagTool.Cache;
using TagTool.Serialization;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Commands.Common;
using TagTool.Animations;
using TagTool.Tags.Resources;
using TagTool.Cache.HaloOnline;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.ModelAnimationGraphs
{
    public class RebuildFPAnimationCommand : Command
    {
        private GameCacheHaloOnlineBase CacheContext { get; }
        private ModelAnimationGraph Animation { get; set; }
        private ModelAnimationGraph.FrameType AnimationType = ModelAnimationGraph.FrameType.Base;
        private ModelAnimationTagResource.GroupMemberMovementDataType FrameInfoType = ModelAnimationTagResource.GroupMemberMovementDataType.None;
        private bool isWorldRelative { get; set; }
        private CachedTag Jmad { get; set; }
        private bool ReachFixup = false;
        private bool ScaleFix = false;

        public RebuildFPAnimationCommand(GameCache cachecontext, ModelAnimationGraph animation, CachedTag jmad)
            : base(false,

                  "RebuildFPAnimation",
                  "Rebuild a first person animation or animations in a ModelAnimationGraph tag",

                  "RebuildFPAnimation [reachfix] <file or folder path>",

                  "Rebuild a first person animation or animations in a ModelAnimationGraph tag from animations in JMA/JMM/JMO/JMR/JMW/JMZ/JMT format\n" +
                  "All animation files must be named the same as animations in the tag, with the space character in place of the ':' character\n" +
                  "Specify a folder to replace multiple animations or a file to replace a single animation")
        {
            CacheContext = (GameCacheHaloOnlineBase)cachecontext;
            Animation = animation;
            Jmad = jmad;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: <filepath>
            if (args.Count < 1 || args.Count > 2)
                return new TagToolError(CommandError.ArgCount);

            var argStack = new Stack<string>(args.AsEnumerable().Reverse());

            if(argStack.Count == 2)
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
                foreach(var file in Directory.GetFiles(directoryarg))
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
            Console.WriteLine("Enter the tagname of the primary (actor) render model tag followed by the tagname of the secondary (object) render model tag");
            Console.WriteLine("Enter a blank line to finish.");
            Console.WriteLine("------------------------------------------------------------------");
            for (string line; !String.IsNullOrWhiteSpace(line = Console.ReadLine());)
            {
                //remove any tag type info from tagname
                line = line.Split('.')[0];
                ModelList.Add(line);
            }

            //Adjust jmad nodes to match secondary model
            AdjustjmadNodes(ModelList[0], ModelList[1]);

            Console.WriteLine($"###Replacing {fileList.Count} animation(s)...");

            foreach(var filepath in fileList)
            {
                string file_extension = filepath.Extension;
                AnimationType = ModelAnimationGraph.FrameType.Base;
                isWorldRelative = false;

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

                //find existing animation block that matches animation filename
                int matchingindex = -1;
                foreach(var animationblock in Animation.Animations)
                {
                    if(animationblock.Name == animation_name)
                    {
                        matchingindex = Animation.Animations.IndexOf(animationblock);
                        break;
                    }
                }
                if(matchingindex == -1)
                {
                    Console.WriteLine($"###WARNING: No existing animation found for animation {file_name}!");
                    continue;
                }

                //create new importer class and import the source file
                var importer = new AnimationImporter();
                importer.ScaleFix = ScaleFix;
                if (!importer.Import(filepath.FullName))
                    continue;

                if (importer.Version >= 16394)
                {
                    string errormessage = "Only Halo:CE animation files are currently supported because newer versions offer no benefits but add node-space complications. " +
                        "Please export your animations to Halo:CE format (JMA Version < 16394) and try importing again.";
                    return new TagToolError(CommandError.CustomError, errormessage);
                }

                //fixup Reach FP animations
                if (ReachFixup)
                    FixupReachFP(importer);

                //remove excess nodes and reorder to match the tag
                AdjustImportedNodes(importer);

                //set up node flags for serialization
                importer.ProcessNodeFrames((GameCacheHaloOnlineBase)CacheContext, AnimationType, FrameInfoType);

                //Check the nodes to verify that this animation can be imported to this jmad
                //if (!importer.CompareNodes(Animation.SkeletonNodes, (GameCacheHaloOnlineBase)CacheContext))
                //    return false;

                int ResourceGroupIndex = Animation.Animations[matchingindex].AnimationData.ResourceGroupIndex;
                int ResourceGroupMemberIndex = Animation.Animations[matchingindex].AnimationData.ResourceGroupMemberIndex;

                //ModelAnimationTagResource resource = CacheContext.ResourceCache.GetModelAnimationTagResource(Animation.ResourceGroups[ResourceGroupIndex].ResourceReference);
                //ModelAnimationTagResource.GroupMember membercopy = resource.GroupMembers[ResourceGroupMemberIndex].DeepClone();

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
                Animation.Animations[matchingindex].AnimationData.NodeListChecksum = (int)(importer.CalculateNodeListChecksum(0));
                Animation.Animations[matchingindex].AnimationData.FrameCount = (short)importer.frameCount;
                Animation.Animations[matchingindex].AnimationData.NodeCount = (sbyte)importer.AnimationNodes.Count;
                Animation.Animations[matchingindex].AnimationData.ResourceGroupIndex = (short)(Animation.ResourceGroups.Count - 1);
                Animation.Animations[matchingindex].AnimationData.ResourceGroupMemberIndex = 0;

                Console.WriteLine($"Replaced {file_name} successfully!");

                /*
                //write the resource data to a stream and then replace the existing resource in the cache
                using (var definitionStream = new MemoryStream())
                using (var dataStream = new MemoryStream())
                using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.LittleEndian))
                using (var dataWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
                {
                    var pageableResource = Animation.ResourceGroups[ResourceGroupIndex].ResourceReference.HaloOnlinePageableResource;

                    var context = new ResourceDefinitionSerializationContext(dataWriter, definitionWriter, CacheAddressType.Definition);
                    var serializer = new ResourceSerializer(CacheContext.Version);
                    serializer.Serialize(context, resource);
                    //reset stream position to beginning so it can be read
                    dataStream.Position = 0;
                    CacheContext.ResourceCaches.ReplaceResource(pageableResource, dataStream);

                    var definitionData = definitionStream.ToArray();

                    // add resource definition and fixups
                    pageableResource.Resource.DefinitionData = definitionData;
                    pageableResource.Resource.FixupLocations = context.FixupLocations;
                    pageableResource.Resource.DefinitionAddress = context.MainStructOffset;
                    pageableResource.Resource.InteropLocations = context.InteropLocations;
                    Animation.ResourceGroups[ResourceGroupIndex].ResourceReference.HaloOnlinePageableResource = pageableResource;
                }
                */
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

        public void AdjustjmadNodes(string PrimaryModel, string SecondaryModel)
        {
            //remove existing secondary model nodes
            List<ModelAnimationGraph.SkeletonNode> Skelnodescopy = Animation.SkeletonNodes.DeepClone();
            foreach(var skelnode in Skelnodescopy)
            {
                if (skelnode.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.SecondaryModel))
                    Animation.SkeletonNodes.RemoveAt(Animation.SkeletonNodes.FindIndex(x => x.Name.Equals(skelnode.Name)));
            }

            //repair any node inconsistencies
            foreach(var node in Animation.SkeletonNodes)
            {
                if(node.ParentNodeIndex != -1)
                    node.ParentNodeIndex = (short)Animation.SkeletonNodes.FindIndex(x => x.Name.Equals(Skelnodescopy[node.ParentNodeIndex].Name));
                if (node.FirstChildNodeIndex != -1)
                    node.FirstChildNodeIndex = (short)Animation.SkeletonNodes.FindIndex(x => x.Name.Equals(Skelnodescopy[node.FirstChildNodeIndex].Name));
                if (node.NextSiblingNodeIndex != -1)
                    node.NextSiblingNodeIndex = (short)Animation.SkeletonNodes.FindIndex(x => x.Name.Equals(Skelnodescopy[node.NextSiblingNodeIndex].Name));
            }

            using (var CacheStream = CacheContext.OpenCacheReadWrite())
            {
                var mode_tag_ref = CacheContext.TagCacheGenHO.GetTag<RenderModel>(SecondaryModel);
                var mode_tag = CacheContext.Deserialize<RenderModel>(CacheStream, mode_tag_ref);
                var mode_nodes = mode_tag.Nodes;

                //build set of nodes for secondary model from mode nodes
                List<ModelAnimationGraph.SkeletonNode> SecondaryNodes = new List<ModelAnimationGraph.SkeletonNode>();
                foreach(var mode_node in mode_nodes)
                {
                    ModelAnimationGraph.SkeletonNode newnode = new ModelAnimationGraph.SkeletonNode
                    {
                        Name = mode_node.Name,
                        NextSiblingNodeIndex = (short)(mode_node.NextSiblingNode == -1 ? -1 : mode_node.NextSiblingNode + Animation.SkeletonNodes.Count),
                        FirstChildNodeIndex = (short)(mode_node.FirstChildNode == -1 ? -1 : mode_node.FirstChildNode + Animation.SkeletonNodes.Count),
                        ParentNodeIndex = (short)(mode_node.ParentNode == -1 ? -1 : mode_node.ParentNode + Animation.SkeletonNodes.Count),
                        ModelFlags = ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.SecondaryModel,
                        ZPosition = mode_node.DefaultTranslation.Z
                    };
                    //first node is local root
                    if (SecondaryNodes.Count == 0)
                    {
                        newnode.ModelFlags |= ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.LocalRoot;
                    }
                    SecondaryNodes.Add(newnode);
                }

                //add secondary model nodes to jmad nodes
                Animation.SkeletonNodes.AddRange(SecondaryNodes);        

                //fixup references to secondary model
                var right_hand_index = Animation.SkeletonNodes.FindIndex(x => x.Name.Equals(CacheContext.StringTable.GetStringId("r_hand")));
                var secondary_model_root_index = Animation.SkeletonNodes.FindIndex(x => x.ModelFlags.Equals(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.LocalRoot | ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.SecondaryModel));
                var r_index_low_index = Animation.SkeletonNodes.FindIndex(x => x.Name.Equals(CacheContext.StringTable.GetStringId("r_index_low")));
                Animation.SkeletonNodes[right_hand_index].FirstChildNodeIndex = (short)secondary_model_root_index;
                Animation.SkeletonNodes[secondary_model_root_index].ParentNodeIndex = (short)right_hand_index;
                Animation.SkeletonNodes[secondary_model_root_index].NextSiblingNodeIndex = (short)r_index_low_index;
            }           
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
                    newAnimationNodes.Add(new AnimationImporter.AnimationNode() {Name = nodeName, FirstChildNode = skellynode.FirstChildNodeIndex, NextSiblingNode = skellynode.NextSiblingNodeIndex, ParentNode = skellynode.ParentNodeIndex});
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
            if(Animation.Modes.Count > 0)
            {
                if (Animation.Modes[0].WeaponClass.Count > 0)
                {
                    if(Animation.Modes[0].WeaponClass[0].WeaponIk.Count > 0)
                    {
                        Animation.Modes[0].WeaponClass[0].WeaponIk[0].AttachToMarker = CacheContext.StringTable.GetStringId("left_hand_spartan_fp");
                    }
                }
            }

            /*
            List<string> BadNodes = new List<string>() { "pedestal", "aim_pitch", "aim_yaw", "l_humerus", "r_humerus", "l_radius", "r_radius", "l_handguard", "r_handguard" };           
            //var jmad_nodes = Animation.SkeletonNodes;

            //prune nodes from currently importing animation file
            List<AnimationImporter.AnimationNode> importedNodesCopy = imported_nodes.DeepClone();
            foreach (var node in importedNodesCopy)
            {
                if (BadNodes.Contains(node.Name))
                {
                    imported_nodes.RemoveAt(imported_nodes.FindIndex(x => x.Name.Equals(node.Name)));
                }
            }
            */

            /*
            //prune nodes from jmad nodes
            List<ModelAnimationGraph.SkeletonNode> skeletonNodesCopy = jmad_nodes.DeepClone();
            foreach(var skellynode in skeletonNodesCopy)
            {
                string nodename = CacheContext.StringTable.GetString(skellynode.Name);
                if (BadNodes.Contains(nodename))
                {
                    jmad_nodes.RemoveAt(jmad_nodes.FindIndex(x => x.Name.Equals(skellynode.Name)));
                }
            }
            //loop through a second time to fix node tree
            foreach (var skellynode in jmad_nodes)
            {
                if(skellynode.FirstChildNodeIndex != -1)
                    skellynode.FirstChildNodeIndex = (short)jmad_nodes.FindIndex(x => x.Name.Equals(skeletonNodesCopy[skellynode.FirstChildNodeIndex].Name));
                if (skellynode.NextSiblingNodeIndex != -1)
                    skellynode.NextSiblingNodeIndex = (short)jmad_nodes.FindIndex(x => x.Name.Equals(skeletonNodesCopy[skellynode.NextSiblingNodeIndex].Name));
                if (skellynode.ParentNodeIndex != -1)
                    skellynode.ParentNodeIndex = (short)jmad_nodes.FindIndex(x => x.Name.Equals(skeletonNodesCopy[skellynode.ParentNodeIndex].Name));
            }

            //make sure that the first node has the appropriate root flag
            jmad_nodes[0].ModelFlags |= ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.LocalRoot;

            //set left and right arm flags in tag data
            int flagcount = (int)Math.Ceiling(importer.AnimationNodes.Count / 32.0f);
            List<int> leftarmflags = new List<int>(new int[flagcount]);
            List<int> rightarmflags = new List<int>(new int[flagcount]);
            for (int nodeindex = 0; nodeindex < importer.AnimationNodes.Count; nodeindex++)
            {
                int flagset = (int)Math.Floor(nodeindex / 32.0f);
                if (importer.AnimationNodes[nodeindex].Name.Contains("l_"))
                    leftarmflags[flagset] |= 1 << (nodeindex - (flagset * 32));
                if (importer.AnimationNodes[nodeindex].Name.Contains("r_"))
                    rightarmflags[flagset] |= 1 << (nodeindex - (flagset * 32));
            }
            Animation.LeftArmNodes = new uint[8];
            Animation.RightArmNodes = new uint[8];
            for (int flagsindex = 0; flagsindex < flagcount; flagsindex++)
            {
                Animation.LeftArmNodes[flagsindex] = (uint)leftarmflags[flagsindex];
                Animation.RightArmNodes[flagsindex] = (uint)rightarmflags[flagsindex];
            }
           
            */
        }
    }
}
