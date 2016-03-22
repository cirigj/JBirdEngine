using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine.UI;

// TODO:
// Add library of story branches
// Add library of characters
// Add button to check for parsing errors
//    Verify Characters (speaking/entrance/exit)
//    Verify Moods
//    Verify Jumps
//    Verify Stats (set/check)
// Add actual decoding of story branches into UI
//    GoToNextLine
//    ChooseOption
// Add "none" keyword for not jumping to a new branch
// Add ability to add multiple commands in story branches
// Add name of character speaking (#[name]:)
// Add character moods/portraits (#mood [name] [mood]:)
// Add character entrances/exits (#enter [name] (location):/#exit [name] (location):) - default to center
// Add character stats changing (#stat [name] [stat] [n]:) - also for option consequence
// Add backgrounds (#bg [image]:)
// Add fade (#fade [seconds]:) - also for option consequences
// Add if statements to options (#if [stat] [n]:) - only for option consequence
// Add if statements to branching (#if [stat] [n] [jumpTo]: #jump [defaultJumpTo]) - also for option consequence, otherwise happens immediately

namespace JBirdEngine {

	namespace RenUnity {

		/// <summary>
		/// RenUnity base class for viewing variables via editor script.
		/// </summary>
		public class RenUnityBase : MonoBehaviour { 
		
			public static RenUnityBase singleton;

			public StoryBranchOrganizer singletonSBO;

			public List<string> conditionalFlags = new List<string>();

			public List<CharacterData> characterData = new List<CharacterData>();

			public float writeSpeed = 0.1f;

			public RenUnityMessageBox messageBoxPrefab;

			public Canvas uiCanvas;

			void Awake () {
				if (singleton == null) {
					singleton = this;
				}
				GetCharacters();
				CharacterDatabase.characters = characterData;
				DialogueBoxHandler.writeSpeed = writeSpeed;
			}

			void Start () {
				DialogueParser.ParseBranch(StoryBranchOrganizer.singleton.entries[0].thisBranch);
			}

			void Update () {
				if (Input.GetKeyDown(KeyCode.Return)) {
					DialogueBoxHandler.Next();
				}
			}
				
			public void GetCharacters () {
				List<CharacterData> newData = CharacterDatabase.AddAllCharacters();
				for (int i = 0; i < newData.Count; i++) {
					bool charFound = false;
					if (i < characterData.Count && i < System.Enum.GetValues(typeof(Character)).Length) {
						charFound = true;
					}
					if (charFound) {
						for (int j = 0; j < newData[i].stats.Count; j++) {
							bool statFound = false;
							if (j < characterData[i].stats.Count && j < System.Enum.GetValues(typeof(Stat)).Length) {
								statFound = true;
							}
							if (statFound) {
								newData[i].stats[j] = characterData[i].stats[j];
							}
						}
						for (int j = 0; j < newData[i].portraits.Count; j++) {
							bool portraitFound = false;
							if (j < characterData[i].portraits.Count && j < System.Enum.GetValues(typeof(Mood)).Length) {
								portraitFound = true;
							}
							if (portraitFound) {
								newData[i].portraits[j] = characterData[i].portraits[j];
							}
						}
						newData[i].defaultPortrait = characterData[i].defaultPortrait;
					}
				}
				characterData = newData;
			}
		
		}

		/// <summary>
		/// Branch class. Contains dialogue objects.
		/// </summary>
		[System.Serializable]
		public class Branch {

			public string branchName;
			public List<string> script;

			public Branch () {
				script = new List<string>();
			}

		}

        public static class StoryBranchJsonSerializer {

            public static Branch Read (TextAsset file) {
                Branch newBranch = new Branch();
                using (StringReader reader = new StringReader(file.text)) {
                    string json = reader.ReadToEnd();
                    newBranch = JsonUtility.FromJson<Branch>(json);
                }
                return newBranch;
            }

            public static void Write (string fileName, Branch branch) {
                string json = JsonUtility.ToJson(branch, true);
				if (File.Exists(fileName)) {
					File.Delete(fileName);
				}
				using (FileStream writer = new FileStream(fileName, FileMode.CreateNew)) {
                    writer.Write(Encoding.UTF8.GetBytes(json), 0, Encoding.UTF8.GetByteCount(json));
                }
            }

        }

		[System.Serializable]
		public class StoryBranchEntry {

			public StoryBranch thisBranch;
			public StoryBranch parentBranch;
			public List<StoryBranch> jumpList;

			public StoryBranchEntry () {
				jumpList = new List<StoryBranch>();
			}

		}

		public static class ConditionalFlags {

			public static List<string> flags = new List<string>();

			public static bool FlagExists (string flagName) {
				for (int i = flags.Count - 1; i >= 0; i--) {
					if (flags[i] == flagName) {
						flags.RemoveAt(i);
						flags.Add(flagName);
						if (RenUnityBase.singleton != null) {
							RenUnityBase.singleton.conditionalFlags.RemoveAt(i);
							RenUnityBase.singleton.conditionalFlags.Add(flagName);
						}
						return true;
					}
				}
				return false;
			}

			public static void AddFlag (string flagName) {
				if (!FlagExists(flagName)) {
					flags.Add(flagName);
					if (RenUnityBase.singleton != null) {
						RenUnityBase.singleton.conditionalFlags.Add(flagName);
					}
				}
			}

		}

		[System.Serializable]
		public class StatData {

			public Stat stat;
			public float value;

			public StatData (Stat s, float v) {
				stat = s;
				value = v;
			}

		}

