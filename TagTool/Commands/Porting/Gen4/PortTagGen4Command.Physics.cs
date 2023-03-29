using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using PhysicsModelGen4 = TagTool.Tags.Definitions.Gen4.PhysicsModel;

namespace TagTool.Commands.Porting.Gen4
{
    partial class PortTagGen4Command : Command
    {
        public PhysicsModel ConvertPhysicsModel(CachedTag tag, PhysicsModelGen4 gen4PhysicsModel)
        {
            var physicsModel = new PhysicsModel()
            {
                Flags = (PhysicsModel.PhysicsModelFlags)gen4PhysicsModel.Flags,
                Mass = gen4PhysicsModel.Mass,
                LowFrequencyDeactivationScale = gen4PhysicsModel.LowFreqDeactivationScale,
                HighFrequencyDeactivationScale = gen4PhysicsModel.HighFreqDeactivationScale,
                PositionMotors = new List<PhysicsModel.PositionMotor>(),
                PhantomTypes = new List<PhysicsModel.PhantomType>(),
                NodeEdges = new List<PhysicsModel.NodeEdge>(),
                RigidBodies = new List<PhysicsModel.RigidBody>(),
                Materials = new List<PhysicsModel.Material>(),
                Spheres = new List<PhysicsModel.Sphere>(),
                Pills = new List<PhysicsModel.Pill>(),
                Boxes = new List<PhysicsModel.Box>(),
                Triangles = new List<PhysicsModel.Triangle>(),
                Polyhedra = new List<PhysicsModel.Polyhedron>(),
                PolyhedronFourVectors = new List<PhysicsModel.PolyhedronFourVector>(),
                PolyhedronPlaneEquations = new List<PhysicsModel.PolyhedronPlaneEquation>(),
                Lists = new List<PhysicsModel.List>(),
                ListShapes = new List<PhysicsModel.ListShape>(),
                HingeConstraints = new List<PhysicsModel.HingeConstraint>(),
                RagdollConstraints = new List<PhysicsModel.RagdollConstraint>(),
                Regions = new List<PhysicsModel.Region>(),
                Nodes = new List<PhysicsModel.Node>(),
                LimitedHingeConstraints = new List<PhysicsModel.LimitedHingeConstraint>(),
                Phantoms = new List<PhysicsModel.Phantom>()
            };

            //convert position motors
            foreach (var gen4motor in gen4PhysicsModel.PositionMotors)
            {
                PhysicsModel.PositionMotor newMotor = new PhysicsModel.PositionMotor
                {
                    Name = gen4motor.Name,
                    MaximumForce = gen4motor.MaximumForce,
                    MinimumForce = gen4motor.MinimumForce,
                    Tau = gen4motor.Tau,
                    Damping = gen4motor.Damping,
                    ProportionRecoverVelocity = gen4motor.ProportionRecoverVel,
                    ConstantRecoverVelocity = gen4motor.ConsantRecoverVel,
                    InitialPosition = gen4motor.InitialPosition
                };
                //ConvertHavokShape(newPhantom, gen4phantom);
                physicsModel.PositionMotors.Add(newMotor);
            }

            //convert phantom types
            foreach (var gen4phantomtype in gen4PhysicsModel.PhantomTypes)
            {
                PhysicsModel.PhantomType newPhantomType = new PhysicsModel.PhantomType
                {
                    Flags = new PhysicsModel.PhantomTypeFlags(),
                    MinimumSize = (PhysicsModel.PhantomTypeSize)gen4phantomtype.MinimumSize,
                    MaximumSize = (PhysicsModel.PhantomTypeSize)gen4phantomtype.MaximumSize,
                    MarkerName = gen4phantomtype.MarkerName,
                    AlignmentMarkerName = gen4phantomtype.AlignmentMarkerName,
                    HookesLawE = gen4phantomtype.HookesLawE,
                    LinearDeadRadius = gen4phantomtype.LinearDeadRadius,
                    CenterAcceleration = gen4phantomtype.CenterAcc,
                    CenterMaxLevel = gen4phantomtype.CenterMaxVel,
                    AxisAcceleration = gen4phantomtype.AxisAcc,
                    AxisMaxVelocity = gen4phantomtype.AxisMaxVel,
                    DirectionAcceleration = gen4phantomtype.DirectionAcc,
                    DirectionMaxVelocity = gen4phantomtype.DirectionMaxVel,
                    AlignmentHookesLawE = gen4phantomtype.AlignmentHookesLawE,
                    AlignmentAcceleration = gen4phantomtype.AlignmentAcc,
                    AlignmentMaxVelocity = gen4phantomtype.AlignmentMaxVel
                };
                //fix up phantom type flags
                if (!Enum.TryParse(gen4phantomtype.Flags.ToString(), out newPhantomType.Flags.HaloOnline))
                    new TagToolWarning($"Some phantom type flags failed to convert!");

                physicsModel.PhantomTypes.Add(newPhantomType);
            }

            //convert phantom shapes
            foreach (var gen4phantom in gen4PhysicsModel.Phantoms)
            {
                PhysicsModel.Phantom newPhantom = new PhysicsModel.Phantom
                {
                    ShapeType = (Havok.BlamShapeType)gen4phantom.HavokShapeReferenceStruct1.ShapeType,
                    ShapeIndex = gen4phantom.HavokShapeReferenceStruct1.Shape,
                    ShapeBase = ConvertHavokShapeBaseNoRadius(gen4phantom.BvShape),
                    PhantomShape = ConvertHavokShapeBaseNoRadius(gen4phantom.PhantomShape),
                    Unknown4 = new PlatformUnsignedValue((uint)gen4phantom.ChildShapePointer)
                };
                //ConvertHavokShape(newPhantom, gen4phantom);
                physicsModel.Phantoms.Add(newPhantom);
            }

            //convert node edges
            foreach(var gen4edge in gen4PhysicsModel.NodeEdges)
            {
                PhysicsModel.NodeEdge newEdge = new PhysicsModel.NodeEdge
                {
                    NodeAGlobalMaterialIndex = gen4edge.RuntimeMaterialTypeA,
                    NodeBGlobalMaterialIndex = gen4edge.RuntimeMaterialTypeB,
                    NodeA = gen4edge.NodeA,
                    NodeB = gen4edge.NodeB,
                    Constraints = new List<PhysicsModel.NodeEdge.Constraint>(),
                    NodeAMaterial = gen4edge.NodeAMaterial,
                    NodeBMaterial = gen4edge.NodeBMaterial
                };

                //constraints
                foreach(var gen4constraint in gen4edge.Constraints)
                {
                    PhysicsModel.NodeEdge.Constraint newConstraint = new PhysicsModel.NodeEdge.Constraint
                    {
                        Type = (PhysicsModel.ConstraintType)gen4constraint.Type,
                        Index = gen4constraint.Index,
                        Flags = (PhysicsModel.NodeEdge.Constraint.ConstraintFlags)gen4constraint.Flags,
                        Friction = gen4constraint.Friction,
                        RagdollMotors = new List<PhysicsModel.NodeEdge.Constraint.RagdollMotor>(),
                        LimitedHingeMotors = new List<PhysicsModel.NodeEdge.Constraint.LimitedHingeMotor>()
                    };

                    //ragdoll motors
                    foreach(var gen4ragdoll in gen4constraint.RagdollMotors)
                    {
                        newConstraint.RagdollMotors.Add(new PhysicsModel.NodeEdge.Constraint.RagdollMotor
                        {
                            TwistMotor = new PhysicsModel.Motor
                            {
                                Type = (PhysicsModel.MotorType)gen4ragdoll.TwistMotor.MotorType,
                                Index = gen4ragdoll.TwistMotor.Index
                            },
                            ConeMotor = new PhysicsModel.Motor
                            {
                                Type = (PhysicsModel.MotorType)gen4ragdoll.ConeMotor.MotorType,
                                Index = gen4ragdoll.ConeMotor.Index
                            },
                            PlaneMotor = new PhysicsModel.Motor
                            {
                                Type = (PhysicsModel.MotorType)gen4ragdoll.PlaneMotor.MotorType,
                                Index = gen4ragdoll.PlaneMotor.Index
                            }
                        });
                    }
                    //Limited hinge motors
                    foreach(var gen4hinge in gen4constraint.LimitedHingeMotors)
                    {
                        newConstraint.LimitedHingeMotors.Add(new PhysicsModel.NodeEdge.Constraint.LimitedHingeMotor
                        {
                            Motor = new PhysicsModel.Motor
                            {
                                Type = (PhysicsModel.MotorType)gen4hinge.Motor.MotorType,
                                Index = gen4hinge.Motor.Index
                            }
                        });
                    }
                    newEdge.Constraints.Add(newConstraint);
                }
                physicsModel.NodeEdges.Add(newEdge);
            }

            //convert rigid bodies
            foreach (var gen4rig in gen4PhysicsModel.RigidBodies)
            {
                PhysicsModel.RigidBody newRig = new PhysicsModel.RigidBody
                {
                    Node = gen4rig.Node,
                    Region = gen4rig.Region,
                    Permutation = gen4rig.Permutation,
                    SerializedShapes = gen4rig.SerializedShapes,
                    BoundingSphereOffset = gen4rig.BoundingSphereOffset,
                    BoundingSphereRadius = gen4rig.BoundingSphereRadius,
                    Flags = (PhysicsModel.RigidBody.RigidBodyFlags)gen4rig.Flags,
                    MotionType = (PhysicsModel.RigidBody.MotionTypeValue)gen4rig.MotionType,
                    NoPhantomPowerAltRigidBody = gen4rig.NoPhantomPowerAlt,
                    Size = (PhysicsModel.RigidBodySize)gen4rig.Size,
                    InertiaTensorScale = gen4rig.InertiaTensorScale,
                    LinearDampening = gen4rig.LinearDamping,
                    AngularDampening = gen4rig.AngularDamping,
                    CenterOfMassOffset = gen4rig.CenterOffMassOffset,
                    ShapeType = (Havok.BlamShapeType)gen4rig.ShapeReference.ShapeType,
                    ShapeIndex = gen4rig.ShapeReference.Shape,
                    Mass = gen4rig.Mass,
                    CenterOfMass = gen4rig.CenterOfMass,
                    CenterOfMassRadius = gen4rig.HavokWCenterOfMass,
                    InertiaTensorX = gen4rig.IntertiaTensorX,
                    InertiaTensorXRadius = gen4rig.HavokWIntertiaTensorX,
                    InertiaTensorY = gen4rig.IntertiaTensorY,
                    InertiaTensorYRadius = gen4rig.HavokWIntertiaTensorY,
                    InertiaTensorZ = gen4rig.IntertiaTensorZ,
                    InertiaTensorZRadius = gen4rig.HavokWIntertiaTensorZ,
                    BoundingSpherePad = gen4rig.BoundingSpherePad
                };
                physicsModel.RigidBodies.Add(newRig);
            }

            //convert pills
            foreach (var gen4pill in gen4PhysicsModel.Pills)
            {
                PhysicsModel.Pill newPill = new PhysicsModel.Pill
                {
                    Bottom = gen4pill.Bottom,
                    BottomRadius = gen4pill.HavokWBottom,
                    Top = gen4pill.Top,
                    TopRadius = gen4pill.HavokWTop
                };
                ConvertHavokShape(newPill, gen4pill.Base);
                newPill.ShapeBase = ConvertHavokShapeBase(gen4pill.CapsuleShape);
                physicsModel.Pills.Add(newPill);
            }

            //convert spheres
            foreach (var gen4sphere in gen4PhysicsModel.Spheres)
            {
                PhysicsModel.Sphere newSphere = new PhysicsModel.Sphere
                {
                    ConvexBase = ConvertHavokShapeBase(gen4sphere.TranslateShape.Convex),
                    Translation = gen4sphere.TranslateShape.Translation,
                    TranslationRadius = gen4sphere.TranslateShape.HavokWTranslation,
                    ShapeReference = new PhysicsModel.HavokShapeReference
                    {
                        ShapeIndex = gen4sphere.TranslateShape.HavokShapeReferenceStruct1.Shape,
                        Shapetype = (TagTool.Havok.BlamShapeType)gen4sphere.TranslateShape.HavokShapeReferenceStruct1.ShapeType
                    }
                };
                ConvertHavokShape(newSphere, gen4sphere.Base);
                newSphere.ShapeBase = ConvertHavokShapeBase(gen4sphere.SphereShape);
                physicsModel.Spheres.Add(newSphere);
            }

            //convert boxes
            foreach (var gen4box in gen4PhysicsModel.Boxes)
            {
                PhysicsModel.Box newBox = new PhysicsModel.Box
                {
                    HalfExtents = gen4box.HalfExtents,
                    HalfExtentsRadius = gen4box.HavokWHalfExtents,
                    RotationI = gen4box.ConvexTransformShape.RotationI,
                    RotationIRadius = gen4box.ConvexTransformShape.HavokWRotationI,
                    RotationJ = gen4box.ConvexTransformShape.RotationJ,
                    RotationJRadius = gen4box.ConvexTransformShape.HavokWRotationJ,
                    RotationK = gen4box.ConvexTransformShape.RotationK,
                    RotationKRadius = gen4box.ConvexTransformShape.HavokWRotationK,
                    Translation = gen4box.ConvexTransformShape.Translation,
                    TranslationRadius = gen4box.ConvexTransformShape.HavokWTranslation,
                    ConvexBase = ConvertHavokShapeBase(gen4box.BoxShape)
                };
                ConvertHavokShape(newBox, gen4box.Base);
                newBox.ShapeBase = ConvertHavokShapeBase(gen4box.BoxShape);
                physicsModel.Boxes.Add(newBox);
            }

            //convert polyhedra
            foreach (var gen4poly in gen4PhysicsModel.Polyhedra)
            {
                PhysicsModel.Polyhedron newPoly = new PhysicsModel.Polyhedron
                {
                    AabbHalfExtents = gen4poly.AabbHalfExtents,
                    AabbHalfExtentsRadius = gen4poly.HavokWAabbHalfExtents,
                    AabbCenter = gen4poly.AabbCenter,
                    AabbCenterRadius = gen4poly.HavokWAabbCenter,
                    //FieldPointerSkip = gen4poly.FieldPointerSkip,
                    FourVectorsSize = gen4poly.FourVectorsSize,
                    FourVectorsCapacity = (uint)gen4poly.FourVectorsSize | 0x80000000,
                    NumVertices = gen4poly.NumVertices,
                    AnotherFieldPointerSkip = new PlatformUnsignedValue((uint)gen4poly.AnotherFieldPointerSkip),
                    PlaneEquationsSize = gen4poly.PlaneEquationsSize,
                    PlaneEquationsCapacity = (uint)gen4poly.PlaneEquationsSize | 0x80000000,
                    Connectivity = (uint)gen4poly.Connectivity
                };

                ConvertHavokShape(newPoly, gen4poly.Base);
                newPoly.ShapeBase = ConvertHavokShapeBase(gen4poly.PolyhedronShape);
                physicsModel.Polyhedra.Add(newPoly);
            }

            //convert polyhedron fourvectors
            foreach(var gen4vector in gen4PhysicsModel.PolyhedronFourVectors)
            {
                physicsModel.PolyhedronFourVectors.Add(new PhysicsModel.PolyhedronFourVector
                {
                    FourVectorsX = gen4vector.FourVectorsX,
                    FourVectorsXW = gen4vector.HavokWFourVectorsX,
                    FourVectorsY = gen4vector.FourVectorsY,
                    FourVectorsYW = gen4vector.HavokWFourVectorsY,
                    FourVectorsZ = gen4vector.FourVectorsZ,
                    FourVectorsZW = gen4vector.HavokWFourVectorsZ
                });
            }

            //convert polyhedron plane equations
            foreach (var gen4peq in gen4PhysicsModel.PolyhedronPlaneEquations)
            {
                physicsModel.PolyhedronPlaneEquations.Add(new PhysicsModel.PolyhedronPlaneEquation
                {
                    PlaneEquation = new TagTool.Common.RealPlane3d
                    {
                        Normal = gen4peq.PlaneEquations,
                        Distance = gen4peq.HavokWPlaneEquations
                    }
                });
            }

            //convert materials
            foreach (var gen4material in gen4PhysicsModel.Materials)
            {
                physicsModel.Materials.Add(new PhysicsModel.Material
                {
                    Name = gen4material.Name,
                    MaterialName = gen4material.GlobalMaterialName,
                    PhantomType = gen4material.PhantomType,
                    //this seems to be a default value
                    RuntimeCollisionGroup = byte.MaxValue
                });
            }

            //convert lists
            foreach (var gen4list in gen4PhysicsModel.Lists)
            {
                physicsModel.Lists.Add(new PhysicsModel.List
                {
                    //FieldPointerSkip = gen4list.ShapeBase.FieldPointerSkip,
                    Size = gen4list.Base.Base.Size,
                    Count = gen4list.Base.Base.Count,
                    Offset = new PlatformUnsignedValue((uint)gen4list.Base.Base.FieldPointerSkip),
                    ChildShapesSize = gen4list.ChildShapesSize,
                    ChildShapesCapacity = (uint)gen4list.ChildShapesCapacity,
                    UserData = new PlatformUnsignedValue(10), //seems to be a default value
                    AabbHalfExtents = gen4list.AabbHalfExtents,
                    AabbHalfExtentsRadius = gen4list.HavokWAabbHalfExtents,
                    AabbCenter = gen4list.AabbCenter,
                    AabbCenterRadius = gen4list.HavokWAabbCenter
                });
            }

            //convert list shapes
            foreach (var gen4listshape in gen4PhysicsModel.ListShapes)
            {
                physicsModel.ListShapes.Add(new PhysicsModel.ListShape
                {
                    CollisionFilter = (uint)gen4listshape.CollisionFilter,
                    ShapeSize = gen4listshape.ShapeSize,
                    NumChildShapes = (uint)gen4listshape.NumChildShapes,
                    Shape = new Havok.HavokShapeReference
                    {
                        Index = gen4listshape.ShapeReference.Shape,
                        Type = (TagTool.Havok.BlamShapeType)gen4listshape.ShapeReference.ShapeType
                    }
                });
            }

            //convert regions
            foreach (var gen4region in gen4PhysicsModel.Regions)
            {
                var newRegion = new PhysicsModel.Region()
                {
                    Name = gen4region.Name,
                    Permutations = new List<PhysicsModel.Region.Permutation>()
                };
                //region permutations
                foreach (var gen4perm in gen4region.Permutations)
                {
                    var newPerm = new PhysicsModel.Region.Permutation
                    {
                        Name = gen4perm.Name,
                        RigidBodies = new List<PhysicsModel.Region.Permutation.RigidBody>()
                    };
                    //permutation rigidbodies
                    foreach (var gen4rigid in gen4perm.RigidBodies)
                    {
                        newPerm.RigidBodies.Add(new PhysicsModel.Region.Permutation.RigidBody
                        {
                            RigidBodyIndex = gen4rigid.RigidBody
                        });
                    }
                    newRegion.Permutations.Add(newPerm);
                }
                physicsModel.Regions.Add(newRegion);
            }

            //convert nodes
            foreach (var gen4node in gen4PhysicsModel.Nodes)
            {
                physicsModel.Nodes.Add(new PhysicsModel.Node
                {
                    Name = gen4node.Name,
                    Flags = (ushort)gen4node.Flags,
                    Parent = gen4node.Parent,
                    Sibling = gen4node.Sibling,
                    Child = gen4node.Child
                });
            }

            //convert ragdoll constraints
            foreach (var gen4ragdoll in gen4PhysicsModel.RagdollConstraints)
            {
                PhysicsModel.RagdollConstraint newRagdoll = new PhysicsModel.RagdollConstraint
                {
                    Name = gen4ragdoll.ConstraintBodies.Name,
                    NodeA = gen4ragdoll.ConstraintBodies.NodeA,
                    NodeB = gen4ragdoll.ConstraintBodies.NodeB,
                    AScale = gen4ragdoll.ConstraintBodies.AScale,
                    AForward = gen4ragdoll.ConstraintBodies.AForward,
                    ALeft = gen4ragdoll.ConstraintBodies.ALeft,
                    AUp = gen4ragdoll.ConstraintBodies.AUp,
                    APosition = gen4ragdoll.ConstraintBodies.APosition,
                    BScale = gen4ragdoll.ConstraintBodies.BScale,
                    BForward = gen4ragdoll.ConstraintBodies.BForward,
                    BLeft = gen4ragdoll.ConstraintBodies.BLeft,
                    BUp = gen4ragdoll.ConstraintBodies.BUp,
                    BPosition = gen4ragdoll.ConstraintBodies.BPosition,
                    EdgeIndex = gen4ragdoll.ConstraintBodies.EdgeIndex,
                    TwistRange = new TagTool.Common.Bounds<float>
                    {
                        Lower = gen4ragdoll.MinTwist,
                        Upper = gen4ragdoll.MaxTwist
                    },
                    ConeRange = new TagTool.Common.Bounds<float>
                    {
                        Lower = gen4ragdoll.MinCone,
                        Upper = gen4ragdoll.MaxCone
                    },
                    PlaneRange = new TagTool.Common.Bounds<float>
                    {
                        Lower = gen4ragdoll.MinPlane,
                        Upper = gen4ragdoll.MaxPlane
                    },
                    MaxFrictionTorque = gen4ragdoll.MaxFricitonTorque
                };
                physicsModel.RagdollConstraints.Add(newRagdoll);
            }

            //convert limited hinge constraints
            foreach (var gen4hinge in gen4PhysicsModel.LimitedHingeConstraints)
            {
                PhysicsModel.LimitedHingeConstraint newHinge = new PhysicsModel.LimitedHingeConstraint
                {
                    Name = gen4hinge.ConstraintBodies.Name,
                    NodeA = gen4hinge.ConstraintBodies.NodeA,
                    NodeB = gen4hinge.ConstraintBodies.NodeB,
                    AScale = gen4hinge.ConstraintBodies.AScale,
                    AForward = gen4hinge.ConstraintBodies.AForward,
                    ALeft = gen4hinge.ConstraintBodies.ALeft,
                    AUp = gen4hinge.ConstraintBodies.AUp,
                    APosition = gen4hinge.ConstraintBodies.APosition,
                    BScale = gen4hinge.ConstraintBodies.BScale,
                    BForward = gen4hinge.ConstraintBodies.BForward,
                    BLeft = gen4hinge.ConstraintBodies.BLeft,
                    BUp = gen4hinge.ConstraintBodies.BUp,
                    BPosition = gen4hinge.ConstraintBodies.BPosition,
                    EdgeIndex = gen4hinge.ConstraintBodies.EdgeIndex,
                    LimitFriction = gen4hinge.LimitFriction,
                    LimitAngleBounds = new TagTool.Common.Bounds<float>
                    {
                        Lower = gen4hinge.LimitMinAngle,
                        Upper = gen4hinge.LimitMaxAngle
                    }
                };
                physicsModel.LimitedHingeConstraints.Add(newHinge);
            }

            if (gen4PhysicsModel.Mopps.Count > 0)
                new TagToolWarning("Gen 4 phmo mopp porting not currently implemented!");

            return physicsModel;
        }

