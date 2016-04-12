using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor(typeof(LazerPixel))]
public class LazerPixelEditor : Editor {

    LazerPixel targetShader;

    public override void OnInspectorGUI () {
        base.OnInspectorGUI();

        targetShader = (LazerPixel)target;

        if (GUILayout.Button("Generate Tex3D from Palette")) {
            targetShader.GenerateTex3D();
        }
    }

}
