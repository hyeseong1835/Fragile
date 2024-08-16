using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpriteTilemapLayerSaveData : TilemapModuleLayerSaveData<SpriteTilemapModule, SpriteTilemapModuleData, SpriteTilemapLayerSaveData>
{

}
public class SpriteTilemapModuleData : TilemapModuleData<SpriteTilemapModule, SpriteTilemapModuleData, SpriteTilemapLayerSaveData>
{
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

    [Header("Ÿ�ϸ� ����")]
    public TileInfo[] tileArray;

    public SpriteTilemapModuleData(SpriteTilemapModule module)
    {
        List<TileInfo> tileInfoList = new List<TileInfo>();
        module.tilemap.FindNotNullTile(
            module.roomLayer.editorRoom.chunckList,
            (pos, tile) => tileInfoList.Add(new TileInfo(pos, tile))
        );
        tileArray = tileInfoList.ToArray();

        sortingOrder = module.tilemapRenderer.sortingOrder;
        material = module.tilemapRenderer.material;
        renderMode = module.tilemapRenderer.mode;
    }
    public override SpriteTilemapModule Load(RoomLayer roomLayer)
    {
        SpriteTilemapModule module = new GameObject(name).AddComponent<SpriteTilemapModule>();
        {
            module.transform.SetParent(roomLayer.transform);

            SpriteTilemapLayerSaveData layerSaveData = (SpriteTilemapLayerSaveData)roomLayer.layerSaveData[module.GetType()];

            //Ÿ�ϸ�
            foreach (TileInfo info in tileArray)
            {
                module.tilemap.SetTile(
                    new Vector3Int(info.pos.x, info.pos.y, 0),
                    info.tile
                );
            }

            //������
            module.tilemapRenderer.sortingOrder = sortingOrder;
            module.tilemapRenderer.sharedMaterial = material;
            module.tilemapRenderer.mode = renderMode;

            module.tilemapRenderer.sortingOrder = layerSaveData.sortingLayer;
        }
        return module;
    }
}
public class SpriteTilemapModule : TilemapModule<SpriteTilemapModule, SpriteTilemapModuleData, SpriteTilemapLayerSaveData>
{
    [Title("Ÿ�ϸ�")]

    [Title("������")]
    [ShowInInspector] int sortingOrder { 
        get => tilemapRenderer.sortingOrder;
        set => tilemapRenderer.sortingOrder = value;
    }
    [ShowInInspector] Material material
    {
        get => tilemapRenderer.sharedMaterial;
        set => tilemapRenderer.sharedMaterial = value;
    }
    [ShowInInspector] TilemapRenderer.Mode renderMode
    {
        get => tilemapRenderer.mode;
        set => tilemapRenderer.mode = value;
    }

    public override SpriteTilemapModuleData Save() => new SpriteTilemapModuleData(this);
    protected override void Load(SpriteTilemapModuleData moduleData, SpriteTilemapLayerSaveData layerSaveData)
    {
        //Ÿ�ϸ�
        foreach (SpriteTilemapModuleData.TileInfo info in moduleData.tileArray)
        {
            tilemap.SetTile(
                new Vector3Int(info.pos.x, info.pos.y, 0),
                info.tile
            );
        }
    }
}
