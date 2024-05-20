using Meryel.UnityCodeAssist.Synchronizer.Model;
using NUnit.Framework.Internal;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UI;
using static Codice.Client.Common.Connection.AskCredentialsToUser;
using static UnityEditor.PlayerSettings;
using WeaponSystem;
using System.Linq;
using System.Threading;
public class WeaponEditor : EditorWindow
{
    /*
    const string skillScriptsFolderPath = "Assets/Script/Weapon/Skill/";

    int weaponTabIndex = 0;
    int activeSkillTabIndex = 0;
    int selectionIndex = 0;


   
    void OnSelectionChange()
    {
        if (Selection.count == 0) return;

        //선택한 개체들만
        for(int selectIndex = 0; selectIndex < Selection.count; selectIndex++)
        {
            if (Selection.gameObjects[selectIndex].GetComponent<Weapon>() != null)
            {
                weapon = Selection.gameObjects[selectIndex].GetComponent<Weapon>();
                return;
            }
        }
        //부모도 포함해서
        for (int selectIndex = 0; selectIndex < Selection.count; selectIndex++)
        {
            Transform parent = Selection.gameObjects[selectIndex].transform;
            for(; ; )
            {
                parent = parent.transform.parent;

                if (parent == null) break;

                if (parent.GetComponent<Weapon>() != null)
                {
                    weapon = Selection.gameObjects[selectIndex].GetComponent<Weapon>();
                    return;
                }
            }
        }

    }
    void OnGUI()
    {
        

        EditorGUILayout.BeginHorizontal();
        #region Horizontal Default

            weapon.UISprite = (Sprite)EditorGUILayout.ObjectField(weapon.UISprite, typeof(Sprite), false, GUILayout.Width(Editor.propertyHeight * 3.5f), GUILayout.Height(Editor.propertyHeight * 3.5f));

            EditorGUILayout.BeginVertical();
            #region Vertical Stat
                
                EditorStyles.textField.fontSize = Editor.titleFontSize;
                EditorGUILayout.TextField(weapon.moduleName, GUILayout.Height(Editor.titleHeight));
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

        

    void ShowActiveSkill(ref ActiveSkill activeSkill)
        {
            activeSkill.skills[activeSkillTabIndex].moduleName = activeSkill.skills[activeSkillTabIndex].GetType().Name;

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
                string[] activeSkillTitles = new string[activeSkill.skills.Length];
                for (int titleIndex = 0; titleIndex < activeSkillTitles.Length; titleIndex++)
                {
                    activeSkillTitles[titleIndex] = activeSkill.skills[titleIndex].moduleName;
                }

                activeSkillTabIndex = GUILayout.Toolbar(activeSkillTabIndex, activeSkillTitles);
            }

            if (GUILayout.Button(tex))
            {
                EditorGUILayout.EndHorizontal();
                
                
                selectionIndex = EditorGUILayout.Popup("laalbeel", selectionIndex, skillNameArray);
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

                Array.Resize(ref activeSkill.skills, activeSkill.skills.Length + 1);
                //*
            } else EditorGUILayout.EndHorizontal();
            #endregion
        }
    }
*/

    const string weaponObjPrefabFolderPath = "Assets/Weapon/Resources/WeaponObjPrefab";
    const string skillScriptsFolderPath = "Assets/Script/Weapon/Skill";

    Texture weaponIcon;
    Texture errorIcon;

    Weapon weapon;
    List<Skill> skills = new List<Skill>();
    List<UnityEditor.Editor> skillEditors = new List<UnityEditor.Editor>();

    string newWeaponName;

    List<Weapon> weaponFiles = new List<Weapon>();

    bool showSelectWeapon = true;

    Vector2 scrollPosition;

    int weaponTabIndex = 0;
    int activeSkillTabIndex = 0;
    int selectionIndex = 0;
    
    const float c = 50;
    const float p = c * 0.01f;


