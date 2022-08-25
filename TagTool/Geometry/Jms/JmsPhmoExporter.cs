using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Cache;
using TagTool.Common;
using System.Numerics;

namespace TagTool.Geometry.Jms
{
    public class JmsPhmoExporter
    {
        GameCache Cache { get; set; }
        JmsFormat Jms { get; set; }

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
                    Rotation = new RealQuaternion(), //FIX ME
                    Translation = new RealVector3d(box.Translation.I, box.Translation.J, box.Translation.K) * 100.0f,
                    Width = box.HalfExtents.I * 2 * 100.0f,
                    Length = box.HalfExtents.J * 2 * 100.0f,
                    Height = box.HalfExtents.K * 2 * 100.0f
                };
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
                    Translation = pill.Bottom - ((pill.Top - pill.Bottom)/2) * 100.0f,
                    Height = RealVector3d.Norm(pill.Top - pill.Bottom) * 100.0f,
                    Radius = pill.ShapeBase.Radius * 100.0f
                };

                int rigidbody = phmo.RigidBodies.FindIndex(r => r.ShapeType == Havok.BlamShapeType.Pill && r.ShapeIndex == phmo.Pills.IndexOf(pill));
                if (rigidbody != -1)
                    newpill.Parent = phmo.RigidBodies[rigidbody].Node;

                //placeholders
                newpill.Translation = new RealVector3d(0,0,0);
                newpill.Rotation = new RealQuaternion(0,0,0,0);
               
                Jms.Capsules.Add(newpill);
            }
        }
        public Matrix4x4 MatrixFromNode(RealQuaternion rotation, RealVector3d position)
        {
            var quat = new Quaternion(rotation.I, rotation.J, rotation.K, rotation.W);

            Matrix4x4 rot = Matrix4x4.CreateFromQuaternion(quat);
            rot.Translation = new Vector3(position.I, position.J, position.K);

            return rot;
        }
    }
}
