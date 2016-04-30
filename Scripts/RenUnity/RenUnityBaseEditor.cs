using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using JBirdEngine.RenUnity;
using UnityEngine.Events;

#if UNITY_EDITOR
[CustomEditor(typeof(RenUnityBase))]
public class RenUnityBaseEditor : Editor {

	RenUnityBase targetBase;
	bool removeCheck;
	int removeCheckI = -1;
	int removeCheckJ = -1;

	public override void OnInspectorGUI () {
		base.OnInspectorGUI();
		targetBase = (RenUnityBase)target;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("RenUnity File Paths:", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Directory for storing JSON files:");
		RenUnityFilePaths.jsonFilePath = targetBase.jsonFilePath = EditorGUILayout.TextField(targetBase.jsonFilePath);
		EditorGUILayout.LabelField("Directory for storing Branch assets:");
		RenUnityFilePaths.branchesFilePath = targetBase.branchesFilePath = EditorGUILayout.TextField(targetBase.branchesFilePath);
		EditorGUILayout.Space();

		EditorGUILayout.Separator();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("RenUnity Variables:", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("Text write speed (in seconds between characters; lower = faster):");
		targetBase.writeSpeed = EditorGUILayout.FloatField("Write Speed:", targetBase.writeSpeed);
		EditorGUILayout.Space();

		EditorGUILayout.Separator();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Conditional Flags (most recent first):", EditorStyles.boldLabel);
		EditorGUILayout.BeginVertical();
		if (targetBase.conditionalFlags.Count == 0) {
			EditorGUILayout.LabelField("[none]");
		}
		GUI.enabled = false;
		for (int i = targetBase.conditionalFlags.Count - 1; i >= 0; i--) {
			EditorGUILayout.TextField(targetBase.conditionalFlags[i]);
		}
		GUI.enabled = true;
		EditorGUILayout.EndVertical();
		EditorGUILayout.Space();

		EditorGUILayout.Separator();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Characters:", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		if (GUILayout.Button("Update Character List")) {
			targetBase.GetCharacters();
		}
		EditorGUILayout.BeginVertical();
		if (targetBase.characterData.Count == 0) {
			EditorGUILayout.LabelField("[none]");
		}
		for (int i =0; i < targetBase.characterData.Count; i++) {
			EditorGUILayout.Space();
			EditorGUILayout.LabelField(string.Format("Name: {0}", targetBase.characterData[i].name), EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical();
			targetBase.characterData[i].defaultPortrait = (Sprite)EditorGUILayout.ObjectField("Default Portrait", targetBase.characterData[i].defaultPortrait, typeof(Sprite), false);
			for (int j = 0; j < targetBase.characterData[i].portraits.Count; j++) {
				targetBase.characterData[i].portraits[j].portrait = (Sprite)EditorGUILayout.ObjectField(string.Format("{0}", targetBase.characterData[i].portraits[j].mood), targetBase.characterData[i].portraits[j].portrait, typeof(Sprite), false);
			}
			for (int j = 0; j < targetBase.characterData[i].stats.Count; j++) {
				targetBase.characterData[i].stats[j].value = EditorGUILayout.FloatField(string.Format("{0}", targetBase.characterData[i].stats[j].stat), targetBase.characterData[i].stats[j].value);
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.Space();

		EditorGUILayout.Separator();

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Story Branch Organizer:", EditorStyles.boldLabel);
		AssetDatabase.Refresh();
		if (StoryBranchOrganizer.singleton == null && targetBase.singletonSBO == null) {
			EditorGUILayout.LabelField("[Create in Assets folder to use]");
		}
		else {
			if (targetBase.singletonSBO != null) {
				StoryBranchOrganizer.singleton = targetBase.singletonSBO;
			}
			else {
				targetBase.singletonSBO = StoryBranchOrganizer.singleton;
				EditorUtility.SetDirty(target);
				EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
				EditorUtility.SetDirty(StoryBranchOrganizer.singleton);
			}
			EditorGUILayout.BeginVertical();
			for (int i = 0; i < StoryBranchOrganizer.singleton.entries.Count; i++) {
				EditorGUILayout.Space();
				if (StoryBranchOrganizer.singleton.entries[i].thisBranch == null) {
					EditorGUILayout.LabelField(string.Format("Index {0} - Branch Name: {1}", i, "N/A"), EditorStyles.boldLabel);
				}
				else {
                    EditorGUILayout.LabelField(string.Format("Index {0} - Branch Name: {1}", i, StoryBranchOrganizer.singleton.entries[i].thisBranch.name), EditorStyles.boldLabel);
				}
				StoryBranchOrganizer.singleton.entries[i].thisBranch = (StoryBranch)EditorGUILayout.ObjectField(StoryBranchOrganizer.singleton.entries[i].thisBranch, typeof(StoryBranch), false);
				if (StoryBranchOrganizer.singleton.entries[i].parentBranch == null) {
					EditorGUILayout.LabelField(string.Format("Parent Branch: {0}", "N/A"));
				}
				else {
					EditorGUILayout.LabelField(string.Format("Parent Branch: {0}", StoryBranchOrganizer.singleton.entries[i].parentBranch.name));
				}
				StoryBranchOrganizer.singleton.entries[i].parentBranch = (StoryBranch)EditorGUILayout.ObjectField(StoryBranchOrganizer.singleton.entries[i].parentBranch, typeof(StoryBranch), false);
				EditorGUILayout.BeginVertical();
				for (int j = 0; j < StoryBranchOrganizer.singleton.entries[i].jumpList.Count; j++) {
					if (StoryBranchOrganizer.singleton.entries[i].jumpList[j] == null) {
						EditorGUILayout.LabelField(string.Format("Branch Index {0}: {1}", j, "N/A"));
					}
					else {
						EditorGUILayout.LabelField(string.Format("Branch Index {0}: {1}", j, StoryBranchOrganizer.singleton.entries[i].jumpList[j].name));
					}
					StoryBranchOrganizer.singleton.entries[i].jumpList[j] = (StoryBranch)EditorGUILayout.ObjectField(StoryBranchOrganizer.singleton.entries[i].jumpList[j], typeof(StoryBranch), false);
					if (!removeCheck || removeCheckI != i || removeCheckJ != j) {
						if (GUILayout.Button("Remove This Jump Branch")) {
							removeCheck = true;
							removeCheckI = i;
							removeCheckJ = j;
						}
					}
					else if (removeCheckI == i && removeCheckJ == j) {
						if (GUILayout.Button("Are you sure?")) {
							StoryBranchOrganizer.singleton.entries[i].jumpList.RemoveAt(j);
							EditorUtility.SetDirty(StoryBranchOrganizer.singleton);
							removeCheck = false;
							removeCheckI = -1;
							removeCheckJ = -1;
						}
					}
				}
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space();
				if (GUILayout.Button("Add Jump Branch")) {
					removeCheck = false;
					removeCheckI = -1;
					removeCheckJ = -1;
					StoryBranch newBranch = StoryBranch.CreateInstance<StoryBranch>();
					StoryBranchOrganizer.singleton.entries[i].jumpList.Add(newBranch);
					EditorUtility.SetDirty(newBranch);
					EditorUtility.SetDirty(StoryBranchOrganizer.singleton);
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
			EditorGUILayout.Space();
			if (GUILayout.Button("Add Entry")) {
				removeCheck = false;
				removeCheckI = -1;
				removeCheckJ = -1;
				StoryBranchOrganizer.singleton.entries.Add(new StoryBranchEntry());
				EditorUtility.SetDirty(StoryBranchOrganizer.singleton);
			}
		}
		EditorUtility.SetDirty(targetBase);
	}

}
#endif