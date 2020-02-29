using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Common;

namespace TagTool.Geometry.Jms
{
    // REFERENCE: MosesEditingKit https://github.com/Sigmmma/mek/blob/master/notes/jms_file_format.txt

    public class JmsFormat
    {
        private int Version = 8200; // 8200 supports both CE and H2
        private int NodeListChecksum = 3251;

        private uint NodeCount;
        public List<JmsNode> Nodes = new List<JmsNode>();
        private uint MaterialCount;
        public List<JmsMaterial> Materials = new List<JmsMaterial>();
        private uint MarkerCount;
        public List<JmsMarker> Markers = new List<JmsMarker>();
        private uint RegionCount;
        public List<JmsRegion> Regions = new List<JmsRegion>();
        private uint VertexCount;
        public List<JmsVertex> Vertices = new List<JmsVertex>();
        private uint TriangleCount;
        public List<JmsTriangle> Triangles = new List<JmsTriangle>();

        public bool TryRead(FileInfo file)
        {
            try
            {
                Read(file);
            }
            catch (Exception)
            {
                Console.WriteLine("ERROR: Invalid JMS.");
                return false;
            }
            return true;
        }

        public void Read(FileInfo file)
        {
            using (var stream = file.OpenText())
            {
                if (stream.BaseStream.Length > 0)
                {
                    Version = int.Parse(stream.ReadLine());
                    if (Version != 8200) // 8200 supports both CE and H2
                    {
                        Console.WriteLine("ERROR: Invalid JMS");
                        return;
                    }

                    NodeListChecksum = int.Parse(stream.ReadLine());

                    // parse Nodes
                    NodeCount = uint.Parse(stream.ReadLine());
                    for (int i = 0; i < NodeCount; i++)
                    {
                        JmsNode node = new JmsNode
                        {
                            Name = stream.ReadLine(),
                            FirstChildNodeIndex = int.Parse(stream.ReadLine()),
                            SiblingNodeIndex = int.Parse(stream.ReadLine())
                        };

                        // parse rotation quartenion
                        string[] quaternionArray = stream.ReadLine().Split('\t');
                        node.Rotation = new RealQuaternion(float.Parse(quaternionArray[0]), float.Parse(quaternionArray[1]), float.Parse(quaternionArray[2]), float.Parse(quaternionArray[3]));

                        // parse position point
                        string[] point3dArray = stream.ReadLine().Split('\t');
                        node.Position = new RealPoint3d(float.Parse(point3dArray[0]), float.Parse(point3dArray[1]), float.Parse(point3dArray[2]));

                        Nodes.Add(node);
                    }

                    // parse Materials
                    MaterialCount = uint.Parse(stream.ReadLine());
                    for (int i = 0; i < MaterialCount; i++)
                    {
                        JmsMaterial material = new JmsMaterial
                        {
                            Name = stream.ReadLine(),
                            TifFilePath = stream.ReadLine()
                        };

                        Materials.Add(material);
                    }

                    // parse Markers
                    MarkerCount = uint.Parse(stream.ReadLine());
                    for (int i = 0; i < MarkerCount; i++)
                    {
                        JmsMarker marker = new JmsMarker
                        {
                            Name = stream.ReadLine(),
                            Region = int.Parse(stream.ReadLine()),
                            ParentNode = int.Parse(stream.ReadLine())
                        };

                        // parse rotation quarternion
                        string[] quaternionArray = stream.ReadLine().Split('\t');
                        marker.Rotation = new RealQuaternion(float.Parse(quaternionArray[0]), float.Parse(quaternionArray[1]), float.Parse(quaternionArray[2]), float.Parse(quaternionArray[3]));

                        // parse position point
                        string[] point3dArray = stream.ReadLine().Split('\t');
                        marker.Position = new RealPoint3d(float.Parse(point3dArray[0]), float.Parse(point3dArray[1]), float.Parse(point3dArray[2]));

                        Markers.Add(marker);
                    }

                    // parse Regions
                    RegionCount = uint.Parse(stream.ReadLine());
                    for (int i = 0; i < RegionCount; i++)
                    {
                        JmsRegion region = new JmsRegion
                        {
                            Name = stream.ReadLine()
                        };

                        Regions.Add(region);
                    }

                    // parse Vertices
                    VertexCount = uint.Parse(stream.ReadLine());
                    for (int i = 0; i < VertexCount; i++)
                    {
                        JmsVertex vertex = new JmsVertex();

                        vertex.NodeZeroIndex = int.Parse(stream.ReadLine());

                        // parse position point
                        string[] point3dArray = stream.ReadLine().Split('\t');
                        vertex.Position = new RealPoint3d(float.Parse(point3dArray[0]), float.Parse(point3dArray[1]), float.Parse(point3dArray[2]));

                        // parse vertex normal
                        string[] vector3dArray = stream.ReadLine().Split('\t');
                        vertex.Normal = new RealVector3d(float.Parse(vector3dArray[0]), float.Parse(vector3dArray[1]), float.Parse(vector3dArray[2]));

                        vertex.NodeOneIndex = int.Parse(stream.ReadLine());
                        vertex.NodeOneWeight = float.Parse(stream.ReadLine());

                        // parse uv tex coords
                        string[] texCoord3dArray = stream.ReadLine().Split('\t');
                        vertex.UvTexCoords = new RealVector3d(float.Parse(texCoord3dArray[0]), float.Parse(texCoord3dArray[1]), float.Parse(texCoord3dArray[2]));

                        Vertices.Add(vertex);
                    }

                    // parse Triangles
                    TriangleCount = uint.Parse(stream.ReadLine());
                    for (int i = 0; i < TriangleCount; i++)
                    {
                        JmsTriangle triangle = new JmsTriangle
                        {
                            RegionIndex = int.Parse(stream.ReadLine()),
                            ShaderIndex = int.Parse(stream.ReadLine())
                        };

                        // parse triangle indices
                        string[] indexArray = stream.ReadLine().Split('\t');
                        for (byte index = 0; index < indexArray.Length; index++)
                            triangle.VertexIndices[index] = uint.Parse(indexArray[index]);

                        Triangles.Add(triangle);
                    }
                }
            }
            return;
        }

        public class JmsNode
        {
            public string Name = "default";
            public int FirstChildNodeIndex = -1;
            public int SiblingNodeIndex = -1;
            public RealQuaternion Rotation = new RealQuaternion(0, 0, 0, 0);
            public RealPoint3d Position = new RealPoint3d(0, 0, 0);
        }

        public class JmsMaterial
        {
            public string Name = "default";
            public string TifFilePath = "<none>";
        }

        public class JmsMarker
        {
            public string Name = "default";
            public int Region = -1;
            public int ParentNode = -1;
            public RealQuaternion Rotation = new RealQuaternion(0, 0, 0, 0);
            public RealPoint3d Position = new RealPoint3d(0, 0, 0);
        }

        public class JmsRegion
        {
            public string Name = "default";
        }

        public class JmsVertex
        {
            public int NodeZeroIndex = -1;
            public RealPoint3d Position = new RealPoint3d(0, 0, 0);
            public RealVector3d Normal = new RealVector3d(0, 0, 0);
            public int NodeOneIndex = -1;
            public float NodeOneWeight = -1;
            public RealVector3d UvTexCoords = new RealVector3d(0, 0, 0);
        }

        public class JmsTriangle
        {
            public int RegionIndex = -1;
            public int ShaderIndex = -1;
            public uint[] VertexIndices = new uint[3];
        }
    }
}
