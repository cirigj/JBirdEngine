using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

namespace JBirdEngine {

	namespace Hexagonal {

        /// <summary>
        /// Contains functions and properties to assist with implementing a hexagonal grid.
        /// </summary>
        public static class HexGrid {

            public enum ConnectionIndex {
                Down = 0,
                LeftDown = 1,
                LeftUp = 2,
                Up = 3,
                RightUp = 4,
                RightDown = 5
            }

            /// <summary>
            /// Reverses the connection index.
            /// </summary>
            /// <returns>The reversed connection index.</returns>
            /// <param name="index">Index to reverse.</param>
            public static int ReverseConnectionIndex (ConnectionIndex index) {
                return ReverseConnectionIndex((int)index);
            }

            /// <summary>
            /// Reverses the connection index.
            /// </summary>
            /// <returns>The reversed connection index.</returns>
            /// <param name="index">Index to reverse.</param>
            public static int ReverseConnectionIndex (int index) {
                return (index + 3) % 6;
            }

            public class Edge {
                public Vector3 point1, point2;
                public Edge (Vector3 pt1, Vector3 pt2) {
                    point1 = pt1;
                    point2 = pt2;
                }
                public Edge (Edge e) {
                    point1 = e.point1;
                    point2 = e.point2;
                }
            }

            private static Vector3 _cornerRight = new Vector3(1f, 0f, 0f) / Mathf.Sin(60f * Mathf.Deg2Rad);
            private static Vector3 _cornerLeft = new Vector3(-1f, 0f, 0f) / Mathf.Sin(60f * Mathf.Deg2Rad);
            private static Vector3 _cornerUpRight = new Vector3(Mathf.Cos(60f * Mathf.Deg2Rad), 0f, Mathf.Sin(60f * Mathf.Deg2Rad)) / Mathf.Sin(60f * Mathf.Deg2Rad);
            private static Vector3 _cornerUpLeft = new Vector3(-Mathf.Cos(60f * Mathf.Deg2Rad), 0f, Mathf.Sin(60f * Mathf.Deg2Rad)) / Mathf.Sin(60f * Mathf.Deg2Rad);
            private static Vector3 _cornerDownRight = new Vector3(Mathf.Cos(60f * Mathf.Deg2Rad), 0f, -Mathf.Sin(60f * Mathf.Deg2Rad)) / Mathf.Sin(60f * Mathf.Deg2Rad);
            private static Vector3 _cornerDownLeft = new Vector3(-Mathf.Cos(60f * Mathf.Deg2Rad), 0f, -Mathf.Sin(60f * Mathf.Deg2Rad)) / Mathf.Sin(60f * Mathf.Deg2Rad);

            /// <summary>
            /// Right corner of a hex tile.
            /// </summary>
            public static Vector3 cornerRight {
                get { return _cornerRight; }
            }

            /// <summary>
            /// Left corner of a hex tile.
            /// </summary>
            public static Vector3 cornerLeft {
                get { return _cornerLeft; }
            }

            /// <summary>
            /// Top-right corner of a hex tile.
            /// </summary>
            public static Vector3 cornerUpRight {
                get { return _cornerUpRight; }
            }

            /// <summary>
            /// Top-left corner of a hex tile.
            /// </summary>
            public static Vector3 cornerUpLeft {
                get { return _cornerUpLeft; }
            }

            /// <summary>
            /// Bottom-right corner of a hex tile.
            /// </summary>
            public static Vector3 cornerDownRight {
                get { return _cornerDownRight; }
            }

            /// <summary>
            /// Bottom-left corner of a hex tile.
            /// </summary>
            public static Vector3 cornerDownLeft {
                get { return _cornerDownLeft; }
            }

            private static List<Vector3> _corners = ListHelper.ListFromObjects<Vector3>(_cornerDownRight, _cornerDownLeft, _cornerLeft, _cornerUpLeft, _cornerUpRight, _cornerRight);

