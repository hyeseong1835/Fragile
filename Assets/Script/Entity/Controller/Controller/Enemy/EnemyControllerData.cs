using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyControllerData : ScriptableObject
{
    [LabelWidth(Editor.labelWidth)]
    public float maxHp = 1;

    [LabelWidth(Editor.labelWidth)]
    public Vector2 center = new Vector2(0, 0.5f);

    [LabelWidth(Editor.labelWidth)]
    public float moveSpeed = 1;

    [LabelWidth(Editor.labelWidth)]
    public int inventorySize = 0;
}
