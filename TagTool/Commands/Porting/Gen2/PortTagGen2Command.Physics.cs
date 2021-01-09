using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using PhysicsModelGen2 = TagTool.Tags.Definitions.Gen2.PhysicsModel;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        public PhysicsModel ConverPhysicsModel(CachedTag tag, PhysicsModelGen2 gen2PhysicsModel)
        {
            var physicsModel = new PhysicsModel()
            {
                Flags = (PhysicsModel.PhysicsModelFlags)gen2PhysicsModel.Flags,
                Mass = gen2PhysicsModel.Mass,
                LowFrequencyDeactivationScale = gen2PhysicsModel.LowFreqDeactivationScale,
                HighFrequencyDeactivationScale = gen2PhysicsModel.HighFreqDeactivationScale,
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
            
            //convert pills
            //TODO: FINISH CONVERSION
            foreach(var gen2pill in gen2PhysicsModel.Pills)
            {
                physicsModel.Pills.Add(new PhysicsModel.Pill
                {
                    Name = gen2pill.Name,
                    MaterialIndex = gen2pill.Material,
                    RuntimeMaterialType = (short)gen2pill.Flags,
                    RelativeMassScale = gen2pill.RelativeMassScale,
                    Restitution = gen2pill.Restitution,
                    Friction = gen2pill.Friction,
                    Volume = gen2pill.Volume,
                    Mass = gen2pill.Mass,
                    MassDistributionIndex = gen2pill.MassDistributionIndex,
                    PhantomIndex = (sbyte)gen2pill.Phantom,
                    Bottom = gen2pill.Bottom,
                    BottomRadius = gen2pill.BottomRadius,
                    Top = gen2pill.Top,
                    TopRadius = gen2pill.TopRadius
                });
            }

            //convert materials
            foreach (var gen2material in gen2PhysicsModel.Materials)
            {
                physicsModel.Materials.Add(new PhysicsModel.Material
                {
                    Name = gen2material.Name,
                    MaterialName = gen2material.GlobalMaterialName,
                    PhantomType = gen2material.PhantomType,
                    //this seems to be a default value
                    RuntimeCollisionGroup = byte.MaxValue
                });
            }

            //convert lists
            foreach (var gen2list in gen2PhysicsModel.Lists)
            {
                physicsModel.Lists.Add(new PhysicsModel.List
                {
                    Size = gen2list.Size,
                    Count = gen2list.Count,
                    ChildShapesSize = gen2list.ChildShapesSize,
                    ChildShapesCapacity = (uint)gen2list.ChildShapesCapacity
                    //TODO: Calculate Half Extents and Radius
                });
            }

            //convert polyhedronfourvectors
            foreach (var gen2vector in gen2PhysicsModel.PolyhedronFourVectors)
            {
                physicsModel.PolyhedronFourVectors.Add(new PhysicsModel.PolyhedronFourVector
                {
                    FourVectorsX = gen2vector.FourVectorsX,
                    FourVectorsXRadius = gen2vector.FourVectorsXRadius,
                    FourVectorsY = gen2vector.FourVectorsY,
                    FourVectorsYRadius = gen2vector.FourVectorsYRadius,
                    FourVectorsZ = gen2vector.FourVectorsZ,
                    FourVectorsZRadius = gen2vector.FourVectorsZRadius
                });
            }

            //convert polyhedron plane equations
            foreach (var gen2peq in gen2PhysicsModel.PolyhedronPlaneEquations)
            {
                physicsModel.PolyhedronPlaneEquations.Add(new PhysicsModel.PolyhedronPlaneEquation
                {
                    PlaneEquation = gen2peq.PlaneEquation
                });
            }

            //convert regions
            foreach (var gen2region in gen2PhysicsModel.Regions)
            {
                var newRegion = new PhysicsModel.Region()
                {
                    Name = gen2region.Name,
                    Permutations = new List<PhysicsModel.Region.Permutation>()
                };
                //region permutations
                foreach (var gen2perm in gen2region.Permutations)
                {
                    var newPerm = new PhysicsModel.Region.Permutation
                    {
                        Name = gen2perm.Name,
                        RigidBodies = new List<PhysicsModel.Region.Permutation.RigidBody>()
                    };
                    //permutation rigidbodies
                    foreach (var gen2rigid in gen2perm.RigidBodies)
                    {
                        newPerm.RigidBodies.Add(new PhysicsModel.Region.Permutation.RigidBody
                        {
                            RigidBodyIndex = gen2rigid.RigidBody
                        });
                    }
                    newRegion.Permutations.Add(newPerm);
                }
                physicsModel.Regions.Add(newRegion);
            }

            //convert nodes
            foreach(var gen2node in gen2PhysicsModel.Nodes)
            {
                physicsModel.Nodes.Add(new PhysicsModel.Node
                {
                    Name = gen2node.Name,
                    Flags = (ushort)gen2node.Flags,
                    Parent = gen2node.Parent,
                    Sibling = gen2node.Sibling,
                    Child = gen2node.Child
                });
            }

            return physicsModel;
        }
    }
}
