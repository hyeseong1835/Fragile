using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    [LabelWidth(Editor.propertyLabelWidth)]
    new public string name;

    [LabelWidth(Editor.propertyLabelWidth)]
    public float maxHp = 1;
}
