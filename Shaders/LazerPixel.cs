using UnityEngine;
using System.Collections;

public class LazerPixel : MonoBehaviour {

    [Range(1,16)]
    public int pixelScale = 1;
    [Range(-.5f, .5f)]
    public float checkering = .05f;
    private Material material;

    void Awake () {
        material = new Material(Shader.Find("Hidden/LazerPixel"));
    }

    void OnRenderImage (RenderTexture source, RenderTexture destination) {
        material.SetFloat("_Width", (float)source.width);
        material.SetFloat("_Height", (float)source.height);
        material.SetInt("_PixelScale", pixelScale);
        material.SetFloat("_Checkering", checkering);
        Graphics.Blit(source, destination, material);
    }

}