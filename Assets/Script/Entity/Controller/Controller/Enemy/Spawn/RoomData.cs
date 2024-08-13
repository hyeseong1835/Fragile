using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public struct ChunkData
{
    public Vector2Int pos;
    
}
public class RoomData : ScriptableObject
{
    public PhaseData[] phaseArray;
    public ChunkData[] chunkArray;
}