		[System.Serializable]
		public class MoodData {

			public Mood mood;
			public Sprite portrait;

			public MoodData (Mood m, Sprite p) {
				mood = m;
				portrait = p;
			}

		}

		[System.Serializable]
		public class CharacterData {

			public Character name;
			public Sprite defaultPortrait;
			public List<MoodData> portraits;
			public List<StatData> stats;

			public CharacterData (Character c) {
				name = c;
				defaultPortrait = new Sprite();
				portraits = new List<MoodData>();
				stats = new List<StatData>();
			}

		}

		public static class CharacterDatabase {

			private static List<CharacterData> _characters = new List<CharacterData>();
			public static List<CharacterData> characters {
				get {
					if (_characters == null || _characters.Count == 0) {
						_characters = AddAllCharacters();
					}
					return _characters;
				}
				set {
					_characters = value;
				}
			}

			public static List<CharacterData> AddAllCharacters () {
				List<CharacterData> returnList = new List<CharacterData>();
				for (int i = 1; i < System.Enum.GetValues(typeof(Character)).Length; i++) {
					CharacterData newChar = new CharacterData((Character)i);
					for (int j = 1; j < System.Enum.GetValues(typeof(Mood)).Length; j++) {
						newChar.portraits.Add(new MoodData((Mood)j, new Sprite()));
					}
					for (int j = 1; j < System.Enum.GetValues(typeof(Stat)).Length; j++) {
						newChar.stats.Add(new StatData((Stat)j, 0f));
					}
					returnList.Add(newChar);
				}
				return returnList;
			}

			public static Sprite GetSprite (Character character, Mood mood) {
				if (character == Character.InvalidName) {
					Debug.LogErrorFormat("RenUnity.CharacterDatabase: Cannot get character portrait of non-existant character.");
					return new Sprite();
				}
				else if (mood == Mood.InvalidMood) {
					return characters[(int)character - 1].defaultPortrait;
				}
				else {
					return characters[(int)character - 1].portraits[(int)mood - 1].portrait;
				}
			}

			public static float GetStat (Character character, Stat stat) {
				if (character == Character.InvalidName) {
					Debug.LogErrorFormat("RenUnity.CharacterDatabase: Cannot get stat {0} of non-existant character.", stat);
					return -1;
				}
				if (stat == Stat.InvalidStat) {
					Debug.LogErrorFormat("RenUnity.CharacterDatabase: Attempting to get invalid stat.");
					return -1;
				}
				return characters[(int)character - 1].stats[(int)stat - 1].value;
			}

		}

		public static class DialogueBoxHandler {

			private static bool writingDialogue = false;
			private static bool skipDialogue = false;
			private static Coroutine writingRoutine;

			public static RenUnityMessageBox messageBox;
			public static Text boxText;
			public static Image portraitImage;
			public static float writeSpeed;

			public static Character currentCharacter;
			public static Mood currentMood;

			public static bool waitingForPicSwap = false;

			public static void Next () {
				if (writingDialogue) {
					skipDialogue = true;
				}
				else {
					DialogueParser.ContinueParsingScript();
				}
			}

