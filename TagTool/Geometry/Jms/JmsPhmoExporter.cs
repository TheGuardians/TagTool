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
using static TagTool.Tags.Definitions.PhysicsModel;

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

            foreach(var ragdoll in phmo.RagdollConstraints)
            {                
                JmsFormat.JmsRagdoll newRagdoll = new JmsFormat.JmsRagdoll
                {
                    Name = Cache.StringTable.GetString(ragdoll.Name),
                    AttachedIndex = ragdoll.NodeA,
                    ReferencedIndex = ragdoll.NodeB,
                    MinTwist = ragdoll.TwistRange.Lower,
                    MaxTwist = ragdoll.TwistRange.Upper,
                    MinCone = ragdoll.ConeRange.Lower,
                    MaxCone = ragdoll.ConeRange.Upper,
                    MinPlane = ragdoll.PlaneRange.Lower,
                    MaxPlane = ragdoll.PlaneRange.Upper,
                    FrictionLimit = ragdoll.MaxFrictionTorque
                };

                Matrix4x4 MatrixA = new Matrix4x4(ragdoll.AForward.I, ragdoll.AForward.J, ragdoll.AForward.K, 0.0f,
                                    ragdoll.ALeft.I, ragdoll.ALeft.J, ragdoll.ALeft.K, 0.0f,
                                    ragdoll.AUp.I, ragdoll.AUp.J, ragdoll.AUp.K, 0.0f,
                                    ragdoll.APosition.X, ragdoll.APosition.Y, ragdoll.APosition.Z, 0.0f);
                Matrix4x4 MatrixB = new Matrix4x4(ragdoll.BForward.I, ragdoll.BForward.J, ragdoll.BForward.K, 0.0f,
                                    ragdoll.BLeft.I, ragdoll.BLeft.J, ragdoll.BLeft.K, 0.0f,
                                    ragdoll.BUp.I, ragdoll.BUp.J, ragdoll.BUp.K, 0.0f,
                                    ragdoll.BPosition.X, ragdoll.BPosition.Y, ragdoll.BPosition.Z, 0.0f);

                Quaternion ragQuatA = Quaternion.CreateFromRotationMatrix(MatrixA);
                ragQuatA = Quaternion.Negate(ragQuatA);
                newRagdoll.AttachedTransformOrientation = new RealQuaternion(ragQuatA.X, ragQuatA.Y, ragQuatA.Z, ragQuatA.W);
                Vector3 ragPosA = MatrixA.Translation * 100.0f;
                newRagdoll.AttachedTransformPosition = new RealVector3d(ragPosA.X, ragPosA.Y, ragPosA.Z);

                Quaternion ragQuatB = Quaternion.CreateFromRotationMatrix(MatrixB);
                ragQuatB = Quaternion.Negate(ragQuatB);
                newRagdoll.ReferenceTransformOrientation = new RealQuaternion(ragQuatB.X, ragQuatB.Y, ragQuatB.Z, ragQuatB.W);
                Vector3 ragPosB = MatrixB.Translation * 100.0f;
                newRagdoll.ReferenceTransformPosition = new RealVector3d(ragPosB.X, ragPosB.Y, ragPosB.Z);

                Jms.Ragdolls.Add(newRagdoll);
            }

            //non limited hinges
            foreach (var hinge in phmo.HingeConstraints)
            {
                JmsFormat.JmsHinge newhinge = new JmsFormat.JmsHinge
                {
                    Name = Cache.StringTable.GetString(hinge.Name),
                    BodyAIndex = hinge.NodeA,
                    BodyBIndex = hinge.NodeB,
                    IsLimited = 0,
                };

                Matrix4x4 MatrixA = new Matrix4x4(hinge.AForward.I, hinge.AForward.J, hinge.AForward.K, 0.0f,
                                    hinge.ALeft.I, hinge.ALeft.J, hinge.ALeft.K, 0.0f,
                                    hinge.AUp.I, hinge.AUp.J, hinge.AUp.K, 0.0f,
                                    hinge.APosition.X, hinge.APosition.Y, hinge.APosition.Z, 0.0f);
                Matrix4x4 MatrixB = new Matrix4x4(hinge.BForward.I, hinge.BForward.J, hinge.BForward.K, 0.0f,
                                    hinge.BLeft.I, hinge.BLeft.J, hinge.BLeft.K, 0.0f,
                                    hinge.BUp.I, hinge.BUp.J, hinge.BUp.K, 0.0f,
                                    hinge.BPosition.X, hinge.BPosition.Y, hinge.BPosition.Z, 0.0f);

                Quaternion QuatA = Quaternion.CreateFromRotationMatrix(MatrixA);
                newhinge.BodyATransformOrientation = new RealQuaternion(QuatA.X, QuatA.Y, QuatA.Z, QuatA.W);
                Vector3 PosA = MatrixA.Translation * 100.0f;
                newhinge.BodyATransformPosition = new RealVector3d(PosA.X, PosA.Y, PosA.Z);

                Quaternion QuatB = Quaternion.CreateFromRotationMatrix(MatrixB);
                newhinge.BodyBTransformOrientation = new RealQuaternion(QuatB.X, QuatB.Y, QuatB.Z, QuatB.W);
                Vector3 PosB = MatrixB.Translation * 100.0f;
                newhinge.BodyBTransformPosition = new RealVector3d(PosB.X, PosB.Y, PosB.Z);

                Jms.Hinges.Add(newhinge);
            }

            //limited hinges
            foreach (var hinge in phmo.LimitedHingeConstraints)
            {
                JmsFormat.JmsHinge newhinge = new JmsFormat.JmsHinge
                {
                    Name = Cache.StringTable.GetString(hinge.Name),
                    BodyAIndex = hinge.NodeA,
                    BodyBIndex = hinge.NodeB,
                    IsLimited = 1,
                    MinAngle = hinge.LimitAngleBounds.Lower,
                    MaxAngle = hinge.LimitAngleBounds.Upper,
                    FrictionLimit = hinge.LimitFriction
                };

                Matrix4x4 MatrixA = new Matrix4x4(hinge.AForward.I, hinge.AForward.J, hinge.AForward.K, 0.0f,
                                    hinge.ALeft.I, hinge.ALeft.J, hinge.ALeft.K, 0.0f,
                                    hinge.AUp.I, hinge.AUp.J, hinge.AUp.K, 0.0f,
                                    hinge.APosition.X, hinge.APosition.Y, hinge.APosition.Z, 0.0f);
                Matrix4x4 MatrixB = new Matrix4x4(hinge.BForward.I, hinge.BForward.J, hinge.BForward.K, 0.0f,
                                    hinge.BLeft.I, hinge.BLeft.J, hinge.BLeft.K, 0.0f,
                                    hinge.BUp.I, hinge.BUp.J, hinge.BUp.K, 0.0f,
                                    hinge.BPosition.X, hinge.BPosition.Y, hinge.BPosition.Z, 0.0f);

                Quaternion QuatA = Quaternion.CreateFromRotationMatrix(MatrixA);
                newhinge.BodyATransformOrientation = new RealQuaternion(QuatA.X, QuatA.Y, QuatA.Z, QuatA.W);
                Vector3 PosA = MatrixA.Translation * 100.0f;
                newhinge.BodyATransformPosition = new RealVector3d(PosA.X, PosA.Y, PosA.Z);

                Quaternion QuatB = Quaternion.CreateFromRotationMatrix(MatrixB);
                newhinge.BodyBTransformOrientation = new RealQuaternion(QuatB.X, QuatB.Y, QuatB.Z, QuatB.W);
                Vector3 PosB = MatrixB.Translation * 100.0f;
                newhinge.BodyBTransformPosition = new RealVector3d(PosB.X, PosB.Y, PosB.Z);

                Jms.Hinges.Add(newhinge);
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
