
using System;
using UnityEngine;

[Serializable]
public abstract class RoomModuleDataBase
{
    public string name;
    public abstract RoomModuleBase CreateRoomModule(RoomLayer roomLayer);
}
[Serializable]
public abstract class RoomModuleData<TModule, TModuleData> : RoomModuleDataBase
    where TModule : RoomModule<TModule, TModuleData>
    where TModuleData : RoomModuleData<TModule, TModuleData>
{
    public override RoomModuleBase CreateRoomModule(RoomLayer roomLayer) => Load(roomLayer);

    public virtual TModule Load(RoomLayer roomLayer)
    {
        GameObject obj = new GameObject();
        TModule result = obj.AddComponent<TModule>();
        {
            result.roomLayer = roomLayer;
        }
        return result;
    }
}
[Serializable]
public abstract class RoomModuleData<TModule, TModuleData, TLayerSaveData> : RoomModuleDataBase
    where TModule : RoomModule<TModule, TModuleData, TLayerSaveData>
    where TModuleData : RoomModuleData<TModule, TModuleData, TLayerSaveData>
    where TLayerSaveData : RoomModuleLayerSaveData<TModule, TModuleData, TLayerSaveData>
{
    public override RoomModuleBase CreateRoomModule(RoomLayer roomLayer) => Load(roomLayer, (TLayerSaveData)roomLayer.layerSaveData[typeof(TLayerSaveData)]);

    public abstract TModule Load(RoomLayer roomLayer, TLayerSaveData layerSaveData);
}