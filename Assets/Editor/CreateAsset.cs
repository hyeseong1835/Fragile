using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

public class CreateAsset : EditorWindow
{
    const string presetFolderPath = "Assets/Editor Default Resources/Preset";
    string newControllerName;

    string[] controllerPresetNameArray;
    string newControllerPresetName;

    bool showControllerTypeNameDropDown = false;

    void Awake()
    {
        RefreshControllerPresetNameArray();
    }

    [MenuItem("Window/CreateAsset")]
    public static void CreateController()
    {
        CreateAsset window = GetWindow<CreateAsset>();
        window.Show();
    }

    void RefreshControllerPresetNameArray()
    {
        const string controllerPresetPath = presetFolderPath + "/Controller";

        //로드
        string[] controllerPresetPathArray = Directory.GetFiles(controllerPresetPath, "*.prefab", SearchOption.TopDirectoryOnly);

        //할당
        controllerPresetNameArray = new string[controllerPresetPathArray.Length];
        for (int i = 0; i < controllerPresetPathArray.Length; i++)
        {
            controllerPresetNameArray[i] = controllerPresetPathArray[i].Substring(controllerPresetPath.Length + 1, controllerPresetPathArray[i].Length - controllerPresetPath.Length - 8);
        }

        //임시 값 지정
        if (newControllerName == default)
        {
            if (controllerPresetPathArray.Length > 0)
            {
                newControllerPresetName = controllerPresetNameArray[0];
            }
            else newControllerPresetName = string.Empty;
        }
    }
    
    void OnGUI()
    {
        GUILayout.Label("Controller");

        GUILayout.BeginHorizontal();
        #region Horizontal Controller

            newControllerName = EditorGUILayout.TextField(newControllerName);
            
            if (GUILayout.Button(newControllerPresetName, GUILayout.Width(Editor.shortButtonWidth), GUILayout.Height(Editor.propertyHeight)))
            {
                RefreshControllerPresetNameArray();
                showControllerTypeNameDropDown = true;
            }
            
            int selectIndex = DropDown(GUILayoutUtility.GetLastRect().AddY(Editor.propertyHeight),
                showControllerTypeNameDropDown, controllerPresetNameArray
            );

            Event prevEvent = new Event(Event.current);
            if (selectIndex != -1)
            {
                newControllerPresetName = controllerPresetNameArray[selectIndex];
                showControllerTypeNameDropDown = false;
            }
            else if (Event.current.OnMouseDown(0) || Event.current.OnMouseDown(1) || Event.current.isKey)
            {
                showControllerTypeNameDropDown = false;
                Event.current = prevEvent;
            }

            if (GUILayout.Button("Create", GUILayout.Width(Editor.shortButtonWidth)))
            {
                CreateController();
            }
        
        GUILayout.EndHorizontal();
        #endregion



        void CreateController()
        {
            //Folder
            string folderPath = $"{ControllerData.controllersFolderPath}/{newControllerName}";
            if (AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogWarning($"Already Exist : {folderPath}");
                return;
            }

            AssetDatabase.CreateFolder(ControllerData.controllersFolderPath, newControllerName);

            GameObject prefabInstance = Instantiate(EditorResources.Load<GameObject>($"Preset/Controller/{newControllerPresetName}.prefab"));
            Controller con = prefabInstance.GetComponent<Controller>();

            con.ControllerData = (ControllerData)ScriptableObject.CreateInstance(con.DataType);
            AssetDatabase.CreateAsset(con.ControllerData, $"{folderPath}/ControllerData.asset");

            con.ControllerData.name = newControllerName;

            PrefabUtility.SaveAsPrefabAsset(prefabInstance, $"{folderPath}/{newControllerName}.prefab");
            DestroyImmediate(prefabInstance);

            Debug.Log($"Create Controller : {newControllerName}({newControllerPresetName})");
        }
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