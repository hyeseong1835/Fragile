using Sirenix.OdinInspector.Editor.Modules;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public abstract class TilemapModuleLayerSaveData<TModule, TModuleData, TLayerSaveData> : RoomModuleLayerSaveData<TModule, TModuleData, TLayerSaveData>
    where TModule : RoomModule<TModule, TModuleData, TLayerSaveData>
    where TModuleData : RoomModuleData<TModule, TModuleData, TLayerSaveData>
    where TLayerSaveData : TilemapModuleLayerSaveData<TModule, TModuleData, TLayerSaveData>, new()
{
    [LayerDropdown] public int sortingLayer;
}
[Serializable]
public abstract class TilemapModuleData<TModule, TModuleData, TLayerSaveData> : RoomModuleData<TModule, TModuleData, TLayerSaveData>
    where TModule : RoomModule<TModule, TModuleData, TLayerSaveData>
    where TModuleData : RoomModuleData<TModule, TModuleData, TLayerSaveData>
    where TLayerSaveData : TilemapModuleLayerSaveData<TModule, TModuleData, TLayerSaveData>, new()
{
    [Header("·»´õ·¯ ¼³Á¤")]
    public int sortingOrder;
    public Material material;
    public TilemapRenderer.Mode renderMode;
}
[Serializable]
public abstract class TilemapModule<TModule, TModuleData, TLayerSaveData> : RoomModule<TModule, TModuleData, TLayerSaveData>
    where TModule : TilemapModule<TModule, TModuleData, TLayerSaveData>
    where TModuleData : TilemapModuleData<TModule, TModuleData, TLayerSaveData>
    where TLayerSaveData : TilemapModuleLayerSaveData<TModule, TModuleData, TLayerSaveData>, new()
{
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;

    protected abstract void Load(TModuleData moduleData, TLayerSaveData layerSaveData);
#if UNITY_EDITOR
    public override void Refresh()
    {
        TLayerSaveData layerSaveData = (TLayerSaveData)roomLayer.layerSaveDataList.Find(x => x.GetType() == typeof(TLayerSaveData));
        if (layerSaveData == null) roomLayer.layerSaveDataList.Add(layerSaveData = new TLayerSaveData());

        if (tilemap == null || tilemap.gameObject != gameObject)
        {
            tilemap = GetComponent<Tilemap>();
        }
        if (tilemapRenderer == null || tilemapRenderer.gameObject != gameObject)
        {
            tilemapRenderer = GetComponent<TilemapRenderer>();
        }

        tilemapRenderer.sortingOrder = layerSaveData.sortingLayer;
    }
#endif
}
