using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    /// <summary> 
    /// ��ƼƼ ���� ���
    /// </summary>
    public const string entityFolderPath = "Assets/Data/Entity";
    /// <summary> 
    /// ���ҽ� ���� ���
    /// </summary>
    public const string entityResourceFolderPath = EntityData.entityFolderPath + "/Resources";

    /// <summary> 
    /// ���ҽ� ���� ���
    /// </summary>
    public abstract string ResourceFolderPath { get; }

    /// <summary> 
    /// ���� ���ҽ����
    /// </summary>
    public abstract string FolderResourcePath { get; }
    /// <summary> 
    /// ������ ���ҽ����
    /// </summary>
    public abstract string DataResourcePath { get; }

    /// <summary> 
    /// ���� ���
    /// </summary>
    public string FolderPath { get => $"{ResourceFolderPath}/{FolderResourcePath}"; }
    /// <summary> 
    /// ������ ���
    /// </summary>
    public string DataPath { get => $"{ResourceFolderPath}/{DataResourcePath}.asset"; }

    [LabelWidth(Editor.propertyLabelWidth)]
    new public string name;

    [LabelWidth(Editor.propertyLabelWidth)]
    public float maxHp = 1;
}
