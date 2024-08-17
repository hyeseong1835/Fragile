using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RoomLayer : MonoBehaviour
{
    public static string[] moduleNames;

    public Grid grid;

    /// <summary>
    /// 에디터 룸에서 직접 할당
    /// </summary>
    public EditorRoom editorRoom;
    public List<RoomModuleBase> roomModuleList;
    public Dictionary<Type, RoomModuleLayerSaveDataBase> layerSaveData = new Dictionary<Type, RoomModuleLayerSaveDataBase>();
#if UNITY_EDITOR
    [SerializeReference] public List<RoomModuleLayerSaveDataBase> layerSaveDataList = new List<RoomModuleLayerSaveDataBase>();
#endif
    public static RoomLayer Create(EditorRoom editorRoom, RoomLayerData layerData)
    {
        GameObject roomLayerObj = new GameObject(layerData.name);
        RoomLayer roomLayer = roomLayerObj.AddComponent<RoomLayer>();
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
            {
                foreach (RoomModuleLayerSaveDataBase saveData in layerData.layerSaveDataArray)
                {
                    roomLayer.layerSaveData.Add(saveData.GetType(), saveData);
                }
            }
            else
            {
                roomLayer.layerSaveDataList = layerData.layerSaveDataArray.ToList();
            }
#else
            foreach (RoomModuleLayerSaveDataBase saveData in layerData.layerSaveDataArray)
            {
                roomLayer.layerSaveData.Add(saveData.GetType(), saveData);
            }
#endif
            roomLayer.grid = roomLayerObj.AddComponent<Grid>();
            roomLayer.editorRoom = editorRoom;
            roomLayer.roomModuleList = new List<RoomModuleBase>();
            {
                foreach (RoomModuleDataBase moduleData in layerData.moduleData)
                {
                    RoomModuleBase module = moduleData.CreateRoomModule(roomLayer);
                    module.transform.SetParent(roomLayer.transform);
                    roomLayer.roomModuleList.Add(module);
                }
            }
        }
        return roomLayer;
    }
    public void Refresh()
    {
        grid = GetComponent<Grid>();
        RefreshModule();
    }
    [Button("모듈 다시 로드하기")]
    void RefreshModule()
    {
        roomModuleList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            RoomModuleBase module = transform.GetChild(i).GetComponent<RoomModuleBase>();
            module.roomLayer = this;
            module.Refresh();
            roomModuleList.Add(module);
        }
    }
}
