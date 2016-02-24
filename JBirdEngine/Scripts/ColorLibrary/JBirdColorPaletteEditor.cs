using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JBirdEngine {

	namespace ColorLibrary {

		/// <summary>
		/// Color palette editor. Allows for changing palette in the inspector.
		/// </summary>
		#if UNITY_EDITOR
		[CustomEditor(typeof(JBirdColorPalette))]
		public class JBirdColorPaletteEditor : Editor {

			Color baseColor = Color.black;
			JBirdColorPalette targetPalette;

			void AddToUndoStack () {
				Undo.RecordObject(targetPalette, "JBirdColorPalette_ModifyColors");
			}

			void Analogous () {
				for (int i = 0; i < 5; i++) {
					targetPalette.colors[i].rgb = baseColor.ToHSV().ShiftHue(15 * (i - 2)).ToColor();
					if ((i + 1) % 2 == 0) {
						ColorHelper.ColorHSV hsv = targetPalette.colors[i].rgb.ToHSV();
						hsv.s *= 0.8f;
						hsv.v = Mathf.Min(hsv.v * 1.2f, 1f);
						targetPalette.colors[i].rgb = hsv.ToColor();
					}
				}
			}

			void AnalogousAccent () {
				for (int i = 0; i < 5; i++) {
					targetPalette.colors[i].rgb = baseColor.ToHSV().ShiftHue(15 * (i - 2)).ToColor();
					if ((i + 1) % 2 == 0) {
						ColorHelper.ColorHSV hsv = targetPalette.colors[i].rgb.ToHSV();
						hsv.s *= 0.8f;
						hsv.v = Mathf.Min(hsv.v * 1.2f, 1f);
						hsv.ShiftHue(180f);
						targetPalette.colors[i].rgb = hsv.ToColor();
					}
				}
				ColorHelper.ColorHSVRGB temp = targetPalette.colors[1];
				targetPalette.colors.RemoveAt(1);
				targetPalette.colors.Add(temp);
				temp = targetPalette.colors[2];
				targetPalette.colors.RemoveAt(2);
				targetPalette.colors.Add(temp);
			}

			void Monochromatic () {
				for (int i = 0; i < 5; i++) {
					ColorHelper.ColorHSV hsv = baseColor.ToHSV();
					if ((i + 2) % 3 == 0) {
						if (hsv.s > 0.5f) {
							hsv.s = hsv.s / 1.5f;
						}
						else {
							hsv.s = hsv.s * 1.5f;
						}
					}
					else if (i % 3 == 0) {
						if (hsv.s > 0.5f) {
							hsv.s = Mathf.Min(hsv.s * 1.5f, 1f);
						}
						else {
							hsv.s = hsv.s / 1.5f;
						}
					}
					if ((i + 1) % 2 == 0) {
						if (hsv.v > 0.5f) {
							hsv.v = hsv.v / 1.5f;
						}
						else {
							hsv.v = hsv.v * 1.5f;
						}
					}
					targetPalette.colors[i].rgb = hsv.ToColor();
				}
			}

			void Triad () {
				Monochromatic();
				targetPalette.colors[0].rgb = targetPalette.colors[0].rgb.ToHSV().ShiftHue(120f).ToColor();
				targetPalette.colors[3].rgb = targetPalette.colors[3].rgb.ToHSV().ShiftHue(-120f).ToColor();
				targetPalette.colors[4].rgb = targetPalette.colors[4].rgb.ToHSV().ShiftHue(-120f).ToColor();
			}

			void Complementary () {
				Monochromatic();
				targetPalette.colors[3].rgb = targetPalette.colors[3].rgb.ToHSV().ShiftHue(180f).ToColor();
				targetPalette.colors[4].rgb = targetPalette.colors[4].rgb.ToHSV().ShiftHue(180f).ToColor();
			}

			void Compound () {
				Monochromatic();
				targetPalette.colors[0].rgb = targetPalette.colors[0].rgb.ToHSV().ShiftHue(173f).ToColor();
				targetPalette.colors[1].rgb = targetPalette.colors[1].rgb.ToHSV().ShiftHue(162f).ToColor();
				targetPalette.colors[3].rgb = targetPalette.colors[3].rgb.ToHSV().ShiftHue(18f).ToColor();
				targetPalette.colors[4].rgb = targetPalette.colors[4].rgb.ToHSV().ShiftHue(18f).ToColor();
			}

			void ReverseCompound () {
				Monochromatic();
				targetPalette.colors[0].rgb = targetPalette.colors[0].rgb.ToHSV().ShiftHue(-173f).ToColor();
				targetPalette.colors[1].rgb = targetPalette.colors[1].rgb.ToHSV().ShiftHue(-162f).ToColor();
				targetPalette.colors[3].rgb = targetPalette.colors[3].rgb.ToHSV().ShiftHue(-18f).ToColor();
				targetPalette.colors[4].rgb = targetPalette.colors[4].rgb.ToHSV().ShiftHue(-18f).ToColor();
			}

			public override void OnInspectorGUI () {
				targetPalette = (JBirdColorPalette)target;
				baseColor = EditorGUILayout.ColorField("Base Color", baseColor);
				EditorGUILayout.Space();
				if (GUILayout.Button("Monochromatic")) {
					AddToUndoStack();
					Monochromatic();
					EditorUtility.SetDirty(target);
				}
				EditorGUILayout.Space();
				if (GUILayout.Button("Analogous")) {
					AddToUndoStack();
					Analogous();
					EditorUtility.SetDirty(target);
				}
				EditorGUILayout.Space();
				if (GUILayout.Button("Accented Analogous")) {
					AddToUndoStack();
					AnalogousAccent();
					EditorUtility.SetDirty(target);
				}
				EditorGUILayout.Space();
				if (GUILayout.Button("Complementary")) {
					AddToUndoStack();
					Complementary();
					EditorUtility.SetDirty(target);
				}
				EditorGUILayout.Space();
				if (GUILayout.Button("Triad")) {
					AddToUndoStack();
					Triad();
					EditorUtility.SetDirty(target);
				}
				EditorGUILayout.Space();
				if (GUILayout.Button("Compound")) {
					AddToUndoStack();
					Compound();
					EditorUtility.SetDirty(target);
				}
				EditorGUILayout.Space();
				if (GUILayout.Button("Reverse Compound")) {
					AddToUndoStack();
					ReverseCompound();
					EditorUtility.SetDirty(target);
				}
				EditorGUILayout.Space();
				foreach (ColorHelper.ColorHSVRGB color in targetPalette.colors) {
					EditorGUILayout.ColorField(color.rgb);
				}
				EditorGUILayout.Space();
				base.OnInspectorGUI();
				foreach (ColorHelper.ColorHSVRGB color in targetPalette.colors) {
					color.hsv = color.rgb.ToHSV();
				}
			}

		}
		#endif

	}

}
