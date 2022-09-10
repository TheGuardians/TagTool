using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;

/*
https://github.com/kellygravelyn/Triangulator
The MIT License (MIT)

Copyright(c) 2008, Kelly Gravelyn

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

namespace TagTool.Geometry.Utils
{  
    internal class Triangulator
    {
        static readonly IndexableCyclicalLinkedList<Vertex> polygonVertices = new IndexableCyclicalLinkedList<Vertex>();
        static readonly IndexableCyclicalLinkedList<Vertex> earVertices = new IndexableCyclicalLinkedList<Vertex>();
        static readonly CyclicalList<Vertex> convexVertices = new CyclicalList<Vertex>();
        static readonly CyclicalList<Vertex> reflexVertices = new CyclicalList<Vertex>();

        public static List<List<RealPoint3d>> Triangulate(List<RealPoint3d> polygon, RealPlane3d plane)
        {
            List<List<RealPoint3d>> triangles = new List<List<RealPoint3d>>();
            List<Vector2> points2d = new List<Vector2>();
            foreach (var point in polygon)
                points2d.Add(point2d_from_point3d(point, plane));
            int[] Indices = new int[0];
            Vector2[] outputPoints = new Vector2[0];
            Triangulate2d(points2d.ToArray(), WindingOrder.Clockwise, out outputPoints, out Indices);
            for(var i = 0; i < Indices.Length; i += 3)
            {
                List<RealPoint3d> newPoly = new List<RealPoint3d>();
                newPoly.Add(point2d_and_plane_to_point3d(plane, new RealPoint2d(outputPoints[Indices[i]].X, outputPoints[Indices[i]].Y)));
                newPoly.Add(point2d_and_plane_to_point3d(plane, new RealPoint2d(outputPoints[Indices[i + 1]].X, outputPoints[Indices[i + 1]].Y)));
                newPoly.Add(point2d_and_plane_to_point3d(plane, new RealPoint2d(outputPoints[Indices[i + 2]].X, outputPoints[Indices[i + 2]].Y)));
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

        public static void Triangulate2d(
            Vector2[] inputVertices,
            WindingOrder desiredWindingOrder,
            out Vector2[] outputVertices,
            out int[] indices)
        {
            List<Triangle> triangles = new List<Triangle>();

            //make sure we have our vertices wound properly
            if (DetermineWindingOrder(inputVertices) == WindingOrder.Clockwise)
                outputVertices = ReverseWindingOrder(inputVertices);
            else
                outputVertices = (Vector2[])inputVertices.Clone();

            //clear all of the lists
            polygonVertices.Clear();
            earVertices.Clear();
            convexVertices.Clear();
            reflexVertices.Clear();

            //generate the cyclical list of vertices in the polygon
            for (int i = 0; i < outputVertices.Length; i++)
                polygonVertices.AddLast(new Vertex(outputVertices[i], i));

            //categorize all of the vertices as convex, reflex, and ear
            FindConvexAndReflexVertices();
            FindEarVertices();

            //clip all the ear vertices
            while (polygonVertices.Count > 3 && earVertices.Count > 0)
                ClipNextEar(triangles);

            //if there are still three points, use that for the last triangle
            if (polygonVertices.Count == 3)
                triangles.Add(new Triangle(
                    polygonVertices[0].Value,
                    polygonVertices[1].Value,
                    polygonVertices[2].Value));

            //add all of the triangle indices to the output array
            indices = new int[triangles.Count * 3];

            //move the if statement out of the loop to prevent all the
            //redundant comparisons
            if (desiredWindingOrder == WindingOrder.CounterClockwise)
            {
                for (int i = 0; i < triangles.Count; i++)
                {
                    indices[(i * 3)] = triangles[i].A.Index;
                    indices[(i * 3) + 1] = triangles[i].B.Index;
                    indices[(i * 3) + 2] = triangles[i].C.Index;
                }
            }
            else
            {
                for (int i = 0; i < triangles.Count; i++)
                {
                    indices[(i * 3)] = triangles[i].C.Index;
                    indices[(i * 3) + 1] = triangles[i].B.Index;
                    indices[(i * 3) + 2] = triangles[i].A.Index;
                }
            }
        }

        struct Triangle
        {
            public readonly Vertex A;
            public readonly Vertex B;
            public readonly Vertex C;

            public Triangle(Vertex a, Vertex b, Vertex c)
            {
                A = a;
                B = b;
                C = c;
            }

            public bool ContainsPoint(Vertex point)
            {
                //return true if the point to test is one of the vertices
                if (point.Equals(A) || point.Equals(B) || point.Equals(C))
                    return true;

                bool oddNodes = false;

                if (checkPointToSegment(C, A, point))
                    oddNodes = !oddNodes;
                if (checkPointToSegment(A, B, point))
                    oddNodes = !oddNodes;
                if (checkPointToSegment(B, C, point))
                    oddNodes = !oddNodes;

                return oddNodes;
            }

            public static bool ContainsPoint(Vertex a, Vertex b, Vertex c, Vertex point)
            {
                return new Triangle(a, b, c).ContainsPoint(point);
            }

            static bool checkPointToSegment(Vertex sA, Vertex sB, Vertex point)
            {
                if ((sA.Position.Y < point.Position.Y && sB.Position.Y >= point.Position.Y) ||
                    (sB.Position.Y < point.Position.Y && sA.Position.Y >= point.Position.Y))
                {
                    float x =
                        sA.Position.X +
                        (point.Position.Y - sA.Position.Y) /
                        (sB.Position.Y - sA.Position.Y) *
                        (sB.Position.X - sA.Position.X);

                    if (x < point.Position.X)
                        return true;
                }

                return false;
            }

            public override bool Equals(object obj)
            {
                if (obj.GetType() != typeof(Triangle))
                    return false;
                return Equals((Triangle)obj);
            }

            public bool Equals(Triangle obj)
            {
                return obj.A.Equals(A) && obj.B.Equals(B) && obj.C.Equals(C);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int result = A.GetHashCode();
                    result = (result * 397) ^ B.GetHashCode();
                    result = (result * 397) ^ C.GetHashCode();
                    return result;
                }
            }
        }

        struct Vertex
        {
            public readonly Vector2 Position;
            public readonly int Index;

            public Vertex(Vector2 position, int index)
            {
                Position = position;
                Index = index;
            }

            public override bool Equals(object obj)
            {
                if (obj.GetType() != typeof(Vertex))
                    return false;
                return Equals((Vertex)obj);
            }

            public bool Equals(Vertex obj)
            {
                return obj.Position.Equals(Position) && obj.Index == Index;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Position.GetHashCode() * 397) ^ Index;
                }
            }

            public override string ToString()
            {
                return string.Format("{0} ({1})", Position, Index);
            }
        }

        /// <summary>
        /// Specifies a desired winding order for the shape vertices.
        /// </summary>
        public enum WindingOrder
        {
            Clockwise,
            CounterClockwise
        }

        /// <summary>
        /// Implements a List structure as a cyclical list where indices are wrapped.
        /// </summary>
        /// <typeparam name="T">The Type to hold in the list.</typeparam>
        class CyclicalList<T> : List<T>
        {
            public new T this[int index]
            {
                get
                {
                    //perform the index wrapping
                    while (index < 0)
                        index = Count + index;
                    if (index >= Count)
                        index %= Count;

                    return base[index];
                }
                set
                {
                    //perform the index wrapping
                    while (index < 0)
                        index = Count + index;
                    if (index >= Count)
                        index %= Count;

                    base[index] = value;
                }
            }

            public CyclicalList() { }

            public CyclicalList(IEnumerable<T> collection)
                : base(collection)
            {
            }

            public new void RemoveAt(int index)
            {
                Remove(this[index]);
            }
        }

        /// <summary>
        /// Ensures that a set of vertices are wound in a particular order, reversing them if necessary.
        /// </summary>
        /// <param name="vertices">The vertices of the polygon.</param>
        /// <param name="windingOrder">The desired winding order.</param>
        /// <returns>A new set of vertices if the winding order didn't match; otherwise the original set.</returns>
        public static Vector2[] EnsureWindingOrder(Vector2[] vertices, WindingOrder windingOrder)
        {
            if (DetermineWindingOrder(vertices) != windingOrder)
            {
                return ReverseWindingOrder(vertices);
            }
            return vertices;
        }

        /// <summary>
		/// Reverses the winding order for a set of vertices.
		/// </summary>
		/// <param name="vertices">The vertices of the polygon.</param>
		/// <returns>The new vertices for the polygon with the opposite winding order.</returns>
		public static Vector2[] ReverseWindingOrder(Vector2[] vertices)
        {
            Vector2[] newVerts = new Vector2[vertices.Length];

            newVerts[0] = vertices[0];
            for (int i = 1; i < newVerts.Length; i++)
                newVerts[i] = vertices[vertices.Length - i];

            return newVerts;
        }

        /// <summary>
		/// Determines the winding order of a polygon given a set of vertices.
		/// </summary>
		/// <param name="vertices">The vertices of the polygon.</param>
		/// <returns>The calculated winding order of the polygon.</returns>
		public static WindingOrder DetermineWindingOrder(Vector2[] vertices)
        {
            int clockWiseCount = 0;
            int counterClockWiseCount = 0;
            Vector2 p1 = vertices[0];

            for (int i = 1; i < vertices.Length; i++)
            {
                Vector2 p2 = vertices[i];
                Vector2 p3 = vertices[(i + 1) % vertices.Length];

                Vector2 e1 = p1 - p2;
                Vector2 e2 = p3 - p2;

                if (e1.X * e2.Y - e1.Y * e2.X >= 0)
                    clockWiseCount++;
                else
                    counterClockWiseCount++;

                p1 = p2;
            }

            return (clockWiseCount > counterClockWiseCount)
                ? WindingOrder.Clockwise
                : WindingOrder.CounterClockwise;
        }

        /// <summary>
        /// Implements a LinkedList that is both indexable as well as cyclical. Thus
        /// indexing into the list with an out-of-bounds index will automatically cycle
        /// around the list to find a valid node.
        /// </summary>
        class IndexableCyclicalLinkedList<T> : LinkedList<T>
        {
            /// <summary>
            /// Gets the LinkedListNode at a particular index.
            /// </summary>
            /// <param name="index">The index of the node to retrieve.</param>
            /// <returns>The LinkedListNode found at the index given.</returns>
            public LinkedListNode<T> this[int index]
            {
                get
                {
                    //perform the index wrapping
                    while (index < 0)
                        index = Count + index;
                    if (index >= Count)
                        index %= Count;

                    //find the proper node
                    LinkedListNode<T> node = First;
                    for (int i = 0; i < index; i++)
                        node = node.Next;

                    return node;
                }
            }

            /// <summary>
            /// Removes the node at a given index.
            /// </summary>
            /// <param name="index">The index of the node to remove.</param>
            public void RemoveAt(int index)
            {
                Remove(this[index]);
            }

            /// <summary>
            /// Finds the index of a given item.
            /// </summary>
            /// <param name="item">The item to find.</param>
            /// <returns>The index of the item if found; -1 if the item is not found.</returns>
            public int IndexOf(T item)
            {
                for (int i = 0; i < Count; i++)
                    if (this[i].Value.Equals(item))
                        return i;

                return -1;
            }
        }

        private static void ClipNextEar(ICollection<Triangle> triangles)
        {
            //find the triangle
            Vertex ear = earVertices[0].Value;
            Vertex prev = polygonVertices[polygonVertices.IndexOf(ear) - 1].Value;
            Vertex next = polygonVertices[polygonVertices.IndexOf(ear) + 1].Value;
            triangles.Add(new Triangle(ear, next, prev));

            //remove the ear from the shape
            earVertices.RemoveAt(0);
            polygonVertices.RemoveAt(polygonVertices.IndexOf(ear));

            //validate the neighboring vertices
            ValidateAdjacentVertex(prev);
            ValidateAdjacentVertex(next);
        }

        private static void ValidateAdjacentVertex(Vertex vertex)
        {
            if (reflexVertices.Contains(vertex))
            {
                if (IsConvex(vertex))
                {
                    reflexVertices.Remove(vertex);
                    convexVertices.Add(vertex);
                }
            }

            if (convexVertices.Contains(vertex))
            {
                bool wasEar = earVertices.Contains(vertex);
                bool isEar = IsEar(vertex);

                if (wasEar && !isEar)
                {
                    earVertices.Remove(vertex);
                }
                else if (!wasEar && isEar)
                {
                    earVertices.AddFirst(vertex);
                }
            }
        }

        private static void FindConvexAndReflexVertices()
        {
            for (int i = 0; i < polygonVertices.Count; i++)
            {
                Vertex v = polygonVertices[i].Value;

                if (IsConvex(v))
                {
                    convexVertices.Add(v);
                }
                else
                {
                    reflexVertices.Add(v);
                }
            }
        }

        private static void FindEarVertices()
        {
            for (int i = 0; i < convexVertices.Count; i++)
            {
                Vertex c = convexVertices[i];

                if (IsEar(c))
                {
                    earVertices.AddLast(c);
                }
            }
        }

        private static bool IsEar(Vertex c)
        {
            Vertex p = polygonVertices[polygonVertices.IndexOf(c) - 1].Value;
            Vertex n = polygonVertices[polygonVertices.IndexOf(c) + 1].Value;

            foreach (Vertex t in reflexVertices)
            {
                if (t.Equals(p) || t.Equals(c) || t.Equals(n))
                    continue;

                if (Triangle.ContainsPoint(p, c, n, t))
                {
                    return false;
                }
            }

            return true;
        }
        private static bool IsConvex(Vertex c)
        {
            Vertex p = polygonVertices[polygonVertices.IndexOf(c) - 1].Value;
            Vertex n = polygonVertices[polygonVertices.IndexOf(c) + 1].Value;

            Vector2 d1 = Vector2.Normalize(c.Position - p.Position);
            Vector2 d2 = Vector2.Normalize(n.Position - c.Position);
            Vector2 n2 = new Vector2(-d2.Y, d2.X);

            return (Vector2.Dot(d1, n2) <= 0f);
        }
    }
}
