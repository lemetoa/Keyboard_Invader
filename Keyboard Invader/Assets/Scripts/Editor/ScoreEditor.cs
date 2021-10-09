using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Score))]
public class ScoreEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("최고기록 초기화", GUILayout.MinWidth(100)))
        {
            Score.ResetHighScore();
        } 

    }
}