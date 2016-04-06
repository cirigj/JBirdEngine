using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using JBirdEngine.EditorHelper;
#endif

namespace JBirdEngine {

	namespace RenUnity {

		[CreateAssetMenu]
		public class StoryBranch : ScriptableObject {

            #if UNITY_EDITOR
            [ViewOnly]
            #endif
            public int cachedIndex = -1;
			public Branch branch;
			
		}

	}

}
