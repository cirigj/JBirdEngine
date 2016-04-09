using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JBirdEngine.ColorLibrary;
using System.Linq;

namespace JBirdEngine {

    namespace ColorLibrary {

        public class Chameleon : MonoBehaviour {

            public RenderTexture source;
            public Color c1;
            public Color c2;
            public Color c3;
            public float saturation = 0.75f;
            public float value = 0.75f;

            public float surveyTimeStep = 1f;
            public int iterationsPerStep = 8196;

            public int tolerance = 10;

            public List<Color> colors;

            private Material mat;

            class hueFreq {
                public int hue;
                public int freq;

                public hueFreq (int h) {
                    hue = h;
                    freq = 1;
                }

                public void Increment () {
                    freq++;
                }
            }

            void Awake () {
                StartCoroutine(ScheduleSurveys());
                mat = GetComponent<Renderer>().material;
            }

            void Update () {
                mat.SetColor("_Color1", c1);
                mat.SetColor("_Color2", c2);
                mat.SetColor("_Color3", c3);
            }

            IEnumerator SurveySurroundings () {
                RenderTexture.active = source;
                Texture2D sourceTex = new Texture2D(source.width, source.height);
                sourceTex.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
                sourceTex.Apply();
                yield return null;

                Color[] allColors = sourceTex.GetPixels();
                yield return null;

                int counter = 0;
                List<hueFreq> topHues = new List<hueFreq>();
                foreach (Color c in allColors) {
                    counter++;
                    int hue = (c.GetHue() / tolerance) * tolerance;
                    bool hueFound = false;
                    foreach (hueFreq entry in topHues) {
                        if (entry.hue == hue) {
                            entry.Increment();
                            hueFound = true;
                            break;
                        }
                    }
                    if (!hueFound) {
                        topHues.Add(new hueFreq(hue));
                    }
                    if (counter >= iterationsPerStep) {
                        yield return null;
                        counter = 0;
                    }
                }
                yield return null;

                topHues.OrderBy(h => h.freq);
                colors = new List<Color>();
                foreach (hueFreq h in topHues) {
                    colors.Add(new ColorHelper.ColorHSV(h.hue, 1, 1, 1f).ToColor());
                }
                if (topHues.Count > 0) {
                    c1 = new ColorHelper.ColorHSV(topHues[0].hue, saturation, value, 1f).ToColor();
                    if (topHues.Count > 1) {
                        c2 = new ColorHelper.ColorHSV(topHues[1].hue, saturation, value, 1f).ToColor();
                        if (topHues.Count > 2) {
                            c3 = new ColorHelper.ColorHSV(topHues[2].hue, saturation, value, 1f).ToColor();
                        }
                        else {
                            c3 = new ColorHelper.ColorHSV(topHues[1].hue, saturation, value, 1f).ToColor();
                        }
                    }
                    else {
                        c2 = new ColorHelper.ColorHSV(topHues[0].hue, saturation, value, 1f).ToColor();
                        c3 = new ColorHelper.ColorHSV(topHues[0].hue, saturation, value, 1f).ToColor();
                    }
                }
                yield break;
            }

            IEnumerator ScheduleSurveys () {
                while (true) {
                    StartCoroutine(SurveySurroundings());
                    yield return new WaitForSeconds(surveyTimeStep);
                }
            }

        }

    }

}
