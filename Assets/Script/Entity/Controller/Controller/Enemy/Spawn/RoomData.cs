using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct RoomLayerData
{
    public string name;
    [SerializeReference] public RoomModuleDataBase[] moduleData;
    [SerializeReference] public RoomModuleLayerSaveDataBase[] layerSaveDataArray;

#if UNITY_EDITOR
    public RoomLayerData(RoomLayer layer)
    {
        name = layer.gameObject.name;
        this.layerSaveDataArray = layer.layerSaveDataList.ToArray();
        this.moduleData = new RoomModuleDataBase[layer.roomModuleList.Count];
        {
            for (int i = 0; i < this.moduleData.Length; i++)
            {
                this.moduleData[i] = layer.roomModuleList[i].CreateModuleData();
            }
        }
    }
#endif
}
public class RoomData : ScriptableObject
{
    public Vector2Int[] chunkArray;
    public RoomLayerData[] roomLayerDataArray;
}