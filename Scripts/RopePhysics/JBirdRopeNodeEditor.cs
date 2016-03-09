using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JBirdEngine {

	namespace RopePhysics {

        #if UNITY_EDITOR
        public static class AnchorLock {
			public static bool lockAnchors;
			public static Tool prevTool;
		}

		[CustomEditor(typeof(JBirdRopeNode))]
		public class JBirdRopeNodeEditor : Editor {

			JBirdRopeNode targetNode;

			Vector3 pos;

			public override void OnInspectorGUI (){
				targetNode = (JBirdRopeNode)target;
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("The rope that this segment is part of:", EditorStyles.boldLabel);
				EditorGUILayout.ObjectField("Parent Rope", targetNode.rope, typeof(JBirdRope), true);
				EditorGUILayout.Space();
				base.OnInspectorGUI ();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Check to lock anchors during translation:", EditorStyles.boldLabel);
				AnchorLock.lockAnchors = EditorGUILayout.Toggle("Lock Anchors?", AnchorLock.lockAnchors);
			}

			void OnSceneGUI () {
				targetNode = (JBirdRopeNode)target;
				if (Tools.current != Tool.None) {
					AnchorLock.prevTool = Tools.current;
				}
				if (Tools.current == Tool.Move || Tools.current == Tool.None) {
					Tools.current = Tool.None;
					pos = Handles.PositionHandle(targetNode.transform.position, Tools.handleRotation);
					targetNode.PullRaw(pos, !AnchorLock.lockAnchors);
					if (targetNode.rope.equalizeTension) {
						targetNode.rope.EqualizeTension();
					}
					if (targetNode.rope.reorientSegments) {
						targetNode.rope.ReorientSegments();
					}
					targetNode.rope.segmentList[0].UpdateRopeHead();
				}
			}

			void OnDisable () {
				Tools.current = AnchorLock.prevTool;
			}

		}
		#endif

	}

}
