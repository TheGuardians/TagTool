using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using System.IO;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions.Gen4;
using TagTool.Tags;
using System.Numerics;
using TagTool.Commands.Common;
using TagTool.Animations;
using TagTool.Animations.Data;
using TagTool.Tags.Resources;
using System.Text;
using System.Threading.Tasks;
using static TagTool.Tags.Definitions.Gen4.ModelAnimationGraph;
using static TagTool.Tags.Definitions.Gen4.ModelAnimationGraph.AnimationGraphDefinitionsStruct.AnimationPoolBlockStruct.SharedModelAnimationBlock;

namespace TagTool.Commands.Gen4.ModelAnimationGraphs
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
                AnimationIndices.AddRange(Enumerable.Range(0, Animation.Definitions.Animations.Count).ToList());
            else
                AnimationIndices.Add(int.Parse(index));
            string directoryarg = argStack.Pop();

            if (!Directory.Exists(directoryarg))
                return new TagToolError(CommandError.FileNotFound);

            Console.WriteLine($"###Extracting {AnimationIndices.Count} animation(s)...");

            List<Node> renderModelNodes = GetNodeDefaultValues();
            foreach (var animationindex in AnimationIndices)
            {
                AnimationGraphDefinitionsStruct.AnimationPoolBlockStruct animationblock = Animation.Definitions.Animations[animationindex];
                AnimationResourceData animationData1 = BuildAnimationResourceData(animationblock);
                Animation animation = new Animation(renderModelNodes, animationData1);
                bool worldRelative = animationblock.SharedAnimationData[0].InternalFlags.HasFlag(InternalAnimationFlags.WorldRelative);
                string animationExtension = animation.GetAnimationExtension((int)animationblock.SharedAnimationData[0].AnimationType, (int)animationblock.SharedAnimationData[0].FrameInfoType, worldRelative);
                string str = CacheContext.StringTable.GetString(animationblock.Name).Replace(':', ' ');
                string fileName = directoryarg + "\\" + str + "." + animationExtension;
                if (animationblock.SharedAnimationData[0].AnimationType == AnimationTypeEnum.Overlay || 
                    animationblock.SharedAnimationData[0].AnimationType == AnimationTypeEnum.Replacement)
                {
                    AnimationGraphDefinitionsStruct.AnimationPoolBlockStruct baseAnimation1 = GetBaseAnimation(CacheContext.StringTable.GetString(animationblock.Name));
                    if (baseAnimation1 != null)
                    {
                        AnimationResourceData animationData2 = BuildAnimationResourceData(baseAnimation1);
                        Animation baseAnimation2 = new Animation(renderModelNodes, animationData2);
                        if (animationblock.SharedAnimationData[0].AnimationType == AnimationTypeEnum.Overlay)
                        {
                            animation.Overlay(baseAnimation2);
                            animation.InsertBaseFrame(baseAnimation2);
                        }
                        else if (animationblock.SharedAnimationData[0].AnimationType == AnimationTypeEnum.Replacement)
                            animation.Replace(baseAnimation2);
                    }
                    else if (animationblock.SharedAnimationData[0].AnimationType == AnimationTypeEnum.Overlay)
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
        public AnimationResourceData BuildAnimationResourceData(AnimationGraphDefinitionsStruct.AnimationPoolBlockStruct animationblock)
        {
            var resourceref = Animation.TagResourceGroups[animationblock.SharedAnimationData[0].ResourceGroup].TagResource;
            var resourcedata = ((GameCacheGen4)CacheContext).ResourceCacheGen4.GetModelAnimationTagResourceGen4(resourceref);
            var resourcemember = resourcedata.GroupMembers[animationblock.SharedAnimationData[0].ResourceGroupMember];
            AnimationResourceData data = new AnimationResourceData(resourcemember.FrameCount,
                resourcemember.NodeCount, CalculateNodeListChecksum(Animation.Definitions.SkeletonNodes, 0),
                (FrameInfoType)resourcemember.MovementDataType);
            using (var stream = new MemoryStream(resourcemember.AnimationData))
            using (var reader = new EndianReader(stream, CacheContext.Endianness))
            {
                data.Read(reader);
            }
            return data;
        }

        public List<Node> GetNodeDefaultValues()
        {
            List<Node> NodeList = new List<Node>();
            List<RenderModel.RenderModelNodeBlock> PrimaryRenderModelNodes = new List<RenderModel.RenderModelNodeBlock>();
            List<RenderModel.RenderModelNodeBlock> SecondaryRenderModelNodes = new List<RenderModel.RenderModelNodeBlock>();
            if (Animation.Definitions.SkeletonNodes.Any(n => n.ModelFlags.HasFlag(AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock.AnimationNodeModelFlags.PrimaryModel)))
            {
                PrimaryRenderModelNodes = GetRenderModelNodes(
                    Animation.Definitions.SkeletonNodes.Where(n => n.ModelFlags.HasFlag(AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock.AnimationNodeModelFlags.PrimaryModel)).ToList(),
                    CalculateNodeListChecksum(Animation.Definitions.SkeletonNodes, 0, true));
                if (PrimaryRenderModelNodes.Count == 0)
                    new TagToolWarning($"Matching primary model not found!");
            }
            if (Animation.Definitions.SkeletonNodes.Any(n => n.ModelFlags.HasFlag(AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock.AnimationNodeModelFlags.SecondaryModel)))
            {
                SecondaryRenderModelNodes = GetRenderModelNodes(
                    Animation.Definitions.SkeletonNodes.Where(n => n.ModelFlags.HasFlag(AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock.AnimationNodeModelFlags.SecondaryModel)).ToList(),
                    CalculateNodeListChecksum(Animation.Definitions.SkeletonNodes, 0, false));
                if (SecondaryRenderModelNodes.Count == 0)
                    new TagToolWarning($"Matching secondary model not found!");
            }

            foreach (var skellynode in Animation.Definitions.SkeletonNodes)
            {
                RenderModel.RenderModelNodeBlock matchingnode = new RenderModel.RenderModelNodeBlock();
                if (skellynode.ModelFlags.HasFlag(AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock.AnimationNodeModelFlags.PrimaryModel))
                {
                    matchingnode = PrimaryRenderModelNodes.FirstOrDefault(n => n.Name == skellynode.Name);
                }
                else if (skellynode.ModelFlags.HasFlag(AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock.AnimationNodeModelFlags.SecondaryModel))
                {
                    matchingnode = SecondaryRenderModelNodes.FirstOrDefault(n => n.Name == skellynode.Name);
                }

                if (matchingnode == null)
                    matchingnode = new RenderModel.RenderModelNodeBlock();

                NodeList.Add(new Node
                {
                    Name = CacheContext.StringTable.GetString(skellynode.Name),
                    ParentNode = skellynode.ParentNodeIndex,
                    FirstChildNode = skellynode.FirstChildNodeIndex,
                    NextSiblingNode = skellynode.NextSiblingNodeIndex,
                    Translation = matchingnode.DefaultTranslation,
                    Rotation = new Quaternion(matchingnode.DefaultRotation.I, matchingnode.DefaultRotation.J, matchingnode.DefaultRotation.K, matchingnode.DefaultRotation.W),
                    Scale = 1.0f
                });
            }

            return NodeList;
        }

        public List<RenderModel.RenderModelNodeBlock> GetRenderModelNodes(List<AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock> jmadnodes, int nodelistchecksum)
        {
            List<RenderModel.RenderModelNodeBlock> Nodes = new List<RenderModel.RenderModelNodeBlock>();
            using (var stream = CacheContext.OpenCacheRead())
            {
                foreach (CachedTag tag in CacheContext.TagCache.TagTable)
                {
                    if (!tag.IsInGroup(new Tag("mode")))
                        continue;

                    RenderModel modetag = CacheContext.Deserialize<RenderModel>(stream, tag);
                    int modenodechecksum = CalculateNodeListChecksum(modetag.Nodes, 0);
                    if (modenodechecksum == nodelistchecksum)
                    {
                        bool mismatch = false;
                        foreach (var jmadnode in jmadnodes)
                        {
                            if (!modetag.Nodes.Any(n => n.Name == jmadnode.Name))
                            {
                                mismatch = true;
                                break;
                            }
                        }
                        if (mismatch)
                            continue;
                        Nodes = modetag.Nodes.DeepClone();
                        return Nodes;
                    }
                }
            }
            return Nodes;
        }
        public AnimationGraphDefinitionsStruct.AnimationPoolBlockStruct GetBaseAnimation(string animationName)
        {
            var baseanims = Animation.Definitions.Animations.Where(q => q.SharedAnimationData[0].AnimationType == 0 && q.SharedAnimationData[0].FrameInfoType == 0);
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
        public int CalculateNodeListChecksum(List<AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock> Nodes, int nodeindex, bool isPrimary, int checksum = 0)
        {
            AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock node = Nodes[nodeindex];

            bool isValidNode = isPrimary ? node.ModelFlags.HasFlag(AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock.AnimationNodeModelFlags.PrimaryModel) :
                node.ModelFlags.HasFlag(AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock.AnimationNodeModelFlags.SecondaryModel);

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
        public int CalculateNodeListChecksum(List<AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock> Nodes, int nodeindex, int checksum = 0)
        {
            AnimationGraphDefinitionsStruct.AnimationGraphNodeBlock node = Nodes[nodeindex];

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
        public int CalculateNodeListChecksum(List<RenderModel.RenderModelNodeBlock> Nodes, int nodeindex, int checksum = 0)
        {
            RenderModel.RenderModelNodeBlock node = Nodes[nodeindex];
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
