using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using System.IO;

namespace TagTool.Geometry.BspCollisionGeometry.Utils
{
    class ErrorGeometryBuilder
    {
        //these are used for the error geometry output
        public List<RealPoint3d> Vertices = new List<RealPoint3d>();
        public List<error_geometry> Geometry = new List<error_geometry>();

        public bool WriteOBJ()
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string strWorkPath = Path.GetDirectoryName(strExeFilePath);
            string outputdir = Path.Combine(strWorkPath, "error_geometry.obj");

            if (!WriteMaterialFile())
                return false;

            if (File.Exists(outputdir))
                File.Delete(outputdir);

            Console.WriteLine($"###Collision Geometry import has resulted in one or more errors. " +  
                $"Please see the error geometry output at {outputdir}");

            using (var writer = File.CreateText(outputdir))
            {
                writer.WriteLine("mtllib error_geometry.mtl");

                foreach (RealPoint3d vertex in Vertices)
                {
                    writer.WriteLine($"v {vertex.X * 100.0} {vertex.Y * 100.0} {vertex.Z * 100.0}");
                }

                //sort the error geometry so that it can be grouped in the obj file
                IEnumerable<error_geometry> OpenEdges = Geometry.Where(geo => geo.Type == error_geometry_type.openedge);
                IEnumerable<error_geometry> BadSurfaces = Geometry.Where(geo => geo.Type == error_geometry_type.badsurface);
                IEnumerable<error_geometry> DegenerateEdges = Geometry.Where(geo => geo.Type == error_geometry_type.degenerateedge);
                IEnumerable<error_geometry> IntersectingSurfaces = Geometry.Where(geo => geo.Type == error_geometry_type.intersectingsurface);
                IEnumerable<error_geometry> OverlappingSurfaces = Geometry.Where(geo => geo.Type == error_geometry_type.overlappingsurface);

                if(OpenEdges.Count() > 0)
                {
                    writer.WriteLine("usemtl red");
                    writer.WriteLine("o open_edges");
                    foreach(var entry in OpenEdges)
                    {
                        writer.Write("l"); //line
                        foreach (var index in entry.Indices)
                            writer.Write($" {index}");
                        writer.Write($"\r\n");
                    }
                }
                if (DegenerateEdges.Count() > 0)
                {
                    writer.WriteLine("usemtl blue");
                    writer.WriteLine("o degenerate_edges");
                    foreach (var entry in DegenerateEdges)
                    {
                        writer.Write("l"); //line
                        foreach (var index in entry.Indices)
                            writer.Write($" {index}");
                        writer.Write($"\r\n");
                    }
                }
                if (IntersectingSurfaces.Count() > 0)
                {
                    writer.WriteLine("usemtl pink");
                    writer.WriteLine("o intersecting_surfaces");
                    foreach (var entry in IntersectingSurfaces)
                    {
                        writer.Write("l"); //line
                        foreach (var index in entry.Indices)
                            writer.Write($" {index}");
                        writer.Write($"\r\n");
                    }
                }
                if (OverlappingSurfaces.Count() > 0)
                {
                    writer.WriteLine("usemtl orange");
                    writer.WriteLine("o overlapping_surfaces");
                    foreach (var entry in OverlappingSurfaces)
                    {
                        writer.Write("f"); //face
                        foreach (var index in entry.Indices)
                            writer.Write($" {index}");
                        writer.Write($"\r\n");
                    }
                }
                if (BadSurfaces.Count() > 0)
                {
                    writer.WriteLine("usemtl red");
                    writer.WriteLine("o bad_surfaces");
                    foreach (var entry in BadSurfaces)
                    {
                        writer.Write("f"); //face
                        foreach (var index in entry.Indices)
                            writer.Write($" {index}");
                        writer.Write($"\r\n");
                    }
                }
            }
            return true;
        }

        public bool WriteMaterialFile()
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string strWorkPath = Path.GetDirectoryName(strExeFilePath);
            string outputdir = Path.Combine(strWorkPath, "error_geometry.mtl");

            if (File.Exists(outputdir))
                File.Delete(outputdir);

            using (var writer = File.CreateText(outputdir))
            {
                writer.WriteLine("newmtl red");
                writer.WriteLine("Kd 1.000 0.000 0.000");
                writer.WriteLine("newmtl orange");
                writer.WriteLine("Kd 1.000 0.500 0.000");
                writer.WriteLine("newmtl pink");
                writer.WriteLine("Kd 1.000 0.000 1.000");
                writer.WriteLine("newmtl blue");
                writer.WriteLine("Kd 0.000 0.000 1.000");
            }

            return true;
        }

        public enum error_geometry_type
        {
            openedge,
            degenerateedge,
            intersectingsurface,
            overlappingsurface,
            badsurface
        }

        public struct error_geometry
        {
            public error_geometry_type Type;
            public List<int> Indices;
        }
    }
}
