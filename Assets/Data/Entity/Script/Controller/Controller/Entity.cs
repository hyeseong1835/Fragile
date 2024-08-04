using UnityEngine;
using System;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;
using System.Reflection;
using UnityEditor;
using UnityEngine.Windows;
using static UnityEditor.LightingExplorerTableColumn;
using System.Diagnostics.CodeAnalysis;

public abstract class Entity : MonoBehaviour 
{
    [ShowInInspector]
    [HorizontalGroup("Object/Data", Order = 0)]
    #region Horizontal ControllerData

        [LabelText("Data")][LabelWidth(Editor.propertyLabelWidth)]
        public abstract EntityData EntityData { get; set; }

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
                    EntityData = AssetDatabase.LoadAssetAtPath<EntityData>(path);
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
                EntityData = (EntityData)ScriptableObject.CreateInstance(DataType);
            }
            catch
            {
                Debug.LogError($"Failed Create Instance ({DataType.Name})");
                return;
            }

            try
            {
                AssetDatabase.CreateAsset(EntityData, path);
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

    [FoldoutGroup("Stat")]
    #region Foldout Stat - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|                                         

        [HorizontalGroup("Stat/HP")]
        #region Horizontal HP

            [LabelWidth(Editor.propertyLabelWidth)]
            #if UNITY_EDITOR
            [ProgressBar(0, nameof(_maxHp), ColorGetter = nameof(_hpColor))]
            #endif
            public float hp = 1;

            #if UNITY_EDITOR
                                                                                 [HorizontalGroup("Stat/HP", Width = Editor.shortNoLabelPropertyWidth)]
            [ShowInInspector][HideLabel]
            [DelayedProperty]
            float _maxHp{
                get { 
                    if(EntityData == null) return default;
                    return EntityData.maxHp; 
                }
                set {
                    if (hp == EntityData.maxHp || hp > value) hp = value;
            EntityData.maxHp = value;
                }
            }

//          HideInInspector_____________________________________________________|
            Color _hpColor {
                get {
                    if(EntityData == null) return default;
                    
                    Gradient gradient = new Gradient();
                    gradient.SetKeys(
                        new GradientColorKey[] {
                            new GradientColorKey(Color.yellow, 0),
                            new GradientColorKey(Color.red, 1)
                        },
                        new GradientAlphaKey[] { new GradientAlphaKey(1, 0) }//-|
                    );
                    return gradient.Evaluate(hp / EntityData.maxHp);
                }
            }

            #endif

        #endregion  - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -|    

    #endregion - - - - - - - - - - - - - - - - - - - - -|
    
    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0) Die();
    }
    public void Die()
    {
        Destroy(gameObject);
    }
}