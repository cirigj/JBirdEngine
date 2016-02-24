using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JBirdEngine {

	namespace ColorLibrary {

		/// <summary>
		/// Color palette with 5 colors. Can be modified in the inspector using the JBirdPaletteEditor.
		/// </summary>
		[CreateAssetMenu]
		public class JBirdColorPalette : ScriptableObject {

			/// <summary>
			/// The colors on this palette (HSV & RGB).
			/// </summary>
			public List<ColorHelper.ColorHSVRGB> colors;

			/// <summary>
			/// The colors on this palette (RGB only).
			/// </summary>
			public List<Color> colorList {
				get { 
					List<Color> getList = new List<Color>();
					foreach (ColorHelper.ColorHSVRGB color in colors) {
						getList.Add(color.rgb);
					}
					return getList;
				}
			}

			/// <summary>
			/// Initializer.
			/// </summary>
			public JBirdColorPalette () {
				colors = new List<ColorHelper.ColorHSVRGB>();
				for (int i = 0; i < 5; i++) {
					colors.Add(new ColorHelper.ColorHSVRGB());
					colors[i].rgb = Color.black;
					colors[i].hsv = colors[i].rgb.ToHSV();
				}
			}

		}

	}

}
