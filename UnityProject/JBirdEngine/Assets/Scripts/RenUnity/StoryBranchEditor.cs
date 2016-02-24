using UnityEngine;
using System.Collections;
using UnityEditor;

namespace JBirdEngine {

	namespace RenUnity {

		/// <summary>
		/// Story branch custom inspector class.
		/// </summary>
		[CustomEditor(typeof(StoryBranch))]
		public class StoryBranchEditor : Editor {

			public TextAsset readFile;
			public string writeFileName = "Assets/RenUnity/Json/Untitled.txt";
			public string assetFileName = "Assets/RenUnity/Branches/Untitled.asset";

			public override void OnInspectorGUI () {

				DrawDefaultInspector();

				StoryBranch editorTarget = (StoryBranch)target;

				GUILayout.Space(16);
				if (GUILayout.Button("Read From File")) {
					editorTarget.storyBranch = StoryBranchJsonSerializer.Read(readFile);
				}
				readFile = EditorGUILayout.ObjectField("File to read from:", readFile, typeof(TextAsset), true) as TextAsset;

				GUILayout.Space(16);
				if (GUILayout.Button("Write To File")) {
					StoryBranchJsonSerializer.Write(writeFileName, editorTarget.storyBranch);
					AssetDatabase.Refresh();
				}
				writeFileName = EditorGUILayout.TextField("File to write to:", writeFileName);

				EditorGUILayout.Space();
				if (GUILayout.Button("Save as Asset")) {
					AssetDatabase.CreateAsset(editorTarget, assetFileName);
					//AssetDatabase.SaveAssets();
					AssetDatabase.Refresh();
				}
				assetFileName = EditorGUILayout.TextField("File to save to:", assetFileName);

			}

		}

	}

}
