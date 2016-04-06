using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JBirdEngine {

    namespace ColorLibrary {

        /// <summary>
        /// More colors; because Unity's base colors just aren't enough.
        /// </summary>
        public static class MoreColors {

            /// <summary>
            /// I'll tell you what, you know me; let's have some fun.
            /// </summary>
            public static class BobRoss {

                /// <summary>
                /// An enum that contains all the colors of the Bob Ross palette.
                /// </summary>
                public enum ColorPalette {
                    happyLittleAccident,
                    alizarinCrimson,
                    brightRed,
                    permanentRed,
                    cadmiumYellow,
                    darkSienna,
                    indianYellow,
                    midnightBlack,
                    phthaloBlue,
                    phthaloGreen,
                    prussianBlue,
                    sapGreen,
                    titaniumWhite,
                    vanDykeBrown,
                    yellowOchre
                }

                /// <summary>
                /// Converts ColorPalette enum value to a Color.
                /// </summary>
                /// <param name="enumValue">Enum value.</param>
                public static Color EnumToColor (ColorPalette enumValue) {
                    switch (enumValue) {
                        case ColorPalette.happyLittleAccident:
                            return HappyLittleAccident();
                        case ColorPalette.alizarinCrimson:
                            return alizarinCrimson;
                        case ColorPalette.brightRed:
                            return brightRed;
                        case ColorPalette.permanentRed:
                            return permanentRed;
                        case ColorPalette.cadmiumYellow:
                            return cadmiumYellow;
                        case ColorPalette.darkSienna:
                            return darkSienna;
                        case ColorPalette.indianYellow:
                            return indianYellow;
                        case ColorPalette.midnightBlack:
                            return midnightBlack;
                        case ColorPalette.phthaloBlue:
                            return phthaloBlue;
                        case ColorPalette.phthaloGreen:
                            return phthaloGreen;
                        case ColorPalette.prussianBlue:
                            return prussianBlue;
                        case ColorPalette.sapGreen:
                            return sapGreen;
                        case ColorPalette.titaniumWhite:
                            return titaniumWhite;
                        case ColorPalette.vanDykeBrown:
                            return vanDykeBrown;
                        case ColorPalette.yellowOchre:
                            return yellowOchre;
                        default:
                            return HappyLittleAccident();
                    }
                }

                /// <summary>
                /// One of the most versatile of the Bob Ross colors.
                /// </summary>
                public static Color alizarinCrimson {
                    get { return ColorHelper.ToColor(0xE32636); }
                }

                /// <summary>
                /// Bob’s customary color for signing his paintings.
                /// </summary>
                public static Color brightRed {
                    get { return ColorHelper.ToColor(0xAA0114); }
                }

                /// <summary>
                /// An oldie, but a goodie.
                /// </summary>
                public static Color permanentRed {
                    get { return ColorHelper.ToColor(0x742C1E); }
                }

                /// <summary>
                /// Sometimes used to indicate the brilliant sunlight in the sky.
                /// </summary>
                public static Color cadmiumYellow {
                    get { return ColorHelper.ToColor(0xFFF600); }
                }

                /// <summary>
                /// The lighter of the two Bob Ross brown colors.
                /// </summary>
                public static Color darkSienna {
                    get { return ColorHelper.ToColor(0x3C1414); }
                }

                /// <summary>
                /// Occasionally Bob will use Indian Yellow to paint the sun in the sky of his painting.
                /// </summary>
                public static Color indianYellow {
                    get { return ColorHelper.ToColor(0xE3A857); }
                }

                /// <summary>
                /// Often used as a base color to block in the basic shapes of trees and bushes.
                /// </summary>
                public static Color midnightBlack {
                    get { return ColorHelper.ToColor(0x000316); }
                }

                /// <summary>
                /// A very strong color, most commonly used to paint skies and water.
                /// </summary>
                public static Color phthaloBlue {
                    get { return ColorHelper.ToColor(0x000F89); }
                }

                /// <summary>
                /// An almost fluorescent green color.
                /// </summary>
                public static Color phthaloGreen {
                    get { return ColorHelper.ToColor(0x123524); }
                }

                /// <summary>
                /// Often found in the skies of Bob’s frigid winter scenes.
                /// </summary>
                public static Color prussianBlue {
                    get { return ColorHelper.ToColor(0x003153); }
                }

                /// <summary>
                /// Used primarily for all things foliage.
                /// </summary>
                public static Color sapGreen {
                    get { return ColorHelper.ToColor(0x507D2A); }
                }

                /// <summary>
                /// The most consistently used of all the Bob Ross colors.
                /// </summary>
                public static Color titaniumWhite {
                    get { return ColorHelper.ToColor(0xFCFFF0); }
                }

                /// <summary>
                /// A "go-to" color for all of your dark basecoat needs.
                /// </summary>
                public static Color vanDykeBrown {
                    get { return ColorHelper.ToColor(0x584630); }
                }

                /// <summary>
                /// A beautiful soft hue, excellent for highlighting foliage.
                /// </summary>
                public static Color yellowOchre {
                    get { return ColorHelper.ToColor(0xF5C52C); }
                }

                /// <summary>
                /// There are no mistakes, just happy little accidents.
                /// </summary>
                /// <returns>A random landscape paint color.</returns>
                public static Color HappyLittleAccident () {
                    int colorIndex = UnityEngine.Random.Range(0, 14);
                    switch (colorIndex) {
                        case 0:
                            return alizarinCrimson;
                        case 1:
                            return brightRed;
                        case 2:
                            return permanentRed;
                        case 3:
                            return cadmiumYellow;
                        case 4:
                            return darkSienna;
                        case 5:
                            return indianYellow;
                        case 6:
                            return midnightBlack;
                        case 7:
                            return phthaloBlue;
                        case 8:
                            return phthaloGreen;
                        case 9:
                            return prussianBlue;
                        case 10:
                            return sapGreen;
                        case 11:
                            return titaniumWhite;
                        case 12:
                            return vanDykeBrown;
                        case 13:
                            return yellowOchre;
                        default:
                            return titaniumWhite;
                    }
                }

            }

            /// <summary>
            /// The people demanded it, so I delivered. You're welcome, 90's kids.
            /// </summary>
            public static class Vaporwave {

                /// <summary>
                /// It's an enum, but for vaporwave color indices.
                /// </summary>
                public enum ColorPalette {
                    yukikaze,
                    amatsukaze,
                    kongou,
                    haruna,
                    nagato,
                    shimakaze,
                    yamashiro,
                    akagi,
                    yahagi,
                    agano,
                    zuikaku,
                    shoukaku,
                    souryuu,
                    mogami,
                    yamato
                }

                /// <summary>
                /// Converts the enum value from the Vaporwave palette to a usable color.
                /// </summary>
                /// <param name="enumValue">The value to convert.</param>
                /// <returns>The corresponding color.</returns>
                public static Color EnumToColor (ColorPalette enumValue) {
                    switch (enumValue) {
                        case ColorPalette.yukikaze:
                            return yukikaze;
                        case ColorPalette.amatsukaze:
                            return amatsukaze;
                        case ColorPalette.kongou:
                            return kongou;
                        case ColorPalette.haruna:
                            return haruna;
                        case ColorPalette.nagato:
                            return nagato;
                        case ColorPalette.shimakaze:
                            return shimakaze;
                        case ColorPalette.yamashiro:
                            return yamashiro;
                        case ColorPalette.akagi:
                            return akagi;
                        case ColorPalette.yahagi:
                            return yahagi;
                        case ColorPalette.agano:
                            return agano;
                        case ColorPalette.zuikaku:
                            return zuikaku;
                        case ColorPalette.shoukaku:
                            return shoukaku;
                        case ColorPalette.souryuu:
                            return souryuu;
                        case ColorPalette.mogami:
                            return mogami;
                        case ColorPalette.yamato:
                            return yamato;
                        default:
                            return yukikaze;
                    }
                }

                public static Color yukikaze {
                    get { return ColorHelper.ToColor(0xC6ACC9); }
                }

                public static Color amatsukaze {
                    get { return ColorHelper.ToColor(0xECB4BF); }
                }

                public static Color kongou {
                    get { return ColorHelper.ToColor(0xFDD6B5); }
                }

                public static Color haruna {
                    get { return ColorHelper.ToColor(0xFDF3B8); }
                }

                public static Color nagato {
                    get { return ColorHelper.ToColor(0xA7E4AE); }
                }

                public static Color shimakaze {
                    get { return ColorHelper.ToColor(0xBFFFE6); }
                }

                public static Color yamashiro {
                    get { return ColorHelper.ToColor(0x8699D1); }
                }

                public static Color akagi {
                    get { return ColorHelper.ToColor(0xDBA9CE); }
                }

                public static Color yahagi {
                    get { return ColorHelper.ToColor(0xAAE7E8); }
                }

                public static Color agano {
                    get { return ColorHelper.ToColor(0xEFE6EB); }
                }

                public static Color zuikaku {
                    get { return ColorHelper.ToColor(0x47D1D5); }
                }

                public static Color shoukaku {
                    get { return ColorHelper.ToColor(0xE8D2FF); }
                }

                public static Color souryuu {
                    get { return ColorHelper.ToColor(0xC1D1FD); }
                }

                public static Color mogami {
                    get { return ColorHelper.ToColor(0xD9E0FC); }
                }

                public static Color yamato {
                    get { return ColorHelper.ToColor(0xFFE0F1); }
                }

            }

            public static Color purple {
                get { return ColorHelper.ToColor(0x800080); }
            }

            public static Color orange {
                get { return ColorHelper.ToColor(0xFF8C00); }
            }

            public static Color chartreuseYellow {
                get { return ColorHelper.ToColor(0xDFFF00); }
            }

            public static Color chartreuseGreen {
                get { return ColorHelper.ToColor(0x7FFF00); }
            }

            public static Color kokiriTunic {
                get { return ColorHelper.ToColor(0x00CC00); }
            }

            public static Color goronTunic {
                get { return ColorHelper.ToColor(0xCC0000); }
            }

            public static Color zoraTunic {
                get { return ColorHelper.ToColor(0x0000CC); }
            }

            public static Color teal {
                get { return ColorHelper.ToColor(0x008080); }
            }

            public static Color indigo {
                get { return ColorHelper.ToColor(0x4B0082); }
            }

            public static Color sage {
                get { return ColorHelper.ToColor(0x9C9F84); }
            }

            public static Color mintIceCream {
                get { return ColorHelper.ToColor(0xBAEBAE); }
            }

            public static Color sarcoline {
                get { return ColorHelper.ToColor(0xFADFAE); }
            }

            public static Color coquelicot {
                get { return ColorHelper.ToColor(0xFF3800); }
            }

            public static Color smaragdine {
                get { return ColorHelper.ToColor(0x50C875); }
            }

            public static Color mikado {
                get { return ColorHelper.ToColor(0xFFC40C); }
            }

            public static Color glaucous {
                get { return ColorHelper.ToColor(0x6082B6); }
            }

            public static Color wenge {
                get { return ColorHelper.ToColor(0x645452); }
            }

            public static Color fulvous {
                get { return ColorHelper.ToColor(0xE48400); }
            }

            public static Color xanadu {
                get { return ColorHelper.ToColor(0x738678); }
            }

            public static Color falu {
                get { return ColorHelper.ToColor(0x7F1917); }
            }

            public static Color eburnean {
                get { return ColorHelper.ToColor(0xFEF6CC); }
            }

            public static Color amaranth {
                get { return ColorHelper.ToColor(0xE52B50); }
            }

        }

        /// <summary>
        /// Helper class to convert other color formats to Unity-recognized colors.
        /// </summary>
        public static class ColorHelper {

			/// <summary>
			/// A color that uses HSV instead of RGB. Can be converted to RGB using ToColor().
			/// </summary>
			[System.Serializable]
            public class ColorHSV {

                public int h;
                public float s;
                public float v;
                public float a;

                public ColorHSV () {
                    h = 0;
                    s = 0;
                    v = 0;
                    a = 0;
                }

                public ColorHSV (ColorHSV cHSV) {
                    h = cHSV.h;
                    s = cHSV.s;
                    v = cHSV.v;
                    a = cHSV.a;
                }

                public ColorHSV (int hue, float saturation, float value, float alpha) {
                    h = hue;
                    if (h > 360) {
                        h -= 360;
                    }
                    if (h < 0) {
                        h += 360;
                    }
                    s = saturation;
                    if (s > 1f) {
                        s = 1f;
                    }
                    if (s < 0f) {
                        s = 0f;
                    }
                    v = value;
                    if (v > 1f) {
                        v = 1f;
                    }
                    if (v < 0f) {
                        v = 0f;
                    }
                    a = alpha;
                    if (a > 1f) {
                        a = 1f;
                    }
                    if (a < 0f) {
                        a = 0f;
                    }
                }

            }

			/// <summary>
			/// Contains both a color and an HSV color. Can be used to easily convert via inspector if using JBirdEditor.
			/// </summary>
			[System.Serializable]
			public class ColorHSVRGB {

				public Color rgb;
				public ColorHSV hsv;

				public ColorHSVRGB () {
					hsv = new ColorHSV();
					rgb = new Color();
				}

			}

            /// <summary>
            /// Converts a Color to the ColorHSV class.
            /// </summary>
            /// <returns>ColorHSV class instance.</returns>
            /// <param name="color">Color to convert.</param>
            public static ColorHSV ToHSV (this Color color) {
                float hue;
                float sat;
                float val;
                float cMax = Mathf.Max(color.r, color.g, color.b);
                float cMin = Mathf.Min(color.r, color.g, color.b);
                float delta = cMax - cMin;
                //HUE
                if (delta == 0f) {
                    hue = 0;
                }
                else if (cMax == color.r) {
					hue = ((color.g - color.b)) / delta;
                }
                else if (cMax == color.g) {
					hue = 2 + (color.b - color.r) / delta;
                }
                else {
					hue = 4 + (color.r - color.g) / delta;
                }
                //Convert to degrees
                hue *= 60f;
                if (hue < 0f) {
                    hue += 360f;
                }
                //SATURATION
                if (cMax == 0f) {
                    sat = 0f;
                }
                else {
                    sat = delta / cMax;
                }
                //VALUE
                val = cMax;
				return new ColorHSV((int)hue, sat, val, color.a);
            }

            public static Color ToColor (this ColorHSV colorHSV) {
                float c = colorHSV.v * colorHSV.s;
                float x = c * (1f - Mathf.Abs((colorHSV.h / 60f) % 2 - 1));
                float m = colorHSV.v - c;
                float r = 0f;
                float g = 0f;
                float b = 0f;
                if (0f <= colorHSV.h && colorHSV.h < 60f) {
                    r = c + m;
                    g = x + m;
                    b = m;
                }
                else if (60f <= colorHSV.h && colorHSV.h < 120f) {
                    r = x + m;
                    g = c + m;
                    b = m;
                }
                else if (120f <= colorHSV.h && colorHSV.h < 180f) {
                    r = m;
                    g = c + m;
                    b = x + m;
                }
                else if (180f <= colorHSV.h && colorHSV.h < 240f) {
                    r = m;
                    g = x + m;
                    b = c + m;
                }
                else if (240f <= colorHSV.h && colorHSV.h < 300f) {
                    r = x + m;
                    g = m;
                    b = c + m;
                }
                else {
                    r = c + m;
                    g = m;
                    b = x + m;
                }
                return new Color(r, g, b, colorHSV.a);
            }

            public static Color ToColor (this int HexValue) {
                byte r = (byte)((HexValue >> 16) & 0xFF);
                byte g = (byte)((HexValue >> 8) & 0xFF);
                byte b = (byte)((HexValue) & 0xFF);
                return ToColor(new Color32(r, g, b, 255));
            }

            public static Color ToColor (this Color32 color32) {
                return (Color)color32;
            }

            /// <summary>
            /// Create a Color using red, green, and blue values from 0 to 255.
            /// </summary>
            /// <param name="red">Red (0 to 255).</param>
            /// <param name="green">Green (0 to 255).</param>
            /// <param name="blue">Blue (0 to 255).</param>
            public static Color From255to1 (int red, int green, int blue) {
                return new Color((float)red / 255f, (float)green / 255f, (float)blue / 255f);
            }

            /// <summary>
            /// A container class for holding a color and an amount (for mixing).
            /// </summary>
            public class ColorAmount {

                public Color color;
                public float amount;

                /// <summary>
                /// Initializes a new instance of the ColorAmount class.
                /// </summary>
                /// <param name="c">Color.</param>
                /// <param name="a">Amount (should be between 0.0f and 1.0f).</param>
                public ColorAmount (Color c, float a) {
                    color = c;
                    amount = a;
                }

                public ColorAmount (ColorAmount cA) {
                    color = cA.color;
                    amount = cA.amount;
                }

            }

            /// <summary>
            /// A container class for holding a color (in HSV) and an amount (for mixing).
            /// </summary>
            public class ColorHSVAmount {

                public ColorHSV colorHSV;
                public float amount;

                /// <summary>
                /// Initializes a new instance of the ColorHSVAmount class.
                /// </summary>
                /// <param name="c">Color (in HSV).</param>
                /// <param name="a">Amount (should be between 0.0f and 1.0f).</param>
                public ColorHSVAmount (ColorHSV c, float a) {
                    colorHSV = c;
                    amount = a;
                }

                public ColorHSVAmount (ColorHSVAmount cHSVA) {
                    colorHSV = cHSVA.colorHSV;
                    amount = cHSVA.amount;
                }

            }

            /// <summary>
            /// Mixes the specified colors in their respective quantities using RGB.
            /// </summary>
            /// <returns>The result of mixing the colors.</returns>
            /// <param name="colors">Colors to mix.</param>
            public static Color MixColors (params ColorAmount[] colors) {
                float redBalance = 0f;
                float greenBalance = 0f;
                float blueBalance = 0f;
                float alpha = 0f;
                foreach (ColorAmount colorAmount in colors) {
                    redBalance += colorAmount.color.r * colorAmount.amount;
                    greenBalance += colorAmount.color.g * colorAmount.amount;
                    blueBalance += colorAmount.color.b * colorAmount.amount;
                    alpha += colorAmount.color.a * colorAmount.amount;
                }
                return new Color(redBalance, greenBalance, blueBalance, alpha);
            }

            /// <summary>
            /// Mixes the specified colors in equal quantities using RGB.
            /// </summary>
            /// <returns>The result of mixing the colors.</returns>
            /// <param name="colors">Colors to mix.</param>
            public static Color MixColors (params Color[] colors) {
                ColorAmount[] colorAmounts = new ColorAmount[colors.Length];
                for (int i = 0; i < colors.Length; i++) {
                    colorAmounts[i] = new ColorAmount(colors[i], 1f / (float)colors.Length);
                }
                return MixColors(colorAmounts);
            }

            /// <summary>
            /// Mixes the specified HSV colors in their respective quantities using HSV.
            /// </summary>
            /// <returns>The result of mixing the colors.</returns>
            /// <param name="colors">Colors to mix.</param>
            public static Color MixColorsHSV (params ColorHSVAmount[] colorsHSV) {
                float hue = 0f;
                float saturation = 0f;
                float value = 0f;
                float alpha = 0f;
                foreach (ColorHSVAmount colorHSVAmount in colorsHSV) {
                    hue += colorHSVAmount.colorHSV.h * colorHSVAmount.amount;
                    saturation += colorHSVAmount.colorHSV.s * colorHSVAmount.amount;
                    value += colorHSVAmount.colorHSV.v * colorHSVAmount.amount;
                    alpha += colorHSVAmount.colorHSV.a * colorHSVAmount.amount;
                }
				return new ColorHSV((int)hue, saturation, value, alpha).ToColor();
            }

            /// <summary>
            /// Mixes the specified colors in their respective quantities using HSV.
            /// </summary>
            /// <returns>The result of mixing the colors.</returns>
            /// <param name="colors">Colors to mix.</param>
            public static Color MixColorsHSV (params ColorAmount[] colors) {
                ColorHSVAmount[] colorHSVAmounts = new ColorHSVAmount[colors.Length];
                for (int i = 0; i < colors.Length; i++) {
                    colorHSVAmounts[i] = new ColorHSVAmount(colors[i].color.ToHSV(), colors[i].amount);
                }
                return MixColorsHSV(colorHSVAmounts);
            }

            /// <summary>
            /// Mixes the specified HSV colors in their respective quantities using HSV.
            /// </summary>
            /// <returns>The result of mixing the colors.</returns>
            /// <param name="colors">Colors to mix.</param>
            public static Color MixColorsHSV (params ColorHSV[] colorsHSV) {
                ColorHSVAmount[] colorHSVAmounts = new ColorHSVAmount[colorsHSV.Length];
                for (int i = 0; i < colorsHSV.Length; i++) {
                    colorHSVAmounts[i] = new ColorHSVAmount(colorsHSV[i], 1f / (float)colorsHSV.Length);
                }
                return MixColorsHSV(colorHSVAmounts);
            }

            /// <summary>
            /// Mixes the specified colors in their respective quantities using HSV.
            /// </summary>
            /// <returns>The result of mixing the colors.</returns>
            /// <param name="colors">Colors to mix.</param>
            public static Color MixColorsHSV (params Color[] colors) {
                ColorHSVAmount[] colorHSVAmounts = new ColorHSVAmount[colors.Length];
                for (int i = 0; i < colors.Length; i++) {
                    colorHSVAmounts[i] = new ColorHSVAmount(colors[i].ToHSV(), 1f / (float)colors.Length);
                }
                return MixColorsHSV(colorHSVAmounts);
            }

            /// <summary>
            /// Shifts the hue by the specified number of degrees.
            /// </summary>
            /// <returns>The HSV color with hue shifted.</returns>
            /// <param name="startColor">Starting HSV color.</param>
            /// <param name="degrees">Degrees to shift the hue.</param>
            public static ColorHSV ShiftHue (this ColorHSV startColor, float degrees) {
				float hue = (float)startColor.h;
				hue += degrees;
                if (hue > 360f) {
                    hue -= 360f;
                }
                if (hue < 0f) {
                    hue += 360f;
                }
				startColor.h = (int)hue;
                return startColor;
            }

            /// <summary>
            /// Make a gradient between two colors (Returns a list of colors blended from startColor to endColor).
            /// </summary>
            /// <returns>A list of colors blended from startColor to endColor.</returns>
            /// <param name="startColor">Start color.</param>
            /// <param name="endColor">End color.</param>
            /// <param name="blendColors">Number of blended colors in the middle (returned list's length will be this value plus two).</param>
            public static List<Color> MakeGradient (Color startColor, Color endColor, int blendColors) {
                List<Color> gradient = new List<Color>();
                float amountPerBlendStep = 1f / (float)(blendColors + 1);
                float blendAmount = 0f;
                gradient.Add(startColor);
                for (int i = 0; i < blendColors; i++) {
                    blendAmount += amountPerBlendStep;
                    gradient.Add(MixColors(new ColorAmount(startColor, 1f - blendAmount), new ColorAmount(endColor, blendAmount)));
                }
                gradient.Add(endColor);
                return gradient;
            }

            /// <summary>
            /// Make a gradient between two colors using HSV (Returns a list of colors blended from startColor to endColor).
            /// </summary>
            /// <returns>A list of colors blended from startColor to endColor.</returns>
            /// <param name="startColor">Start color.</param>
            /// <param name="endColor">End color.</param>
            /// <param name="blendColors>Number of blended colors in the middle (returned list's length will be this value plus two).</param>
            public static List<Color> MakeGradientHSV (Color startColor, Color endColor, int blendColors) {
                List<Color> gradient = new List<Color>();
                float startHue = startColor.ToHSV().h;
                float endHue = endColor.ToHSV().h;
                float startSat = startColor.ToHSV().s;
                float endSat = endColor.ToHSV().s;
                float startVal = startColor.ToHSV().v;
                float endVal = endColor.ToHSV().v;
                float degreesPerStep = (endHue - startHue);
                if (degreesPerStep > 180f) {
                    degreesPerStep = degreesPerStep - 360f;
                }
                if (degreesPerStep < -180f) {
                    degreesPerStep = degreesPerStep + 360f;
                }
                float saturationPerStep = (endSat - startSat);
                float valuePerStep = (endVal - startVal);
                degreesPerStep /= (float)(blendColors + 1);
                saturationPerStep /= (float)(blendColors + 1);
                valuePerStep /= (float)(blendColors + 1);
                gradient.Add(startColor);
                ColorHSV colorHSV = startColor.ToHSV();
                for (int i = 0; i < blendColors; i++) {
                    colorHSV.ShiftHue(degreesPerStep);
                    colorHSV.s += saturationPerStep;
                    colorHSV.v += valuePerStep;
                    gradient.Add(colorHSV.ToColor());
                }
                gradient.Add(endColor);
                return gradient;
            }

            /// <summary>
            /// Make a rainbow from the start color, shifting the hue a specified number of degrees each step (Returns a list of Colors).
            /// </summary>
            /// <param name="startColor">Start color.</param>
            /// <param name="degreesPerStep">Degrees to shift the hue per step.</param>
            /// <param name="length">Desired length of the list.</param>
            public static List<Color> Rainbowify (Color startColor, float degreesPerStep, int length) {
                List<Color> rainbow = new List<Color>();
                rainbow.Add(startColor);
                ColorHSV colorHSV = startColor.ToHSV();
                for (int i = 0; i < length - 1; i++) {
                    colorHSV.ShiftHue(degreesPerStep);
                    rainbow.Add(colorHSV.ToColor());
                }
                return rainbow;
            }

            /// <summary>
            /// Make a rainbow from the start color, shifting the hue until it reaches the end hue. (Returns a list of Colors).
            /// </summary>
            /// <param name="startColor">Start color.</param>
            /// <param name="endColor">Hue to stop shifting at (will only use the hue of this color, not value or saturation).</param>
            /// <param name="length">Desired length of the list.</param>
            public static List<Color> Rainbowify (Color startColor, Color endColor, int length) {
                List<Color> rainbow = new List<Color>();
                float startHue = startColor.ToHSV().h;
                float endHue = endColor.ToHSV().h;
                if (endHue < startHue) {
                    endHue += 360f;
                }
                float degreesPerStep = (endHue - startHue);
                degreesPerStep /= (float)length - 1;
                rainbow.Add(startColor);
                ColorHSV colorHSV = startColor.ToHSV();
                for (int i = 0; i < length - 1; i++) {
                    colorHSV.ShiftHue(degreesPerStep);
                    rainbow.Add(colorHSV.ToColor());
                }
                return rainbow;
            }

			public class Chroma {
				
				public int hue;
				public float saturation;

				public Chroma () {
					hue = 0;
					saturation = 0;
				}

				public Chroma (int h, float s) {
					hue = h;
					saturation = s;
				}

			}

			public static float GetLuma (this Color color, bool useBT709 = false) {
				float rVal = 0.299f;
				float gVal = 0.587f;
				float bVal = 0.114f;
				if (useBT709) { //using BT.709 instead of BT.601
					rVal = 0.2126f;
					gVal = 0.7152f;
					bVal = 0.0722f;
				}
				return color.r * rVal + color.g * gVal + color.b * bVal;
			}

			public static Color FromChromaAndLuma (Chroma chroma, float luma, bool useBT709 = false) {
				return FromChromaAndLuma(chroma.hue, chroma.saturation, luma, useBT709);
			}

			/// <summary>
			/// Returns a color with the specified hue and luma. Defaults to BT.601.
			/// </summary>
			/// <returns>Color with the specified hue and luma.</returns>
			/// <param name="hue">Hue.</param>
			/// <param name="luma">Luma.</param>
			/// <param name="useBT709">Uses BT.709 instead of BT.601 if set to true.</param>
			public static Color FromChromaAndLuma (int hue, float saturation, float luma, bool useBT709 = false) {
				float rVal = 0.299f;
				float gVal = 0.587f;
				float bVal = 0.114f;
				if (useBT709) { //using BT.709 instead of BT.601
					rVal = 0.2126f;
					gVal = 0.7152f;
					bVal = 0.0722f;
				}
				float red = 0f;
				float green = 0f;
				float blue = 0f;
				if (hue >= 360) {
					hue = 0;
				}
				else if (hue < 0) {
					hue = 0;
				}
				luma = Mathf.Clamp(luma, 0f, 1f);
				switch (hue) {
				case 0: //red
					if (luma > rVal) {
						red = 1f;
						green = blue = (luma - rVal) / (gVal + bVal);
					}
					else {
						red = (luma / rVal);
						green = blue = 0f;
					}
					break;
				case 60: //yellow
					if (luma > rVal + gVal) {
						red = green = 1f;
						blue = (luma - rVal - gVal) / bVal;
					}
					else {
						red = green = (luma / (rVal + gVal));
						blue = 0f;
					}
					break;
				case 120: //green
					if (luma > gVal) {
						green = 1f;
						red = blue = (luma - gVal) / (rVal + bVal);
					}
					else {
						green = (luma / gVal);
						red = blue = 0f;
					}
					break;
				case 180: //cyan
					if (luma > gVal + bVal) {
						green = blue = 1f;
						red = (luma - gVal - bVal) / rVal;
					}
					else {
						green = blue = (luma / (gVal + bVal));
						red = 0f;
					}
					break;
				case 240: //blue
					if (luma > bVal) {
						blue = 1f;
						red = green = (luma - bVal) / (rVal + gVal);
					}
					else {
						blue = (luma / bVal);
						red = green = 0f;
					}
					break;
				case 300: //magenta
					if (luma > rVal + bVal) {
						red = blue = 1f;
						green = (luma - rVal - bVal) / gVal;
					}
					else {
						red = blue = (luma / (rVal + bVal));
						green = 0f;
					}
					break;
				default:
					float fHue = (float)hue;
					if (hue > 0 && hue < 60) { //between red and yellow
						float huePercentage = (60f / (fHue - 0));
						float xVal = (rVal * huePercentage + gVal);
						float x = luma / xVal;
						red = x * huePercentage;
						green = x;
						blue = 0f;
						if (red > 1f) {
							red = 1f;
							green = 1f / huePercentage;
							float yVal = (gVal * (1f - green) + bVal);
							float y = (luma - rVal - gVal * green) / yVal;
							green += y * (1f - (1f / huePercentage));
							blue = y;
						}
					}
					else if (hue > 60 && hue < 120) { //between yellow and green
						float huePercentage = (60f / (120 - fHue));
						float xVal = (gVal * huePercentage + rVal);
						float x = luma / xVal;
						green = x * huePercentage;
						red = x;
						blue = 0f;
						if (green > 1f) {
							green = 1f;
							red = 1f / huePercentage;
							float yVal = (rVal * (1f - red) + bVal);
							float y = (luma - gVal - rVal * red) / yVal;
							red += y * (1f - (1f / huePercentage));
							blue = y;
						}
					}
					else if (hue > 120 && hue < 180) { //between green and cyan
						float huePercentage = (60f / (fHue - 120));
						float xVal = (gVal * huePercentage + bVal);
						float x = luma / xVal;
						green = x * huePercentage;
						blue = x;
						red = 0f;
						if (green > 1f) {
							green = 1f;
							blue = 1f / huePercentage;
							float yVal = (bVal * (1f - blue) + rVal);
							float y = (luma - gVal - bVal * blue) / yVal;
							blue += y * (1f - (1f / huePercentage));
							red = y;
						}
					}
					else if (hue > 180 && hue < 240) { //between cyan and blue
						float huePercentage = (60f / (240 - fHue));
						float xVal = (bVal * huePercentage + gVal);
						float x = luma / xVal;
						blue = x * huePercentage;
						green = x;
						red = 0f;
						if (blue > 1f) {
							blue = 1f;
							green = 1f / huePercentage;
							float yVal = (gVal * (1f - green) + rVal);
							float y = (luma - bVal - gVal * green) / yVal;
							green += y * (1f - (1f / huePercentage));
							red = y;
						}
					}
					else if (hue > 240 && hue < 300) { //between blue and magenta
						float huePercentage = (60f / (fHue - 240));
						float xVal = (bVal * huePercentage + rVal);
						float x = luma / xVal;
						blue = x * huePercentage;
						red = x;
						green = 0f;
						if (blue > 1f) {
							blue = 1f;
							red = 1f / huePercentage;
							float yVal = (rVal * (1f - red) + gVal);
							float y = (luma - bVal - rVal * red) / yVal;
							red += y * (1f - (1f / huePercentage));
							green = y;
						}
					}
					else if (hue > 300 && hue < 360) { //between magenta and red
						float huePercentage = (60f / (360 - fHue));
						float xVal = (rVal * huePercentage + bVal);
						float x = luma / xVal;
						red = x * huePercentage;
						blue = x;
						green = 0f;
						if (red > 1f) {
							red = 1f;
							blue = 1f / huePercentage;
							float yVal = (bVal * (1f - blue) + gVal);
							float y = (luma - rVal - bVal * blue) / yVal;
							blue += y * (1f - (1f / huePercentage));
							green = y;
						}
					}
					break;
				}
				return Color.Lerp(new Color(luma, luma, luma), new Color (red, green, blue), saturation);
			}

        }
    }
}
