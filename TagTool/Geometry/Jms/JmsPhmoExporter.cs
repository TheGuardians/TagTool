using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using System.Numerics;
using TagTool.Geometry.BspCollisionGeometry.Utils;
using TagTool.Geometry.Utils;
using System.Threading.Tasks.Sources;

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
                RealVector3d pillvec = (pill.Top - pill.Bottom);
                Vector3 pillVector = new Vector3(pillvec.I, pillvec.J, pillvec.K);
                Vector3 inverseVec = pillVector * (newpill.Radius / newpill.Height);
                Vector3 newPos = pillBottom - inverseVec;
                newpill.Translation = new RealVector3d(newPos.X, newPos.Y, newPos.Z);
                
                Quaternion newQuat = Quaternion.CreateFromRotationMatrix(
                    Matrix4x4.CreateLookAt(pillBottom, pillTop, FindVectorUpAxis(pillVector)));
                newpill.Rotation = new RealQuaternion(newQuat.X, newQuat.Y, newQuat.Z, newQuat.W);                    
               
                Jms.Capsules.Add(newpill);
            }

            int fourvectorsoffset = 0;        
            foreach(var poly in phmo.Polyhedra)
            {
                HashSet<RealPoint3d> points = new HashSet<RealPoint3d>();
                for(var i = fourvectorsoffset; i < poly.FourVectorsSize + fourvectorsoffset; i++)
                {
                    PhysicsModel.PolyhedronFourVector fourVector = phmo.PolyhedronFourVectors[i];
                    points.Add(new RealPoint3d(fourVector.FourVectorsX.I, fourVector.FourVectorsY.I, fourVector.FourVectorsZ.I) * 100.0f);
                    points.Add(new RealPoint3d(fourVector.FourVectorsX.J, fourVector.FourVectorsY.J, fourVector.FourVectorsZ.J) * 100.0f);
                    points.Add(new RealPoint3d(fourVector.FourVectorsX.K, fourVector.FourVectorsY.K, fourVector.FourVectorsZ.K) * 100.0f);
                    points.Add(new RealPoint3d(fourVector.FourVectorsXW, fourVector.FourVectorsYW, fourVector.FourVectorsZW) * 100.0f);
                }
                fourvectorsoffset += poly.FourVectorsSize;

                JmsFormat.JmsConvexShape newConvex = new JmsFormat.JmsConvexShape
                {
                    Name = Cache.StringTable.GetString(poly.Name),
                    Parent = -1,
                    Material = poly.MaterialIndex,
                    Rotation = new RealQuaternion(),
                    Translation = new RealVector3d(),
                    ShapeVertexCount = points.Count,
                    ShapeVertices = points.ToList()
                };

                int rigidbody = phmo.RigidBodies.FindIndex(r => r.ShapeType == Havok.BlamShapeType.Polyhedron && r.ShapeIndex == phmo.Polyhedra.IndexOf(poly));
                if (rigidbody != -1)
                    newConvex.Parent = phmo.RigidBodies[rigidbody].Node;

                Jms.ConvexShapes.Add(newConvex);
            }
        }
        private Matrix4x4 MatrixFromNode(RealQuaternion rotation, RealVector3d position)
        {
            var quat = new Quaternion(rotation.I, rotation.J, rotation.K, rotation.W);

            Matrix4x4 rot = Matrix4x4.CreateFromQuaternion(quat);
            rot.Translation = new Vector3(position.I, position.J, position.K);

            return rot;
        }

        private Quaternion QuatFromRealQuat(RealQuaternion q)
        {
            return new Quaternion(q.I, q.J, q.K, q.W);
        }

        /*
        private Quaternion QuatFromVectors(Vector3 vectorA, Vector3 vectorB)
        {
            vectorA = Vector3.Normalize(vectorA);
            vectorB = Vector3.Normalize(vectorB);
            double F = Math.Acos(Vector3.Dot(vectorA, vectorB)) / 2.0;
            Vector3 u = Vector3.Normalize(Vector3.Cross(vectorA, vectorB));
            Vector3 mult = Vector3.Multiply((float)Math.Sin(F), u);
            float cos = (float)Math.Cos(F);
            return new Quaternion(mult.X, mult.Y, mult.Z, cos);
        }
        */

        private Quaternion get_rotation_between(Vector3 u, Vector3 v)
        {
            float k_cos_theta = Vector3.Dot(u, v);
            float k = (float)Math.Sqrt(u.LengthSquared() * v.LengthSquared());

            if (k_cos_theta / k == -1)
            {
                // 180 degree rotation around any orthogonal vector
                return new Quaternion(Vector3.Normalize(orthogonal(u)), 0);
            }

            return new Quaternion(Vector3.Cross(u, v), k_cos_theta + k);
        }

        private Vector3 orthogonal(Vector3 v)
        {
            Vector3 X_AXIS = new Vector3(1, 0, 0);
            Vector3 Y_AXIS = new Vector3(0, 1, 0);
            Vector3 Z_AXIS = new Vector3(0, 0, 1);

            float x = Math.Abs(v.X);
            float y = Math.Abs(v.Y);
            float z = Math.Abs(v.Z);

            Vector3 other = x < y ? (x < z ? X_AXIS : Z_AXIS) : (y < z ? Y_AXIS : Z_AXIS);
            return Vector3.Cross(v, other);
        }

        private Vector3 FindVectorUpAxis(Vector3 v)
        {
            Vector3 X_AXIS = new Vector3(1, 0, 0);
            Vector3 Y_AXIS = new Vector3(0, 1, 0);
            Vector3 Z_AXIS = new Vector3(0, 0, 1);

            float x = Math.Abs(v.X);
            float y = Math.Abs(v.Y);
            float z = Math.Abs(v.Z);

            Vector3 other = x < y ? (x < z ? X_AXIS : Z_AXIS) : (y < z ? Y_AXIS : Z_AXIS);
            return other;
        }
    }
}
