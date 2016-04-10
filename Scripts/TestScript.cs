using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JBirdEngine;
using JBirdEngine.RenUnity;

public class TestScript : MonoBehaviour {

	public JBirdEngine.ColorLibrary.ColorHelper.ColorHSVRGB hsvrgb;
	public JBirdEngine.ColorLibrary.ColorHelper.ColorHSV hsv;
	public Markov.NameGenerator nameGenerator;

	[Range(0f, 1f)]
	public float luma;
	[Range(0, 360)]
	public int hue;
	[Range(0f, 1f)]
	public float saturation;

	public Color lumaChromaColor;

	public JBirdEngine.ColorLibrary.MoreColors.BobRoss.ColorPalette bobRoss;
    public JBirdEngine.ColorLibrary.MoreColors.Vaporwave.ColorPalette vaporwave;

    enum TestEnum {
        ohgeezrick,
        thisisatest,
    }

	void Start () {
		JBirdEngine.AI.AIHelper.GetHeuristic(Vector3.zero, Vector3.one, JBirdEngine.AI.AIHelper.HeuristicMode.hexagonal);
        "thisisatest".ToEnum<TestEnum>();
		DialogueParser.CommandInfo info = DialogueParser.ParseLine("/option if stat JohnCena Suspicion != 7 \"this is a message\" if flag butts 7 3");
		Debug.Log(info);
	}

	void Update () {
		lumaChromaColor = JBirdEngine.ColorLibrary.ColorHelper.FromChromaAndLuma(hue, saturation, luma);
		Debug.Log(JBirdEngine.ColorLibrary.ColorHelper.GetLuma(JBirdEngine.ColorLibrary.MoreColors.Vaporwave.EnumToColor(vaporwave)));
	}

}
