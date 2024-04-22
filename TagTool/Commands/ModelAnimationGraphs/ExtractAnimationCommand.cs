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
using static TagTool.Animations.AnimationDefaultNodeHelper;
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
                  "Extract an animation to a JMA/JMM/JMO/JMR/JMW/JMZ/JMT file.",

                  "ExtractAnimation <(name in quotes)/index/all> <filepath>",

                  "Extract an animation to a JMA/JMM/JMO/JMR/JMW/JMZ/JMT file. " +
                  "\nProvide the index of the Animation block or the animation name as the first argument." +
                  "\nAlternatively, use 'all' to extract all animations in this jmad.")
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

            if (int.TryParse(index, out int i))
            {
                if (i >= 0 && i < Animation.Animations.Count())
                    AnimationIndices.Add(i);
                else
                    return new TagToolError(CommandError.CustomError, $"{i} is not a valid animation index. (count: {Animation.Animations.Count()})");
            }
            else
            {
                if (index.ToLower() == "all")
                    AnimationIndices.AddRange(Enumerable.Range(0, Animation.Animations.Count).ToList());
                else
                {
                    for (int j = 0; j < Animation.Animations.Count(); j++)
                    {
                        string name = CacheContext.StringTable.GetString(Animation.Animations[j].Name);
                        var request = index.ToLower().Replace(' ', ':');

                        if (request == name)
                            AnimationIndices.Add(j);
                    }
                    
                    if (AnimationIndices.Count() == 0)
                        return new TagToolError(CommandError.CustomError, $"Animation named {index} could not be found.");
                }                   
            }

            string directoryarg = argStack.Pop();

            Directory.CreateDirectory(directoryarg);

            Console.WriteLine($"###Extracting {AnimationIndices.Count} animation(s)...");

            List<Node> renderModelNodes = GetNodeDefaultValues(CacheContext, Animation);

            //shift reach data into h3 fields
            if (CacheContext.Version >= CacheVersion.HaloReach)
            {
                foreach(var animation in Animation.Animations)
                {
                    if (animation.AnimationDataBlock == null || animation.AnimationDataBlock.Count <= 0)
                        continue;
                    animation.AnimationData = animation.AnimationDataBlock[0];
                    var animationtypereach = animation.AnimationData.AnimationTypeReach;
                    animation.AnimationData.AnimationType = animationtypereach == ModelAnimationGraph.FrameTypeReach.None ?
                        ModelAnimationGraph.FrameType.Base : (ModelAnimationGraph.FrameType)(animationtypereach - 1);
                }
            }

            foreach (var animationindex in AnimationIndices)
            {                  
                ModelAnimationGraph.Animation animationblock = Animation.Animations[animationindex];

                if(animationblock.AnimationData == null)
                {
                    new TagToolWarning($"Animation {CacheContext.StringTable.GetString(animationblock.Name)} inherits from another jmad...skipping...");
                    continue;
                }

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
                Console.WriteLine($"Exporting \"{str}\"");
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
                resourcemember.NodeCount, CalculateNodeListChecksum(CacheContext, Animation.SkeletonNodes, 0), 
                (FrameInfoType)resourcemember.MovementDataType, staticflagssize, animatedflagssize, staticdatasize);
            using(var stream = new MemoryStream(resourcemember.AnimationData.Data))
            using(var reader = new EndianReader(stream, CacheContext.Endianness))
            {
                if (!data.Read(reader))
                    return null;
            }
            return data;
        }
        
        public ModelAnimationGraph.Animation GetBaseAnimation(string animationName)
        {
            var baseanims = Animation.Animations.Where(q => q.AnimationData != null && q.AnimationData.AnimationType == 0 && q.AnimationData.FrameInfoType == 0);
            char separatorChar = ':';
            string[] strArray = animationName.Split(separatorChar);
            string baseAnimationPrefix = strArray.First();
            if (animationName.StartsWith("s_ping"))
                baseAnimationPrefix = "combat";
            else if (animationName.Count(c => c == separatorChar) > 1)
                baseAnimationPrefix = strArray[0] + ":" + strArray[1];
            return baseanims.FirstOrDefault(q => CacheContext.StringTable.GetString(q.Name).StartsWith(baseAnimationPrefix) && CacheContext.StringTable.GetString(q.Name).Contains("idle"));
        }   
    }
}
