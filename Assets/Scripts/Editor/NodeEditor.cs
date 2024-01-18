using System;
using System.Collections;
using System.Collections.Generic;
using Omega_Leo_Toolbox.Editor.Drawers;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        /*EditorGUILayout.PropertyField(serializedObject.FindProperty("_deffendedTexture"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_attackedTexture"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_attackedPercentageText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("_nodesInProximity"));

        serializedObject.ApplyModifiedProperties();*/
        
        // No need to do changes here, this custom editor was created just so the properties would show up properly
        
        base.OnInspectorGUI();
    }
}
