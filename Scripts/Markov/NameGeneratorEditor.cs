using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(JBirdNameGenerator))]
public class NameGeneratorEditor : Editor {

	JBirdNameGenerator targetNameGen;
	string newName;
	string outputName;
	int minLength = 3;
	int maxLength = 10;
	bool removeAll = false;
	bool clearSave = false;

	public override void OnInspectorGUI () {
		targetNameGen = (JBirdNameGenerator)target;
		newName = EditorGUILayout.TextField("Name:", newName);
		if (GUILayout.Button("Add Name")) {
			targetNameGen.nameGenerator.AddName(newName);
			removeAll = false;
			clearSave = false;
			EditorUtility.SetDirty(target);
		}
		if (GUILayout.Button("Remove Name")) {
			targetNameGen.nameGenerator.RemoveName(newName);
			removeAll = false;
			clearSave = false;
			EditorUtility.SetDirty(target);
		}
		GUILayout.Space(15f);
		minLength = EditorGUILayout.IntField("Minimum Length:", minLength);
		maxLength = EditorGUILayout.IntField("Maximum Length:", maxLength);
		if (GUILayout.Button("Generate!")) {
			outputName = targetNameGen.nameGenerator.GenerateName(minLength, maxLength);
			removeAll = false;
			clearSave = false;
		}
		EditorGUILayout.TextField("Output:", outputName);
		if (GUILayout.Button("Save Output")) {
			if (outputName != "") {
				targetNameGen.nameGenerator.SaveName(outputName);
				EditorUtility.SetDirty(target);
			}
			else {
				Debug.LogWarning("NameGeneratorEditor: Output field is empty!");
			}
			removeAll = false;
			clearSave = false;
		}
		GUILayout.Space (15f);
		if (!removeAll) {
			if (GUILayout.Button("Remove All Names")) {
				removeAll = true;
				clearSave = false;
			}
		}
		if (removeAll) {
			if (GUILayout.Button("Click again to confirm.")) {
				targetNameGen.nameGenerator.RemoveAllNames();
				removeAll = false;
				clearSave = false;
				EditorUtility.SetDirty(target);
			}
		}
		if (!clearSave) {
			if (GUILayout.Button("Clear Saved Names")) {
				removeAll = false;
				clearSave = true;
			}
		}
		if (clearSave) {
			if (GUILayout.Button("Click again to confirm.")) {
				targetNameGen.nameGenerator.ClearSavedNames();
				removeAll = false;
				clearSave = false;
				EditorUtility.SetDirty(target);
			}
		}
		GUILayout.Space(30f);
		base.OnInspectorGUI ();
	}

}
#endif
