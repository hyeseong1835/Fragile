using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEditor.SceneManagement;
using Sirenix.OdinInspector;


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
[System.Serializable]
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
[ExecuteInEditMode]
public class EditorRoom : MonoBehaviour
{
    [Title("파일")]
    public string path;

    [Title("오브젝트")]
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;
    public Grid grid;

    [Title("데이터")]
    public List<EditorPhaseData> phaseDataList = new List<EditorPhaseData>();
    public List<Vector2Int> chunckList = new List<Vector2Int>();
    public List<Vector2Int> canAddChunckList;


    //
    float pixelPerUnit = 1;
    void OnEnable()
    {
        EditorSceneManager.sceneClosing += OnSceneClosing;
        SceneView.duringSceneGui += OnSceneGUI;
    }
    void OnDisable()
    {
        EditorSceneManager.sceneClosing -= OnSceneClosing;
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    void OnSceneClosing(Scene scene, bool removingScene)
    {
        RoomEditor.SaveRoom(this);
    }

    void Start()
    {

    }
    void Update()
    {
        
    }
    void OnValidate()
    {
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;

        tilemap.transform.position = Vector2.zero;
        tilemap.transform.rotation = Quaternion.identity;

        tilemapRenderer.transform.position = Vector2.zero;
        tilemapRenderer.transform.rotation = Quaternion.identity;

        grid.transform.position = Vector2.zero;
        grid.transform.rotation = Quaternion.identity;

        canAddChunckList.Clear();
        foreach (Vector2Int pos in chunckList)
        {
            AddChunck(Vector2Int.up);
            AddChunck(Vector2Int.down);
            AddChunck(Vector2Int.right);
            AddChunck(Vector2Int.left);

            void AddChunck(Vector2Int offset)
            {
                if (chunckList.Contains(pos + offset)) return;
                if (canAddChunckList.Contains(pos + offset)) return;

                canAddChunckList.Add(pos + offset);
            }
        }
        
    }
    void OnSceneGUI(SceneView view)
    {
        pixelPerUnit = view.camera.pixelHeight / (2 * view.camera.orthographicSize);
    }
    void OnDrawGizmos()
    {
        if (chunckList != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.1f);
            foreach (Vector2Int pos in canAddChunckList)
            {
                Gizmos.DrawCube(
                    ((Vector2)pos + 0.5f * Vector2.one) * (Vector2)grid.cellSize * 16,
                    grid.cellSize * 16
                );
            }
            Gizmos.color = Color.green;
            foreach (Vector2Int pos in canAddChunckList)
            {
                Gizmos.DrawWireCube(
                    ((Vector2)pos + 0.5f * Vector2.one) * (Vector2)grid.cellSize * 16,
                    grid.cellSize * 16 - new Vector3(2 / pixelPerUnit, 2 / pixelPerUnit, 0)
                );
            }
            Gizmos.color = Color.white;
            foreach (Vector2Int pos in chunckList)
            {
                Gizmos.DrawWireCube(
                    ((Vector2)pos + 0.5f * Vector2.one) * grid.cellSize * 16,
                    grid.cellSize * 16 - new Vector3(2 / pixelPerUnit, 2 / pixelPerUnit, 0)
                );
            }
        }
    }
}
