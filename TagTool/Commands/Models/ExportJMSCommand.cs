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
        private bool ExportRender = false;
        private bool ExportPhysics = false;
        private bool ExportCollision = false;
        private bool ExportAnimations = false;

        public ExportJMSCommand(GameCache cache, Model definition) :
            base(true,

                "ExportJMS",
                "",

                "ExportJMS <coll/mode/phmo> <File>",

                "")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            switch (args[0])
            {
                case "coll":
                    ExportCollision = true;
                    break;
                case "mode":
                    ExportRender = true;
                    break;
                case "phmo":
                    ExportPhysics = true;
                    break;
                default:
                    return new TagToolError(CommandError.ArgInvalid);
            }

            var file = new FileInfo(args[1]);

            if (!file.Directory.Exists)
                file.Directory.Create();

            JmsFormat jms = new JmsFormat();

            //build nodes
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
                if (ExportCollision && Definition.CollisionModel != null)
                {
                    CollisionModel coll = Cache.Deserialize<CollisionModel>(cacheStream, Definition.CollisionModel);
                    JmsCollExporter exporter = new JmsCollExporter(Cache, jms);
                    exporter.Export(coll);
                }
                if (ExportPhysics && Definition.PhysicsModel != null)
                {
                    PhysicsModel phmo = Cache.Deserialize<PhysicsModel>(cacheStream, Definition.PhysicsModel);
                    JmsPhmoExporter exporter = new JmsPhmoExporter(Cache, jms);
                    exporter.Export(phmo);
                }
                if (ExportRender && Definition.RenderModel != null)
                {
                    RenderModel mode = Cache.Deserialize<RenderModel>(cacheStream, Definition.RenderModel);
                    JmsModeExporter exporter = new JmsModeExporter(Cache, jms);
                    exporter.Export(mode);
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