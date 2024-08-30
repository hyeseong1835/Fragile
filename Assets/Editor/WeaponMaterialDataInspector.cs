using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WeaponSystem;
using WeaponSystem.Material;
using WeaponSystem.Material.Usage;

[CustomEditor(typeof(WeaponMaterialData))]
public class WeaponMaterialDataInspector : Editor
{
    const float labelHeight = 20;
    const float textHeight = 25;

    WeaponMaterialData material;
    int usageSelectIndex = 0;
    
    public static FloatingAreaManager floatingManager = new FloatingAreaManager();
    public static Rect floatingHeaderRect;

    void OnEnable()
    {
        material = (WeaponMaterialData)target;
    }
    public override void OnInspectorGUI()
    {
        floatingManager.SetRect(floatingHeaderRect);

        floatingManager.EventListen();

        DrawDescription();

        EditorGUILayout.Space(10);

        DrawUsage();

        floatingManager.SetRect(floatingHeaderRect);

        floatingManager.Draw();

        //===============================================================================================

        void DrawDescription()
        {
            EditorGUILayout.BeginHorizontal();
            {
                Rect iconPreviewRect = GUILayoutUtility.GetAspectRect(1,
                    GUILayout.MinWidth(100),
                        GUILayout.MaxWidth(200),
                        GUILayout.MinHeight(100),
                        GUILayout.MaxHeight(200)
                );

                material.icon = (Sprite)EditorGUI.ObjectField(
                    iconPreviewRect,
                    material.icon,
                    typeof(Sprite),
                    false
                );

                if (Event.current.type == EventType.Repaint)
                {
                    Handles.color = Color.white;
                    Handles.DrawWireCube(iconPreviewRect.center, iconPreviewRect.size);
                }

                EditorGUILayout.BeginVertical();
                {
                    EditorGUILayout.LabelField(
                        "무기 이름",
                        GUILayout.Height(labelHeight),
                            GUILayout.ExpandHeight(false)
                    );
                    material.displayedName = EditorGUILayout.TextField(
                        material.displayedName,
                        GUILayout.Height(textHeight),
                            GUILayout.ExpandHeight(false)
                    );
                    EditorGUILayout.LabelField(
                        "무기 설명",
                        GUILayout.Height(labelHeight),
                            GUILayout.ExpandHeight(false)
                    );
                    material.description = EditorGUI.TextArea(
                        new Rect(
                            iconPreviewRect.xMax + 3,
                            iconPreviewRect.y + (2 * labelHeight + textHeight) + 6,
                            EditorGUIUtility.currentViewWidth - iconPreviewRect.width - 25,
                            iconPreviewRect.height - (2 * labelHeight + textHeight) - 2
                        ),
                        material.description
                    );
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

        void DrawUsage()
        {
            if (material.usages == null) material.usages = new WeaponMaterialUsage[0];

            string[] materialUsageTitleArray = new string[material.usages.Length];
            for (int i = 0; i < material.usages.Length; i++)
            {
                materialUsageTitleArray[i] = material.usages[i].info.name;
            }
            if (usageSelectIndex >= materialUsageTitleArray.Length) 
                usageSelectIndex = materialUsageTitleArray.Length - 1;

            Rect toolBarRect = GUILayoutUtility.GetRect(1, InspectorLayout.toolBarHeight);
            usageSelectIndex = GUI.Toolbar(toolBarRect.AddWidth(-100), usageSelectIndex, materialUsageTitleArray);

            if (GUI.Button(toolBarRect.SetWidth(100, 1), "Add"))
            {
                List<WeaponMaterialUsageInfo> infoList = new List<WeaponMaterialUsageInfo>();
                List<string> names = new List<string>();
                foreach (WeaponMaterialUsageInfo info in WeaponMaterialUsage.infos)
                {
                    if (material.IndexOfUsage(info.type) == -1)
                    {
                        infoList.Add(info);
                        names.Add(info.name);
                    }
                }

                floatingManager.Create(
                    new TextPopupFloatingArea(
                        names.ToArray(),
                        i =>
                        {
                            Debug.Log(infoList[i]);
                            material.AddUsage(infoList[i]);
                            return true;
                        }
                    )
                );
            }
            if (toolBarRect.width > 1) floatingHeaderRect = toolBarRect;

            if (usageSelectIndex >= 0)
            {
                WeaponMaterialUsage usage = material.usages[usageSelectIndex];
                Rect titleRect = GUILayoutUtility.GetRect(1, InspectorLayout.toolBarHeight);
                CustomGUI.TitleHeaderLabel(titleRect, material.usages[usageSelectIndex].info.name);
                if(GUI.Button(titleRect.SetWidth(50, 1).AddHeight(-1), "Delete"))
                {
                    material.usages = material.usages.RemoveAt(usageSelectIndex);
                }
                if (usage.info.type == null)
                {
                    usage.info = WeaponMaterialUsage.GetInfo(GetType());
                }
                Rect headerRect = GUILayoutUtility.GetRect(1, InspectorLayout.toolBarHeight);
                //if (headerRect.width > 1) floatingHeaderRect = headerRect;

                CustomGUI.TitleHeaderLabel(headerRect, usage.info.name);

                if (usage != null)
                {
                    if (Event.current.type == EventType.MouseDown
                    && Event.current.button == 1
                    && headerRect.Contains(Event.current.mousePosition))
                    {
                        GenericMenu menu = new GenericMenu();
                        usage.SetMenu(menu);
                        menu.ShowAsContext();
                    }
                    usage.OnGUI();
                }
            }
        }
    }
}