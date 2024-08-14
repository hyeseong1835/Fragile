using DG.DemiEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomObjectRule))]
public class RoomObjectRuleInspector : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RoomObjectRule rule = (RoomObjectRule)target;


    }
}
