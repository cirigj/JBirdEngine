using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JBirdLib;
using JBirdLib.ColorLibrary;

public class TestScript : MonoBehaviour {

	public JBirdLib.ColorLibrary.ColorHelper.ColorHSVRGB hsvrgb;
	public JBirdLib.ColorLibrary.ColorHelper.ColorHSV hsv;
	public Markov.NameGenerator nameGenerator;

	[Range(0f, 1f)]
	public float luma;
	[Range(0, 360)]
	public int hue;
	[Range(0f, 1f)]
	public float saturation;

	public Color lumaChromaColor;

	public JBirdLib.ColorLibrary.MoreColors.BobRoss.ColorPalette bobRoss;
    public JBirdLib.ColorLibrary.MoreColors.Vaporwave.ColorPalette vaporwave;

    enum TestEnum {
        ohgeezrick,
        thisisatest,
    }

	void Start () {
		JBirdLib.AI.AIHelper.GetHeuristic(Vector3.zero, Vector3.one, JBirdLib.AI.AIHelper.HeuristicMode.hexagonal);
        "thisisatest".ToEnum<TestEnum>();
	}

	void Update () {
		lumaChromaColor = JBirdLib.ColorLibrary.ColorHelper.FromChromaAndLuma(hue, saturation, luma);
		//Debug.Log(JBirdEngine.ColorLibrary.ColorHelper.GetLuma(JBirdEngine.ColorLibrary.MoreColors.Vaporwave.EnumToColor(vaporwave)));
		//lumaChromaColor = (Color)(new ColorHelper.ColorHSV(Color.red));
    }

}
