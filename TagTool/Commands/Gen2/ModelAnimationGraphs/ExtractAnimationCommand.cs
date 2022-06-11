using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using TagTool.Animations;
using TagTool.Animations.Data;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions.Gen2;

namespace TagTool.Commands.Gen2.ModelAnimationGraphs
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
                AnimationIndices.AddRange(Enumerable.Range(0, Animation.Resources.AnimationsAbcdcc.Count).ToList());
            else
                AnimationIndices.Add(int.Parse(index));
            string directoryarg = argStack.Pop();

            if (!Directory.Exists(directoryarg))
                return new TagToolError(CommandError.FileNotFound);

            Console.WriteLine($"###Extracting {AnimationIndices.Count} animation(s)...");

            List<Node> renderModelNodes = GetNodeDefaultValues();

            //fixup for h2x, get raw resource data and place it in the animation blocks
            if (CacheContext.Version < CacheVersion.Halo2Vista)
                foreach (var animationindex in AnimationIndices)
                {
                    ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock animationblock = Animation.Resources.AnimationsAbcdcc[animationindex];
                    GameCacheGen2 gen2Cache = (GameCacheGen2)CacheContext;
                    byte[] rawdata = gen2Cache.GetCacheRawData(Animation.AnimationData[animationblock.ResourceIndex].RawDataOffset, Animation.AnimationData[animationblock.ResourceIndex].DataSize);
                    int total_animation_data_size = animationblock.DataSizes.CompressedDataSize + animationblock.DataSizes.UncompressedDataSize + animationblock.DataSizes.StaticDataSize + animationblock.DataSizes.StaticNodeFlags + animationblock.DataSizes.AnimatedNodeFlags + animationblock.DataSizes.MovementData + animationblock.DataSizes.PillOffsetData;
                    animationblock.AnimationData = new byte[total_animation_data_size];
                    Buffer.BlockCopy(rawdata, animationblock.ResourceBlockOffset, animationblock.AnimationData, 0, total_animation_data_size);
                };

            foreach (var animationindex in AnimationIndices)
            {
                ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock animationblock = Animation.Resources.AnimationsAbcdcc[animationindex];
                AnimationResourceData animationData1 = BuildAnimationResourceData(animationblock);

                string str = CacheContext.StringTable.GetString(animationblock.Name).Replace(':', ' ');

                if (animationData1 == null)
                {
                    new TagToolWarning($"Failed to export {str} (invalid resource?)");
                    continue;
                }
                Animation animation = new Animation(renderModelNodes, animationData1);
                bool worldRelative = animationblock.InternalFlags.HasFlag(ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock.InternalFlagsValue.WorldRelative);
                string animationExtension = animation.GetAnimationExtension((int)animationblock.Type, (int)animationblock.FrameInfoType, worldRelative);
                string fileName = directoryarg + "\\" + str + "." + animationExtension;
                if (animationblock.Type == ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock.TypeValue.Overlay || animationblock.Type == ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock.TypeValue.Replacement)
                {
                    ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock baseAnimation1 = GetBaseAnimation(CacheContext.StringTable.GetString(animationblock.Name));
                    if (baseAnimation1 != null)
                    {
                        AnimationResourceData animationData2 = BuildAnimationResourceData(baseAnimation1);
                        Animation baseAnimation2 = new Animation(renderModelNodes, animationData2);
                        if (animationblock.Type == ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock.TypeValue.Overlay)
                        {
                            animation.Overlay(baseAnimation2);
                            animation.InsertBaseFrame(baseAnimation2);
                        }
                        else if (animationblock.Type == ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock.TypeValue.Replacement)
                            animation.Replace(baseAnimation2);
                    }
                    else if (animationblock.Type == ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock.TypeValue.Overlay)
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
        public AnimationResourceData BuildAnimationResourceData(ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock animationblock)
        {
            //var resourceref = Animation.ResourceGroups[animationblock.AnimationData.ResourceGroupIndex].ResourceReference;
            //var resourcedata = CacheContext.ResourceCache.GetModelAnimationTagResource(resourceref);
            //if (resourcedata == null)
            //    return null;
            var staticflagssize = animationblock.DataSizes.StaticNodeFlags;
            var animatedflagssize = animationblock.DataSizes.AnimatedNodeFlags;
            var staticdatasize = animationblock.DataSizes.StaticDataSize;
            AnimationResourceData data = new AnimationResourceData(
                animationblock.FrameCount,
                animationblock.NodeCount,
                CalculateNodeListChecksum(Animation.Resources.SkeletonNodesAbcdcc, 0),
                (FrameInfoType)animationblock.FrameInfoType,
                staticflagssize,
                animatedflagssize,
                staticdatasize);

            using (var stream = new MemoryStream(animationblock.AnimationData))
            using (var reader = new EndianReader(stream, CacheContext.Endianness))
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
            if (Animation.Resources.SkeletonNodesAbcdcc.Any(n => n.ModelFlags.HasFlag(ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock.ModelFlagsValue.PrimaryModel)))
            {
                var primarynodes = Animation.Resources.SkeletonNodesAbcdcc.Where(n => n.ModelFlags.HasFlag(ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock.ModelFlagsValue.PrimaryModel)).ToList();
                PrimaryRenderModelNodes = GetRenderModelNodes(primarynodes,
                    CalculateNodeListChecksum(Animation.Resources.SkeletonNodesAbcdcc, 0, true));
                if (PrimaryRenderModelNodes.Count < primarynodes.Count)
                    new TagToolWarning($"Matching primary model not found! Animation may not appear properly.");
            }
            if (Animation.Resources.SkeletonNodesAbcdcc.Any(n => n.ModelFlags.HasFlag(ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock.ModelFlagsValue.SecondaryModel)))
            {
                var secondarynodes = Animation.Resources.SkeletonNodesAbcdcc.Where(n => n.ModelFlags.HasFlag(ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock.ModelFlagsValue.SecondaryModel)).ToList();
                SecondaryRenderModelNodes = GetRenderModelNodes(secondarynodes,
                    CalculateNodeListChecksum(Animation.Resources.SkeletonNodesAbcdcc, 0, false));
                if (SecondaryRenderModelNodes.Count < secondarynodes.Count)
                    new TagToolWarning($"Matching secondary model not found! Animation may not appear properly.");
            }

            foreach (var skellynode in Animation.Resources.SkeletonNodesAbcdcc)
            {
                RenderModel.Node matchingnode = new RenderModel.Node();
                if (skellynode.ModelFlags.HasFlag(ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock.ModelFlagsValue.PrimaryModel))
                {
                    matchingnode = PrimaryRenderModelNodes.FirstOrDefault(n => n.Name == skellynode.Name);
                }
                else if (skellynode.ModelFlags.HasFlag(ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock.ModelFlagsValue.SecondaryModel))
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

        public List<RenderModel.Node> GetRenderModelNodes(List<ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock> jmadnodes, int nodelistchecksum)
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
        public ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationPoolBlock GetBaseAnimation(string animationName)
        {
            var baseanims = Animation.Resources.AnimationsAbcdcc.Where(q => q.Type == 0 && q.FrameInfoType == 0);
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
        public int CalculateNodeListChecksum(List<ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock> Nodes, int nodeindex, bool isPrimary, int checksum = 0)
        {
            ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock node = Nodes[nodeindex];

            bool isValidNode = isPrimary ? node.ModelFlags.HasFlag(ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock.ModelFlagsValue.PrimaryModel) :
                node.ModelFlags.HasFlag(ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock.ModelFlagsValue.SecondaryModel);

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
        public int CalculateNodeListChecksum(List<ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock> Nodes, int nodeindex, int checksum = 0)
        {
            ModelAnimationGraph.AnimationGraphResourcesStructBlock.AnimationGraphNodeBlock node = Nodes[nodeindex];

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
