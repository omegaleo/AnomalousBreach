using System;
using System.Collections;
using System.Collections.Generic;
using Omega_Leo_Toolbox.Editor.Drawers;
using UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeButton))]
public class UpgradeButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // No need to do changes here, this custom editor was created just so the properties would show up properly
        
        base.OnInspectorGUI();
    }
}
