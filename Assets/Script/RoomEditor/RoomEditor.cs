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

        RoomData room = AssetDatabase.LoadAssetAtPath<RoomData>($"{directoryPath}/Data.asset");
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
                {
                    for (int chunckIndex = 0; chunckIndex < roomSetting.chunckList.Count; chunckIndex++)
                    {
                        Vector3Int chunckOriginPos = (Vector3Int)room.chunkArray[chunckIndex].pos * 16;
                        for (int x = 0; x < 16; x++)
                        {
                            for (int y = 0; y < 16; y++)
                            {
                                roomSetting.tilemap.SetTile(
                                    chunckOriginPos + new Vector3Int(x, y, 0),
                                    room.chunkArray[chunckIndex].tiles[x + y * 16]
                                );
                            }
                        }
                    }
                }
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
                roomData.chunkArray[i].pos = room.chunckList[i];
                roomData.chunkArray[i].tiles = new TileBase[256];
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++)
                    {
                        roomData.chunkArray[i].tiles[x + y * 16] = room.tilemap.GetTile(new Vector3Int(x, y, 0));
                    }
                }
            }
        }
        AssetDatabase.CreateAsset(roomData, $"{room.path}/Data.asset");
    }
}
