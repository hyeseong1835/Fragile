using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class WeaponMaker : EditorWindow
{

    [MenuItem("Window/WeaponMaker")]

    public static void ShowWindow()
    {
        GetWindow(typeof(WeaponMaker));
    }

    void OnGUI()
    {
        // The actual window code goes here
    }
}