using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntityData
{
    public const string folderPath = "Assets/Data/Entity";
}
public abstract class EntityData<EntityT, DataT> : ScriptableObject
    where EntityT : Entity<EntityT, DataT>
    where DataT : EntityData<EntityT, DataT>
{
    [LabelWidth(Editor.propertyLabelWidth)]
    new public string name;

    [LabelWidth(Editor.propertyLabelWidth)]
    public float maxHp = 1;
}
