using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry.Utils;
using System.Linq;

namespace TagTool.Commands.StructureDesigns
{
    class ExtractStructureDesignCommand : Command
    {
        private GameCache Cache { get; }
        private StructureDesign Definition { get; }
        public float[,] Bounds = new float[3, 2];
        public List<float> BoundingCoords = new List<float>();

        public ExtractStructureDesignCommand(GameCache cache, StructureDesign definition) :
            base(true,

                "ExtractStructureDesign",
                "",

                "ExtractStructureDesign <OBJ File>",

                "")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            setup_bounding_coords();

            using (var writer = File.CreateText(args[0]))
            {
                //build water polygons from water planes
                List<List<List<RealPoint3d>>> watervolumes = new List<List<List<RealPoint3d>>>();
                foreach(var watervolume in Definition.WaterInstances)
                {
                    List<List<RealPoint3d>> currentvolume = new List<List<RealPoint3d>>();
                    foreach(var plane in watervolume.WaterPlanes)
                    {
                        List<RealPlane3d> planelist = watervolume.WaterPlanes.Select(p => p.Plane).ToList();
                        currentvolume.Add(plane_clip_to_polygon(plane.Plane, planelist));
                    }
                    watervolumes.Add(currentvolume);
                }

                //write soft ceiling vertices
                foreach (var softceiling in Definition.SoftCeilings)
                {
                    foreach(var triangle in softceiling.SoftCeilingTriangles)
                    {
                        writer.WriteLine($"v {triangle.Point1.X} {triangle.Point1.Z} {triangle.Point1.Y}");
                        writer.WriteLine($"v {triangle.Point2.X} {triangle.Point2.Z} {triangle.Point2.Y}");
                        writer.WriteLine($"v {triangle.Point3.X} {triangle.Point3.Z} {triangle.Point3.Y}");
                    }
                }

                //write water volume vertices
                foreach (var watervolume in watervolumes)
                {
                    foreach(var waterpoly in watervolume)
                    {
                        foreach(var watervertex in waterpoly)
                            writer.WriteLine($"v {watervertex.X} {watervertex.Z} {watervertex.Y}");
                    }
                }

                //write soft ceiling faces
                var i = 1;
                foreach (var softceiling in Definition.SoftCeilings)
                {
                    writer.WriteLine($"o {Cache.StringTable.GetString(softceiling.Name)}");
                    foreach (var triangle in softceiling.SoftCeilingTriangles)
                    {
                        writer.WriteLine($"f {i} {i+1} {i+2}");
                        i += 3;
                    }
                }

                //write water volume faces
                for (var watervolumeindex = 0; watervolumeindex < Definition.WaterInstances.Count; watervolumeindex++)
                {
                    var watervolume = watervolumes[watervolumeindex];
                    writer.WriteLine($"o {Cache.StringTable.GetString(Definition.WaterGroups[Definition.WaterInstances[watervolumeindex].WaterNameIndex].Name)}");
                    foreach (var waterpoly in watervolume)
                    {
                        writer.Write("f");
                        foreach (var watervertex in waterpoly)
                            writer.Write($" {i++}");
                        writer.WriteLine();
                    }
                }
            }

            return true;
        }
        public void setup_bounding_coords()
        {
            //set bounds as arbitrarily large, they will be cut down later
            Bounds = new float[,]
            {
                {-1000.0f, 1000.0f },
                {-1000.0f, 1000.0f },
                {-1000.0f, 1000.0f }
            };

            //create a list of 48 floats that consist of 2d points that compose each face of a bounding box surrounding the geometry
            int[,] coordinate_pairs = new int[,]
            { { 2, 1 }, { 1, 2 }, { 0, 2 }, { 2, 0 }, { 1, 0 }, { 0, 1 }  };
            for (var i = 0; i < 6; i++)
            {
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 1]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 1]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 1]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 0], 0]);
                BoundingCoords.Add(Bounds[coordinate_pairs[i, 1], 1]);
            }
        }

        public List<RealPoint3d> plane_clip_to_polygon(RealPlane3d plane, List<RealPlane3d> planelist)
        {
            CollisionBSPGen3Builder gen3builder = new CollisionBSPGen3Builder();
            int projection_axis = gen3builder.plane_get_projection_coefficient(plane);
            int projection_sign = gen3builder.plane_get_projection_sign(plane, projection_axis) ? 1 : 0;

            //build a polygon that exists on the current plane that extends to the maximum bounds
            List<RealPoint3d> vertices = new List<RealPoint3d>();
            for (var i = 0; i < 8; i += 2)
            {
                RealPoint2d point2d = new RealPoint2d
                {
                    X = BoundingCoords[16 * projection_axis + 8 * projection_sign + i],
                    Y = BoundingCoords[16 * projection_axis + 8 * projection_sign + i + 1]
                };
                vertices.Add(gen3builder.point2d_and_plane_to_point3d(plane, point2d));
            }

            //use the other planes to clip this new polygon down to size
            foreach(var temp_plane in planelist)
            {
                if (temp_plane == plane)
                    continue;
                vertices = gen3builder.plane_cut_polygon(vertices, temp_plane, false);
            }

            return vertices;
        }


    }
}