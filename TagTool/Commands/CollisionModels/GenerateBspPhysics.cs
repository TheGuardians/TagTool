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
    public class GenerateBspPhysics
    {
        public CollisionGeometry Bsp { get; set; }

        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary")]
        static extern int LoadLibrary(
            [MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        static extern IntPtr GetProcAddress(int hModule,
            [MarshalAs(UnmanagedType.LPStr)] string lpProcName);
        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary")]
        static extern bool FreeLibrary(int hModule);

        public const string dllpath = @"C:\Users\Hari\Documents\My Games\Halo_Online\Mopp\NifMopp\NifMopp.dll";

        //[DllImport(dllpath, CharSet = CharSet.Auto)]
        //public unsafe static extern int fnGenerateMoppCode(int nVerts, [MarshalAs(UnmanagedType.LPArray)] Vector3[] verts, int nTris, [MarshalAs(UnmanagedType.LPArray)] Triangle[] tris);
        //[DllImport(dllpath, CharSet = CharSet.Auto)]
        //public unsafe static extern int fnRetrieveMoppCode(int nBuffer, [MarshalAs(UnmanagedType.LPArray)] byte[] code);
        //[DllImport(dllpath, CharSet = CharSet.Auto)]
        //public unsafe static extern int fnRetrieveMoppScale(float* value);
        //[DllImport(dllpath, CharSet = CharSet.Auto)]
        //public unsafe static extern int fnRetrieveMoppOrigin(Vector3* value);

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

        unsafe public bool generate_mopp_codes()
        {
            int hModule = LoadLibrary(dllpath);
            if (hModule == 0) return false;

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
            Vector3 origin;
            byte[] code;
            if (len > 0)
            {
                code = new byte[len];
                if (0 != fnRetrieveMoppCode_delegate(len, code))
                {
                    fnRetrieveMoppScale_delegate(&scale);
                    fnRetrieveMoppOrigin_delegate(&origin);
                }
            }

            FreeLibrary(hModule);
            return true;
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