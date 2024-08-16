using Sirenix.OdinInspector;
using System;
using UnityEngine;


public abstract class RoomModuleBase : MonoBehaviour
{
    public RoomLayer roomLayer;

    public abstract RoomModuleDataBase CreateModuleData();
    public abstract void Refresh();
}
public abstract class RoomModule<TModule, TModuleData> : RoomModuleBase
    where TModule : RoomModule<TModule, TModuleData>
    where TModuleData : RoomModuleData<TModule, TModuleData>
{
#if UNITY_EDITOR
    [Button("초기화")]
    public override RoomModuleDataBase CreateModuleData() => Save();
    protected abstract TModuleData Save();
#endif
}
public abstract class RoomModule<TModule, TModuleData, TLayerSaveData> : RoomModuleBase
    where TModule : RoomModule<TModule, TModuleData, TLayerSaveData>
    where TModuleData : RoomModuleData<TModule, TModuleData, TLayerSaveData>
    where TLayerSaveData : RoomModuleLayerSaveData<TModule, TModuleData, TLayerSaveData>
{
#if UNITY_EDITOR
    [Button("초기화")]
    public override RoomModuleDataBase CreateModuleData() => Save();
    public abstract TModuleData Save();
#endif
}
