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
public class RoomData : ScriptableObject
{
    public PhaseData[] phaseArray;
}