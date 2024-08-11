using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class EditorEnemySpawnData : EditorSpawn
{
    public string enemyName;
}
public abstract class EditorSpawn
{
    public Vector2 pos;
    public abstract void OnSceneGUI(SceneView view);
    public abstract IStageSpawn Apply();
}
public class EditorPhaseData
{
    public List<EditorSpawn> spawnDataList = new List<EditorSpawn>();
    public PhaseData Apply()
    {
        return new PhaseData(
            spawnDataList.Select(s => s.Apply()).ToArray()
        );
    }
}
public class EditorRoomData
{
    public List<EditorPhaseData> phaseDataList = new List<EditorPhaseData>();
    public RoomData Apply()
    {
        return RoomEditor.data.openRoomData.Apply(
            phaseDataList.Select(p => p.Apply()).ToArray()
        );
    }
}
[CreateAssetMenu(menuName = "Data/RoomEditor/Data", fileName = "Data")]
public class RoomEditorData : ScriptableObject
{
    public RoomData openRoomData;
    public EditorRoomData room;
    public Scene scene;
    public int curPhaseIndex = -1;
}