using TagTool.Havok;
using TagTool.IO;
using TagTool.Tags.Definitions;
using System;
using System.IO;
using TagTool.Serialization;
using TagTool.Cache;
using System.Collections.Generic;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private PhysicsModel ConvertPhysicsModel(CachedTag instance, PhysicsModel phmo)
        {
            phmo.MoppData = HavokConverter.ConvertHkpMoppData(BlamCache.Version, CacheContext.Version, BlamCache.Platform, CacheContext.Platform, phmo.MoppData);

            //fix for ODST phantoms getting stuck on environment
            if (instance.Name == @"objects\vehicles\phantom\phantom" && BlamCache.Version == CacheVersion.Halo3ODST)
            {
                phmo.RigidBodies[0].Flags |= PhysicsModel.RigidBody.RigidBodyFlags.DoesNotInteractWithEnvironment;
                phmo.RigidBodies[1].Flags |= PhysicsModel.RigidBody.RigidBodyFlags.DoesNotInteractWithEnvironment;
            }

            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                foreach (var rigidbody in phmo.RigidBodies)
                {
                    string ReachValue = rigidbody.MotionType_Reach.ToString();
                    rigidbody.MotionType = (PhysicsModel.RigidBody.MotionTypeValue)Enum.Parse(typeof(PhysicsModel.RigidBody.MotionTypeValue), ReachValue);
                    rigidbody.ShapeType = rigidbody.ShapeType_Reach;
                    rigidbody.ShapeIndex = rigidbody.ShapeIndex_Reach;
                    rigidbody.Mass = rigidbody.Mass_Reach;
                }

                phmo.Mopps = new List<CMoppBvTreeShape>();
                foreach(var reachmopp in phmo.ReachMopps)
                {
                    phmo.Mopps.Add(new CMoppBvTreeShape
                    {
                        ReferencedObject = new HkpReferencedObject(),
                        Child = new HkpSingleShapeContainer
                        {
                            Shape = new HavokShapeReference
                            {
                                Type = (BlamShapeType)reachmopp.ChildshapePointer.ShapeType,
                                Index = reachmopp.ChildshapePointer.Shape
                            }
                        },
                        Scale = reachmopp.Scale,
                    });
                }
            }
            return phmo;
        }
    }
}