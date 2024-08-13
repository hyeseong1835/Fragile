using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEditor.SceneManagement;
using Sirenix.OdinInspector;
using static UnityEditor.PlayerSettings;


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
    Event e => Event.current;

    [Title("파일")]
    public string path;

    [Title("오브젝트")]
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;
    public Grid grid;

    [Title("데이터")]
    public List<EditorPhaseData> phaseDataList = new List<EditorPhaseData>();
    public List<Vector2Int> chunckList = new List<Vector2Int>();
    public List<Vector2Int> canAddChunckList = new List<Vector2Int>();
    [SerializeField] Vector2Int onMouseChunck;

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
    [Button("저장하기")]
    void Save()
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

        if (tilemap != null)
        {
            tilemap.transform.position = Vector2.zero;
            tilemap.transform.rotation = Quaternion.identity;
        }

        if (tilemapRenderer != null)
        {
            tilemapRenderer.transform.position = Vector2.zero;
            tilemapRenderer.transform.rotation = Quaternion.identity;
        }

        if (grid != null)
        {
            grid.transform.position = Vector2.zero;
            grid.transform.rotation = Quaternion.identity;
        }

        RefreshCanAddChunckList();
    }
    void OnSceneGUI(SceneView view)
    {
        if (e.type == EventType.Repaint)
        {
            pixelPerUnit = (2 * view.camera.orthographicSize) / view.camera.pixelHeight;
            onMouseChunck = ScreenToChunckPosInt(e.mousePosition);
        }
        if (e.type == EventType.MouseMove)
        {
            Vector2Int onMouse = ScreenToChunckPosInt(e.mousePosition);
            if (onMouseChunck != onMouse)
            {
                onMouseChunck = onMouse;
                SceneView.RepaintAll();
            }
        }
        if (e.type == EventType.MouseDown)
        {
            if (e.button == 0)
            {
                if (canAddChunckList.Contains(onMouseChunck))
                {
                    chunckList.Add(onMouseChunck);
                    canAddChunckList.Remove(onMouseChunck);

                    AddChunck(Vector2Int.up);
                    AddChunck(Vector2Int.down);
                    AddChunck(Vector2Int.right);
                    AddChunck(Vector2Int.left);

                    void AddChunck(Vector2Int offset)
                    {
                        if (chunckList.Contains(onMouseChunck + offset)) return;
                        if (canAddChunckList.Contains(onMouseChunck + offset)) return;

                        canAddChunckList.Add(onMouseChunck + offset);
                    }
                }
            }
            else if (e.button == 1)
            {
                if (chunckList.Contains(onMouseChunck))
                {
                    chunckList.Remove(onMouseChunck);
                    RefreshCanAddChunckList();
                }
            }
        }
        if (e.type == EventType.Repaint)
        {
            for (int i = 0; i < canAddChunckList.Count; i++)
            {
                Vector2Int pos = canAddChunckList[i];
                Rect rect = GetChunckWorldRect(pos);

                if (pos == onMouseChunck)
                {
                    Handles.DrawSolidRectangleWithOutline(
                        rect,
                        Color.green,
                        Color.white
                    );

                }
                else
                {
                    Handles.DrawSolidRectangleWithOutline(
                        rect,
                        new Color(0, 1, 0, 0.1f),
                        Color.white
                    );
                }
            }
            foreach (Vector2Int pos in chunckList)
            {
                Handles.DrawSolidRectangleWithOutline(
                    GetChunckWorldRect(pos),
                    new Color(1, 1, 1, 0.1f),
                    Color.clear
                );
            }
            Handles.color = Color.red;
            Handles.DrawWireDisc(
                ScreenToWorldPos(e.mousePosition),
                Vector3.forward,
                0.25f
            );
        }
    }
    void RefreshCanAddChunckList()
    {
        canAddChunckList.Clear();
        if (chunckList.Count == 0)
        {
            canAddChunckList.Add(Vector2Int.zero);
        }
        else
        {
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
    }
    void OnDrawGizmos()
    {

    }
    Vector2 ScreenToWorldPos(Vector2 screenPos)
    {
        return pixelPerUnit * new Vector2(
            screenPos.x - 0.5f * Screen.width,
            0.5f * Screen.height - screenPos.y - 25
        ) + (Vector2)SceneView.currentDrawingSceneView.camera.transform.position;
    }
    Rect GetChunckWorldRect(Vector2Int chunckPos)
    {
        return new Rect(
            (Vector2)chunckPos * (Vector2)grid.cellSize * 16,
            grid.cellSize * 16
        );
    }
    Vector2Int ScreenToChunckPosInt(Vector2 screenPos)
    {
        Vector2 worldPos = ScreenToWorldPos(screenPos);
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / (grid.cellSize.x * 16)),
            Mathf.FloorToInt(worldPos.y / (grid.cellSize.y * 16))
        );
    }
}
