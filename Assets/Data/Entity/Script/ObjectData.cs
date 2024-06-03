using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : EntityData
{
    /// <summary>
    /// 오브젝트 리소스경로
    /// </summary>
    public const string objectResourcePath = "Object";
    /// <summary>
    /// 오브젝트 리소스 폴더 경로
    /// </summary>
    public const string objectResourceFolderPath = EntityData.entityResourceFolderPath + "/" + objectResourcePath;

    public const string resourceFolderPath = entityResourceFolderPath + "/" + objectResourcePath;

    public string FolderResourcePath { get => $"{objectResourcePath}/{name}"; }
    public string DataResourcePath { get => $"{FolderResourcePath}/{objectResourcePath}"; }

}
