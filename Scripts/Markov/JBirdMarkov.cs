using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JBirdLib {

	/// <summary>
	/// Class containing Markov Chain implementations.
	/// </summary>
	public static class Markov {

		/// <summary>
		/// Converts char to int index (for name generator).
		/// </summary>
		public static int ToInt (this char c) {
			int val = (int)c.Lowercase() - 97;
			if (0 <= val && val <= 25) {
				return val;
			}
			else if (c == '/') {
				return 26;
			}
			else {
				Debug.Log ("Markov: Invalid character.");
				return -1;
			}
		}

		/// <summary>
		/// Converts int index to char (for name generator).
		/// </summary>
		public static char ToChar (this int i) {
			if (0 <= i && i <= 25) {
				return (char)(i + 97);
			}
			else if (i == 26) {
				return '/';
			}
			else {
				Debug.Log ("Markov: Invalid character index.");
				return ' ';
			}
		}

		/// <summary>
		/// Class for containing information about char frequency (for name generator).
		/// </summary>
		[System.Serializable]
		public class CharFrequency {
			
			public char character;
			public int frequency;
			
			public CharFrequency (char c) {
				character = c;
				frequency = 0;
			}
			
		}

		/// <summary>
		/// Class for containing information for char-based markov chains (for name generator).
		/// </summary>
		[System.Serializable]
		public class NameChain {
			
			public List<CharFrequency> nextChars;
			public int totalFrequency;

			/// <summary>
			/// Initializer.
			/// </summary>
			public NameChain () {
				nextChars = new List<CharFrequency>();
				for (int i = 0; i < 27; i++) {
					nextChars.Add(new CharFrequency(i.ToChar()));
				}
				totalFrequency = 0;
			}

			/// <summary>
			/// Increments the frequency of the specified char for this chain.
			/// </summary>
			public void AddNextChar (char c) {
				if (c.ToInt() != -1) {
					totalFrequency++;
					nextChars[c.ToInt()].frequency++;
				}
				else {
					Debug.LogWarningFormat("Markov.NameChain: Non-alphabet char {0} cannot be used.", c);
				}
			}

			/// <summary>
			/// Decrements the frequency of the specified char for this chain.
			/// </summary>
			public void RemoveNextChar (char c) {
				if (c.ToInt() != -1) {
					totalFrequency--;
					nextChars[c.ToInt()].frequency--;
				}
				else {
					Debug.LogWarningFormat("Markov.NameChain: Non-alphabet char {0} cannot be used.", c);
				}
			}

			/// <summary>
			/// Returns a random character that has a non-zero frequency.
			/// </summary>
			public char GetRandomNext () {
				int rand = Random.Range(0, totalFrequency);
				int selector = 0;
				int i = 0;
				while (i < nextChars.Count) {
					if (nextChars[i].frequency > 0) {
						int j = nextChars[i].frequency;
						while (j > 0) {
							selector++;
							j--;
							if (selector > rand) {
								return nextChars[i].character;
							}
						}
					}
					i++;
				}
				return '/';
			}
			
		}

		/// <summary>
		/// Markov Name Generator.
		/// </summary>
		[System.Serializable]
		public class NameGenerator {
			
			public List<string> baseNames;
			public List<NameChain> chains;
			public List<string> savedNames;

			/// <summary>
			/// Initializer.
			/// </summary>
			public NameGenerator () {
				baseNames = new List<string>();
				chains = new List<NameChain>();
				for (int i = 0; i < 676; i++) {
					chains.Add(new NameChain());
				}
			}

			/// <summary>
			/// Returns the index of the bin containing the chain which follows the pair of consecutive chars specified.
			/// </summary>
			int CharsToIndex (char a, char b) {
				if (0 <= a.Lowercase().ToInt() && a.Lowercase().ToInt() <= 25 && 0 <= b.Lowercase().ToInt() && b.Lowercase().ToInt() <= 25) {
					return 26 * a.Lowercase().ToInt() + b.Lowercase().ToInt();
				}
				else {
					Debug.LogErrorFormat("Markov.NameGenerator: Invalid character pair ({0},{1})", a, b);
					return -1;
				}
			}

			/// <summary>
			/// Checks if a chain exists from the specified pair of consecutive chars.
			/// </summary>
			bool ChainExistsFromPair (char a, char b) {
				return (chains[CharsToIndex(a,b)].totalFrequency > 0 && chains[CharsToIndex(a,b)].nextChars[26].frequency != chains[CharsToIndex(a,b)].totalFrequency);
			}

			/// <summary>
			/// Checks if a chain exists from the specified pair index.
			/// </summary>
			bool ChainExistsFromPair (int id) {
				return (chains[id].totalFrequency > 0 && chains[id].nextChars[26].frequency != chains[id].totalFrequency);
			}

			/// <summary>
			/// Adds the specified name to the database.
			/// </summary>
			public bool AddName (string name) {
				char[] chars = name.ToCharArray();
				if (chars.Length < 3) {
					Debug.LogErrorFormat("Markov.NameGenerator: Invalid name \"{0}\" (must be at least 3 characters in length).", name);
					return false;
				}
				if (baseNames.Contains(name)) {
					Debug.LogWarningFormat("Markov.NameGenerator: Name \"{0}\" has already been entered. Adding additional weight.", name);
				}
				for (int i = 2; i < chars.Length; i++) {
					chains[CharsToIndex(chars[i-2],chars[i-1])].AddNextChar(chars[i]);
				}
				chains[CharsToIndex(chars[chars.Length - 2], chars[chars.Length - 1])].AddNextChar('/');
				baseNames.Add(name);
				return true;
			}

			/// <summary>
			/// Removes the specified name from the database.
			/// </summary>
			public bool RemoveName (string name) {
				if (!baseNames.Contains(name)) {
					Debug.LogErrorFormat("Markov.NameGenerator: Name \"{0}\" cannot be removed because it does not exist.", name);
					return false;
				}
				char[] chars = name.ToCharArray();
				for (int i = 2; i < chars.Length; i++) {
					chains[CharsToIndex(chars[i-2],chars[i-1])].RemoveNextChar(chars[i]);
				}
				chains[CharsToIndex(chars[chars.Length - 2], chars[chars.Length - 1])].RemoveNextChar('/');
				baseNames.Remove(name);
				return true;
			}

			/// <summary>
			/// Removes all names from the database.
			/// </summary>
			public void RemoveAllNames () {
				while (baseNames.Count > 0) {
					RemoveName(baseNames[0]);
				}
			}

			/// <summary>
			/// Generates and returns a name using the base names as examples.
			/// </summary>
			public string GenerateName (int minLength = 3, int maxLength = 10, char first = 'a', char second = 'b') {
				if (baseNames.Count < 1) {
					Debug.LogError("Markov.NameGenerator: No base names supplied!");
					return "Nameless";
				}
				bool nameFinished = false;
				bool chainOkay = false;
				int pairIndex = -1;
				char a = first;
				char b = second;
				char c = 'c';
				string name = "";
				while (!chainOkay) {
					pairIndex = Random.Range(0, 676);
					if (ChainExistsFromPair(pairIndex)) {
						chainOkay = true;
						a = (pairIndex / 26).ToChar();
						b = (pairIndex % 26).ToChar();
						name += a.Capitalize();
						name += b;
					}
				}
				while (!nameFinished) {
					c = chains[pairIndex].GetRandomNext();
					if (c != '/') {
						name += c;
						a = b;
						b = c;
						pairIndex = CharsToIndex(a,b);
						if (name.Length > maxLength) {
							break;
						}
					}
					else {
						if (name.Length >= minLength) {
							nameFinished = true;
						}
						break;
					}
				}
				if (nameFinished) {
					return name;
				}
				else {
					return GenerateName(minLength, maxLength, first, second);
				}
			}

			/// <summary>
			/// Saves a generated name (called by editor).
			/// </summary>
			public void SaveName (string name) {
				savedNames.Add(name);
			}

			/// <summary>
			/// Clears all saved names.
			/// </summary>
			public void ClearSavedNames () {
				savedNames.Clear();
			}
			
		}
		
	}

}
