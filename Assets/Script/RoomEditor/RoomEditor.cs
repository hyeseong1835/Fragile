using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public static class RoomEditor
{
    public static RoomEditorData data;
    public static RoomEditorSetting setting;

    
    [InitializeOnLoadMethod]
    static void OnLoad()
    {
        data = (RoomEditorData)EditorGUIUtility.Load("RoomEditor/Data.asset");
        setting = (RoomEditorSetting)EditorGUIUtility.Load("RoomEditor/Setting.asset");
    }

    public static void OpenRoom(string directoryPath)
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject roomSettingObj = new GameObject("Room Setting");
        EditorRoom roomSetting = roomSettingObj.AddComponent<EditorRoom>();
        {
            roomSetting.path = directoryPath;

            GameObject gridObj = new GameObject("Grid");
            {
                roomSetting.grid = gridObj.AddComponent<Grid>();
            }
            GameObject tilemapObj = new GameObject("Tilemap");
            {
                tilemapObj.transform.parent = gridObj.transform;
                roomSetting.tilemap = tilemapObj.AddComponent<Tilemap>();
                roomSetting.tilemapRenderer = tilemapObj.AddComponent<TilemapRenderer>();
            }
            SceneManager.MoveGameObjectToScene(gridObj, scene);
        }
        SceneManager.MoveGameObjectToScene(roomSettingObj, scene);

        SceneManager.SetActiveScene(scene);
    }
    public static void SaveRoom(EditorRoom room)
    {
        if (Directory.Exists(room.path) == false)
        {
            Directory.CreateDirectory(room.path);
        }
        RoomData roomData = ScriptableObject.CreateInstance<RoomData>();
        {

        }
        AssetDatabase.CreateAsset(roomData, $"{room.path}/Data.asset");
    }
}
