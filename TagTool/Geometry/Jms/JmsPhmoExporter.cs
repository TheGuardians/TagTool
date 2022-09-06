using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using System.Numerics;
using TagTool.Geometry.BspCollisionGeometry.Utils;

namespace TagTool.Geometry.Jms
{
    public class JmsPhmoExporter
    {
        GameCache Cache { get; set; }
        JmsFormat Jms { get; set; }

        private float[,] Bounds = new float[3, 2];
        private List<float> BoundingCoords = new List<float>();

        public JmsPhmoExporter(GameCache cacheContext, JmsFormat jms)
        {
            Cache = cacheContext;
            Jms = jms;
        }

        public void Export(PhysicsModel phmo)
        {
            foreach (var material in phmo.Materials)
                Jms.Materials.Add(new JmsFormat.JmsMaterial
                {
                    Name = Cache.StringTable.GetString(material.Name),
                    MaterialName = Cache.StringTable.GetString(material.MaterialName)
                });
            foreach (var box in phmo.Boxes)
            {
                JmsFormat.JmsBox newbox = new JmsFormat.JmsBox
                {
                    Name = Cache.StringTable.GetString(box.Name),
                    Parent = -1,
                    Material = box.MaterialIndex,
                    Rotation = new RealQuaternion(),
                    Translation = new RealVector3d(box.Translation.I, box.Translation.J, box.Translation.K) * 100.0f,
                    Width = box.HalfExtents.I * 2 * 100.0f,
                    Length = box.HalfExtents.J * 2 * 100.0f,
                    Height = box.HalfExtents.K * 2 * 100.0f
                };

                //quaternion rotation from 4x4 matrix
                Matrix4x4 boxMatrix = new Matrix4x4(box.RotationI.I, box.RotationI.J, box.RotationI.K, box.RotationIRadius,
                    box.RotationJ.I, box.RotationJ.J, box.RotationJ.K, box.RotationJRadius,
                    box.RotationK.I, box.RotationK.J, box.RotationK.K, box.RotationKRadius,
                    box.Translation.I, box.Translation.J, box.Translation.K, box.TranslationRadius);
                Quaternion boxQuat = Quaternion.CreateFromRotationMatrix(boxMatrix);
                newbox.Rotation = new RealQuaternion(boxQuat.X, boxQuat.Y, boxQuat.Z, boxQuat.W);

                int rigidbody = phmo.RigidBodies.FindIndex(r => r.ShapeType == Havok.BlamShapeType.Box && r.ShapeIndex == phmo.Boxes.IndexOf(box));
                if (rigidbody != -1)
                    newbox.Parent = phmo.RigidBodies[rigidbody].Node;
                Jms.Boxes.Add(newbox);
            }
            foreach(var sphere in phmo.Spheres)
            {
                JmsFormat.JmsSphere newsphere = new JmsFormat.JmsSphere
                {
                    Name = Cache.StringTable.GetString(sphere.Name),
                    Parent = -1,
                    Material = sphere.MaterialIndex,
                    Rotation = new RealQuaternion(), //doesn't matter
                    Translation = new RealVector3d(sphere.Translation.I, sphere.Translation.J, sphere.Translation.K) * 100.0f,
                    Radius = sphere.ShapeBase.Radius * 100.0f
                };
                int rigidbody = phmo.RigidBodies.FindIndex(r => r.ShapeType == Havok.BlamShapeType.Sphere && r.ShapeIndex == phmo.Spheres.IndexOf(sphere));
                if (rigidbody != -1)
                    newsphere.Parent = phmo.RigidBodies[rigidbody].Node;
                Jms.Spheres.Add(newsphere);
            }
            foreach (var pill in phmo.Pills)
            {
                JmsFormat.JmsCapsule newpill = new JmsFormat.JmsCapsule
                {
                    Name = Cache.StringTable.GetString(pill.Name),
                    Parent = -1,
                    Material = pill.MaterialIndex,
                    Rotation = new RealQuaternion(),
                    Translation = pill.Bottom * 100.0f,
                    Height = RealVector3d.Norm(pill.Top - pill.Bottom) * 100.0f,
                    Radius = pill.ShapeBase.Radius * 100.0f
                };

                int rigidbody = phmo.RigidBodies.FindIndex(r => r.ShapeType == Havok.BlamShapeType.Pill && r.ShapeIndex == phmo.Pills.IndexOf(pill));
                if (rigidbody != -1)
                    newpill.Parent = phmo.RigidBodies[rigidbody].Node;

                //pill translation needs to be adjusted to include pill radius
                var pillBottom = new Vector3(pill.Bottom.I, pill.Bottom.J, pill.Bottom.K) * 100.0f;
                var pillTop = new Vector3(pill.Top.I, pill.Top.J, pill.Top.K) * 100.0f;
                RealVector3d pillvec = (pill.Top - pill.Bottom) * 100.0f;
                Vector3 pillVector = new Vector3(pillvec.I, pillvec.J, pillvec.K);
                Vector3 inverseVec = pillVector * (newpill.Radius / newpill.Height);
                Vector3 newPos = pillBottom - inverseVec;
                newpill.Translation = new RealVector3d(newPos.X, newPos.Y, newPos.Z);

                //calculate quaternion rotation from pill vector
                Matrix4x4 newMatrix = Matrix4x4.CreateLookAt(pillBottom, pillTop, new Vector3(0,1,0));
                Quaternion newQuat = Quaternion.CreateFromRotationMatrix(newMatrix);
                newpill.Rotation = new RealQuaternion(newQuat.X, newQuat.Y, newQuat.Z, newQuat.W);                    
               
                Jms.Capsules.Add(newpill);
            }

            setup_bounding_coords();
            int planeEquationsOffset = 0;
            List<RealPlane3d> planelist = phmo.PolyhedronPlaneEquations.Select(e => e.PlaneEquation).ToList();
            foreach(var poly in phmo.Polyhedra)
            {
                List<RealPoint3d> points = new List<RealPoint3d>();
                List<RealPlane3d> currentPlanes = planelist.GetRange(planeEquationsOffset, poly.PlaneEquationsSize);
                planeEquationsOffset += poly.PlaneEquationsSize;
                foreach (var plane in currentPlanes)
                {
                    List<RealPoint3d> polygon = plane_clip_to_polygon(plane, currentPlanes);
                    foreach (var point in polygon)
                    {
                        bool duplicatedPoint = false;
                        foreach(var existingPoint in points)
                            if(RealVector3d.Norm(PointToVector(point * 100.0f - existingPoint)) < 0.01)
                            {
                                duplicatedPoint = true;
                                break;
                            }
                        if(!duplicatedPoint)
                            points.Add(point * 100.0f);
                    }                        
                };
                JmsFormat.JmsConvexShape newConvex = new JmsFormat.JmsConvexShape
                {
                    Name = Cache.StringTable.GetString(poly.Name),
                    Parent = -1,
                    Material = poly.MaterialIndex,
                    Rotation = new RealQuaternion(),
                    Translation = new RealVector3d(),
                    ShapeVertexCount = points.Count,
                    ShapeVertices = points
                };

                int rigidbody = phmo.RigidBodies.FindIndex(r => r.ShapeType == Havok.BlamShapeType.Polyhedron && r.ShapeIndex == phmo.Polyhedra.IndexOf(poly));
                if (rigidbody != -1)
                    newConvex.Parent = phmo.RigidBodies[rigidbody].Node;

                Jms.ConvexShapes.Add(newConvex);
            }
        }
        public Matrix4x4 MatrixFromNode(RealQuaternion rotation, RealVector3d position)
        {
            var quat = new Quaternion(rotation.I, rotation.J, rotation.K, rotation.W);

            Matrix4x4 rot = Matrix4x4.CreateFromQuaternion(quat);
            rot.Translation = new Vector3(position.I, position.J, position.K);

            return rot;
        }

