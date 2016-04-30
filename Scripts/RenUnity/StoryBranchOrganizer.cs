using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JBirdEngine.RenUnity;

[CreateAssetMenu]
[System.Serializable]
public class StoryBranchOrganizer : ScriptableObject {

	public static StoryBranchOrganizer singleton;

	public List<StoryBranchEntry> entries;

	void OnEnable () {
		singleton = this;
		if (entries == null) {
			entries = new List<StoryBranchEntry>();
		}
	}

}