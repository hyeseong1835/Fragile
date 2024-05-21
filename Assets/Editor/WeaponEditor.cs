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
using UnityEditorInternal.VR;


public class WeaponEditor : EditorWindow
{
    public class EditorWeaponData : ScriptableObject
    {
        public Weapon weapon;
        public List<ModuleData> modules = new List<ModuleData>();
    }
    [Serializable]
    public abstract class ModuleData
    {
        public Rect rect;
        public abstract string GetName();
    }
    
    [Serializable]
    public class PassiveSkillModuleData : ModuleData
    {
        public PassiveSkillModuleData(string _name, ActiveSkill _activeSkill, Rect _rect)
        {
            name = _name;
            activeSkill = _activeSkill;
            rect = _rect;
        }
        public string name;
        public ActiveSkill activeSkill;

        public override string GetName()
        {
            return name;
        }
    }
    [Serializable]
    public class ActiveSkillModuleData : ModuleData
    {
        public ActiveSkillModuleData(string _name, ActiveSkill _activeSkill, Rect _rect)
        {
            name = _name;
            activeSkill = _activeSkill;
            rect = _rect;
        }
        public string name;
        public ActiveSkill activeSkill;

        public override string GetName()
        {
            return name;
        }
    }
    [Serializable]
    public class SkillModuleData : ModuleData
    {
        public SkillModuleData(Skill _skill, UnityEditor.Editor _editor, Rect _rect)
        {
            skill = _skill;
            editor = _editor;
            rect = _rect;
        }
        public SkillModuleData(SkillModuleData data, Rect _rect)
        {
            skill = data.skill;
            editor = data.editor;
            rect = _rect;
        }
        public Skill skill;
        public UnityEditor.Editor editor;

        public override string GetName()
        {
            return skill.moduleName;
        }
    }
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

    const string weaponDataFolderPath = "Assets/Editor Default Resources/WeaponData";
    const string weaponObjPrefabFolderPath = "Assets/Weapon/Resources/WeaponObjPrefab";
    const string skillScriptsFolderPath = "Assets/Script/Weapon/Skill";

    const float scrollSizeX = 2000;
    const float scrollSizeY = 2000;
    const float c = 50;
    const float p = c * 0.01f;
    const float headerH = 0.5f * c;
    const float w = 4 * c;

    Rect defaultModuleRect = new Rect(0, 0, w, 0);

    Texture weaponIcon;
    Texture errorIcon;

    Event e { get { return Event.current; } }
    Weapon weapon { 
        get { return editorData.weapon; }
        set { editorData.weapon = value; }
    }
    List<ModuleData> modules { 
        get { return editorData.modules; }
        set { editorData.modules = value; }
    }


    EditorWeaponData editorData;
    

    string newWeaponName;
    List<EditorWeaponData> weaponDatas = new List<EditorWeaponData>();

    bool showSelectWeapon = true;

    Rect scrollRect;
    Vector2 scrollPosition;

    SkillModuleData holdSkillModule = default;
    Vector2 holdSkillPosOffset = default;

