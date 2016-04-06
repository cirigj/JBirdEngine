using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;

namespace JBirdEngine {

    /// <summary>
    /// Helpful math functions that don't exist in base UnityEngine.
    /// </summary>
    public static class MathHelper {

        /// <summary>
        /// Returns n raised to the p power.
        /// </summary>
        /// <param name="n">Base.</param>
        /// <param name="p">Power.</param>
        /// <returns>n to the p</returns>
        public static int IntPow (int n, int p) {
            if (p == 0) {
                return 1;
            }
            return n * IntPow(n, p - 1);
        }

    }

	/// <summary>
	/// Contains extension methods for Unity base functionality.
	/// </summary>
	public static class UnityHelper {

		/// <summary>
		/// Calls SetActive on the component's gameObject.
		/// </summary>
		public static void SetActive (this Component component, bool value) {
			component.gameObject.SetActive(value);
		}

	}

	/// <summary>
	/// Contains functions for managing singleton classes.
	/// </summary>
	public static class Singleton {

        /// <summary>
        /// THERE CAN ONLY BE ONE (Makes sure there's only one of this class). For use in Awake();
        /// </summary>
        /// <param name="instance">This instance.</param>
        /// <param name="singleton">Singleton variable.</param>
        /// <typeparam name="T">Must inherit from Component.</typeparam>
        public static void ManageSingleton<T> (T instance, ref T singleton) where T : Component {
            if (singleton == null) {
                singleton = instance;
            }
            else {
                if (Application.isPlaying) {
                    GameObject.Destroy(instance.gameObject);
                }
            }
        }

	}

	/// <summary>
	/// Contains functions for easily making enums into flags.
	/// </summary>
	public static class EnumHelper {

		/// <summary>
		/// Returns an enum that is a combination of the given flags.
		/// </summary>
		/// <param name="flags">Flags to combine.</param>
		/// <typeparam name="T">Must be an enum.</typeparam>
		public static T CombineFlags<T> (params T[] flags) where T : IConvertible, IFormattable, IComparable {
			if (!typeof(T).IsEnum) {
				throw new ArgumentException ("CombineFlags<T>(): 'T' must be of type 'enum'");
			}
			T newFlags = (T)Enum.ToObject(typeof(T), 0);
			foreach (T flag in flags) {
				newFlags = (T)Enum.ToObject(typeof(T), Convert.ToInt32(newFlags) | Convert.ToInt32(flag));
			}
			return newFlags;
		}

		/// <summary>
		/// Returns the collection of flags that have been toggled.
		/// </summary>
		/// <param name="flag">Base collection of flags.</param>
		/// <param name="toggleList">List of flags to toggle.</param>
		/// <typeparam name="T">Must be an enum.</typeparam>
		public static T ToggleFlags<T> (T flag, params T[] toggleList) where T : IConvertible, IFormattable, IComparable {
			if (!typeof(T).IsEnum) {
				throw new ArgumentException ("ToggleFlags<T>(): 'T' must be of type 'enum'");
			}
			T newFlags = flag;
			foreach (T toggle in toggleList) {
				newFlags = (T)Enum.ToObject(typeof(T), Convert.ToInt32(newFlags) ^ Convert.ToInt32(toggle));
			}
			return newFlags;
		}

		/// <summary>
		/// Returns the base collection of flags minus the flags from the list.
		/// </summary>
		/// <param name="flag">Base collection of flags.</param>
		/// <param name="removeList">List of flags to toggle.</param>
		/// <typeparam name="T">Must be an enum.</typeparam>
		public static T RemoveFlags<T> (T flag, params T[] removeList) where T : IConvertible, IFormattable, IComparable {
			if (!typeof(T).IsEnum) {
				throw new ArgumentException ("RemoveFlags<T>(): 'T' must be of type 'enum'");
			}
			T newFlags = flag;
			foreach (T remove in removeList) {
				newFlags = (T)Enum.ToObject(typeof(T), Convert.ToInt32(newFlags) ^ Convert.ToInt32(remove));
				newFlags = (T)Enum.ToObject(typeof(T), Convert.ToInt32(flag) & Convert.ToInt32(newFlags));
				flag = newFlags;
			}
			return newFlags;
		}

		/// <summary>
		/// Returns whether or not a collection of flags contains another collection of flags.
		/// </summary>
		/// <returns><c>true</c>, if flag contained checkFor, <c>false</c> otherwise.</returns>
		/// <param name="flag">Base collection of flags.</param>
		/// <param name="checkFor">Collection of flags to check for.</param>
		/// <typeparam name="T">Must be an enum.</typeparam>
		public static bool ContainsFlag<T> (T flag, T checkFor) where T : IConvertible, IFormattable, IComparable {
			if (!typeof(T).IsEnum) {
				throw new ArgumentException ("ContainsFlag<T>(): 'T' must be of type 'enum'");
			}
			return (Convert.ToInt32(flag) & Convert.ToInt32(checkFor)) == Convert.ToInt32(checkFor);
		}