            /// <summary>
            /// A list of corners of a hex tile (using the connection index on this list returns the corner that is located in a clockwise direction from the specified connection).
            /// </summary>
            public static List<Vector3> corners {
                get { return new List<Vector3>(_corners); }
            }

            private static List<Edge> _edges = ListHelper.ListFromObjects<Edge>(new Edge(_cornerDownRight, _cornerDownLeft),
                                                                                new Edge(_cornerDownLeft, _cornerLeft),
                                                                                new Edge(_cornerLeft, _cornerUpLeft),
                                                                                new Edge(_cornerUpLeft, _cornerUpRight),
                                                                                new Edge(_cornerUpRight, _cornerRight),
                                                                                new Edge(_cornerRight, _cornerDownRight));

            /// <summary>
            /// A list of edges of a hex tile (using the connection index returns the edge perpendicular to the specified connection).
            /// </summary>
            public static List<Edge> edges {
                get { return new List<Edge>(_edges); }
            }

            /// <summary>
            /// Returns the edge perpendicular to the specified connection.
            /// </summary>
            public static Edge GetEdge (ConnectionIndex direction) {
                return new Edge(_edges[(int)direction]);
            }

            /// <summary>
            /// Returns the edge perpendicular to the specified connection as a list of two vectors (the positions of the two corners connection by that edge).
            /// </summary>
            public static List<Vector3> GetEdgeAsVectorList (ConnectionIndex direction) {
                List<Vector3> pointList = new List<Vector3>();
                pointList.Add(edges[(int)direction].point1);
                pointList.Add(edges[(int)direction].point2);
                return pointList;
            }

            public static Mesh hexMesh = CreateHexMesh();

            /// <summary>
            /// Creates the hex mesh and stores it in the assets folder.
            /// </summary>
            private static Mesh CreateHexMesh () {
                Mesh mesh = new Mesh();
                mesh.name = "HexMesh";
                mesh.vertices = new Vector3[] {
				Vector3.zero,
				_cornerDownRight,
				_cornerDownLeft,
				_cornerLeft,
				_cornerUpLeft,
				_cornerUpRight,
				_cornerRight
			};
                Vector2[] uvs = new Vector2[mesh.vertices.Length];
                for (int i = 0; i < mesh.vertices.Length; i++) {
                    uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
                }
                mesh.uv = uvs;
                mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 6, 0, 6, 1 };
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
#if UNITY_EDITOR
            //AssetDatabase.CreateAsset(mesh, "Assets/Meshes/HexMesh.asset");
			//AssetDatabase.SaveAssets();
#endif
                return mesh;
            }

            // Thickness of the hex ring (must be between 0.0f and 1.0f)
            static float outerDistance = 0.98f;
            static float ringThickness = 0.2f;
            public static Mesh hexRingMesh = CreateHexRingMesh();