			public static IEnumerator OpenMessageBox () {
				messageBox.animator.SetTrigger("openTrig");
				yield return new WaitForSeconds(messageBox.animator.GetCurrentAnimatorStateInfo(0).length * messageBox.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
				DialogueParser.ContinuePostAnimation();
				yield break;
			}

			public static IEnumerator SwapCharacters () {
				messageBox.animator.SetTrigger("swapTrig");
				waitingForPicSwap = true;
				while (waitingForPicSwap) {
					yield return null;
				}
				UpdateMood(currentMood);
				DialogueParser.ContinuePostAnimation();
				yield break;
			}

			public static IEnumerator CloseMessageBox () {
				if (!messageBox.isActiveAndEnabled) {
					yield break;
				}
				messageBox.animator.SetTrigger("closeTrig");
				yield return new WaitForSeconds(messageBox.animator.GetCurrentAnimatorStateInfo(0).length * messageBox.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
				DialogueParser.ContinuePostAnimation();
				boxText.text = string.Empty;
				if (messageBox != null) {
					messageBox.SetActive(false);
				}
				boxText = null;
				portraitImage = null;
				currentCharacter = Character.InvalidName;
				yield break;
			}

			public static void CharacterStartTalking (Character character, Mood mood = Mood.InvalidMood) {
				if (RenUnityBase.singleton == null) {
					Debug.LogErrorFormat("RenUnity.DialogueBoxHandler: No RenUnityBase instance exists! Please make sure one is instantiated.");
					return;
				}
				if (messageBox == null) {
					if (RenUnityBase.singleton.messageBoxPrefab == null) {
						Debug.LogErrorFormat("RenUnity.DialogueBoxHandler: RenUnityBase has no Message Box Prefab set!");
						return;
					}
					messageBox = GameObject.Instantiate(RenUnityBase.singleton.messageBoxPrefab, Vector3.zero, Quaternion.identity) as RenUnityMessageBox;
					if (RenUnityBase.singleton.uiCanvas == null) {
						Debug.LogErrorFormat("RenUnity.DialogueBoxHandler: RenUnityBase has no UI Canvas set!");
						return;
					}
					messageBox.transform.SetParent(RenUnityBase.singleton.uiCanvas.transform, false);
				}
				messageBox.SetActive(true);
				boxText = messageBox.messageTextBox;
				portraitImage = messageBox.characterPortrait;
				if (currentCharacter == Character.InvalidName) {
					currentCharacter = character;
					UpdateMood(mood);
					RenUnityBase.singleton.StartCoroutine(OpenMessageBox());
				}
				else if (currentCharacter == character) {
					UpdateMood(mood);
					DialogueParser.ContinuePostAnimation();
				}
				else {
					currentCharacter = character;
					currentMood = mood;
					RenUnityBase.singleton.StartCoroutine(SwapCharacters());
				}
			}

			public static void CharacterStopTalking () {
				if (RenUnityBase.singleton == null) {
					Debug.LogErrorFormat("RenUnity.DialogueBoxHandler: No RenUnityBase instance exists! Please make sure one is instantiated.");
					return;
				}
				RenUnityBase.singleton.StartCoroutine(CloseMessageBox());
			}

			public static void DisplayMessage (string message) {
				ClearOptions();
				if (RenUnityBase.singleton == null) {
					Debug.LogErrorFormat("RenUnity.DialogueBoxHandler: No RenUnityBase instance exists! Please make sure one is instantiated, and start this coroutine on 'RenUnityBase.singleton'.");
					return;
				}
				writingRoutine = RenUnityBase.singleton.StartCoroutine(UpdateMessage(message));
			}

			public static IEnumerator UpdateMessage (string message) {
				if (boxText == null) {
					Debug.LogErrorFormat("RenUnity.DialogueBoxHandler: No text box to write to! Make sure to use '/start_talk' command before attempting to display dialogue.");
					yield break;
				}
				boxText.text = string.Empty;
				writingDialogue = true;
				float snapshotWriteSpeed = writeSpeed;
				for (int i = 0; i < message.Length; i++) {
					//special characters
					if (message[i] == '/') {
						i++;
						if (message[i] == 'p') {
							if (!skipDialogue) {
								yield return new WaitForSeconds(snapshotWriteSpeed);
							}
						}
						else if (message[i] == 'b') {
							boxText.text = boxText.text.Substring(0, boxText.text.Length - 1);
							if (!skipDialogue) {
								yield return new WaitForSeconds(snapshotWriteSpeed);
							}
						}
						else if (message[i] == 'h') {
							snapshotWriteSpeed = 2f * writeSpeed;
						}
						else if (message[i] == 'd') {
							snapshotWriteSpeed = 0.5f * writeSpeed;
						}
						else if (message[i] == 'q') {
							snapshotWriteSpeed = 4f * writeSpeed;
						}
						else if (message[i] == 'f') {
							snapshotWriteSpeed = 0.25f * writeSpeed;
						}
						else if (message[i] == 'r') {
							snapshotWriteSpeed = writeSpeed;
						}
						else if (message[i] == 'i') {
							i++;
							while (i < message.Length && message[i] != '/') {
								boxText.text = string.Concat(boxText.text, message[i]);
								i++;
							}
						}
						else if (message[i] == '/') {
							boxText.text = string.Concat(boxText.text, message[i]);
							if (!skipDialogue) {
								yield return new WaitForSeconds(snapshotWriteSpeed);
							}
						}
					}
					else {
						boxText.text = string.Concat(boxText.text, message[i]);
						if (!skipDialogue) {
							yield return new WaitForSeconds(snapshotWriteSpeed);
						}
					}
				}
				skipDialogue = false;
				writingDialogue = false;
				writingRoutine = null;
				yield break;
			}

			public static void ClearOptions () {
				for (int i = 0; i < messageBox.buttons.Count; i++) {
					messageBox.buttons[i].DisableOption();
				}
			}

			public static void AddOption (string message, StoryBranch branch) {
				for (int i = 0; i < messageBox.buttons.Count; i++) {
					if (messageBox.buttons[i].buttonActive) {
						continue;
					}
					messageBox.buttons[i].EnableOption(message, branch);
					return;
				}
				Debug.LogErrorFormat("RenUnity.DialogueBoxHandler: Attempting to add option '{0}' (jumps to branch {1}) when no more slots are available.", message, branch.branch.branchName);
			}

			public static void UpdateMood (Mood newMood) {
				if (RenUnityBase.singleton == null) {
					Debug.LogErrorFormat("RenUnity.DialogueBoxHandler: No RenUnityBase instance exists! Please make sure one is instantiated.");
					return;
				}
				if (portraitImage == null) {
					Debug.LogErrorFormat("RenUnity.DialogueBoxHandler: The Message Box Prefab does not have an image set to display the character portraits!");
					return;
				}
				if (currentCharacter == Character.InvalidName) {
					Debug.LogErrorFormat("RenUnity.DialogueBoxHandler: Cannot set the mood of a non-existant character.");
					return;
				}
				portraitImage.sprite = CharacterDatabase.GetSprite(currentCharacter, newMood);
				currentMood = newMood;
			}

		}

		public static class DialogueParser {

			private static bool waitingForInput = false;
			private static bool waitingForAnim = false;

			public static int currentBranchIndex;

			public static Coroutine parseRoutine;

			public static void ParseBranch (StoryBranch branch) {
				if (DialogueParser.parseRoutine != null) {
					RenUnityBase.singleton.StopCoroutine(parseRoutine);
				}
				parseRoutine = RenUnityBase.singleton.StartCoroutine(DialogueParser.ParseDialogue(branch));
			}

			public static void ContinueParsingScript () {
				if (waitingForInput) {
					waitingForInput = false;
				}
			}

			public static void ContinuePostAnimation () {
				if (waitingForAnim) {
					waitingForAnim = false;
				}
			}

			public enum CommandType {
				Message,
				StartTalk,
				StopTalk,
				Option,
				Jump,
				SetFlag,
				SetStat,
				SetMood,
				Wait,
			}

			public enum ConditionalType {
				Flag,
				Stat,
			}

			public enum EvaluationType {
				Equals,
				NotEquals,
				Less,
				Greater,
				LessEqual,
				GreaterEqual,
			}

			public class ConditionalInfo {

				public ConditionalType type;
				public string flag;
				public bool exists;
				public Character character;
				public Stat stat;
				public EvaluationType evalType;
				public float value;

				public ConditionalInfo () {
					
				}

				public override string ToString () {
					if (type == ConditionalType.Flag) {
						if (exists) {
							return string.Format("if flag {0}", flag);
						}
						else {
							return string.Format("if !flag {0}", flag);
						}
					}
					else if (type == ConditionalType.Stat) {
						string eval = string.Empty;
						switch (evalType) {
						case EvaluationType.Equals:
							eval = string.Concat(eval, "==");
							break;
						case EvaluationType.NotEquals:
							eval = string.Concat(eval, "!=");
							break;
						case EvaluationType.Greater:
							eval = string.Concat(eval, ">");
							break;
						case EvaluationType.GreaterEqual:
							eval = string.Concat(eval, ">=");
							break;
						case EvaluationType.Less:
							eval = string.Concat(eval, "<");
							break;
						case EvaluationType.LessEqual:
							eval = string.Concat(eval, "<=");
							break;
						}
						return string.Format("if stat {0} {1} {2} {3}", character.ToString(), stat.ToString(), eval, value);
					}
					return string.Empty;
				}

				public bool Evaluate () {
					switch (type) {
					case ConditionalType.Flag:
						if (exists) {
							return ConditionalFlags.FlagExists(flag);
						}
						else {
							return !ConditionalFlags.FlagExists(flag);
						}
					case ConditionalType.Stat:
						switch (evalType) {
						case EvaluationType.Equals:
							return CharacterDatabase.characters[(int)character - 1].stats[(int)stat - 1].value == value;
						case EvaluationType.NotEquals:
							return CharacterDatabase.characters[(int)character - 1].stats[(int)stat - 1].value == value;
						case EvaluationType.Greater:
							return CharacterDatabase.characters[(int)character - 1].stats[(int)stat - 1].value > value;
						case EvaluationType.GreaterEqual:
							return CharacterDatabase.characters[(int)character - 1].stats[(int)stat - 1].value >= value;
						case EvaluationType.Less:
							return CharacterDatabase.characters[(int)character - 1].stats[(int)stat - 1].value < value;
						case EvaluationType.LessEqual:
							return CharacterDatabase.characters[(int)character - 1].stats[(int)stat - 1].value <= value;
						default:
							return false;
						}
					default:
						return false;
					}
				}

				public int GetLength () {
					if (type == ConditionalType.Flag) {
						return 3;
					}
					if (type == ConditionalType.Stat) {
						return 6;
					}
					return 0;
				}

			}

			public class CommandInfo {

				public CommandType type;
				public string message;
				public Character character;
				public Stat stat;
				public Mood mood;
				public bool relative;
				public bool negate;
				public float value;
				public ConditionalInfo availability;
				public ConditionalInfo conditional;
				public int branch;
				public int conditionalBranch;

				public CommandInfo () {
					
				}

				public override string ToString () {
					return string.Format ("[CommandInfo] Type: '{0}', Message: '{1}', Character: '{2}', Stat: '{3}', Relative: {4}, Negate: {5}, Value: {6} Availability: {7}, Conditional: {8}, Branch: {9}, ConditionalBranch: {10}",
						type, message, character, stat, relative, negate, value, availability, conditional, branch, conditionalBranch);
				}

			}

			/// <summary>
			/// Custom predicate function for determining if a string is the empty string.
			/// </summary>
			/// <returns><c>true</c> if the specified str is the empty string; otherwise, <c>false</c>.</returns>
			/// <param name="str">String to check.</param>
			public static bool IsEmptyString (this string str) {
				return str == string.Empty;
			}

			public static bool IsCommand (this string line) {
				if (line == string.Empty) {
					return false;
				}
				return (line[0] == '/');
			}

			private static Character VerifyCharacter (string name, int lineNumber = -1, string branchName = "N/A") {
				Character character = Character.InvalidName;
				if (!EnumHelper.TryParse<Character>(name, out character)) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid name '{0}' (branch {1}, line {2}).", name, branchName, lineNumber);
				}
				else if (character == Character.InvalidName) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Did you really name a character 'InvalidName'? Really? (branch {0}, line {1})", branchName, lineNumber);
				}
				return character;
			}

