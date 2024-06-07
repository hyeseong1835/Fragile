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
    /// 스크립트 폴더 경로
    /// </summary>
    public const string scriptsFolderPath = EntityData.entityFolderPath + "/Script/Controller";
    /// <summary>
    /// 컨트롤러 폴더 리소스경로
    /// </summary>
    public const string controllersFolderResourcePath = "Controller";
    /// <summary>
    /// 컨트롤러 폴더 경로
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