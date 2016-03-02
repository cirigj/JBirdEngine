using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace JBirdEngine {

	namespace RenUnity {

		public class RenUnityOptionButton : MonoBehaviour {

			public Text textBox;
			public Button button;
			public bool buttonActive;

			public StoryBranch jumpBranch;

			void Awake () {
				DisableOption();
			}

			public void Jump () {
				if (jumpBranch == null) {
					Debug.LogErrorFormat("RenUnity.OptionButton: Attempting to jump to a null branch.");
					return;
				}
				DialogueParser.ParseBranch(jumpBranch);
			}

			public void EnableOption (string message, StoryBranch branch) {
				button.SetActive(true);
				textBox.text = message;
				buttonActive = true;
				jumpBranch = branch;
			}

			public void DisableOption () {
				button.SetActive(false);
				textBox.text = string.Empty;
				buttonActive = false;
				jumpBranch = null;
			}

		}

	}

}
