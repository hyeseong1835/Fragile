using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap), typeof(TilemapRenderer))]
public abstract class EditorTileMapModule : MonoBehaviour
{
    public Tilemap tilemap;
    public TilemapRenderer tilemapRenderer;

    public abstract string Save();
    public abstract void Init(string json);

    public TileInfo[] GetTileInfoArray()
    {
        List<TileInfo> result = new List<TileInfo>();
        {
            BoundsInt bounds = tilemap.cellBounds;
            for (int x = bounds.xMin; x <= bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y <= bounds.yMax; y++)
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
        return result.ToArray();
    }
}
