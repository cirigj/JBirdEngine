using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System;
#endif

namespace JBirdLib {

	namespace ColorLibrary {

		#if UNITY_EDITOR
		[CustomPropertyDrawer(typeof(MoreColors.BobRoss.ColorPalette))]
		public class BobRossDrawer : PropertyDrawer {

			public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
				return base.GetPropertyHeight (property, label) * 2f;
			}

			public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

				EditorGUI.BeginProperty(position, label, property);

				property.enumValueIndex = Convert.ToInt32(EditorGUI.EnumPopup(position, (MoreColors.BobRoss.ColorPalette)Enum.ToObject(typeof(MoreColors.BobRoss.ColorPalette), property.enumValueIndex)));
				position.height = position.height / 2f;
				position.y += position.height;

				EditorGUI.ColorField(position, MoreColors.BobRoss.EnumToColor((MoreColors.BobRoss.ColorPalette)Enum.ToObject(typeof(MoreColors.BobRoss.ColorPalette), property.enumValueIndex)));

				EditorGUI.EndProperty();

			}

		}
		#endif

	}

}
