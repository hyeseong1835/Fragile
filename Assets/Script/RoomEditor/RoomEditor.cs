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
            roomSetting.chunckList.AddRange(room.chunkArray);
            roomSetting.RefreshCanAddChunckList();
        }
        SceneManager.MoveGameObjectToScene(roomSettingObj, scene);

        GameObject gridObj = new GameObject("Grid");
        {
            roomSetting.grid = gridObj.AddComponent<Grid>();

            roomSetting.layerList = new List<Layer>();
            {
                foreach (TileLayer tileLayer in room.tileLayerArray)
                {
                    GameObject layerObj = new GameObject(tileLayer.name);
                    {
                        layerObj.transform.parent = gridObj.transform;
                    }
                    Layer addLayer = new Layer(
                        layerObj.AddComponent<Tilemap>(),
                        layerObj.AddComponent<TilemapRenderer>()
                    );
                    {
                        foreach (TileInfo tileInfo in tileLayer.tileArray)
                        {
                            addLayer.tilemap.SetTile(
                                new Vector3Int(tileInfo.pos.x, tileInfo.pos.y, 0),
                                tileInfo.tile
                            );
                        }
                    }
                    roomSetting.layerList.Add(addLayer);
                }
            }
        }
        SceneManager.MoveGameObjectToScene(gridObj, scene);

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
            roomData.chunkArray = room.chunckList.ToArray();

            roomData.tileLayerArray = new TileLayer[room.layerList.Count];
            {
                for (int i = 0; i < roomData.tileLayerArray.Length; i++)
                {
                    Tilemap tilemap = room.layerList[i].tilemap;

                    roomData.tileLayerArray[i].name = tilemap.gameObject.name;

                    List<TileInfo> tileInfos = new List<TileInfo>();
                    {
                        BoundsInt bounds = tilemap.cellBounds;
                        for (int x = bounds.xMin; x <= bounds.xMax; x++)
                        {
                            for (int y = bounds.yMin; y <= bounds.yMax; y++)
                            {
                                Vector3Int pos = new Vector3Int(x, y, 0);
                                TileBase tile = tilemap.GetTile(pos);
                                if (tile != null)
                                {
                                    tileInfos.Add(new TileInfo
                                    {
                                        pos = new Vector2Int(x, y),
                                        tile = tile
                                    });
                                }
                            }
                        }
                    }
                    roomData.tileLayerArray[i].tileArray = tileInfos.ToArray();
                }
            }
        }
        AssetDatabase.CreateAsset(roomData, $"{room.path}/Data.asset");
    }
}
