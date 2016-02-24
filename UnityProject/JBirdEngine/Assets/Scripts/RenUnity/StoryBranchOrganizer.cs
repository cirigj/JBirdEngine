using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JBirdEngine {
	
	namespace RenUnity {

		[CreateAssetMenu]
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

	}

}
