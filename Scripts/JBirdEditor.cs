//Comment out the following line if you're not using the JBirdEngine Color Library
#define COLOR_LIB

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

        public delegate Vector3 PositionHandleDelegate (Vector3 v3, Quaternion quat);
        public delegate bool ButtonHandleCapDelegate (Vector3 v3, Quaternion quat, float f1, float f2, Handles.CapFunction capFunc);
        public delegate bool ButtonHandleDrawCapDelegate (Vector3 v3, Quaternion quat, float f1, float f2, Handles.DrawCapFunction drawCapFunc);
        public delegate Quaternion DiscHandleDelegate (Quaternion quat, Vector3 v31, Vector3 v32, float f1, bool b, float f2);
        public delegate Quaternion RotationHandleDelegate (Quaternion quat, Vector3 v3);
        public delegate Vector3 ScaleHandleDelegate (Vector3 v31, Vector3 v32, Quaternion quat, float f);
        public delegate Vector3 FreeMoveHandleCapDelegate (Vector3 v31, Quaternion quat, float f, Vector3 v32, Handles.CapFunction capFunc);
        public delegate Vector3 FreeMoveHandleDrawCapDelegate (Vector3 v31, Quaternion quat, float f, Vector3 v32, Handles.DrawCapFunction drawCapFunc);
        public delegate Quaternion FreeRotateHandleDelegate (Quaternion quat, Vector3 v3, float f);
        public delegate float RadiusHandleDelegate (Quaternion quat, Vector3 v3, float f);
        public delegate float RadiusHandleBoolsOnlyDelegate (Quaternion quat, Vector3 v3, float f, bool b);
        public delegate float ScaleSliderDelegate (float f1, Vector3 v31, Vector3 v32, Quaternion quat, float f2, float f3);
        public delegate float ScaleValueHandleCapDelegate (float f1, Vector3 v3, Quaternion quat, float f2, Handles.CapFunction capFunc, float f3);
        public delegate float ScaleValueHandleDrawCapDelegate (float f1, Vector3 v3, Quaternion quat, float f2, Handles.DrawCapFunction drawCapFunc, float f3);
        public delegate Vector3 SliderDelegate (Vector3 v31, Vector3 v32);
        public delegate Vector3 SliderCapDelegate (Vector3 v31, Vector3 v32, float f1, Handles.CapFunction capFunc, float f2);
        public delegate Vector3 SliderDrawCapDelegate (Vector3 v31, Vector3 v32, float f1, Handles.DrawCapFunction drawCapFunc, float f2);

        public static class Utilities {

            public static void HandleCustomEditorMethod (this Editor editor, System.Action method) {
                Undo.RecordObject(editor.target, string.Format("{0}{1}", editor.target.GetType().ToString(), method.Method.Name));
                method.Invoke();
                EditorUtility.SetDirty(editor.target);
                SceneView.RepaintAll();
                editor.Repaint();
            }

            public static void UpdateVariableViaHandle<T> (this Editor editor, ref T var, System.Delegate handleMethod, params object[] handleParams) {
                UpdateVariableViaHandle(editor, ref var, handleMethod, "", handleParams);
            }

            public static void UpdateVariableViaHandle<T> (this Editor editor, ref T var, System.Delegate handleMethod, string undoName, params object[] handleParams) {
                EditorGUI.BeginChangeCheck();
                T value = default(T);
                try {
                    value = (T)handleMethod.DynamicInvoke(handleParams);
                }
                catch (System.Exception e) {
                    Debug.LogErrorFormat("JBirdEditor.UpdateListIndexViaHandle: Method '{0}' does not return type '{1}'. Exception message: {2}", handleMethod.Method.Name, typeof(T).ToString(), e.Message);
                    EditorGUI.EndChangeCheck();
                    return;
                }

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(editor.target, string.Format("{0}Change{1}", editor.target.GetType().ToString(), undoName == "" ? handleMethod.Method.Name : undoName));
                    var = value;
                    EditorUtility.SetDirty(editor.target);
                    SceneView.RepaintAll();
                    editor.Repaint();
                }
            }

            public static void UpdateListIndexViaHandle<T> (this Editor editor, ref List<T> list, int index, System.Delegate handleMethod, params object[] handleParams) {
                UpdateListIndexViaHandle(editor, ref list, index, handleMethod, "", handleParams);
            }

            public static void UpdateListIndexViaHandle<T> (this Editor editor, ref List<T> list, int index, System.Delegate handleMethod, string undoName, params object[] handleParams) {
                EditorGUI.BeginChangeCheck();
                T value = default(T);
                try {
                    value = (T)handleMethod.DynamicInvoke(handleParams);
                }
                catch (System.Exception e) {
                    Debug.LogErrorFormat("JBirdEditor.UpdateListIndexViaHandle: Method '{0}' does not return type '{1}'. Exception message: {2}", handleMethod.Method.Name, typeof(T).ToString(), e.Message);
                    EditorGUI.EndChangeCheck();
                    return;
                }

                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(editor.target, string.Format("{0}Change{1}AtIndex{2}", editor.target.GetType().ToString(), undoName == "" ? handleMethod.Method.Name : undoName, index.ToString()));
                    list[index] = value;
                    EditorUtility.SetDirty(editor.target);
                    SceneView.RepaintAll();
                    editor.Repaint();
                }
            }

        }

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
