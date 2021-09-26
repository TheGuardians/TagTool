using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using System.IO;
using System.Linq;
using TagTool.Tags.Resources;
using TagTool.Cache.HaloOnline;
using ModelAnimationGraphGen2 = TagTool.Tags.Definitions.Gen2.ModelAnimationGraph;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public ModelAnimationGraph ConvertModelAnimationGraph(CachedTag tag, ModelAnimationGraphGen2 gen2Animation)
        {
            var Animation = new ModelAnimationGraph()
            {
                ParentAnimationGraph = null,
                InheritanceFlags = (ModelAnimationGraph.AnimationInheritanceFlags)gen2Animation.Resources.InheritanceFlags,
                PrivateFlags = (ModelAnimationGraph.AnimationPrivateFlags)gen2Animation.Resources.PrivateFlags,
                AnimationCodecPack = gen2Animation.Resources.AnimationCodecPack,
                SkeletonNodes = new List<ModelAnimationGraph.SkeletonNode>(),
                SoundReferences = new List<ModelAnimationGraph.AnimationTagReference>(),
                EffectReferences = new List<ModelAnimationGraph.AnimationTagReference>(),
                BlendScreens = new List<ModelAnimationGraph.BlendScreen>(),
                Animations = new List<ModelAnimationGraph.Animation>(),
                Modes = new List<ModelAnimationGraph.Mode>(),
                VehicleSuspension = new List<ModelAnimationGraph.VehicleSuspensionBlock>(),
                ObjectOverlays = new List<ModelAnimationGraph.ObjectOverlay>(),
                InheritanceList = new List<ModelAnimationGraph.Inheritance>(),
                WeaponList = new List<ModelAnimationGraph.WeaponListBlock>(),
                LeftArmNodes = gen2Animation.RunTimeData.LeftArmNodes,
                RightArmNodes = gen2Animation.RunTimeData.RightArmNodes,
                AdditionalNodeData = new List<ModelAnimationGraph.AdditionalNodeDataBlock>(),
                ResourceGroups = new List<ModelAnimationGraph.ResourceGroup>()
            };

            //set this flag, it seems to be usually checked and is not present in H2
            Animation.PrivateFlags |= ModelAnimationGraph.AnimationPrivateFlags.ReadyForUse;

            //convert skeleton nodes
            foreach (var gen2Node in gen2Animation.Resources.SkeletonNodesAbcdcc)
                Animation.SkeletonNodes.Add(new ModelAnimationGraph.SkeletonNode()
                {
                    Name = gen2Node.Name,
                    NextSiblingNodeIndex = gen2Node.NextSiblingNodeIndex,
                    FirstChildNodeIndex = gen2Node.FirstChildNodeIndex,
                    ParentNodeIndex = gen2Node.ParentNodeIndex,
                    ModelFlags = (ModelAnimationGraph.SkeletonNode.SkeletonModelFlags)gen2Node.ModelFlags,
                    NodeJointFlags = (ModelAnimationGraph.SkeletonNode.SkeletonNodeJointFlags)gen2Node.NodeJointFlags,
                    BaseVector = gen2Node.BaseVector,
                    VectorRange = gen2Node.VectorRange,
                    ZPosition = gen2Node.ZPos
                });

            //convert sound references
            foreach(var gen2snd in gen2Animation.Resources.SoundReferencesAbcdcc)
            {
                Animation.SoundReferences.Add(new ModelAnimationGraph.AnimationTagReference
                {
                    Flags = (ModelAnimationGraph.AnimationTagReferenceFlags)gen2snd.Flags
                });
            }

            //convert effect references
            foreach (var gen2effe in gen2Animation.Resources.EffectReferencesAbcdcc)
            {
                Animation.EffectReferences.Add(new ModelAnimationGraph.AnimationTagReference
                {
                    Flags = (ModelAnimationGraph.AnimationTagReferenceFlags)gen2effe.Flags
                });
            }

            //convert BlendScreens
            foreach (var gen2blend in gen2Animation.Resources.BlendScreensAbcdcc)
                Animation.BlendScreens.Add(new ModelAnimationGraph.BlendScreen()
                {
                    Label = gen2blend.Label,
                    RightYawPerFrame = gen2blend.AimingScreen.RightYawPerFrame,
                    LeftYawPerFrame = gen2blend.AimingScreen.LeftYawPerFrame,
                    RightFrameCount = gen2blend.AimingScreen.RightFrameCount,
                    LeftFrameCount = gen2blend.AimingScreen.LeftFrameCount,
                    DownPitchPerFrame = gen2blend.AimingScreen.DownPitchPerFrame,
                    UpPitchPerFrame = gen2blend.AimingScreen.UpPitchPerFrame,
                    DownPitchFrameCount = gen2blend.AimingScreen.DownPitchFrameCount,
                    UpPitchFrameCount = gen2blend.AimingScreen.UpPitchFrameCount
                });

            //create resource struct to begin adding animation data
            ModelAnimationTagResource animationTagResource = new ModelAnimationTagResource()
            {
                GroupMembers = new TagBlock<ModelAnimationTagResource.GroupMember>()
            };
            animationTagResource.GroupMembers.AddressType = CacheAddressType.Definition;

            //convert Animations
            foreach (var gen2anim in gen2Animation.Resources.AnimationsAbcdcc)
            {
                var Sounds = new List<ModelAnimationGraph.Animation.SoundEvent>();
                foreach(var gen2snd in gen2anim.SoundEventsAbcdcc)
                {
                    Sounds.Add(new ModelAnimationGraph.Animation.SoundEvent()
                    {
                        Sound = gen2snd.Sound,
                        Frame = gen2snd.Frame,
                        MarkerName = gen2snd.MarkerName
                    });
                }

                var Effects = new List<ModelAnimationGraph.Animation.EffectEvent>();
                foreach (var gen2effe in gen2anim.EffectEventsAbcdcc)
                {
                    Effects.Add(new ModelAnimationGraph.Animation.EffectEvent()
                    {
                        Effect = gen2effe.Effect,
                        Frame = gen2effe.Frame
                    });
                }

                var Frames = new List<ModelAnimationGraph.Animation.FrameEvent>();
                foreach (var gen2frame in gen2anim.FrameEventsAbcdcc)
                    Frames.Add(new ModelAnimationGraph.Animation.FrameEvent()
                    {
                        TypeHO = (ModelAnimationGraph.Animation.FrameEventTypeHO)gen2frame.Type,
                        Frame = gen2frame.Frame
                    });

                var Spacenodes = new List<ModelAnimationGraph.Animation.ObjectSpaceParentNode>();
                foreach (var gen2spacenode in gen2anim.ObjectSpaceParentNodesAbcdcc)
                    Spacenodes.Add(new ModelAnimationGraph.Animation.ObjectSpaceParentNode()
                    {
                        NodeIndex = gen2spacenode.NodeIndex,
                        Flags = (ModelAnimationGraph.Animation.ObjectSpaceParentNodeFlags)gen2spacenode.ComponentFlags,
                        RotationX = gen2spacenode.Orientation.RotationX,
                        RotationY = gen2spacenode.Orientation.RotationY,
                        RotationZ = gen2spacenode.Orientation.RotationZ,
                        RotationW = gen2spacenode.Orientation.RotationW,
                        DefaultTranslation = gen2spacenode.Orientation.DefaultTranslation,
                        DefaultScale = gen2spacenode.Orientation.DefaultScale
                    });

                Animation.Animations.Add(new ModelAnimationGraph.Animation()
                {
                    Name = gen2anim.Name,
                    Weight = gen2anim.Weight,
                    LoopFrameIndex = gen2anim.LoopFrameIndex,
                    PlaybackFlags = (ModelAnimationGraph.Animation.PlaybackFlagsValue)gen2anim.PlaybackFlags,
                    AnimationData = new ModelAnimationGraph.Animation.SharedAnimationData()
                    {
                        BlendScreen = gen2anim.BlendScreen,
                        DesiredCompression = (ModelAnimationGraph.Animation.CompressionValue)gen2anim.DesiredCompression,
                        CurrentCompression = (ModelAnimationGraph.Animation.CompressionValue)gen2anim.CurrentCompression,
                        NodeCount = gen2anim.NodeCount,
                        FrameCount = gen2anim.FrameCount,
                        AnimationType = (ModelAnimationGraph.FrameType)gen2anim.Type,
                        FrameInfoType = (Animations.FrameInfoType)gen2anim.FrameInfoType,
                        ProductionFlags = (ModelAnimationGraph.Animation.ProductionFlagsValue)gen2anim.ProductionFlags,
                        InternalFlags = (ModelAnimationGraph.Animation.InternalFlagsValue)gen2anim.InternalFlags,
                        NodeListChecksum = gen2anim.NodeListChecksum,
                        ProductionChecksum = gen2anim.ProductionChecksum,
                        Unknown2 = 5, //not sure what these do, setting default values
                        Unknown3 = 6,
                        PreviousVariantSibling= gen2anim.PreviousVariantSibling,
                        NextVariantSibling = gen2anim.NextVariantSibling,

                        //these may need to be adjusted later, hackfix for now
                        ResourceGroupIndex = 0,
                        ResourceGroupMemberIndex = (short)Animation.Animations.Count,

                        FrameEvents = Frames,
                        ObjectSpaceParentNodes = Spacenodes,
                        EffectEvents = Effects,
                        SoundEvents = Sounds,
                        //I don't know what this is or what it does, but it seems to be usually 1.0
                        Heading = new RealVector3d(1.0f, 0.0f, 0.0f) { }
                    }
                });

                //add animation data to new animation tag resource
                animationTagResource.GroupMembers.Add(new ModelAnimationTagResource.GroupMember()
                {
                    Name = gen2anim.Name,
                    Checksum = gen2anim.ImportChecksum,
                    FrameCount = gen2anim.FrameCount,
                    NodeCount = (byte)gen2anim.NodeCount,
                    MovementDataType = (ModelAnimationTagResource.GroupMemberMovementDataType)gen2anim.Type,
                    PackedDataSizes = new ModelAnimationTagResource.GroupMember.PackedDataSizesStructBlock
                    {
                        CompressedDataSize = gen2anim.DataSizes.CompressedDataSize,
                        UncompressedDataSize = gen2anim.DataSizes.UncompressedDataSize,
                        StaticDataSize = gen2anim.DataSizes.StaticDataSize,
                        PillOffsetData = gen2anim.DataSizes.PillOffsetData,
                        MovementData = gen2anim.DataSizes.MovementData,
                        AnimatedNodeFlags = gen2anim.DataSizes.AnimatedNodeFlags,
                        StaticNodeFlags = gen2anim.DataSizes.StaticNodeFlags,
                    },
                    AnimationData = new TagData {Data = gen2anim.AnimationData, AddressType = CacheAddressType.Data }
                });
            }

            //Create and add the resource
            TagResourceReference resourceref = Cache.ResourceCache.CreateModelAnimationGraphResource(animationTagResource);
            Animation.ResourceGroups.Add(new ModelAnimationGraph.ResourceGroup
            {
                MemberCount = animationTagResource.GroupMembers.Count,
                ResourceReference = resourceref
            });

            //convert modes
            foreach (var gen2mode in gen2Animation.Content.ModesAabbcc)
            {
                var newMode = new ModelAnimationGraph.Mode()
                {
                    Name = gen2mode.Label,
                    WeaponClass = new List<ModelAnimationGraph.Mode.WeaponClassBlock>(),
                    ModeIk = new List<ModelAnimationGraph.Mode.ModeIkBlock>()
                };
                //weapon classes
                foreach (var gen2wc in gen2mode.WeaponClassAabbcc)
                {
                    var newWeaponClass = new ModelAnimationGraph.Mode.WeaponClassBlock()
                    {
                        Label = gen2wc.Label,
                        WeaponType = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock>(),
                        WeaponIk = new List<ModelAnimationGraph.Mode.ModeIkBlock>()
                    };
                    //weapon types
                    foreach (var gen2wt in gen2wc.WeaponTypeAabbcc)
                    {
                        var newWeaponType = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock()
                        {
                            Label = gen2wt.Label,
                            Set = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.AnimationSet()
                            {
                                Actions = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry>(),
                                Overlays = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry>(),
                                DeathAndDamage = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock>(),
                                Transitions = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition>()
                            }
                        };

                        //weapon type actions
                        foreach (var gen2action in gen2wt.ActionsAabbcc)
                        {
                            var newAction = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry()
                            {
                                Label = gen2action.Label,
                                GraphIndex = gen2action.Animation.GraphIndex,
                                Animation = gen2action.Animation.Animation
                            };
                            newWeaponType.Set.Actions.Add(newAction);
                        }
                        //weapon type overlays
                        foreach (var gen2overlay in gen2wt.OverlaysAabbcc)
                        {
                            var newOverlay = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Entry()
                            {
                                Label = gen2overlay.Label,
                                GraphIndex = gen2overlay.Animation.GraphIndex,
                                Animation = gen2overlay.Animation.Animation
                            };
                            newWeaponType.Set.Overlays.Add(newOverlay);
                        }
                        //weapon type deathanddamage
                        foreach (var gen2death in gen2wt.DeathAndDamageAabbcc)
                        {
                            var newDeath = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock()
                            {
                                Label = gen2death.Label,
                                Directions = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock.Direction>()
                            };
                            //deathanddamage directions
                            foreach (var gen2direction in gen2death.DirectionsAabbcc)
                            {
                                var newDirection = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock.Direction()
                                {
                                    Regions = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock.Direction.Region>()
                                };
                                //direction regions
                                foreach (var gen2region in gen2direction.RegionsAabbcc)
                                {
                                    var newRegion = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.DeathAndDamageBlock.Direction.Region()
                                    {
                                        GraphIndex = gen2region.Animation.GraphIndex,
                                        Animation = gen2region.Animation.Animation
                                    };
                                    newDirection.Regions.Add(newRegion);
                                }
                                newDeath.Directions.Add(newDirection);
                            }
                            newWeaponType.Set.DeathAndDamage.Add(newDeath);
                        }
                        //weapon type transitions
                        foreach (var gen2transition in gen2wt.TransitionsAabbcc)
                        {
                            var newTransition = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition()
                            {
                                FullName = gen2transition.FullName,
                                StateName = gen2transition.StateInfo.StateName,
                                IndexA = gen2transition.StateInfo.IndexA,
                                IndexB = gen2transition.StateInfo.IndexB,
                                Destinations = new List<ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition.Destination>()
                            };
                            //transition destinations
                            foreach (var gen2destination in gen2transition.DestinationsAabbcc)
                            {
                                var newDestination = new ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition.Destination()
                                {
                                    FullName = gen2destination.FullName,
                                    ModeName = gen2destination.Mode,
                                    StateName = gen2destination.StateInfo.StateName,
                                    FrameEventLink = (ModelAnimationGraph.Mode.WeaponClassBlock.WeaponTypeBlock.Transition.Destination.FrameEventLinkValue)gen2destination.StateInfo.FrameEventLink,
                                    IndexA = gen2destination.StateInfo.IndexA,
                                    IndexB = gen2destination.StateInfo.IndexB,
                                    GraphIndex = gen2destination.Animation.GraphIndex,
                                    Animation = gen2destination.Animation.Animation
                                };
                                newTransition.Destinations.Add(newDestination);
                            }
                            newWeaponType.Set.Transitions.Add(newTransition);
                        }
                        newWeaponClass.WeaponType.Add(newWeaponType);
                    }
                    //weapon class weapon IK
                    foreach (var gen2wik in gen2wc.WeaponIkAabbcc)
                    {
                        var newWeaponIK = new ModelAnimationGraph.Mode.ModeIkBlock()
                        {
                            Marker = gen2wik.Marker,
                            AttachToMarker = gen2wik.AttachToMarker
                        };
                        newWeaponClass.WeaponIk.Add(newWeaponIK);
                    }
                    newMode.WeaponClass.Add(newWeaponClass);
                }
                //model IKs
                foreach (var gen2mik in gen2mode.ModeIkAabbcc)
                {
                    var newModelIK = new ModelAnimationGraph.Mode.ModeIkBlock()
                    {
                        Marker = gen2mik.Marker,
                        AttachToMarker = gen2mik.AttachToMarker
                    };
                    newMode.ModeIk.Add(newModelIK);
                }
                Animation.Modes.Add(newMode);
            }

            //convert vehicle suspensions
            foreach (var gen2suspension in gen2Animation.Content.VehicleSuspensionCcaabb)
            {
                var newSuspension = new ModelAnimationGraph.VehicleSuspensionBlock()
                {
                    Label = gen2suspension.Label,
                    GraphIndex = gen2suspension.Animation.GraphIndex,
                    Animation = gen2suspension.Animation.Animation,
                    MarkerName = gen2suspension.MarkerName,
                    MassPointOffset = gen2suspension.MassPointOffset,
                    FullExtensionGroundDepth = gen2suspension.FullExtensionGroundDepth,
                    FullCompressionGroundDepth = gen2suspension.FullCompressionGroundDepth,
                    RegionName = gen2suspension.RegionName,
                    DestroyedMassPointOffset = gen2suspension.DestroyedMassPointOffset,
                    DestroyedFullExtensionGroundDepth = gen2suspension.DestroyedFullExtensionGroundDepth,
                    DestroyedFullCompressionGroundDepth = gen2suspension.DestroyedFullCompressionGroundDepth
                };
                Animation.VehicleSuspension.Add(newSuspension);
            }

            //convert object overlays
            foreach(var gen2overlay in gen2Animation.Content.ObjectOverlaysCcaabb)
            {
                var newOverlay = new ModelAnimationGraph.ObjectOverlay()
                {
                    Label = gen2overlay.Label,
                    GraphIndex = gen2overlay.Animation.GraphIndex,
                    Animation = gen2overlay.Animation.Animation,
                    FunctionControls = (ModelAnimationGraph.ObjectOverlay.FunctionControlsValue)gen2overlay.FunctionControls,
                    Function = gen2overlay.Function
                };
                Animation.ObjectOverlays.Add(newOverlay);
            }

            //convert inheritance list
            foreach(var gen2inht in gen2Animation.RunTimeData.InheritenceListBbaaaa)
            {
                var newInherit = new ModelAnimationGraph.Inheritance()
                {
                    InheritedGraph = null,
                    NodeMap = new List<ModelAnimationGraph.Inheritance.NodeMapBlock>(),
                    NodeMapFlags = new List<ModelAnimationGraph.Inheritance.NodeMapFlag>(),
                    RootZOffset = gen2inht.RootZOffset,
                    Flags = (ModelAnimationGraph.InheritanceListFlags)gen2inht.InheritanceFlags
                };
                //inheritance nodemap
                foreach(var gen2nodemap in gen2inht.NodeMap)
                {
                    newInherit.NodeMap.Add(new ModelAnimationGraph.Inheritance.NodeMapBlock()
                    {
                        LocalNode = gen2nodemap.LocalNode
                    });
                }
                //inheritance nodemap flags
                foreach (var gen2nodemapflags in gen2inht.NodeMapFlags)
                {
                    newInherit.NodeMapFlags.Add(new ModelAnimationGraph.Inheritance.NodeMapFlag()
                    {
                        LocalNodeFlags = gen2nodemapflags.LocalNodeFlags
                    });
                }
                Animation.InheritanceList.Add(newInherit);
            }

            //convert weapon list
            foreach(var gen2weaponlist in gen2Animation.RunTimeData.WeaponListBbaaaa)
            {
                Animation.WeaponList.Add(new ModelAnimationGraph.WeaponListBlock
                {
                    WeaponName = gen2weaponlist.WeaponName,
                    WeaponClass = gen2weaponlist.WeaponClass
                });
            }

            //convert additional node data
            foreach(var gen2adnode in gen2Animation.AdditionalNodeData)
            {
                Animation.AdditionalNodeData.Add(new ModelAnimationGraph.AdditionalNodeDataBlock
                {
                    NodeName = gen2adnode.NodeName,
                    DefaultRotation = gen2adnode.DefaultRotation,
                    DefaultTranslation = gen2adnode.DefaultTranslation,
                    DefaultScale = gen2adnode.DefaultScale,
                    MinimumBounds = gen2adnode.MinBounds,
                    MaximumBounds = gen2adnode.MaxBounds
                });
            }

            //Sort nodes by stringid order
            var resolver = Cache.StringTable.Resolver;
            Animation.Modes = Animation.Modes.OrderBy(a => resolver.GetSet(a.Name)).ThenBy(a => resolver.GetIndex(a.Name)).ToList();

            foreach (var mode in Animation.Modes)
            {
                mode.WeaponClass = mode.WeaponClass.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();

                foreach (var weaponClass in mode.WeaponClass)
                {
                    weaponClass.WeaponType = weaponClass.WeaponType.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();

                    foreach (var weaponType in weaponClass.WeaponType)
                    {
                        weaponType.Set.Actions = weaponType.Set.Actions.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Set.Overlays = weaponType.Set.Overlays.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Set.DeathAndDamage = weaponType.Set.DeathAndDamage.OrderBy(a => resolver.GetSet(a.Label)).ThenBy(a => resolver.GetIndex(a.Label)).ToList();
                        weaponType.Set.Transitions = weaponType.Set.Transitions.OrderBy(a => resolver.GetSet(a.FullName)).ThenBy(a => resolver.GetIndex(a.FullName)).ToList();

                        foreach (var transition in weaponType.Set.Transitions)
                            transition.Destinations = transition.Destinations.OrderBy(a => resolver.GetSet(a.FullName)).ThenBy(a => resolver.GetIndex(a.FullName)).ToList();
                    }
                }
            }

            return Animation;
        }      
    }
}
