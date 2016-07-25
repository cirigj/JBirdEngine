//Comment out the following line if you're not using the JBirdEngine Color Library
#define COLOR_LIB

using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if COLOR_LIB
using JBirdEngine.ColorLibrary;
#endif

namespace JBirdEngine {

	namespace EditorHelper {

		/// <summary>
		/// View only attribute (for greying out in inspector).
		/// </summary>
		public class ViewOnlyAttribute : PropertyAttribute {
            public ViewOnlyAttribute () { }
        }

		#if UNITY_EDITOR
		[CustomPropertyDrawer(typeof(ViewOnlyAttribute))]
		public class ViewOnlyDrawer : PropertyDrawer {
			public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
				return EditorGUI.GetPropertyHeight(property, label, true);
			}
			
			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
				GUI.enabled = false;
				EditorGUI.PropertyField(position, property, label, true);
				GUI.enabled = true;
			}
		}

        [CustomPropertyDrawer(typeof(EnumHelper.EnumFlagsAttribute))]
        public class EnumFlagsPropertyDrawer : PropertyDrawer {

            public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
                property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
            }

        }

        #if COLOR_LIB
        [CustomPropertyDrawer(typeof(ColorHelper.ColorHSVRGB))]
		public class ColorHSVRGBDrawer : PropertyDrawer {

			public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
				return base.GetPropertyHeight (property, label) * 4f;
			}

			public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
				EditorGUI.BeginProperty(position, label, property);

				EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

				position.height = position.height / 4f;
				position.y += position.height;

				EditorGUI.indentLevel += 1;

				Rect contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("RGB"));

				EditorGUI.indentLevel = 0;

				Rect redRect = new Rect(contentPosition.x, contentPosition.y, contentPosition.width * .3f, contentPosition.height);
				Rect greenRect = new Rect(contentPosition.x + contentPosition.width * .35f, contentPosition.y, contentPosition.width * .3f, contentPosition.height);
				Rect blueRect = new Rect(contentPosition.x + contentPosition.width * .7f, contentPosition.y, contentPosition.width * .3f, contentPosition.height);

				EditorGUIUtility.labelWidth = 14f;

				EditorGUI.PropertyField(redRect, property.FindPropertyRelative("rgb.r"), new GUIContent("R"));
				EditorGUI.PropertyField(greenRect, property.FindPropertyRelative("rgb.g"), new GUIContent("G"));
				EditorGUI.PropertyField(blueRect, property.FindPropertyRelative("rgb.b"), new GUIContent("B"));

				EditorGUI.indentLevel += 1;

				EditorGUIUtility.labelWidth = 0f;
				position.y += position.height;

				contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("HSV"));
				EditorGUI.indentLevel = 0;
				EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("hsv"), GUIContent.none);

				EditorGUI.indentLevel += 1;

				EditorGUIUtility.labelWidth = 0f;
				position.y += position.height;
				contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent("Color"));

				Rect colorRect = new Rect(contentPosition.x, contentPosition.y, contentPosition.width, contentPosition.height);

				EditorGUI.PropertyField(colorRect, property.FindPropertyRelative("rgb"), GUIContent.none);

				EditorGUI.EndProperty();
			}

		}

		[CustomPropertyDrawer(typeof(ColorHelper.ColorHSV))]
		public class ColorHSVDrawer : PropertyDrawer {

			public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
				return base.GetPropertyHeight (property, label) * 1f;
			}

			public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
				EditorGUI.BeginProperty(position, label, property);

				EditorGUIUtility.labelWidth = 0f;
				Rect contentPosition = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
				
				EditorGUI.indentLevel = 0;
				
				Rect hueRect = new Rect(contentPosition.x, contentPosition.y, contentPosition.width * .3f, contentPosition.height);
				Rect satRect = new Rect(contentPosition.x + contentPosition.width * .35f, contentPosition.y, contentPosition.width * .3f, contentPosition.height);
				Rect valRect = new Rect(contentPosition.x + contentPosition.width * .7f, contentPosition.y, contentPosition.width * .3f, contentPosition.height);
				
				EditorGUIUtility.labelWidth = 14f;
				
				EditorGUI.PropertyField(hueRect, property.FindPropertyRelative("h"), new GUIContent("H"));
				EditorGUI.PropertyField(satRect, property.FindPropertyRelative("s"), new GUIContent("S"));
				EditorGUI.PropertyField(valRect, property.FindPropertyRelative("v"), new GUIContent("V"));

				EditorGUI.EndProperty();
			}
		}
		#endif

		#endif

	}

}
