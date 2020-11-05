using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Animations
{
    /// <summary>
    /// Animation extraction class to build assimp animations. May have to expose the scene so that models and animations can be merged together.
    /// </summary>
    public class AnimationExtractor
    {
        private Scene Scene;
        private readonly GameCache CacheContext;
        private readonly ModelAnimationGraph ModelAnimationGraph;
        private List<Node> Nodes;

        public AnimationExtractor(ModelAnimationGraph modelAnimationGraph, GameCache cacheContext)
        {
            ModelAnimationGraph = modelAnimationGraph;
            CacheContext = cacheContext;
            Scene = new Scene();
            Nodes = new List<Node>();
            for(int i = 0; i< ModelAnimationGraph.SkeletonNodes.Count; i++)
            {
                Nodes.Add(new Node(CacheContext.StringTable.GetString(ModelAnimationGraph.SkeletonNodes[i].Name)));
            }
        }

        public void ExtractAnimations(string animationName="*")
        {



            for(int i = 0; i< ModelAnimationGraph.Animations.Count; i++)
            {
                var tagAnimation = ModelAnimationGraph.Animations[i];
                var name = CacheContext.StringTable.GetString(tagAnimation.Name);

                if(animationName == "*" || name == animationName)
                {
                    Animation animation = new Animation
                    {
                        Name = name
                    };

                    animation.TicksPerSecond = 30;
                    animation.DurationInTicks = tagAnimation.AnimationData.FrameCount;

                    //
                    // Convert tag animation into assimp animation. A channel affects only a single node (bone)
                    //

                    //Animation data should go here

                    for(int j = 0; j< tagAnimation.AnimationData.NodeCount; j++)
                    {
                        NodeAnimationChannel channel = new NodeAnimationChannel();
                        channel.NodeName = Nodes[j].Name;

                        // parse animation data for a specific node


                    }
                    Scene.Animations.Add(animation);
                }
            }


            

            
        }

        public NodeAnimationChannel ExtractChannel(AnimationData data)
        {
            NodeAnimationChannel channel = new NodeAnimationChannel();
            channel.NodeName = "";





            return null;
        }



        public List<VectorKey> GetPositionVectorKeys(AnimationData data)
        {

            List<VectorKey> positions = new List<VectorKey>();

            /*for(int i = 0; i< data.Translations.Count; i++)
            {

            }*/
            
            return null;
        }
    }
}
