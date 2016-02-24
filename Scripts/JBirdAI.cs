//Comment out the following line if JBirdEngine.Hexagonal is not being used in this project
#define HEXAGONAL

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JBirdEngine {

    namespace AI {

        /// <summary>
        /// Interface for an A*-capable node.
        /// </summary>
        public interface INode<T> where T : Component {

            List<T> GetConnections ();
            float GetG ();
            void SetG (float value);
            float GetH ();
            void SetH (float value);
            float GetF ();
            void ResetGH ();
            T GetCameFrom ();
            void SetCameFrom (T cameFrom);

        }

        /// <summary>
        /// Contains functions to make programming AI easier.
        /// </summary>
        public static class AIHelper {

            public enum HeuristicMode {
                manhattan,
                euclidian,
                hexagonal,
            }

            /// <summary>
            /// Checks if the object is within a given range of the supplied position.
            /// </summary>
            /// <returns><c>true</c>, if the object was within range of position, <c>false</c> otherwise.</returns>
            /// <param name="obj">Object to check.</param>
            /// <param name="position">Position to check against.</param>
            /// <param name="range">Range to check within (inclusive).</param>
            /// <typeparam name="T">Must inherit from Component.</typeparam>
            public static bool WithinRange<T> (T obj, Vector3 position, float range) where T : Component {
                return (Vector3.Distance(obj.transform.position, position) <= range);
            }

            /// <summary>
            /// Uses A* to find a path over a node graph using the INode interface.
            /// </summary>
            /// <returns>A list of nodes that represents the fastest path from start to end.</returns>
            /// <param name="start">Start node.</param>
            /// <param name="end">End node.</param>
            /// <param name="maxDist">Max distance to search from the start node (defaults to Mathf.Infinity).</param>
            /// <param name="mode">Heuristic mode (defaults to Euclidian).</param>
            /// <typeparam name="T">The node type.</typeparam>
            public static List<T> AStar<T> (T start, T end, float maxDist = Mathf.Infinity, HeuristicMode mode = HeuristicMode.euclidian) where T : Component, INode<T> {
                List<T> path = new List<T>();
                List<T> openList = new List<T>();
                List<T> closedList = new List<T>();
                start.SetG(0f);
                start.SetH(GetHeuristic<T>(start, end, mode));
                openList.Add(start);
                while (openList.Count > 0) {
                    T currentNode = NodeWithBestF<T>(openList);
                    if (currentNode == end) {
                        break;
                    }
                    foreach (T node in currentNode.GetConnections()) {
                        if (node == null) {
                            continue;
                        }
                        if (closedList.Contains(node)) {
                            continue;
                        }
                        SetGH(currentNode, node, end, mode);
                        if (node.GetF() < maxDist) {
                            openList.Add(node);
                        }
                    }
                    openList.Remove(currentNode);
                    closedList.Add(currentNode);
                }
                //Get cameFrom list and reverse it
                path.Add(end);
                T nextCameFrom = end.GetCameFrom();
                while (nextCameFrom != null) {
                    path.Add(nextCameFrom);
                    nextCameFrom = nextCameFrom.GetCameFrom();
                }
                path.Reverse();
                //Reset nodes
                foreach (T node in openList) {
                    node.ResetGH();
                }
                foreach (T node in closedList) {
                    node.ResetGH();
                }
                return path;
            }

            /// <summary>
            /// Used by the A* function.
            /// </summary>
            private static void SetGH<T> (T node, T next, T end, HeuristicMode mode) where T : Component, INode<T> {
                next.SetG(Mathf.Min(node.GetG() + Vector3.Distance(node.transform.position, next.transform.position), next.GetG()));
                next.SetH(GetHeuristic(next, end, mode));
                next.SetCameFrom(node);
            }

            /// <summary>
            /// Used by the A* function.
            /// </summary>
            private static T NodeWithBestF<T> (List<T> list) where T : Component, INode<T> {
                float bestF = Mathf.Infinity;
                T bestNode = null;
                foreach (T node in list) {
                    if (node.GetF() < bestF) {
                        bestF = node.GetF();
                        bestNode = node;
                    }
                }
                return bestNode;
            }

            /// <summary>
            /// Returns the predicted distance from start to end based on the given heuristic mode.
            /// </summary>
            /// <param name="start">Start node.</param>
            /// <param name="end">End node.</param>
            /// <param name="mode">Heuristic mode.</param>
            /// <typeparam name="T">The node type.</typeparam>
            public static float GetHeuristic<T> (T start, T end, HeuristicMode mode) where T : Component, INode<T> {
                return GetHeuristic(start.transform.position, end.transform.position, mode);
            }

            /// <summary>
            /// Returns the predicted distance from start to end based on the given heuristic mode.
            /// </summary>
            /// <param name="start">Start position.</param>
            /// <param name="end">End position.</param>
            /// <param name="mode">Heuristic mode.</param>
            public static float GetHeuristic (Vector3 start, Vector3 end, HeuristicMode mode) {
                if (mode == HeuristicMode.euclidian) {
                    return (Vector3.Distance(start, end));
                }
                if (mode == HeuristicMode.manhattan) {
                    return ((Mathf.Abs(end.x - start.x) + Mathf.Abs(end.y - start.y) + Mathf.Abs(end.z - start.z)));
                }

                if (mode == HeuristicMode.hexagonal) {
					#if HEXAGONAL
                    return (Mathf.Abs(Vector3.Dot(end - start, Hexagonal.HexGrid.cornerUpRight)) * Vector3.Distance(end, start) +
                            Mathf.Abs(Vector3.Dot(end - start, Hexagonal.HexGrid.cornerDownRight)) * Vector3.Distance(end, start) +
                            Mathf.Abs(Vector3.Dot(end - start, Hexagonal.HexGrid.cornerLeft)) * Vector3.Distance(end, start)) / 2f;
					#else
					Debug.LogError("JBirdAI: Trying to use hexagonal heuristic, but HEXAGONAL is not defined in the JBirdAI script.");
					return 0f;
					#endif
                }
                else {
                    Debug.LogError("JBirdEngine.AIHelper: Attempting to use unimplemented Heuristic Mode!");
                    return 0f;
                }
            }

        }

    }

}
