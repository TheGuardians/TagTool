using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Animations;
using TagTool.Animations.Data;
using TagTool.Cache;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Animations
{
    class AnimationExtractor
    {
        /*
        public override void Reconstruct(CacheReader reader)
        {
            int frameCount = (int)animationPoolBlock.FrameCount.Value;
            int nodeCount = (int)animationPoolBlock.NodeCount.Value;
            int nodeListChecksum = this.GetNodeListChecksum(model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags.primary_model) ^ this.GetNodeListChecksum(model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags.secondary_model);
            FrameInfoType frameInfoType = (FrameInfoType)animationPoolBlock.FrameInfoType.Value;
            reader1.BaseStream.Position = (long)num;
            animationPoolBlock.Animation_Data = new AnimationData(frameCount, nodeCount, nodeListChecksum, frameInfoType);
            animationPoolBlock.Animation_Data.Read(reader1);
        }

        public override void Extract(CacheReader reader, string path)
        {
            string path1 = path + "\\animations";
            IList<model_animation_graph_group.model_animation_graph_block.animation_pool_block> elements = (this.Fields as model_animation_graph_group.model_animation_graph_block).Resources.Fields.Animationsabcdcc.Elements;
            IList<render_model_group.render_model_block.render_model_node_block> renderModelNodes = this.GetRenderModelNodes(reader);
            if (!Directory.Exists(path1))
                Directory.CreateDirectory(path1);
            foreach (model_animation_graph_group.model_animation_graph_block.animation_pool_block animationPoolBlock in (IEnumerable<model_animation_graph_group.model_animation_graph_block.animation_pool_block>)elements)
            {
                AnimationData animationData1 = animationPoolBlock.AnimationData;
                Animation animation = new Animation(renderModelNodes, animationData1);
                bool worldRelative = ((int)animationPoolBlock.InternalFlags.Value & 2) == 2;
                string animationExtension = animation.GetAnimationExtension((Blam.Assets.Animations.Type)animationPoolBlock.Type.Value, (FrameInfoType)animationPoolBlock.FrameInfoType.Value, worldRelative);
                string str = animationPoolBlock.Name.StringValue.Replace(':', ' ');
                string fileName = path1 + "\\" + str + "." + animationExtension;
                if (animationPoolBlock.Type.Value == 1 || animationPoolBlock.Type.Value == 2)
                {
                    model_animation_graph_group.model_animation_graph_block.animation_pool_block baseAnimation1 = this.GetBaseAnimation(animationPoolBlock.Name.StringValue);
                    if (baseAnimation1 != null)
                    {
                        AnimationData animationData2 = baseAnimation1.AnimationData;
                        Animation baseAnimation2 = new Animation(renderModelNodes, animationData2);
                        if (animationPoolBlock.Type.Value == 1)
                        {
                            animation.Overlay(baseAnimation2);
                            animation.InsertBaseFrame(baseAnimation2);
                        }
                        else if (animationPoolBlock.Type.Value == 2)
                            animation.Replace(baseAnimation2);
                    }
                    else if (animationPoolBlock.Type.Value == 1)
                    {
                        animation.Overlay();
                        animation.InsertBaseFrame();
                    }
                }
                animation.Export(fileName);
            }
        }

        private IList<render_model_group.render_model_block.render_model_node_block> GetRenderModelNodes(
          CacheReader reader)
        {
            IList<model_animation_graph_group.model_animation_graph_block.animation_graph_node_block> elements = (this.Fields as model_animation_graph_group.model_animation_graph_block).Resources.Fields.SkeletonNodesabcdcc.Elements;
            model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags primaryModelFlag = model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags.primary_model;
            model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags secondaryModelFlag = model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags.secondary_model;
            IList<render_model_group.render_model_block.render_model_node_block> list = this.GetRenderModel(reader, primaryModelFlag)?.Fields is render_model_group.render_model_block fields ? fields.Nodes.Elements : (IList<render_model_group.render_model_block.render_model_node_block>)null;
            if (elements.Any<model_animation_graph_group.model_animation_graph_block.animation_graph_node_block>((Func<model_animation_graph_group.model_animation_graph_block.animation_graph_node_block, bool>)(q => ((model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags)q.ModelFlags.Value & secondaryModelFlag) == secondaryModelFlag)))
            {
                IList<render_model_group.render_model_block.render_model_node_block> renderModelNodeBlockList = this.GetRenderModel(reader, model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags.secondary_model)?.Fields is render_model_group.render_model_block mode_block ? mode_block.Nodes.Elements : (IList<render_model_group.render_model_block.render_model_node_block>)null;
                list.AddRange<render_model_group.render_model_block.render_model_node_block>((IEnumerable<render_model_group.render_model_block.render_model_node_block>)renderModelNodeBlockList);
            }
            foreach (model_animation_graph_group.model_animation_graph_block.animation_graph_node_block animationGraphNodeBlock in elements.Where<model_animation_graph_group.model_animation_graph_block.animation_graph_node_block>((Func<model_animation_graph_group.model_animation_graph_block.animation_graph_node_block, bool>)(q => ((model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags)q.ModelFlags.Value & primaryModelFlag) == (model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags)0 && ((model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags)q.ModelFlags.Value & secondaryModelFlag) == (model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags)0)))
                list.Add(new render_model_group.render_model_block.render_model_node_block()
                {
                    Name = animationGraphNodeBlock.Name,
                    ParentNode = animationGraphNodeBlock.ParentNodeIndex,
                    FirstChildNode = animationGraphNodeBlock.FirstChildNodeIndex,
                    NextSiblingNode = animationGraphNodeBlock.NextSiblingNodeIndex
                });
            return list;
        }

        private render_model_group GetRenderModel(
          CacheReader reader,
          model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags flag)
        {
            int nodeListChecksum = this.GetNodeListChecksum(flag);
            foreach (Blam.Cache.CacheIndexItem cacheIndexItem in reader.Game.CacheFile.GetCacheIndexItems("mode"))
            {
                reader.Game.ReadCacheIndexItem(cacheIndexItem);
                render_model_group tagGroup = cacheIndexItem.TagGroup as render_model_group;
                if (tagGroup.GetNodeListChecksum() == nodeListChecksum)
                    return tagGroup;
            }
            return (render_model_group)null;
        }

        private model_animation_graph_group.model_animation_graph_block.animation_pool_block GetBaseAnimation(
          string animationName)
        {
            IEnumerable<model_animation_graph_group.model_animation_graph_block.animation_pool_block> source = (this.Fields as model_animation_graph_group.model_animation_graph_block).Resources.Fields.Animationsabcdcc.Elements.Where<model_animation_graph_group.model_animation_graph_block.animation_pool_block>((Func<model_animation_graph_group.model_animation_graph_block.animation_pool_block, bool>)(q => q.Type.Value == 0 && q.FrameInfoType.Value == 0));
            char separatorChar = ':';
            string[] strArray = animationName.Split(separatorChar);
            string baseAnimationPrefix = ((IEnumerable<string>)strArray).First<string>();
            if (animationName.StartsWith("s_ping"))
                baseAnimationPrefix = "combat";
            else if (animationName.Count<char>((Func<char, bool>)(c => (int)c == (int)separatorChar)) > 1)
                baseAnimationPrefix = strArray[0] + ":" + strArray[1];
            return source.FirstOrDefault<model_animation_graph_group.model_animation_graph_block.animation_pool_block>((Func<model_animation_graph_group.model_animation_graph_block.animation_pool_block, bool>)(q => q.Name.StringValue.StartsWith(baseAnimationPrefix) && q.Name.StringValue.Contains("idle")));
        }

        public int GetNodeListChecksum(
          model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags flag)
        {
            IEnumerable<model_animation_graph_group.model_animation_graph_block.animation_graph_node_block> animationGraphNodeBlocks = (this.Fields as model_animation_graph_group.model_animation_graph_block).Resources.Fields.SkeletonNodesabcdcc.Elements.Where<model_animation_graph_group.model_animation_graph_block.animation_graph_node_block>((Func<model_animation_graph_group.model_animation_graph_block.animation_graph_node_block, bool>)(q => ((model_animation_graph_group.model_animation_graph_block.animation_graph_node_block.model_flags)q.ModelFlags.Value & flag) == flag));
            int num1 = 0;
            foreach (model_animation_graph_group.model_animation_graph_block.animation_graph_node_block animationGraphNodeBlock in animationGraphNodeBlocks)
            {
                int num2 = (int)animationGraphNodeBlock.Name.Value[0] << 16 | (int)animationGraphNodeBlock.Name.Value[1];
                num1 ^= num2;
            }
            return num1;
        }
        */
    }
}
