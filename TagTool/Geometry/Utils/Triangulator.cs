using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using Poly2Tri;
using Poly2Tri.Triangulation.Polygon;

namespace TagTool.Geometry.Utils
{  
    internal class Triangulator
    {
        public static List<List<RealPoint3d>> Triangulate(List<RealPoint3d> polygon, RealPlane3d plane)
        {
            List<List<RealPoint3d>> triangles = new List<List<RealPoint3d>>();
            List<Vector2> points2d = new List<Vector2>();
            foreach (var point in polygon)
                points2d.Add(point2d_from_point3d(point, plane));

            List<PolygonPoint> polygonPoints = new List<PolygonPoint>();
            foreach (Vector2 point in points2d)
            {
                polygonPoints.Add(new PolygonPoint(point.X, point.Y));
            }

            Polygon polygon2d = new Polygon(polygonPoints);
            P2T.Triangulate(polygon2d);

            for(var i = 0; i < polygon2d.Triangles.Count; i += 1)
            {
                List<RealPoint3d> newPoly = new List<RealPoint3d>();
                newPoly.Add(point2d_and_plane_to_point3d(plane, new RealPoint2d((float)polygon2d.Triangles[i].Points[0].X, (float)polygon2d.Triangles[i].Points[0].Y)));
                newPoly.Add(point2d_and_plane_to_point3d(plane, new RealPoint2d((float)polygon2d.Triangles[i].Points[1].X, (float)polygon2d.Triangles[i].Points[1].Y)));
                newPoly.Add(point2d_and_plane_to_point3d(plane, new RealPoint2d((float)polygon2d.Triangles[i].Points[2].X, (float)polygon2d.Triangles[i].Points[2].Y)));
                triangles.Add(newPoly);
            }

            return triangles;
        }

        public static Vector2 point2d_from_point3d(RealPoint3d point, RealPlane3d plane)
        {
            int projection_axis = plane_get_projection_coefficient(plane);
            int projection_sign = plane_get_projection_sign(plane, projection_axis) ? 1 : 0;
            float[] planecoords = new float[3] { plane.I, plane.J, plane.K };
            float[] result_coords = new float[3];
            int v4 = 2 * (projection_sign + 2 * projection_axis);
            List<int> coordinate_list = new List<int> { 2, 1, 1, 2, 0, 2, 2, 0, 1, 0, 0, 1 };
            int vertex_X_coord_index = coordinate_list[v4];
            int vertex_Y_coord_index = coordinate_list[v4 + 1];

            float[] vertex = new float[] { point.X, point.Y, point.Z };
            return new Vector2(vertex[vertex_X_coord_index], vertex[vertex_Y_coord_index]);
        }
        public static RealPoint3d point2d_and_plane_to_point3d(RealPlane3d plane, RealPoint2d point)
        {
            int projection_axis = plane_get_projection_coefficient(plane);
            int projection_sign = plane_get_projection_sign(plane, projection_axis) ? 1 : 0;
            float[] planecoords = new float[3] { plane.I, plane.J, plane.K };
            float[] result_coords = new float[3];
            int v4 = 2 * (projection_sign + 2 * projection_axis);
            List<int> coordinate_list = new List<int> { 2, 1, 1, 2, 0, 2, 2, 0, 1, 0, 0, 1 };
            int vertex_X_coord_index = coordinate_list[v4];
            int vertex_Y_coord_index = coordinate_list[v4 + 1];

            //assign X and Y coords
            result_coords[vertex_X_coord_index] = point.X;
            result_coords[vertex_Y_coord_index] = point.Y;

            //calculate projection axis coordinate
            //otherwise result coord at projection axis defaults to 0.0
            if (Math.Abs(planecoords[projection_axis]) >= 0.00009999999747378752d)
            {
                result_coords[projection_axis] =
                    (plane.D - planecoords[vertex_X_coord_index] * point.X - planecoords[vertex_Y_coord_index] * point.Y) /
                    planecoords[projection_axis];
            }
            return new RealPoint3d(result_coords[0], result_coords[1], result_coords[2]);
        }

        public static int plane_get_projection_coefficient(RealPlane3d plane)
        {
            int minimum_coefficient;
            float plane_I = Math.Abs(plane.I);
            float plane_J = Math.Abs(plane.J);
            float plane_K = Math.Abs(plane.K);
            if (plane_K < plane_J || plane_K < plane_I)
            {
                if (plane_J >= plane_I)
                    minimum_coefficient = 1;
                else
                    minimum_coefficient = 0;
            }
            else
                minimum_coefficient = 2;
            return minimum_coefficient;
        }

        public static bool plane_get_projection_sign(RealPlane3d plane, int projection_axis)
        {
            switch (projection_axis)
            {
                case 0: //x axis
                    return plane.I > 0.0f;
                case 1: //y axis
                    return plane.J > 0.0f;
                case 2: //z axis
                    return plane.K > 0.0f;
            }
            return false;
        }
    }
}
