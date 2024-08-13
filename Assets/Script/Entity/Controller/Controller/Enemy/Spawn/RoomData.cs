using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public interface IStageSpawn
{
    public void Spawn();
}
public struct PhaseData
{
    public IStageSpawn[] spawnArray;

    public PhaseData(IStageSpawn[] spawnArray)
    {
        this.spawnArray = spawnArray;
    }
}
[Serializable]
public struct ChunkData
{
    public Vector2Int pos;
    public TileBase[] tiles;
}
public class RoomData : ScriptableObject
{
    public PhaseData[] phaseArray;
    public ChunkData[] chunkArray;
}