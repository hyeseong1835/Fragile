using UnityEngine;
using UnityEditor;
using System.IO;

public class RoomEditorFileView : EditorWindow
{
    public float scroll;
    public string[] filePathArray;
    
    public static string openFolderPath = "Assets/Resources/Stage/Stage 1/Room";

    
    [MenuItem("Window/RoomEditor/File")]
    public static void ShowWindow()
    {
        RoomEditorFileView window = GetWindow<RoomEditorFileView>();
        window.Show();
    }
    void OnEnable()
    {
        filePathArray = Directory.GetDirectories(openFolderPath);
    }
    void OnGUI()
    {
        EditorGUILayout.BeginScrollView(new Vector2(0, scroll));
        {
            EditorGUILayout.BeginHorizontal();
            {
                openFolderPath = GUILayout.TextField(openFolderPath);
                if (GUILayout.Button("Load"))
                {
                    filePathArray = Directory.GetDirectories(openFolderPath);
                }
            }
            EditorGUILayout.EndHorizontal();

            CustomGUILayout.TitleHeaderLabel("File");
            if (filePathArray != null)
            {
                foreach (string path in filePathArray)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(Path.GetFileName(path));
                        if (GUILayout.Button("Load"))
                        {
                            RoomEditor.OpenRoom(path);
                            
                            GUIUtility.ExitGUI();
                            return;
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
        EditorGUILayout.EndScrollView();
    }
}
