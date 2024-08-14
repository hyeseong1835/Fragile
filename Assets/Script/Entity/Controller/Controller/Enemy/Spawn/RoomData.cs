using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public struct TileInfo
{
    public Vector2Int pos;
    public TileBase tile;

#if UNITY_EDITOR
    public TileInfo(Vector3Int pos, TileBase tile)
    {
        this.pos = new Vector2Int(pos.x, pos.y);
        this.tile = tile;
    }
    public TileInfo(Vector2Int pos, TileBase tile)
    {
        this.pos = pos;
        this.tile = tile;
    }
#endif
}
public abstract class TileMapModule
{
    public abstract string Save();
    public abstract TileMapModule Load(TileMapModuleData data);
}
[Serializable]
public struct TileMapModuleData
{
    public string name;
    public TileInfo[] tileArray;
    public Material material;
    public TilemapRenderer.Mode renderMode;

    public Type moduleType;
    public string jsonData;

#if UNITY_EDITOR
    public TileMapModuleData(EditorTileMapModule module)
    {
        this.name = module.gameObject.name;
        this.tileArray = module.GetTileInfoArray();
        this.material = module.tilemapRenderer.material;
        this.renderMode = module.tilemapRenderer.mode;
        this.moduleType = module.GetType();
        this.jsonData = module.Save();
    }
#endif
}
[Serializable]
public struct TileLayer
{
    [LayerDropdown] public int layer;
    public TileMapModuleData[] tileMapModuleData;

#if UNITY_EDITOR
    public TileLayer(EditorRoomLayer layer)
    {
        this.layer = layer.layer;
        this.tileMapModuleData = new TileMapModuleData[layer.tileMapModules.Count];
        {
            for (int i = 0; i < this.tileMapModuleData.Length; i++)
            {
                this.tileMapModuleData[i] = new TileMapModuleData(layer.tileMapModules[i]);
            }
        }
    }
#endif
}
public class RoomData : ScriptableObject
{
    public Vector2Int[] chunkArray;
    public TileLayer[] tileLayerArray;
}