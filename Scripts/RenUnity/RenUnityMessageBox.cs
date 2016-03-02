using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace JBirdEngine {

	namespace RenUnity {

		public class RenUnityMessageBox : MonoBehaviour {

			public Text messageTextBox;
			public Image characterPortrait;
			public Text characterNameTextBox;

			public List<RenUnityOptionButton> buttons;

			public Animator animator;

			void Awake () {
				animator = GetComponent<Animator>();
			}

			public void SwapPicture () {
				DialogueBoxHandler.waitingForPicSwap = false;
			}

		}

	}

}
