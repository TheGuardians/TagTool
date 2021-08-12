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

        */
    }
}
