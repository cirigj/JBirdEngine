using UnityEngine;
using System.Collections;
using JBirdEngine.EditorHelper;
using JBirdEngine.RenUnity;

[CreateAssetMenu]
public class StoryBranch : ScriptableObject {

    [ViewOnly]
    public int cachedIndex = -1;
	public Branch branch;
			
}