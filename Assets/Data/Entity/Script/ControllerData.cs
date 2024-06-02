using Sirenix.OdinInspector;
using UnityEngine;

public abstract class ControllerData : EntityData
{
    /// <summary>
    /// ��ũ��Ʈ ���� ���
    /// </summary>
    public const string scriptsFolderPath = EntityData.entityFolderPath + "/Script/Controller";
    /// <summary>
    /// ��Ʈ�ѷ� ���� ���ҽ����
    /// </summary>
    public const string controllersResourcePath = "Controller";
    /// <summary>
    /// ��Ʈ�ѷ� ���� ���
    /// </summary>
    public const string controllersFolderPath = entityResourceFolderPath + "/" + controllersResourcePath;

    public override string ResourceFolderPath => $"{entityResourceFolderPath}/{controllersResourcePath}";
    public override string FolderResourcePath { get => $"{controllersResourcePath}/{name}"; }
    public override string DataResourcePath { get => $"{FolderResourcePath}/ControllerData"; }

    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 center = new Vector2(0, 0.5f);

    [LabelWidth(Editor.propertyLabelWidth)]
    public float moveSpeed = 1;

    [LabelWidth(Editor.propertyLabelWidth)]
    public int inventorySize = 0;
}