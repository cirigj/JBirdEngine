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
	}

    public enum ClampMode {
        NoClamp,
        ClosestRGB,
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
                    if (clampMode == ClampMode.ClosestRGB) {
                        ColorHelper.ColorHSV hsv = c.ToHSV();
                        hsv.h = Mathf.RoundToInt((float)hsv.h / 30) * 30;
                        hsv.s = Mathf.Round(hsv.s * 2f) / 2f;
                        hsv.v = Mathf.Round(hsv.v * 2f) / 2f;
                        c = hsv.ToColor();
                    }
                    colors[texIndex] = c;
                    //texColors.Add(c);
                }
            }
        }
        paletteTexture.SetPixels(colors);
        paletteTexture.Apply();
    }

}