    void Awake()
    {
        weaponIcon = EditorGUIUtility.Load("WeaponIcon16.png") as Texture;
        errorIcon = EditorGUIUtility.Load("WeaponIcon16.png") as Texture;

        string[] weaponDataPathes = Directory.GetFiles(weaponDataFolderPath);
        foreach (string path in weaponDataPathes) weaponDatas.Add(AssetDatabase.LoadAssetAtPath<EditorWeaponData>(path));
        //SkillRefresh();
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
            if (weaponDatas == null) WeaponFileRefresh();

            if (GUILayout.Button("Refresh"))
            {
                WeaponFileRefresh();
            }

            for (int i = 0; i < weaponDatas.Count; i++)
            {
                Rect weaponPreview = EditorGUILayout.BeginHorizontal();
                #region Horizontal WeaponPreview

                    const float deletewidth = 60;

                    //배경(버튼)
                    if (GUI.Button(new Rect(weaponPreview.position.x, weaponPreview.position.y,
                            weaponPreview.size.x - deletewidth, weaponPreview.size.y), ""))
                    {
                    editorData = weaponDatas[i];
                        SkillRefresh();
                    }

                    //삭제 버튼
                    GUI.backgroundColor = Color.red;
                    if (GUI.Button(new Rect(weaponPreview.position.x + weaponPreview.size.x - deletewidth, weaponPreview.position.y,
                            deletewidth, weaponPreview.size.y), "Delete"))
                    {
                        string path = $"{weaponObjPrefabFolderPath}/{weaponDatas[i].weapon.moduleName}.prefab";
                        if (AssetDatabase.DeleteAsset(path))
                        {
                            Debug.Log($"Delete: {path}");
                            weaponDatas.RemoveAt(i);
                        }
                        else Debug.Log($"Fail: {path}");
                    }
                    
                    //무기 텍스처
                    GUI.backgroundColor = Color.white;
                    if (weaponDatas[i].weapon.UISprite != null) GUILayout.Box(weaponDatas[i].weapon.UISprite.texture);
                    else GUILayout.Box(errorIcon);

                    //무기 이름
                    GUILayout.Box(weaponDatas[i].weapon.moduleName);

                EditorGUILayout.EndHorizontal();
                #endregion
            }

            //새로운무기 생성 버튼
            newWeaponName = EditorGUILayout.TextField(newWeaponName);
            if (GUILayout.Button("Create Weapon"))
            {
                UnityEngine.GameObject obj = new UnityEngine.GameObject(newWeaponName);
                Weapon newWeapon = obj.AddComponent<Weapon>();
                newWeapon.moduleName = newWeaponName;
                newWeapon.SpawnModule();
                PrefabUtility.SaveAsPrefabAsset(obj, $"Assets/Weapon/Resources/WeaponObjPrefab/{newWeaponName}.prefab");
                DestroyImmediate(obj);

                WeaponFileRefresh();
                //정렬해서 삽입 weaponFiles.Add(weapon);
            }
        }
        if (editorData == null)
        {
            showSelectWeapon = true;
            return;
        }
        

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


            scrollRect = new Rect(10,
                HorizontalDefault.position.y + HorizontalDefault.size.y + 10,
                HorizontalDefault.size.x - 20,
                Screen.height - (HorizontalDefault.position.y + HorizontalDefault.size.y + 20) - 20);
            scrollPosition = GUI.BeginScrollView(scrollRect, scrollPosition, new Rect(0, 0, scrollSizeX, scrollSizeY));
            #region ScrollView Skill

            ScrollMouseDown();
            ScrollMouseStay();
            ScrollMouseUp();


            BeginModule(new ActiveSkillModuleData("Attack", weapon.attack, defaultModuleRect));

            ShowActiveSkill(weapon.attack);

            EndModule();

            BeginModule(new ActiveSkillModuleData("Special", weapon.attack, defaultModuleRect.AddPosition(w + 0.5f, 0)));

            ShowActiveSkill(weapon.special);

            EndModule();

            for (int i = 0; i < modules.Count; i++)
            {
                BeginModule(modules[i]);

                if (modules[i] is SkillModuleData)
                {
                    SkillModuleData skillModule = modules[i] as SkillModuleData;

                    if (skillModule.editor == null || skillModule.editor.target != skillModule.skill)
                    {
                        skillModule.editor = UnityEditor.Editor.CreateEditor(skillModule.skill);
                    }
                    skillModule.editor.OnInspectorGUI();
                }
                else if (modules[i] is ActiveSkillModuleData)
                {
                    ActiveSkillModuleData activeModule = modules[i] as ActiveSkillModuleData;

                    ShowActiveSkill(activeModule.activeSkill);
                }
                else if (modules[i] is PassiveSkillModuleData)
                {

                }
                else Debug.LogError("타입이 유효하지 않음");

                EndModule();
        }

            GUI.EndScrollView();
            #endregion

