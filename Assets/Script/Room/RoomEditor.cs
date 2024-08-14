using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static UnityEditor.Experimental.GraphView.GraphView;

public static class RoomEditor
{
    public static RoomEditorData data;
    public static RoomEditorSetting setting;

    public static void OpenRoom(string directoryPath)
    {
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        {
            //데이터 로드
            RoomData room = AssetDatabase.LoadAssetAtPath<RoomData>($"{directoryPath}/Data.asset");

            EditorRoom editorRoom = EditorRoom.Create(room, directoryPath);
            SceneManager.MoveGameObjectToScene(editorRoom.gameObject, scene);
            SceneManager.MoveGameObjectToScene(editorRoom.scene.gameObject, scene);
        }
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
                    roomData.tileLayerArray[i] = new TileLayer(room.layerList[i]);
                }
            }
        }
        AssetDatabase.CreateAsset(roomData, $"{room.path}/Data.asset");
    }
}
