using DG.DemiEditor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CreateAsset : EditorWindow
{
    string newControllerName;

    string[] controllerTypeNameArray;
    string newControllerTypeName;

    bool showControllerTypeNameDropDown = false;

    void Awake()
    {
        RefreshControllerTypeNameArray();
    }
    [MenuItem("Window/CreateAsset")]
    public static void CreateController()
    {
        CreateAsset window = GetWindow<CreateAsset>();
        window.Show();
    }
    void RefreshControllerTypeNameArray()
    {
        //로드
        string[] controllerTypePathArray = Directory.GetFiles(Controller.scriptFolderPath, "*.cs", SearchOption.TopDirectoryOnly);

        //할당
        controllerTypeNameArray = new string[controllerTypePathArray.Length];
        for (int i = 0; i < controllerTypePathArray.Length; i++)
        {
            controllerTypeNameArray[i] = controllerTypePathArray[i].Substring(Controller.scriptFolderPath.Length + 1, controllerTypePathArray[i].Length - Controller.scriptFolderPath.Length - 4);
        }

        //임시 값 지정
        if (newControllerName == default)
        {
            if (controllerTypePathArray.Length > 0)
            {
                newControllerTypeName = controllerTypeNameArray[0];
            }
            else newControllerTypeName = string.Empty;
        }
    }
    
    void OnGUI()
    {
        if (GUILayout.Button("Refresh")) RefreshControllerTypeNameArray();

        GUILayout.Space(Editor.propertyHeight);

        GUILayout.Label("Controller");

        GUILayout.BeginHorizontal();
        #region Horizontal Controller

            newControllerName = EditorGUILayout.TextField(newControllerName);
            
            if (GUILayout.Button(newControllerTypeName, GUILayout.Width(Editor.shortButtonWidth), GUILayout.Height(Editor.propertyHeight)))
            {
                RefreshControllerTypeNameArray();
                showControllerTypeNameDropDown = true;
            }
            
            int selectIndex = DropDown(GUILayoutUtility.GetLastRect().AddY(Editor.propertyHeight),
                showControllerTypeNameDropDown, controllerTypeNameArray
            );

            Event prevEvent = new Event(Event.current);
            if (selectIndex != -1)
            {
                newControllerTypeName = controllerTypeNameArray[selectIndex];
                showControllerTypeNameDropDown = false;
            }
            else if (Event.current.OnMouseDown(0) || Event.current.OnMouseDown(1) || Event.current.isKey)
            {
                showControllerTypeNameDropDown = false;
                Event.current = prevEvent;
            }


            if (GUILayout.Button("Create", GUILayout.Width(Editor.shortButtonWidth)))
            {
                Debug.Log($"Create Controller : {newControllerName}");
                //Folder
                string folderPath = $"{ControllerData.folderPath}/{newControllerName}";
                if (AssetDatabase.IsValidFolder(folderPath) == false)
                {
                    AssetDatabase.CreateFolder(ControllerData.folderPath, newControllerName);
                    Debug.Log($"Create Folder : {folderPath}");
                }

                //Prefab
                Controller con;
                string prefabPath = $"{folderPath}/Prefab.prefab";

                //Data
                ControllerData data;
                string dataPath = $"{folderPath}/ControllerData.asset";

                if (Directory.Exists(prefabPath) == false)
                {
                    GameObject prefab = new GameObject("Prefab");

                    con = (Controller)prefab.AddComponent(Utility.LoadType(newControllerTypeName));
                
                    LoadData();

                    con.Create();

                    PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath);
                    DestroyImmediate(prefab);

                    Debug.Log($"Create Prefab : {prefabPath}");
                } 
                else
                { 
                    con = AssetDatabase.LoadAssetAtPath<Controller>(prefabPath);

                    LoadData();
                }
                void LoadData()
                {
                    if (Directory.Exists(dataPath) == false)
                    {
                        data = (ControllerData)ScriptableObject.CreateInstance(con.DataType.Name);
                        AssetDatabase.CreateAsset(data, dataPath);
                        Debug.Log($"Create Data : {dataPath}");
                    }
                    else data = AssetDatabase.LoadAssetAtPath<ControllerData>(dataPath);

                    data.name = newControllerName;
                    con.ControllerData = data;
                }
            }
        
        GUILayout.EndHorizontal();
        #endregion
    }


    int DropDown(Rect firstItemRect, bool show, string[] items)
    {
        if (show)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (GUI.Button(firstItemRect.AddY(i * firstItemRect.height), items[i]))
                {
                    return i;
                }
            }
        }
     
        return -1;
    }
}
