using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Havok;
using TagTool.Tags.Definitions;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using TagTool.Geometry.Jms;
using System.Linq;
using Newtonsoft.Json;
using TagTool.Cache;
using static TagTool.Tags.Definitions.PhysicsModel;

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
        }

        private double CalculateDistance(PhysicsModel.PolyhedronFourVector v1, PhysicsModel.PolyhedronFourVector v2)
        {
            double dx = v1.FourVectorsX.I - v2.FourVectorsX.I;
            double dy = v1.FourVectorsY.I - v2.FourVectorsY.I;
            double dz = v1.FourVectorsZ.I - v2.FourVectorsZ.I;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public bool ConvertJmsToJson(JmsFormat jms, GameCacheHaloOnlineBase cache, Stream cacheStream)
        {
            var output = new List<object>();
            var shapeToNodeIndexMap = new Dictionary<int, int>();

            // Group triangles by their material index
            var groups = jms.Triangles.GroupBy(tri => tri.MaterialIndex);

            foreach (var group in groups)
            {
                var allVertices = new List<RealPoint3d>();

                var firstTriangle = group.First();
                var firstVertexIndex = firstTriangle.VertexIndices[0];
                var firstNodeSet = jms.Vertices[firstVertexIndex].NodeSets.FirstOrDefault();
                var nodeIndex = firstNodeSet != null ? firstNodeSet.NodeIndex : -1;
                shapeToNodeIndexMap[output.Count] = nodeIndex;

                // Collect all vertices from all triangles in the current group
                foreach (var triangle in group)
                {
                    foreach (var vertexIndex in triangle.VertexIndices)
                    {
                        var vertex = jms.Vertices[vertexIndex].Position;

                        // Apply node transformations
                        var transformedVertex = ApplyInverseTranslation(vertex, jms.Nodes[nodeIndex].Position);
                        transformedVertex = ApplyInverseRotation(transformedVertex, jms.Nodes[nodeIndex].Rotation);

                        allVertices.Add(transformedVertex);
                    }
                }

                // Remove duplicate vertices and scale down by 100
                allVertices = allVertices.Distinct().Select(v => new RealPoint3d(v.X / 100, v.Y / 100, v.Z / 100)).ToList();

                // Calculate planes, centroid, and extents for the entire polyhedron
                // Adjust these methods to work with the aggregated vertices
                var planes = allVertices.Select(v => CalculatePlane(allVertices)).Distinct().ToList();
                var centroid = CalculateCentroid(allVertices);
                var extents = CalculateExtents(allVertices);

                // Create a single polyhedron for the current material group
                var serializedPolyhedron = new
                {
                    Type = "Polyhedron",
                    Data = new
                    {
                        Planes = planes.Select(p => new List<float> { p.Item1.I, p.Item1.J, p.Item1.K, p.Item2 }).ToList(),
                        Vertices = allVertices.Select(v => new List<float> { v.X, v.Y, v.Z }).ToList(),
                        Center = centroid,
                        Extents = extents,
                        Mass = 100, // Placeholder value
                        Friction = 0.85f, // Placeholder value
                        Restitution = 0.3f // Placeholder value
                    }
                };

                output.Add(serializedPolyhedron);
            }

            // Convert the polyhedra list to a JSON string
            string jsonString = JsonConvert.SerializeObject(output, Formatting.Indented);

            // Assuming ParseFromFile has been adjusted to accept a JSON string
            // Replace ParseFromFile with your method capable of processing JSON string
            ParseFromFile(jsonString, false, jms, cache, cacheStream, shapeToNodeIndexMap);
            return true;
        }

        public RealPoint3d ApplyInverseTranslation(RealPoint3d point, RealVector3d translation)
        {
            return new RealPoint3d(point.X - translation.I, point.Y - translation.J, point.Z - translation.K);
        }

        public RealPoint3d ApplyInverseRotation(RealPoint3d point, RealQuaternion rotation)
        {
            // Convert RealPoint3d to Vector3
            var v = new Vector3(point.X, point.Y, point.Z);

            // Convert RealQuaternion to Quaternion and then get its conjugate (inverse)
            var q = new Quaternion(rotation.I, rotation.J, rotation.K, rotation.W);
            var qConjugate = Quaternion.Conjugate(q);

            // Apply the conjugate rotation to the vector
            v = Vector3.Transform(v, qConjugate);

            // Convert back to RealPoint3d
            return new RealPoint3d(v.X, v.Y, v.Z);
        }

        private (RealVector3d, float) CalculatePlane(List<RealPoint3d> vertices)
        {
            if (vertices.Count < 3)
                throw new ArgumentException("Need at least three vertices to calculate a plane.");

            // Convert RealPoint3d to Vector3 for easier calculations
            var p0 = new Vector3(vertices[0].X, vertices[0].Y, vertices[0].Z);
            var p1 = new Vector3(vertices[1].X, vertices[1].Y, vertices[1].Z);
            var p2 = new Vector3(vertices[2].X, vertices[2].Y, vertices[2].Z);

            // Calculate vectors from the points
            var v1 = Vector3.Subtract(p1, p0);
            var v2 = Vector3.Subtract(p2, p0);

            // Calculate the cross product of the vectors to get the normal to the plane
            var normal = Vector3.Cross(v1, v2);
            normal = Vector3.Normalize(normal);

            // Calculate D (distance from origin to the plane along the normal)
            // using the formula D = - (Nx * Px + Ny * Py + Nz * Pz)
            var d = -(normal.X * p0.X + normal.Y * p0.Y + normal.Z * p0.Z);

            // Convert back to RealVector3d for the normal
            var normalRv3d = new RealVector3d(normal.X, normal.Y, normal.Z);

            return (normalRv3d, d);
        }

        public List<float> CalculateCentroid(List<RealPoint3d> vertices)
        {
            var centroid = new RealPoint3d(
                vertices.Average(vertex => vertex.X),
                vertices.Average(vertex => vertex.Y),
                vertices.Average(vertex => vertex.Z));

            // Return as a list of floats
            return new List<float> { centroid.X, centroid.Y, centroid.Z };
        }

        public List<float> CalculateExtents(List<RealPoint3d> vertices)
        {
            // Calculate AABB
            float minX = vertices.Min(v => v.X);
            float maxX = vertices.Max(v => v.X);
            float minY = vertices.Min(v => v.Y);
            float maxY = vertices.Max(v => v.Y);
            float minZ = vertices.Min(v => v.Z);
            float maxZ = vertices.Max(v => v.Z);

            // Calculate half-extents
            float halfExtentX = (maxX - minX) / 2.0f;
            float halfExtentY = (maxY - minY) / 2.0f;
            float halfExtentZ = (maxZ - minZ) / 2.0f;

            // Return as a list of floats
            return new List<float> { halfExtentX, halfExtentY, halfExtentZ };
        }

        public bool ParseFromFile(string fname, bool mopps, JmsFormat jms, GameCacheHaloOnlineBase cache, Stream cacheStream, Dictionary<int, int> shapeToNodeIndexMap)
        {
            BlenderPhmoReader reader = new BlenderPhmoReader(fname);
            fileStruct = reader.ReadFile();
            if (fileStruct == null)
            {
                fileStruct = reader.ReadString();
                if (fileStruct == null)
                {
                    new TagToolError(CommandError.CustomError, "Could not parse file!");
                    return false;
                }
            }

            // deserialize globals tag
            cache.TagCacheGenHO.TryGetTag("globals\\globals.globals", out CachedTag globals);
            Globals globalsInstance = cache.Deserialize<Globals>(cacheStream, globals);

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

                // parse the jms regions if it exists
                Dictionary<string, int> regionNameToIndex = new Dictionary<string, int>();
                Dictionary<string, int> permutationNameToIndex = new Dictionary<string, int>();

                _phmo.Regions = new List<PhysicsModel.Region>();
                _phmo.Materials = new List<PhysicsModel.Material>();

                if (jms != null)
                {
                    // Step 1: Parse material names to extract region and permutation information
                    foreach (var material in jms.Materials)
                    {
                        var parts = material.MaterialName.Split(' ');
                        if (parts.Length >= 3)
                        {
                            var permutationName = parts[1];
                            var regionName = parts[2];

                            if (!regionNameToIndex.ContainsKey(regionName))
                            {
                                var region = new PhysicsModel.Region
                                {
                                    Name = cache.StringTable.GetStringId(regionName),
                                    Permutations = new List<PhysicsModel.Region.Permutation>()
                                };
                                _phmo.Regions.Add(region);
                                regionNameToIndex[regionName] = _phmo.Regions.Count - 1;
                            }

                            if (!permutationNameToIndex.ContainsKey(permutationName))
                            {
                                var permutation = new PhysicsModel.Region.Permutation
                                {
                                    Name = cache.StringTable.GetStringId(permutationName)
                                };
                                _phmo.Regions[regionNameToIndex[regionName]].Permutations.Add(permutation);
                                permutationNameToIndex[permutationName] = _phmo.Regions[regionNameToIndex[regionName]].Permutations.Count - 1;
                            }
                        }
                        _phmo.Materials = new List<PhysicsModel.Material>();
                        var phmoMaterial = new PhysicsModel.Material
                        {
                            Name = cache.StringTable.GetStringId(material.Name),
                            MaterialName = cache.StringTable.GetStringId(material.Name),
                            PhantomType = -1
                        };
                        _phmo.Materials.Add(phmoMaterial);
                    }
                }

                int listelemIndex = -1;
                foreach (JSONNode listelem in shapedefs)
                {
                    listelemIndex++;
                    int nodeIndex = shapeToNodeIndexMap[listelemIndex];
                    BlamShapeType typeAdded = AddShape(_phmo, listelem, globalsInstance, cache);
                    if (typeAdded == BlamShapeType.TriangleMesh)
                    {
                        new TagToolError(CommandError.CustomError, "Failed to load shape!");
                        return false;
                    }

                    var shapeElem = new PhysicsModel.ListShape
                    {
                        //assumes the shape added should be at the end of the respected list.
                        Shape = new Havok.HavokShapeReference(typeAdded, (short)(GetNumberOfShapes(_phmo, typeAdded) - 1))
                    };

                    _phmo.ListShapes.Add(shapeElem);

                    if (!mopps)
                    {
                        var lastPoly = _phmo.Polyhedra[_phmo.Polyhedra.Count - 1];
                        var materialName = jms.Materials[lastPoly.MaterialIndex].MaterialName;
                        var parts = materialName.Split(' ');
                        short regionIndex = -1, permutationIndex = -1;
                        if (parts.Length >= 3)
                        {
                            var permutationName = parts[1];
                            var regionName = parts[2];
                            regionIndex = regionNameToIndex.ContainsKey(regionName) ? (short)regionNameToIndex[regionName] : (short)-1;
                            permutationIndex = permutationNameToIndex.ContainsKey(permutationName) ? (short)permutationNameToIndex[permutationName] : (short)-1;
                        }
                        var i = lastPoly.AabbCenter.I;
                        var j = lastPoly.AabbCenter.J;
                        var k = lastPoly.AabbCenter.K;

                        double maxDistance = 0;
                        for (int l = 0; l < _phmo.PolyhedronFourVectors.Count; l++)
                        {
                            for (int m = l + 1; m < _phmo.PolyhedronFourVectors.Count; m++)
                            {
                                double distance = CalculateDistance(_phmo.PolyhedronFourVectors[l], _phmo.PolyhedronFourVectors[m]);
                                if (distance > maxDistance)
                                {
                                    maxDistance = distance;
                                }
                            }
                        }

                        // calculate inertia tensor based on aab half extents
                        var hx = lastPoly.AabbHalfExtents.I;
                        var hy = lastPoly.AabbHalfExtents.J;
                        var hz = lastPoly.AabbHalfExtents.K;
                        var cx = lastPoly.AabbCenter.I;
                        var cy = lastPoly.AabbCenter.J;
                        var cz = lastPoly.AabbCenter.K;
                        float mass = lastPoly.Mass;

                        // Calculate original inertia tensor at origin
                        float Ixx = (1.0f / 12.0f) * mass * (hy * hy + hz * hz);
                        float Iyy = (1.0f / 12.0f) * mass * (hx * hx + hz * hz);
                        float Izz = (1.0f / 12.0f) * mass * (hx * hx + hy * hy);

                        // Apply parallel axis theorem to shift inertia tensor to actual center of mass
                        float IxxShifted = Ixx + mass * (cy * cy + cz * cz);
                        float IyyShifted = Iyy + mass * (cx * cx + cz * cz);
                        float IzzShifted = Izz + mass * (cx * cx + cy * cy);

                        var shapeIndex = (short)(GetNumberOfShapes(_phmo, typeAdded) - 1);
                        var polyMaterialIndex = _phmo.Polyhedra[shapeIndex].MaterialIndex;

                        var (friction, restitution, density) = GetMaterialProperties(globalsInstance, materialName, cache);

                        PhysicsModel.RigidBody shapeRigidBody = new PhysicsModel.RigidBody
                        {
                            Node = (short)nodeIndex,
                            Region = regionIndex,
                            Permutation = permutationIndex,
                            Mass = lastPoly.Mass, //probably not important
                            CenterOfMass = new RealVector3d(i, j, k), //use center from corresponding polyhedron
                            ShapeType = typeAdded,
                            ShapeIndex = shapeIndex, //important
                            MotionType = PhysicsModel.RigidBody.MotionTypeValue.Box, // box by default.
                            BoundingSphereRadius = (float)(maxDistance / 2.0),
                            NoPhantomPowerAltRigidBody = -1,
                            InertiaTensorScale = 1,
                            AngularDampening = 1 - restitution,
                            InertiaTensorX = new RealVector3d(IxxShifted, 0, 0),
                            InertiaTensorY = new RealVector3d(0, IyyShifted, 0),
                            InertiaTensorZ = new RealVector3d(0, 0, IzzShifted),
                        };
                        _phmo.RigidBodies.Add(shapeRigidBody);
                        int rigidBodyIndex = _phmo.RigidBodies.Count - 1; // Get the index of the newly added rigid body

                        // Ensure the region and permutation exist
                        if (regionIndex >= 0 && regionIndex < _phmo.Regions.Count)
                        {
                            var region = _phmo.Regions[regionIndex];
                            if (permutationIndex >= 0 && permutationIndex < region.Permutations.Count)
                            {
                                var permutation = region.Permutations[permutationIndex];

                                // Initialize the RigidBodies list if it's null
                                if (permutation.RigidBodies == null)
                                {
                                    permutation.RigidBodies = new List<PhysicsModel.Region.Permutation.RigidBody>();
                                }

                                // Add a new RigidBodyReference to the permutation's list of rigid bodies
                                var rigidBodyRef = new PhysicsModel.Region.Permutation.RigidBody
                                {
                                    RigidBodyIndex = (short)rigidBodyIndex
                                };
                                permutation.RigidBodies.Add(rigidBodyRef);
                            }
                        }
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
                        Child = new HkpSingleShapeContainer { Shape = new Havok.HavokShapeReference { Type = BlamShapeType.List, Index = 0 } }
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
        /// <param name="globals">the globals tag from which we can calculate mass and stuff</param>
        /// <param name="cache">the cache reference for deserializing the globals tag</param>
        /// <returns>shape type added, 'Unused0' is used to represent failure.</returns>
        private BlamShapeType AddShape(PhysicsModel phmo, JSONNode n, Globals globals, GameCacheHaloOnlineBase cache)
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
                    return AddPolyhedron(phmo, n["Data"], globals, cache) ? BlamShapeType.Polyhedron : BlamShapeType.TriangleMesh;
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

        public (float friction, float restitution, float density) GetMaterialProperties(Globals globals, string materialName, GameCacheHaloOnlineBase cache)
        {
            foreach (var material in globals.Materials)
            {
                if (cache.StringTable.GetString(material.Name).Equals(materialName, StringComparison.OrdinalIgnoreCase))
                {
                    return (material.Friction, material.Restitution, material.Density);
                }
            }

            // Default values if not found
            return (0.85f, 0.3f, 2500f);
        }

        private bool AddPolyhedron(PhysicsModel phmo, JSONNode n, Globals globals, GameCacheHaloOnlineBase cache)
        {
            if (n == null)
                return false;

            //In the control flow, it could have been possible to have
            // no polyhedra up to this point.
            if (phmo.Polyhedra == null)
                phmo.Polyhedra = new List<Polyhedron>();

            int index = phmo.Polyhedra.Count;
            Polyhedron poly = new Polyhedron();

            float friction, mass, restitution, density;
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

            var materialIndex = poly.MaterialIndex;
            var materialName = cache.StringTable.GetString(phmo.Materials[materialIndex].MaterialName);
            (friction, restitution, density) = GetMaterialProperties(globals, materialName, cache);

            var center = n["Center"].AsArray;
            var extents = n["Extents"].AsArray;

            float volume = 8 * extents[0].AsFloat * extents[1].AsFloat * extents[2].AsFloat;

            mass = density * volume;

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
            poly.ShapeBase.Userdata = new Cache.PlatformUnsignedValue((uint)(32 + index * 128));

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
            poly.Volume = volume;

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
                fourvert.FourVectorsXW = v3.AsArray[0].AsFloat;
                fourvert.FourVectorsY.I = v0.AsArray[1].AsFloat;
                fourvert.FourVectorsY.J = v1.AsArray[1].AsFloat;
                fourvert.FourVectorsY.K = v2.AsArray[1].AsFloat;
                fourvert.FourVectorsYW = v3.AsArray[1].AsFloat;
                fourvert.FourVectorsZ.I = v0.AsArray[2].AsFloat;
                fourvert.FourVectorsZ.J = v1.AsArray[2].AsFloat;
                fourvert.FourVectorsZ.K = v2.AsArray[2].AsFloat;
                fourvert.FourVectorsZW = v3.AsArray[2].AsFloat;

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
