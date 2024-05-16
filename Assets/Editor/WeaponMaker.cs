using Meryel.UnityCodeAssist.Synchronizer.Model;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using static Codice.Client.Common.Connection.AskCredentialsToUser;

public class WeaponMaker : EditorWindow
{
    const string skillScriptsFolderPath = "Assets/Script/Weapon/Skill/";

    Texture tex;
    Weapon weapon;
    int weaponTabIndex = 0;
    int skillTabIndex = 0;

    void Awake()
    {
        tex = EditorGUIUtility.Load("WeaponIcon16.png") as Texture;

        EditorGUIUtility.labelWidth = Editor.propertyLabelWidth;
    }
    void Update()
    {
        //if (//Selection.count == 1 
          //   Editor.GetObjectState(Selection.objects[0].GetComponent<GameObject>()) == Editor.ObjectState.PrefabEdit)
            weapon = Selection.objects[0].GetComponent<Weapon>();
    }
    [MenuItem("Window/WeaponMaker")]
    public static void ShowWindow()
    {
        GetWindow(typeof(WeaponMaker));
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        #region Horizontal Default

            weapon.UISprite = (Sprite)EditorGUILayout.ObjectField(weapon.UISprite, typeof(Sprite), false, GUILayout.Width(Editor.propertyHeight * 3.5f), GUILayout.Height(Editor.propertyHeight * 3.5f));

            EditorGUILayout.BeginVertical();
            #region Vertical Stat
                
                EditorStyles.textField.fontSize = Editor.titleFontSize;
                EditorGUILayout.TextField(weapon.weaponName, GUILayout.Height(Editor.titleHeight));
                EditorStyles.textField.fontSize = Editor.propertyFontSize;

                weapon.damage = EditorGUILayout.FloatField("Damage", weapon.damage);
                weapon.durability = EditorGUILayout.IntField("Durability", weapon.maxDurability);

            EditorGUILayout.EndVertical();
            #endregion

        EditorGUILayout.EndHorizontal();
        #endregion

        weaponTabIndex = GUILayout.Toolbar(weaponTabIndex, new string[] { "Passive", "Attack", "Special" });
        switch (weaponTabIndex)
        {
            case 0:

                break;
            case 1:
                ShowActiveSkill(ref weapon.attack);
                break;
            case 2:
                ShowActiveSkill(ref weapon.special);
                break;
        }
        Event current = Event.current;

        if (new Rect(0, 0, Screen.width, Screen.height).Contains(current.mousePosition) && current.type == EventType.ContextClick)
        {
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
    }
    void ShowActiveSkill(ref ActiveSkill activeSkill)
    {
        EditorGUILayout.BeginHorizontal();
        #region Horizontal Delay

        EditorGUILayout.LabelField("Delay", GUILayout.Width(Editor.propertyLabelWidth));
        activeSkill.maxDistance = EditorGUILayout.FloatField(activeSkill.maxDistance);
        activeSkill.delay = EditorGUILayout.FloatField(activeSkill.delay);
        activeSkill.backDelay = EditorGUILayout.FloatField(activeSkill.backDelay);

        EditorGUILayout.EndHorizontal();
        #endregion

        Rect range = EditorGUILayout.BeginHorizontal();
        #region Range

        EditorGUILayout.LabelField("Range", GUILayout.Width(Editor.propertyLabelWidth));

        float attackMaxDistance = activeSkill.maxDistance;
        EditorGUILayout.MinMaxSlider(ref activeSkill.minDistance, ref activeSkill.maxDistance, 0, activeSkill.maxDistance);
        activeSkill.maxDistance = attackMaxDistance;

        activeSkill.minDistance = EditorGUI.DelayedFloatField(
                new Rect(Editor.propertyLabelWidth - Editor.shortNoLabelPropertyWidth
                    + (range.size.x - Editor.propertyLabelWidth - Editor.shortNoLabelPropertyWidth - 20) * (activeSkill.minDistance / activeSkill.maxDistance)
                    , range.position.y, Editor.shortNoLabelPropertyWidth, Editor.propertyHeight
                ), activeSkill.minDistance);

        activeSkill.maxDistance = EditorGUILayout.FloatField(activeSkill.maxDistance, GUILayout.Width(Editor.shortNoLabelPropertyWidth));

        EditorGUILayout.EndHorizontal();
        #endregion

        Rect skillsTitle = EditorGUILayout.BeginHorizontal();
        #region Horizontal SkillsTitle

        if (activeSkill.skills != null)
        {
            string[] attackSkillTitles = new string[activeSkill.skills.Length];
            for (int titleIndex = 0; titleIndex < attackSkillTitles.Length; titleIndex++)
            {
                attackSkillTitles[titleIndex] = activeSkill.skills[titleIndex].moduleName;
            }

            skillTabIndex = GUILayout.Toolbar(weaponTabIndex, attackSkillTitles);
        }

        if (GUILayout.Button(tex))
        {
            // Get all files in the specified directory
            string[] scriptFiles = Directory.GetFiles(skillScriptsFolderPath, "*.cs", SearchOption.TopDirectoryOnly);

            string[] skillNameArray = new string[scriptFiles.Length];

            for (int i = 0; i < scriptFiles.Length; i++)
            {
                skillNameArray[i] = scriptFiles[i];//.Substring(skillScriptsFolderPath.Length, scriptFiles[i].Length - 1);
            }
            int selectionIndex = EditorGUI.Popup(new Rect(skillsTitle.position.x, skillsTitle.position.y, 100, 500), 0, skillNameArray);

            Debug.Log(skillNameArray[selectionIndex]);

            /*
                // Load the script asset
                MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(filePath);

                if (script != null)
                {
                    // Get the System.Type from the script
                    Type type = ;

                    if (type != null)
                    {
                        // Check if the type is a MonoBehaviour
                        if (typeof(Skill).IsAssignableFrom(type))
                        {
                            // Add component if it's a MonoBehaviour
                            weapon.AddComponent(System.Type.GetType(script.GetClass()));
                            Debug.Log($"Added component of type {type.Name} to GameObject.");
                        }
                        else
                        {
                            Debug.LogWarning($"Type {type.Name} in script {Path.GetFileName(filePath)} is not a MonoBehaviour.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to load type from script {Path.GetFileName(filePath)}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Failed to load script at path {filePath}");
                }
            */
            Array.Resize(ref activeSkill.skills, activeSkill.skills.Length + 1);
        }

        EditorGUILayout.EndHorizontal();
        #endregion

        if (skillTabIndex < activeSkill.skills.Length)
        {
            activeSkill.skills[skillTabIndex].OnWeaponMakerGUI();
        }
    }
}