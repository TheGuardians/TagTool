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
                if (!importer.CompareNodes(Animation.SkeletonNodes, (GameCacheHaloOnlineBase)CacheContext))
                    return false;

                int ResourceGroupIndex = Animation.Animations[matchingindex].AnimationData.ResourceGroupIndex;
                int ResourceGroupMemberIndex = Animation.Animations[matchingindex].AnimationData.ResourceGroupMemberIndex;

                ModelAnimationTagResource resource = CacheContext.ResourceCache.GetModelAnimationTagResource(Animation.ResourceGroups[ResourceGroupIndex].ResourceReference);
                resource.GroupMembers[ResourceGroupMemberIndex] = importer.SerializeAnimationData((GameCacheHaloOnlineBase)CacheContext);

                //write the resource data to a stream and then replace the existing resource in the cache
                using (var definitionStream = new MemoryStream())
                using (var dataStream = new MemoryStream())
                using (var definitionWriter = new EndianWriter(definitionStream, EndianFormat.LittleEndian))
                using (var dataWriter = new EndianWriter(dataStream, EndianFormat.LittleEndian))
                {
                    var context = new ResourceDefinitionSerializationContext(dataWriter, definitionWriter, CacheAddressType.Definition);
                    var serializer = new ResourceSerializer(CacheContext.Version);
                    serializer.Serialize(context, resource);
                    //reset stream position to beginning so it can be read
                    dataStream.Position = 0;
                    CacheContext.ResourceCaches.ReplaceResource(Animation.ResourceGroups[ResourceGroupIndex].ResourceReference.HaloOnlinePageableResource, dataStream);
                }

                //serialize animation block values
                Animation.Animations[matchingindex].AnimationData.FrameCount = (short)importer.frameCount;
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
            var imported_nodes = importer.AnimationNodes;
            if(imported_nodes[0].Name == "pedestal" &&
               imported_nodes[1].Name == "aim_pitch" &&
               imported_nodes[2].Name == "aim_yaw" &&
               imported_nodes[3].Name == "base")
            {
                importer.AnimationNodes.RemoveRange(0, 3);
                //rotate node 'base' frames x by 180 degrees
                foreach(var Frame in importer.AnimationNodes[0].Frames)
                {
                    Frame.Rotation = new RealQuaternion(-Frame.Rotation.I, Frame.Rotation.J, Frame.Rotation.K, Frame.Rotation.W);
                }
            }

            var jmad_nodes = Animation.SkeletonNodes;
            if (CacheContext.StringTable.GetString(jmad_nodes[0].Name) == "pedestal" &&
                CacheContext.StringTable.GetString(jmad_nodes[1].Name) == "aim_pitch" &&
                CacheContext.StringTable.GetString(jmad_nodes[2].Name) == "aim_yaw" &&
                CacheContext.StringTable.GetString(jmad_nodes[3].Name) == "base")
            {
                Animation.SkeletonNodes.RemoveRange(0, 3);
                Animation.SkeletonNodes[0].ModelFlags |= ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.LocalRoot;
                for (int node_index = 0; node_index < Animation.SkeletonNodes.Count; node_index++)
                {
                    if(Animation.SkeletonNodes[node_index].FirstChildNodeIndex != -1)
                        Animation.SkeletonNodes[node_index].FirstChildNodeIndex = (short)(Animation.SkeletonNodes[node_index].FirstChildNodeIndex < 3 ? 0 : Animation.SkeletonNodes[node_index].FirstChildNodeIndex - 3);
                    if(Animation.SkeletonNodes[node_index].NextSiblingNodeIndex != -1)
                        Animation.SkeletonNodes[node_index].NextSiblingNodeIndex = (short)(Animation.SkeletonNodes[node_index].NextSiblingNodeIndex < 3 ? 0 : Animation.SkeletonNodes[node_index].NextSiblingNodeIndex - 3);
                    if(Animation.SkeletonNodes[node_index].ParentNodeIndex != -1)
                        Animation.SkeletonNodes[node_index].ParentNodeIndex = (short)(Animation.SkeletonNodes[node_index].ParentNodeIndex < 3 ? 0 : Animation.SkeletonNodes[node_index].ParentNodeIndex - 3);
                }
            }
        }
    }
}