        public void setup_bounding_coords()
        {
            //set bounds as arbitrarily large, they will be cut down later
            Bounds = new float[,]
            {
                {-1000.0f, 1000.0f },
                {-1000.0f, 1000.0f },
                {-1000.0f, 1000.0f }
            };

            //create a list of 48 floats that consist of 2d points that compose each face of a bounding box surrounding the geometry
            int[,] coordinate_pairs = new int[,]
            { { 2, 1 }, { 1, 2 }, { 0, 2 }, { 2, 0 }, { 1, 0 }, { 0, 1 }  };
            for (var i = 0; i < 6; i++)
            {
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 1]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 1]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 1]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 1]);
            }
        }

        public List<RealPoint3d> plane_clip_to_polygon(RealPlane3d plane, List<RealPlane3d> planelist)
        {
            CollisionBSPGen3Builder gen3builder = new CollisionBSPGen3Builder();
            int projection_axis = gen3builder.plane_get_projection_coefficient(plane);
            int projection_sign = gen3builder.plane_get_projection_sign(plane, projection_axis) ? 1 : 0;

            //build a polygon that exists on the current plane that extends to the maximum bounds
            List<RealPoint3d> vertices = new List<RealPoint3d>();
            for (var i = 0; i < 8; i += 2)
            {
                RealPoint2d point2d = new RealPoint2d
                {
                    X = BoundingCoords[16 * projection_axis + 8 * projection_sign + i],
                    Y = BoundingCoords[16 * projection_axis + 8 * projection_sign + i + 1]
                };
                vertices.Add(gen3builder.point2d_and_plane_to_point3d(plane, point2d));
            }

            //use the other planes to clip this new polygon down to size
            foreach (var temp_plane in planelist)
            {
                if (temp_plane == plane)
                    continue;
                vertices = gen3builder.plane_cut_polygon(vertices, temp_plane, true);
            }

            return vertices;
        }

        RealVector3d PointToVector(RealPoint3d point)
        {
            return new RealVector3d(point.X, point.Y, point.Z);
        }
    }
}