        /// <summary>
        /// Attempts to convert a string value to an enum value of the given type. Returns boolean based on success.
        /// </summary>
        /// <typeparam name="T">Enum type to convert to.</typeparam>
        /// <param name="value">String to convert.</param>
        /// <param name="returnEnum">Enum created from string.</param>
        /// <returns>True if the enum can be parsed from the string, false otherwise.</returns>
        public static bool TryParse<T> (string value, out T returnEnum) where T : IConvertible, IFormattable, IComparable {
            if (!typeof(T).IsEnum) {
                throw new ArgumentException("TryParse<T>(): 'T' must be of type 'enum'");
            }
            returnEnum = default(T);
            if (Enum.IsDefined(typeof(T), value)) {
                returnEnum = (T)Enum.Parse(typeof(T), value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to convert a string value to an enum value of the given type. Returns default on failure.
        /// </summary>
        /// <typeparam name="T">Enum type to convert to.</typeparam>
        /// <param name="value">String to convert.</param>
        /// <returns>Enum value from string, or default if TryParse fails.</returns>
        public static T ToEnum<T> (this string value) where T : IConvertible, IFormattable, IComparable {
            if (!typeof(T).IsEnum) {
                throw new ArgumentException("ToEnum<T>(): 'T' must be of type 'enum'");
            }
            T enumValue;
            if (!TryParse<T>(value, out enumValue)) {
                Debug.LogErrorFormat("ToEnum<{0}>(): Value '{1}' does not exist within {0}.", typeof(T), value);
            }
            return enumValue;
        }

	}

	/// <summary>
	/// Contains functions for list management and statistics.
	/// </summary>
	public static class ListHelper {

		/// <summary>
		/// Creates a list out of the parameters.
		/// </summary>
		/// <returns>A new list containing the objects passed as parameters.</returns>
		/// <param name="objects">Objects to put in a new list.</param>
		public static List<T> ListFromObjects<T> (params T[] objects) {
			List<T> newList = new List<T>();
			foreach (T obj in objects) {
				newList.Add(obj);
			}
			return newList;
		}

		/// <summary>
		/// Returns the element of the list which is the closest to a given position (or null if none are within a specified range).
		/// </summary>
		/// <param name="list">List to check.</param>
		/// <param name="position">Position to check against.</param>
		/// <param name="maxDist">Max distance the element can be from the position in question (defaults to Mathf.Infinity).</param>
		public static T GetClosestToPosition<T> (List<T> list, Vector3 position, float maxDist = Mathf.Infinity) where T : Component {
			T bestObj = null;
			foreach (T obj in list) {
				float dist = Vector3.Distance(obj.transform.position, position);
				if (dist < maxDist) {
					maxDist = dist;
					bestObj = obj;
				}
			}
			return bestObj;
		}

		/// <summary>
		/// Returns the element of the list which is the closest to a given position within a certain range (or null if none are within range).
		/// </summary>
		/// <param name="list">List to check.</param>
		/// <param name="position">Position to check against.</param>
		/// <param name="maxDist">Max distance the element can be from the position in question.</param>
		public static T GetClosestWithinRange<T> (List<T> list, Vector3 position, float maxDist) where T : Component {
			return GetClosestToPosition(list, position, maxDist);
		}

        /// <summary>
        /// Returns the first element from a list and then removes it from the list (Returns default of the specified type if list is empty).
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The list to pop from.</param>
        /// <param name="hideWarnings">Set to true to disable warning messages if the list is empty.</param>
        /// <returns>First element of supplied list.</returns>
        public static T PopFront<T> (this List<T> list, bool hideWarnings = false) {
            if (list.Count == 0) {
                if (!hideWarnings) {
                    Debug.LogWarningFormat("List<{0}>.PopFront(): List is empty! Returning default {0}.", typeof(T));
                }
                return default(T);
            }
            T temp = list[0];
            list.RemoveAt(0);
            return temp;
        }

	}

	/// <summary>
	/// Contains helper functions for char type.
	/// </summary>
	public static class CharHelper {

		/// <summary>
		/// Capitalize the specified char (must be alphabetical).
		/// </summary>
		public static char Capitalize (this char c) {
			int i = (int)c;
			if (97 <= i && i <= 122) {
				return (char)(i - 32);
			}
			else {
				return c;
			}
		}

		/// <summary>
		/// Lowercase the specified char (must be alphabetical).
		/// </summary>
		public static char Lowercase (this char c) {
			int i = (int)c;
			if (65 <= i && i <= 90) {
				return (char)(i + 32);
			}
			else {
				return c;
			}
		}

	}
	
}
