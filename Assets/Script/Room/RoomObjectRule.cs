using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room/ObjectRule", fileName = "New ObjectRule")]
public class RoomObjectRule : ScriptableObject
{
    [Serializable]
    public class Rule
    {
        public GameObject prefab;
        public int ratio;
    }
    public Rule[] rules;
    public int ratioMax = 0;

    void OnValidate()
    {
        ratioMax = 0;
        foreach (Rule drop in rules)
        {
            ratioMax += drop.ratio;
        }
    }
}
