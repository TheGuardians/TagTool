using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Havok;
using TagTool.Tags.Definitions;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;

namespace TagTool.Geometry
{
    public class PhysicsModelBuilder
    {
        private JSONNode fileStruct;

        private PhysicsModel _phmo;

        /// <summary>
        /// Initialise the physics model to be built
        /// </summary>
        public PhysicsModelBuilder()
        {
            _phmo = new PhysicsModel
            {
                RigidBodies = new List<PhysicsModel.RigidBody>(),
                Nodes = new List<PhysicsModel.Node>()
            };

            _phmo.Materials = new List<PhysicsModel.Material>();
            var material = new PhysicsModel.Material
            {
                //the 'default' stringid
                Name = new StringId(1),
                // ??? material.Flags = -256;
                PhantomType = -1
            };

            _phmo.Materials.Add(material);
        }

        public bool ParseFromFile(string fname, bool mopps)
        {
            BlenderPhmoReader reader = new BlenderPhmoReader(fname);
            fileStruct = reader.ReadFile();
            if (fileStruct == null)
            {
                new TagToolError(CommandError.CustomError, "Could not parse file!");
                return false;
            }

            //create a rigidbody for the phmo
            var defaultRigidBody = new PhysicsModel.RigidBody
            {
                Node = -1,  //this default value prevents offset from render model when there is only one rigid body.
                Mass = 100f, //probably not important
                CenterOfMass = new RealVector3d(0.0f, 0.0f, 0.0f), //actually is important. use 0 as default.
                ShapeIndex = 0, //important
                MotionType = PhysicsModel.RigidBody.MotionTypeValue.Box // box by default.
            };

            var shapedefs = fileStruct.AsArray;

            if (shapedefs.Count < 1)
            {
                new TagToolError(CommandError.CustomError, "No shapes found!");
                return false;
            }
            else
            {
                PhysicsModel.List shapeList = new PhysicsModel.List();

                //phmo needs to use shapelist and listelements reflexives
                _phmo.Lists = new List<PhysicsModel.List>();
                _phmo.ListShapes = new List<PhysicsModel.ListShape>();

                int amountAdded = 0;

                foreach (JSONNode listelem in shapedefs)
                {
                    BlamShapeType typeAdded = AddShape(_phmo, listelem);
                    if (typeAdded == BlamShapeType.TriangleMesh)
                    {
                        new TagToolError(CommandError.CustomError, "Failed to load shape!");
                        return false;
                    }

                    var shapeElem = new PhysicsModel.ListShape
                    {
                        ShapeType = typeAdded,
                        //assumes the shape added should be at the end of the respected list.
                        ShapeIndex = (short)(GetNumberOfShapes(_phmo, typeAdded) - 1)
                    };

                    _phmo.ListShapes.Add(shapeElem);

                    if (!mopps)
                    {
                        var lastPoly = _phmo.Polyhedra[_phmo.Polyhedra.Count - 1];
                        var i = lastPoly.AabbCenter.I;
                        var j = lastPoly.AabbCenter.J;
                        var k = lastPoly.AabbCenter.K;

                        PhysicsModel.RigidBody shapeRigidBody = new PhysicsModel.RigidBody
                        {
                            Node = -1,  //this will have to be set manually.
                            Mass = lastPoly.Mass, //probably not important
                            CenterOfMass = new RealVector3d(i, j, k), //use center from corresponding polyhedron
                            ShapeType = typeAdded,
                            ShapeIndex = (short)(GetNumberOfShapes(_phmo, typeAdded) - 1), //important
                            MotionType = PhysicsModel.RigidBody.MotionTypeValue.Box // box by default.
                        };

                        _phmo.RigidBodies.Add(shapeRigidBody);
                    }

                    PhysicsModel.Node node = new PhysicsModel.Node
                    {
                        Child = -1,
                        Sibling = -1,
                        Parent = -1,
                        //the 'default' stringid
                        Name = new StringId(1)
                    };

                    _phmo.Nodes.Add(node);

                    amountAdded++;
                }

                shapeList.Count = 128;
                shapeList.ChildShapesSize = amountAdded;
                shapeList.ChildShapesCapacity = (uint)(amountAdded + 0x80000000);
                _phmo.Lists.Add(shapeList);

                Console.WriteLine("Added {0} shapes.", amountAdded);

                if (mopps)
                {
                    MemoryStream moppCodeBlockStream = BlenderPhmoMoppUtil.GenerateForBlenderPhmoJsonFile(fname);

                    defaultRigidBody.ShapeType = BlamShapeType.Mopp;
                    defaultRigidBody.ShapeIndex = 0;

                    _phmo.Mopps = new List<CMoppBvTreeShape>();

                    var moppTagblock = new CMoppBvTreeShape
                    {
                        ReferencedObject = new HkpReferencedObject { ReferenceCount = 0x80 },
                        Type = 27,
                        Child = new HkpSingleShapeContainer { Type = BlamShapeType.List, Index = 0 }
                    };

                    _phmo.MoppData = moppCodeBlockStream.ToArray();
                    _phmo.Mopps.Add(moppTagblock);
                    //_phmo.MoppCodes = new
                    _phmo.RigidBodies.Add(defaultRigidBody);
                }
            }

            return true;
        }