    void Awake()
    {
        weaponIcon = EditorGUIUtility.Load("WeaponIcon16.png") as Texture;
        errorIcon = EditorGUIUtility.Load("WeaponIcon16.png") as Texture;
        EditorGUIUtility.labelWidth = Editor.propertyLabelWidth;
        SkillRefresh();
    }
    [MenuItem("Window/WeaponEditor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(WeaponEditor));
    }
    private void CreateGUI()
    {

    }
    
    void OnGUI()
    {
        if (showSelectWeapon)
        {
            if (weaponFiles == null) WeaponFileRefresh();

            if (GUILayout.Button("Refresh"))
            {
                WeaponFileRefresh();
            }

            for (int i = 0; i < weaponFiles.Count; i++)
            {
                Rect weaponPreview = EditorGUILayout.BeginHorizontal();
                #region WeaponPreview

                const float deletewidth = 60;
                if (GUI.Button(new Rect(weaponPreview.position.x, weaponPreview.position.y,
                        weaponPreview.size.x - deletewidth, weaponPreview.size.y), ""))
                {
                    weapon = weaponFiles[i];
                }
                GUI.backgroundColor = Color.red;
                if (GUI.Button(new Rect(weaponPreview.position.x + weaponPreview.size.x - deletewidth, weaponPreview.position.y,
                        deletewidth, weaponPreview.size.y), "Delete"))
                {
                    string path = $"{weaponObjPrefabFolderPath}/{weaponFiles[i].moduleName}.prefab";
                    if (AssetDatabase.DeleteAsset(path))
                    {
                        Debug.Log($"Delete: {path}");
                        weaponFiles.RemoveAt(i);
                    }
                    else Debug.Log($"Fail: {path}");
                }
                GUI.backgroundColor = Color.white;
                if (weaponFiles[i].UISprite != null) GUILayout.Box(weaponFiles[i].UISprite.texture);
                else GUILayout.Box(errorIcon);

                GUILayout.Box(weaponFiles[i].moduleName);

                EditorGUILayout.EndHorizontal();
                #endregion
            }

            newWeaponName = EditorGUILayout.TextField(newWeaponName);
            if (GUILayout.Button("Create Weapon"))
            {
                UnityEngine.GameObject obj = new UnityEngine.GameObject(newWeaponName);
                Weapon weapon = obj.AddComponent<Weapon>();
                weapon.SpawnModule();
                PrefabUtility.SaveAsPrefabAsset(obj, $"Assets/Weapon/Resources/WeaponObjPrefab/{newWeaponName}.prefab");

                WeaponFileRefresh();
                //정렬해서 삽입 weaponFiles.Add(weapon);
                DestroyImmediate(obj);
            }
        }
        if (weapon == null) showSelectWeapon = true;
        else
        {
            if (GUILayout.Button("Refresh"))
            {
                SkillRefresh();
            }
            GUILayout.Toggle(showSelectWeapon, "Show");

            Rect HorizontalDefault = EditorGUILayout.BeginHorizontal();
            #region Horizontal Default

                weapon.UISprite = (Sprite)EditorGUILayout.ObjectField(weapon.UISprite, typeof(Sprite), false, GUILayout.Width(Editor.propertyHeight * 3.5f), GUILayout.Height(Editor.propertyHeight * 3.5f));

                EditorGUILayout.BeginVertical();
                #region Vertical Stat

                    EditorStyles.textField.fontSize = Editor.titleFontSize;
                    EditorGUILayout.TextField(weapon.moduleName, GUILayout.Height(Editor.titleHeight));
                    EditorStyles.textField.fontSize = Editor.propertyFontSize;

                    weapon.damage = EditorGUILayout.FloatField("Damage", weapon.damage);
                    weapon.durability = EditorGUILayout.IntField("Durability", weapon.maxDurability);

                EditorGUILayout.EndVertical();
                #endregion

            EditorGUILayout.EndHorizontal();
            #endregion

            Vector2 scrollSize = new Vector2(2000, 2000);
            Rect scrollRect = new Rect(
                10,
                HorizontalDefault.position.y + HorizontalDefault.size.y + 10,
                HorizontalDefault.size.x - 20,
                Screen.height - (HorizontalDefault.position.y + HorizontalDefault.size.y + 20) - 20
                );
            scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, new Rect(0, 0, scrollSize.x, scrollSize.y));
            
            Event e = Event.current;
            if (scrollRect.Contains(e.mousePosition) && e.type == EventType.ContextClick)
            {
                GenericMenu menu = new GenericMenu();

                string[] scriptFiles = Directory.GetFiles(skillScriptsFolderPath, "*.cs", SearchOption.TopDirectoryOnly);

                for (int i = 0; i < scriptFiles.Length; i++)
                {
                    string scriptName = scriptFiles[i].Substring(skillScriptsFolderPath.Length, scriptFiles[i].Length - skillScriptsFolderPath.Length - ".cs".Length);
                    menu.AddItem(new GUIContent(scriptName.Substring("Skill_".Length, scriptName.Length - "Skill_".Length)), false, () => CreateSkill(scriptName));
                }
                menu.ShowAsContext();

                e.Use();
            }
            void CreateSkill(string name)
            {
                UnityEngine.GameObject skillObject = new UnityEngine.GameObject();
                skillObject.transform.parent = weapon.transform.Find("NULL");
                Skill skill = (Skill)skillObject.AddComponent(System.Type.GetType($"Skill_{name}"));
                Debug.Log(skill);
            }

                BeginModule("Attack", 4, new Vector2(0, 0));
                ShowActiveSkill("Attack", ref weapon.attack);
            EndModule();

            BeginModule("Special", 4, new Vector2(205, 0));
                ShowActiveSkill("Special", ref weapon.special);
            EndModule();

            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i] == null || skillEditors[i].target != skills[i])
                {
                    skillEditors[i] = UnityEditor.Editor.CreateEditor(skills[i]);
                }
                BeginModule("Special", 4, new Vector2(205, 0));
                    skillEditors[i].OnInspectorGUI();
                EndModule();
            }
            GUI.EndScrollView();

            //if (new Rect(0, 0, Screen.width, Screen.height).Contains(current.mousePosition) && current.type == EventType.ContextClick)
            /*
            {
                GenericMenu menu = new GenericMenu();

                menu.AddDisabledItem(new GUIContent("I clicked on a thing"));
                menu.AddItem(new GUIContent("Do a thing"), false, YourCallback);
                menu.ShowAsContext();

                current.Use();
            }
            */

            void ShowActiveSkill(string title, ref ActiveSkill activeSkill)
            {
                /*
                Rect skillsTitle = EditorGUILayout.BeginHorizontal(GUILayout.Width(width));
                #region Horizontal SkillsTitle

                    string[] activeSkillTitles = new string[activeSkill.skills.Length];
                    for (int titleIndex = 0; titleIndex < activeSkillTitles.Length; titleIndex++)
                    {
                        activeSkillTitles[titleIndex] = activeSkill.skills[titleIndex].moduleName;
                    }

                    activeSkillTabIndex = GUILayout.Toolbar(activeSkillTabIndex, activeSkillTitles);
                    if (activeSkillTabIndex < activeSkill.skills.Length)
                    {
                        if (skillEditor == null || skillEditor.target != activeSkill.skills[activeSkillTabIndex])
                        {
                            skillEditor = UnityEditor.Editor.CreateEditor(activeSkill.skills[activeSkillTabIndex]);
                        }
                        if (skillEditor != null)
                        {
                            skillEditor.OnInspectorGUI();
                        }
                    }

                EditorGUILayout.EndHorizontal();
                #endregion
                */
                EditorGUILayout.BeginHorizontal();
                #region Horizontal Delay

                    EditorGUILayout.LabelField("Delay", GUILayout.Width(Editor.propertyLabelWidth * p));
                    activeSkill.maxDistance = EditorGUILayout.FloatField(activeSkill.maxDistance, GUILayout.MinWidth(0));
                    activeSkill.delay = EditorGUILayout.FloatField(activeSkill.delay, GUILayout.MinWidth(0));
                    activeSkill.backDelay = EditorGUILayout.FloatField(activeSkill.backDelay, GUILayout.MinWidth(0));

                EditorGUILayout.EndHorizontal();
                #endregion

                Rect range = EditorGUILayout.BeginHorizontal();
                #region Horizontal Range

                    EditorGUILayout.LabelField("Range", GUILayout.Width(Editor.propertyLabelWidth * p));

                    float attackMaxDistance = activeSkill.maxDistance;
                    EditorGUILayout.MinMaxSlider(ref activeSkill.minDistance, ref activeSkill.maxDistance, 0, activeSkill.maxDistance);
                    activeSkill.maxDistance = attackMaxDistance;

                    activeSkill.minDistance = EditorGUI.DelayedFloatField(
                            new Rect(Editor.propertyLabelWidth - Editor.shortNoLabelPropertyWidth
                                + (range.size.x - Editor.propertyLabelWidth - Editor.shortNoLabelPropertyWidth - 20) * (activeSkill.minDistance / activeSkill.maxDistance)
                                , range.position.y, Editor.shortNoLabelPropertyWidth, Editor.propertyHeight
                            ), activeSkill.minDistance);

                    activeSkill.maxDistance = EditorGUILayout.FloatField(activeSkill.maxDistance, GUILayout.Width(Editor.shortNoLabelPropertyWidth * p));

                EditorGUILayout.EndHorizontal();
                #endregion

                
            }

            void BeginModule(string name, int width, Vector2 pos)
            {
                EditorGUIUtility.labelWidth = Editor.propertyLabelWidth * p;
                GUILayout.MinWidth(0);

                GUILayout.BeginArea(new Rect(pos.x, pos.y, c * width, scrollRect.size.y - pos.y));
                GUILayout.Box(name, GUILayout.Width(c * width));
            }
            void EndModule()
            {
                EditorGUIUtility.labelWidth = Editor.propertyLabelWidth;

                GUILayout.EndArea();
            }
        }
    }
    void WeaponFileRefresh()
    {
        string[] weaponFileAddresses = Directory.GetFiles(weaponObjPrefabFolderPath, "*.prefab", SearchOption.AllDirectories);
        weaponFiles.Clear();
        for (int i = 0; i < weaponFileAddresses.Length; i++)
        {
            weaponFiles.Add(AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>(weaponFileAddresses[i]).GetComponent<Weapon>());
        }
    }
    void SkillRefresh()
    {
        skills.Clear();
        
        List<Transform> openHolder = new List<Transform> { weapon.transform.Find("Event"), weapon.transform.Find("NULL")};

        for (; ;)
        {
            for (int readIndex = 0; readIndex < weapon.transform.childCount; readIndex++)
            {
                for (int childIndex = 0; childIndex < openHolder[readIndex].childCount; childIndex++)
                {
                    Skill skill = openHolder[readIndex].GetChild(childIndex).GetComponent<Skill>();
                    skills.Add(skill);
                    Transform holder = skill.transform.Find("Event");
                    if (holder != null) openHolder.Add(holder);
                }
            }
        }
    }
}
/*
if (GUILayout.Button(weaponIcon))
{
    EditorGUILayout.EndHorizontal();

    //string[] scriptFiles = Directory.GetFiles(skillScriptsFolderPath, "*.cs", SearchOption.TopDirectoryOnly);

    //string[] skillNameArray = new string[scriptFiles.Length];

    for (int i = 0; i < scriptFiles.Length; i++)
    {
        skillNameArray[i] = scriptFiles[i];//.Substring(skillScriptsFolderPath.Length, scriptFiles[i].Length - 1);
        Debug.Log($"{i}.{scriptFiles[i]}");
    }
    selectionIndex = EditorGUILayout.Popup("laalbeel", selectionIndex, skillNameArray);
*/
