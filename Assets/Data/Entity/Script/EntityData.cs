using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    /// <summary> 
    /// 엔티티 폴더 경로
    /// </summary>
    public const string entityFolderPath = "Assets/Data/Entity";
    /// <summary> 
    /// 리소스 폴더 경로
    /// </summary>
    public const string entityResourceFolderPath = EntityData.entityFolderPath + "/Resources";

    /// <summary> 
    /// 리소스 폴더 경로
    /// </summary>
    public abstract string ResourceFolderPath { get; }

    /// <summary> 
    /// 폴더 리소스경로
    /// </summary>
    public abstract string FolderResourcePath { get; }
    /// <summary> 
    /// 데이터 리소스경로
    /// </summary>
    public abstract string DataResourcePath { get; }

    /// <summary> 
    /// 폴더 경로
    /// </summary>
    public string FolderPath { get => $"{ResourceFolderPath}/{FolderResourcePath}"; }
    /// <summary> 
    /// 데이터 경로
    /// </summary>
    public string DataPath { get => $"{ResourceFolderPath}/{DataResourcePath}.asset"; }

    [LabelWidth(Editor.propertyLabelWidth)]
    new public string name;

    [LabelWidth(Editor.propertyLabelWidth)]
    public float maxHp = 1;
}