        /// <summary>
        /// Finds the type of the shape and adds it. Currently, only 'Polyhedron' is supported.
        /// </summary>
        /// <param name="phmo">the tag to add the shape to</param> 
        /// <param name="n">the json node from which to parse the shape description.</param>
        /// <returns>shape type added, 'Unused0' is used to represent failure.</returns>
        private BlamShapeType AddShape(PhysicsModel phmo, JSONNode n)
        {
            if (n == null)
            {
                return BlamShapeType.TriangleMesh;
            }

            //an element of the top-level JSON array is a map
            // containing values for keys: 'Type', 'Data'
            switch (n["Type"])
            {
                case "Polyhedron":
                    return AddPolyhedron(phmo, n["Data"]) ? BlamShapeType.Polyhedron : BlamShapeType.TriangleMesh;
                default:
                    return BlamShapeType.TriangleMesh;
            }
        }

        /// <summary>
        /// Gets the number of shapes in the list for the particular shape
        /// </summary>
        /// <param name="phmo">the serialized phmo from which to get the counts</param>
        /// <param name="type">the shape for which to get the count</param>
        /// <returns></returns>
        private int GetNumberOfShapes(PhysicsModel phmo, BlamShapeType type)
        {
            switch (type)
            {
                case BlamShapeType.Box:
                    return phmo.Boxes != null ? phmo.Boxes.Count : 0;
                case BlamShapeType.List:
                    return phmo.Lists != null ? phmo.Lists.Count : 0;
                case BlamShapeType.Mopp:
                    return phmo.Mopps != null ? phmo.Mopps.Count : 0;
                case BlamShapeType.Pill:
                    return phmo.Pills != null ? phmo.Pills.Count : 0;
                case BlamShapeType.Polyhedron:
                    return phmo.Polyhedra != null ? phmo.Polyhedra.Count : 0;
                case BlamShapeType.Sphere:
                    return phmo.Spheres != null ? phmo.Spheres.Count : 0;

                default:
                    return 0;
            }
        }

        private bool AddPolyhedron(PhysicsModel phmo, JSONNode n)
        {
            if (n == null)
                return false;

            //In the control flow, it could have been possible to have
            // no polyhedra up to this point.
            if (phmo.Polyhedra == null)
                phmo.Polyhedra = new List<PhysicsModel.Polyhedron>();

            int index = phmo.Polyhedra.Count;
            PhysicsModel.Polyhedron poly = new PhysicsModel.Polyhedron();

            float friction, mass, restitution;
            int polyListOffset = phmo.Polyhedra.Count; //the index this polyhedron will be put into.

            bool fault = false;
            foreach (string attr in new string[]{
                "Friction", "Mass", "Center", "Extents", "Restitution" })
            {
                if (n[attr] == null)
                {
                    fault = true;
                    Console.WriteLine("Polyhedra {0} had no \"{1}\" attribute.", polyListOffset, attr);
                }
            }

            if (fault)
                return false;

            friction = n["Friction"].AsFloat;
            mass = n["Mass"].AsFloat;
            restitution = n["Restitution"].AsFloat;

            var center = n["Center"].AsArray;
            var extents = n["Extents"].AsArray;

            int nPlanes = AddManyPlanes(phmo, n["Planes"]);

            int nFVS = AddManyFVS(phmo, n["Vertices"]);

            if (nPlanes <= 0 || nFVS <= 0)
                return false;

            //This portion of the 'Polyhedron'  tagblock becomes severly 
            // mutated at runtime. The AABB center and half-extents
            // are still there however. This field may not be 
            // correctly named however this  number is always used for it.
            poly.ShapeBase = new PhysicsModel.HavokShapeBase();
            poly.ShapeBase.Size = 0;

            poly.ShapeBase.Count = 128; // uncertain as to what this does.
            poly.ShapeBase.Offset = 32 + index * 128;

            //The axis-aligned fields are used to optimise collisions
            // with other physics objects. If they are set incorrectly,
            // other objects will pass through this object. 
            poly.AabbCenter.I = center[0].AsFloat;
            poly.AabbCenter.J = center[1].AsFloat;
            poly.AabbCenter.K = center[2].AsFloat;

            poly.AabbHalfExtents.I = extents[0].AsFloat;
            poly.AabbHalfExtents.J = extents[1].AsFloat;
            poly.AabbHalfExtents.K = extents[2].AsFloat;

            //The field 'Radius' is strange. When the byte at 0x18 of
            // the main-struct is assigned a value x > 0, the 'radius'
            // of the Polyhedron is used to repel other physics objects
            // which enter the radius. This might be used to correctly
            // repel penetrating objects. 
            //poly.Radius = __

            poly.Restitution = restitution;
            poly.Friction = friction;
            poly.Mass = mass;
            poly.RelativeMassScale = 1.0f;
            poly.PhantomIndex = 0;
            poly.PhantomIndex--;
            poly.ProxyCollisionGroup = 0;
            poly.ProxyCollisionGroup--;
            poly.FourVectorsSize = nFVS;
            poly.FourVectorsCapacity = (uint)(0x80000000 + nFVS); //
            poly.PlaneEquationsSize = nPlanes;
            poly.PlaneEquationsCapacity = (uint)(0x80000000 + nPlanes); //
            poly.RuntimeMaterialType = 0;
            //A possible improvement could be to calculate this
            poly.Volume = 0.1f;

            phmo.Polyhedra.Add(poly);



            return true;
        }

