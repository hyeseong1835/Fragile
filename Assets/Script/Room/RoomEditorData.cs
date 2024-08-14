using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "RoomEditor/Data")]
public class RoomEditorData : ScriptableObject
{
    void OnEnable()
    {
        RoomEditor.data = this;
    }
}