            void ShowActiveSkill(ActiveSkill activeSkill)
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
    }
    void WeaponFileRefresh()
    {
        string[] weaponFileAddresses = Directory.GetFiles(weaponObjPrefabFolderPath, "*.asset", SearchOption.TopDirectoryOnly);
        weaponDatas.Clear();
        for (int i = 0; i < weaponFileAddresses.Length; i++)
        {
            weaponDatas.Add(AssetDatabase.LoadAssetAtPath<EditorWeaponData>(weaponFileAddresses[i]));
        }
    }
    void SkillRefresh()
    {
        modules.Clear();

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
                        modules.Add(
                            new SkillModuleData (skill, UnityEditor.Editor.CreateEditor(skill), new Rect())
                        );
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
    void ScrollMouseDown()
    {
        if (e.type == EventType.MouseDown)
        {
            //모듈 헤더 클릭
            for (int dataIndex = 0; dataIndex < modules.Count; dataIndex++)
            {
                if (modules[dataIndex].rect.SetSize(w, headerH).Contains(e.mousePosition) == false) continue;

                Debug.Log("Hit");
                switch (e.button)
                {
                    //좌클릭
                    case 0:
                        if (modules[dataIndex] is SkillModuleData)
                        {
                            SkillModuleData skillModule = modules[dataIndex] as SkillModuleData;

                            holdSkillModule = skillModule;
                            modules.Remove(holdSkillModule);
                            modules.Insert(0, holdSkillModule);

                            holdSkillPosOffset = e.mousePosition - holdSkillModule.rect.position;
                        }
                        else if (modules[dataIndex] is ActiveSkillModuleData)
                        {
                            ActiveSkillModuleData activeModule = modules[dataIndex] as ActiveSkillModuleData;

                        }
                        else if (modules[dataIndex] is PassiveSkillModuleData)
                        {

                        }
                        else Debug.LogError("타입이 유효하지 않음");
                        return;
                    //우클릭
                    case 1:
                        GenericMenu menu = new GenericMenu();



                        menu.ShowAsContext();
                        e.Use();
                        return;
                }
            }
            //배경 클릭
            if (scrollRect.Contains(e.mousePosition))
            {
                switch (e.button)
                {
                    //좌클릭
                    case 0:
                        
                        return;
                    //우클릭
                    case 1:
                        GenericMenu menu = new GenericMenu();
                        string[] scriptFiles = Directory.GetFiles(skillScriptsFolderPath, "*.cs", SearchOption.TopDirectoryOnly);

                        for (int i = 0; i < scriptFiles.Length; i++)
                        {
                            string scriptName = scriptFiles[i].Substring(skillScriptsFolderPath.Length + 1, scriptFiles[i].Length - skillScriptsFolderPath.Length - ".cs".Length - 1);
                            menu.AddItem(new GUIContent(scriptName.Substring("Skill_".Length, scriptName.Length - "Skill_".Length)), false, () => CreateSkill(scriptName, e.mousePosition));
                        }
                        menu.ShowAsContext();
                        e.Use();
                        return;
                }
            }
        }
    }
    void ScrollMouseStay()
    {
        Event e = Event.current;

        if (holdSkillModule != null && e.type == EventType.MouseMove)
        {
            holdSkillModule.rect.position = e.mousePosition + holdSkillPosOffset;
        }
        
    }
    void ScrollMouseUp()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseUp)
        {
            holdSkillModule = null;
        }
    }
    Skill CreateSkill(string scriptName, Vector2 pos)
    {
        //새 프리팹 생성
        string resourcePath = $"WeaponObjPrefab/{weapon.moduleName}";
        UnityEngine.GameObject newPrefab = Instantiate(Resources.Load<UnityEngine.GameObject>(resourcePath));
        newPrefab.name = weapon.moduleName;
        weapon = newPrefab.GetComponent<Weapon>();

        //스킬 추가
        UnityEngine.GameObject skillObject = new UnityEngine.GameObject(scriptName);
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

        //저장
        string prefabPath = $"{weaponObjPrefabFolderPath}/{weapon.moduleName}.prefab";
        AssetDatabase.DeleteAsset(prefabPath);
        PrefabUtility.SaveAsPrefabAsset(newPrefab, prefabPath);
        DestroyImmediate(newPrefab);

        weapon = Resources.Load<UnityEngine.GameObject>(resourcePath).GetComponent<Weapon>();

        SkillRefresh();

        return skill;
    }
    void BeginModule(ModuleData data)
    {
        GUILayout.BeginArea(data.rect.SetSize(data.rect.size.x, scrollRect.size.y - data.rect.position.y));
        data.rect = EditorGUILayout.BeginVertical();
        GUILayout.Box(data.GetName(), GUILayout.Width(data.rect.size.x), GUILayout.Height(headerH));
    }
    void EndModule()
    {
        EditorGUILayout.EndVertical();
        GUILayout.EndArea();
    }
    Rect ScreenToScrollRect(Rect rect)
    {
        return rect.SetPosition(rect.position + (scrollPosition + scrollRect.position));
    }
    Vector2 ScreenToScrollRect(Vector2 pos)
    {
        return pos + (scrollPosition + scrollRect.position);
    }
    Rect ScrollRectToScreen(Rect rect)
    {
        return rect.SetPosition(rect.position - (scrollPosition + scrollRect.position));
    }
    Vector2 ScrollRectToScreen(Vector2 pos)
    {
        return pos - (scrollPosition + scrollRect.position);
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
