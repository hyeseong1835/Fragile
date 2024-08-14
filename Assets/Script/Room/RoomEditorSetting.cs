using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Setting", menuName = "RoomEditor/Setting")]
public class RoomEditorSetting : ScriptableObject
{
    public GameObject roomPrefab;
    public GameObject layerPrefab;

    void OnEnable()
    {
        RoomEditor.setting = this;
    }
}
