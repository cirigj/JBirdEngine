//Comment out the following line if you aren't using the JBird Color Library
#define COLOR

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using JBirdLib;

#if UNITY_EDITOR
[CustomEditor(typeof(JBirdRope))]
public class JBirdRopeEditor : Editor {

	JBirdRope targetRope;

	Vector3 direction;
	float tension;

	public override void OnInspectorGUI () {
		targetRope = (JBirdRope)target;
		base.OnInspectorGUI ();
		targetRope.UpdateTapering();
		targetRope.UpdateTensionVars();
		targetRope.UpdateAnchors();
		if (targetRope.numberOfSegments < 0) {
			targetRope.numberOfSegments = 0;
		}
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Rope Creation Parameters:", EditorStyles.boldLabel);
		direction = EditorGUILayout.Vector3Field("Direction", direction);
		if (direction == Vector3.zero) {
			direction = Vector3.forward;
		}
		tension = EditorGUILayout.FloatField("Tension", tension);
		if (tension > targetRope.maxTension) {
			tension = targetRope.maxTension;
		}
		if (tension < targetRope.minTension) {
			tension = targetRope.minTension;
		}
		if (targetRope.segmentList.Count == 0) {
			if (GUILayout.Button("Create Rope")) {
				targetRope.CreateRope(direction, tension);
				targetRope.UpdateAnchors();
			}
		}
		else {
			if (GUILayout.Button("Resize")) {
				targetRope.Resize(direction, tension);
				targetRope.UpdateAnchors();
			}
		}
		EditorGUILayout.Space();
		if (GUILayout.Button("Equalize Tension")) {
			targetRope.EqualizeTension();
		}
		#if COLOR
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Color:", EditorStyles.boldLabel);
		targetRope.c1 = EditorGUILayout.ColorField("Start Color", targetRope.c1);
		targetRope.c2 = EditorGUILayout.ColorField("End Color", targetRope.c2);
		EditorGUILayout.LabelField("WARNING: Using gradient will instantiate a material for each segment.", EditorStyles.boldLabel);
		targetRope.useGradient = EditorGUILayout.Toggle("Use Gradient", targetRope.useGradient);
		#endif
	}

}
#endif