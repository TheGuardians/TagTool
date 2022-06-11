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

            List<Node> renderModelNodes = GetNodeDefaultValues();

            //shift reach data into h3 fields
            if (CacheContext.Version >= CacheVersion.HaloReach)
            {
                foreach(var animation in Animation.Animations)
                {
                    animation.AnimationData = animation.AnimationDataBlock[0];
                    var animationtypereach = animation.AnimationData.AnimationTypeReach;
                    animation.AnimationData.AnimationType = animationtypereach == ModelAnimationGraph.FrameTypeReach.None ?
                        ModelAnimationGraph.FrameType.Base : (ModelAnimationGraph.FrameType)(animationtypereach - 1);
                }
            }

            foreach (var animationindex in AnimationIndices)
            {                  
                ModelAnimationGraph.Animation animationblock = Animation.Animations[animationindex];
                AnimationResourceData animationData1 = BuildAnimationResourceData(animationblock);

                string str = CacheContext.StringTable.GetString(animationblock.Name).Replace(':', ' ');

                if (animationData1 == null)
                {
                    new TagToolWarning($"Failed to export {str} (invalid resource?)");
                    continue;
                }
                Animation animation = new Animation(renderModelNodes, animationData1);
                bool worldRelative = animationblock.AnimationData.InternalFlags.HasFlag(ModelAnimationGraph.Animation.InternalFlagsValue.WorldRelative);
                string animationExtension = animation.GetAnimationExtension((int)animationblock.AnimationData.AnimationType, (int)animationblock.AnimationData.FrameInfoType, worldRelative);
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
            if (resourcedata == null)
                return null;
            var resourcemember = resourcedata.GroupMembers[animationblock.AnimationData.ResourceGroupMemberIndex];
            var staticflagssize = CacheContext.Version >= CacheVersion.HaloReach ? resourcemember.PackedDataSizesReach.StaticNodeFlags : resourcemember.PackedDataSizes.StaticNodeFlags;
            var animatedflagssize = CacheContext.Version >= CacheVersion.HaloReach ? resourcemember.PackedDataSizesReach.AnimatedNodeFlags : resourcemember.PackedDataSizes.AnimatedNodeFlags;
            var staticdatasize = CacheContext.Version >= CacheVersion.HaloReach ? resourcemember.PackedDataSizesReach.StaticDataSize : resourcemember.PackedDataSizes.StaticDataSize;
            AnimationResourceData data = new AnimationResourceData(resourcemember.FrameCount, 
                resourcemember.NodeCount, CalculateNodeListChecksum(Animation.SkeletonNodes, 0), 
                (FrameInfoType)resourcemember.MovementDataType, staticflagssize, animatedflagssize, staticdatasize);
            using(var stream = new MemoryStream(resourcemember.AnimationData.Data))
            using(var reader = new EndianReader(stream, CacheContext.Endianness))
            {
                if (!data.Read(reader))
                    return null;
            }
            return data;
        }

        public List<Node> GetNodeDefaultValues()
        {
            List<Node> NodeList = new List<Node>();
            List<RenderModel.Node> PrimaryRenderModelNodes = new List<RenderModel.Node>();
            List<RenderModel.Node> SecondaryRenderModelNodes = new List<RenderModel.Node>();
            if (Animation.SkeletonNodes.Any(n => n.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.PrimaryModel)))
            {
                var primarynodes = Animation.SkeletonNodes.Where(n => n.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.PrimaryModel)).ToList();
                PrimaryRenderModelNodes = GetRenderModelNodes(primarynodes, 
                    CalculateNodeListChecksum(Animation.SkeletonNodes, 0, true));
                if(PrimaryRenderModelNodes.Count < primarynodes.Count)
                    new TagToolWarning($"Matching primary model not found! Animation may not appear properly.");
            }
            if (Animation.SkeletonNodes.Any(n => n.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.SecondaryModel)))
            {
                var secondarynodes = Animation.SkeletonNodes.Where(n => n.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.SecondaryModel)).ToList();
                SecondaryRenderModelNodes = GetRenderModelNodes(secondarynodes,
                    CalculateNodeListChecksum(Animation.SkeletonNodes, 0, false));
                if (SecondaryRenderModelNodes.Count < secondarynodes.Count)
                    new TagToolWarning($"Matching secondary model not found! Animation may not appear properly.");
            }

            foreach (var skellynode in Animation.SkeletonNodes)
            {
                RenderModel.Node matchingnode = new RenderModel.Node();
                if (skellynode.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.PrimaryModel))
                {
                    matchingnode = PrimaryRenderModelNodes.FirstOrDefault(n => n.Name == skellynode.Name);
                }
                else if (skellynode.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.SecondaryModel))
                {
                    matchingnode = SecondaryRenderModelNodes.FirstOrDefault(n => n.Name == skellynode.Name);
                }

                if (matchingnode == null)
                {
                    matchingnode = new RenderModel.Node();
                    new TagToolWarning($"No matching render model node found for {CacheContext.StringTable.GetString(skellynode.Name)}");
                }

                NodeList.Add(new Node
                {
                    Name = CacheContext.StringTable.GetString(skellynode.Name),
                    ParentNode = skellynode.ParentNodeIndex,
                    FirstChildNode = skellynode.FirstChildNodeIndex,
                    NextSiblingNode = skellynode.NextSiblingNodeIndex,
                    Translation = matchingnode.DefaultTranslation * 100.0f,
                    Rotation = new Quaternion(matchingnode.DefaultRotation.I, matchingnode.DefaultRotation.J, matchingnode.DefaultRotation.K, matchingnode.DefaultRotation.W),
                    Scale = matchingnode.DefaultScale
                });
            }
                
            return NodeList;
        }

        public List<RenderModel.Node> GetRenderModelNodes(List<ModelAnimationGraph.SkeletonNode> jmadnodes, int nodelistchecksum)
        {
            List<RenderModel.Node> Nodes = new List<RenderModel.Node>();
            using (var stream = CacheContext.OpenCacheRead())
            {
                List<StringId> jmadnodenames = jmadnodes.Select(n => n.Name).ToList();
                int bestmatchcount = 0;

                foreach (CachedTag tag in CacheContext.TagCache.NonNull())
                {
                    if (!tag.IsInGroup(new Tag("mode")))
                        continue;

                    RenderModel modetag = CacheContext.Deserialize<RenderModel>(stream, tag);

                    int currentmatchcount = 0;
                    var currentNodes = modetag.Nodes.Where(n => jmadnodenames.Contains(n.Name)).ToList();
                    currentmatchcount = currentNodes.Count;
                    if (currentmatchcount >= bestmatchcount)
                    {
                        bestmatchcount = currentmatchcount;
                        Nodes = currentNodes.DeepClone();
                        if (currentmatchcount == jmadnodes.Count &&
                            CalculateNodeListChecksum(modetag.Nodes, 0) == nodelistchecksum)
                        {
                            return Nodes;
                        }
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
        public int CalculateNodeListChecksum(List<ModelAnimationGraph.SkeletonNode> Nodes, int nodeindex, bool isPrimary, int checksum = 0)
        {
            ModelAnimationGraph.SkeletonNode node = Nodes[nodeindex];

            bool isValidNode = isPrimary ? node.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.PrimaryModel) :
                node.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.SecondaryModel);

            if (isValidNode)
            {
                checksum = (int)((checksum >> 31 | checksum << 1) & 0xFFFFFFFF);
                checksum += CalculateSingleNodeChecksum(CacheContext.StringTable.GetString(node.Name));
                checksum = (int)((checksum >> 30 | checksum << 2) & 0xFFFFFFFF);
            }

            int nextnodeindex = node.FirstChildNodeIndex;
            while (nextnodeindex != -1)
            {
                checksum = CalculateNodeListChecksum(Nodes, nextnodeindex, isPrimary, checksum);
                nextnodeindex = Nodes[nextnodeindex].NextSiblingNodeIndex;
            }

            if (isValidNode)
                checksum = (int)((checksum << 30 | checksum >> 2) & 0xFFFFFFFF);
            return checksum;
        }
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
