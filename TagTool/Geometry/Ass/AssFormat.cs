using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;

// REFERENCE:  Halo-Asset-Blender-Development-Toolset
// https://github.com/General-101/Halo-Asset-Blender-Development-Toolset

namespace TagTool.Geometry.Ass
{
    public class AssFormat
    {
        public List<AssMaterial> Materials = new List<AssMaterial>();
        public List<AssObject> Objects = new List<AssObject>();
        public List<AssInstance> Instances = new List<AssInstance>();

        public void Write(FileInfo file)
        {
            using (var stream = file.CreateText())
            {
                stream.WriteLine(";### HEADER ###");
                stream.WriteLine(7); //version
                stream.WriteLine("\"\""); //program (e.g. MAX)
                stream.WriteLine("\"\""); //program version (e.g. 8.0)
                stream.WriteLine("\"TAGTOOL\""); //author
                stream.WriteLine("\"\""); //workstation?
                stream.WriteLine();

                stream.WriteLine(";### MATERIALS ###");
                stream.WriteLine(Materials.Count);
                stream.WriteLine();
                for(var materialIndex = 0; materialIndex < Materials.Count; materialIndex++)
                {
                    stream.WriteLine($";MATERIAL {materialIndex}");
                    Materials[materialIndex].Write(stream);
                    stream.WriteLine();
                }

                stream.WriteLine(";### OBJECTS ###");
                stream.WriteLine(Objects.Count);
                stream.WriteLine();
                for (var objectIndex = 0; objectIndex < Objects.Count; objectIndex++)
                {
                    stream.WriteLine($";OBJECT {objectIndex}");
                    Objects[objectIndex].Write(stream);
                    stream.WriteLine();
                }

                stream.WriteLine(";### INSTANCES ###");
                stream.WriteLine(Instances.Count);
                stream.WriteLine();
                for (var instanceIndex = 0; instanceIndex < Instances.Count; instanceIndex++)
                {
                    stream.WriteLine($";INSTANCE {instanceIndex}");
                    Instances[instanceIndex].Write(stream);
                    stream.WriteLine();
                }

                stream.WriteLine();
            }
        }

        public class AssMaterial : AssElement
        {
            public string Name = "";
            public string MaterialEffect = "";
            public List<string> MaterialStrings = new List<string>();
            public void Write(StreamWriter stream)
            {
                stream.WriteLine($"\"{Name}\"");
                stream.WriteLine($"\"{MaterialEffect}\"");
                stream.WriteLine(MaterialStrings.Count);
                foreach(var materialstring in MaterialStrings)
                    stream.WriteLine($"\"{materialstring}\"");
            }
        }

        public class AssObject : AssElement
        {
            public AssObjectType ObjectType = AssObjectType.Mesh;
            public string XrefPath = "";
            public string XrefName = "";
            public List<AssVertex> Vertices = new List<AssVertex>();
            public List<AssTriangle> Triangles = new List<AssTriangle>();
            public AssLightInfo LightInfo = new AssLightInfo();
            public void Write(StreamWriter stream)
            {
                switch (ObjectType)
                {
                    case AssObjectType.Mesh:
                        stream.WriteLine($"\"MESH\"");
                        stream.WriteLine($"\"{XrefPath}\"");
                        stream.WriteLine($"\"{XrefName}\"");
                        stream.WriteLine(Vertices.Count);
                        foreach(var vertex in Vertices)
                        {
                            vertex.Write(stream);
                            stream.WriteLine();
                        }
                        stream.WriteLine(Triangles.Count);
                        foreach(var triangle in Triangles)
                        {
                            triangle.Write(stream);
                        }
                        stream.WriteLine();
                        break;
                    case AssObjectType.GenericLight:
                        stream.WriteLine($"\"GENERIC_LIGHT\"");
                        stream.WriteLine($"\"{XrefPath}\"");
                        stream.WriteLine($"\"{XrefName}\"");
                        break;
                }
            }

            public enum AssObjectType
            {
                Sphere,
                Box,
                Pill,
                Mesh,
                GenericLight
            };

            public class AssLightInfo
            {
                public string light_type = "";
                public RealRgbColor light_color = new RealRgbColor();
                public float intensity = 0.0f;
                public float hotspot_size = 0.0f;
                public float hotspot_falloff_size = 0.0f;
                public int uses_near_attenuation = 0;
                public float near_attenuation_start = 0.0f;
                public float near_attenuation_end = 0.0f;
                public int uses_far_attenuation = 0;
                public float far_attenuation_start = 0.0f;
                public float far_attenuation_end = 0.0f;
                public int light_shape = 0;
                public float light_aspect_ratio = 0.0f;
            }

            public class AssVertex : AssElement
            {
                public RealPoint3d Position = new RealPoint3d(0, 0, 0);
                public RealVector3d Normal = new RealVector3d(0, 0, 0);
                public RealRgbColor VertexColor = new RealRgbColor();
                public List<NodeSet> NodeSets = new List<NodeSet>();
                public List<UvSet> UvSets = new List<UvSet>();

