using Sirenix.OdinInspector;
using UnityEngine;

public static class ControllerData
{
    public const string folderPath = EntityData.folderPath + "/Resources/Controller";
    public const string resourceFolderPath = "Controller";
}
public abstract class ControllerData<ConT, DataT> : EntityData<ConT, DataT>
    where ConT : Controller<ConT, DataT>
    where DataT : ControllerData<ConT, DataT>
{
    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 center = new Vector2(0, 0.5f);

    [LabelWidth(Editor.propertyLabelWidth)]
    public float moveSpeed = 1;

    [LabelWidth(Editor.propertyLabelWidth)]
    public int inventorySize = 0;
}