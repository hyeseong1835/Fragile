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

    public override string ResourceFolderPath => $"{entityResourceFolderPath}/{objectResourcePath}";
    public override string FolderResourcePath { get => $"{objectResourcePath}/{name}"; }
    public override string DataResourcePath { get => $"{FolderResourcePath}/ObjectData"; }

}
