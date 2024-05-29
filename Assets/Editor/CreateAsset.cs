using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CreateAsset : EditorWindow
{
    string newControllerName;

    [MenuItem("Window/CreateAsset")]
    public static void CreateController()
    {
        CreateAsset window = GetWindow<CreateAsset>();
        window.Show();
    }
    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        newControllerName = EditorGUILayout.TextField(newControllerName);
        if (GUILayout.Button("Create Controller"))
        {
            AssetDatabase.CreateFolder(Controller.entityFolderPath, newControllerName);

            string folderPath = $"{Controller.entityFolderPath}/{newControllerName}";
            ControllerData data = ScriptableObject.CreateInstance<ControllerData>();
            data.name = newControllerName;
            AssetDatabase.CreateAsset(data, $"{folderPath}/ControllerData.asset");

            string[] scriptFiles = Directory.GetFiles(Controller.controllerFolderPath, "*.cs", SearchOption.TopDirectoryOnly);

            int index = 1;
            string scriptName = scriptFiles[index].Substring(Controller.controllerFolderPath.Length + 1, scriptFiles[index].Length - Controller.controllerFolderPath.Length - 4);

            GameObject prefab = new GameObject("Prefab");

            System.Type ConT = System.Type.GetType(scriptName);
            if (ConT == null)
            {
                var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();
                var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
                foreach (var assemblyName in referencedAssemblies)
                {
                    var assembly = System.Reflection.Assembly.Load(assemblyName);
                    if (assembly != null)
                    {
                        ConT = assembly.GetType(scriptName);
                        if (ConT != null) break;
                    }
                }
            }

            Controller con = (Controller)prefab.AddComponent(ConT);

            if (con != null) Debug.Log($"Add: {scriptName}");
            else Debug.LogError($"Fail: {scriptName}");

            con.data = data;
            con.Create();
            PrefabUtility.SaveAsPrefabAsset(prefab, $"{folderPath}/Prefab.prefab");
            DestroyImmediate(prefab);
        }
        GUILayout.EndHorizontal();
    }
}
