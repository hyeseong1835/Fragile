using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    /// <summary> 
    /// 엔티티 폴더 경로
    /// </summary>
    public const string entityFolderPath = "Assets/Data/Entity";
    /// <summary> 
    /// 리소스 폴더 경로
    /// </summary>
    public const string entityResourceFolderPath = EntityData.entityFolderPath + "/Resources";

    [LabelWidth(Editor.propertyLabelWidth)]
    new public string name;

    [LabelWidth(Editor.propertyLabelWidth)]
    public float maxHp = 1;
}
