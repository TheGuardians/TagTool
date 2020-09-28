using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using TagTool.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using System.Diagnostics;

namespace TagTool.Commands.CollisionModels
{
    public class GenerateBspPhysicsCommand : Command
    {
        public CollisionGeometry Bsp { get; set; }
        public CollisionModel Definition { get; set; }

        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        static extern int LoadLibrary(
            [MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        static extern IntPtr GetProcAddress(int hModule,
            [MarshalAs(UnmanagedType.LPStr)] string lpProcName);
        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        static extern bool FreeLibrary(int hModule);

        public const string dllpath = @"Tools\NifMopp.dll";

        //[DllImport(dllpath, CharSet = CharSet.Auto)]
        //public unsafe static extern int fnGenerateMoppCode(int nVerts, [MarshalAs(UnmanagedType.LPArray)] Vector3[] verts, int nTris, [MarshalAs(UnmanagedType.LPArray)] Triangle[] tris);
        //[DllImport(dllpath, CharSet = CharSet.Auto)]
        //public unsafe static extern int fnRetrieveMoppCode(int nBuffer, [MarshalAs(UnmanagedType.LPArray)] byte[] code);
        //[DllImport(dllpath, CharSet = CharSet.Auto)]
        //public unsafe static extern int fnRetrieveMoppScale(float* value);
        //[DllImport(dllpath, CharSet = CharSet.Auto)]
        //public unsafe static extern int fnRetrieveMoppOrigin(Vector3* value);

        public GenerateBspPhysicsCommand(ref CollisionModel definition) :
            base(true,

                "GenerateBspPhysics",
                "Generates BspPhysics for the current collision tag, removing the previous BspPhysics",

                "GenerateBspPhysics",

                "")
        {
            Definition = definition;
            Bsp = new CollisionGeometry();
        }
        public override object Execute(List<string> args)
        {
            CollisionModel.Region.Permutation Permutation = Definition.Regions[0].Permutations[0];
            Bsp = Definition.Regions[0].Permutations[0].Bsps[0].Geometry;
            Console.WriteLine("NOOOO you can't just generate mopps with that shitty code! (begin mopp generation)");
            if (!generate_mopp_codes(ref Permutation))
            {
                Console.WriteLine("ERROR: Failed to build mopps!");
                return false;
            }
            else
            {
                Console.WriteLine("HAHA mopps go BRRRRRRR (Mopps built successfully!)");
                Definition.Regions[0].Permutations[0] = Permutation;
            }           
            return true;
        }

        public struct Vector3
        {
            public float a;
            public float b;
            public float c;
        }
        public struct Triangle
        {
            public short a;
            public short b;
            public short c;
        }

        public unsafe delegate int fnGenerateMoppCode(int nVerts, [MarshalAs(UnmanagedType.LPArray)] Vector3[] verts, int nTris, [MarshalAs(UnmanagedType.LPArray)] Triangle[] tris);
        public unsafe delegate int fnRetrieveMoppCode(int nBuffer, [MarshalAs(UnmanagedType.LPArray)] byte[] code);
        public unsafe delegate int fnRetrieveMoppScale(float* value);
        public unsafe delegate int fnRetrieveMoppOrigin(Vector3* value);

        unsafe public bool generate_mopp_codes(ref CollisionModel.Region.Permutation Permutation)
        {
            if(IntPtr.Size == 8)
            {
                Console.WriteLine("ERROR: Mopp generation can only be performed with x86 Tagtool!!");
                return false;
            }

            int hModule = LoadLibrary(dllpath);
            if (hModule == 0) 
            {
                Console.WriteLine("ERROR: Could not load NifMopp.dll!");
                return false;
            }

            IntPtr GenerateMoppCode = GetProcAddress(hModule, "GenerateMoppCode");
            IntPtr RetrieveMoppCode = GetProcAddress(hModule, "RetrieveMoppCode");
            IntPtr RetrieveMoppScale = GetProcAddress(hModule, "RetrieveMoppScale");
            IntPtr RetrieveMoppOrigin = GetProcAddress(hModule, "RetrieveMoppOrigin");

            if (GenerateMoppCode == null || RetrieveMoppCode == null || RetrieveMoppScale == null || RetrieveMoppOrigin == null)
                return false;

            fnGenerateMoppCode fnGenerateMoppCode_delegate = (fnGenerateMoppCode)Marshal.GetDelegateForFunctionPointer(GenerateMoppCode, typeof(fnGenerateMoppCode));
            fnRetrieveMoppCode fnRetrieveMoppCode_delegate = (fnRetrieveMoppCode)Marshal.GetDelegateForFunctionPointer(RetrieveMoppCode, typeof(fnRetrieveMoppCode));
            fnRetrieveMoppScale fnRetrieveMoppScale_delegate = (fnRetrieveMoppScale)Marshal.GetDelegateForFunctionPointer(RetrieveMoppScale, typeof(fnRetrieveMoppScale));
            fnRetrieveMoppOrigin fnRetrieveMoppOrigin_delegate = (fnRetrieveMoppOrigin)Marshal.GetDelegateForFunctionPointer(RetrieveMoppOrigin, typeof(fnRetrieveMoppOrigin));

            int nv = Bsp.Vertices.Count;
            Vector3[] v = new Vector3[nv];
            int nt = Bsp.Surfaces.Count;
            Triangle[] t = new Triangle[nt];

            for(int vertex_index = 0; vertex_index < Bsp.Vertices.Count; vertex_index++)
            {
                v[vertex_index] = new Vector3 { a = Bsp.Vertices[vertex_index].Point.X, b = Bsp.Vertices[vertex_index].Point.Y, c = Bsp.Vertices[vertex_index].Point.Z };
            }

            for(int surface_index = 0; surface_index < Bsp.Surfaces.Count; surface_index++)
            {
                List<int> vertex_indices = surface_collect_vertices(surface_index);
                if (vertex_indices.Count > 3)
                {
                    Console.WriteLine("ERROR: Mopp generation only accepts triangles!!");
                    return false;
                }
                t[surface_index] = new Triangle { a = (short)vertex_indices[0], b = (short)vertex_indices[1], c = (short)vertex_indices[2] };                
            }

            int len = fnGenerateMoppCode_delegate(nv, v, nt, t);
            float scale = 0.0f;
            Vector3 origin = new Vector3();
            byte[] code = new byte[0];
            if (len > 0)
            {
                code = new byte[len];
                if (0 != fnRetrieveMoppCode_delegate(len, code))
                {
                    fnRetrieveMoppScale_delegate(&scale);
                    fnRetrieveMoppOrigin_delegate(&origin);
                }
            }
            else
            {
                Console.WriteLine("No moppcodes produced!");
                return false;
            }

            FreeLibrary(hModule);

            //populate fields with the appropriate values
            Permutation.BspMoppCodes = new List<Havok.TagHkpMoppCode> { new Havok.TagHkpMoppCode { Data = new TagBlock<byte>() } };
            Permutation.BspPhysics = new List<CollisionBspPhysicsDefinition> { new CollisionBspPhysicsDefinition() };

            foreach(byte moppcode in code)
            {
                Permutation.BspMoppCodes[0].Data.Add(moppcode);
            }

            Permutation.BspMoppCodes[0].Info = new Havok.CodeInfo { Offset = new RealQuaternion(origin.a, origin.b, origin.c, scale) };
            Permutation.BspMoppCodes[0].ArrayBase = new Havok.HkArrayBase { Size = (uint)len, CapacityAndFlags = (uint)(len | 0x80000000)};
            Permutation.BspMoppCodes[0].ReferencedObject = new Havok.HkpReferencedObject { ReferenceCount = 128 };

            Permutation.BspPhysics[0].MoppBvTreeShape = new Havok.CMoppBvTreeShape
            {
                ReferencedObject = new Havok.HkpReferencedObject { ReferenceCount = 128 },
                Type = 27
            };

            List<RealQuaternion> AABB_parameters = get_model_half_extents();

            Permutation.BspPhysics[0].GeometryShape = new CollisionGeometryShape
            {
                Type = 2,
                BspIndex = -1,
                CollisionGeometryShapeKey = 65535,
                AABB_Center = AABB_parameters[0],
                AABB_Half_Extents = AABB_parameters[1]
            };
                
            return true;
        }

        List<RealQuaternion> get_model_half_extents()
        {

            RealPoint3d mincoords = new RealPoint3d(float.MaxValue, float.MaxValue, float.MaxValue);
            RealPoint3d maxcoords = new RealPoint3d(float.MinValue, float.MinValue, float.MinValue);

            foreach(Vertex vertex in Bsp.Vertices)
            {
                RealPoint3d point = vertex.Point;
                if (point.X < mincoords.X)
                    mincoords.X = point.X;
                if (point.X > maxcoords.X)
                    maxcoords.X = point.X;
                if (point.Y < mincoords.Y)
                    mincoords.Y = point.Y;
                if (point.Y > maxcoords.Y)
                    maxcoords.Y = point.Y;
                if (point.Z < mincoords.Z)
                    mincoords.Z = point.Z;
                if (point.Z > maxcoords.Z)
                    maxcoords.Z = point.Z;
            }

            RealPoint3d scale = new RealPoint3d(maxcoords.X - mincoords.X, maxcoords.Y - mincoords.Y, maxcoords.Z - mincoords.Z);

            List<RealQuaternion> AABB_parameters = new List<RealQuaternion>();
            RealQuaternion half_extents = new RealQuaternion(scale.X / 2, scale.Y / 2, scale.Z / 2);
            RealQuaternion center = new RealQuaternion(maxcoords.X - half_extents.I, maxcoords.Y - half_extents.J, maxcoords.Z - half_extents.K);

            AABB_parameters.Add(center);
            AABB_parameters.Add(half_extents);
            return AABB_parameters;
        }

        List<int> surface_collect_vertices(int surface_index)
        {
            List<int> vertex_index_list = new List<int>();
            Surface surface_block = Bsp.Surfaces[surface_index];
            int first_Edge_index = surface_block.FirstEdge;
            int current_edge_index = surface_block.FirstEdge;
            do
            {
                Edge edge_block = Bsp.Edges[current_edge_index];
                if (edge_block.RightSurface == surface_index)
                {
                    current_edge_index = edge_block.ReverseEdge;
                    vertex_index_list.Add(edge_block.EndVertex);
                }
                else
                {
                    current_edge_index = edge_block.ForwardEdge;
                    vertex_index_list.Add(edge_block.StartVertex);
                }
            }
            while (current_edge_index != first_Edge_index);
            return vertex_index_list;
        }
    }
}