            /// <summary>
            /// Creates the hex mesh and stores it in the assets folder.
            /// </summary>
            private static Mesh CreateHexRingMesh () {
                Mesh mesh = new Mesh();
                mesh.name = "HexRingMesh";
                mesh.vertices = new Vector3[] {
				_cornerDownRight * outerDistance,
				_cornerDownLeft * outerDistance,
				_cornerLeft * outerDistance,
				_cornerUpLeft * outerDistance,
				_cornerUpRight * outerDistance,
				_cornerRight * outerDistance,
				_cornerDownRight * (outerDistance - ringThickness),
				_cornerDownLeft * (outerDistance - ringThickness),
				_cornerLeft * (outerDistance - ringThickness),
				_cornerUpLeft * (outerDistance - ringThickness),
				_cornerUpRight * (outerDistance- ringThickness),
				_cornerRight * (outerDistance - ringThickness)
			};
                Vector2[] uvs = new Vector2[mesh.vertices.Length];
                for (int i = 0; i < mesh.vertices.Length; i++) {
                    uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].z);
                }
                mesh.uv = uvs;
                mesh.triangles = new int[] { 0, 1, 6, 7, 6, 1, 1, 2, 7, 8, 7, 2, 2, 3, 8, 9, 8, 3, 3, 4, 9, 10, 9, 4, 4, 5, 10, 11, 10, 5, 5, 0, 11, 6, 11, 0 };
                mesh.RecalculateNormals();
                mesh.RecalculateBounds();
			#if UNITY_EDITOR
			//AssetDatabase.CreateAsset(mesh, "Assets/Meshes/HexRingMesh.asset");
			//AssetDatabase.SaveAssets();
			#endif
                return mesh;
            }

            private static Vector3 _linkUp = new Vector3(0f, 0f, 1f);
            private static Vector3 _linkDown = new Vector3(0f, 0f, -1f);
            private static Vector3 _linkRightUp = new Vector3(Mathf.Cos(30f * Mathf.Deg2Rad), 0f, Mathf.Sin(30f * Mathf.Deg2Rad));
            private static Vector3 _linkRightDown = new Vector3(Mathf.Cos(30f * Mathf.Deg2Rad), 0f, -Mathf.Sin(30f * Mathf.Deg2Rad));
            private static Vector3 _linkLeftUp = new Vector3(-Mathf.Cos(30f * Mathf.Deg2Rad), 0f, Mathf.Sin(30f * Mathf.Deg2Rad));
            private static Vector3 _linkLeftDown = new Vector3(-Mathf.Cos(30f * Mathf.Deg2Rad), 0f, -Mathf.Sin(30f * Mathf.Deg2Rad));

            private static List<Vector3> _linkDirections = ListHelper.ListFromObjects<Vector3>(_linkDown, _linkLeftDown, _linkLeftUp, _linkUp, _linkRightUp, _linkRightDown);

            /// <summary>
            /// Returns the list of connection directions between hex tiles.
            /// </summary>
            public static List<Vector3> linkDirections {
                get { return new List<Vector3>(_linkDirections); }
            }

            /// <summary>
            /// Returns the vector from the center of the hex tile to the edge shared by the hex tile in the specified direction.
            /// </summary>
            public static Vector3 GetDirectionVector (ConnectionIndex direction) {
                return _linkDirections[(int)direction];
            }

            /// <summary>
            /// Vector from center of the hex tile to the top edge.
            /// </summary>
            public static Vector3 linkUp {
                get { return _linkUp; }
            }

            /// <summary>
            /// Vector from center of the hex tile to the bottom edge.
            /// </summary>
            public static Vector3 linkDown {
                get { return _linkDown; }
            }

            /// <summary>
            /// Vector from center of the hex tile to the top-right edge.
            /// </summary>
            public static Vector3 linkRightUp {
                get { return _linkRightUp; }
            }

            /// <summary>
            /// Vector from center of the hex tile to the bottom-right edge.
            /// </summary>
            public static Vector3 linkRightDown {
                get { return _linkRightDown; }
            }

            /// <summary>
            /// Vector from center of the hex tile to the top-left edge.
            /// </summary>
            public static Vector3 linkLeftUp {
                get { return _linkLeftUp; }
            }

            /// <summary>
            /// Vector from center of the hex tile to the bottom-left edge.
            /// </summary>
            public static Vector3 linkLeftDown {
                get { return _linkLeftDown; }
            }

            /// <summary>
            /// Returns a vector with the same magnitude as the original vector, but snapped to the closest hex tile link direction.
            /// </summary>
            /// <param name="original">Original vector.</param>
            public static Vector3 SnapToHexDirection (Vector3 original) {
                float originalMagnitude = original.magnitude;
                float bestDotProduct = 0f;
                Vector3 bestVector = Vector3.zero;
                original.Normalize();
                foreach (Vector3 direction in linkDirections) {
                    float dotProduct = Vector3.Dot(original, direction);
                    if (dotProduct > bestDotProduct) {
                        bestDotProduct = dotProduct;
                        bestVector = direction;
                    }
                }
                return bestVector * originalMagnitude;
            }

        }

    }

}