			private static Stat VerifyStat (string name, int lineNumber = -1, string branchName = "N/A") {
				Stat stat = Stat.InvalidStat;
				if (!EnumHelper.TryParse<Stat>(name, out stat)) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid stat '{0}' (branch {1}, line {2}).", name, branchName, lineNumber);
				}
				else if (stat == Stat.InvalidStat) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Did you really name a stat 'InvalidStat'? Really? (branch {0}, line {1})", branchName, lineNumber);
				}
				return stat;
			}

			private static Mood VerifyMood (string name, int lineNumber = -1, string branchName = "N/A") {
				Mood mood = Mood.InvalidMood;
				if (!EnumHelper.TryParse<Mood>(name, out mood)) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid mood '{0}' (branch {1}, line {2}).", name, branchName, lineNumber);
				}
				else if (mood == Mood.InvalidMood) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Did you really name a mood 'InvalidMood'? Really? (branch {0}, line {1})", branchName, lineNumber);
				}
				return mood;
			}

			public static List<string> Tokenize (this string line, params char[] splitChars) {
				List<string> returnList = new List<string>();
				List<char> splitCharList = splitChars.ToList();
				returnList.Add(string.Empty);
				for (int i = 0; i < line.Length; i++) {
					if (splitCharList.Contains(line[i])) {
						returnList.Add(string.Empty);
					}
					else {
						returnList[returnList.Count - 1] = string.Concat(returnList[returnList.Count - 1], line[i]);
					}
				}
				returnList.RemoveAll(IsEmptyString);
				return returnList;
			}

			public static void SetCharacterStat (Character character, Stat stat, float value) {
				CharacterDatabase.characters[(int)character - 1].stats[(int)stat - 1].value = value;
			}

			public static void SetCharacterStatRelative (Character character, Stat stat, float value) {
				CharacterDatabase.characters[(int)character - 1].stats[(int)stat - 1].value += value;
			}

			public static IEnumerator ParseDialogue (StoryBranch currentStoryBranch) {
				if (StoryBranchOrganizer.singleton == null) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: No StoryBranchOrganizer instance exists! Please create one somewhere in the Assets folder.");
					yield break;
				}
				if (RenUnityBase.singleton == null) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: No RenUnityBase instance exists! Please make sure one is instantiated, and start this coroutine on 'RenUnityBase.singleton'.");
					yield break;
				}
				int branchIndex = -1;
				for (int i = 0; i < StoryBranchOrganizer.singleton.entries.Count; i++) {
					if (StoryBranchOrganizer.singleton.entries[i].thisBranch == currentStoryBranch) {
						branchIndex = i;
					}
				}
				if (branchIndex == -1) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Story Branch {0} has not been added to the Story Branch Organizer!", currentStoryBranch.branch.branchName);
					yield break;
				}
				currentBranchIndex = branchIndex;
				for (int i = 0; i < currentStoryBranch.branch.script.Count; i++) {
					CommandInfo info = ParseLine(currentStoryBranch.branch.script[i], i, currentStoryBranch.branch.branchName);
					switch (info.type) {
					case CommandType.Message:
						DialogueBoxHandler.DisplayMessage(info.message);
						break;
					case CommandType.StartTalk:
						waitingForAnim = true;
						DialogueBoxHandler.CharacterStartTalking(info.character, info.mood);
						while (waitingForAnim) {
							yield return null;
						}
						break;
					case CommandType.StopTalk:
						waitingForAnim = true;
						DialogueBoxHandler.CharacterStopTalking();
						while (waitingForAnim) {
							yield return null;
						}
						break;
					case CommandType.Option:
						if (info.availability != null && !info.availability.Evaluate()) {
							break;
						}
						if (info.conditional != null && info.conditional.Evaluate()) {
							info.branch = info.conditionalBranch;
						}
						StoryBranch jumpBranch;
						if (info.branch == -2) {
							jumpBranch = StoryBranchOrganizer.singleton.entries[currentBranchIndex].thisBranch;
						}
						else if (info.branch == -1) {
							if (StoryBranchOrganizer.singleton.entries[currentBranchIndex].parentBranch == null) {
								Debug.LogErrorFormat("RenUnity.DialogueParser: Branch {0} has no parent! Cannot have option that jumps to parent branch.", currentStoryBranch.branch.branchName);
								yield break;
							}
							jumpBranch = StoryBranchOrganizer.singleton.entries[currentBranchIndex].parentBranch;
						}
						else {
							if (StoryBranchOrganizer.singleton.entries[currentBranchIndex].jumpList.Count < info.branch + 1) {
								Debug.LogErrorFormat("RenUnity.DialogueParser: Branch {0} does not have a jump branch at index {1}!", currentStoryBranch.branch.branchName, info.branch);
								yield break;
							}
							for (int j = 0; j < StoryBranchOrganizer.singleton.entries.Count; j++) {
								if (StoryBranchOrganizer.singleton.entries[j].thisBranch == StoryBranchOrganizer.singleton.entries[currentBranchIndex].jumpList[info.branch]) {
									StoryBranchOrganizer.singleton.entries[j].parentBranch = StoryBranchOrganizer.singleton.entries[currentBranchIndex].thisBranch;
									break;
								}
							}
							jumpBranch = StoryBranchOrganizer.singleton.entries[currentBranchIndex].jumpList[info.branch];
						}
						DialogueBoxHandler.AddOption(info.message, jumpBranch);
						break;
					case CommandType.Jump:
						if (info.conditional != null && !info.conditional.Evaluate()) {
							break;
						}
						if (info.branch == -2) {
							parseRoutine = RenUnityBase.singleton.StartCoroutine(ParseDialogue(StoryBranchOrganizer.singleton.entries[currentBranchIndex].thisBranch));
						}
						else if (info.branch == -1) {
							if (StoryBranchOrganizer.singleton.entries[currentBranchIndex].parentBranch == null) {
								Debug.LogErrorFormat("RenUnity.DialogueParser: Branch {0} has no parent! Cannot use '/jump_back' command.", currentStoryBranch.branch.branchName);
								yield break;
							}
							parseRoutine = RenUnityBase.singleton.StartCoroutine(ParseDialogue(StoryBranchOrganizer.singleton.entries[currentBranchIndex].parentBranch));
						}
						else {
							if (StoryBranchOrganizer.singleton.entries[currentBranchIndex].jumpList.Count < info.branch + 1) {
								Debug.LogErrorFormat("RenUnity.DialogueParser: Branch {0} does not have a jump branch at index {1}!", currentStoryBranch.branch.branchName, info.branch);
								yield break;
							}
							for (int j = 0; j < StoryBranchOrganizer.singleton.entries.Count; j++) {
								if (StoryBranchOrganizer.singleton.entries[j].thisBranch == StoryBranchOrganizer.singleton.entries[currentBranchIndex].jumpList[info.branch]) {
									StoryBranchOrganizer.singleton.entries[j].parentBranch = StoryBranchOrganizer.singleton.entries[currentBranchIndex].thisBranch;
									break;
								}
							}
							parseRoutine = RenUnityBase.singleton.StartCoroutine(ParseDialogue(StoryBranchOrganizer.singleton.entries[currentBranchIndex].jumpList[info.branch]));
						}
						yield break;
					case CommandType.SetFlag:
						ConditionalFlags.AddFlag(info.message);
						break;
					case CommandType.SetStat:
						if (info.relative) {
							if (info.negate) {
								info.value *= -1;
							}
							SetCharacterStatRelative(info.character, info.stat, info.value);
						}
						else {
							SetCharacterStat(info.character, info.stat, info.value);
						}
						break;
					case CommandType.SetMood:
						DialogueBoxHandler.UpdateMood(info.mood);
						break;
					case CommandType.Wait:
						if (info.value > 0) {
							yield return new WaitForSeconds(info.value);
						}
						else {
							waitingForInput = true;
							while (waitingForInput) {
								yield return null;
							}
						}
						break;
					}
				}
				parseRoutine = null;
				if (DialogueBoxHandler.messageBox == null) {
					yield break;
				}
				for (int i = 0; i < DialogueBoxHandler.messageBox.buttons.Count; i++) {
					if (DialogueBoxHandler.messageBox.buttons[i].buttonActive) {
						yield break;
					}
				}
				DialogueBoxHandler.CharacterStopTalking();
				yield break;
			}

			public static CommandInfo ParseLine (string line, int lineNumber = -1, string branchName = "N/A") {
				CommandInfo info = new CommandInfo();
				if (!line.IsCommand()) {
					if (line.IsEmptyString()) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Empty line detected (branch {0}, line {1}).", branchName, lineNumber);
						return null;
					}
					info.type = CommandType.Message;
					info.message = line;
					return info;
				}
				List<string> tokens = line.Tokenize(' ', '/');
				if (tokens.Count == 0) {
					return null;
				}
				string command = tokens[0];
				switch (command) {
				case "start_talk":
					if (tokens.Count != 2 && tokens.Count != 3) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid use of '/start_talk' command (branch {0}, line {1}). Correct syntax is '/start_talk [characterName] (moodName)'.", branchName, lineNumber);
						return null;
					}
					return StartTalk(tokens, lineNumber, branchName);
				case "stop_talk":
					if (tokens.Count != 1) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Extraneous arguments supplied to '/stop_talk' command (branch {0}, line {1}). Correct syntax is '/stop_talk'.", branchName, lineNumber);
						return null;
					}
					return StopTalk(lineNumber, branchName);
				case "option":
					if (tokens.Count < 3) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid use of '/option' command (branch {0}, line {1}). Correct syntax is '/option (availabilityConditional) \"[text]\" (conditional branchIndex) [branchIndex]'.", branchName, lineNumber);
						return null;
					}
					return Option(tokens, lineNumber, branchName);
				case "jump":
					if (tokens.Count != 2 && tokens.Count != 5 && tokens.Count != 8) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid use of '/jump' command (branch {0}, line {1}). Correct syntax is '/jump (conditional) [branchIndex]'.", branchName, lineNumber);
						return null;
					}
					return Jump(tokens, lineNumber, branchName);
				case "jump_back":
					if (tokens.Count != 1) {
						Debug.LogWarningFormat("RenUnity.DialogueParser: '/jump_back' command does not require additional arguments (branch {0}, line {1}).", branchName, lineNumber);
					}
					info.type = CommandType.Jump;
					info.branch = -1;
					return info;
				case "set_flag":
					if (tokens.Count != 2) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid use of '/set_flag' command (branch {0}, line {1}). Correct syntax is '/set_flag [flagName]'.", branchName, lineNumber);
						return null;
					}
					info.type = CommandType.SetFlag;
					info.message = tokens[1];
					return info;
				case "set_stat":
					if (tokens.Count != 4 && tokens.Count != 5) {
						for (int i = 0; i < tokens.Count; i++) {
							Debug.Log(tokens[i]);
						}
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid use of '/set_stat' command (branch {0}, line {1}). Correct syntax is '/set_stat [characterName] [stat] (+,-) [value]'.", branchName, lineNumber);
						return null;
					}
					return SetStat(tokens, lineNumber, branchName);
				case "wait":
					info.type = CommandType.Wait;
					if (tokens.Count == 1) {
						return info;
					}
					else if (tokens.Count != 2) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid use of '/wait' command (branch {0}, line {1}). Correct syntax is '/wait (seconds)'.", branchName, lineNumber);
						return null;
					}
					float value = 0f;
					if (!float.TryParse(tokens[1], out value) || value < 0) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid value '{0}' for '/wait' command (branch {1}, line {2}).", tokens[1], branchName, lineNumber);
						return null;
					}
					info.value = value;
					return info;
				case "mood":
					if (tokens.Count != 2) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid use of '/mood' command (branch {0}, line {1}). Correct syntax is '/mood [moodName]'.", branchName, lineNumber);
						return null;
					}
					return SetMood(tokens[1], lineNumber, branchName);
				default:
					//***FIX THIS***
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid command '{0}' (branch {1}, line {2}).{3}Attempted to parse line: '{4}'.", command, branchName, lineNumber, System.Environment.NewLine, line);
					return null;
				}
				Debug.LogErrorFormat("RenUnity.DialogueParser: Command '{0}' recognized, but not implemented (branch {1}, line {2}).", tokens[0], branchName, lineNumber);
				return null;
			}

			private static ConditionalInfo HandleConditional (List<string> tokens, int startIndex, int lineNumber = -1, string branchName = "N/A") {
				ConditionalInfo conditional = new ConditionalInfo();
				if (tokens.Count < startIndex + 1) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid conditional syntax (branch {0}, line {1}). Proper syntax is 'if flag [flagName]' or 'if stat [characterName] [statName] [==, !=, >, >=, <, <=] [value]'", branchName, lineNumber);
					return null;
				}
				if (tokens[startIndex] == "flag") {
					conditional.type = ConditionalType.Flag;
					conditional.exists = true;
					startIndex++;
					if (tokens.Count < startIndex + 1) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid conditional syntax (branch {0}, line {1}). Proper syntax is 'if flag [flagName]'", branchName, lineNumber);
						return null;
					}
					conditional.flag = tokens[startIndex];
					return conditional;
				}
				else if (tokens[startIndex] == "!flag") {
					conditional.type = ConditionalType.Flag;
					conditional.exists = false;
					startIndex++;
					if (tokens.Count < startIndex + 1) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid conditional syntax (branch {0}, line {1}). Proper syntax is 'if flag [flagName]'", branchName, lineNumber);
						return null;
					}
					conditional.flag = tokens[startIndex];
					return conditional;
				}
				else if (tokens[startIndex] == "stat") {
					conditional.type = ConditionalType.Stat;
					startIndex++;
					if (tokens.Count < startIndex + 1) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid conditional syntax (branch {0}, line {1}). Proper syntax is 'if stat [characterName] [statName] [==, !=, >, >=, <, <=] [value]'", branchName, lineNumber);
						return null;
					}
					conditional.character = VerifyCharacter(tokens[startIndex], lineNumber, branchName);
					if (conditional.character == Character.InvalidName) {
						return null;
					}
					startIndex++;
					if (tokens.Count < startIndex + 1) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid conditional syntax (branch {0}, line {1}). Proper syntax is 'if stat [characterName] [statName] [==, !=, >, >=, <, <=] [value]'", branchName, lineNumber);
						return null;
					}
					conditional.stat = VerifyStat(tokens[startIndex], lineNumber, branchName);
					if (conditional.stat == Stat.InvalidStat) {
						return null;
					}
					startIndex++;
					if (tokens.Count < startIndex + 1) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid conditional syntax (branch {0}, line {1}). Proper syntax is 'if stat [characterName] [statName] [==, !=, >, >=, <, <=] [value]'", branchName, lineNumber);
						return null;
					}
					switch (tokens[startIndex]) {
					case "==":
						conditional.evalType = EvaluationType.Equals;
						break;
					case "!=":
						conditional.evalType = EvaluationType.NotEquals;
						break;
					case ">":
						conditional.evalType = EvaluationType.Greater;
						break;
					case ">=":
						conditional.evalType = EvaluationType.GreaterEqual;
						break;
					case "<":
						conditional.evalType = EvaluationType.Less;
						break;
					case "<=":
						conditional.evalType = EvaluationType.LessEqual;
						break;
					default:
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid operator '{0}' (branch {1}, line {2}). Accepted operators are: ==, !=, >, >=, <, or <=.", tokens[startIndex], branchName, lineNumber);
						return null;
					}
					startIndex++;
					if (tokens.Count < startIndex + 1) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid conditional syntax (branch {0}, line {1}). Proper syntax is 'if stat [characterName] [statName] [==, !=, >, >=, <, <=] [value]'", branchName, lineNumber);
						return null;
					}
					float value;
					if (!float.TryParse(tokens[startIndex], out value)) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Non-numerical value '{0}' (branch {1}, line {2}).", tokens[startIndex], branchName, lineNumber);
						return null;
					}
					conditional.value = value;
					return conditional;
				}
				else {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid conditional type '{0}' (branch {1}, line {2})", tokens[startIndex], branchName, lineNumber);
					return null;
				}
			}

			private static CommandInfo StartTalk (List<string> tokens, int lineNumber = -1, string branchName = "N/A") {
				CommandInfo info = new CommandInfo();
				info.type = CommandType.StartTalk;
				Character character = VerifyCharacter(tokens[1], lineNumber, branchName);
				if (character != Character.InvalidName) {
					info.character = character;
				}
				else {
					return null;
				}
				if (tokens.Count == 3) {
					Mood mood = VerifyMood(tokens[2], lineNumber, branchName);
					if (mood != Mood.InvalidMood) {
						info.mood = mood;
						return info;
					}
					else {
						return null;
					}
				}
				else {
					return info;
				}
			}

			private static CommandInfo StopTalk (int lineNumber = -1, string branchName = "N/A") {
				CommandInfo info = new CommandInfo();
				info.type = CommandType.StopTalk;
				return info;
			}

			private static CommandInfo Option (List<string> tokens, int lineNumber = -1, string branchName = "N/A") {
				CommandInfo info = new CommandInfo();
				info.type = CommandType.Option;
				int parseIndex = 1;
				if (tokens[parseIndex] == "if") {
					info.availability = HandleConditional(tokens, parseIndex + 1, lineNumber, branchName);
					if (info.availability == null) {
						return null;
					}
					parseIndex += info.availability.GetLength();
				}
				if (parseIndex > tokens.Count - 1) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid arguments for '/option' command (branch {0}, line {1}). Proper syntax is '/option (availabilityConditional) \"[text]\" (conditional branchIndex) [branchIndex]'", branchName, lineNumber);
					return null;
				}
				if (tokens[parseIndex][0] != '"') {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid arguments for '/option' command (branch {0}, line {1}). Are you perhaps missing quotation marks? Proper syntax is '/option (availabilityConditional) \"[text]\" (conditional branchIndex) [branchIndex]'", branchName, lineNumber);
					return null;
				}
				string message = string.Empty;
				bool finishedWriting = false;
				while (parseIndex < tokens.Count) {
					message = string.Concat(message, tokens[parseIndex]);
					if (tokens[parseIndex][tokens[parseIndex].Length - 1] == '"') {
						finishedWriting = true;
						message = message.Substring(1, message.Length - 2);
						info.message = message;
						parseIndex++;
						break;
					}
					message = string.Concat(message, " ");
					parseIndex++;
				}
				if (!finishedWriting) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid arguments for '/option' command (branch {0}, line {1}). Could not find endquote.", branchName, lineNumber);
					return null;
				}
				if (parseIndex > tokens.Count - 1) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid arguments for '/option' command (branch {0}, line {1}). Proper syntax is '/option (availabilityConditional) \"[text]\" (conditional branchIndex) [branchIndex]'", branchName, lineNumber);
					return null;
				}
				if (tokens[parseIndex] == "if") {
					info.conditional = HandleConditional(tokens, parseIndex + 1, lineNumber, branchName);
					if (info.conditional == null) {
						return null;
					}
					parseIndex += info.conditional.GetLength();
					if (parseIndex > tokens.Count - 1) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid arguments for '/option' command (branch {0}, line {1}). Proper syntax is '/option (availabilityConditional) \"[text]\" (conditional branchIndex) [branchIndex]'", branchName, lineNumber);
						return null;
					}
					int cBranch;
					if (!int.TryParse(tokens[parseIndex], out cBranch) || cBranch < -2) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid branch index '{0}' (branch {1}, line {2}).", tokens[parseIndex], branchName, lineNumber);
						return null;
					}
					info.conditionalBranch = cBranch;
					parseIndex++;
				}
				if (parseIndex > tokens.Count - 1) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid arguments for '/option' command (branch {0}, line {1}). Proper syntax is '/option (availabilityConditional) \"[text]\" (conditional branchIndex) [branchIndex]'", branchName, lineNumber);
					return null;
				}
				int branch;
				if (!int.TryParse(tokens[parseIndex], out branch) || branch < -2) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid branch index '{0}' (branch {1}, line {2}).", tokens[parseIndex], branchName, lineNumber);
					return null;
				}
				info.branch = branch;
				return info;
			}

			private static CommandInfo Jump (List<string> tokens, int lineNumber = -1, string branchName = "N/A") {
				CommandInfo info = new CommandInfo();
				info.type = CommandType.Jump;
				int branch;
				if (!int.TryParse(tokens[1], out branch) || branch < -2) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid branch index '{0}' (branch {1}, line {2}).", tokens[1], branchName, lineNumber);
					return null;
				}
				info.branch = branch;
				if (tokens.Count == 2) {
					return info;
				}
				if (tokens[2] == "if") {
					info.conditional = HandleConditional(tokens, 3, lineNumber, branchName);
					if (info.conditional == null) {
						return null;
					}
				}
				return info;
			}

			private static CommandInfo SetMood (string moodName, int lineNumber = -1, string branchName = "N/A") {
				CommandInfo info = new CommandInfo();
				info.type = CommandType.SetMood;
				Mood mood = VerifyMood(moodName, lineNumber, branchName);
				if (mood == Mood.InvalidMood) {
					return null;
				}
				info.mood = mood;
				return info;
			}

			private static CommandInfo SetStat (List<string> tokens, int lineNumber = -1, string branchName = "N/A") {
				CommandInfo info = new CommandInfo();
				info.type = CommandType.SetStat;
				//verify character
				Character character = VerifyCharacter(tokens[1], lineNumber, branchName);
				if (character == Character.InvalidName) {
					return null;
				}
				info.character = character;
				//verify stat
				Stat stat = VerifyStat(tokens[2], lineNumber, branchName);
				if (stat == Stat.InvalidStat) {
					return null;
				}
				info.stat = stat;
				//determine if relative
				bool relative = false;
				bool negate = false;
				int parseIndex = 3;
				if (tokens[3] == "+") {
					relative = true;
					parseIndex = 4;
				}
				else if (tokens[3] == "-") {
					relative = true;
					negate = true;
					parseIndex = 4;
				}
				if (parseIndex == 4) {
					if (tokens.Count != 5) {
						Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid arguments for '/set_stat' command (branch {0}, line {1}).", branchName, lineNumber);
						return null;
					}
				}
				info.relative = relative;
				info.negate = negate;
				//verify value
				float value = 0f;
				if (!float.TryParse(tokens[parseIndex], out value)) {
					Debug.LogErrorFormat("RenUnity.DialogueParser: Invalid value '{0}' for '/set_stat' command (branch {1}, line {2}).", tokens[parseIndex], branchName, lineNumber);
					return null;
				}
				info.value = value;
				return info;
			}

		}

	}

}
