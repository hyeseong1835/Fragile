
using System;

[Serializable]
public abstract class RoomModuleLayerSaveDataBase
{

}
public abstract class RoomModuleLayerSaveData<TModule, TModuleData> : RoomModuleLayerSaveDataBase
    where TModule : RoomModule<TModule, TModuleData>
    where TModuleData : RoomModuleData<TModule, TModuleData>
{

}
public abstract class RoomModuleLayerSaveData<TModule, TModuleData, TLayerSaveData> : RoomModuleLayerSaveDataBase
    where TModule : RoomModule<TModule, TModuleData, TLayerSaveData>
    where TModuleData : RoomModuleData<TModule, TModuleData, TLayerSaveData>
    where TLayerSaveData : RoomModuleLayerSaveData<TModule, TModuleData, TLayerSaveData>
{

}