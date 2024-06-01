using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ControllerData : EntityData
{
    public const string folderPath = Entity.folderPath + "/Resources/Controller";
    public const string resourceFolderPath = "Controller";

    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 center = new Vector2(0, 0.5f);

    [LabelWidth(Editor.propertyLabelWidth)]
    public float moveSpeed = 1;

    [LabelWidth(Editor.propertyLabelWidth)]
    public int inventorySize = 0;
}