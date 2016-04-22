using UnityEngine;
using System.Collections;

public class CRTEffect : MonoBehaviour {

	Material material;

	public Texture2D fisheyeMap;
	public float fisheyeStrength = 0.05f;

	[Range(0f,1f)]
	public float CRTBrightness = 0.2f;
	public int CRTSize = 180;

	public float CRTLines = 64;
	public float lineDarkness = 2.5f;

	public float scanLineTime = 1.5f;
	[Range(0f,1f)]
	public float scanLineAlpha = 0.1f;
	[Range(0f,1f)]
	public float scanLineSize = 0.05f;

	[Range(-1f,1f)]
	public float staticFuzz = 0.2f;

	void Awake () {
		material = new Material(Shader.Find("Hidden/CRTEffect"));
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat ("_CRTBrightness", CRTBrightness);
		material.SetInt ("_CRTSize", CRTSize);
		material.SetTexture ("_Fisheye", fisheyeMap);
		material.SetFloat ("_FisheyeStrength", fisheyeStrength);
		material.SetFloat ("_Lines", CRTLines);
		material.SetFloat ("_LineDarkness", lineDarkness);
		material.SetFloat ("_ScanLineTime", scanLineTime);
		material.SetFloat ("_ScanLineSize", scanLineSize);
		material.SetFloat ("_ScanLineAlpha", scanLineAlpha);
		material.SetFloat ("_Static", staticFuzz);
		Graphics.Blit(source, destination, material);
	}

}
