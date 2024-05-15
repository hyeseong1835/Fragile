using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class WeaponMaker : EditorWindow
{
    Texture tex;
    Weapon weapon;

    void Awake()
    {
        tex = EditorGUIUtility.Load("WeaponIcon16.png") as Texture;
    }
    void Update()
    {
        if (Selection.count == 1) weapon = Selection.objects[0].GetComponent<Weapon>();
    }
    [MenuItem("Window/WeaponMaker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(WeaponMaker));
    }

    void OnGUI()
    {
        GUI.skin.label.fixedWidth = Editor.propertyLabelWidth;

        GUI.skin.textField.fontSize = 20;
        GUI.TextField(new Rect(0, 0, Screen.width, 30), weapon.name);

        EditorGUILayout.Space(30);
        GUI.skin.textField.fontSize = 10;
        weapon.damage = EditorGUILayout.FloatField("Damage", weapon.damage);
        weapon.durability = EditorGUILayout.IntField("Durability", weapon.maxDurability);


        //GUI.DrawTexture(new Rect(0, 0, 50, 50), tex);
        //GUI.DrawTexture(new Rect(0, 0, 50, 50), tex);

        Event current = Event.current;

        if (new Rect(0, 0, Screen.width, Screen.height).Contains(current.mousePosition) && current.type == EventType.ContextClick)
        {
            Debug.Log(current.mousePosition);
            //Do a thing, in this case a drop down menu

            GenericMenu menu = new GenericMenu();

            menu.AddDisabledItem(new GUIContent("I clicked on a thing"));
            menu.AddItem(new GUIContent("Do a thing"), false, YourCallback);
            menu.ShowAsContext();

            current.Use();
        }
        void YourCallback()
        {
            Debug.Log("Hi there");
        }
        //EditorGUILayout.EndHorizontal();

        //EditorGUILayout.EndVertical();
    }
}