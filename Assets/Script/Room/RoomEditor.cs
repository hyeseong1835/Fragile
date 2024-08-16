using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            roomData.roomLayerDataArray = new RoomLayerData[room.layerList.Count];
            {
                for (int i = 0; i < roomData.roomLayerDataArray.Length; i++)
                {
                    roomData.roomLayerDataArray[i] = new RoomLayerData(room.layerList[i]);
                }
            }

        }
        AssetDatabase.CreateAsset(roomData, $"{room.path}/Data.asset");
    }
}
