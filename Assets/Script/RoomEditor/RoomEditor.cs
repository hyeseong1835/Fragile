using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public static class RoomEditor
{
    public static RoomEditorData data;
    public static RoomEditorSetting setting;

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
            roomData.chunkArray = new ChunkData[room.chunckList.Count];

            for (int i = 0; i < roomData.chunkArray.Length; i++)
            {
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++)
                    {
                        roomData.chunkArray[i].pos = room.chunckList[i];
                    }
                }
            }
        }
        AssetDatabase.CreateAsset(roomData, $"{room.path}/Data.asset");
    }
}
