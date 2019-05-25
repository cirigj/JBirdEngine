using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using System;
#endif

namespace JBirdLib {

    namespace ColorLibrary {

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(MoreColors.Vaporwave.ColorPalette))]
        public class VaporwaveDrawer : PropertyDrawer {

            public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
                return base.GetPropertyHeight(property, label) * 2f;
            }

            public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {

                EditorGUI.BeginProperty(position, label, property);

                property.enumValueIndex = Convert.ToInt32(EditorGUI.EnumPopup(position, (MoreColors.Vaporwave.ColorPalette)Enum.ToObject(typeof(MoreColors.Vaporwave.ColorPalette), property.enumValueIndex)));
                position.height = position.height / 2f;
                position.y += position.height;

                EditorGUI.ColorField(position, MoreColors.Vaporwave.EnumToColor((MoreColors.Vaporwave.ColorPalette)Enum.ToObject(typeof(MoreColors.Vaporwave.ColorPalette), property.enumValueIndex)));

                EditorGUI.EndProperty();

            }

        }
#endif

    }

}
