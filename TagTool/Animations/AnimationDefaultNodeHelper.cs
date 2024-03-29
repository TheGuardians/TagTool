using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TagTool.Tags.Definitions;
using System.Threading.Tasks;
using TagTool.Commands.Common;
using TagTool.Cache;
using System.Numerics;
using TagTool.Common;

namespace TagTool.Animations
{
    public static class AnimationDefaultNodeHelper
    {
        public static List<Node> GetNodeDefaultValues(GameCache CacheContext, ModelAnimationGraph Animation)
        {
            List<Node> NodeList = new List<Node>();
            List<RenderModel.Node> PrimaryRenderModelNodes = new List<RenderModel.Node>();
            List<RenderModel.Node> SecondaryRenderModelNodes = new List<RenderModel.Node>();
            if (Animation.SkeletonNodes.Any(n => n.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.PrimaryModel)))
            {
                var primarynodes = Animation.SkeletonNodes.Where(n => n.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.PrimaryModel)).ToList();
                PrimaryRenderModelNodes = GetRenderModelNodes(CacheContext, primarynodes,
                    CalculateNodeListChecksum(CacheContext, Animation.SkeletonNodes, 0, true));
                if (PrimaryRenderModelNodes.Count < primarynodes.Count)
                    new TagToolWarning($"Matching primary model not found! Animation may not appear properly.");
            }
            if (Animation.SkeletonNodes.Any(n => n.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.SecondaryModel)))
            {
                var secondarynodes = Animation.SkeletonNodes.Where(n => n.ModelFlags.HasFlag(ModelAnimationGraph.SkeletonNode.SkeletonModelFlags.SecondaryModel)).ToList();
                SecondaryRenderModelNodes = GetRenderModelNodes(CacheContext, secondarynodes,
                    CalculateNodeListChecksum(CacheContext, Animation.SkeletonNodes, 0, false));
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
                    Translation = matchingnode.DefaultTranslation,
                    Rotation = new Quaternion(matchingnode.DefaultRotation.I, matchingnode.DefaultRotation.J, matchingnode.DefaultRotation.K, matchingnode.DefaultRotation.W),
                    Scale = matchingnode.DefaultScale
                });
            }

            return NodeList;
        }

        public static List<RenderModel.Node> GetRenderModelNodes(GameCache CacheContext, List<ModelAnimationGraph.SkeletonNode> jmadnodes, int nodelistchecksum)
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
                            CalculateNodeListChecksum(CacheContext, modetag.Nodes, 0) == nodelistchecksum)
                        {
                            return Nodes;
                        }
                    }
                }
            }
            return Nodes;
        }

        //this function generates a nodelist checksum identical to the official halo 1 blitzkrieg jma exporter
        //later halo games also use this same format
        public static int CalculateNodeListChecksum(GameCache CacheContext, List<ModelAnimationGraph.SkeletonNode> Nodes, int nodeindex, bool isPrimary, int checksum = 0)
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
                checksum = CalculateNodeListChecksum(CacheContext, Nodes, nextnodeindex, isPrimary, checksum);
                nextnodeindex = Nodes[nextnodeindex].NextSiblingNodeIndex;
            }

            if (isValidNode)
                checksum = (int)((checksum << 30 | checksum >> 2) & 0xFFFFFFFF);
            return checksum;
        }
        public static int CalculateNodeListChecksum(GameCache CacheContext, List<ModelAnimationGraph.SkeletonNode> Nodes, int nodeindex, int checksum = 0)
        {
            ModelAnimationGraph.SkeletonNode node = Nodes[nodeindex];

            checksum = (int)((checksum >> 31 | checksum << 1) & 0xFFFFFFFF);
            checksum += CalculateSingleNodeChecksum(CacheContext.StringTable.GetString(node.Name));
            checksum = (int)((checksum >> 30 | checksum << 2) & 0xFFFFFFFF);

            int nextnodeindex = node.FirstChildNodeIndex;
            while (nextnodeindex != -1)
            {
                checksum = CalculateNodeListChecksum(CacheContext, Nodes, nextnodeindex, checksum);
                nextnodeindex = Nodes[nextnodeindex].NextSiblingNodeIndex;
            }

            checksum = (int)((checksum << 30 | checksum >> 2) & 0xFFFFFFFF);
            return checksum;
        }
        public static int CalculateNodeListChecksum(GameCache CacheContext, List<RenderModel.Node> Nodes, int nodeindex, int checksum = 0)
        {
            RenderModel.Node node = Nodes[nodeindex];
            checksum = (int)((checksum >> 31 | checksum << 1) & 0xFFFFFFFF);
            checksum += CalculateSingleNodeChecksum(CacheContext.StringTable.GetString(node.Name));
            checksum = (int)((checksum >> 30 | checksum << 2) & 0xFFFFFFFF);

            int nextnodeindex = node.FirstChildNode;
            while (nextnodeindex != -1)
            {
                checksum = CalculateNodeListChecksum(CacheContext, Nodes, nextnodeindex, checksum);
                nextnodeindex = Nodes[nextnodeindex].NextSiblingNode;
            }

            checksum = (int)((checksum << 30 | checksum >> 2) & 0xFFFFFFFF);
            return checksum;
        }
        public static int CalculateSingleNodeChecksum(string nodename)
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
