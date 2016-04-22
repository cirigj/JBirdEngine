using UnityEngine;
using System.Collections;
using JBirdEngine.ColorLibrary;
using System.Collections.Generic;

public class LazerPixel : MonoBehaviour {

	public enum Mode {
		Color51,
		YoAesthetic,
		NESEmulation,
        CustomPalette,
        GBAGrayscaleRamp,
	}

    public enum ClampMode {
        NoClamp,
        ClosestRGB,
		HSVCylinder,
		HSVCone,
		ClosestLuma,
    }

	public Mode mode;
    [Range(1,16)]
    public int pixelScale = 1;
    [Range(-.5f, .5f)]
    public float checkering = .05f;
	[Range(-.5f, .5f)]
	public float saturationCorrection = 0f;
    private Material material;
    [Header("Custom Palette Specs:")]
    public JBirdEngine.ColorLibrary.JBirdColorPalette JBirdPalette;
    public Texture3D paletteTexture;
    public int texSize;
    public ClampMode clampMode;
    [Header("GBA Color Ramp Specs:")]
    public Color color1 = new Color(0f, 0f, 0f, 1f);
    public Color color2 = new Color(.333f, .333f, .333f, 1f);
    public Color color3 = new Color(.667f, .667f, .667f, 1f);
    public Color color4 = new Color(1f, 1f, 1f, 1f);

    void Awake () {
        material = new Material(Shader.Find("Hidden/LazerPixel"));
    }

    void OnRenderImage (RenderTexture source, RenderTexture destination) {
        material.SetFloat("_Width", (float)source.width);
        material.SetFloat("_Height", (float)source.height);
        material.SetInt("_PixelScale", pixelScale);
        material.SetFloat("_Checkering", checkering);
		material.SetInt("_Mode", (int)mode);
		material.SetFloat("_SatCorrect", saturationCorrection);
        material.SetTexture("_CustomPalette", paletteTexture);
        material.SetInt("_TexSize", texSize);
        paletteTexture.wrapMode = TextureWrapMode.Clamp;
        material.SetFloat("_Scale", (paletteTexture.width - 1) / (1.0f * paletteTexture.width));
        material.SetFloat("_Offset", 1.0f / (2.0f * paletteTexture.width));
        material.SetColor("_GBAColor1", color1);
        material.SetColor("_GBAColor2", color2);
        material.SetColor("_GBAColor3", color3);
        material.SetColor("_GBAColor4", color4);
        Graphics.Blit(source, destination, material);
    }

    void ClampToAcceptableSize () {
        int[] acceptableSizes = {4, 8, 16, 32, 64, 128, 256};
        if (texSize < 4) {
            texSize = 4;
            return;
        }
        if (texSize > 256) {
            texSize = 256;
            return;
        }
        int closestDist = 256;
        int newSize = 0;
        for (int i = 0; i < acceptableSizes.Length; i++) {
            int dist = Mathf.Abs(texSize - acceptableSizes[i]);
            if (dist < closestDist) {
                closestDist = dist;
                newSize = acceptableSizes[i];
            }
        }
        texSize = newSize;
    }

	float ColorDistanceRGB (Color c1, Color c2) {
		return (Mathf.Sqrt((c1.r - c2.r) * (c1.r - c2.r) + (c1.g - c2.g) * (c1.g - c2.g) + (c1.b - c2.b) * (c1.b - c2.b)));
	}

	float ColorDistanceHSVCylinder (Color c1, Color c2) {
		ColorHelper.ColorHSV c1hsv = new ColorHelper.ColorHSV(c1);
		ColorHelper.ColorHSV c2hsv = new ColorHelper.ColorHSV(c2);
		return (Vector3.Distance(Quaternion.AngleAxis(c1hsv.h, Vector3.up) * (Vector3.right * c1hsv.v + Vector3.up * c1hsv.s), Quaternion.AngleAxis(c2hsv.h, Vector3.up) * (Vector3.right * c2hsv.v + Vector3.up * c2hsv.s)));
	}

	float ColorDistanceHSVCone (Color c1, Color c2) {
		return 0;
	}

	float ColorDistanceLuma (Color c1, Color c2) {
		return Mathf.Abs(c1.GetLuma() - c2.GetLuma());
	}

	Color ClampToNearestColor (Color c, ClampMode clampMode) {
		if (JBirdPalette == null) {
			return c;
		}
		float minDist = 1000f;
		Color closest = Color.white;
		switch (clampMode) {
		case ClampMode.NoClamp:
			return c;
		case ClampMode.ClosestRGB:
			foreach (Color c2 in JBirdPalette.colorList) {
				float dist = ColorDistanceRGB(c, c2);
				if (dist < minDist) {
					minDist = dist;
					closest = c2;
				}
			}
			break;
		case ClampMode.HSVCylinder:
			foreach (Color c2 in JBirdPalette.colorList) {
				float dist = ColorDistanceHSVCylinder(c, c2);
				if (dist < minDist) {
					minDist = dist;
					closest = c2;
				}
			}
			break;
		case ClampMode.HSVCone:
			foreach (Color c2 in JBirdPalette.colorList) {
				float dist = ColorDistanceHSVCone(c, c2);
				if (dist < minDist) {
					minDist = dist;
					closest = c2;
				}
			}
			break;
		case ClampMode.ClosestLuma:
			foreach (Color c2 in JBirdPalette.colorList) {
				float dist = ColorDistanceLuma(c, c2);
				if (dist < minDist) {
					minDist = dist;
					closest = c2;
				}
			}
			break;
		}
		return closest;
	}

    public void GenerateTex3D () {
        ClampToAcceptableSize();
        paletteTexture = new Texture3D(texSize, texSize, texSize, TextureFormat.ARGB32, true);
        Color[] colors = new Color[texSize * texSize * texSize];
        float colorStep = 1.0f / (texSize);
        int texIndex = 0;
        Color c = Color.white;
        for (int b = 0; b < texSize; b++) {
            for (int g = 0; g < texSize; g++) {
                for (int r = 0; r < texSize; r++, texIndex++) {
                    c.r = r * colorStep;
                    c.g = g * colorStep;
                    c.b = b * colorStep;
					c = ClampToNearestColor(c, clampMode);
                    colors[texIndex] = c;
                    //texColors.Add(c);
                }
            }
        }
        paletteTexture.SetPixels(colors);
        paletteTexture.Apply();
    }

}