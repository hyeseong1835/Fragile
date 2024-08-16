using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using Unity.VisualScripting;

public static class TilemapUtility
{
    public static void FindNotNullTile(this Tilemap tilemap, IEnumerable<Vector2Int> chunckList, Action<Vector2Int, TileBase> action)
    {
        foreach (Vector2Int chunckPos in chunckList)
        {
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    TileBase tile = tilemap.GetTile(new Vector3Int(chunckPos.x * 16 + x, chunckPos.y * 16 + y, 0));
                    if (tile != null)
                    {
                        action.Invoke(new Vector2Int(chunckPos.x * 16 + x, chunckPos.y * 16 + y), tile);
                    }
                }
            }
        }
    }
}