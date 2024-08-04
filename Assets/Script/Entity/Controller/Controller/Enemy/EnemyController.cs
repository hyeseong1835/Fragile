using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Reflection.Emit;
using UnityEngine.AI;
using System;
using UnityEditor;
using System.IO;

public enum EnemyType
{
    None, Friend, Hostile, Neutral
}
public abstract class EnemyController : Controller
{
    public override float MaxHp {
        get => data.maxHp;
        set => data.maxHp = value;
    }
    public override int InventorySize { 
        get => data.inventorySize; 
        set => data.inventorySize = value; 
    }
    public override Vector2 Center { 
        get => data.center; 
        set => data.center = value;
    }

    [HideInInspector]
    public EnemyType enemyType;

    public Controller target => PlayerController.instance;

    [BoxGroup("Object", Order = 0)]
    [HorizontalGroup("Object/Data", Order = 0)]
    #region Horizontal ControllerData

        [ShowInInspector]
        [LabelText("Data")][LabelWidth(Editor.labelWidth)]
        public EnemyControllerData data;

    #if UNITY_EDITOR
        [HorizontalGroup("Object/Data", Width = Editor.shortButtonWidth)]
        [Button("Create")]
        public void CreateData()
        {
            string currentPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(gameObject);
            int currentPathLastSlashIndex = currentPath.LastIndexOf('/');
            if (currentPathLastSlashIndex < 0)
            {
                Debug.LogError($"currentPath is invalid\n"
                    + $"gameObject: {gameObject.name}\n"
                    + $"currentPath: {currentPath}\n"
                    + $"currentPathLastSlashIndex: {currentPathLastSlashIndex}");
                return;
            }
            string path = $"{currentPath.Substring(0, currentPathLastSlashIndex)}/{DataType.Name}.asset";

            if (Directory.Exists(path))
            {
                try
                {
                    data = AssetDatabase.LoadAssetAtPath<EnemyControllerData>(path);
                }
                catch
                {
                    Debug.LogError($"Failed Load ({path})");
                    return;
                }
                Debug.Log($"Load ({path})");
                return;
            }
            try
            {
                data = (EnemyControllerData)ScriptableObject.CreateInstance(DataType);
            }
            catch
            {
                Debug.LogError($"Failed Create Instance ({DataType.Name})");
                return;
            }

            try
            {
                AssetDatabase.CreateAsset(data, path);
            }
            catch
            {
                Debug.LogError($"Failed Create ({path})");
                return;
            }
            Debug.Log($"Create ({path})");
        }
    #endif

    #endregion

    public abstract Type DataType { get; }
}