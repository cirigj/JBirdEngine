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

        /// <summary>
        /// Returns n raised to the p power.
        /// </summary>
        /// <param name="n">Base.</param>
        /// <param name="p">Power.</param>
        /// <returns>n to the p</returns>
        public static float IntPow (float n, int p) {
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
        /// Attribute for drawing an enum as a mask field in inspector.
        /// </summary>
        public class EnumFlagsAttribute : PropertyAttribute {
            public EnumFlagsAttribute () { }
        }

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

        /// <summary>
        /// Removes the item from the list, but leaves an empty slot in its place with the default value.
        /// </summary>
        /// <typeparam name="T">The type of item stored in the list.</typeparam>
        /// <param name="passedList">The list to remove from.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns></returns>
        public static bool RemoveToDefault<T> (this List<T> passedList, T item) {
            if (passedList.Contains(item)) {
                passedList[passedList.IndexOf(item)] = default(T);
            }
            return false;
        }

        /// <summary>
        /// Adds the item to the first empty slot in the list (empty slot defined by the default value).
        /// </summary>
        /// <typeparam name="T">The type of item stored in the list (must inherit from class).</typeparam>
        /// <param name="passedList">The list to add to.</param>
        /// <param name="item">The item to add.</param>
        /// <param name="expandList">If false, won't expand the list (defaults to true).</param>
        /// <returns></returns>
        public static bool AddToFirstEmpty<T> (this List<T> passedList, T item, bool expandList = true) where T : class {
            for (int i = 0; i < passedList.Count; i++) {
                if (passedList[i] == default(T)) {
                    passedList[i] = item;
                    return true;
                }
            }
            if (expandList) {
                passedList.Add(item);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds the item to the beginning of the list.
        /// </summary>
        /// <typeparam name="T">The type of item stored in the list.</typeparam>
        /// <param name="passedList">The list to add to.</param>
        /// <param name="item">The item to add.</param>
        public static void AddToFront<T> (this List<T> passedList, T item) {
            List<T> tempList = new List<T>(passedList);
            passedList.Clear();
            passedList.Add(item);
            foreach (T entry in tempList) {
                passedList.Add(entry);
            }
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

    namespace Angles {

        /// <summary>
        /// A Vector3 where the x and y components are treated as azimuth and elevation angles, respectively.
        /// Can be implicitly cast as a Vector3.
        /// </summary>
        public class AngleVector3 {

            public AngleVector3 (Vector3 v) {
                vec = new Vector3(v.x % 360f, v.y % 360f, v.z);
            }

            private Vector3 vec;
            public Angle azimuth { get { return vec.x; } set { vec.x = value; } }
            public Angle elevation { get { return vec.y; } set { vec.y = value; } }
            public float magnitude { get { return vec.z; } set { vec.z = value; } }

            public float x { get { return vec.x % 360f; } set { vec.x = value % 360f; } }
            public float y { get { return vec.y % 360f; } set { vec.y = value % 360f; } }
            public float z { get { return vec.z; } set { vec.z = value; } }

            public static implicit operator Vector3 (AngleVector3 aVec) {
                return aVec.vec;
            }

            public static implicit operator AngleVector3 (Vector3 vec) {
                return new AngleVector3(vec);
            }

            public Vector3 ToVector3 () {
                return VectorHelper.FromAzimuthAndElevation(this) * vec.magnitude;
            }
        }

        /// <summary>
        /// A float that automatically uses modulo 360.
        /// Can be implicitly cast as a float.
        /// </summary>
        public class Angle {

            public Angle (float f) {
                val = f;
            }

            private float _val;
            public float val { get { return _val % 360f; } set { _val = value % 360f; } }

            public static implicit operator float (Angle angle) {
                return angle.val;
            }

            public static implicit operator Angle (float f) {
                return new Angle(f);
            }
        }

    }

    /// <summary>
    /// Contains helper functions for Vector3 type.
    /// </summary>
    public static class VectorHelper {

        /// <summary>
        /// Returns the midpoint between two vectors.
        /// </summary>
        /// <param name="v1">The first vector.</param>
        /// <param name="v2">The second vector.</param>
        /// <returns>The midpoint of the two vectors.</returns>
        public static Vector3 Midpoint (Vector3 v1, Vector3 v2) {
            return (v1 + v2) / 2f;
        }

        /// <summary>
        /// Returns a unit vector created via azimuth and elevation angles relative to the world coordinates.
        /// </summary>
        /// <param name="azimuth">Angle around the y-axis.</param>
        /// <param name="elevation">Angle between the desired vector and the xz-plane.</param>
        /// <returns>A unit vector with the given azimuth and elevation angles.</returns>
        public static Vector3 FromAzimuthAndElevation (float azimuth, float elevation) {
            return FromAzimuthAndElevation(azimuth, elevation, Vector3.up, Vector3.forward);
        }

        /// <summary>
        /// Returns a unit vector created via azimuth and elevation angles relative to the given directional vectors.
        /// </summary>
        /// <param name="angles">The set of angles to for the calculation.</param>
        /// <returns>A unit vector with the given azimuth and elevation angles.</returns>
        public static Vector3 FromAzimuthAndElevation (Angles.AngleVector3 angles) {
            return FromAzimuthAndElevation(angles, Vector3.up, Vector3.forward);
        }

        /// <summary>
        /// Returns a unit vector created via azimuth and elevation angles relative to the given directional vectors.
        /// </summary>
        /// <param name="azimuth">Angle around the y-axis.</param>
        /// <param name="elevation">Angle between the desired vector and the xz-plane.</param>
        /// <param name="up">The local positive y direction.</param>
        /// <param name="forward">The local positive z direction.</param>
        /// <returns>A unit vector with the given azimuth and elevation angles.</returns>
        public static Vector3 FromAzimuthAndElevation (float azimuth, float elevation, Vector3 up, Vector3 forward) {
            Vector3 compoundVec = Vector3.zero;
            up.Normalize();
            forward.Normalize();
            // relative x component
            compoundVec += Mathf.Sin(Mathf.Deg2Rad * azimuth) * Mathf.Cos(Mathf.Deg2Rad * elevation) * Vector3.Cross(up, forward);
            // relative y component
            compoundVec += Mathf.Sin(Mathf.Deg2Rad * elevation) * up;
            // relative z component
            compoundVec += Mathf.Cos(Mathf.Deg2Rad * azimuth) * Mathf.Cos(Mathf.Deg2Rad * elevation) * forward;
            return compoundVec;
        }

        /// <summary>
        /// Returns a unit vector created via azimuth and elevation angles relative to the given directional vectors.
        /// </summary>
        /// <param name="angles">The set of angles to for the calculation.</param>
        /// <param name="up">The local positive y direction.</param>
        /// <param name="forward">The local positive z direction.</param>
        /// <returns>A unit vector with the given azimuth and elevation angles.</returns>
        public static Vector3 FromAzimuthAndElevation (Angles.AngleVector3 angles, Vector3 up, Vector3 forward) {
            Vector3 compoundVec = Vector3.zero;
            up.Normalize();
            forward.Normalize();
            // relative x component
            compoundVec += Mathf.Sin(Mathf.Deg2Rad * angles.azimuth) * Mathf.Cos(Mathf.Deg2Rad * angles.elevation) * Vector3.Cross(up, forward);
            // relative y component
            compoundVec += Mathf.Sin(Mathf.Deg2Rad * angles.elevation) * up;
            // relative z component
            compoundVec += Mathf.Cos(Mathf.Deg2Rad * angles.azimuth) * Mathf.Cos(Mathf.Deg2Rad * angles.elevation) * forward;
            return compoundVec;
        }

        /// <summary>
        /// Get the elevation angle to this vector from a reference point.
        /// </summary>
        /// <param name="position">The vector to find the elevation of.</param>
        /// <param name="center">The reference point.</param>
        /// <param name="up">The normal of the plane to find the elevation from.</param>
        /// <returns>The elevation angle.</returns>
        public static float GetElevation (this Vector3 position, Vector3 center, Vector3 up) {
            return Mathf.Asin(Vector3.Dot((position - center).normalized, up.normalized)) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Get the azimuth angle to this vector from a reference point.
        /// </summary>
        /// <param name="position">The vector to find the azimuth of.</param>
        /// <param name="center">The reference point.</param>
        /// <param name="up">The normal of the reference plane.</param>
        /// <param name="forward">The forward direction to find the azimuth from in degrees.</param>
        /// <returns>The azimuth angle.</returns>
        public static float GetAzimuth (this Vector3 position, Vector3 center, Vector3 up, Vector3 forward) {
            float elevation;
            return position.GetAzimuth(center, up, forward, out elevation);
        }

        /// <summary>
        /// Get the azimuth angle to this vector from a reference point.
        /// </summary>
        /// <param name="position">The vector to find the azimuth of.</param>
        /// <param name="center">The reference point.</param>
        /// <param name="up">The normal of the reference plane.</param>
        /// <param name="forward">The forward direction to find the azimuth from in degrees.</param>
        /// <param name="elevation">Optional out parameter to get the elevation value used during azimuth calculation.</param>
        /// <returns>The azimuth angle.</returns>
        public static float GetAzimuth (this Vector3 position, Vector3 center, Vector3 up, Vector3 forward, out float elevation) {
            elevation = position.GetElevation(center, up);
            Vector3 right = Vector3.Cross(up.normalized, forward.normalized).normalized;
            float azimuth = Mathf.Acos(Vector3.Dot((position - center).normalized, forward.normalized) / Mathf.Cos(elevation * Mathf.Deg2Rad)) * Mathf.Rad2Deg;
            if (Vector3.Dot((position - center).normalized, right) > 0f) {
                azimuth = 360f - azimuth;
            }
            return azimuth;
        }

    }
	
}
