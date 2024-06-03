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
    public const string controllersFolderResourcePath = "Controller";
    /// <summary>
    /// ��Ʈ�ѷ� ���� ���
    /// </summary>
    public const string controllersFolderPath = entityResourceFolderPath + "/" + controllersFolderResourcePath;

    public const string resourceFolderPath = entityResourceFolderPath + "/" +controllersFolderResourcePath;

    public string FolderPath { get => $"{controllersFolderPath}/{name}"; }
    public string FolderResourcePath { get => $"{controllersFolderResourcePath}/{name}"; }
    public string DataPath { get => $"{FolderPath}/{controllersFolderResourcePath}"; }
    public string DataResourcePath { get => $"{FolderResourcePath}/{controllersFolderResourcePath}"; }

    
    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 center = new Vector2(0, 0.5f);

    [LabelWidth(Editor.propertyLabelWidth)]
    public float moveSpeed = 1;

    [LabelWidth(Editor.propertyLabelWidth)]
    public int inventorySize = 0;
}