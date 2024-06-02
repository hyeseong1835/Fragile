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

    public override string ResourceFolderPath => $"{entityResourceFolderPath}/{objectResourcePath}";
    public override string FolderResourcePath { get => $"{objectResourcePath}/{name}"; }
    public override string DataResourcePath { get => $"{FolderResourcePath}/ObjectData"; }

}
