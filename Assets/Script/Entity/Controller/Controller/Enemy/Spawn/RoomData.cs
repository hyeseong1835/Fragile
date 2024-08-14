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
}
[Serializable]
public struct TileLayer
{
    public string name;
    public TileInfo[] tileArray;
}
public class RoomData : ScriptableObject
{
    public Vector2Int[] chunkArray;
    public TileLayer[] tileLayerArray;
}