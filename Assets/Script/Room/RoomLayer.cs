using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    public static RoomLayer Create(EditorRoom editorRoom, RoomLayerData layerData)
    {
        GameObject roomLayerObj = new GameObject(layerData.name);
        RoomLayer roomLayer = roomLayerObj.AddComponent<RoomLayer>();
        {
            roomLayer.layerSaveData = layerData.layerSaveData;
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
            module.Refresh();
            roomModuleList.Add(module);
        }
    }
}
