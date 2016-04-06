using UnityEngine;
using System.Collections;

public class OneHueEffect : MonoBehaviour {

    public int hue;
    public int tolerance;
    private Material material;

    void Awake () {
        material = new Material(Shader.Find("Hidden/OneHueEffect"));
    }

    void OnRenderImage (RenderTexture source, RenderTexture destination) {
        material.SetInt("_Hue", hue);
        material.SetInt("_Tolerance", tolerance);
        Graphics.Blit(source, destination, material);
    }

}
