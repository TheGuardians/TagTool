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
    public class ReplaceAnimationCommand : Command
    {
        private GameCacheHaloOnlineBase CacheContext { get; }
        private ModelAnimationGraph Animation { get; set; }
        private ModelAnimationGraph.FrameType AnimationType = ModelAnimationGraph.FrameType.Base;
        private bool isWorldRelative { get; set; }
        private CachedTag Jmad { get; set; }
        private bool ReachFixup = false;

        public ReplaceAnimationCommand(GameCache cachecontext, ModelAnimationGraph animation, CachedTag jmad)
            : base(false,

                  "ReplaceAnimation",
                  "Replace an animation or animations in a ModelAnimationGraph tag",

                  "ReplaceAnimation [reachfix] <file or folder path>",

                  "Replace an animation or animations in a ModelAnimationGraph tag from animations in JMA/JMM/JMO/JMR/JMW/JMZ/JMT format\n" +
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
            Console.WriteLine("Enter the tagname of each render model tag that the animation(s) use(s)");
            Console.WriteLine("Enter a blank line to finish.");
            Console.WriteLine("------------------------------------------------------------------");
            for (string line; !String.IsNullOrWhiteSpace(line = Console.ReadLine());)
            {
                //remove any tag type info from tagname
                line = line.Split('.')[0];
                ModelList.Add(line);
            }

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
                    Console.WriteLine($"###ERROR: No existing animation found for animation {file_name}!");
                    continue;
                }

                //create new importer class and import the source file
                var importer = new AnimationImporter();
                importer.Import(filepath.FullName, (GameCacheHaloOnlineBase)CacheContext, ModelList, AnimationType);

                //fixup Reach FP animations
                if (ReachFixup)
                    FixupReachFP(importer);

                //Check the nodes to verify that this animation can be imported to this jmad
                //if (!importer.CompareNodes(Animation.SkeletonNodes, (GameCacheHaloOnlineBase)CacheContext))
                //    return false;

                int ResourceGroupIndex = Animation.Animations[matchingindex].AnimationData.ResourceGroupIndex;
                int ResourceGroupMemberIndex = Animation.Animations[matchingindex].AnimationData.ResourceGroupMemberIndex;

                ModelAnimationTagResource resource = CacheContext.ResourceCache.GetModelAnimationTagResource(Animation.ResourceGroups[ResourceGroupIndex].ResourceReference);
                ModelAnimationTagResource.GroupMember membercopy = resource.GroupMembers[ResourceGroupMemberIndex].DeepClone();
                resource.GroupMembers[ResourceGroupMemberIndex] = importer.SerializeAnimationData((GameCacheHaloOnlineBase)CacheContext);

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

                //serialize animation block values
                Animation.Animations[matchingindex].AnimationData.FrameCount = (short)importer.frameCount;
                Animation.Animations[matchingindex].AnimationData.NodeCount = (sbyte)importer.AnimationNodes.Count;
            }
           
            //save changes to the current tag
            CacheContext.SaveStrings();
            using (Stream cachestream = CacheContext.OpenCacheReadWrite())
            {
                CacheContext.Serialize(cachestream, Jmad, Animation);
            }

            Console.WriteLine("Animation(s) replaced successfully!");
            return true;
        }

        public void FixupReachFP(AnimationImporter importer)
        {
            List<string> BadNodes = new List<string>() { "pedestal", "aim_pitch", "aim_yaw", "l_humerus", "r_humerus", "l_radius", "r_radius", "l_handguard", "r_handguard" };

            var imported_nodes = importer.AnimationNodes;
            var jmad_nodes = Animation.SkeletonNodes;

            //prune nodes from currently importing animation file
            List<AnimationImporter.AnimationNode> importedNodesCopy = imported_nodes.DeepClone();
            foreach (var node in importedNodesCopy)
            {
                if (BadNodes.Contains(node.Name))
                {
                    imported_nodes.RemoveAt(imported_nodes.FindIndex(x => x.Name.Equals(node.Name)));
                }
            }

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

            /*
            int basenode_index = imported_nodes.FindIndex(x => x.Name.Equals("base"));
            if(basenode_index != -1)
            {
                //fixup rotated base node
                foreach (var Frame in imported_nodes[basenode_index].Frames)
                {
                    Frame.Rotation = new RealQuaternion(-Frame.Rotation.I, Frame.Rotation.J, Frame.Rotation.K, Frame.Rotation.W);
                }
            }
            */
        }
    }
}
