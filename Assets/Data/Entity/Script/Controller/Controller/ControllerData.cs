using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public abstract class ControllerData : EntityData
{
#if UNITY_EDITOR
    public override void ResetName()
    {
        name = AssetDatabase.GetAssetPath(this).Split('/')[^2];
    }
#endif
    
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
    public const string controllersFolderPath = EntityData.entityResourceFolderPath + "/" + controllersFolderResourcePath;

    public string FolderPath { get => $"{controllersFolderPath}/{name}"; }
    public string FolderResourcePath { get => $"{controllersFolderResourcePath}/{name}"; }
    
    [LabelWidth(Editor.propertyLabelWidth)]
    public Vector2 center = new Vector2(0, 0.5f);

    [LabelWidth(Editor.propertyLabelWidth)]
    public float moveSpeed = 1;

    [LabelWidth(Editor.propertyLabelWidth)]
    public int inventorySize = 0;
}