                public class NodeSet
                {
                    public int NodeIndex = -1;
                    public float NodeWeight = 0.0f;
                }
                public class UvSet
                {
                    public RealPoint3d TextureCoordinates = new RealPoint3d();
                }
                public void Read(StreamReader stream)
                {
                }
                public void Write(StreamWriter stream)
                {
                    WritePoint3d(Position, stream);
                    WriteVector3d(Normal, stream);
                    WriteRealRGB(VertexColor, stream);
                    stream.WriteLine(NodeSets.Count);
                    if (NodeSets.Count > 0)
                    {
                        foreach (var nodeset in NodeSets)
                        {
                            stream.WriteLine(nodeset.NodeIndex);
                            WriteFloat(nodeset.NodeWeight, stream);
                        }
                    }
                    stream.WriteLine(UvSets.Count);
                    if (UvSets.Count > 0)
                    {
                        foreach (var uvset in UvSets)
                        {
                            WritePoint3d(uvset.TextureCoordinates, stream);
                        }
                    }
                }
            }

            public class AssTriangle : AssElement
            {
                public int MaterialIndex = -1;
                public List<int> VertexIndices = new List<int>();

                public void Read(StreamReader stream)
                {
                }

                public void Write(StreamWriter stream)
                {
                    stream.WriteLine(MaterialIndex);
                    stream.WriteLine($"{MaterialIndex}\t\t{VertexIndices[0]}\t{VertexIndices[1]}\t{VertexIndices[2]}");
                }
            }
        }
        
        public class AssInstance : AssElement
        {
            public int ObjectIndex = -1;
            public string Name = "";
            public int UniqueID = -1;
            public int ParentID = -1;
            public int InheritanceFlag = 0;
            public Transform LocalTransform = new Transform();
            public Transform PivotTransform = new Transform();

            public class Transform
            {
                public RealQuaternion Rotation = new RealQuaternion();
                public RealVector3d Translation = new RealVector3d();
                public float Scale = 1.0f;
            }

            public void Write(StreamWriter stream)
            {
                stream.WriteLine(ObjectIndex);
                stream.WriteLine($"\"{Name}\"");
                stream.WriteLine(UniqueID);
                stream.WriteLine(ParentID);
                stream.WriteLine(InheritanceFlag);
                WriteQuaternion(LocalTransform.Rotation, stream);
                WriteVector3d(LocalTransform.Translation, stream);
                WriteFloat(LocalTransform.Scale, stream);
                WriteQuaternion(PivotTransform.Rotation, stream);
                WriteVector3d(PivotTransform.Translation, stream);
                WriteFloat(PivotTransform.Scale, stream);
            }
        }

        public class AssElement
        {
            public void WriteQuaternion(RealQuaternion quaternion, StreamWriter stream)
            {
                stream.Write(quaternion.I.ToString("0.0000000000"));
                stream.Write('\t');
                stream.Write(quaternion.J.ToString("0.0000000000"));
                stream.Write('\t');
                stream.Write(quaternion.K.ToString("0.0000000000"));
                stream.Write('\t');
                stream.Write(quaternion.W.ToString("0.0000000000"));
                stream.WriteLine();
            }

            public void WriteVector3d(RealVector3d point, StreamWriter stream)
            {
                stream.Write(point.I.ToString("0.0000000000"));
                stream.Write('\t');
                stream.Write(point.J.ToString("0.0000000000"));
                stream.Write('\t');
                stream.Write(point.K.ToString("0.0000000000"));
                stream.WriteLine();
            }
            public void WritePoint3d(RealPoint3d point, StreamWriter stream)
            {
                stream.Write(point.X.ToString("0.0000000000"));
                stream.Write('\t');
                stream.Write(point.Y.ToString("0.0000000000"));
                stream.Write('\t');
                stream.Write(point.Z.ToString("0.0000000000"));
                stream.WriteLine();
            }
            public void WriteRealRGB(RealRgbColor color, StreamWriter stream)
            {
                stream.Write(color.Red.ToString("0.0000000000"));
                stream.Write('\t');
                stream.Write(color.Green.ToString("0.0000000000"));
                stream.Write('\t');
                stream.Write(color.Blue.ToString("0.0000000000"));
                stream.WriteLine();
            }

            public void WritePoint2d(RealPoint2d point, StreamWriter stream)
            {
                stream.Write(point.X.ToString("0.0000000000"));
                stream.Write('\t');
                stream.Write(point.Y.ToString("0.0000000000"));
                stream.WriteLine();
            }

            public void WriteFloat(float number, StreamWriter stream)
            {
                stream.WriteLine(number.ToString("0.0000000000"));
            }

            public RealVector3d ReadVector3d(StreamReader stream)
            {
                string[] vector3dArray = stream.ReadLine().Split('\t');
                return new RealVector3d(float.Parse(vector3dArray[0]), float.Parse(vector3dArray[1]), float.Parse(vector3dArray[2]));
            }

            public RealPoint3d ReadPoint3d(StreamReader stream)
            {
                string[] point3dArray = stream.ReadLine().Split('\t');
                return new RealPoint3d(float.Parse(point3dArray[0]), float.Parse(point3dArray[1]), float.Parse(point3dArray[2]));
            }
        }
    }
}
