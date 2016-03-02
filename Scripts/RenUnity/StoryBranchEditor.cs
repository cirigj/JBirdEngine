using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JBirdEngine {

	namespace RenUnity {

        #if UNITY_EDITOR
		public static class RenUnityFilePaths {
			public static string jsonFilePath = "Assets/JBirdEngine/RenUnity/Json/";
			public static string branchesFilePath = "Assets/JBirdEngine/RenUnity/Branches/";
		}

		/// <summary>
		/// Story branch custom inspector class.
		/// </summary>
		[CustomEditor(typeof(StoryBranch))]
		public class StoryBranchEditor : Editor {

			public TextAsset readFile;
			public string writeFileName = "Assets/JBirdEngine/RenUnity/Json/Untitled.txt";
			public string assetFileName = "Assets/JBirdEngine/RenUnity/Branches/Untitled.asset";

			public override void OnInspectorGUI () {

				DrawDefaultInspector();

				StoryBranch editorTarget = (StoryBranch)target;

				if (editorTarget.branch.branchName != string.Empty) {
					writeFileName = string.Format("{0}{1}.txt", RenUnityFilePaths.jsonFilePath, editorTarget.branch.branchName);
					assetFileName = string.Format("{0}{1}.asset", RenUnityFilePaths.branchesFilePath, editorTarget.branch.branchName);
				}

				GUILayout.Space(16);
				if (GUILayout.Button("Read From File")) {
					editorTarget.branch = StoryBranchJsonSerializer.Read(readFile);
					EditorUtility.SetDirty(target);
				}
				readFile = EditorGUILayout.ObjectField("File to read from:", readFile, typeof(TextAsset), true) as TextAsset;

				GUILayout.Space(16);
				if (GUILayout.Button("Write To File")) {
					StoryBranchJsonSerializer.Write(writeFileName, editorTarget.branch);
					EditorUtility.SetDirty(target);
					AssetDatabase.Refresh();
				}
				writeFileName = EditorGUILayout.TextField("File to write to:", writeFileName);

				EditorGUILayout.Space();
				if (GUILayout.Button("Save as Asset")) {
					AssetDatabase.CreateAsset(editorTarget, assetFileName);
					EditorUtility.SetDirty(target);
					AssetDatabase.Refresh();
				}
				assetFileName = EditorGUILayout.TextField("File to save to:", assetFileName);

			}

		}
        #endif

	}

}
