using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

#if UNITY_EDITOR
    /// <summary>
    /// EDITOR Only!
    /// </summary>
    public RoomData Apply(PhaseData[] phaseDataArray)
    {

        return this;
    }
#endif
}