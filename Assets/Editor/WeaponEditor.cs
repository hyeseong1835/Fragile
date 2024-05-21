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
using System.Linq;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using DG.Tweening.Plugins.Core.PathCore;

public class WeaponEditor : EditorWindow
{
    /*
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
*/

    const string weaponObjPrefabFolderPath = "Assets/Weapon/Resources/WeaponObjPrefab";
    const string skillScriptsFolderPath = "Assets/Script/Weapon/Skill";

    Texture weaponIcon;
    Texture errorIcon;

    Weapon weapon;
    List<Skill> skills = new List<Skill>();

    string newWeaponName;

    List<Weapon> weaponFiles = new List<Weapon>();

    bool showSelectWeapon = true;

    Vector2 scrollPosition;

    
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
                weapon.moduleName = newWeaponName;
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
            Rect scrollRect = new Rect(10,
                HorizontalDefault.position.y + HorizontalDefault.size.y + 10,
                HorizontalDefault.size.x - 20,
                Screen.height - (HorizontalDefault.position.y + HorizontalDefault.size.y + 20) - 20);
            scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, new Rect(0, 0, scrollSize.x, scrollSize.y));
            #region ScrollView Skill

                Event e = Event.current;
                if (scrollRect.Contains(e.mousePosition) && e.type == EventType.ContextClick)
                {
                    GenericMenu menu = new GenericMenu();

                    string[] scriptFiles = Directory.GetFiles(skillScriptsFolderPath, "*.cs", SearchOption.TopDirectoryOnly);

                    for (int i = 0; i < scriptFiles.Length; i++)
                    {
                        string scriptName = scriptFiles[i].Substring(skillScriptsFolderPath.Length + 1, scriptFiles[i].Length - skillScriptsFolderPath.Length - ".cs".Length - 1);
                        menu.AddItem(new GUIContent(scriptName.Substring("Skill_".Length, scriptName.Length - "Skill_".Length)), false, () => CreateSkill(scriptName));
                    }
                    menu.ShowAsContext();

                    e.Use();
                }
                

                BeginModule("Attack", 4, new Vector2(0, 0));

                    ShowActiveSkill("Attack", ref weapon.attack);

                EndModule();

                BeginModule("Special", 4, new Vector2(205, 0));

                    ShowActiveSkill("Special", ref weapon.special);

                EndModule();

                for (int i = 0; i < skills.Count; i++)
                {
                    if (skills[i] == null)
                    {
                        skills.RemoveAt(i);
                    }
                    BeginModule(skills[i].moduleName, 4, new Vector2(405 + 200 * i, 0));

                        if (skills[i].skillEditor == null || skills[i].skillEditor.target != skills[i])
                        {
                            skills[i].skillEditor = UnityEditor.Editor.CreateEditor(skills[i]);
                        }
                        skills[i].skillEditor.OnInspectorGUI();

                    EndModule();
                }

            GUI.EndScrollView();
            #endregion

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
            void CreateSkill(string scriptName)
            {
                //PrefabUtility.ReplacePrefab(gameObject, PrefabUtility.GetPrefabParent(gameObject), ReplacePrefabOptions.ConnectToPrefab);
                UnityEngine.GameObject newPrefab = Instantiate(Resources.Load<UnityEngine.GameObject>($"WeaponObjPrefab/{weapon.moduleName}"));
                newPrefab.name = weapon.moduleName;

                UnityEngine.GameObject skillObject = new UnityEngine.GameObject();
                skillObject.transform.parent = newPrefab.transform.GetChild(0).GetChild(3);
                Type skillT = Type.GetType(scriptName);
                if (skillT == null)
                {
                    var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                    var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
                    foreach (var assemblyName in referencedAssemblies)
                    {
                        var assembly = System.Reflection.Assembly.Load(assemblyName);
                        if (assembly != null)
                        {
                            skillT = assembly.GetType(scriptName);
                            if (skillT != null) break;
                        }
                    }
                }
                Skill skill = (Skill)skillObject.AddComponent(skillT);

                if (skill != null) Debug.Log($"Add: {scriptName}");
                else Debug.LogError($"Fail: {scriptName}");
                
                string path = $"{weaponObjPrefabFolderPath}/{weapon.moduleName}.prefab";
                AssetDatabase.DeleteAsset(path);
                PrefabUtility.SaveAsPrefabAsset(newPrefab, path);
            }
            void BeginModule(string name, int width, Vector2 pos)
            {
                //EditorGUIUtility.labelWidth = Editor.propertyLabelWidth * p;
                //GUILayout.MinWidth(0);

                GUILayout.BeginArea(new Rect(pos.x, pos.y, c * width, scrollRect.size.y - pos.y));
                GUILayout.Box(name, GUILayout.Width(c * width));
            }
            void EndModule()
            {
                //EditorGUIUtility.labelWidth = Editor.propertyLabelWidth;

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

        if (weapon == null) return;

        List<Transform> openHolder = new List<Transform>();

        for (int i = 0; i < weapon.transform.childCount; i++)
        {
            Transform holder = weapon.transform.GetChild(i);
            if (holder.name == "Event") openHolder.Add(holder);
        }
        for (int i = 0; i < weapon.transform.childCount; i++)
        {
            Transform holder = weapon.transform.GetChild(i);
            if (holder.name == "NULL") openHolder.Add(holder);
        }
        
        while (openHolder.Count > 0)
        {
            for (int readIndex = 0; readIndex < openHolder.Count; readIndex++)
            {
                Transform open = openHolder[readIndex];
                for (int eventIndex = 0; eventIndex < open.childCount; eventIndex++)
                {
                    Transform skillEvent = open.GetChild(eventIndex);
                    for (int skillIndex = 0; skillIndex < skillEvent.childCount; skillIndex++)
                    {
                        Skill skill = skillEvent.GetChild(skillIndex).GetComponent<Skill>();
                        if (skill == null)
                        {
                            Debug.LogError($"{skillEvent} has NotSkill Object({skillEvent.GetChild(skillIndex).name})");
                            continue;
                        }
                        skills.Add(skill);
                        for (int i = 0; i < skill.transform.childCount; i++)
                        {
                            Transform eventHolder = skill.transform.GetChild(i);
                            if (eventHolder.name == "Event") openHolder.Add(eventHolder);
                        }
                    }
                }
                openHolder.RemoveAt(readIndex);
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
