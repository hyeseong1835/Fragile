using Sirenix.OdinInspector;
using UnityEngine;

public class ControllerData : ScriptableObject
{
    public string FilePath => $"{Controller.entityFolderPath}/{name}";

    new public string name;

    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 center = new Vector2(0, 0.5f);

    [LabelWidth(Editor.propertyLabelWidth)]
    public float maxHp = 1;

    [LabelWidth(Editor.propertyLabelWidth)]
    public float moveSpeed = 1;

    [LabelWidth(Editor.propertyLabelWidth)]
    public int inventorySize = 0;
}