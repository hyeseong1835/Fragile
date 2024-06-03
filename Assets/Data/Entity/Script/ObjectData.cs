using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : EntityData
{
    /// <summary>
    /// ������Ʈ ���ҽ����
    /// </summary>
    public const string objectResourcePath = "Object";
    /// <summary>
    /// ������Ʈ ���ҽ� ���� ���
    /// </summary>
    public const string objectResourceFolderPath = EntityData.entityResourceFolderPath + "/" + objectResourcePath;

    public const string resourceFolderPath = entityResourceFolderPath + "/" + objectResourcePath;

    public string FolderResourcePath { get => $"{objectResourcePath}/{name}"; }
    public string DataResourcePath { get => $"{FolderResourcePath}/{objectResourcePath}"; }

}