        public void ConvertHavokShape(PhysicsModel.Shape newShape, PhysicsModelGen4.HavokPrimitiveStruct gen4shape)
        {
            newShape.Name = gen4shape.Name;
            newShape.MaterialIndex = gen4shape.Material;
            newShape.MaterialFlags = (PhysicsModel.MaterialFlags)gen4shape.MaterialFlags;
            newShape.RelativeMassScale = gen4shape.RelativeMassScale;
            newShape.Friction = gen4shape.Friction;
            newShape.Restitution = gen4shape.Restitution;
            newShape.Volume = gen4shape.Volume;
            newShape.Mass = gen4shape.Mass;
            newShape.MassDistributionIndex = gen4shape.MassDistributionIndex;
            newShape.PhantomIndex = (sbyte)gen4shape.Phantom;
            newShape.ProxyCollisionGroup = (sbyte)gen4shape.ProxyCollisionGroup;
            return;
        }

        public PhysicsModel.HavokShapeBase ConvertHavokShapeBase(PhysicsModelGen4.HavokConvexShapeStruct gen4shapebase)
        {
            PhysicsModel.HavokShapeBase newShapeBase = new PhysicsModel.HavokShapeBase
            {
                //FieldPointerSkip = gen4shapebase.FieldPointerSkip,
                Size = gen4shapebase.Base.Size,
                Count = gen4shapebase.Base.Count,
                //Offset = gen4shapebase.Offset,
                Radius = gen4shapebase.Radius
            };
            return newShapeBase;
        }

        public PhysicsModel.HavokShapeBaseNoRadius ConvertHavokShapeBaseNoRadius(PhysicsModelGen4.HavokShapeStruct gen4shapebase)
        {
            PhysicsModel.HavokShapeBaseNoRadius newShapeBase = new PhysicsModel.HavokShapeBaseNoRadius
            {
                //FieldPointerSkip = gen4shapebase.FieldPointerSkip,
                Size = gen4shapebase.Size,
                Count = gen4shapebase.Count
                //Offset = gen4shapebase.Offset
            };
            return newShapeBase;
        }
    }
}
