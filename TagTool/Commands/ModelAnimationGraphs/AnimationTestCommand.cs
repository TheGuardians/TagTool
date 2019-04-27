using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
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
                
                "AnimationTest <Tag1> <Tag2>",
                
                "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 2 || args.Count > 3)
                return false;

            var destCacheContext = CacheContext;

            if (args.Count == 3)
                destCacheContext = new HaloOnlineCacheContext(new DirectoryInfo(args[2]));

            if (!CacheContext.TryGetTag(args[0], out var srcTag) || srcTag == null || !srcTag.IsInGroup<ModelAnimationGraph>())
            {
                Console.WriteLine($"ERROR: Invalid source model_animation_graph tag specifier: \"{args[0]}\".");
                return true;
            }

            if (!destCacheContext.TryGetTag(args[1], out var destTag) || destTag == null || !destTag.IsInGroup<ModelAnimationGraph>())
            {
                Console.WriteLine($"ERROR: Invalid destination model_animation_graph tag specifier: \"{args[1]}\".");
                return true;
            }

            ModelAnimationGraph srcDef = null;
            using (var tagsStream = CacheContext.OpenTagCacheRead())
                srcDef = CacheContext.Deserialize<ModelAnimationGraph>(tagsStream, srcTag);

            ModelAnimationGraph destDef = null;
            using (var tagsStream = destCacheContext.OpenTagCacheRead())
                destDef = destCacheContext.Deserialize<ModelAnimationGraph>(tagsStream, destTag);

            var destGroupMembers = new List<ModelAnimationTagResource.GroupMember>();
            var groupMemberCount = 0;

            foreach (var srcMode in srcDef.Modes)
            {
                var modeName = CacheContext.GetString(srcMode.Name);
                var destMode = destDef.Modes.Find(x => modeName == destCacheContext.GetString(x.Name));

                foreach (var srcWeaponClass in srcMode.WeaponClass)
                {
                    var weaponClassName = CacheContext.GetString(srcWeaponClass.Label);
                    var destWeaponClass = destMode.WeaponClass.Find(x => weaponClassName == destCacheContext.GetString(x.Label));

                    foreach (var srcWeaponType in srcWeaponClass.WeaponType)
                    {
                        var weaponTypeName = CacheContext.GetString(srcWeaponType.Label);
                        var destWeaponType = destWeaponClass.WeaponType.Find(x => weaponTypeName == destCacheContext.GetString(x.Label));

                        foreach (var srcAction in srcWeaponType.Actions)
                        {
                            if (srcAction.Label == StringId.Invalid || srcAction.Animation == -1)
                                continue;

                            var actionLabel = CacheContext.GetString(srcAction.Label);
                            var actionFound = false;

                            switch (actionLabel)
                            {
                                case "sprint_enter":
                                case "sprint_loop":
                                case "sprint_loop_airborne":
                                case "sprint_exit":
                                    actionFound = true;
                                    break;
                            }

                            if (!actionFound)
                                continue;

                            var srcAnimation = srcDef.Animations[srcAction.Animation];

                            // Clone a new animation block element
                            var destAnimation = srcAnimation.DeepClone();

                            destAnimation.Name = destCacheContext.GetStringId(CacheContext.GetString(srcAnimation.Name));

                            destAnimation.ResourceGroupIndex = (short)destDef.ResourceGroups.Count;
                            destAnimation.ResourceGroupMemberIndex = (short)groupMemberCount++;

                            foreach (var soundEvent in destAnimation.SoundEvents)
                                soundEvent.MarkerName = destCacheContext.GetStringId(CacheContext.GetString(soundEvent.MarkerName));

                            foreach (var effectEvent in destAnimation.EffectEvents)
                                effectEvent.MarkerName = destCacheContext.GetStringId(CacheContext.GetString(effectEvent.MarkerName));

                            destDef.Animations.Add(destAnimation);

                            // Clone a new action block element
                            var destAction = srcAction.DeepClone();
                            destAction.Label = destCacheContext.GetStringId(CacheContext.GetString(srcAction.Label));
                            destAction.Animation = (short)destDef.Animations.IndexOf(destAnimation);
                            destWeaponType.Actions.Add(destAction);

                            // Extract the resource group member
                            var srcResourceGroup = srcDef.ResourceGroups[srcAnimation.ResourceGroupIndex];
                            var srcResource = CacheContext.Deserialize<ModelAnimationTagResource>(srcResourceGroup.Resource);

                            var destGroupMember = srcResource.GroupMembers[srcAnimation.ResourceGroupIndex].DeepClone();
                            destGroupMember.Name = destCacheContext.GetStringId(CacheContext.GetString(destGroupMember.Name));

                            using (var resourceStream = new MemoryStream())
                            {
                                CacheContext.ExtractResource(srcResourceGroup.Resource, resourceStream);
                                destGroupMember.AnimationData.Data = new byte[destGroupMember.AnimationData.Size];
                                resourceStream.Position = destGroupMember.AnimationData.Address.Offset;
                                resourceStream.Read(destGroupMember.AnimationData.Data, 0, destGroupMember.AnimationData.Size);
                            }

                            destGroupMembers.Add(destGroupMember);
                        }
                    }
                }
            }

            using (var destResourceStream = new MemoryStream())
            {
                // Write the resource data for each group member
                foreach (var groupMember in destGroupMembers)
                {
                    groupMember.AnimationData.Address = new CacheAddress(CacheAddressType.Resource, (int)destResourceStream.Position);
                    destResourceStream.Write(groupMember.AnimationData.Data, 0, groupMember.AnimationData.Size);
                }

                destResourceStream.Position = 0;

                // Cache a new tag resource for the animation resource group
                var resource = new PageableResource(TagResourceTypeGen3.Animation, ResourceLocation.ResourcesB);
                destCacheContext.Serialize(resource, new ModelAnimationTagResource { GroupMembers = destGroupMembers });
                destCacheContext.AddResource(resource, destResourceStream);

                // Add a new resource group to the output model_animation_graph
                destDef.ResourceGroups.Add(new ModelAnimationGraph.ResourceGroup
                {
                    MemberCount = groupMemberCount,
                    Resource = resource
                });
            }

            // Serialize the output model_animation_graph
            using (var tagsStream = destCacheContext.OpenTagCacheReadWrite())
                destCacheContext.Serialize(tagsStream, destTag, destDef);

            return true;
        }
    }
}
