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
using TagTool.Commands.Common;

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
                Vector3 pillVector = pillTop - pillBottom;
                Vector3 inverseVector = pillBottom - pillTop;
                inverseVector = Vector3.Normalize(inverseVector) * newpill.Radius;
                Vector3 newPos = pillBottom + inverseVector;
                newpill.Translation = new RealVector3d(newPos.X, newPos.Y, newPos.Z);

                Quaternion newQuat = QuaternionFromVector(pillVector);
                newpill.Rotation = new RealQuaternion(newQuat.X, newQuat.Y, newQuat.Z, newQuat.W);                    
               
                Jms.Capsules.Add(newpill);
            }

            int fourvectorsoffset = 0;
            if (phmo.Polyhedra.Count > 0)
                new TagToolWarning("Physics model polyhedra are modified on import, and exported polyhedra will not match source assets.");
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

        public static Quaternion QuaternionFromVector(Vector3 vec)
        {
            vec = Vector3.Normalize(vec);
            var up = new Vector3(0, 0, -1); //modified up reference given different coordinate space
            var c = Vector3.Dot(vec, up);
            if (Math.Abs(c + 1.0) < 1e-5)
                return new Quaternion(0, 0, 0, 1);
            else if (Math.Abs(c - 1.0) < 1e-5)
                return new Quaternion((float)Math.PI, 0, 0, 1);
            else
            {
                var axis = Vector3.Normalize(Vector3.Cross(vec, up));
                var angle = (float)Math.Acos(c);
                var w = (float)Math.Cos(angle / 2.0);
                var sin = (float)Math.Sin(angle / 2.0);
                return new Quaternion(axis.X * sin, axis.Y * sin, axis.Z * sin, w);
            }
        }
    }
}
