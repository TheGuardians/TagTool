using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Geometry.Jms;
using System.Numerics;

namespace TagTool.Commands.Models
{
    class ExportJMSCommand : Command
    {
        private GameCache Cache { get; }
        private Model Definition { get; }

        public ExportJMSCommand(GameCache cache, Model definition) :
            base(true,

                "ExportJMS",
                "",

                "ExportJMS <File>",

                "")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var file = new FileInfo(args[0]);

            if (!file.Directory.Exists)
                file.Directory.Create();

            JmsFormat jms = new JmsFormat();

            foreach (var node in Definition.Nodes)
            {
                var newnode = new JmsFormat.JmsNode
                {
                    Name = Cache.StringTable.GetString(node.Name),
                    ParentNodeIndex = node.ParentNode,
                    Rotation = node.DefaultRotation,
                    Position = new RealVector3d(node.DefaultTranslation.X, node.DefaultTranslation.Y, node.DefaultTranslation.Z) * 100.0f
                };
                if (newnode.ParentNodeIndex != -1)
                {
                    Matrix4x4 transform = MatrixFromNode(newnode.Rotation, newnode.Position);
                    Matrix4x4 parent_transform = MatrixFromNode(jms.Nodes[newnode.ParentNodeIndex].Rotation,
                        jms.Nodes[newnode.ParentNodeIndex].Position);
                    Matrix4x4 result = Matrix4x4.Multiply(transform, parent_transform);

                    Vector3 out_scale = new Vector3();
                    Vector3 out_translate = new Vector3();
                    Quaternion out_rotate = new Quaternion();
                    Matrix4x4.Decompose(result, out out_scale, out out_rotate, out out_translate);
                    newnode.Position = new RealVector3d(out_translate.X * out_scale.X, out_translate.Y * out_scale.Y, out_translate.Z * out_scale.Z);
                    newnode.Rotation = new RealQuaternion(out_rotate.X, out_rotate.Y, out_rotate.Z, out_rotate.W);
                }

                jms.Nodes.Add(newnode);
            }

            using (var cacheStream = Cache.OpenCacheRead())
            {
                if (Definition.PhysicsModel != null)
                {
                    PhysicsModel phmo = Cache.Deserialize<PhysicsModel>(cacheStream, Definition.PhysicsModel);
                    JmsPhmoExporter exporter = new JmsPhmoExporter(Cache, jms);
                    exporter.Export(phmo);
                }


            }

            jms.Write(file);
            return true;
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