        /// <summary>
        /// Adds planes to the physics model as described by the JSON node
        /// </summary>
        /// <param name="phmo"></param>
        /// <param name="n">a node that is a list of plane equations</param>
        /// <returns>The number of plane-equation tag-blocks added</returns>
        private int AddManyPlanes(PhysicsModel phmo, JSONNode n)
        {
            if (n == null)
            {
                Console.WriteLine("could not find \"Planes\" attribute.");
                return 0;
            }

            if (phmo.PolyhedronPlaneEquations == null)
                phmo.PolyhedronPlaneEquations = new List<PhysicsModel.PolyhedronPlaneEquation>();

            foreach (JSONNode p in n.AsArray)
            {
                var p_vals = p.AsArray;
                var plane = new PhysicsModel.PolyhedronPlaneEquation
                {
                    PlaneEquation = new RealPlane3d
                    {
                        I = p_vals[0].AsFloat,
                        J = p_vals[1].AsFloat,
                        K = p_vals[2].AsFloat,
                        D = p_vals[3].AsFloat
                    }
                };

                phmo.PolyhedronPlaneEquations.Add(plane);
            }

            return n.AsArray.Count;
        }

        /// <summary>
        /// Adds Four-vertex tag-blocks to the physics model. For lists
        /// of vertices that are not multiples of four, the last vertex
        /// is copied. 
        /// </summary>
        /// <param name="phmo"></param>
        /// <param name="n">a node that is a list of vertex equations</param>
        /// <returns>the number of four-vertex tag-blocks added.</returns>
        private int AddManyFVS(PhysicsModel phmo, JSONNode n)
        {
            if (n == null)
            {
                Console.WriteLine("could not find \"Vertices\" attribute.");
                return 0;
            }

            if (phmo.PolyhedronFourVectors == null)
                phmo.PolyhedronFourVectors = new List<PhysicsModel.PolyhedronFourVector>();

            int vertCount = n.AsArray.Count;
            int numfvs = vertCount / 4 + ((vertCount % 4) > 0 ? 1 : 0);

            for (int i = 0; i < numfvs; ++i)
            {
                JSONNode v0 = (i * 4) < vertCount ? n.AsArray[i * 4] : n.AsArray[vertCount - 1];
                JSONNode v1 = (i * 4 + 1) < vertCount ? n.AsArray[i * 4 + 1] : n.AsArray[vertCount - 1];
                JSONNode v2 = (i * 4 + 2) < vertCount ? n.AsArray[i * 4 + 2] : n.AsArray[vertCount - 1];
                JSONNode v3 = (i * 4 + 3) < vertCount ? n.AsArray[i * 4 + 3] : n.AsArray[vertCount - 1];

                var fourvert = new PhysicsModel.PolyhedronFourVector();

                //The plugin had these named incorrectly, the four vectors are really
                //four vertices, with the coordinates grouped (four X's, four Y's, four Z's)
                //This is likely done for SIMD. If the number of vertices aren't a multiple
                //of four, usually the last vertex is copied several times.
                fourvert.FourVectorsX.I = v0.AsArray[0].AsFloat;
                fourvert.FourVectorsX.J = v1.AsArray[0].AsFloat;
                fourvert.FourVectorsX.K = v2.AsArray[0].AsFloat;
                fourvert.FourVectorsXRadius = v3.AsArray[0].AsFloat;
                fourvert.FourVectorsY.I = v0.AsArray[1].AsFloat;
                fourvert.FourVectorsY.J = v1.AsArray[1].AsFloat;
                fourvert.FourVectorsY.K = v2.AsArray[1].AsFloat;
                fourvert.FourVectorsYRadius = v3.AsArray[1].AsFloat;
                fourvert.FourVectorsZ.I = v0.AsArray[2].AsFloat;
                fourvert.FourVectorsZ.J = v1.AsArray[2].AsFloat;
                fourvert.FourVectorsZ.K = v2.AsArray[2].AsFloat;
                fourvert.FourVectorsZRadius = v3.AsArray[2].AsFloat;

                phmo.PolyhedronFourVectors.Add(fourvert);
            }

            return numfvs;
        }

        public PhysicsModel Build()
        {
            //A possible improvement could be to calculate this
            _phmo.Mass = 120.0f;

            return _phmo;
        }
    }
}
