using UnityEngine;
using System.Collections;

public class OneHueEffect : MonoBehaviour {

    public int hue;
    private Material material;

    void Awake () {
        material = new Material(Shader.Find("Hidden/OneHueEffect"));
    }

    void OnRenderImage (RenderTexture source, RenderTexture destination) {
        material.SetInt("_Hue", hue);
        Graphics.Blit(source, destination, material);
    }

}
