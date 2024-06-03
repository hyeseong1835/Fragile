using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    /// <summary> 
    /// ��ƼƼ ���� ���
    /// </summary>
    public const string entityFolderPath = "Assets/Data/Entity";
    /// <summary> 
    /// ���ҽ� ���� ���
    /// </summary>
    public const string entityResourceFolderPath = EntityData.entityFolderPath + "/Resources";

    [LabelWidth(Editor.propertyLabelWidth)]
    new public string name;

    [LabelWidth(Editor.propertyLabelWidth)]
    public float maxHp = 1;
}
