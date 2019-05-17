/*
 * This file was created by Mark Botaish on May 17th, 2019
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ARMon;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor {

    public static float max = 100.0f;  
    /*
     * This function is for creating the sliders in the GameManagerScript. 
     * It ensures that no matter what the sum of all the drop rates is 100%
     */
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GameManager myManager = (GameManager)target;
       // Debug.Log(myManager.commonDropRate + " " + myManager.uncommonDropRate + " " + myManager.rareDropRate);
        //Creates a slide for the common drop rate and sets it to the commonDropRate in the GameManager script
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("CommonDropRate");
        SerializedProperty common = serializedObject.FindProperty("commonDropRate");
        common.floatValue = EditorGUILayout.Slider(common.floatValue, 0, max);
        EditorGUILayout.EndHorizontal();
 

        //Creates a slide for the uncommon drop rate and sets it to the uncommonDropRate in the GameManager script
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("UnCommonDropRate");
        SerializedProperty uncommon = serializedObject.FindProperty("uncommonDropRate");
        uncommon.floatValue = EditorGUILayout.Slider(uncommon.floatValue, 0, max - myManager.commonDropRate);
        EditorGUILayout.EndHorizontal();

        //Creates text for the rare drop rate and sets it to the rateDropRate in the GameManager script
        EditorGUILayout.BeginHorizontal();
        GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight };
        SerializedProperty rare = serializedObject.FindProperty("rareDropRate");
        rare.floatValue = (max - common.floatValue - uncommon.floatValue);
        EditorGUILayout.LabelField("RareDropRate",(max - myManager.commonDropRate - myManager.uncommonDropRate).ToString(), style);
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
