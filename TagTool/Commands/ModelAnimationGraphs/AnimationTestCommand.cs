using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Animations;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.ModelAnimationGraphs
{
    class AnimationTestCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public AnimationTestCommand(HaloOnlineCacheContext cacheContext) :
            base(true,

                "AnimationTest",
                "",

                "AnimationTest <Tag>",

                "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            if (!CacheContext.TryGetTag(args[0], out var instance) || !instance.IsInGroup<ModelAnimationGraph>())
            {
                Console.WriteLine($"ERROR: Invalid model_animation_graph tag specifier: {args[0]}");
                return true;
            }

            ModelAnimationGraph definition = null;

            using (var stream = CacheContext.OpenTagCacheRead())
                definition = CacheContext.Deserialize<ModelAnimationGraph>(stream, instance);

            foreach (var resourceGroup in definition.ResourceGroups)
            {
                var resource = CacheContext.Deserialize<ModelAnimationTagResource>(resourceGroup.Resource);

                using (var stream = new MemoryStream())
                using (var reader = new EndianReader(stream))
                {
                    CacheContext.ExtractResource(resourceGroup.Resource, stream);

                    foreach (var groupMember in resource.GroupMembers)
                    {
                        reader.BaseStream.Position = groupMember.AnimationData.Address.Offset;

                        var header = CacheContext.Deserialize<AnimatedCodecHeader>(
                            new DataSerializationContext(reader));

                        if (header.CodecType == AnimationCodecType.UncompressedStatic)
                            continue;

                        //var data = new AnimationData(groupMember, header);
                        //data.Read(reader);
                    }
                }
            }

            return true;
        }
    }
}