using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using System.IO;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using System.Numerics;
using TagTool.Commands.Common;
using TagTool.Animations;
using TagTool.Animations.Data;
using TagTool.Tags.Resources;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Commands.ModelAnimationGraphs
{
    public class ExtractAnimationCommand : Command
    {
        private GameCache CacheContext { get; }
        private ModelAnimationGraph Animation { get; set; }

        public ExtractAnimationCommand(GameCache cachecontext, ModelAnimationGraph animation)
            : base(false,

                  "ExtractAnimation",
                  "Extract an animation to a JMA/JMM/JMO/JMR/JMW/JMZ/JMT file",

                  "ExtractAnimation <index/all> <filepath>",

                  "Extract an animation to a JMA/JMM/JMO/JMR/JMW/JMZ/JMT file. Use the index of the animation as the first argument or 'all' to extract all animations in this jmad.")
        {
            CacheContext = cachecontext;
            Animation = animation;
        }

        public override object Execute(List<string> args)
        {
            //Arguments needed: <index> <filepath>
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            var argStack = new Stack<string>(args.AsEnumerable().Reverse());

            List<int> AnimationIndices = new List<int>();
            var index = argStack.Pop();
            if (index == "all")
                AnimationIndices.AddRange(Enumerable.Range(0, Animation.Animations.Count).ToList());
            else
                AnimationIndices.Add(int.Parse(index));
            string directoryarg = argStack.Pop();

            if (!Directory.Exists(directoryarg))
                return new TagToolError(CommandError.FileNotFound);

            Console.WriteLine($"###Extracting {AnimationIndices.Count} animation(s)...");

            foreach (var animationindex in AnimationIndices)
            {
                ModelAnimationGraph.Animation animationblock = Animation.Animations[animationindex];
                List<Node> renderModelNodes = GetNodeDefaultValues();
                AnimationResourceData animationData1 = BuildAnimationResourceData(animationblock);
                Animation animation = new Animation(renderModelNodes, animationData1);
                bool worldRelative = animationblock.AnimationData.InternalFlags.HasFlag(ModelAnimationGraph.Animation.InternalFlagsValue.WorldRelative);
                string animationExtension = animation.GetAnimationExtension(animationblock.AnimationData.AnimationType, animationblock.AnimationData.FrameInfoType, worldRelative);
                string str = CacheContext.StringTable.GetString(animationblock.Name).Replace(':', ' ');
                string fileName = directoryarg + "\\" + str + "." + animationExtension;
                if (animationblock.AnimationData.AnimationType == ModelAnimationGraph.FrameType.Overlay || animationblock.AnimationData.AnimationType == ModelAnimationGraph.FrameType.Replacement)
                {
                    ModelAnimationGraph.Animation baseAnimation1 = GetBaseAnimation(CacheContext.StringTable.GetString(animationblock.Name));
                    if (baseAnimation1 != null)
                    {
                        AnimationResourceData animationData2 = BuildAnimationResourceData(baseAnimation1);
                        Animation baseAnimation2 = new Animation(renderModelNodes, animationData2);
                        if (animationblock.AnimationData.AnimationType == ModelAnimationGraph.FrameType.Overlay)
                        {
                            animation.Overlay(baseAnimation2);
                            animation.InsertBaseFrame(baseAnimation2);
                        }
                        else if (animationblock.AnimationData.AnimationType == ModelAnimationGraph.FrameType.Replacement)
                            animation.Replace(baseAnimation2);
                    }
                    else if (animationblock.AnimationData.AnimationType == ModelAnimationGraph.FrameType.Overlay)
                    {
                        animation.Overlay();
                        animation.InsertBaseFrame();
                    }
                }
                Console.WriteLine($"Exporting {str}");
                animation.Export(fileName);
            }
            Console.WriteLine("Done!");
            return true;
        }
        public AnimationResourceData BuildAnimationResourceData(ModelAnimationGraph.Animation animationblock)
        {
            var resourceref = Animation.ResourceGroups[animationblock.AnimationData.ResourceGroupIndex].ResourceReference;
            var resourcedata = CacheContext.ResourceCache.GetModelAnimationTagResource(resourceref);
            var resourcemember = resourcedata.GroupMembers[animationblock.AnimationData.ResourceGroupMemberIndex];
            AnimationResourceData data = new AnimationResourceData
            {
                FrameCount = resourcemember.FrameCount,
                NodeCount = resourcemember.NodeCount,
                NodeListChecksum = CalculateNodeListChecksum(Animation.SkeletonNodes, 0),
                FrameInfoType = (FrameInfoType)resourcemember.MovementDataType
            };
            using(var stream = new MemoryStream(resourcemember.AnimationData.Data))
            using(var reader = new EndianReader(stream, CacheContext.Endianness))
            {
                data.Read(reader);
            }
            return data;
        }

        public List<Node> GetNodeDefaultValues()
        {
            List<Node> NodeList = new List<Node>();
            List<ModelAnimationGraph.SkeletonNode> PrimaryModelNodes = Animation.SkeletonNodes.Where(n => n.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.PrimaryModel)).ToList();
            List<ModelAnimationGraph.SkeletonNode> SecondaryModelNodes = Animation.SkeletonNodes.Where(n => n.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.SecondaryModel)).ToList();
            List<RenderModel.Node> PrimaryRenderModelNodes = new List<RenderModel.Node>();
            List<RenderModel.Node> SecondaryRenderModelNodes = new List<RenderModel.Node>();
            if (PrimaryModelNodes.Count > 0)
            {
                PrimaryRenderModelNodes = GetRenderModelNodes(CalculateNodeListChecksum(PrimaryModelNodes, 0));
            }
            if (SecondaryModelNodes.Count > 0)
            {
                SecondaryRenderModelNodes = GetRenderModelNodes(CalculateNodeListChecksum(SecondaryModelNodes, 0));
            }

            foreach (var skellynode in Animation.SkeletonNodes)
            {
                RenderModel.Node matchingnode = new RenderModel.Node();
                if (skellynode.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.PrimaryModel))
                {
                    matchingnode = PrimaryRenderModelNodes.First(n => n.Name == skellynode.Name);
                }
                else if (skellynode.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.PrimaryModel))
                {
                    matchingnode = SecondaryRenderModelNodes.First(n => n.Name == skellynode.Name);
                }
                
                NodeList.Add(new Node
                {
                    Name = CacheContext.StringTable.GetString(skellynode.Name),
                    ParentNode = skellynode.ParentNodeIndex,
                    FirstChildNode = skellynode.FirstChildNodeIndex,
                    NextSiblingNode = skellynode.NextSiblingNodeIndex,
                    Translation = matchingnode.DefaultTranslation,
                    Rotation = new Quaternion(matchingnode.DefaultRotation.I, matchingnode.DefaultRotation.J, matchingnode.DefaultRotation.K, matchingnode.DefaultRotation.W),
                    Scale = matchingnode.DefaultScale
                });
            }
                
            return NodeList;
        }

        public List<RenderModel.Node> GetRenderModelNodes(int nodelistchecksum)
        {
            List<RenderModel.Node> Nodes = new List<RenderModel.Node>();
            using (var stream = CacheContext.OpenCacheRead())
            {
                foreach (CachedTag tag in CacheContext.TagCache.TagTable)
                {
                    if (!tag.IsInGroup(new Tag("mode")))
                        continue;

                    RenderModel modetag = CacheContext.Deserialize<RenderModel>(stream, tag);
                    int modenodechecksum = CalculateNodeListChecksum(modetag.Nodes, 0);
                    if(modenodechecksum == nodelistchecksum)
                    {
                        Nodes = modetag.Nodes.DeepClone();
                        break;
                    }
                }
            }         
            return Nodes;
        }

        public ModelAnimationGraph.Animation GetBaseAnimation(string animationName)
        {
            var baseanims = Animation.Animations.Where(q => q.AnimationData.AnimationType == 0 && q.AnimationData.FrameInfoType == 0);
            char separatorChar = ':';
            string[] strArray = animationName.Split(separatorChar);
            string baseAnimationPrefix = strArray.First();
            if (animationName.StartsWith("s_ping"))
                baseAnimationPrefix = "combat";
            else if (animationName.Count(c => c == separatorChar) > 1)
                baseAnimationPrefix = strArray[0] + ":" + strArray[1];
            return baseanims.FirstOrDefault(q => CacheContext.StringTable.GetString(q.Name).StartsWith(baseAnimationPrefix) && CacheContext.StringTable.GetString(q.Name).Contains("idle"));
        }

        //this function generates a nodelist checksum identical to the official halo 1 blitzkrieg jma exporter
        //later halo games also use this same format
        public int CalculateNodeListChecksum(List<ModelAnimationGraph.SkeletonNode> Nodes, int nodeindex, int checksum = 0)
        {
            ModelAnimationGraph.SkeletonNode node = Nodes[nodeindex];
            checksum = (int)((checksum >> 31 | checksum << 1) & 0xFFFFFFFF);
            checksum += CalculateSingleNodeChecksum(CacheContext.StringTable.GetString(node.Name));
            checksum = (int)((checksum >> 30 | checksum << 2) & 0xFFFFFFFF);

            int nextnodeindex = node.FirstChildNodeIndex;
            while (nextnodeindex != -1)
            {
                checksum = CalculateNodeListChecksum(Nodes, nextnodeindex, checksum);
                nextnodeindex = Nodes[nextnodeindex].NextSiblingNodeIndex;
            }

            checksum = (int)((checksum << 30 | checksum >> 2) & 0xFFFFFFFF);
            return checksum;
        }
        public int CalculateNodeListChecksum(List<RenderModel.Node> Nodes, int nodeindex, int checksum = 0)
        {
            RenderModel.Node node = Nodes[nodeindex];
            checksum = (int)((checksum >> 31 | checksum << 1) & 0xFFFFFFFF);
            checksum += CalculateSingleNodeChecksum(CacheContext.StringTable.GetString(node.Name));
            checksum = (int)((checksum >> 30 | checksum << 2) & 0xFFFFFFFF);

            int nextnodeindex = node.FirstChildNode;
            while (nextnodeindex != -1)
            {
                checksum = CalculateNodeListChecksum(Nodes, nextnodeindex, checksum);
                nextnodeindex = Nodes[nextnodeindex].NextSiblingNode;
            }

            checksum = (int)((checksum << 30 | checksum >> 2) & 0xFFFFFFFF);
            return checksum;
        }
        public int CalculateSingleNodeChecksum(string nodename)
        {
            int checksum = 0;
            foreach (var chardata in nodename.ToArray())
            {
                checksum = (int)((checksum >> 31 | checksum << 1) & 0xFFFFFFFF);
                checksum += (byte)chardata;
            }
            return (int)(checksum & 0xFFFFFFFF);
        }

    }
}
