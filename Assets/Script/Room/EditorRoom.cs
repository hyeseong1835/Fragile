using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEditor.SceneManagement;
using Sirenix.OdinInspector;
using System;
using Unity.VisualScripting;
using UnityEditor.EditorTools;

[ExecuteInEditMode]
public class EditorRoom : MonoBehaviour
{
    Event e => Event.current;

    [Title("파일")]
    public string path = "";

    [Title("오브젝트")]
    public List<RoomLayer> layerList = new List<RoomLayer>();
    public Transform scene;

    [Title("데이터")]
    public List<Vector2Int> chunckList = new List<Vector2Int>();
    public List<Vector2Int> canAddChunckList = new List<Vector2Int>();
    [SerializeField] Vector2Int onMouseChunck;

    //
    float pixelPerUnit = 1;
    void OnEnable()
    {
        if (path == "") return;
        
        EditorSceneManager.sceneClosing += OnSceneClosing;
        SceneView.duringSceneGui += OnSceneGUI;
    }
    void OnDisable()
    {
        if (path == "") return;

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
    [Button("레이어 추가하기")]
    void AddLayer(string name)
    {
        GameObject layerObj = new GameObject(name);
        {
            layerObj.transform.parent = scene;
        }
        
        layerList.Add(
            Instantiate(
                RoomEditor.setting.layerPrefab
            ).GetComponent<RoomLayer>()
        );
    }
    [Button("초기화")]
    void Refresh()
    {
        layerList.Clear();
        for (int i = 0; i < scene.childCount; i++)
        {
            RoomLayer layer = scene.GetChild(i).GetComponent<RoomLayer>();
            layer.editorRoom = this;
            layer.Refresh();
            layerList.Add(layer);
        }
    }
    public static EditorRoom Create(RoomData data, string savePath = "")
    {
        EditorRoom result = new GameObject("Room Setting").AddComponent<EditorRoom>();
        {
            result.path = savePath;
            result.chunckList.AddRange(data.chunkArray);
            result.RefreshCanAddChunckList();
        }
        result.scene = new GameObject("Scene").transform;
        {
            foreach (RoomLayerData layerData in data.roomLayerDataArray)
            {
                RoomLayer layer = RoomLayer.Create(result, layerData);
                {
                    layer.transform.SetParent(result.scene);
                }
                result.layerList.Add(layer);
            }
        }
        return result;
    }
    void Start()
    {

    }
    void Update()
    {
        
    }
    void OnValidate()
    {
        if (path == "") return;

        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;

        RefreshCanAddChunckList();
    }
    void OnSceneGUI(SceneView view)
    {
        if (Selection.activeObject == gameObject)
        {
            switch (e.type)
            {
                case EventType.Repaint:
                    pixelPerUnit = (2 * view.camera.orthographicSize) / view.camera.pixelHeight;
                    onMouseChunck = ScreenToChunckPosInt(e.mousePosition);

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
                    break;
                case EventType.MouseMove:
                    Vector2Int onMouse = ScreenToChunckPosInt(e.mousePosition);
                    if (onMouseChunck != onMouse)
                    {
                        onMouseChunck = onMouse;
                        SceneView.RepaintAll();
                    }
                    break;
                case EventType.MouseDown:
                    switch (e.button)
                    {
                        case 0:
                            if (canAddChunckList.Contains(onMouseChunck))
                            {
                                chunckList.Add(onMouseChunck);
                                canAddChunckList.Remove(onMouseChunck);

                                AddChunck(Vector2Int.up);
                                AddChunck(Vector2Int.down);
                                AddChunck(Vector2Int.right);
                                AddChunck(Vector2Int.left);

                                e.Use();

                                void AddChunck(Vector2Int offset)
                                {
                                    if (chunckList.Contains(onMouseChunck + offset)) return;
                                    if (canAddChunckList.Contains(onMouseChunck + offset)) return;

                                    canAddChunckList.Add(onMouseChunck + offset);
                                }
                            }
                            break;
                        case 1:
                            if (chunckList.Contains(onMouseChunck))
                            {
                                chunckList.Remove(onMouseChunck);
                                RefreshCanAddChunckList();

                                e.Use();
                            }
                            break;
                    }   
                    break;
            }
        }
    }
    public void RefreshCanAddChunckList()
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
        foreach (Vector2Int chunck in chunckList)
        {
            Check(Vector2Int.up);
            Check(Vector2Int.down);
            Check(Vector2Int.right);
            Check(Vector2Int.left);

            void Check(Vector2Int offset)
            {
                if (chunckList.Contains(chunck + offset) == false)
                {
                    Vector2 chunckCenterPos = ChunckToWorldPos(chunck) + Vector2.one * 8;
                    Vector2 l = new Vector2(offset.y, offset.x) * 8;
                    Gizmos.DrawLine(chunckCenterPos + offset * 8 + l, chunckCenterPos - l + offset * 8);
                }
            }
        }
    }
    Vector2 ScreenToWorldPos(Vector2 screenPos)
    {
        return pixelPerUnit * new Vector2(
            screenPos.x - 0.5f * Screen.width,
            0.5f * Screen.height - screenPos.y - 25
        ) + (Vector2)SceneView.currentDrawingSceneView.camera.transform.position;
    }
    public Vector2 ChunckToWorldPos(Vector2Int chunckPos)
    {
        return (Vector2)chunckPos * Vector2.one * 16;
    }
    Rect GetChunckWorldRect(Vector2Int chunckPos)
    {
        return new Rect(
            ChunckToWorldPos(chunckPos),
            Vector2.one * 16
        );
    }
    Vector2Int ScreenToChunckPosInt(Vector2 screenPos)
    {
        Vector2 worldPos = ScreenToWorldPos(screenPos);
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / 16),
            Mathf.FloorToInt(worldPos.y / 16)
        );
    }
}
