using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap), typeof(TilemapRenderer))]
public abstract class TileMapModule : MonoBehaviour
{
    public RoomLayer roomLayer;
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;

#if UNITY_EDITOR
    public abstract string Save();
#endif
    public abstract TileMapModule Load(TileMapModuleData data);

    public static TileMapModule Create(RoomLayer roomLayer)
    {
        TileMapModule result = new GameObject().AddComponent<SpriteTilemapModule>();
        {
            result.roomLayer = roomLayer;
            result.tilemap = result.AddComponent<Tilemap>();
            result.tilemapRenderer = result.AddComponent<TilemapRenderer>();
        }
        return result;
    }
    public TileInfo[] GetTileInfoArray()
    {
        List<TileInfo> result = new List<TileInfo>();
        {
            foreach (Vector2Int chunckPos in roomLayer.editorRoom.chunckList)
            {
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++)
                    {
                        Vector3Int pos = new Vector3Int(x, y, 0);
                        TileBase tile = tilemap.GetTile(pos);
                        if (tile != null)
                        {
                            result.Add(new TileInfo(pos, tile));
                        }
                    }
                }
            }
        }
        return result.ToArray();
    }
}
